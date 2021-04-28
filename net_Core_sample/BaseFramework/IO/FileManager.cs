using Amazon.S3;
using Amazon.S3.Model;
using BaseFramework.Const;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFramework.IO
{
    public class FileManager
    {
        //웹 appsettings.Json 추출 
        public static AppConfiguration AppConfig = new AppConfiguration();

        #region 이미지 크기를 {가로,세로} 배열로 비율에 맞추어 반환
        /// <summary>
        /// 이미지 크기를 {가로,세로} 배열로 비율에 맞추어 반환. 최대크기가 0일경우 기본크기 그대로 반환
        /// </summary>
        /// <param name="ImageWidth">원본이미지가로</param>
        /// <param name="ImageHeight">원본이미지세로</param>
        /// <param name="ThumbWidth">가로 최대값</param>
        /// <param name="ThumbHeight">세로 최대값</param>
        /// <returns>조건에 맞는 적정 이미지 사이즈</returns>
        private static int[] GetThumbSize(int ImageWidth, int ImageHeight, int ThumbWidth, int ThumbHeight)
        {
            int[] ReturnValue;

            if (ThumbHeight == 0 || ThumbWidth == 0)
            {
                ReturnValue = new int[] { ImageWidth, ImageHeight };
            }
            else
            {
                float wRatio = (float)ThumbWidth / (float)ImageWidth;
                float hRatio = (float)ThumbHeight / (float)ImageHeight;
                float setRatio;

                if (wRatio > hRatio)
                {
                    setRatio = hRatio;
                }
                else
                {
                    setRatio = wRatio;
                }

                int ReturnWidth = (int)(ImageWidth * setRatio);
                int ReturnHeight = (int)(ImageHeight * setRatio);

                ReturnValue = new int[] { ReturnWidth, ReturnHeight };
            }

            return ReturnValue;
        }
        #endregion

        #region Aws Core 파일 저장

        /// <summary>
        /// S3 파입 업로드
        /// </summary>
        /// <param name="FileSaveDir"></param> 폴더 경로 폴더 없을시 신규 생성 됨 
        /// <param name="bucketName"></param>  업로드 버킷 이름
        /// <param name="postedFile"></param>  파일 정보
        /// <returns></returns>
        public static async Task<string> FileSaveAmzom(string FileSaveDir, string bucketName, IFormFile postedFile)
        {
            AmazonS3Client client = null;

            if (AppConfig.Mode == "dev")
            {
                client = new AmazonS3Client(AmazomCDN.AWSAccessKeyId, AmazomCDN.AWSSecretKey, Amazon.RegionEndpoint.APNortheast2);
            }
            else
            {
                client = new AmazonS3Client(AmazomCDN.AWSAccessKeyId, AmazomCDN.AWSSecretKey, Amazon.RegionEndpoint.APNortheast2);
            }

            string saveFilename = postedFile.FileName.Split('\\')[postedFile.FileName.Split('\\').Length - 1];

            //동일 파일이 존재하는 경우 파일 체크 타 이름으로 업로드 처리 
            saveFilename = await CheckFileAmazom(bucketName, FileSaveDir, saveFilename);



            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = postedFile.OpenReadStream(),
                    BucketName = bucketName,
                    Key = FileSaveDir + "/" + saveFilename
                };
                PutObjectResponse response = await client.PutObjectAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return saveFilename;
                else
                    return "";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 이미지를 저장합니다. 
        /// </summary>
        /// <param name="FileSaveDir">이미지저장경로</param>
        /// <param name="postedFile">저장할 이미지 정보를 담고있는 HttpPostedFile객체</param>
        /// <param name="originFilename">저장된 파일 원래이름</param>
        /// <param name="MaxWidth">최대 이미지 가로사이즈</param>
        /// <param name="MaxHeight">최대 이미지 세로사이즈</param>
        /// <param name="Ratio">비율에 맞게 리사이징할지 여부</param>
        /// <returns>생성된 파일명</returns>
        public static async Task<string> ImageSaveAmzom(string FileSaveDir, string bucketName, IFormFile postedFile, int MaxWidth, int MaxHeight, bool Ratio)
        {
            AmazonS3Client client = null;

            if (AppConfig.Mode == "dev")
            {
                client = new AmazonS3Client(AmazomCDN.AWSAccessKeyId, AmazomCDN.AWSSecretKey, Amazon.RegionEndpoint.APNortheast2);
            }
            else
            {
                client = new AmazonS3Client(AmazomCDN.AWSAccessKeyId, AmazomCDN.AWSSecretKey, Amazon.RegionEndpoint.APNortheast2);
            }

            string saveFilename = postedFile.FileName.Split('\\')[postedFile.FileName.Split('\\').Length - 1];

            //동일 파일이 존재하는 경우 파일 체크 타 이름으로 업로드 처리 
            saveFilename = await CheckFileAmazom(bucketName, FileSaveDir, saveFilename);

            System.Drawing.Image image;

            var memoryStream = new MemoryStream();
            await postedFile.CopyToAsync(memoryStream);

            image = System.Drawing.Image.FromStream(memoryStream, true);

            int[] ThumbSize = Ratio ? GetThumbSize(image.Width, image.Height, MaxWidth, MaxHeight) : new int[] { MaxWidth, MaxHeight };

            int ThumbWidth = ThumbSize[0];
            int ThumbHeight = ThumbSize[1];

            Bitmap bm = new Bitmap(ThumbWidth, ThumbHeight, PixelFormat.Format32bppArgb);
            bm.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            Graphics gp = Graphics.FromImage(bm);
            gp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gp.DrawImage(image, new Rectangle(0, 0, ThumbWidth, ThumbHeight), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            gp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            gp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gp.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            gp.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            MemoryStream stream = new MemoryStream();
            string FileExt = saveFilename.Substring(saveFilename.LastIndexOf('.') + 1);

            switch (FileExt)
            {
                case "jpeg":
                case "jpg":
                    //var codec = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().Where(x => x.FormatID == ImageFormat.Jpeg.Guid).FirstOrDefault();
                    //var eps = new System.Drawing.Imaging.EncoderParameters(1);
                    //eps.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                    //bm.Save(stream, codec, eps);

                    bm.Save(stream, ImageFormat.Png);
                    break;
                case "gif":
                    bm.Save(stream, ImageFormat.Png);
                    break;
                case "png":
                    bm.Save(stream, ImageFormat.Png);
                    break;
                case "ico":
                    bm.Save(stream, ImageFormat.Icon);
                    break;
                case "bmp":
                    bm.Save(stream, ImageFormat.Bmp);
                    break;
                default:
                    break;
            }
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = stream,
                BucketName = bucketName,
                Key = FileSaveDir + "/" + saveFilename
            };
            PutObjectResponse response = await client.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return saveFilename;
            else
                return "";
        }



        /// <summary>
        /// S3 해당 폴더에 중복 파일명이 있는지 확인 있는 경우. 파일 명 변경
        /// </summary>
        /// <param name="bucketName"></param> 버킷명 
        /// <param name="FileSaveDir"></param> 파일 경로
        /// <param name="FileName"></param> 원본 파일 이름
        /// <returns></returns>
        public static async Task<string> CheckFileAmazom(string bucketName, string FileSaveDir, string FileName)
        {
            string ReturnFileName = "";

            AmazonS3Client client = null;

            if (AppConfig.Mode == "dev")
            {
                client = new AmazonS3Client(AmazomCDN.AWSAccessKeyId, AmazomCDN.AWSSecretKey, Amazon.RegionEndpoint.APNortheast2);
            }
            else
            {
                client = new AmazonS3Client(AmazomCDN.AWSAccessKeyId, AmazomCDN.AWSSecretKey, Amazon.RegionEndpoint.APNortheast2);
            }

            ListObjectsRequest request = new ListObjectsRequest()
            {
                BucketName = bucketName,
                Prefix = FileSaveDir
            };

            ListObjectsResponse response = await client.ListObjectsAsync(request);

            string FileExt = FileName.Substring(FileName.LastIndexOf('.') + 1);

            List<string> availableFileExt = new List<string>(){
                                            "jpg", "gif", "png", "jpeg", "ico", "bmp"								//이미지
											, "mpg", "mpeg", "avi", "asf", "wmv", "mp4"								//동영상
											, "flv", "swf"															//플래시
											, "txt", "doc", "docx", "ppt", "pptx", "xls", "xlsx", "hwp", "rtf"		//문서
											, "zip", "alz", "rar", "a7"												//압축포멧
										};
            if (!availableFileExt.Contains(FileExt.ToLower()))
            {
                throw new Exception("금지된 파일 타입 업로드 시도");
            }

            foreach (S3Object itemsInsideDirectory in response.S3Objects)
            {
                if (itemsInsideDirectory.Key == FileSaveDir + "/" + FileName)
                {
                    ReturnFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N") + "." + FileExt;
                }
            }
            return ReturnFileName;
        }
        #endregion
    }
}
