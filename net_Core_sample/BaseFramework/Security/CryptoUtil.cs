using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace BaseFramework.Security
{
	public class CryptoUtil
	{
		//웹 appsettings.Json 추출 
		public static AppConfiguration AppConfig = new AppConfiguration();

		#region 기본 암호화는 AES로 연결
		//암호화
		public static string Encrypt(string val)
		{
			string key = AppConfig.CryptoKey_AES;

			return Encrypt(val, key);
		}
		public static string Encrypt(object val) { return val == null ? "" : Encrypt(val.ToString()); }
		public static string Encrypt(string val, string key) { return EncAes(val, key); }
		public static string Encrypt(object val, string key) { return val == null ? "" : Encrypt(val.ToString(), key); }

		//복호화
		public static string Decrypt(string val)
		{
			string key = AppConfig.CryptoKey_AES;

			return Decrypt(val, key);
		}
		public static string Decrypt(object val) { return val == null ? "" : Decrypt(val.ToString()); }
		public static string Decrypt(string val, string key) { return DecAes(val, key); }
		public static string Decrypt(object val, string key) { return val == null ? "" : Decrypt(val.ToString(), key); }
		#endregion

		#region Aes 암호화
		//암호화
		public static string EncAes(string str)
		{
			//AppConfiguration AppConfig = new AppConfiguration();

			string key = AppConfig.CryptoKey_AES;
			return EncAes(str, key);
		}

		public static String EncAes(String Input, String key)
		{
			var aes = System.Security.Cryptography.Aes.Create();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			aes.Key = Encoding.Default.GetBytes(key.Substring(0, 32));
			aes.IV = Encoding.Default.GetBytes(key.Substring(0, 16));
			//aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			//aes.IV = new byte[] { 0, 1, 0, 3, 2, 2, 8, 0, 2, 6, 4, 0, 8, 0, 3, 0 };

			var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
			byte[] xBuff = null;
			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
				{
					byte[] xXml = Encoding.UTF8.GetBytes(Input);
					cs.Write(xXml, 0, xXml.Length);
				}

				xBuff = ms.ToArray();
			}

			String Output = Convert.ToBase64String(xBuff);
			return Output;
		}


		//복호화
		public static string DecAes(string str)
		{
			AppConfiguration a = new AppConfiguration();
			string key = a.CryptoKey_AES;
			return DecAes(str, key);
		}

		public static String DecAes(String Input, String key)
		{
			var aes = System.Security.Cryptography.Aes.Create();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			aes.Key = Encoding.Default.GetBytes(key.Substring(0, 32));
			aes.IV = Encoding.Default.GetBytes(key.Substring(0, 16));
			//aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			//aes.IV = new byte[] { 0, 1, 0, 3, 2, 2, 8, 0, 2, 6, 4, 0, 8, 0, 3, 0 };

			var decrypt = aes.CreateDecryptor();
			byte[] xBuff = null;
			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
				{
					byte[] xXml = Convert.FromBase64String(Input);
					cs.Write(xXml, 0, xXml.Length);
				}

				xBuff = ms.ToArray();
			}

			String Output = Encoding.UTF8.GetString(xBuff);
			return Output;
		}

		#endregion
	}
}
