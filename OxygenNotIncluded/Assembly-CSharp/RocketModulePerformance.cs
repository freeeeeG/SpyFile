using System;

// Token: 0x020009B7 RID: 2487
[Serializable]
public class RocketModulePerformance
{
	// Token: 0x06004A1B RID: 18971 RVA: 0x001A176F File Offset: 0x0019F96F
	public RocketModulePerformance(float burden, float fuelKilogramPerDistance, float enginePower)
	{
		this.burden = burden;
		this.fuelKilogramPerDistance = fuelKilogramPerDistance;
		this.enginePower = enginePower;
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06004A1C RID: 18972 RVA: 0x001A178C File Offset: 0x0019F98C
	public float Burden
	{
		get
		{
			return this.burden;
		}
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x06004A1D RID: 18973 RVA: 0x001A1794 File Offset: 0x0019F994
	public float FuelKilogramPerDistance
	{
		get
		{
			return this.fuelKilogramPerDistance;
		}
	}

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x06004A1E RID: 18974 RVA: 0x001A179C File Offset: 0x0019F99C
	public float EnginePower
	{
		get
		{
			return this.enginePower;
		}
	}

	// Token: 0x040030C4 RID: 12484
	public float burden;

	// Token: 0x040030C5 RID: 12485
	public float fuelKilogramPerDistance;

	// Token: 0x040030C6 RID: 12486
	public float enginePower;
}
