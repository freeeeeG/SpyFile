using System;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CE3 RID: 3299
	[Serializable]
	public sealed class OmenArms : Ability
	{
		// Token: 0x060042C3 RID: 17091 RVA: 0x000C25E7 File Offset: 0x000C07E7
		public override void Initialize()
		{
			base.Initialize();
			this._additionalAttackOnGround.Initialize();
			this._additionalAttackOnAir.Initialize();
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x000C2605 File Offset: 0x000C0805
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OmenArms.Instance(owner, this);
		}

		// Token: 0x04003311 RID: 13073
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04003312 RID: 13074
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _additionalAttackOnGround;

		// Token: 0x04003313 RID: 13075
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _additionalAttackOnAir;

		// Token: 0x02000CE4 RID: 3300
		public class Instance : AbilityInstance<OmenArms>
		{
			// Token: 0x17000DE6 RID: 3558
			// (get) Token: 0x060042C6 RID: 17094 RVA: 0x000C260E File Offset: 0x000C080E
			public override float iconFillAmount
			{
				get
				{
					return this._remainCooldownTime / this.ability._cooldownTime;
				}
			}

			// Token: 0x060042C7 RID: 17095 RVA: 0x000C2622 File Offset: 0x000C0822
			public Instance(Character owner, OmenArms ability) : base(owner, ability)
			{
			}

			// Token: 0x060042C8 RID: 17096 RVA: 0x000C262C File Offset: 0x000C082C
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.HandleOnStartAction;
			}

			// Token: 0x060042C9 RID: 17097 RVA: 0x000C2645 File Offset: 0x000C0845
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.HandleOnStartAction;
			}

			// Token: 0x060042CA RID: 17098 RVA: 0x000C265E File Offset: 0x000C085E
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x060042CB RID: 17099 RVA: 0x000C2678 File Offset: 0x000C0878
			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.BasicAttack && action.type != Characters.Actions.Action.Type.JumpAttack)
				{
					return;
				}
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				OperationInfo.Subcomponents subcomponents = this.owner.movement.isGrounded ? this.ability._additionalAttackOnGround : this.ability._additionalAttackOnAir;
				this.owner.StartCoroutine(subcomponents.CRun(this.owner));
				this._remainCooldownTime = this.ability._cooldownTime;
			}

			// Token: 0x04003314 RID: 13076
			private float _remainCooldownTime;
		}
	}
}
