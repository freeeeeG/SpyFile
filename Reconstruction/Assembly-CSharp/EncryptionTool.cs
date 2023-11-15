using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Token: 0x020002A6 RID: 678
public class EncryptionTool
{
	// Token: 0x060010B0 RID: 4272 RVA: 0x0002E3AC File Offset: 0x0002C5AC
	public static string EncryptString(string str, string key)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] key2 = md5CryptoServiceProvider.ComputeHash(utf8Encoding.GetBytes(key));
		TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
		{
			Key = key2,
			Mode = CipherMode.ECB,
			Padding = PaddingMode.PKCS7
		};
		byte[] bytes = utf8Encoding.GetBytes(str);
		byte[] inArray;
		try
		{
			inArray = tripleDESCryptoServiceProvider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
		}
		finally
		{
			tripleDESCryptoServiceProvider.Clear();
			md5CryptoServiceProvider.Clear();
		}
		return Convert.ToBase64String(inArray);
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x0002E434 File Offset: 0x0002C634
	public static string DecryptString(string str, string key)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] key2 = md5CryptoServiceProvider.ComputeHash(utf8Encoding.GetBytes(key));
		TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
		{
			Key = key2,
			Mode = CipherMode.ECB,
			Padding = PaddingMode.PKCS7
		};
		byte[] bytes;
		try
		{
			byte[] array = Convert.FromBase64String(str);
			bytes = tripleDESCryptoServiceProvider.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
		}
		catch (Exception ex)
		{
			Debug.LogError("DecryptString failed. return empty string." + ex.Message);
			return "";
		}
		finally
		{
			tripleDESCryptoServiceProvider.Clear();
			md5CryptoServiceProvider.Clear();
		}
		return utf8Encoding.GetString(bytes);
	}
}
