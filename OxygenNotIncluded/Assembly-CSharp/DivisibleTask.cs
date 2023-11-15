using System;

// Token: 0x020006F5 RID: 1781
internal abstract class DivisibleTask<SharedData> : IWorkItem<SharedData>
{
	// Token: 0x060030DD RID: 12509 RVA: 0x001031C8 File Offset: 0x001013C8
	public void Run(SharedData sharedData)
	{
		this.RunDivision(sharedData);
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x001031D1 File Offset: 0x001013D1
	protected DivisibleTask(string name)
	{
		this.name = name;
	}

	// Token: 0x060030DF RID: 12511
	protected abstract void RunDivision(SharedData sharedData);

	// Token: 0x04001D65 RID: 7525
	public string name;

	// Token: 0x04001D66 RID: 7526
	public int start;

	// Token: 0x04001D67 RID: 7527
	public int end;
}
