using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007C8 RID: 1992
	public class ClientHordeEnemy : ClientSynchroniserBase
	{
		// Token: 0x06002620 RID: 9760 RVA: 0x000B50A9 File Offset: 0x000B34A9
		public void RegisterOnBeginState(object handle, GenericVoid<ClientHordeEnemy, HordeEnemyBehaviorState, HordeEnemyBehaviorState> onBeginState)
		{
			this.m_onBeginState = (GenericVoid<ClientHordeEnemy, HordeEnemyBehaviorState, HordeEnemyBehaviorState>)Delegate.Combine(this.m_onBeginState, onBeginState);
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000B50C2 File Offset: 0x000B34C2
		public void UnregisterOnBeginState(object handle, GenericVoid<ClientHordeEnemy, HordeEnemyBehaviorState, HordeEnemyBehaviorState> onBeginState)
		{
			this.m_onBeginState = (GenericVoid<ClientHordeEnemy, HordeEnemyBehaviorState, HordeEnemyBehaviorState>)Delegate.Remove(this.m_onBeginState, onBeginState);
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x000B50DB File Offset: 0x000B34DB
		public override EntityType GetEntityType()
		{
			return EntityType.HordeEnemy;
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000B50E0 File Offset: 0x000B34E0
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_enemy = (HordeEnemy)synchronisedObject;
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
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				this.m_stateMachine = new HordeStateMachine<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Start, states, new HordeStateMachine<HordeEnemyBehaviorState>.OnBegin(this.OnBeginStateServerLocalClient), null, new HordeStateMachine<HordeEnemyBehaviorState>.OnUpdate(this.OnUpdateState));
			}
			else
			{
				this.m_stateMachine = new HordeStateMachine<HordeEnemyBehaviorState>(HordeEnemyBehaviorState.Start, states, new HordeStateMachine<HordeEnemyBehaviorState>.OnBegin(this.OnBeginStateRemoteClient), null, new HordeStateMachine<HordeEnemyBehaviorState>.OnUpdate(this.OnUpdateState));
			}
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000B51B4 File Offset: 0x000B35B4
		public override void ApplyServerEvent(Serialisable serialisable)
		{
			HordeEnemyMessage hordeEnemyMessage = (HordeEnemyMessage)serialisable;
			HordeEnemyMessage.Kind kind = hordeEnemyMessage.m_kind;
			if (kind == HordeEnemyMessage.Kind.Transition)
			{
				this.m_stateMachine.Transition(hordeEnemyMessage.m_toState);
			}
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x000B51F4 File Offset: 0x000B35F4
		public override void UpdateSynchronising()
		{
			if (this.m_stateMachine != null)
			{
				this.m_stateMachine.Tick(TimeManager.GetDeltaTime(base.gameObject));
			}
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x000B5217 File Offset: 0x000B3617
		public void Setup(ClientHordeFlowController flowController, ClientHordeTarget target)
		{
			this.m_target = target;
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x000B5220 File Offset: 0x000B3620
		private void OnBeginStateServerLocalClient(IHordeStateMachine<HordeEnemyBehaviorState> stateMachine, HordeEnemyBehaviorState fromState, HordeEnemyBehaviorState toState)
		{
			ServerHordeEnemy serverHordeEnemy = base.gameObject.RequireComponent<ServerHordeEnemy>();
			serverHordeEnemy.Transition(toState);
			this.m_onBeginState(this, fromState, toState);
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000B524E File Offset: 0x000B364E
		private void OnBeginStateRemoteClient(IHordeStateMachine<HordeEnemyBehaviorState> stateMachine, HordeEnemyBehaviorState fromState, HordeEnemyBehaviorState toState)
		{
			this.m_onBeginState(this, fromState, toState);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000B5260 File Offset: 0x000B3660
		private void OnUpdateState(IHordeStateMachine<HordeEnemyBehaviorState> stateMachine, HordeEnemyBehaviorState state, float dT)
		{
			this.m_time += dT;
			if (state == HordeEnemyBehaviorState.Move)
			{
				if (VectorUtils.DistanceSq(base.transform.position, this.m_target.TargetTransform.position) > this.m_target.TargetRadius + this.m_target.TargetRadius)
				{
					float num = this.m_enemy.m_movementCurve.Evaluate(this.m_time % 1f) * this.m_enemy.m_movementSpeed;
					base.transform.position = Vector3.MoveTowards(base.transform.position, this.m_target.TargetTransform.position, num * dT);
				}
				else
				{
					this.m_stateMachine.Transition(HordeEnemyBehaviorState.Idle);
					this.m_time = 0f;
				}
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600262A RID: 9770 RVA: 0x000B533C File Offset: 0x000B373C
		public bool IsAlive
		{
			get
			{
				return this.m_stateMachine.StateId != HordeEnemyBehaviorState.Despawned;
			}
		}

		// Token: 0x04001E2E RID: 7726
		private HordeEnemy m_enemy;

		// Token: 0x04001E2F RID: 7727
		private HordeEnemyMessage m_message = default(HordeEnemyMessage);

		// Token: 0x04001E30 RID: 7728
		private HordeStateMachine<HordeEnemyBehaviorState> m_stateMachine;

		// Token: 0x04001E31 RID: 7729
		private ClientHordeTarget m_target;

		// Token: 0x04001E32 RID: 7730
		public GenericVoid<ClientHordeEnemy, HordeEnemyBehaviorState, HordeEnemyBehaviorState> m_onBeginState = delegate(ClientHordeEnemy A_0, HordeEnemyBehaviorState A_1, HordeEnemyBehaviorState A_2)
		{
		};

		// Token: 0x04001E33 RID: 7731
		private float m_time;
	}
}
