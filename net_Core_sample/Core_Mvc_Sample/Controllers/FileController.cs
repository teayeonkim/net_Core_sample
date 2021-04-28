using BaseFramework.Const;
using BaseFramework.IO;
using BaseFramework.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Mvc_Sample.Controllers
{
    public class FileController : Controller
    {
        /// <summary>
        /// appsettings JSON 선언
        /// </summary>
        IConfiguration _configuration;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.Message = "테스트";

            return View();
        }

        public async Task<IActionResult> FileUploadAmazonProc(IFormFile upFile, string bucketName, string path, int maxSize, string callbackFunc, ValidateExpressionHelper.FileType fileExtType, int? maxWidth, int? maxHeight, bool? resizeRatio)
        {
            if (string.IsNullOrEmpty(bucketName)) bucketName = AmazomCDN.AwsUploadhost;

            string Awspath = _configuration["uploadUrl"] + _configuration["rootDir"] + path;

            if (maxSize != 0 && upFile.Length > 10240)
            {
                ViewBag.Message = "에러::파일 크기 초과 - " + 10 + "MB 까지만 업로드 가능합니다.\n 업로드한 파일 크기 : " + upFile.Length;
            }

            if (upFile != null)
            {
                if (!ValidateExpressionHelper.CheckUsableFileType(upFile.FileName, fileExtType))
                {
                    ViewBag.Message = "에러:: 허용되지 않은 파일 종류입니다.";
                }
            }
            else
            {
                ViewBag.Message = "에러:: 파일을 선택해 주세요.";
            }

            //AWS 파일 업로드 

            string saveFilename = "";

            // 동영상 업로드
            if (ValidateExpressionHelper.CheckUsableFileType(upFile.FileName, ValidateExpressionHelper.FileType.Movie))
            {
                //미구현 예정 
            }
            else
            {
                if (!maxWidth.HasValue && !maxHeight.HasValue)
                {
                    saveFilename = await FileManager.FileSaveAmzom(Awspath, bucketName, upFile);
                }
                else
                {
                    if (!resizeRatio.HasValue) resizeRatio = false;
                    saveFilename = await FileManager.ImageSaveAmzom(Awspath, bucketName, upFile, maxWidth.Value, maxHeight.Value, resizeRatio.Value);
                }

                //스크립트 동작 안함. 처리 방법 고민 필요.

                ////  returnScript = string.Format("parent.{0}('{1}', '{2}', {3}, '{4}'); " + (Popup.HasValue ? " parent.clipJs.Utilities.CloseLayerPop();" : "")
                //                                          , callbackFunc
                //                                          , ConfigurationManager.AppSettings["uploadUrl"] + ConfigurationManager.AppSettings["rootDir"] + path
                //                                          , fileInfo.Name
                //                                          , fileInfo.Length
                //                                          , originFilename.Replace("'", "\\'"));


            }
            //ViewBag 로 파일명 리턴후 . 해당 페이지에서 스크립트 실행 진행
            ViewBag.Script = saveFilename;

            return View();
        }
    }
}
