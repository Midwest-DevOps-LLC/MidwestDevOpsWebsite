using System;

namespace MidwestDevOpsWebsite.Models
{
    public class ErrorViewModel
    {
        public string StatusCode { get; set; }
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string Path { get; set; }
    }
}