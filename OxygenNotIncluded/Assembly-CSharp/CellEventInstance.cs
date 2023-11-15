using System;
using KSerialization;

// Token: 0x020007AC RID: 1964
[SerializationConfig(MemberSerialization.OptIn)]
public class CellEventInstance : EventInstanceBase, ISaveLoadable
{
	// Token: 0x06003689 RID: 13961 RVA: 0x0012648E File Offset: 0x0012468E
	public CellEventInstance(int cell, int data, int data2, CellEvent ev) : base(ev)
	{
		this.cell = cell;
		this.data = data;
		this.data2 = data2;
	}

	// Token: 0x04002143 RID: 8515
	[Serialize]
	public int cell;

	// Token: 0x04002144 RID: 8516
	[Serialize]
	public int data;

	// Token: 0x04002145 RID: 8517
	[Serialize]
	public int data2;
}
