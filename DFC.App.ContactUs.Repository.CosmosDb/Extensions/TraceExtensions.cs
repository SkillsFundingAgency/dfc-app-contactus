using DFC.App.ContactUs.Data.Models;
using System;
using System.Diagnostics;

namespace DFC.App.ContactUs.Repository.CosmosDb.Extensions
{
    public static class TraceExtensions
    {
        public static void AddTraceInformation(this RequestTrace trace)
        {
            if (trace == null)
            {
                throw new ArgumentException($"{nameof(trace)} cannot be null");
            }

            //Can be null if called from a hosted service as Activity is initialized in middleware on incoming HTTP requests
            if (Activity.Current == null)
            {
                throw new InvalidOperationException($"Current Activity is null, Activity has not been initialized");
            }

            trace.AddTraceId(Activity.Current.TraceId.ToString());

            //Might not have a Parent Id if this is the first call
            trace.AddParentId(Activity.Current.ParentId != null ? Activity.Current.ParentId.ToString() : Activity.Current.TraceId.ToString());
        }
    }
}
