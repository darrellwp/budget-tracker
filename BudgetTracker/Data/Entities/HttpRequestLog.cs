namespace BudgetTracker.Data.Entities
{
    /// <summary>
    /// Represents a log entry for an HTTP request and its corresponding response.
    /// </summary>
    public class HttpRequestLog
    {
        public string RequestUrl { get; set; } = null!;
        public string RequestMethod { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public string ClientIpAddress { get; set; } = null!;
        public string ReferrerUrl { get; set; } = null!;
        public int ResponseStatusCode { get; set; }
        public TimeSpan ResponseTime { get; set; }
        public DateTime DateTimeStamp { get; set; }
    }
}
