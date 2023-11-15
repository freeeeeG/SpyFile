using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
[Serializable]
public class BattleClipSet
{
	// Token: 0x040000FE RID: 254
	[Range(0f, 1f)]
	public float pct_AttackMain = 0.33f;

	// Token: 0x040000FF RID: 255
	public AudioClip[] clips = new AudioClip[0];

	// Token: 0x04000100 RID: 256
	public float[] volumes = new float[0];

	// Token: 0x04000101 RID: 257
	public int index_Prepare1_Enter;

	// Token: 0x04000102 RID: 258
	public int index_Prepare2_Normal = 1;

	// Token: 0x04000103 RID: 259
	public int index_Attack1_Enter = 2;

	// Token: 0x04000104 RID: 260
	public int index_Attack2_AfterEnter = 3;

	// Token: 0x04000105 RID: 261
	public int index_Attack3_Main = 5;

	// Token: 0x04000106 RID: 262
	public int index_Attack4_AfterBoss = 5;

	// Token: 0x04000107 RID: 263
	public int index_Attack5_End = 3;

	// Token: 0x04000108 RID: 264
	public int index_AttackSpecial_GameOver = 3;

	// Token: 0x04000109 RID: 265
	public int[] indexs_Boss_Normal = new int[]
	{
		6,
		7
	};

	// Token: 0x0400010A RID: 266
	public int[] indexs_Boss_Super = new int[]
	{
		8,
		9
	};
}
