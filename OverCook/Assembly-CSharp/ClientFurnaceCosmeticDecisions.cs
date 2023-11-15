using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003BC RID: 956
public class ClientFurnaceCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x060011C7 RID: 4551 RVA: 0x0006560C File Offset: 0x00063A0C
	public override void StartSynchronising(Component _synchronisedObject)
	{
		base.StartSynchronising(_synchronisedObject);
		this.m_cosmetics = (FurnaceCosmeticDecisions)_synchronisedObject;
		this.m_heatedStation = base.gameObject.RequireComponent<ClientHeatedStation>();
		this.m_heatedStation.RegisterHeatRangeChangedCallback(new GenericVoid<HeatRange>(this.OnHeatRangeChanged));
		this.m_heatedStation.RegisterOnItemAddedCallback(new CallbackVoid(this.OnItemAdded));
		this.UpdateVisuals(HeatRange.Low);
		this.m_audioManager = GameUtils.RequestManager<AudioManager>();
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x00065680 File Offset: 0x00063A80
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		float value = MathUtils.ClampedRemap(this.m_heatedStation.HeatValue, 0f, 1f, this.m_cosmetics.m_heatedAnimatorMinSpeed, this.m_cosmetics.m_heatedAnimatorMaxSpeed);
		for (int i = 0; i < this.m_cosmetics.m_heatedAnimators.Length; i++)
		{
			if (this.m_cosmetics.m_heatedAnimators[i] != null)
			{
				this.m_cosmetics.m_heatedAnimators[i].SetFloat(FurnaceCosmeticDecisions.s_heatedAnimatorParameterHash, value);
			}
		}
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x00065712 File Offset: 0x00063B12
	private void OnItemAdded()
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_07_Coal_Dispense, base.gameObject.layer);
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x0006572A File Offset: 0x00063B2A
	private void OnHeatRangeChanged(HeatRange _heatRange)
	{
		this.UpdateVisuals(_heatRange);
		this.UpdateAudio(_heatRange);
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x0006573C File Offset: 0x00063B3C
	private void UpdateVisuals(HeatRange _heat)
	{
		this.ToggleEffect(this.m_cosmetics.m_highEffect, _heat == HeatRange.High);
		this.ToggleEffect(this.m_cosmetics.m_mediumEffect, _heat == HeatRange.Moderate);
		this.ToggleEffect(this.m_cosmetics.m_lowEffect, _heat == HeatRange.Low);
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x00065788 File Offset: 0x00063B88
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
				GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_07_Furnace_Die, base.gameObject.layer);
			}
			else
			{
				this.m_activeToken = this.m_mediumHeatToken;
				GameUtils.StartAudio(GameLoopingAudioTag.DLC_07_Furnace_Burn_Med, this.m_activeToken, base.gameObject.layer);
			}
		}
		else
		{
			this.m_activeToken = this.m_highHeatToken;
			GameUtils.StartAudio(GameLoopingAudioTag.DLC_07_Furnace_Burn_Lrg, this.m_activeToken, base.gameObject.layer);
		}
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x00065838 File Offset: 0x00063C38
	private void ToggleEffect(GameObject _effect, bool _turnOn)
	{
		if (_effect != null)
		{
			_effect.SetActive(_turnOn);
		}
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x0006584D File Offset: 0x00063C4D
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_audioManager != null && this.m_activeToken != null)
		{
			this.m_audioManager.StopAudio(GameLoopingAudioTag.COUNT, this.m_activeToken);
		}
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x00065888 File Offset: 0x00063C88
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_heatedStation != null)
		{
			this.m_heatedStation.UnregisterHeatRangeChangedCallback(new GenericVoid<HeatRange>(this.OnHeatRangeChanged));
			this.m_heatedStation.UnregisterOnItemAddedCallback(new CallbackVoid(this.OnItemAdded));
		}
	}

	// Token: 0x04000DD8 RID: 3544
	private FurnaceCosmeticDecisions m_cosmetics;

	// Token: 0x04000DD9 RID: 3545
	private ClientHeatedStation m_heatedStation;

	// Token: 0x04000DDA RID: 3546
	private AudioManager m_audioManager;

	// Token: 0x04000DDB RID: 3547
	private object m_highHeatToken = new object();

	// Token: 0x04000DDC RID: 3548
	private object m_mediumHeatToken = new object();

	// Token: 0x04000DDD RID: 3549
	private object m_activeToken;
}
