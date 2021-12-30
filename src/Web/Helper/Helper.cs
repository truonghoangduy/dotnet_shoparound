using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Enum;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Web.Helper
{
    // Render helper
    public static class ListExtensions
    {
        //  https://stackoverflow.com/questions/11463734/split-a-list-into-smaller-lists-of-n-size
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            // Convert 1D array to 2D array with consite chunk size
            // [1,2,3,4,5] chunked 2 => [[1,2],[3,4],[5]]
            // Ex chunked by 3 

            // LinQ doing it magic group by stuff madness
            return source
                .Select((x, i) => new { Index = i, Value = x })
                // 1,2,3 / 3 => which number divvied and over 3 will be group in it own list
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }

    public static class TransactionStatusColorHelper
    {
        public static String ColorByTransactionStatus(TransactionStatus status)
        {
            var order_status_color = "";
            if (status == TransactionStatus.WaitForComformation)
            {
                order_status_color = "bg-warning";
            }
            else if (status == TransactionStatus.Comformed)
            {
                order_status_color = "bg-info";

            }
            else if (status == TransactionStatus.Pending)
            {
                order_status_color = "bg-dark";

            }
            else if (status == TransactionStatus.Shipping)
            {
                order_status_color = "bg-danger";

            }
            else if (status == TransactionStatus.Shipping)
            {
                order_status_color = "bg-success";

            }
            return order_status_color;
        }

    }

    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
