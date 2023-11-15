using System;
using System.Security.Cryptography;
using System.Text;

// Token: 0x02000070 RID: 112
public static class MyEncrypt
{
	// Token: 0x06000434 RID: 1076 RVA: 0x0001A5AC File Offset: 0x000187AC
	public static string Encrypt(string content)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(MyEncrypt.key);
		ICryptoTransform cryptoTransform = new RijndaelManaged
		{
			Key = bytes,
			Mode = CipherMode.ECB,
			Padding = PaddingMode.PKCS7
		}.CreateEncryptor();
		byte[] bytes2 = Encoding.UTF8.GetBytes(content);
		byte[] array = cryptoTransform.TransformFinalBlock(bytes2, 0, bytes2.Length);
		return Convert.ToBase64String(array, 0, array.Length);
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x0001A60C File Offset: 0x0001880C
	public static string Decipher(string content)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(MyEncrypt.key);
		ICryptoTransform cryptoTransform = new RijndaelManaged
		{
			Key = bytes,
			Mode = CipherMode.ECB,
			Padding = PaddingMode.PKCS7
		}.CreateDecryptor();
		byte[] array = Convert.FromBase64String(content);
		byte[] bytes2 = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
		return Encoding.UTF8.GetString(bytes2);
	}

	// Token: 0x04000392 RID: 914
	private static string key = "08060811013731401016042185144900";
}
