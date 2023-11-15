using System;

// Token: 0x0200078C RID: 1932
public interface IEnergyConsumer : ICircuitConnected
{
	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x060035A3 RID: 13731
	float WattsUsed { get; }

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x060035A4 RID: 13732
	float WattsNeededWhenActive { get; }

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x060035A5 RID: 13733
	int PowerSortOrder { get; }

	// Token: 0x060035A6 RID: 13734
	void SetConnectionStatus(CircuitManager.ConnectionStatus status);

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x060035A7 RID: 13735
	string Name { get; }

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x060035A8 RID: 13736
	bool IsConnected { get; }

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x060035A9 RID: 13737
	bool IsPowered { get; }
}
