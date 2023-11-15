using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200037F RID: 895
public class ClientBarbequeCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x060010F8 RID: 4344 RVA: 0x000614F4 File Offset: 0x0005F8F4
	public override void StartSynchronising(Component _synchronisedObject)
	{
		base.StartSynchronising(_synchronisedObject);
		this.m_cosmetics = (BarbequeCosmeticDecisions)_synchronisedObject;
		this.m_attachStation = base.gameObject.RequireComponent<ClientAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.ItemAdded));
		this.m_heatedStation = base.gameObject.RequireComponent<ClientHeatedStation>();
		this.m_heatedStation.RegisterHeatRangeChangedCallback(new GenericVoid<HeatRange>(this.OnHeatRangeChanged));
		this.UpdateVisuals(HeatRange.Low);
		this.m_audioManager = GameUtils.RequestManager<AudioManager>();
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x00061578 File Offset: 0x0005F978
	private void ItemAdded(IClientAttachment _item)
	{
		GameObject gameObject = _item.AccessGameObject();
		if (gameObject != null)
		{
			gameObject.transform.localRotation = Quaternion.identity;
		}
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x000615A8 File Offset: 0x0005F9A8
	private void OnHeatRangeChanged(HeatRange _heatRange)
	{
		this.UpdateVisuals(_heatRange);
		this.UpdateAudio(_heatRange);
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x000615B8 File Offset: 0x0005F9B8
	private void UpdateVisuals(HeatRange _heat)
	{
		this.ToggleEffect(this.m_cosmetics.m_highEffect, _heat == HeatRange.High);
		this.ToggleEffect(this.m_cosmetics.m_mediumEffect, _heat == HeatRange.Moderate);
		this.ToggleEffect(this.m_cosmetics.m_lowEffect, _heat == HeatRange.Low);
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x00061604 File Offset: 0x0005FA04
	private void UpdateAudio(HeatRange _heat)
	{
		if (this.m_activeToken != null)
		{
			GameUtils.StopAudio(GameLoopingAudioTag.COUNT, this.m_activeToken);
		}
		if (_heat != HeatRange.High)
		{
			if (_heat != HeatRange.Moderate)
			{
				this.m_activeToken = null;
				GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_02_BBQ_Death, base.gameObject.layer);
			}
			else
			{
				this.m_activeToken = this.m_mediumHeatToken;
				GameUtils.StartAudio(GameLoopingAudioTag.DLC_02_BBQ_Smoke, this.m_activeToken, base.gameObject.layer);
			}
		}
		else
		{
			this.m_activeToken = this.m_highHeatToken;
			GameUtils.StartAudio(GameLoopingAudioTag.DLC_02_BBQ_Idle, this.m_activeToken, base.gameObject.layer);
		}
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x000616B4 File Offset: 0x0005FAB4
	private void ToggleEffect(GameObject _effect, bool _turnOn)
	{
		if (_effect != null)
		{
			_effect.SetActive(_turnOn);
		}
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x000616C9 File Offset: 0x0005FAC9
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_audioManager != null && this.m_activeToken != null)
		{
			this.m_audioManager.StopAudio(GameLoopingAudioTag.COUNT, this.m_activeToken);
		}
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x00061704 File Offset: 0x0005FB04
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_attachStation != null)
		{
			this.m_attachStation.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.ItemAdded));
		}
		if (this.m_heatedStation != null)
		{
			this.m_heatedStation.UnregisterHeatRangeChangedCallback(new GenericVoid<HeatRange>(this.OnHeatRangeChanged));
		}
	}

	// Token: 0x04000D1D RID: 3357
	private BarbequeCosmeticDecisions m_cosmetics;

	// Token: 0x04000D1E RID: 3358
	private ClientHeatedStation m_heatedStation;

	// Token: 0x04000D1F RID: 3359
	private ClientAttachStation m_attachStation;

	// Token: 0x04000D20 RID: 3360
	private AudioManager m_audioManager;

	// Token: 0x04000D21 RID: 3361
	private object m_highHeatToken = new object();

	// Token: 0x04000D22 RID: 3362
	private object m_mediumHeatToken = new object();

	// Token: 0x04000D23 RID: 3363
	private object m_activeToken;
}
