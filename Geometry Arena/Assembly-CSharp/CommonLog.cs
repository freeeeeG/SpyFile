using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
[CreateAssetMenu(fileName = "NewCommomLog", menuName = "CreateAsset/CommonLog")]
public class CommonLog : ScriptableObject
{
	// Token: 0x04000001 RID: 1
	public string title = "Title";

	// Token: 0x04000002 RID: 2
	public CommonLog.SmallTitle[] smallTitles = new CommonLog.SmallTitle[0];

	// Token: 0x020000FC RID: 252
	[Serializable]
	public class SmallTitle
	{
		// Token: 0x040007A2 RID: 1954
		public string name = "小标题";

		// Token: 0x040007A3 RID: 1955
		public string[] infos = new string[0];

		// Token: 0x040007A4 RID: 1956
		public string[] specialTexts = new string[0];
	}
}
