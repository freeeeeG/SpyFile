using System;

// Token: 0x020006C0 RID: 1728
public interface IServerRoundTimer
{
	// Token: 0x17000287 RID: 647
	// (get) Token: 0x060020AF RID: 8367
	bool IsSuppressed { get; }

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x060020B0 RID: 8368
	SuppressionController Suppressor { get; }

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x060020B1 RID: 8369
	float TimeElapsed { get; }

	// Token: 0x060020B2 RID: 8370
	void Initialise();

	// Token: 0x060020B3 RID: 8371
	bool TimeExpired();

	// Token: 0x060020B4 RID: 8372
	void Update();
}
