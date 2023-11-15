using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007DF RID: 2015
	public class ServerHordeTarget : ServerSynchroniserBase
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x000B80BA File Offset: 0x000B64BA
		// (set) Token: 0x060026B0 RID: 9904 RVA: 0x000B80C2 File Offset: 0x000B64C2
		public float Health { get; private set; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x000B80CB File Offset: 0x000B64CB
		public float MaxHealth
		{
			get
			{
				return (float)this.m_levelConfig.m_targetHealth;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x000B80D9 File Offset: 0x000B64D9
		public bool IsAlive
		{
			get
			{
				return this.Health > 0f;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x000B80E8 File Offset: 0x000B64E8
		public Transform TargetTransform
		{
			get
			{
				return this.m_target.TargetTransform;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060026B4 RID: 9908 RVA: 0x000B80F5 File Offset: 0x000B64F5
		public float TargetRadius
		{
			get
			{
				return this.m_target.TargetRadius;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060026B5 RID: 9909 RVA: 0x000B8102 File Offset: 0x000B6502
		public Transform SpawnTransform
		{
			get
			{
				return this.m_target.SpawnTransform;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060026B6 RID: 9910 RVA: 0x000B810F File Offset: 0x000B650F
		public float SpawnRadius
		{
			get
			{
				return this.m_target.SpawnRadius;
			}
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000B811C File Offset: 0x000B651C
		public override EntityType GetEntityType()
		{
			return EntityType.HordeTarget;
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000B8120 File Offset: 0x000B6520
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_target = (HordeTarget)synchronisedObject;
			this.m_interactable = base.gameObject.RequireComponent<ServerInteractable>();
			this.m_interactable.RegisterCallbacks(new ServerInteractable.BeginInteractCallback(this.OnBeginInteract), new ServerInteractable.EndInteractCallback(this.OnEndInteract));
			this.m_levelConfig = (GameUtils.GetLevelConfig() as HordeLevelConfig);
			this.Health = (float)this.m_levelConfig.m_targetHealth;
			Mailbox.Server.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000B81B0 File Offset: 0x000B65B0
		private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			GameStateMessage gameStateMessage = (GameStateMessage)message;
			if (gameStateMessage.m_State == GameState.StartedEntities)
			{
				FlowControllerBase flowControllerBase = GameUtils.RequireManager<FlowControllerBase>();
				this.m_flowController = flowControllerBase.gameObject.RequireComponent<ServerHordeFlowController>();
				this.m_flowController.RegisterOnMoneyChanged(null, new GenericVoid<int>(this.OnPlayerMoneyChanged));
				this.m_interactable.SetStickyInteractionCallback(() => true);
				this.m_interactable.SetInteractionSuppressed(true);
			}
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000B8234 File Offset: 0x000B6634
		public override void OnDestroy()
		{
			Mailbox.Server.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
			base.OnDestroy();
			if (this.m_flowController != null)
			{
				this.m_flowController.UnregisterOnMoneyChanged(null, new GenericVoid<int>(this.OnPlayerMoneyChanged));
			}
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000B8288 File Offset: 0x000B6688
		public void Damage(float targetDamage)
		{
			this.Health = Mathf.Max(this.Health - targetDamage, 0f);
			this.m_interactable.SetInteractionSuppressed(this.Health >= this.MaxHealth);
			HordeTargetMessage.Health(ref this.m_message, this.Health);
			this.SendServerEvent(this.m_message);
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000B82EB File Offset: 0x000B66EB
		private void OnBeginInteract(GameObject interacter, Vector2 dir)
		{
			if (this.Health < this.MaxHealth && this.m_flowController.Money > 0)
			{
				this.m_interactors++;
			}
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x000B8320 File Offset: 0x000B6720
		private void OnEndInteract(GameObject interacter)
		{
			if (this.m_interactors > 0)
			{
				this.m_interactors--;
				if (this.m_repairAmount > 0f)
				{
					int amount = Mathf.CeilToInt(Mathf.Clamp01(this.m_repairAmount / this.MaxHealth) * (float)this.m_levelConfig.m_targetRepairCostMax);
					if (this.m_flowController.SpendMoney(amount))
					{
						this.Health = Mathf.Min(this.Health + this.m_repairAmount, this.MaxHealth);
						this.m_interactable.SetInteractionSuppressed(!this.CanInteract());
						HordeTargetMessage.Health(ref this.m_message, this.Health);
						this.SendServerEvent(this.m_message);
					}
					this.m_repairAmount = 0f;
				}
			}
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x000B83EC File Offset: 0x000B67EC
		private bool CanInteract()
		{
			return this.Health < this.MaxHealth && this.m_flowController.Money > 0;
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x000B8410 File Offset: 0x000B6810
		private void OnPlayerMoneyChanged(int _money)
		{
			this.m_interactable.SetInteractionSuppressed(!this.CanInteract());
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x000B8428 File Offset: 0x000B6828
		public override void UpdateSynchronising()
		{
			if (this.m_interactors > 0 && this.Health < this.MaxHealth)
			{
				float num = this.m_levelConfig.m_targetRepairSpeed * (float)this.m_interactors * TimeManager.GetDeltaTime(base.gameObject);
				this.m_repairAmount += num;
				if (this.m_repairAmount >= this.m_levelConfig.m_targetRepairThreshold)
				{
					float num2 = Mathf.Clamp01(this.m_repairAmount / this.MaxHealth) * (float)this.m_levelConfig.m_targetRepairCostMax;
					if (this.m_flowController.SpendMoney((num2 >= 1f) ? ((int)num2) : 1))
					{
						this.Health = Mathf.Min(this.Health + this.m_repairAmount, this.MaxHealth);
						this.m_interactable.SetInteractionSuppressed(!this.CanInteract());
						HordeTargetMessage.Health(ref this.m_message, this.Health);
						this.SendServerEvent(this.m_message);
					}
					this.m_repairAmount = 0f;
				}
			}
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x000B8538 File Offset: 0x000B6938
		public Vector3 GenerateSpawnPosition(out Quaternion dir)
		{
			Vector3 vector = UnityEngine.Random.insideUnitCircle;
			vector = vector.XZY();
			vector = vector * this.SpawnRadius + this.SpawnTransform.position;
			dir = Quaternion.LookRotation(vector - this.TargetTransform.position);
			return vector;
		}

		// Token: 0x04001EB0 RID: 7856
		private HordeTarget m_target;

		// Token: 0x04001EB1 RID: 7857
		private HordeTargetMessage m_message = default(HordeTargetMessage);

		// Token: 0x04001EB2 RID: 7858
		private ServerInteractable m_interactable;

		// Token: 0x04001EB3 RID: 7859
		private int m_interactors;

		// Token: 0x04001EB4 RID: 7860
		private ServerHordeFlowController m_flowController;

		// Token: 0x04001EB5 RID: 7861
		private HordeLevelConfig m_levelConfig;

		// Token: 0x04001EB7 RID: 7863
		private float m_repairAmount;
	}
}
