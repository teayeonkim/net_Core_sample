using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BaseFramework.Security;
using BaseFramework.Utilities;
using System.Configuration;
using Microsoft.EntityFrameworkCore;



namespace BaseFramework.Web
{
    public class SessionHelper
    {
        /*
      * NuGet 패키지 패키지 설치
      * Newtonsoft.Json
      * Microsoft.AspNetCore.Http.Abstractions
      * Microsoft.AspNetCore.Http.Extensions
      */



        //웹 appsettings.Json 추출 
        public static AppConfiguration AppConfig = new AppConfiguration();

        #region 세션 관련 처리

        /// <summary>
        /// 세션 일괄 삭제
        /// </summary>
        /// <param name="session"></param>
        public static void clear()
        {
            HttpContextAccessor context = new HttpContextAccessor();
            context.HttpContext.Session.Clear();
        }

        public static void Set(string key, string value)
        {
            HttpContextAccessor context = new HttpContextAccessor();
            context.HttpContext.Session.SetString(key, value);
        }

        public static string Get(string name)
        {
            HttpContextAccessor context = new HttpContextAccessor();
            return context.HttpContext.Session.GetString(name);
        }

        #endregion

        #region 쿠키 관련 처리

        public static void SetCookie(string key, string value)
        {
            HttpContextAccessor context = new HttpContextAccessor();

            var CookieOptions = new CookieOptions
            {
                // Set the secure flag, which Chrome's changes will require for SameSite none.
                // Note this will also require you to be running on HTTPS
                Secure = true,

                // Set the cookie to HTTP only which is good practice unless you really do need
                // to access it client side in scripts.
                HttpOnly = true,

                // Add the SameSite attribute, this will emit the attribute with a value of none.
                // To not emit the attribute at all set the SameSite property to SameSiteMode.Unspecified.
                SameSite = SameSiteMode.None
            };
            context.HttpContext.Response.Cookies.Append(context.HttpContext.Request.Host.Host + "_" + key, CryptoUtil.Encrypt(value), CookieOptions);
        }

        public static string GetCookie(string key)
        {
            HttpContextAccessor context = new HttpContextAccessor();

            return CryptoUtil.Decrypt(context.HttpContext.Request.Cookies[context.HttpContext.Request.Host.Host + "_" + key]);
        }

        public static void RemoveCookie(string key)
        {
            HttpContextAccessor context = new HttpContextAccessor();

            context.HttpContext.Response.Cookies.Delete(context.HttpContext.Request.Host.Host + "_" + key, new CookieOptions()
            {
                Secure = true,

                HttpOnly = true,

                SameSite = SameSiteMode.None
            });
        }

        #endregion

    }




}


