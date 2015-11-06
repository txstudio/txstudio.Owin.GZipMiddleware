using Newtonsoft.Json;
using System.Web.Http;

namespace txstudio.Owin.GZipMiddleware
{
    //此為測試 WebApi 對於 GZip 壓縮支援的詳細內容
    [RoutePrefix("gzip-api")]
    public class GZipController : ApiController
    {
        [HttpPost]
        [Route("v1/AddObject")]
        public string AddObject(object item)
        {
            return JsonConvert.SerializeObject(item);
        }
    }
}
