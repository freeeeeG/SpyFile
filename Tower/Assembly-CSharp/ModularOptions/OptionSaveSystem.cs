using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x020000AB RID: 171
	public static class OptionSaveSystem
	{
		// Token: 0x0600024F RID: 591 RVA: 0x000092BA File Offset: 0x000074BA
		public static void SaveFloat(string _key, float _value)
		{
			PlayerPrefs.SetFloat(_key, _value);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000092C3 File Offset: 0x000074C3
		public static void SaveInt(string _key, int _value)
		{
			PlayerPrefs.SetInt(_key, _value);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000092CC File Offset: 0x000074CC
		public static void SaveBool(string _key, bool _value)
		{
			PlayerPrefs.SetInt(_key, _value ? 1 : 0);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000092DB File Offset: 0x000074DB
		public static float LoadFloat(string _key, float _defaultValue)
		{
			return PlayerPrefs.GetFloat(_key, _defaultValue);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x000092E4 File Offset: 0x000074E4
		public static int LoadInt(string _key, int _defaultValue)
		{
			return PlayerPrefs.GetInt(_key, _defaultValue);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000092ED File Offset: 0x000074ED
		public static bool LoadBool(string _key, bool _defaultValue)
		{
			if (PlayerPrefs.HasKey(_key))
			{
				return PlayerPrefs.GetInt(_key) > 0;
			}
			return _defaultValue;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00009302 File Offset: 0x00007502
		public static void SaveToDisk()
		{
			PlayerPrefs.Save();
		}
	}
}
