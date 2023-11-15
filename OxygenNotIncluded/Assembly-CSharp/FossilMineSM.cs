using System;

// Token: 0x02000604 RID: 1540
public class FossilMineSM : ComplexFabricatorSM
{
	// Token: 0x060026A7 RID: 9895 RVA: 0x000D2004 File Offset: 0x000D0204
	protected override void OnSpawn()
	{
	}

	// Token: 0x060026A8 RID: 9896 RVA: 0x000D2006 File Offset: 0x000D0206
	public void Activate()
	{
		base.smi.StartSM();
	}

	// Token: 0x060026A9 RID: 9897 RVA: 0x000D2013 File Offset: 0x000D0213
	public void Deactivate()
	{
		base.smi.StopSM("FossilMine.Deactivated");
	}
}
