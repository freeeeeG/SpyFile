using System;

// Token: 0x02000022 RID: 34
[Serializable]
public class DifficultyOption
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000B41B File Offset: 0x0000961B
	public string Language_Name
	{
		get
		{
			return this.names[(int)Setting.Inst.language];
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000B42E File Offset: 0x0000962E
	public string Language_Info
	{
		get
		{
			return this.infos[(int)Setting.Inst.language];
		}
	}

	// Token: 0x040001A5 RID: 421
	public string dataName = "UNINITED";

	// Token: 0x040001A6 RID: 422
	public string[] names = new string[3];

	// Token: 0x040001A7 RID: 423
	public string[] infos = new string[3];

	// Token: 0x040001A8 RID: 424
	public int id = -1;

	// Token: 0x040001A9 RID: 425
	public DifficultyOption.Fac[] facs = new DifficultyOption.Fac[7];

	// Token: 0x040001AA RID: 426
	public float[] dailyChances = new float[1];

	// Token: 0x02000142 RID: 322
	[Serializable]
	public class Fac
	{
		// Token: 0x0400098C RID: 2444
		public int type = -1;

		// Token: 0x0400098D RID: 2445
		public float num = -1f;
	}
}
