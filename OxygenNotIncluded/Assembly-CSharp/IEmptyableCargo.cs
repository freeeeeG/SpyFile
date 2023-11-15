using System;

// Token: 0x02000C2E RID: 3118
public interface IEmptyableCargo
{
	// Token: 0x060062A6 RID: 25254
	bool CanEmptyCargo();

	// Token: 0x060062A7 RID: 25255
	void EmptyCargo();

	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x060062A8 RID: 25256
	IStateMachineTarget master { get; }

	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x060062A9 RID: 25257
	bool CanAutoDeploy { get; }

	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x060062AA RID: 25258
	// (set) Token: 0x060062AB RID: 25259
	bool AutoDeploy { get; set; }

	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x060062AC RID: 25260
	bool ChooseDuplicant { get; }

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x060062AD RID: 25261
	bool ModuleDeployed { get; }

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x060062AE RID: 25262
	// (set) Token: 0x060062AF RID: 25263
	MinionIdentity ChosenDuplicant { get; set; }
}
