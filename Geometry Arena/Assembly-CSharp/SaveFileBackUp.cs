using System;
using System.IO;
using LitJson;
using UnityEngine;

// Token: 0x02000069 RID: 105
[Serializable]
public class SaveFileBackUp
{
	// Token: 0x060003EC RID: 1004 RVA: 0x00019594 File Offset: 0x00017794
	private static void SaveIndex(int indexOld, int indexNew)
	{
		SaveFileBackUp saveFileBackUp = new SaveFileBackUp();
		saveFileBackUp.indexOld = indexOld;
		saveFileBackUp.indexNew = indexNew;
		string path = Application.persistentDataPath + "//BackUpIndex.sav";
		string value = JsonMapper.ToJson(saveFileBackUp);
		StreamWriter streamWriter = new StreamWriter(path);
		streamWriter.Write(value);
		streamWriter.Close();
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x000195DC File Offset: 0x000177DC
	private static SaveFileBackUp LoadIndex()
	{
		SaveFileBackUp result;
		try
		{
			string path = Application.persistentDataPath + "//BackUpIndex.sav";
			if (!File.Exists(path))
			{
				result = new SaveFileBackUp();
			}
			else
			{
				StreamReader streamReader = new StreamReader(path);
				string json = streamReader.ReadToEnd();
				streamReader.Close();
				result = JsonMapper.ToObject<SaveFileBackUp>(json);
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Warning_存档备份读取失败:" + ex.ToString());
			result = new SaveFileBackUp();
		}
		return result;
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x00019654 File Offset: 0x00017854
	public static void BackUp()
	{
		SaveFileBackUp saveFileBackUp = SaveFileBackUp.LoadIndex();
		if (saveFileBackUp == null)
		{
			Debug.LogWarning("Warning_备份Index文件为空！");
			saveFileBackUp = new SaveFileBackUp();
		}
		int num = saveFileBackUp.indexOld;
		int num2 = saveFileBackUp.indexNew;
		num2++;
		string text = string.Concat(new object[]
		{
			Application.persistentDataPath,
			"//savedataBackUp",
			num2,
			".sav"
		});
		string text2 = Application.persistentDataPath + "//savedata.sav";
		if (!File.Exists(text2))
		{
			Debug.LogError("Error_没有存档文件，无法备份！");
			return;
		}
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		File.Copy(text2, text);
		if (num < num2 - 10)
		{
			num++;
			string path = string.Concat(new object[]
			{
				Application.persistentDataPath,
				"//savedataBackUp",
				num,
				".sav"
			});
			if (!File.Exists(path))
			{
				return;
			}
			File.Delete(path);
		}
		SaveFileBackUp.SaveIndex(num, num2);
	}

	// Token: 0x04000385 RID: 901
	public int indexOld = -1;

	// Token: 0x04000386 RID: 902
	public int indexNew = -1;
}
