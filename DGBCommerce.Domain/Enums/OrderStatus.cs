namespace DGBCommerce.Domain.Enums
{
    public enum OrderStatus
    {
        New = 0,
        AwaitingPayment = 1,
        Abandoned = 2,
        Paid = 3,
        Shipped = 4,
        Finished = 5
    }
}