using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFramework
{
    public class AppConfiguration
    {
        /* Web 프로젝트 설정값 수취 패키지 설치 진행
            Microsoft.Extensions.Configuration
            Microsoft.Extensions.Configuration.Abstractions 
            Microsoft.Extensions.Configuration.Json
            Microsoft.AspNetCore.Http
            
            패키지 설치 필요
        */
        //클립 암호화키 AES
        public readonly string _CryptoKey_AES = string.Empty;
        //SIte 엔티티
        public readonly string _Mail_Db = string.Empty;
        //디버깅 여부
        public readonly string _Mode = string.Empty;
        
        public AppConfiguration()
        {
            
            var configurationBuilder = new ConfigurationBuilder();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
   
            #if DEBUG //디버깅 모드
                path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
            #endif
            configurationBuilder.AddJsonFile(path, optional: true, reloadOnChange: true);
            var root = configurationBuilder.Build();

            //암호화키 추출
            _CryptoKey_AES = root.GetSection("CryptoKey_AES").Value;

            //사이트 엔티티 추출
            _Mail_Db = root.GetSection("ConnectiongStrings:Mail_Db").Value;

            //디버깅 모드여부 추출
            _Mode = root.GetSection("Mode").Value;
        }

        //암호화키 수취
        public string CryptoKey_AES
        {
            get => _CryptoKey_AES;
        }

        //사이트 엔티티 추출
        public string Mail_Db
        {
            get => _Mail_Db;
        }

        //디버깅 여부 
        public string Mode
        {
            get => _Mode;
        }


    }
}
