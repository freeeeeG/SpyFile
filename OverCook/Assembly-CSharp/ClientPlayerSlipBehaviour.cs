using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A21 RID: 2593
public class ClientPlayerSlipBehaviour : ClientSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06003369 RID: 13161 RVA: 0x000F2331 File Offset: 0x000F0731
	public override EntityType GetEntityType()
	{
		return EntityType.PlayerSlip;
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x000F2335 File Offset: 0x000F0735
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_slipBehaviour = (PlayerSlipBehaviour)synchronisedObject;
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x000F234A File Offset: 0x000F074A
	private void Start()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x000F2364 File Offset: 0x000F0764
	protected override void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		base.OnDestroy();
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x000F2384 File Offset: 0x000F0784
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.Initialise();
		}
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x000F23AC File Offset: 0x000F07AC
	private void Initialise()
	{
		this.m_animator = base.gameObject.RequireComponentInImmediateChildren<Animator>();
		this.m_attachPointSlip = base.gameObject.transform.FindChildRecursive("PART_Slip_Locator");
		this.m_attachPointImpact = base.gameObject.transform.FindChildRecursive("Head");
		this.m_playerIDProvider = base.gameObject.RequireComponent<PlayerIDProvider>();
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x000F2414 File Offset: 0x000F0814
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		PlayerSlipMessage playerSlipMessage = (PlayerSlipMessage)serialisable;
		PlayerSlipMessage.MsgType msgType = playerSlipMessage.m_msgType;
		if (msgType != PlayerSlipMessage.MsgType.Slip)
		{
			if (msgType != PlayerSlipMessage.MsgType.Stand)
			{
				if (msgType == PlayerSlipMessage.MsgType.Finished)
				{
					this.m_animator.SetBool(ClientPlayerSlipBehaviour.m_iSlipped, false);
					if (this.m_slipParticles != null)
					{
						this.m_slipParticles.Stop(true);
					}
					if (this.m_streakParticles != null)
					{
						this.m_streakParticles.Stop(true);
					}
					if (this.m_impactParticles != null)
					{
						this.m_impactParticles.Stop(true);
					}
				}
			}
			else
			{
				this.m_animator.SetBool(ClientPlayerSlipBehaviour.m_iSlipped, false);
			}
		}
		else
		{
			this.m_animator.SetBool(ClientPlayerSlipBehaviour.m_iSlipped, true);
			this.m_slipParticles = this.SpawnParticleEffect(this.m_slipBehaviour.m_pfxReferences.m_slipEffect, this.m_attachPointSlip);
			this.m_streakParticles = this.SpawnParticleEffect(this.m_slipBehaviour.m_pfxReferences.m_streakEffect, this.m_attachPointSlip);
			GameUtils.TriggerAudio(GameOneShotAudioTag.PlayerSlip, base.gameObject.layer);
			if (this.m_playerIDProvider != null)
			{
				GameUtils.TriggerNXRumble(this.m_playerIDProvider.GetID(), GameOneShotAudioTag.Boom);
			}
		}
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x000F255C File Offset: 0x000F095C
	private ParticleSystem SpawnParticleEffect(GameObject _prefab, Transform _parent)
	{
		if (_prefab != null)
		{
			GameObject obj = _prefab.InstantiateOnParent(_parent, false);
			ParticleSystem result = obj.RequireComponentRecursive<ParticleSystem>();
			this.m_impactParticles.Play(true);
			return result;
		}
		return null;
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x000F2594 File Offset: 0x000F0994
	public void OnTrigger(string _trigger)
	{
		if (_trigger == this.m_slipBehaviour.m_impactTrigger)
		{
			this.m_impactParticles = this.SpawnParticleEffect(this.m_slipBehaviour.m_pfxReferences.m_impactEffect, this.m_attachPointImpact);
		}
	}

	// Token: 0x0400295D RID: 10589
	private PlayerSlipBehaviour m_slipBehaviour;

	// Token: 0x0400295E RID: 10590
	private static int m_iSlipped = Animator.StringToHash("Slipped");

	// Token: 0x0400295F RID: 10591
	private PlayerIDProvider m_playerIDProvider;

	// Token: 0x04002960 RID: 10592
	private Animator m_animator;

	// Token: 0x04002961 RID: 10593
	private Transform m_attachPointSlip;

	// Token: 0x04002962 RID: 10594
	private Transform m_attachPointImpact;

	// Token: 0x04002963 RID: 10595
	private ParticleSystem m_slipParticles;

	// Token: 0x04002964 RID: 10596
	private ParticleSystem m_impactParticles;

	// Token: 0x04002965 RID: 10597
	private ParticleSystem m_streakParticles;
}
