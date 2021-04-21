using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFramework.Security
{
	public class MaskingHelper
	{
		public static string Masking(string input, MaskingType type)
		{
			return Masking(input, type, false);
		}
		public static string Masking(string input, MaskingType type, bool alwaysMasking)
		{
			StringBuilder returnStr = new StringBuilder();
					
			if (!string.IsNullOrEmpty(input))
			{
				switch (type)
				{
					//아이디 형식
					case MaskingType.ID:

						if (input.Length <= 3)
						{
							returnStr.Append(input);
						}
						else
						{
							for (int i = 0; i < input.Length; i++)
							{
								returnStr.Append(i < 3 ? input[i] : '*');
							}
						}
						break;

					//이름 형식
					case MaskingType.Name:

						if (input.Length == 2)
						{
							returnStr.Append(input[0]);
							returnStr.Append('*');
						}
						else if (input.Length < 2)
						{
							returnStr.Append(input);
						}
						else
						{
							for (int i = 0; i < input.Length; i++)
							{
								returnStr.Append(i >= (int)input.Length / 3 && i < (int)input.Length * 2 / 3 ? '*' : input[i]);
							}
						}
						break;

					//전화번호 형식
					case MaskingType.Tel:

						if (input.Length <= 6)
						{
							returnStr.Append(input);
						}
						else
						{
							if (input.Split('-').Length >= 3)
							{
								bool flag = false;

								for (int i = 0; i < input.Length; i++)
								{
									if (input[i] == '-')
									{
										flag = !flag;
										returnStr.Append('-');
									}
									else
									{
										returnStr.Append(flag ? '*' : input[i]);
									}
								}
							}
							else
							{
								if (input.IndexOf('-') != -1)
								{
									returnStr.Append(input.Split('-')[0] + "-");
									input = input.Split('-')[1];

									for (int i = 0; i < input.Length; i++)
									{
										returnStr.Append(i < (int)input.Length / 2 ? '*' : input[i]);
									}
								}
								else
								{
									for (int i = 0; i < input.Length; i++)
									{
										returnStr.Append(i >= (int)input.Length / 3 && i < (int)(Math.Round((decimal)input.Length) / 3 * 2) ? '*' : input[i]);
									}
								}
							}
						}
						break;

					//이메일 형식
					case MaskingType.Email:

						//@가 없으면 아이디 형식으로 재치환
						if (!input.Contains('@'))
						{
							return Masking(input, MaskingType.ID, alwaysMasking);
						}
						else
						{
							var arr = input.Split('@');

							returnStr.Append(Masking(arr[0], MaskingType.ID, alwaysMasking));
							returnStr.Append('@');
							returnStr.Append(arr[1]);
						}
						break;

					//카드번호 마스킹
					case MaskingType.CardNo:
						for (int i = 0; i < input.Length; i++)
						{
							returnStr.Append((i < 6 || i > 11) ? input[i] : '*');
						}
						break;
				}
			}

			return returnStr.ToString();
		}



		public enum MaskingType
		{
			/// <summary>
			/// 아이디 형식. 3자리 이하인 경우는 그대로 반환, 아닌 경우는 4번째 문자부터 *로 치환
			/// </summary>
			ID,
			Name,
			Tel,
			Email,
			CardNo
		}
	}
}
