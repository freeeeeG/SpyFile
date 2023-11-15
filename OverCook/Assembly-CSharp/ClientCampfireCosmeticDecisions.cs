using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000390 RID: 912
public class ClientCampfireCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x06001135 RID: 4405 RVA: 0x00062D1C File Offset: 0x0006111C
	public override void StartSynchronising(Component _synchronisedObject)
	{
		base.StartSynchronising(_synchronisedObject);
		this.m_cosmetics = (CampfireCosmeticDecisions)_synchronisedObject;
		this.m_heatedStation = base.gameObject.RequireComponent<ClientHeatedStation>();
		this.m_attachStation = base.gameObject.RequireComponent<ClientAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_heatedStation.RegisterHeatRangeChangedCallback(new GenericVoid<HeatRange>(this.OnHeatRangeChanged));
		ParticleSystem[] array = base.gameObject.RequestComponentsRecursive<ParticleSystem>();
		this.m_fireParticles = array.AllRemoved_Predicate((ParticleSystem x) => !x.collision.enabled);
		this.ToggleFireCollision(false);
		this.UpdateVisuals(HeatRange.Low);
		this.m_audioManager = GameUtils.RequestManager<AudioManager>();
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x00062DF4 File Offset: 0x000611F4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_heatedStation != null)
		{
			this.m_heatedStation.UnregisterHeatRangeChangedCallback(new GenericVoid<HeatRange>(this.OnHeatRangeChanged));
		}
		if (this.m_attachStation != null)
		{
			this.m_attachStation.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
			this.m_attachStation.UnregisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		}
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x00062E6E File Offset: 0x0006126E
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_audioManager != null && this.m_activeToken != null)
		{
			this.m_audioManager.StopAudio(GameLoopingAudioTag.COUNT, this.m_activeToken);
		}
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x00062EA8 File Offset: 0x000612A8
	private void OnItemAdded(IClientAttachment _attachment)
	{
		this.ToggleFireCollision(true);
	}

	// Token: 0x06001139 RID: 4409 RVA: 0x00062EB1 File Offset: 0x000612B1
	private void OnItemRemoved(IClientAttachment _attachment)
	{
		this.ToggleFireCollision(false);
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x00062EBA File Offset: 0x000612BA
	private void OnHeatRangeChanged(HeatRange _heatRange)
	{
		this.UpdateVisuals(_heatRange);
		this.UpdateAudio(_heatRange);
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x00062ECC File Offset: 0x000612CC
	private void UpdateVisuals(HeatRange _heat)
	{
		this.ToggleEffect(this.m_cosmetics.m_highVisuals, _heat == HeatRange.High);
		this.ToggleEffect(this.m_cosmetics.m_mediumVisuals, _heat == HeatRange.Moderate);
		this.ToggleEffect(this.m_cosmetics.m_lowVisuals, _heat == HeatRange.Low);
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x00062F18 File Offset: 0x00061318
	private void UpdateAudio(HeatRange _heat)
	{
		if (this.m_activeToken != null)
		{
			this.m_audioManager.StopAudio(GameLoopingAudioTag.COUNT, this.m_activeToken);
		}
		if (_heat != HeatRange.High)
		{
			if (_heat != HeatRange.Moderate)
			{
				this.m_activeToken = null;
				this.m_audioManager.TriggerAudio(GameOneShotAudioTag.DLC_05_Fire_Hiss, base.gameObject.layer);
			}
			else
			{
				this.m_activeToken = this.m_mediumHeatToken;
				this.m_audioManager.StartAudio(GameLoopingAudioTag.DLC_05_Fire_Med, this.m_activeToken, base.gameObject.layer);
			}
		}
		else
		{
			this.m_activeToken = this.m_highHeatToken;
			this.m_audioManager.StartAudio(GameLoopingAudioTag.DLC_05_Fire_Lrg, this.m_activeToken, base.gameObject.layer);
		}
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x00062FE0 File Offset: 0x000613E0
	private void ToggleEffect(GameObject _effect, bool _turnOn)
	{
		if (_effect != null)
		{
			_effect.SetActive(_turnOn);
		}
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x00062FF8 File Offset: 0x000613F8
	private void ToggleFireCollision(bool _collide)
	{
		for (int i = 0; i < this.m_fireParticles.Length; i++)
		{
			this.m_fireParticles[i].collision.enabled = _collide;
		}
	}

	// Token: 0x04000D56 RID: 3414
	private CampfireCosmeticDecisions m_cosmetics;

	// Token: 0x04000D57 RID: 3415
	private ClientAttachStation m_attachStation;

	// Token: 0x04000D58 RID: 3416
	private ClientHeatedStation m_heatedStation;

	// Token: 0x04000D59 RID: 3417
	private AudioManager m_audioManager;

	// Token: 0x04000D5A RID: 3418
	private ParticleSystem[] m_fireParticles = new ParticleSystem[0];

	// Token: 0x04000D5B RID: 3419
	private object m_highHeatToken = new object();

	// Token: 0x04000D5C RID: 3420
	private object m_mediumHeatToken = new object();

	// Token: 0x04000D5D RID: 3421
	private object m_activeToken;
}
