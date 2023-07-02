using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppTests.Extensions
{
    public static class HttpContentExtension
    {
        public static HttpContent ToJsonHttpContent(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            return httpContent;
        }
    }
}
