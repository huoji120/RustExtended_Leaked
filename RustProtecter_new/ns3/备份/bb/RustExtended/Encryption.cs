using System;
using System.Text;

namespace RustExtended
{
	public class Encryption
	{
		private static char[] char_0 = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'#',
			'$'
		};

		public static string Encrypt(string text)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(text);
			byte b = bytes[bytes.Length - 1];
			for (int i = bytes.Length - 2; i >= 0; i--)
			{
				byte b2 = bytes[i];
				byte[] array = bytes;
				int num = i;
				byte[] expr_31_cp_0 = array;
				int expr_31_cp_1 = num;
				expr_31_cp_0[expr_31_cp_1] ^= b;
				b = b2;
			}
			byte[] array2 = new byte[bytes.Length + 1];
			bytes.CopyTo(array2, 1);
			array2[0] = b;
			return BitConverter.ToString(array2).Replace("-", "");
		}

		public static string EncryptBitwise(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			for (int i = 0; i < bytes.Length - 1; i++)
			{
				byte[] array = bytes;
				int num = i;
				byte[] expr_1D_cp_0 = array;
				int expr_1D_cp_1 = num;
				expr_1D_cp_0[expr_1D_cp_1] ^= bytes[i + 1];
			}
			byte[] array2 = bytes;
			int num2 = bytes.Length - 1;
			byte[] expr_55_cp_0 = array2;
			int expr_55_cp_1 = num2;
			expr_55_cp_0[expr_55_cp_1] ^= bytes[0];
			return Encoding.Default.GetString(bytes);
		}

		public static string DecryptBitwise(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			byte[] array = bytes;
			int num = bytes.Length - 1;
			byte[] expr_1C_cp_0 = array;
			int expr_1C_cp_1 = num;
			expr_1C_cp_0[expr_1C_cp_1] ^= bytes[0];
			for (int i = bytes.Length - 1; i > 0; i--)
			{
				byte[] array2 = bytes;
				int num2 = i - 1;
				byte[] expr_46_cp_0 = array2;
				int expr_46_cp_1 = num2;
				expr_46_cp_0[expr_46_cp_1] ^= bytes[i];
			}
			return Encoding.Default.GetString(bytes);
		}

		public static string Encrypt64(string text)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(text);
			int num = bytes.Length;
			int num2;
			int num3;
			if (num % 3 == 0)
			{
				num2 = 0;
				num3 = num / 3;
			}
			else
			{
				num2 = 3 - num % 3;
				num3 = (num + num2) / 3;
			}
			int num4 = num + num2;
			byte[] array = new byte[num4];
			for (int i = 0; i < num4; i++)
			{
				if (i < num)
				{
					array[i] = bytes[i];
				}
				else
				{
					array[i] = 0;
				}
			}
			byte[] array2 = new byte[num3 * 4];
			for (int j = 0; j < num3; j++)
			{
				byte b = array[j * 3];
				byte b2 = array[j * 3 + 1];
				byte b3 = array[j * 3 + 2];
				byte b4 = (byte)((b & 252) >> 2);
				byte b5 = (byte)((b & 3) << 4);
				byte b6 = (byte)((b2 & 240) >> 4);
				b6 += b5;
				b5 = (byte)((b2 & 15) << 2);
				byte b7 = (byte)((b3 & 192) >> 6);
				b7 += b5;
				byte b8 = (byte)(b3 & 63);
				array2[j * 4] = b4;
				array2[j * 4 + 1] = b6;
				array2[j * 4 + 2] = b7;
				array2[j * 4 + 3] = b8;
			}
			char[] array3 = new char[num3 * 4];
			for (int k = 0; k < num3 * 4; k++)
			{
				if (array2[k] >= 0 && array2[k] <= 63)
				{
					array3[k] = Encryption.char_0[(int)array2[k]];
				}
				else
				{
					array3[k] = ' ';
				}
			}
			switch (num2)
			{
			case 1:
				array3[num3 * 4 - 1] = '=';
				break;
			case 2:
				array3[num3 * 4 - 1] = '=';
				array3[num3 * 4 - 2] = '=';
				break;
			}
			return new string(array3);
		}

		public static string Decrypt64(string text)
		{
			int length = text.Length;
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (text[length - i - 1] == '=')
				{
					num++;
				}
			}
			int num2 = length / 4;
			int num3 = num2 * 3;
			byte[] array = new byte[length];
			byte[] array2 = new byte[num3];
			for (int j = 0; j < length; j++)
			{
				array[j] = Encryption.smethod_1(text[j]);
			}
			for (int k = 0; k < num2; k++)
			{
				byte b = array[k * 4];
				byte b2 = array[k * 4 + 1];
				byte b3 = array[k * 4 + 2];
				byte b4 = array[k * 4 + 3];
				byte b5 = (byte)(b << 2);
				byte b6 = (byte)((b2 & 48) >> 4);
				b6 += b5;
				b5 = (byte)((b2 & 15) << 4);
				byte b7 = (byte)((b3 & 60) >> 2);
				b7 += b5;
				b5 = (byte)((b3 & 3) << 6);
				byte b8 = b4;
				b8 += b5;
				array2[k * 3] = b6;
				array2[k * 3 + 1] = b7;
				array2[k * 3 + 2] = b8;
			}
			byte[] array3 = new byte[num3 - num];
			for (int l = 0; l < array3.Length; l++)
			{
				array3[l] = array2[l];
			}
			return Encoding.ASCII.GetString(array3);
		}

		private static char smethod_0(byte byte_0)
		{
			char result;
			if (byte_0 >= 0 && byte_0 <= 63)
			{
				result = Encryption.char_0[(int)byte_0];
			}
			else
			{
				result = ' ';
			}
			return result;
		}

		private static byte smethod_1(char char_1)
		{
			byte result;
			if (char_1 == '=')
			{
				result = 0;
			}
			else
			{
				for (int i = 0; i < 64; i++)
				{
					if (Encryption.char_0[i] == char_1)
					{
						result = (byte)i;
						return result;
					}
				}
				result = 0;
			}
			return result;
		}
	}
}
