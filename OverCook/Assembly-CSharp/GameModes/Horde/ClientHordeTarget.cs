using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007E0 RID: 2016
	public class ClientHordeTarget : ClientSynchroniserBase
	{
		// Token: 0x060026C4 RID: 9924 RVA: 0x000B85BF File Offset: 0x000B69BF
		public void RegisterOnDamaged(object handle, GenericVoid<ClientHordeTarget, float, float> onDamaged)
		{
			this.m_onDamaged = (GenericVoid<ClientHordeTarget, float, float>)Delegate.Combine(this.m_onDamaged, onDamaged);
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x000B85D8 File Offset: 0x000B69D8
		public void UnregisterOnDamaged(object handle, GenericVoid<ClientHordeTarget, float, float> onDamaged)
		{
			this.m_onDamaged = (GenericVoid<ClientHordeTarget, float, float>)Delegate.Remove(this.m_onDamaged, onDamaged);
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000B85F1 File Offset: 0x000B69F1
		public override EntityType GetEntityType()
		{
			return EntityType.HordeTarget;
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x000B85F8 File Offset: 0x000B69F8
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_target = (HordeTarget)synchronisedObject;
			this.m_interactable = base.gameObject.RequireComponent<ClientInteractable>();
			this.m_levelConfig = (GameUtils.GetLevelConfig() as HordeLevelConfig);
			this.Health = (float)this.m_levelConfig.m_targetHealth;
			Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x000B8664 File Offset: 0x000B6A64
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
			if (this.m_flowController != null)
			{
				this.m_flowController.UnregisterOnMoneyChanged(null, new GenericVoid<int>(this.OnPlayerMoneyChanged));
			}
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x000B86B8 File Offset: 0x000B6AB8
		private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			GameStateMessage gameStateMessage = (GameStateMessage)message;
			if (gameStateMessage.m_State == GameState.StartEntities)
			{
				FlowControllerBase flowControllerBase = GameUtils.RequireManager<FlowControllerBase>();
				this.m_flowController = flowControllerBase.gameObject.RequireComponent<ClientHordeFlowController>();
				this.m_flowController.RegisterOnMoneyChanged(null, new GenericVoid<int>(this.OnPlayerMoneyChanged));
				this.m_interactable.SetStickyInteractionCallback(() => true);
				this.m_interactable.SetInteractionSuppressed(true);
			}
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000B873C File Offset: 0x000B6B3C
		public override void ApplyServerEvent(Serialisable serialisable)
		{
			HordeTargetMessage hordeTargetMessage = (HordeTargetMessage)serialisable;
			HordeTargetMessage.Kind kind = hordeTargetMessage.m_kind;
			if (kind == HordeTargetMessage.Kind.Health)
			{
				this.Health = hordeTargetMessage.m_health;
				this.m_interactable.SetInteractionSuppressed(!this.CanInteract());
				this.m_onDamaged(this, this.Health, this.MaxHealth);
			}
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x000B87A2 File Offset: 0x000B6BA2
		private bool CanInteract()
		{
			return this.Health < this.MaxHealth && this.m_flowController.Money > 0;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x000B87C6 File Offset: 0x000B6BC6
		private void OnPlayerMoneyChanged(int _money)
		{
			this.m_interactable.SetInteractionSuppressed(!this.CanInteract());
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060026CD RID: 9933 RVA: 0x000B87DC File Offset: 0x000B6BDC
		// (set) Token: 0x060026CE RID: 9934 RVA: 0x000B87E4 File Offset: 0x000B6BE4
		public float Health { get; private set; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060026CF RID: 9935 RVA: 0x000B87ED File Offset: 0x000B6BED
		public float MaxHealth
		{
			get
			{
				return (float)this.m_levelConfig.m_targetHealth;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x000B87FB File Offset: 0x000B6BFB
		public float NormalisedHealth
		{
			get
			{
				return this.Health / this.MaxHealth;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x000B880A File Offset: 0x000B6C0A
		public bool IsAlive
		{
			get
			{
				return this.Health > 0f;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060026D2 RID: 9938 RVA: 0x000B8819 File Offset: 0x000B6C19
		public Transform TargetTransform
		{
			get
			{
				return this.m_target.TargetTransform;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060026D3 RID: 9939 RVA: 0x000B8826 File Offset: 0x000B6C26
		public float TargetRadius
		{
			get
			{
				return this.m_target.TargetRadius;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060026D4 RID: 9940 RVA: 0x000B8833 File Offset: 0x000B6C33
		public Transform SpawnTransform
		{
			get
			{
				return this.m_target.SpawnTransform;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x000B8840 File Offset: 0x000B6C40
		public float SpawnRadius
		{
			get
			{
				return this.m_target.SpawnRadius;
			}
		}

		// Token: 0x04001EB9 RID: 7865
		private HordeTarget m_target;

		// Token: 0x04001EBA RID: 7866
		private ClientInteractable m_interactable;

		// Token: 0x04001EBB RID: 7867
		private ClientHordeFlowController m_flowController;

		// Token: 0x04001EBC RID: 7868
		private HordeLevelConfig m_levelConfig;

		// Token: 0x04001EBD RID: 7869
		public GenericVoid<ClientHordeTarget, float, float> m_onDamaged = delegate(ClientHordeTarget A_0, float A_1, float A_2)
		{
		};
	}
}
