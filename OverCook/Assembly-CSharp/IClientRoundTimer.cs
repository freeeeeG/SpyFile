using System;

// Token: 0x020006C1 RID: 1729
public interface IClientRoundTimer
{
	// Token: 0x1700028A RID: 650
	// (get) Token: 0x060020B5 RID: 8373
	bool IsSuppressed { get; }

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x060020B6 RID: 8374
	SuppressionController Suppressor { get; }

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x060020B7 RID: 8375
	float TimeElapsed { get; }

	// Token: 0x060020B8 RID: 8376
	void Initialise();

	// Token: 0x060020B9 RID: 8377
	void Update();

	// Token: 0x060020BA RID: 8378
	void Zero();
}
