using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005F5 RID: 1525
public class ServerBellowsSpray : ServerSprayingUtensil, IWindSource, IHeatTransferBehaviour
{
	// Token: 0x06001D05 RID: 7429 RVA: 0x0008EDBC File Offset: 0x0008D1BC
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_bellowsSpray = (BellowsSpray)synchronisedObject;
		this.m_interactable = base.gameObject.RequestComponent<ServerUsableItem>();
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x0008EDE4 File Offset: 0x0008D1E4
	protected override void StartSpray()
	{
		base.StartSpray();
		List<ServerHeatedStation> allHeatedStations = ServerHeatedStation.GetAllHeatedStations();
		for (int i = 0; i < allHeatedStations.Count; i++)
		{
			ServerHeatedStation serverHeatedStation = allHeatedStations[i];
			if (base.IsInSpray(serverHeatedStation.transform))
			{
				ServerCookingStation serverCookingStation = serverHeatedStation.gameObject.RequestComponent<ServerCookingStation>();
				if (serverCookingStation != null && serverCookingStation.StationType == CookingStationType.Barbeque && base.Carrier != null)
				{
					ServerMessenger.Achievement(base.Carrier, 102, 1);
				}
				allHeatedStations[i].ExternalHeatTransfer(this);
			}
		}
		this.m_knockbackTimer = this.m_bellowsSpray.m_knockback.Duration;
		if (this.m_interactable != null)
		{
			this.m_interactable.SetInteractionSuppressed(true);
		}
	}

	// Token: 0x06001D07 RID: 7431 RVA: 0x0008EEB4 File Offset: 0x0008D2B4
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_knockbackTimer > 0f)
		{
			this.m_knockbackTimer -= TimeManager.GetDeltaTime(base.gameObject);
			if (this.m_knockbackTimer <= 0f)
			{
				this.StopSpray();
				if (this.m_interactable != null)
				{
					this.m_interactable.SetInteractionSuppressed(false);
				}
			}
		}
		if (base.IsSpraying())
		{
			List<WindAccumulator> allWindReceivers = WindAccumulator.GetAllWindReceivers();
			for (int i = 0; i < allWindReceivers.Count; i++)
			{
				WindAccumulator windAccumulator = allWindReceivers[i];
				int num = this.m_activeWindReceivers.IndexOf(windAccumulator);
				bool flag = base.IsInSpray(windAccumulator.transform);
				bool flag2 = num >= 0;
				if (!flag2 && flag)
				{
					this.m_activeWindReceivers.Add(windAccumulator);
					allWindReceivers[i].AddWindSource(this);
				}
				else if (flag2 && !flag)
				{
					this.m_activeWindReceivers.RemoveAt(num);
					allWindReceivers[i].RemoveWindSource(this);
				}
			}
		}
		else if (this.m_activeWindReceivers.Count > 0)
		{
			for (int j = 0; j < this.m_activeWindReceivers.Count; j++)
			{
				this.m_activeWindReceivers[j].RemoveWindSource(this);
			}
			this.m_activeWindReceivers.Clear();
		}
	}

	// Token: 0x06001D08 RID: 7432 RVA: 0x0008F01D File Offset: 0x0008D41D
	public Vector3 GetVelocity()
	{
		return base.transform.forward * this.m_bellowsSpray.m_knockback.Force;
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x0008F03F File Offset: 0x0008D43F
	public bool CanTransferToContainer(IHeatContainer _container)
	{
		return true;
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x0008F042 File Offset: 0x0008D442
	public void TransferToContainer(ICarrierPlacement _carrier, IHeatContainer _container)
	{
		_container.IncreaseHeat(this.m_bellowsSpray.m_heatIncrease);
	}

	// Token: 0x04001698 RID: 5784
	private BellowsSpray m_bellowsSpray;

	// Token: 0x04001699 RID: 5785
	private List<WindAccumulator> m_activeWindReceivers = new List<WindAccumulator>();

	// Token: 0x0400169A RID: 5786
	private ServerUsableItem m_interactable;

	// Token: 0x0400169B RID: 5787
	private float m_knockbackTimer;
}
