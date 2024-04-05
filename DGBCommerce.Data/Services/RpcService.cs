using System.Net;
using System.Text;
using DGBCommerce.Domain.Exceptions;
using DGBCommerce.Domain.Interfaces.Services;
using DGBCommerce.Domain.Models.Request;
using DGBCommerce.Domain.Models.Response;
using Newtonsoft.Json;

namespace DGBCommerce.Data.Services
{
    public class RpcService(string deamonUrl, string rpcUsername, string rpcPassword) : IRpcService
    {
        private readonly string _daemonUrl = deamonUrl;
        private readonly string _rpcUsername = rpcUsername;
        private readonly string _rpcPassword = rpcPassword;

        public async Task<uint> GetBlockCount()
            => await Request<uint>("getblockcount");

        public async Task<GetDifficultyResponse> GetDifficulty()
            => await Request<GetDifficultyResponse>("getdifficulty");

        public async Task<GetMiningInfoResponse> GetMiningInfo()
            => await Request<GetMiningInfoResponse>("getmininginfo");

        public async Task<string> GetNewAddress()
            => await Request<string>("getnewaddress");

        public async Task<string> GetNewAddress(string? label)
            => await Request<string>("getnewaddress", label);

        public async Task<string> GetNewAddress(string? label, string? addressType)
            => await Request<string>("getnewaddress", label, addressType);

        public async Task<List<ListReceivedByAddressResponse>> ListReceivedByAddress()
            => await Request<List<ListReceivedByAddressResponse>>("listreceivedbyaddress");

        public async Task<string> SendToAddress(string address, decimal amount)
            => await Request<string>("sendtoaddress", address, amount);

        public async Task<ValidateAddressResponse> ValidateAddress(string address)
            => await Request<ValidateAddressResponse>("validateaddress", address);

        public async Task<string> WalletLock()
            => await Request<string>("walletlock");

        public async Task<string> WalletPassphrase(string passphrase, int timeoutInSeconds)
            => await Request<string>("walletpassphrase", passphrase, timeoutInSeconds);

        private async Task<T> Request<T>(string rpcMethod, params object?[] parameters)
        {
            JsonRpcRequest jsonRpcRequest = new(1, rpcMethod.ToString(), parameters);
            HttpClientHandler httpHandler = new() { Credentials = new NetworkCredential(_rpcUsername, _rpcPassword), Proxy = null };
            HttpClient httpClient = new(httpHandler) { Timeout = new TimeSpan(30000000) };
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, _daemonUrl) { Content = new StringContent(jsonRpcRequest.GetRaw(), Encoding.UTF8, "application/json-rpc") };
            httpRequestMessage.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(_rpcUsername + ":" + _rpcPassword)));

            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            }
            catch (WebException webException)
            {
                #region RPC Internal Server Error (with an Error Code)
                if (webException.Response is HttpWebResponse webResponse)
                {
                    switch (webResponse.StatusCode)
                    {
                        case HttpStatusCode.InternalServerError:
                            {
                                using Stream stream = webResponse.GetResponseStream() ?? throw new RpcException("The RPC request was either not understood by the server or there was a problem executing the request", webException);
                                using StreamReader streamReaderException = new(stream);
                                var result = streamReaderException.ReadToEnd();
                                streamReaderException.Dispose();

                                try
                                {
                                    var jsonRpcResponseObject = JsonConvert.DeserializeObject<JsonRpcResponse<object>>(result);
                                    var exceptionMessage = string.Empty;

                                    if (jsonRpcResponseObject != null && jsonRpcResponseObject.Error != null)
                                        exceptionMessage = jsonRpcResponseObject!.Error!.Message;

                                    RpcInternalServerErrorException internalServerErrorException = new(exceptionMessage!, webException) { RpcErrorCode = jsonRpcResponseObject?.Error?.Code };

                                    throw internalServerErrorException;
                                }
                                catch (JsonException)
                                {
                                    throw new RpcException(result, webException);
                                }
                            }

                        default:
                            throw new RpcException("The RPC request was either not understood by the server or there was a problem executing the request", webException);
                    }
                }
                #endregion
                #region RPC Time-Out
                if (webException.Message == "The operation has timed out")
                {
                    throw new RpcRequestTimeoutException(webException.Message);
                }
                #endregion

                throw new RpcException("An unknown web exception occured while trying to read the JSON response", webException);
            }
            catch (Exception exception)
            {
                throw new RpcException("There was a problem sending the request to the wallet", exception);
            }

            string responseBody;
            using (StreamReader streamReaderResponse = new(httpResponseMessage.Content.ReadAsStream()))
                responseBody = streamReaderResponse.ReadToEnd();

            JsonRpcResponse<T>? rpcResponse;

            try
            {
                rpcResponse = JsonConvert.DeserializeObject<JsonRpcResponse<T>>(responseBody);
            }
            catch (JsonException jsonException)
            {
                throw new RpcResponseDeserializationException("There was a problem deserializing the response from the wallet", jsonException);
            }

            return rpcResponse!.Result ?? throw new RpcException("Result could not be deserialized.");
        }
    }
}