using System;

namespace DFC.App.ContactUs.Data.Models
{
    public class RequestTrace
    {
        public string? TraceId { get; private set; }

        public string? ParentId { get; private set; }

        public void AddTraceId(string traceId)
        {
            if (string.IsNullOrWhiteSpace(traceId))
            {
                throw new ArgumentException($"{nameof(TraceId)} cannot be null");
            }

            TraceId = traceId;
        }

        public void AddParentId(string parentId)
        {
            if (string.IsNullOrWhiteSpace(parentId))
            {
                throw new ArgumentException($"{nameof(TraceId)} cannot be null");
            }

            ParentId = parentId;
        }
    }
}
