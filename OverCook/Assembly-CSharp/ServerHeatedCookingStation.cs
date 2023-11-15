using System;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
public class ServerHeatedCookingStation : ServerCookingStation
{
	// Token: 0x06001634 RID: 5684 RVA: 0x000761E8 File Offset: 0x000745E8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_heatedCookingStation = (HeatedCookingStation)synchronisedObject;
		if (this.m_heatedCookingStation.m_heatSource != null)
		{
			this.m_heatedStation = this.m_heatedCookingStation.m_heatSource.gameObject.RequireComponent<ServerHeatedStation>();
		}
		else
		{
			this.m_heatedStation = base.gameObject.RequireComponent<ServerHeatedStation>();
		}
		this.m_heatedStation.RegisterHeatRangeChangedCallback(new GenericVoid<HeatRange>(this.OnHeatRangeChanged));
		this.OnHeatRangeChanged(HeatRange.Low);
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x0007626D File Offset: 0x0007466D
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_heatedStation != null)
		{
			this.m_heatedStation.UnregisterHeatRangeChangedCallback(new GenericVoid<HeatRange>(this.OnHeatRangeChanged));
		}
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x000762A0 File Offset: 0x000746A0
	private void OnHeatRangeChanged(HeatRange _range)
	{
		if (_range != HeatRange.High)
		{
			if (_range != HeatRange.Moderate)
			{
				if (_range == HeatRange.Low)
				{
					this.m_cookingSpeed = this.m_heatedCookingStation.m_cookingSpeedLow;
				}
			}
			else
			{
				this.m_cookingSpeed = this.m_heatedCookingStation.m_cookingSpeedModerate;
			}
		}
		else
		{
			this.m_cookingSpeed = this.m_heatedCookingStation.m_cookingSpeedHigh;
		}
	}

	// Token: 0x040010CB RID: 4299
	private HeatedCookingStation m_heatedCookingStation;

	// Token: 0x040010CC RID: 4300
	private ServerHeatedStation m_heatedStation;
}
