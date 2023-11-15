using System;

// Token: 0x0200001C RID: 28
[Serializable]
public class BuffInfo
{
	// Token: 0x06000088 RID: 136 RVA: 0x00004887 File Offset: 0x00002A87
	public BuffInfo(EnemyBuffName name, int stacks, bool isAbormal = false)
	{
		this.EnemyBuffName = name;
		this.Stacks = stacks;
		this.IsAbnormal = isAbormal;
	}

	// Token: 0x0400008F RID: 143
	public EnemyBuffName EnemyBuffName;

	// Token: 0x04000090 RID: 144
	public int Stacks;

	// Token: 0x04000091 RID: 145
	public bool IsAbnormal;
}
