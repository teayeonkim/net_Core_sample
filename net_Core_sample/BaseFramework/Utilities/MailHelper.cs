using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BaseFramework.Utilities
{
    public class MailHelper
    {
        //웹 appsettings.Json 추출 
        public static AppConfiguration AppConfig = new AppConfiguration();

        /// <summary>
        /// 웹서버의 SMTP서비스를 이용하여 메일발송
        /// </summary>
        /// <param name="from">보내는이</param>
        /// <param name="to">받는이</param>
        /// <param name="title">메일제목</param>
        /// <param name="content">메일내용</param>
        public static void SendMail(string from, string to, string title, string content)
        {
            SendMail(from, "", new List<string>() { to }, title, content, new List<Attachment>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="fromName"></param>
        /// <param name="to"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void SendMail(string from, string fromName, string to, string title, string content)
        {
            SendMail(from, fromName, new List<string>() { to }, title, content, new List<Attachment>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void SendMail(string from, List<string> to, string title, string content)
        {
            SendMail(from, "", to, title, content, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="to"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="attachments"></param>
        public static void SendMail(string from, List<string> to, string title, string content, List<Attachment> attachments)
        {
            SendMail(from, "", to, title, content, attachments);
        }

        public static void SendMail(string from, string fromName, List<string> to, string title, string content, List<Attachment> attachments)
        {
            MailMessage mm = new MailMessage();
            if (string.IsNullOrEmpty(fromName))
            {
                mm.From = new MailAddress(from, "", System.Text.Encoding.UTF8);
            }
            else
            {
                mm.From = new MailAddress(from, fromName, System.Text.Encoding.UTF8);
            }

            foreach (var t in to)
            {
                mm.To.Add(t);
            }

            mm.Subject = title;
            mm.Body = content;
            mm.BodyEncoding = System.Text.Encoding.UTF8;
            mm.SubjectEncoding = System.Text.Encoding.UTF8;
            mm.IsBodyHtml = true;

            foreach (var attachment in attachments)
            {
                mm.Attachments.Add(attachment);
            }

            try
            {
                SmtpClient client = new SmtpClient("Url", 25);
                client.UseDefaultCredentials = false; // 시스템에 설정된 인증 정보를 사용하지 않는다.
                client.EnableSsl = true;  // SSL
                client.DeliveryMethod = SmtpDeliveryMethod.Network; // 
                client.Credentials = new System.Net.NetworkCredential("아이디", "비밀번호");
                client.Send(mm);



                using (SqlHelper db = new SqlHelper(AppConfig.Mail_Db))
                {
                    db.ClearParameter();
                    db.AddInParameter("@ToMail", SqlDbType.NVarChar, mm.From.ToString());
                    db.AddInParameter("@FoMail", SqlDbType.NVarChar, to.FirstOrDefault().ToString());
                    db.AddInParameter("@title", SqlDbType.NVarChar, title.ToString());
                    db.AddInParameter("@Content", SqlDbType.Text, content.ToString());
                    db.AddInParameter("@errMsg", SqlDbType.Text, "");
                    db.AddInParameter("@regdate", SqlDbType.DateTime2, DateTime.Now);

                    db.ExecuteQuery(@"
                        INSERT INTO [dbo].[tbl_Mail]
                                   ([ToMail]
                                   ,[FoMail]
                                   ,[title]
                                   ,[Content]
                                   ,[errMsg]
                                   ,[regdate])
                             VALUES
                                   (@ToMail
                                   ,@FoMail
                                   ,@title
                                   ,@Content
                                   ,@errMsg
                                   ,@regdate
                       )


                        ", 5);
                }

            }
            catch (Exception e)
            {
                using (SqlHelper db = new SqlHelper(AppConfig.Mail_Db))
                {
                    db.ClearParameter();
                    db.AddInParameter("@ToMail", SqlDbType.NVarChar, mm.From.ToString());
                    db.AddInParameter("@FoMail", SqlDbType.NVarChar, to.FirstOrDefault().ToString());
                    db.AddInParameter("@title", SqlDbType.NVarChar, title.ToString());
                    db.AddInParameter("@Content", SqlDbType.Text, content.ToString());
                    db.AddInParameter("@errMsg", SqlDbType.Text, e.ToString());
                    db.AddInParameter("@regdate", SqlDbType.DateTime2, DateTime.Now);

                    db.ExecuteQuery(@"

                        INSERT INTO [dbo].[tbl_Mail]
                                   ([ToMail]
                                   ,[FoMail]
                                   ,[title]
                                   ,[Content]
                                   ,[errMsg]
                                   ,[regdate])
                             VALUES
                                   (@ToMail
                                   ,@FoMail
                                   ,@title
                                   ,@Content
                                   ,@errMsg
                                   ,@regdate
                       )


                        ", 5);
                }

            }
        }

    }
}
