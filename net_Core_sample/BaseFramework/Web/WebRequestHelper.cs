using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BaseFramework.Web
{
    public class WebRequestHelper
    {
        /// <summary>
        /// Post 통신 진행
        /// </summary>
        /// <param name="url"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static string SendPost(string url, List<KeyValuePair<string, object>> datas)
        {
            string result = "";

            StringBuilder sendData = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in datas)
            {
                sendData.Append(string.Format("{0}{1}={2}", (sendData.Length == 0 ? "" : "&"), pair.Key, WebUtility.UrlEncode(pair.Value == null ? "" : pair.Value.ToString())));
            }

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            req.Timeout = 1200000;

            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            sw.Write(sendData.ToString());
            sw.Close();

            try
            {
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                var sr = new StreamReader(res.GetResponseStream());
                result = sr.ReadToEnd();
                sr.Close();
            }
            catch (WebException e)
            {
                var sr = new StreamReader(((HttpWebResponse)e.Response).GetResponseStream());
                result = sr.ReadToEnd();
                sr.Close();
            }

            return result;
        }
    }
}
