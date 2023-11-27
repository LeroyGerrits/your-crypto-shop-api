using DGBCommerce.Domain.Enums;
using System.Runtime.Serialization;

namespace DGBCommerce.Domain.Exceptions
{
    public class RpcInternalServerErrorException : Exception
    {
        public RpcInternalServerErrorException() { }

        public RpcInternalServerErrorException(string customMessage) : base(customMessage) { }

        public RpcInternalServerErrorException(string customMessage, Exception exception) : base(customMessage, exception) { }

        public RpcErrorCode? RpcErrorCode { get; set; }
    }
}