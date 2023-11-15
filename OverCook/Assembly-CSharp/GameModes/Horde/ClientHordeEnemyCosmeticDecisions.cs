using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007AE RID: 1966
	public class ClientHordeEnemyCosmeticDecisions : ClientSynchroniserBase
	{
		// Token: 0x060025B9 RID: 9657 RVA: 0x000B226B File Offset: 0x000B066B
		public void RegisterAttackAnimationImpactCallback(object handle, CallbackVoid onAnimationImpact)
		{
			this.m_attackAnimationImpactCallback = (CallbackVoid)Delegate.Combine(this.m_attackAnimationImpactCallback, onAnimationImpact);
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x000B2284 File Offset: 0x000B0684
		public void UnregisterAttackAnimationImpactCallback(object handle, CallbackVoid onAnimationImpact)
		{
			this.m_attackAnimationImpactCallback = (CallbackVoid)Delegate.Remove(this.m_attackAnimationImpactCallback, onAnimationImpact);
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x000B22A0 File Offset: 0x000B06A0
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_cosmeticDecisions = (HordeEnemyCosmeticDecisions)synchronisedObject;
			HordeEnemyCosmeticDecisions cosmeticDecisions = this.m_cosmeticDecisions;
			cosmeticDecisions.OnTriggerCallback = (GenericVoid<string>)Delegate.Combine(cosmeticDecisions.OnTriggerCallback, new GenericVoid<string>(this.OnTrigger));
			this.m_enemy = base.gameObject.RequireComponent<ClientHordeEnemy>();
			this.m_enemy.RegisterOnBeginState(this, new GenericVoid<ClientHordeEnemy, HordeEnemyBehaviorState, HordeEnemyBehaviorState>(this.OnBeginState));
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				this.m_serverEnemy = base.gameObject.RequireComponent<ServerHordeEnemy>();
			}
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000B2338 File Offset: 0x000B0738
		protected override void OnDestroy()
		{
			HordeEnemyCosmeticDecisions cosmeticDecisions = this.m_cosmeticDecisions;
			cosmeticDecisions.OnTriggerCallback = (GenericVoid<string>)Delegate.Remove(cosmeticDecisions.OnTriggerCallback, new GenericVoid<string>(this.OnTrigger));
			this.m_enemy.UnregisterOnBeginState(this, new GenericVoid<ClientHordeEnemy, HordeEnemyBehaviorState, HordeEnemyBehaviorState>(this.OnBeginState));
			base.OnDestroy();
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x000B238C File Offset: 0x000B078C
		private void OnBeginState(ClientHordeEnemy enemy, HordeEnemyBehaviorState fromState, HordeEnemyBehaviorState state)
		{
			switch (state)
			{
			case HordeEnemyBehaviorState.Spawn:
				this.m_cosmeticDecisions.m_animator.SetTrigger(this.m_cosmeticDecisions.m_spawnAnimationTriggerId);
				GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_07_Nombie_Spawn, base.gameObject.layer);
				if (this.m_cosmeticDecisions.m_spawnEffectsPrefab != null)
				{
					this.m_cosmeticDecisions.m_spawnEffectsPrefab.InstantiateOnParent(base.gameObject.transform, false);
				}
				break;
			case HordeEnemyBehaviorState.Idle:
				this.m_cosmeticDecisions.m_animator.SetTrigger(this.m_cosmeticDecisions.m_idleAnimationTriggerId);
				break;
			case HordeEnemyBehaviorState.Move:
				this.m_cosmeticDecisions.m_animator.SetTrigger(this.m_cosmeticDecisions.m_moveAnimationTriggerId);
				break;
			case HordeEnemyBehaviorState.Attack:
				this.m_cosmeticDecisions.m_animator.SetTrigger(this.m_cosmeticDecisions.m_attackAnimationTriggerId);
				break;
			case HordeEnemyBehaviorState.Channel:
				this.m_cosmeticDecisions.m_animator.SetTrigger(this.m_cosmeticDecisions.m_channelAnimationTriggerId);
				break;
			case HordeEnemyBehaviorState.Despawn:
				this.m_cosmeticDecisions.m_animator.SetTrigger(this.m_cosmeticDecisions.m_despawnAnimationTriggerId);
				GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_07_Nombie_Despawn, base.gameObject.layer);
				if (this.m_cosmeticDecisions.m_despawnEffectsPrefab != null)
				{
					GameObject gameObject = this.m_cosmeticDecisions.m_despawnEffectsPrefab.InstantiateOnParent(base.gameObject.transform.parent, false);
					gameObject.transform.SetPositionAndRotation(base.gameObject.transform.position, base.gameObject.transform.rotation);
				}
				break;
			}
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x000B254C File Offset: 0x000B094C
		private void OnTrigger(string trigger)
		{
			int num = Animator.StringToHash(trigger);
			if (this.m_serverEnemy != null)
			{
				if (num == this.m_cosmeticDecisions.m_spawnAnimationEndTriggerId)
				{
					this.m_serverEnemy.Transition(HordeEnemyBehaviorState.Move);
				}
				else if (num == this.m_cosmeticDecisions.m_attackAnimationEndTriggerId)
				{
					this.m_serverEnemy.Transition(HordeEnemyBehaviorState.Idle);
				}
				else if (num == this.m_cosmeticDecisions.m_despawnAnimationEndTriggerId)
				{
					this.m_serverEnemy.Transition(HordeEnemyBehaviorState.Despawned);
				}
			}
			if (num == this.m_cosmeticDecisions.m_attackAnimationImpactTriggerId)
			{
				this.m_attackAnimationImpactCallback();
			}
		}

		// Token: 0x04001D62 RID: 7522
		private HordeEnemyCosmeticDecisions m_cosmeticDecisions;

		// Token: 0x04001D63 RID: 7523
		private ClientHordeEnemy m_enemy;

		// Token: 0x04001D64 RID: 7524
		private ServerHordeEnemy m_serverEnemy;

		// Token: 0x04001D65 RID: 7525
		public CallbackVoid m_attackAnimationImpactCallback = delegate()
		{
		};
	}
}
