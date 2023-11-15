using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020009A7 RID: 2471
public class ClientHeatedStationGUI : ClientSynchroniserBase
{
	// Token: 0x06003066 RID: 12390 RVA: 0x000E3A5B File Offset: 0x000E1E5B
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_heatedStationGUI = (HeatedStationGUI)synchronisedObject;
		this.m_heatedStation = base.gameObject.RequireComponent<ClientHeatedStation>();
		this.m_heatValue = this.m_heatedStation.HeatValue;
		this.OnHeatValueChanged(this.m_heatValue);
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x000E3A97 File Offset: 0x000E1E97
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.m_heatedStation != null)
		{
			this.OnHeatValueChanged(this.m_heatedStation.HeatValue);
		}
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x000E3AC1 File Offset: 0x000E1EC1
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_uiInstance != null)
		{
			this.m_uiInstance.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x000E3AEB File Offset: 0x000E1EEB
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_uiInstance != null)
		{
			UnityEngine.Object.Destroy(this.m_uiInstance.gameObject);
			this.m_uiInstance = null;
		}
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x000E3B1B File Offset: 0x000E1F1B
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_heatedStation.HeatValue != this.m_heatValue)
		{
			this.m_heatValue = this.m_heatedStation.HeatValue;
			this.OnHeatValueChanged(this.m_heatValue);
		}
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x000E3B58 File Offset: 0x000E1F58
	private void OnHeatValueChanged(float _heatValue)
	{
		if (base.enabled && base.gameObject.activeInHierarchy)
		{
			if (_heatValue > 0f || this.m_heatedStationGUI.m_displayWhenCold)
			{
				if (this.m_uiInstance == null)
				{
					GameObject obj = GameUtils.InstantiateHoverIconUIController(this.m_heatedStationGUI.m_heatUIPrefab.gameObject, NetworkUtils.FindVisualRoot(base.gameObject), "HoverIconCanvas", this.m_heatedStationGUI.m_Offset);
					this.m_uiInstance = obj.RequireComponent<HeatValueUIController>();
				}
				else
				{
					this.m_uiInstance.gameObject.SetActive(true);
				}
			}
			else if (this.m_uiInstance != null)
			{
				this.m_uiInstance.gameObject.SetActive(false);
			}
			if (this.m_uiInstance != null)
			{
				this.m_uiInstance.SetProgress(_heatValue);
			}
		}
	}

	// Token: 0x040026D5 RID: 9941
	private HeatedStationGUI m_heatedStationGUI;

	// Token: 0x040026D6 RID: 9942
	private ClientHeatedStation m_heatedStation;

	// Token: 0x040026D7 RID: 9943
	private HeatValueUIController m_uiInstance;

	// Token: 0x040026D8 RID: 9944
	private float m_heatValue;
}
