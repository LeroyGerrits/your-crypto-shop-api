namespace YourCryptoShop.Domain.Exceptions
{
    [Serializable]
    public class RpcResponseDeserializationException : Exception
    {
        public RpcResponseDeserializationException() { }

        public RpcResponseDeserializationException(string customMessage) : base(customMessage) { }

        public RpcResponseDeserializationException(string customMessage, Exception exception) : base(customMessage, exception) { }
    }
}