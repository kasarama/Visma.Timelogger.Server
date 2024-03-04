using Newtonsoft.Json;
using System.Text;

namespace Visma.Timelogger.Api.Test.Integration.Base
{
    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
            => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
    }
}
