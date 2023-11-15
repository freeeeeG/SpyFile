using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AAD RID: 2733
	[Serializable]
	public sealed class RunTargetOperationOnGaveDamage : Ability
	{
		// Token: 0x06003854 RID: 14420 RVA: 0x000A6252 File Offset: 0x000A4452
		public override void Initialize()
		{
			this._operations.Initialize();
			base.Initialize();
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x000A6265 File Offset: 0x000A4465
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new RunTargetOperationOnGaveDamage.Instance(owner, this);
		}

		// Token: 0x04002CDC RID: 11484
		[SerializeField]
		private CharacterTypeBoolArray _characterType;

		// Token: 0x04002CDD RID: 11485
		[SerializeField]
		private AttackTypeBoolArray _attackType;

		// Token: 0x04002CDE RID: 11486
		[SerializeField]
		private MotionTypeBoolArray _motionType;

		// Token: 0x04002CDF RID: 11487
		[SerializeField]
		[Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _operations;

		// Token: 0x02000AAE RID: 2734
		private class Instance : AbilityInstance<RunTargetOperationOnGaveDamage>
		{
			// Token: 0x06003857 RID: 14423 RVA: 0x000A626E File Offset: 0x000A446E
			public Instance(Character owner, RunTargetOperationOnGaveDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003858 RID: 14424 RVA: 0x000A6278 File Offset: 0x000A4478
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003859 RID: 14425 RVA: 0x000A62A1 File Offset: 0x000A44A1
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x0600385A RID: 14426 RVA: 0x000A62CC File Offset: 0x000A44CC
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (target.character == null)
				{
					return;
				}
				if (!this.ability._characterType[target.character.type])
				{
					return;
				}
				if (!this.ability._attackType[gaveDamage.attackType])
				{
					return;
				}
				if (!this.ability._motionType[gaveDamage.motionType])
				{
					return;
				}
				this.owner.StartCoroutine(this.ability._operations.CRun(this.owner, target.character));
			}
		}
	}
}
