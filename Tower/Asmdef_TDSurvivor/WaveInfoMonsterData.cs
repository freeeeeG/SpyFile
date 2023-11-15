using System;

// Token: 0x02000030 RID: 48
public class WaveInfoMonsterData
{
	// Token: 0x060000F3 RID: 243 RVA: 0x00004F2F File Offset: 0x0000312F
	public WaveInfoMonsterData(eMonsterType type, int count)
	{
		this.type = type;
		this.count = count;
	}

	// Token: 0x040000AC RID: 172
	public eMonsterType type;

	// Token: 0x040000AD RID: 173
	public int count;
}
