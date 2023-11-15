using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007B4 RID: 1972
	public class ClientHordeTargetCosmeticDecisions : ClientSynchroniserBase
	{
		// Token: 0x060025CF RID: 9679 RVA: 0x000B29F4 File Offset: 0x000B0DF4
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_cosmeticDecisions = (HordeTargetCosmeticDecisions)synchronisedObject;
			this.m_target = base.gameObject.RequireComponent<ClientHordeTarget>();
			this.m_previousTargetHealth = this.m_target.Health;
			this.m_targetInteractable = base.gameObject.RequireComponent<ClientInteractable>();
			this.m_repairHoverIcon = base.gameObject.RequestComponent<ContextualInteractHoverIcon>();
			Transform follower = (!(this.m_cosmeticDecisions.m_healthUITransform != null)) ? base.transform : this.m_cosmeticDecisions.m_healthUITransform;
			this.m_healthUIInstance = GameUtils.InstantiateHoverIconUIController<HordeTargetUIController>(out this.m_healthUIController, this.m_cosmeticDecisions.m_healthUIPrefab, follower, "HoverIconCanvas", default(Vector3));
			Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000B2AC8 File Offset: 0x000B0EC8
		private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			GameStateMessage gameStateMessage = (GameStateMessage)message;
			if (gameStateMessage.m_State == GameState.StartEntities)
			{
				FlowControllerBase flowControllerBase = GameUtils.RequireManager<FlowControllerBase>();
				this.m_flowController = flowControllerBase.gameObject.RequireComponent<ClientHordeFlowController>();
				this.m_flowController.RegisterOnSuccessfulDelivery(null, this.m_target, new GenericVoid<RecipeList.Entry>(this.OnSuccessfulDelivery));
				this.m_flowController.RegisterOnIncorrectDelivery(null, this.m_target, new GenericVoid<RecipeList.Entry>(this.OnIncorrectDelivery));
				this.m_flowController.RegisterOnEntryAdded(null, this.m_target, new GenericVoid<RecipeList.Entry>(this.OnEntryAdded));
				this.m_flowController.RegisterOnEnemyApproaching(null, this.m_target, new GenericVoid<ClientHordeEnemy>(this.OnEnemyApproaching));
				this.m_flowController.RegisterOnEnemyDespawning(null, this.m_target, new GenericVoid<ClientHordeEnemy>(this.OnEnemyDeath));
			}
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000B2B98 File Offset: 0x000B0F98
		protected override void OnDestroy()
		{
			if (this.m_flowController != null)
			{
				this.m_flowController.UnregisterOnSuccessfulDelivery(null, this.m_target, new GenericVoid<RecipeList.Entry>(this.OnSuccessfulDelivery));
				this.m_flowController.UnregisterOnIncorrectDelivery(null, this.m_target, new GenericVoid<RecipeList.Entry>(this.OnIncorrectDelivery));
				this.m_flowController.UnregisterOnEntryAdded(null, this.m_target, new GenericVoid<RecipeList.Entry>(this.OnEntryAdded));
				this.m_flowController.UnregisterOnEnemyApproaching(null, this.m_target, new GenericVoid<ClientHordeEnemy>(this.OnEnemyApproaching));
				this.m_flowController.UnregisterOnEnemyDeath(null, this.m_target, new GenericVoid<ClientHordeEnemy>(this.OnEnemyDeath));
			}
			Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
			base.OnDestroy();
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000B2C6C File Offset: 0x000B106C
		private void OnSuccessfulDelivery(RecipeList.Entry entry)
		{
			if (this.m_orderUIController != null)
			{
				this.m_orderUIController.PlayAnimation(new RecipeSuccessAnimation());
				this.m_retiredOrderUIControllers.Add(this.m_orderUIController);
				this.m_orderUIController = null;
				this.m_orderUIInstance = null;
			}
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000B2CB9 File Offset: 0x000B10B9
		private void OnIncorrectDelivery(RecipeList.Entry entry)
		{
			if (this.m_orderUIController != null)
			{
				this.m_orderUIController.PlayAnimation(new RecipeFailureAnimation());
			}
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x000B2CDC File Offset: 0x000B10DC
		private void OnEntryAdded(RecipeList.Entry entry)
		{
			Transform transform = (!(this.m_cosmeticDecisions.m_orderUITransform != null)) ? base.transform : this.m_cosmeticDecisions.m_orderUITransform;
			this.m_orderUIInstance = GameUtils.InstantiateHoverIconUIController<HordeOrderUIController>(out this.m_orderUIController, this.m_cosmeticDecisions.m_orderUIPrefab, transform, "HoverIconCanvas", default(Vector3));
			this.m_orderUIController.Setup(transform, entry.m_order.m_orderGuiDescription, this.m_cosmeticDecisions.m_orderUIAnchor);
			this.m_orderUIController.PlayAnimation(new RecipeAppearAnimation(this.m_cosmeticDecisions.m_orderAppearAnimationCurve));
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x000B2D80 File Offset: 0x000B1180
		private void OnEnemyApproaching(ClientHordeEnemy enemy)
		{
			this.m_enemy = enemy;
			this.m_enemy.RegisterOnBeginState(null, new GenericVoid<ClientHordeEnemy, HordeEnemyBehaviorState, HordeEnemyBehaviorState>(this.OnEnemyBeginState));
			this.m_enemyCosmetics = enemy.gameObject.RequireComponent<ClientHordeEnemyCosmeticDecisions>();
			this.m_enemyCosmetics.RegisterAttackAnimationImpactCallback(null, new CallbackVoid(this.OnEnemyAttackAnimationImpact));
			this.m_enemyIsAttacking = false;
			HordeEnemy hordeEnemy = this.m_enemy.gameObject.RequireComponent<HordeEnemy>();
			float progressWarningAnticipation = (float)hordeEnemy.m_targetDamage / this.m_target.MaxHealth;
			this.m_healthUIController.SetProgressWarningAnticipation(progressWarningAnticipation);
			this.m_healthUIController.SetState(HordeTargetUIController.State.Idle);
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x000B2E1C File Offset: 0x000B121C
		private void OnEnemyDeath(ClientHordeEnemy enemy)
		{
			if (this.m_target.Health != this.m_previousTargetHealth)
			{
				this.OnEnemyAttackAnimationImpact();
			}
			if (this.m_enemyCosmetics != null)
			{
				this.m_enemyCosmetics.UnregisterAttackAnimationImpactCallback(null, new CallbackVoid(this.OnEnemyAttackAnimationImpact));
				this.m_enemyCosmetics = null;
			}
			if (this.m_enemy != null)
			{
				this.m_enemy.UnregisterOnBeginState(null, new GenericVoid<ClientHordeEnemy, HordeEnemyBehaviorState, HordeEnemyBehaviorState>(this.OnEnemyBeginState));
				this.m_enemy = null;
			}
			this.m_healthUIController.SetProgressWarningAnticipation(0f);
			this.m_cosmeticDecisions.m_animator.SetBool(this.m_cosmeticDecisions.m_enemyAnimationId, false);
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x000B2ED1 File Offset: 0x000B12D1
		private void OnEnemyBeginState(ClientHordeEnemy enemy, HordeEnemyBehaviorState fromState, HordeEnemyBehaviorState state)
		{
			this.m_enemyIsAttacking = (state == HordeEnemyBehaviorState.Idle || state == HordeEnemyBehaviorState.Attack || state == HordeEnemyBehaviorState.Channel);
			this.m_cosmeticDecisions.m_animator.SetBool(this.m_cosmeticDecisions.m_enemyAnimationId, this.m_enemyIsAttacking);
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x000B2F10 File Offset: 0x000B1310
		private void OnEnemyAttackAnimationImpact()
		{
			this.m_cosmeticDecisions.m_animator.SetTrigger(this.m_cosmeticDecisions.m_hitAnimationId);
			this.m_cosmeticDecisions.m_animator.SetFloat(this.m_cosmeticDecisions.m_healthAnimationId, this.m_target.NormalisedHealth);
			if (this.m_target.IsAlive)
			{
				if (this.m_cosmeticDecisions.m_hitParticlePrefab != null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_cosmeticDecisions.m_breakParticlePrefab, base.transform.position, base.transform.rotation);
				}
			}
			else if (this.m_cosmeticDecisions.m_destroyParticlePrefab != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_cosmeticDecisions.m_destroyParticlePrefab, base.transform.position, base.transform.rotation);
			}
			this.m_previousTargetHealth = this.m_target.Health;
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x000B3000 File Offset: 0x000B1400
		public override void UpdateSynchronising()
		{
			this.m_healthUIController.SetProgress(this.m_target.NormalisedHealth);
			bool flag = this.m_targetInteractable.InteractorCount() > 0;
			if (this.m_repairHoverIcon != null)
			{
				this.m_repairHoverIcon.enabled = !flag;
			}
			bool active = this.m_enemy != null || flag;
			this.m_healthUIController.gameObject.SetActive(active);
			if (flag)
			{
				this.m_healthUIController.SetState(HordeTargetUIController.State.Repairing);
				this.m_cosmeticDecisions.m_animator.SetFloat(this.m_cosmeticDecisions.m_healthAnimationId, this.m_target.NormalisedHealth);
			}
			else if (this.m_enemy != null && (!this.m_enemyIsAttacking || this.m_target.IsAlive))
			{
				this.m_healthUIController.SetState(HordeTargetUIController.State.UnderAttack);
			}
			else if (this.m_target.IsAlive)
			{
				this.m_healthUIController.SetState(HordeTargetUIController.State.Idle);
			}
			else
			{
				this.m_healthUIController.SetState(HordeTargetUIController.State.Broken);
			}
			if (this.m_retiredOrderUIControllers.Count > 0)
			{
				for (int i = this.m_retiredOrderUIControllers.Count - 1; i >= 0; i--)
				{
					if (!this.m_retiredOrderUIControllers[i].IsPlayingAnimation())
					{
						GameObject gameObject = this.m_retiredOrderUIControllers[i].gameObject;
						this.m_retiredOrderUIControllers.RemoveAt(i);
						UnityEngine.Object.Destroy(gameObject);
					}
				}
			}
		}

		// Token: 0x04001D96 RID: 7574
		private HordeTargetCosmeticDecisions m_cosmeticDecisions;

		// Token: 0x04001D97 RID: 7575
		private ClientHordeTarget m_target;

		// Token: 0x04001D98 RID: 7576
		private GameObject m_healthUIInstance;

		// Token: 0x04001D99 RID: 7577
		private HordeTargetUIController m_healthUIController;

		// Token: 0x04001D9A RID: 7578
		private GameObject m_orderUIInstance;

		// Token: 0x04001D9B RID: 7579
		private HordeOrderUIController m_orderUIController;

		// Token: 0x04001D9C RID: 7580
		private List<HordeOrderUIController> m_retiredOrderUIControllers = new List<HordeOrderUIController>();

		// Token: 0x04001D9D RID: 7581
		private ContextualInteractHoverIcon m_repairHoverIcon;

		// Token: 0x04001D9E RID: 7582
		private ClientInteractable m_targetInteractable;

		// Token: 0x04001D9F RID: 7583
		private ClientHordeEnemy m_enemy;

		// Token: 0x04001DA0 RID: 7584
		private ClientHordeEnemyCosmeticDecisions m_enemyCosmetics;

		// Token: 0x04001DA1 RID: 7585
		private bool m_enemyIsAttacking;

		// Token: 0x04001DA2 RID: 7586
		private ClientHordeFlowController m_flowController;

		// Token: 0x04001DA3 RID: 7587
		private float m_previousTargetHealth;
	}
}
