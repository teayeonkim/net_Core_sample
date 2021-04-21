using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFramework.Security
{
	public class ValidateExpressionHelper
	{
		/// <summary>
		/// 이메일 형태 검사 정규식
		/// </summary>
		public static string EmailBasic { get { return @"^[_\-\.0-9a-zA-Z]{2,30}@[-.0-9a-zA-z_\-]{2,30}\.[a-zA-Z]{2,30}$"; } }


		/// <summary>
		/// 일반적인 휴대전화 형태 체크 하이픈을 포함하거나 하지 않는 3/3or4/4 자리
		/// </summary>
		public static string BasicPhone { get { return "[0-9]{3}[-]{0,1}[0-9]{3,4}[-]{0,1}[0-9]{4}$"; } }


		/// <summary>
		/// 일반적인 아이디 형태 체크. 영문 소문자로 시작하고 숫자-영문 조합 6~15자리
		/// </summary>
		public static string BasicUserId { get { return @"^[a-z]{1}[a-z0-9]{5,14}$"; } }


		/// <summary>
		/// 주민번호 형태 여부 확인
		/// </summary>
		public static bool IsJumin(string ssn1, string ssn2)
		{
			string val = ssn1 + ssn2;

			char[] Arr = val.ToCharArray();

			if (Arr.Length != 13)
			{
				return false;
			}

			string mm = Arr[2].ToString() + Arr[3].ToString();
			string dd = Arr[4].ToString() + Arr[5].ToString();
			try
			{
				if (Int32.Parse(mm) == 0 || Int32.Parse(mm) > 12)
				{
					return false;
				}
				if (Int32.Parse(dd) == 0 || Int32.Parse(dd) > 31)
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}

			int TempSum = 0;
			int[] ArrTemp = { 2, 3, 4, 5, 6, 7, 8, 9, 2, 3, 4, 5 };
			for (int x = 0; x < ArrTemp.Length; x++)
			{
				TempSum += ArrTemp[x] * Int32.Parse(Arr[x].ToString());
			}

			if (Int32.Parse(Arr[12].ToString()) != 11 - TempSum % 11)
			{
				return false;
			}

			return true;
		}





		/// <summary>
		/// 파일 확장자로 업로드 가능한 파일인지 체크
		/// </summary>
		/// <param name="filename"></param>
		public static bool CheckUsableFileType(string filename, FileType fileType)
		{
			string fileExt = filename.Split('.').Last().ToLower();
			string availExt = "";

			switch (fileType)
			{
				case FileType.Image:
					availExt = "jpg,jpeg,png,gif,jpeg,ico,bmp";
					break;
				case FileType.Flash:
					availExt = "fla,swf,flv";
					break;
				case FileType.Movie:
					availExt = "mpg,mpeg,wmv,avi,asf,mp4";
					break;
				case FileType.Document:
					availExt = "doc,docx,txt,xls,xlsx,ppt,pptx,hwp,hwpx,pdf";
					break;
				default:
					availExt = "jpg,png,gif,jpeg,ico,bmp,fla,swf,flv,mpg,mpeg,wmv,avi,asf,mp4,doc,docx,txt,xls,xlsx,ppt,pptx,hwp,hwpx,zip,rar,alz,z7,iso";
					break;
			}

			return availExt.Split(',').Contains(fileExt);
		}
		public enum FileType { All, Document, Image, Flash, Movie }
	}
}
