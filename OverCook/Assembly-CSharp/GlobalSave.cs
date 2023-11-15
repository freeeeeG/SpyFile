using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x0200070B RID: 1803
public class GlobalSave : IByteSerialization
{
	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06002250 RID: 8784 RVA: 0x000A5B6C File Offset: 0x000A3F6C
	public int ByteSaveSize
	{
		get
		{
			string s = this.ConvertDataToSave();
			return Encoding.UTF8.GetByteCount(s);
		}
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x000A5B8C File Offset: 0x000A3F8C
	public byte[] ByteSave()
	{
		string text = this.ConvertDataToSave();
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] array = this.Obfuscate(bytes, bytes.Length, 0, "jjo+Ffqil5bdpo5VG82kLj8Ng1sK7L/rCqFTa39Zkom2/baqf5j9HMmsuCr0ipjYsPrsaNIOESWy7bDDGYWx1eA==", "SHA1", 256);
		if (array == null)
		{
			return null;
		}
		byte[] bytes2 = BitConverter.GetBytes(CRC32.Calculate(array));
		byte[] array2 = new byte[array.Length + bytes2.Length];
		Array.Copy(array, array2, array.Length);
		Array.Copy(bytes2, 0, array2, array.Length, bytes2.Length);
		return array2;
	}

