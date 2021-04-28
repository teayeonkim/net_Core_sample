using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFramework.Utilities
{
	public class JsonHelper
	{
		public static string Serialize(object obj, JsonSerializerSettings setting)
		{
			return JsonConvert.SerializeObject(obj, setting);
		}
		public static string Serialize(object obj)
		{
			return Serialize(obj, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
		}



		public static T Deserialize<T>(string jsonStr, JsonSerializerSettings setting)
		{
			if (string.IsNullOrEmpty(jsonStr)) return default(T);

			return JsonConvert.DeserializeObject<T>(jsonStr, setting);
		}
		public static T Deserialize<T>(string jsonStr)
		{
			return Deserialize<T>(jsonStr, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
		}

		public static List<T> Deserialize<T>(List<T> list)
		{
			throw new NotImplementedException();
		}
	}
}