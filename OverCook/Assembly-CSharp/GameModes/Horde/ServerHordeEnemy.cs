using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007C7 RID: 1991
	public class ServerHordeEnemy : ServerSynchroniserBase
	{
		// Token: 0x06002617 RID: 9751 RVA: 0x000B4DCD File Offset: 0x000B31CD
		public override EntityType GetEntityType()
		{
			return EntityType.HordeEnemy;
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000B4DD4 File Offset: 0x000B31D4
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_enemy = (HordeEnemy)synchronisedObject;
			this.m_recipeCount = this.m_enemy.m_recipeCount;
			this.m_levelConfig = (GameUtils.GetLevelConfig() as HordeLevelConfig);
			HordeState<HordeEnemyBehaviorState>[] states = new HordeState<HordeEnemyBehaviorState>[]
			{
				new HordeState<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Start),
				new HordeState<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Spawn),
				new HordeState<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Idle),
				new HordeState<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Move),
				new HordeState<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Attack),
				new HordeState<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Channel),
				new HordeState<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Despawn),
				new HordeState<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Despawned)
			};
			this.m_stateMachine = new HordeStateMachine<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Start, states, null, null, new HordeStateMachine<HordeEnemyBehaviorState>.OnUpdate(this.OnUpdateState));
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000B4E7F File Offset: 0x000B327F
		public override void UpdateSynchronising()
		{
			if (!this.m_beginStateMachine)
			{
				this.Transition(HordeEnemyBehaviorState.Spawn);
				this.m_beginStateMachine = true;
			}
			if (this.m_stateMachine != null)
			{
				this.m_stateMachine.Tick(TimeManager.GetDeltaTime(base.gameObject));
			}
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000B4EBB File Offset: 0x000B32BB
		public void Setup(ServerHordeFlowController flowController, ServerHordeTarget target)
		{
			this.m_flowController = flowController;
			this.m_target = target;
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x000B4ECC File Offset: 0x000B32CC
		private void OnUpdateState(IHordeStateMachine<HordeEnemyBehaviorState> stateMachine, HordeEnemyBehaviorState state, float dT)
		{
			if (state != HordeEnemyBehaviorState.Idle)
			{
				if (state != HordeEnemyBehaviorState.Attack)
				{
					if (state == HordeEnemyBehaviorState.Channel)
					{
						this.m_attackTimer += dT;
						if (this.m_target.IsAlive)
						{
							this.Transition(HordeEnemyBehaviorState.Idle);
							this.m_attackTimer = 0f;
						}
						else if (this.m_attackTimer >= this.m_enemy.m_attackKitchenFrequencySeconds)
						{
							this.m_flowController.Damage(this.m_enemy.m_kitchenDamage);
							this.m_attackTimer = 0f;
						}
					}
				}
				else
				{
					this.m_target.Damage((float)this.m_enemy.m_targetDamage);
					this.Transition(HordeEnemyBehaviorState.Idle);
				}
			}
			else
			{
				this.m_attackTimer += dT;
				if (!this.m_target.IsAlive)
				{
					this.Transition(HordeEnemyBehaviorState.Channel);
					this.m_attackTimer = 0f;
				}
				else if (this.m_target.IsAlive && this.m_attackTimer >= this.m_enemy.m_attackTargetFrequencySeconds)
				{
					this.Transition(HordeEnemyBehaviorState.Attack);
					this.m_attackTimer = 0f;
				}
			}
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x000B4FF9 File Offset: 0x000B33F9
		public void Transition(HordeEnemyBehaviorState toState)
		{
			if (this.m_stateMachine.Transition(toState))
			{
				HordeEnemyMessage.Transition(ref this.m_message, toState);
				this.SendServerEvent(this.m_message);
			}
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000B5029 File Offset: 0x000B3429
		public bool Feed(RecipeList.Entry entry)
		{
			this.m_recipeCount--;
			if (this.m_recipeCount <= 0)
			{
				this.Transition(HordeEnemyBehaviorState.Despawn);
				return true;
			}
			return false;
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600261E RID: 9758 RVA: 0x000B504F File Offset: 0x000B344F
		public bool IsAlive
		{
			get
			{
				return this.m_stateMachine.StateId != HordeEnemyBehaviorState.Despawned;
			}
		}

		// Token: 0x04001E25 RID: 7717
		private HordeEnemy m_enemy;

		// Token: 0x04001E26 RID: 7718
		private int m_recipeCount = 1;

		// Token: 0x04001E27 RID: 7719
		private HordeEnemyMessage m_message = default(HordeEnemyMessage);

		// Token: 0x04001E28 RID: 7720
		private HordeStateMachine<HordeEnemyBehaviorState> m_stateMachine;

		// Token: 0x04001E29 RID: 7721
		private ServerHordeFlowController m_flowController;

		// Token: 0x04001E2A RID: 7722
		private ServerHordeTarget m_target;

		// Token: 0x04001E2B RID: 7723
		private HordeLevelConfig m_levelConfig;

		// Token: 0x04001E2C RID: 7724
		private bool m_beginStateMachine;

		// Token: 0x04001E2D RID: 7725
		private float m_attackTimer;
	}
}
