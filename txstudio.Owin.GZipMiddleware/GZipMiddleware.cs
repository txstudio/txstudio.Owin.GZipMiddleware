using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txstudio.Owin.GZipMiddleware
{
    public class GZipMiddleware : OwinMiddleware
    {
        public GZipMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        public async override Task Invoke(IOwinContext context)
        {
            var _requset = context.Request;
            var _gzipStream = new MemoryStream();

            Byte[] _unzip;
            Byte[] _zip;

            await _requset.Body.CopyToAsync(_gzipStream);
            _gzipStream.Position = 0;

            //如果再 Http Header 條件符合下列項目才進行解壓縮作業
            if (_requset.Headers["Content-Type"] == "application/gzip"
                && _requset.Headers["Content-Encoding"] == "gzip")
            {
                //如果包含壓縮內容，進行解壓縮
                _zip = _gzipStream.ToArray();
                _unzip = this.GZipUncompress(_zip);

                //將解壓縮的 Byte 陣列匯入到 HttpRequest 內文
                _requset.Body = new MemoryStream(_unzip);
                _requset.Body.Position = 0;

                //修改 Content-Type 為 application/json
                _requset.Headers.Remove("Content-Type");
                _requset.Headers.Add("Content-Type", new String[] { @"application/json" });
            }

            await Next.Invoke(context);
        }

        public Byte[] GZipUncompress(Byte[] buffer)
        {
            using (MemoryStream _mStream = new MemoryStream(buffer))
            {
                using (GZipStream _gzip = new GZipStream(_mStream, CompressionMode.Decompress))
                {
                    const int _size = 4096;
                    byte[] _buffer = new byte[_size];

                    using (MemoryStream _oStream = new MemoryStream())
                    {
                        int _count = 0;

                        do
                        {
                            _count = _gzip.Read(_buffer, 0, _size);
                            if (_count > 0)
                            {
                                _oStream.Write(_buffer, 0, _count);
                            }
                        } while (_count > 0);

                        return _oStream.ToArray();
                    }
                }
            }
        }
    }
}
