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
            //세션 설정
            SessionTest();
            //쿠키 설정
            CookieTest();

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
        #region


        /// <summary>
        /// 세션 테스트
        /// </summary>
        public void SessionTest()
        {
            #region 세션 처리 방법 
            //세션 처리 방법


          //  SessionHelper.clear();

            SessionHelper.Set("test", "Session_" + DateTime.Now.ToString("yyyyMMddhhmmss"));

            SessionHelper.Set("test1", "Session_1_" + DateTime.Now.ToString("yyyyMMddhhmmss"));

            SessionHelper.Set("test2", "Session_2_" + DateTime.Now.ToString("yyyyMMddhhmmss"));

            SessionHelper.Set("test3", "Session_3_" + DateTime.Now.ToString("yyyyMMddhhmmss"));

            SessionHelper.Set("test4", "Session_4_" + DateTime.Now.ToString("yyyyMMddhhmmss"));


            #endregion
        }

        public IActionResult Session()
        {
            return View();
        }

        #endregion
        #region 쿠키 테스트 
        /// <summary>
        /// 쿠키 테스트
        /// </summary>
        public void CookieTest()
        {
            SessionHelper.SetCookie("Cookie1", "1" + DateTime.Now.ToString("yyyyMMddhhmmss"));
            SessionHelper.SetCookie("Cookie2", "2" + DateTime.Now.ToString("yyyyMMddhhmmss"));
            SessionHelper.SetCookie("Cookie3", "3" + DateTime.Now.ToString("yyyyMMddhhmmss"));
        }

        public IActionResult Cookie()
        {
            return View();
        }

        #endregion
    }
}