	// Token: 0x06002252 RID: 8786 RVA: 0x000A5C14 File Offset: 0x000A4014
	private string GetUniqueId()
	{
		string text = SteamUser.GetSteamID().ToString();
		return (text == null) ? SystemInfo.deviceUniqueIdentifier : text;
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x000A5C48 File Offset: 0x000A4048
	private byte[] Obfuscate(byte[] deobfuscatedText, int size, int start = 0, string salt = "jjo+Ffqil5bdpo5VG82kLj8Ng1sK7L/rCqFTa39Zkom2/baqf5j9HMmsuCr0ipjYsPrsaNIOESWy7bDDGYWx1eA==", string hashFunction = "SHA1", int keySize = 256)
	{
		if (deobfuscatedText == null || deobfuscatedText.Length == 0 || start + size > deobfuscatedText.Length)
		{
			return null;
		}
		byte[] array = new byte[16];
		System.Random random = new System.Random();
		random.NextBytes(array);
		byte[] bytes = new PasswordDeriveBytes(this.GetUniqueId(), Encoding.ASCII.GetBytes(salt), hashFunction, 2).GetBytes(keySize / 8);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = CipherMode.CBC;
		byte[] array2 = null;
		try
		{
			using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(bytes, array))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
					{
						cryptoStream.Write(deobfuscatedText, start, size);
						cryptoStream.FlushFinalBlock();
						array2 = memoryStream.ToArray();
						memoryStream.Close();
						cryptoStream.Close();
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("GlobalSave Obfuscate exception=" + ex.ToString());
			return null;
		}
		finally
		{
			rijndaelManaged.Clear();
		}
		byte[] array3 = new byte[16 + array2.Length];
		Array.Copy(array, array3, 16);
		Array.Copy(array2, 0, array3, 16, array2.Length);
		return array3;
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000A5DD4 File Offset: 0x000A41D4
	private byte[] Deobfuscate(byte[] obfuscatedText, int size, int start = 0, string salt = "jjo+Ffqil5bdpo5VG82kLj8Ng1sK7L/rCqFTa39Zkom2/baqf5j9HMmsuCr0ipjYsPrsaNIOESWy7bDDGYWx1eA==", string hashFunction = "SHA1", int keySize = 256)
	{
		if (obfuscatedText == null || obfuscatedText.Length == 0 || obfuscatedText.Length <= start + size || obfuscatedText.Length <= 16)
		{
			return null;
		}
		byte[] array = new byte[16];
		Array.Copy(obfuscatedText, start, array, 0, 16);
		byte[] array2 = new byte[size - 16 - start];
		Array.Copy(obfuscatedText, 16, array2, 0, array2.Length);
		byte[] bytes = new PasswordDeriveBytes(this.GetUniqueId(), Encoding.ASCII.GetBytes(salt), hashFunction, 2).GetBytes(keySize / 8);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = CipherMode.CBC;
		byte[] array3 = new byte[array2.Length];
		try
		{
			using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(bytes, array))
			{
				using (MemoryStream memoryStream = new MemoryStream(array2))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
					{
						cryptoStream.Read(array3, 0, array3.Length);
						memoryStream.Close();
						cryptoStream.Close();
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("GlobalSave Deobfuscate exception=" + ex.ToString());
			return null;
		}
		finally
		{
			rijndaelManaged.Clear();
		}
		return array3;
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x000A5F58 File Offset: 0x000A4358
	public bool ByteLoad(byte[] _data)
	{
		if (_data == null || (long)_data.Length <= 4L)
		{
			return false;
		}
		int size = _data.Length - 4;
		if (!CRC32.Validate(_data, (uint)size))
		{
			return false;
		}
		byte[] array = this.Deobfuscate(_data, size, 0, "jjo+Ffqil5bdpo5VG82kLj8Ng1sK7L/rCqFTa39Zkom2/baqf5j9HMmsuCr0ipjYsPrsaNIOESWy7bDDGYWx1eA==", "SHA1", 256);
		if (array == null)
		{
			return false;
		}
		string @string = Encoding.UTF8.GetString(array);
		return this.ConvertDataFromSave(@string);
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x000A5FC8 File Offset: 0x000A43C8
	private string ConvertDataToSave()
	{
		int num = 0;
		this.m_GlobalData.m_Entries = new GlobalSave.Entry[this.m_Data.Keys.Count];
		this.m_GlobalData.m_Keys = new string[this.m_Data.Keys.Count];
		foreach (string text in this.m_Data.Keys)
		{
			this.m_GlobalData.m_Keys[num] = text;
			this.m_GlobalData.m_Entries[num] = this.m_Data[text];
			num++;
		}
		return JsonUtility.ToJson(this.m_GlobalData, false);
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x000A60A0 File Offset: 0x000A44A0
	private bool ConvertDataFromSave(string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			return false;
		}
		try
		{
			JsonUtility.FromJsonOverwrite(json, this.m_GlobalData);
		}
		catch
		{
			return false;
		}
		this.m_Data.Clear();
		int num = this.m_GlobalData.m_Keys.Length;
		for (int i = 0; i < num; i++)
		{
			this.m_Data[this.m_GlobalData.m_Keys[i]] = this.m_GlobalData.m_Entries[i];
		}
		return true;
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x000A6138 File Offset: 0x000A4538
	public void Set(string key, int value)
	{
		GlobalSave.Entry entry;
		this.FindOrCreateKey(key, out entry);
		GlobalSave.EntInt obj = new GlobalSave.EntInt(value);
		entry.m_JSON = JsonUtility.ToJson(obj);
	}

	// Token: 0x06002259 RID: 8793 RVA: 0x000A6164 File Offset: 0x000A4564
	public bool Get(string key, out int value, int def)
	{
		GlobalSave.Entry entry;
		this.FindKey(key, out entry);
		if (entry != null)
		{
			try
			{
				GlobalSave.EntInt entInt = JsonUtility.FromJson<GlobalSave.EntInt>(entry.m_JSON);
				value = entInt.m_Value;
				return true;
			}
			catch
			{
				value = def;
				return false;
			}
		}
		value = def;
		return false;
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x000A61BC File Offset: 0x000A45BC
	public void Set(string key, float value)
	{
		GlobalSave.Entry entry;
		this.FindOrCreateKey(key, out entry);
		GlobalSave.EntFloat obj = new GlobalSave.EntFloat(value);
		entry.m_JSON = JsonUtility.ToJson(obj);
	}

	// Token: 0x0600225B RID: 8795 RVA: 0x000A61E8 File Offset: 0x000A45E8
	public bool Get(string key, out float value, float def)
	{
		GlobalSave.Entry entry;
		this.FindKey(key, out entry);
		if (entry != null)
		{
			try
			{
				GlobalSave.EntFloat entFloat = JsonUtility.FromJson<GlobalSave.EntFloat>(entry.m_JSON);
				value = entFloat.m_Value;
				return true;
			}
			catch
			{
				value = def;
				return false;
			}
		}
		value = def;
		return false;
	}

	// Token: 0x0600225C RID: 8796 RVA: 0x000A6240 File Offset: 0x000A4640
	public void Set(string key, string value)
	{
		GlobalSave.Entry entry;
		this.FindOrCreateKey(key, out entry);
		GlobalSave.EntString obj = new GlobalSave.EntString(value);
		entry.m_JSON = JsonUtility.ToJson(obj);
	}

	// Token: 0x0600225D RID: 8797 RVA: 0x000A626C File Offset: 0x000A466C
	public bool Get(string key, out string value, string def)
	{
		GlobalSave.Entry entry;
		this.FindKey(key, out entry);
		if (entry != null)
		{
			try
			{
				GlobalSave.EntString entString = JsonUtility.FromJson<GlobalSave.EntString>(entry.m_JSON);
				value = entString.m_Value;
				return true;
			}
			catch
			{
				value = def;
				return false;
			}
		}
		value = def;
		return false;
	}

	// Token: 0x0600225E RID: 8798 RVA: 0x000A62C4 File Offset: 0x000A46C4
	public void Set(string key, bool value)
	{
		GlobalSave.Entry entry;
		this.FindOrCreateKey(key, out entry);
		GlobalSave.EntBool obj = new GlobalSave.EntBool(value);
		entry.m_JSON = JsonUtility.ToJson(obj);
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x000A62F0 File Offset: 0x000A46F0
	public bool Get(string key, out bool value, bool def)
	{
		GlobalSave.Entry entry;
		this.FindKey(key, out entry);
		if (entry != null)
		{
			try
			{
				GlobalSave.EntBool entBool = JsonUtility.FromJson<GlobalSave.EntBool>(entry.m_JSON);
				value = entBool.m_Value;
				return true;
			}
			catch
			{
				value = def;
				return false;
			}
		}
		value = def;
		return false;
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x000A6348 File Offset: 0x000A4748
	public void Set(string key, long[] value)
	{
		GlobalSave.Entry entry;
		this.FindOrCreateKey(key, out entry);
		GlobalSave.EntInt64Array obj = new GlobalSave.EntInt64Array(value);
		entry.m_JSON = JsonUtility.ToJson(obj);
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x000A6374 File Offset: 0x000A4774
	public bool Get(string key, out long[] value, long[] def)
	{
		GlobalSave.Entry entry;
		this.FindKey(key, out entry);
		if (entry != null)
		{
			try
			{
				GlobalSave.EntInt64Array entInt64Array = JsonUtility.FromJson<GlobalSave.EntInt64Array>(entry.m_JSON);
				value = entInt64Array.m_Value;
				return true;
			}
			catch
			{
				value = def;
				return false;
			}
		}
		value = def;
		return false;
	}

	// Token: 0x06002262 RID: 8802 RVA: 0x000A63CC File Offset: 0x000A47CC
	public void Set(string key, int[] value)
	{
		GlobalSave.Entry entry;
		this.FindOrCreateKey(key, out entry);
		GlobalSave.EntIntArray obj = new GlobalSave.EntIntArray(value);
		entry.m_JSON = JsonUtility.ToJson(obj);
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x000A63F8 File Offset: 0x000A47F8
	public bool Get(string key, out int[] value, int[] def)
	{
		GlobalSave.Entry entry;
		this.FindKey(key, out entry);
		if (entry != null)
		{
			try
			{
				GlobalSave.EntIntArray entIntArray = JsonUtility.FromJson<GlobalSave.EntIntArray>(entry.m_JSON);
				value = entIntArray.m_Value;
				return true;
			}
			catch
			{
				value = def;
				return false;
			}
		}
		value = def;
		return false;
	}

	// Token: 0x06002264 RID: 8804 RVA: 0x000A6450 File Offset: 0x000A4850
	public void Set(string key, Dictionary<string, string> value)
	{
		GlobalSave.Entry entry;
		this.FindOrCreateKey(key, out entry);
		GlobalSave.EntDictionary obj = new GlobalSave.EntDictionary(value);
		entry.m_JSON = JsonUtility.ToJson(obj);
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x000A647C File Offset: 0x000A487C
	public bool Get(string key, out Dictionary<string, string> value, Dictionary<string, string> def)
	{
		GlobalSave.Entry entry;
		this.FindKey(key, out entry);
		if (entry != null)
		{
			try
			{
				GlobalSave.EntDictionary entDictionary = JsonUtility.FromJson<GlobalSave.EntDictionary>(entry.m_JSON);
				value = entDictionary.ToDictionary();
				return true;
			}
			catch
			{
				value = def;
				return false;
			}
		}
		value = def;
		return false;
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x000A64D4 File Offset: 0x000A48D4
	private void FindKey(string key, out GlobalSave.Entry entry)
	{
		if (this.m_Data.ContainsKey(key))
		{
			entry = this.m_Data[key];
		}
		else
		{
			entry = null;
		}
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x000A64FD File Offset: 0x000A48FD
	private void FindOrCreateKey(string key, out GlobalSave.Entry entry)
	{
		if (!this.m_Data.ContainsKey(key))
		{
			this.m_Data[key] = new GlobalSave.Entry();
		}
		entry = this.m_Data[key];
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x000A652F File Offset: 0x000A492F
	public void ResetSave()
	{
		this.m_GlobalData = new GlobalSave.GlobalData();
		this.m_Data.Clear();
	}

	// Token: 0x04001A6C RID: 6764
	private const int GLOBALSAVE_VERSION = 1;

	// Token: 0x04001A6D RID: 6765
	private const string CRC_TAG = "CRC32";

	// Token: 0x04001A6E RID: 6766
	private const int IV_LENGTH = 16;

	// Token: 0x04001A6F RID: 6767
	private const string SALT_DEFAULT = "jjo+Ffqil5bdpo5VG82kLj8Ng1sK7L/rCqFTa39Zkom2/baqf5j9HMmsuCr0ipjYsPrsaNIOESWy7bDDGYWx1eA==";

	// Token: 0x04001A70 RID: 6768
	private Dictionary<string, GlobalSave.Entry> m_Data = new Dictionary<string, GlobalSave.Entry>();

	// Token: 0x04001A71 RID: 6769
	private GlobalSave.GlobalData m_GlobalData = new GlobalSave.GlobalData();

	// Token: 0x0200070C RID: 1804
	[Serializable]
	private class Entry
	{
		// Token: 0x04001A72 RID: 6770
		public string m_JSON;
	}

	// Token: 0x0200070D RID: 1805
	[Serializable]
	private class EntInt
	{
		// Token: 0x0600226A RID: 8810 RVA: 0x000A654F File Offset: 0x000A494F
		public EntInt(int val)
		{
			this.m_Value = val;
		}

		// Token: 0x04001A73 RID: 6771
		public int m_Value;
	}

	// Token: 0x0200070E RID: 1806
	[Serializable]
	private class EntFloat
	{
		// Token: 0x0600226B RID: 8811 RVA: 0x000A655E File Offset: 0x000A495E
		public EntFloat(float val)
		{
			this.m_Value = val;
		}

		// Token: 0x04001A74 RID: 6772
		public float m_Value;
	}

	// Token: 0x0200070F RID: 1807
	[Serializable]
	private class EntString
	{
		// Token: 0x0600226C RID: 8812 RVA: 0x000A656D File Offset: 0x000A496D
		public EntString(string val)
		{
			this.m_Value = val;
		}

		// Token: 0x04001A75 RID: 6773
		public string m_Value;
	}

	// Token: 0x02000710 RID: 1808
	[Serializable]
	private class EntBool
	{
		// Token: 0x0600226D RID: 8813 RVA: 0x000A657C File Offset: 0x000A497C
		public EntBool(bool val)
		{
			this.m_Value = val;
		}

		// Token: 0x04001A76 RID: 6774
		public bool m_Value;
	}

	// Token: 0x02000711 RID: 1809
	[Serializable]
	private class EntInt64Array
	{
		// Token: 0x0600226E RID: 8814 RVA: 0x000A658B File Offset: 0x000A498B
		public EntInt64Array(long[] val)
		{
			this.m_Value = val;
		}

		// Token: 0x04001A77 RID: 6775
		public long[] m_Value;
	}

	// Token: 0x02000712 RID: 1810
	private class EntIntArray
	{
		// Token: 0x0600226F RID: 8815 RVA: 0x000A659A File Offset: 0x000A499A
		public EntIntArray(int[] val)
		{
			this.m_Value = val;
		}

		// Token: 0x04001A78 RID: 6776
		public int[] m_Value;
	}

	// Token: 0x02000713 RID: 1811
	private class EntDictionary
	{
		// Token: 0x06002270 RID: 8816 RVA: 0x000A65AC File Offset: 0x000A49AC
		public EntDictionary(Dictionary<string, string> _dict)
		{
			int num = 0;
			this.m_Key = new string[_dict.Keys.Count];
			this.m_Value = new string[_dict.Keys.Count];
			foreach (string text in _dict.Keys)
			{
				this.m_Key[num] = text;
				this.m_Value[num] = _dict[text];
				num++;
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000A6654 File Offset: 0x000A4A54
		public Dictionary<string, string> ToDictionary()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			for (int i = 0; i < this.m_Key.Length; i++)
			{
				dictionary.Add(this.m_Key[i], this.m_Value[i]);
			}
			return dictionary;
		}

		// Token: 0x04001A79 RID: 6777
		public string[] m_Key;

		// Token: 0x04001A7A RID: 6778
		public string[] m_Value;
	}

	// Token: 0x02000714 RID: 1812
	[Serializable]
	private class GlobalData
	{
		// Token: 0x04001A7B RID: 6779
		public int m_Version = 1;

		// Token: 0x04001A7C RID: 6780
		public string m_What = "OC2GSGD";

		// Token: 0x04001A7D RID: 6781
		public string[] m_Keys;

		// Token: 0x04001A7E RID: 6782
		public GlobalSave.Entry[] m_Entries;
	}
}
