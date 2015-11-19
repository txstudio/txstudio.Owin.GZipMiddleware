using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;

namespace txstudio.Owin.GZipMiddleware.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding _encoding;
            AccountLoginViewModel _model;

            HttpClient _client;
            StreamContent _streamContent;

            Byte[] _beforeCompress;
            Byte[] _afterCompress;

            String _baseAddress;
            String _relativeUrl;

            //使用 Console Self-Host 位址
            _baseAddress = ("http://localhost:8080/");

            //使用 IIS Express 位址
            //_baseAddress = ("http://localhost:60670/");
            _relativeUrl = ("/gzip-api/v1/AddObject");


            //建立資料交換使用的 Json 格式文字
            _model = new AccountLoginViewModel();
            _model.Account = "sys@phihong.com.tw";
            _model.Password = "password";


            #region 初始化相關物件內容
            _encoding = Encoding.GetEncoding("utf-8");
            _beforeCompress = _encoding.GetBytes(JsonConvert.SerializeObject(_model));

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseAddress);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            #endregion


            #region 使用 GZip 壓縮 Json 文字內容為
            using (MemoryStream _mStream = new MemoryStream())
            {
                using (GZipStream _gzipStream = new GZipStream(_mStream, CompressionMode.Compress, true))
                {
                    _gzipStream.Write(_beforeCompress, 0, _beforeCompress.Length);
                }
                _afterCompress = _mStream.ToArray();
            }
            #endregion


            #region 設定要傳遞的 Stream 內容
            /*
                需要設定 Http Header 為 
                Content-Typ = application/gzip
                才可進行伺服器端解壓縮作業
            */
            _streamContent = new StreamContent(new MemoryStream(_afterCompress));
            _streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/gzip");
            #endregion

            /*
            // 如果你要傳遞 application/json 內容，將上方的程式碼修改成下面方式
            _streamContent = new StreamContent(new MemoryStream(_beforeCompress));
            _streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            */

            var _response = _client.PostAsync(_relativeUrl, _streamContent).Result;

            if (_response.IsSuccessStatusCode == true)
            {
                Console.WriteLine("回應內容：{0}", _response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                Console.WriteLine("資料傳遞發生錯誤 {0}", _response.ReasonPhrase);
            }

            Console.WriteLine("請點選任意鍵結束 ... ");
            Console.ReadKey();

        }
    }

    public class AccountLoginViewModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
