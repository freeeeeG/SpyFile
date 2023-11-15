using System;
using UnityEngine;

// Token: 0x02000470 RID: 1136
[Serializable]
public class AudioSheet
{
	// Token: 0x04000DB9 RID: 3513
	public TextAsset asset;

	// Token: 0x04000DBA RID: 3514
	public string defaultType;

	// Token: 0x04000DBB RID: 3515
	public AudioSheet.SoundInfo[] soundInfos;

	// Token: 0x020010EC RID: 4332
	public class SoundInfo : Resource
	{
		// Token: 0x04005A99 RID: 23193
		public string File;

		// Token: 0x04005A9A RID: 23194
		public string Anim;

		// Token: 0x04005A9B RID: 23195
		public string Type;

		// Token: 0x04005A9C RID: 23196
		public string RequiredDlcId;

		// Token: 0x04005A9D RID: 23197
		public float MinInterval;

		// Token: 0x04005A9E RID: 23198
		public string Name0;

		// Token: 0x04005A9F RID: 23199
		public int Frame0;

		// Token: 0x04005AA0 RID: 23200
		public string Name1;

		// Token: 0x04005AA1 RID: 23201
		public int Frame1;

		// Token: 0x04005AA2 RID: 23202
		public string Name2;

		// Token: 0x04005AA3 RID: 23203
		public int Frame2;

		// Token: 0x04005AA4 RID: 23204
		public string Name3;

		// Token: 0x04005AA5 RID: 23205
		public int Frame3;

		// Token: 0x04005AA6 RID: 23206
		public string Name4;

		// Token: 0x04005AA7 RID: 23207
		public int Frame4;

		// Token: 0x04005AA8 RID: 23208
		public string Name5;

		// Token: 0x04005AA9 RID: 23209
		public int Frame5;

		// Token: 0x04005AAA RID: 23210
		public string Name6;

		// Token: 0x04005AAB RID: 23211
		public int Frame6;

		// Token: 0x04005AAC RID: 23212
		public string Name7;

		// Token: 0x04005AAD RID: 23213
		public int Frame7;

		// Token: 0x04005AAE RID: 23214
		public string Name8;

		// Token: 0x04005AAF RID: 23215
		public int Frame8;

		// Token: 0x04005AB0 RID: 23216
		public string Name9;

		// Token: 0x04005AB1 RID: 23217
		public int Frame9;

		// Token: 0x04005AB2 RID: 23218
		public string Name10;

		// Token: 0x04005AB3 RID: 23219
		public int Frame10;

		// Token: 0x04005AB4 RID: 23220
		public string Name11;

		// Token: 0x04005AB5 RID: 23221
		public int Frame11;
	}
}
