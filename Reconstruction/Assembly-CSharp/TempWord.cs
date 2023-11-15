using System;

// Token: 0x02000113 RID: 275
public struct TempWord
{
	// Token: 0x060006C3 RID: 1731 RVA: 0x000128AB File Offset: 0x00010AAB
	public TempWord(TempWordType type, int id)
	{
		this.WordType = type;
		this.ID = id;
	}

	// Token: 0x04000334 RID: 820
	public TempWordType WordType;

	// Token: 0x04000335 RID: 821
	public int ID;
}
