using BaseFramework.Security;
using BaseFramework.Utilities;
using BaseFramework.Web;
using Core_Mvc_Sample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Mvc_Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Post 통신 테스트
        /// </summary>
        public void WebTest()
        {
            string reqUrl = "http://naver.com";
            var reqParams = new List<KeyValuePair<string, object>>();
            reqParams.Add(new KeyValuePair<string, object>("mode", "1000"));
            reqParams.Add(new KeyValuePair<string, object>("USERID", "test"));

            string reqResult = WebRequestHelper.SendPost(reqUrl, reqParams);

        }

        /// <summary>
        /// AES 암호화 테스트
        /// </summary>
        public void EncTest()
        {
            string b = CryptoUtil.Encrypt("t");

            string ab = CryptoUtil.Decrypt(b);
        }

        /// <summary>
        /// 메일 발송 테스트
        /// </summary>
        public void MailTest()
        {
            try
            {
                MailHelper.SendMail("test.naver.com", "tmakdlf1522@naver.com", "테스트", "코어테스트 발송");

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        /// <summary>
        /// 세션 테스트
        /// </summary>
        public void SessionTest()
        {
            #region 세션 처리 방법 
            //세션 처리 방법


            SessionHelper.clear();

            SessionHelper.Set("tese", "1");

            SessionHelper.Set("tese1", "2");

            SessionHelper.Set("tese2", "3");

            SessionHelper.Set("tese3", "4");

            SessionHelper.Set("tese4", "5");

            string aasd = SessionHelper.Get("tese");


            string a2 = SessionHelper.Get("tese1");

            string a3 = SessionHelper.Get("tese2");

            string a4 = SessionHelper.Get("tese3");

            string a5 = SessionHelper.Get("tese4");

            string a6 = SessionHelper.Get("tese5");

            #endregion
        }

        /// <summary>
        /// 쿠키 테스트
        /// </summary>
        public void CookieTest()
        {
            SessionHelper.SetCookie("Cookie1", "1" + DateTime.Now.ToString("yyyyMMddhhmiss"));
            SessionHelper.SetCookie("Cookie2", "2" + DateTime.Now.ToString("yyyyMMddhhmiss"));
            SessionHelper.SetCookie("Cookie3", "3" + DateTime.Now.ToString("yyyyMMddhhmiss"));


            string test = SessionHelper.GetCookie("Cookie1");

            SessionHelper.RemoveCookie("Cookie2");
        }

    }
}
