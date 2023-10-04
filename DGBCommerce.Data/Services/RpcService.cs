using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DGBCommerce.Domain.Exceptions;
using DGBCommerce.Domain.Interfaces.Services;
using DGBCommerce.Domain.Models.Request;
using DGBCommerce.Domain.Models.Response;
using Newtonsoft.Json;

namespace DGBCommerce.Data.Services
{
    public class RpcService : IRpcService
    {
        private readonly string _daemonUrl;
        private readonly string _rpcUsername;
        private readonly string _rpcPassword;

        public RpcService(string deamonUrl, string rpcUsername, string rpcPassword)
        {
            _daemonUrl = deamonUrl;
            _rpcUsername = rpcUsername;
            _rpcPassword = rpcPassword;
        }

        public async Task<uint> GetBlockCount()
            => await Request<uint>("getblockcount");

        public async Task<GetDifficultyResponse> GetDifficulty()
            => await Request<GetDifficultyResponse>("getdifficulty");

        public async Task<GetMiningInfoResponse> GetMiningInfo()
            => await Request<GetMiningInfoResponse>("getmininginfo");

        private async Task<T> Request<T>(string rpcMethod, params object[] parameters)
        {
            var jsonRpcRequest = new JsonRpcRequest(1, rpcMethod.ToString(), parameters);
            var webRequest = (HttpWebRequest)WebRequest.Create(_daemonUrl);
            webRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(_rpcUsername + ":" + _rpcPassword)));
            webRequest.Credentials = new NetworkCredential(_rpcUsername, _rpcPassword);
            webRequest.ContentType = "application/json-rpc";
            webRequest.Method = "POST";
            webRequest.Proxy = null;
            webRequest.Timeout = 30000;
            var byteArray = jsonRpcRequest.GetBytes();
            webRequest.ContentLength = jsonRpcRequest.GetBytes().Length;

            /*HttpClientHandler handler = new() { 
                Credentials = new NetworkCredential(_rpcUsername, _rpcPassword), 
                Proxy = null
            };
            HttpClient client = new(handler)
            {
                Timeout = new TimeSpan(30000000)
            };
            var webRequestX = new HttpRequestMessage(HttpMethod.Post, _daemonUrl)
            {
                Content = new StringContent(jsonRpcRequest.GetRaw(), Encoding.UTF8, "application/json-rpc")
            };
            webRequestX.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(_rpcUsername + ":" + _rpcPassword)));

            var response = await client.SendAsync(webRequestX);
            var reader = new StreamReader(response.Content.ReadAsStream());
            var responseBody = reader.ReadToEnd();*/


            try
            {
                using var dataStream = await webRequest.GetRequestStreamAsync();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Dispose();
            }
            catch (Exception exception)
            {
                throw new RpcException("There was a problem sending the request to the wallet", exception);
            }

            try
            {
                string json;

                using (WebResponse webResponse = await webRequest.GetResponseAsync())
                {
                    using var stream = webResponse.GetResponseStream();
                    using var reader = new StreamReader(stream);
                    var result = reader.ReadToEnd();
                    reader.Dispose();
                    json = result;
                }

                var rpcResponse = JsonConvert.DeserializeObject<JsonRpcResponse<T>>(json);
                return rpcResponse!.Result ?? throw new RpcException("Result could not be deserialized.");
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
                                using var stream = webResponse.GetResponseStream() ?? throw new RpcException("The RPC request was either not understood by the server or there was a problem executing the request", webException);
                                using var reader = new StreamReader(stream);
                                var result = reader.ReadToEnd();
                                reader.Dispose();

                                try
                                {
                                    var jsonRpcResponseObject = JsonConvert.DeserializeObject<JsonRpcResponse<object>>(result);
                                    var exceptionMessage = string.Empty;

                                    if (jsonRpcResponseObject != null && jsonRpcResponseObject.Error != null)
                                    {
                                        exceptionMessage = jsonRpcResponseObject!.Error!.Message;
                                    }

                                    RpcInternalServerErrorException internalServerErrorException = new(exceptionMessage, webException)
                                    {
                                        RpcErrorCode = jsonRpcResponseObject.Error.Code
                                    };

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
            catch (JsonException jsonException)
            {
                throw new RpcResponseDeserializationException("There was a problem deserializing the response from the wallet", jsonException);
            }
            catch (ProtocolViolationException protocolViolationException)
            {
                throw new RpcException("Unable to connect to the server", protocolViolationException);
            }
            catch (Exception exception)
            {
                var queryParameters = jsonRpcRequest.Parameters.Cast<string>().Aggregate(string.Empty, (current, parameter) => current + (parameter + " "));
                throw new Exception($"A problem was encountered while calling MakeRpcRequest() for: {jsonRpcRequest.Method} with parameters: {queryParameters}. \nException: {exception.Message}");
            }
        }
    }
}
