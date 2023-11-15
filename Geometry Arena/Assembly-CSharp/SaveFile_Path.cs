using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
public static class SaveFile_Path
{
	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060003E8 RID: 1000 RVA: 0x000194EC File Offset: 0x000176EC
	public static string[] GetPathCurrentOS
	{
		get
		{
			return new string[]
			{
				Application.persistentDataPath + "//savedata.sav",
				Application.persistentDataPath + "//savedata",
				Application.persistentDataPath + "\\\\savedata",
				Application.persistentDataPath + "//test"
			};
		}
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x00019547 File Offset: 0x00017747
	public static string GetPathCurrenOS_Backup(int i)
	{
		return string.Concat(new object[]
		{
			Application.persistentDataPath,
			"//savedata",
			i,
			".sav"
		});
	}
}
