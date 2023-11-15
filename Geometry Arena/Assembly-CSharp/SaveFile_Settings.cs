using System;
using System.IO;
using LitJson;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class SaveFile_Settings
{
	// Token: 0x060003F0 RID: 1008 RVA: 0x0001975C File Offset: 0x0001795C
	public static SaveFile_Settings NewSaveFileSettings_ReadFromSettings(Setting setting)
	{
		return new SaveFile_Settings
		{
			language = setting.language,
			resolutionIndex = setting.resolutionIndex,
			setBools = setting.setBools,
			setFloats = setting.setFloats,
			setInts = setting.setInts
		};
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x000197AC File Offset: 0x000179AC
	public static void SaveByJson_Settings()
	{
		string value = JsonMapper.ToJson(SaveFile_Settings.NewSaveFileSettings_ReadFromSettings(Setting.Inst));
		StreamWriter streamWriter = new StreamWriter(SaveFile_Settings.GetPathCurrentOS());
		streamWriter.Write(value);
		streamWriter.Close();
		Debug.Log("设置保存成功");
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x000197EC File Offset: 0x000179EC
	public static Setting ReadByJson_Settings()
	{
		string pathCurrentOS = SaveFile_Settings.GetPathCurrentOS();
		if (!File.Exists(pathCurrentOS))
		{
			Debug.LogError("暂时没有设置信息！");
			Setting setting = new Setting();
			setting.DefaultSetting();
			return setting;
		}
		Setting result;
		try
		{
			StreamReader streamReader = new StreamReader(pathCurrentOS);
			string json = streamReader.ReadToEnd();
			streamReader.Close();
			SaveFile_Settings saveFile_Settings = JsonMapper.ToObject<SaveFile_Settings>(json);
			Setting setting2 = new Setting();
			setting2.DefaultSetting();
			setting2.resolutionIndex = saveFile_Settings.resolutionIndex;
			for (int i = 0; i < Mathf.Min(setting2.setBools.Length, saveFile_Settings.setBools.Length); i++)
			{
				setting2.setBools[i] = saveFile_Settings.setBools[i];
			}
			for (int j = 0; j < Mathf.Min(setting2.setFloats.Length, saveFile_Settings.setFloats.Length); j++)
			{
				setting2.setFloats[j] = saveFile_Settings.setFloats[j];
			}
			if (saveFile_Settings.setInts != null)
			{
				for (int k = 0; k < Mathf.Min(setting2.setInts.Length, saveFile_Settings.setInts.Length); k++)
				{
					setting2.setInts[k] = saveFile_Settings.setInts[k];
				}
			}
			setting2.language = saveFile_Settings.language;
			Debug.Log("读取设置成功");
			result = setting2;
		}
		catch (Exception arg)
		{
			Debug.LogError("异常，读取设置失败" + arg);
			Setting setting3 = new Setting();
			setting3.DefaultSetting();
			result = setting3;
		}
		return result;
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x00019948 File Offset: 0x00017B48
	public static string GetPathCurrentOS()
	{
		return Application.persistentDataPath + "//Settings";
	}

	// Token: 0x04000387 RID: 903
	public EnumLanguage language = EnumLanguage.CHINESE_SIM;

	// Token: 0x04000388 RID: 904
	public int resolutionIndex = 3;

	// Token: 0x04000389 RID: 905
	public bool[] setBools;

	// Token: 0x0400038A RID: 906
	public double[] setFloats;

	// Token: 0x0400038B RID: 907
	public int[] setInts;
}
