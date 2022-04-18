namespace Finance.PciDss.Bridge.IceMarket.Server.Models
{
    public class ResultModel<T>
    {
        public bool IsFailed { get; set; }
        public T Value { get; set; }
        public string Message { get; set; }
    }
}