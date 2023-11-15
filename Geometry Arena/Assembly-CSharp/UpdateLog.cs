using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
[CreateAssetMenu(fileName = "UpdateLog", menuName = "CreateAsset/UpdateLog")]
public class UpdateLog : ScriptableObject
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000091 RID: 145 RVA: 0x000049C7 File Offset: 0x00002BC7
	public static UpdateLog Inst
	{
		get
		{
			return LanguageText.Inst.asset_UpdateLog;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000092 RID: 146 RVA: 0x000049D3 File Offset: 0x00002BD3
	public string VersionName
	{
		get
		{
			return this.versions[this.versions.Length - 1].version;
		}
	}

	// Token: 0x040000E7 RID: 231
	public int noUse = 1;

	// Token: 0x040000E8 RID: 232
	public UpdateLog.Versions[] versions = new UpdateLog.Versions[0];

	// Token: 0x02000129 RID: 297
	[Serializable]
	public class Versions
	{
		// Token: 0x0400092A RID: 2346
		[SerializeField]
		public string version = "0.0.0";

		// Token: 0x0400092B RID: 2347
		public string updateDate = "2020.00.00";

		// Token: 0x0400092C RID: 2348
		public UpdateLog.SmallTitle[] titles = new UpdateLog.SmallTitle[0];
	}

	// Token: 0x0200012A RID: 298
	[Serializable]
	public class SmallTitle
	{
		// Token: 0x0400092D RID: 2349
		public string name = "小标题";

		// Token: 0x0400092E RID: 2350
		public string[] infos = new string[0];
	}
}
