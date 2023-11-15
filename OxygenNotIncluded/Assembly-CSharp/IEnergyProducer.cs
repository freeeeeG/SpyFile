using System;

// Token: 0x020007DB RID: 2011
public interface IEnergyProducer
{
	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x060038B0 RID: 14512
	float JoulesAvailable { get; }

	// Token: 0x060038B1 RID: 14513
	void ConsumeEnergy(float joules);
}
