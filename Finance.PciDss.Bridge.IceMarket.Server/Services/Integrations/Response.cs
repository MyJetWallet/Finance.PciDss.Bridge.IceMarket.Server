namespace Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations
{
    public class Response<TSuccessResult, TFailedResult>
        where TSuccessResult : class
        where TFailedResult : class
    {
        private Response(TFailedResult failedResult)
        {
            FailedResult = failedResult;
        }

        private Response(TSuccessResult successResult)
        {
            SuccessResult = successResult;
        }

        public TSuccessResult SuccessResult { get; }
        public TFailedResult FailedResult { get; }

        public bool IsFailed => FailedResult != null;

        public static Response<TSuccessResult, TFailedResult> CreateFailed(TFailedResult result)
        {
            return new Response<TSuccessResult, TFailedResult>(result);
        }

        public static Response<TSuccessResult, TFailedResult> CreateSuccess(TSuccessResult result)
        {
            return new Response<TSuccessResult, TFailedResult>(result);
        }
    }
}