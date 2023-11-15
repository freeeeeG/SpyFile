using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000617 RID: 1559
public class ClientSprayingUtensil : ClientSynchroniserBase
{
	// Token: 0x06001D8A RID: 7562 RVA: 0x0008F05D File Offset: 0x0008D45D
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_SprayingUtensil = (SprayingUtensil)synchronisedObject;
		this.m_interactable = base.gameObject.RequireComponent<ClientInteractable>();
		this.m_interactable.SetStickyInteractionCallback(new Generic<bool>(this.IsStickey));
	}

	// Token: 0x06001D8B RID: 7563 RVA: 0x0008F093 File Offset: 0x0008D493
	public void RegisterOnSpray(CallbackVoid _callback)
	{
		this.m_onSpray = (CallbackVoid)Delegate.Combine(this.m_onSpray, _callback);
	}

	// Token: 0x06001D8C RID: 7564 RVA: 0x0008F0AC File Offset: 0x0008D4AC
	public void UnregisterOnSpray(CallbackVoid _callback)
	{
		this.m_onSpray = (CallbackVoid)Delegate.Remove(this.m_onSpray, _callback);
	}

	// Token: 0x06001D8D RID: 7565 RVA: 0x0008F0C5 File Offset: 0x0008D4C5
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.m_interactable)
		{
			this.m_interactable.SetStickyInteractionCallback(null);
		}
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x0008F0EA File Offset: 0x0008D4EA
	private bool IsStickey()
	{
		return false;
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x0008F0ED File Offset: 0x0008D4ED
	public override EntityType GetEntityType()
	{
		return EntityType.SprayingUtensil;
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x0008F0F0 File Offset: 0x0008D4F0
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		SprayingUtensilMessage sprayingUtensilMessage = (SprayingUtensilMessage)serialisable;
		if (this.m_bSpraying != sprayingUtensilMessage.m_bSpraying)
		{
			if (sprayingUtensilMessage.m_bSpraying)
			{
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(sprayingUtensilMessage.m_Carrier);
				if (entry != null)
				{
					this.m_carrier = entry.m_GameObject;
				}
				this.StartSpray();
			}
			else
			{
				this.StopSpray();
				this.m_carrier = null;
			}
			this.m_bSpraying = sprayingUtensilMessage.m_bSpraying;
		}
	}

	// Token: 0x06001D91 RID: 7569 RVA: 0x0008F162 File Offset: 0x0008D562
	public override void UpdateSynchronising()
	{
		if (TimeManager.IsPaused(base.gameObject))
		{
			this.StopSpray();
		}
	}

	// Token: 0x06001D92 RID: 7570 RVA: 0x0008F17A File Offset: 0x0008D57A
	protected override void OnDisable()
	{
		base.OnDisable();
		this.StopSpray();
	}

	// Token: 0x06001D93 RID: 7571 RVA: 0x0008F188 File Offset: 0x0008D588
	private void StartSpray()
	{
		if (this.m_sprayEffect == null)
		{
			GameObject obj = this.m_SprayingUtensil.m_sprayEffectPrefab.InstantiateOnParent(this.m_SprayingUtensil.m_effectAttachPoint, true);
			this.m_sprayEffect = obj.RequireComponent<ParticleSystem>();
			GameUtils.StartAudio(this.m_SprayingUtensil.m_audioTag, this, this.m_SprayingUtensil.gameObject.layer);
		}
		if (this.m_carrier)
		{
			PlayerControls playerControls = this.m_carrier.RequireComponent<PlayerControls>();
			playerControls.SetMovementScale(0f);
			PlayerIDProvider playerIDProvider = this.m_carrier.RequireComponent<PlayerIDProvider>();
			GameUtils.StartNXRumble(playerIDProvider.GetID(), this.m_SprayingUtensil.m_audioTag);
		}
		if (this.m_onSpray != null)
		{
			this.m_onSpray();
		}
	}

	// Token: 0x06001D94 RID: 7572 RVA: 0x0008F250 File Offset: 0x0008D650
	private void StopSpray()
	{
		if (this.m_sprayEffect != null)
		{
			this.m_sprayEffect.transform.SetParent(null);
			this.m_sprayEffect.Stop();
			this.m_sprayEffect = null;
			GameUtils.StopAudio(this.m_SprayingUtensil.m_audioTag, this);
		}
		if (this.m_carrier)
		{
			PlayerControls playerControls = this.m_carrier.RequireComponent<PlayerControls>();
			playerControls.SetMovementScale(1f);
			PlayerIDProvider playerIDProvider = this.m_carrier.RequireComponent<PlayerIDProvider>();
			GameUtils.StopNXRumble(playerIDProvider.GetID(), this.m_SprayingUtensil.m_audioTag);
		}
	}

	// Token: 0x040016D6 RID: 5846
	private bool m_bSpraying;

	// Token: 0x040016D7 RID: 5847
	private ParticleSystem m_sprayEffect;

	// Token: 0x040016D8 RID: 5848
	private ClientInteractable m_interactable;

	// Token: 0x040016D9 RID: 5849
	private SprayingUtensil m_SprayingUtensil;

	// Token: 0x040016DA RID: 5850
	private GameObject m_carrier;

	// Token: 0x040016DB RID: 5851
	private CallbackVoid m_onSpray;
}
