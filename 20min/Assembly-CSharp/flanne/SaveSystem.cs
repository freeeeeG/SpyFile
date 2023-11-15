using System;
using System.IO;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000109 RID: 265
	public static class SaveSystem
	{
		// Token: 0x06000785 RID: 1925 RVA: 0x00020B48 File Offset: 0x0001ED48
		public static void Load()
		{
			string path = Application.persistentDataPath + "/gamedata.json";
			if (File.Exists(path))
			{
				string json = File.ReadAllText(path);
				try
				{
					SaveSystem.data = JsonUtility.FromJson<SaveData>(json);
					return;
				}
				catch (Exception)
				{
					string path2 = Application.persistentDataPath + "/gamedata_backup.json";
					if (File.Exists(path2))
					{
						string json2 = File.ReadAllText(path2);
						try
						{
							SaveSystem.data = JsonUtility.FromJson<SaveData>(json2);
							goto IL_70;
						}
						catch (Exception)
						{
							SaveSystem.data = new SaveData();
							goto IL_70;
						}
					}
					SaveSystem.data = new SaveData();
					IL_70:
					return;
				}
			}
			SaveSystem.data = new SaveData();
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00020BF0 File Offset: 0x0001EDF0
		public static void Save()
		{
			string contents = JsonUtility.ToJson(SaveSystem.data);
			File.WriteAllText(Application.persistentDataPath + "/gamedata.json", contents);
			File.WriteAllText(Application.persistentDataPath + "/gamedata_backup.json", contents);
		}

		// Token: 0x04000564 RID: 1380
		public static SaveData data;
	}
}
