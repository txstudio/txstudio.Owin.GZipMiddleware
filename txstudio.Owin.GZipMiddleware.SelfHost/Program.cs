using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txstudio.Owin.GZipMiddleware.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string _baseAddress = "http://localhost:8080";

            using (WebApp.Start<Startup>(url: _baseAddress))
            {
                Console.WriteLine("正在監聽位址 {0}", _baseAddress);
                Console.WriteLine();
                Console.WriteLine("請點選任意鍵結束 ... ");
                Console.ReadKey();
            }
        }
    }
}
