using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFramework.Const
{
    public class AmazomCDN
    {
        //웹 appsettings.Json 추출 
        public static AppConfiguration AppConfig = new AppConfiguration();


        public static string AWSAccessKeyId { get { return "아마존S3 아이디"; } }
        public static string AWSSecretKey { get { return "아마존 S3 비밀번호"; } }

        //테스트와 실서버 분리 작업 
        //관리자 업로드 주소
        public static string AwsUploadhost
        {
            get
            {
                if (AppConfig.Mode == "dev")
                {
                    return "Dev.co.kr";
                }
                else
                {
                    return "Real.co.kr";
                }
            }
        }

        //관리자 읽기 호스트
        public static string AwsManagementReadHost
        {
            get
            {
                if (AppConfig.Mode == "dev")
                {
                    return "Dev.co.kr";
                }
                else
                {
                    return "Real.co.kr";
                }
            }
        }
        //사용자 읽기 호스트
        public static string AwsUserReadhost
        {
            get
            {
                if (AppConfig.Mode == "dev")
                {
                    return "Dev.co.kr";
                }
                else
                {
                    return "Real.co.kr";
                }
            }
        }

        // Video 원본 파일 업로드
        public static string AwsManagementUploadVideohost
        {
            get
            {
                if (AppConfig.Mode == "dev")
                {
                    return "Devvideo.co.kr";
                }
                else
                {
                    return "Realvideo.co.kr";
                }
            }
        }

        // Video 컨버팅 버전
        public static string AwsManagementVideoReadhost
        {
            get
            {
                if (AppConfig.Mode == "dev")
                {
                    return "Devvideo.co.kr";
                }
                else
                {
                    return "Realvideo.co.kr";
                }
            }
        }
    }


    //아마존 FileInfo
    public class CDNFileInfo
    {
        public string Name { get; set; }
        public long Length { get; set; }
        public string Thumbnail { get; set; }
    }
}
