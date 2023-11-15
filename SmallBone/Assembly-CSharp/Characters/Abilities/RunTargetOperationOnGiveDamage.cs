using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AB0 RID: 2736
	[Serializable]
	public sealed class RunTargetOperationOnGiveDamage : Ability
	{
		// Token: 0x0600385C RID: 14428 RVA: 0x000A6368 File Offset: 0x000A4568
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x000A637B File Offset: 0x000A457B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new RunTargetOperationOnGiveDamage.Instance(owner, this);
		}

		// Token: 0x04002CE0 RID: 11488
		[SerializeField]
		private CharacterTypeBoolArray _characterType;

		// Token: 0x04002CE1 RID: 11489
		[SerializeField]
		private AttackTypeBoolArray _attackType;

		// Token: 0x04002CE2 RID: 11490
		[SerializeField]
		private MotionTypeBoolArray _motionType;

		// Token: 0x04002CE3 RID: 11491
		[SerializeField]
		[Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _operations;

		// Token: 0x02000AB1 RID: 2737
		private class Instance : AbilityInstance<RunTargetOperationOnGiveDamage>
		{
			// Token: 0x0600385F RID: 14431 RVA: 0x000A6384 File Offset: 0x000A4584
			public Instance(Character owner, RunTargetOperationOnGiveDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003860 RID: 14432 RVA: 0x000A638E File Offset: 0x000A458E
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
			}

			// Token: 0x06003861 RID: 14433 RVA: 0x000A63B4 File Offset: 0x000A45B4
			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				if (target.character == null)
				{
					return false;
				}
				if (!this.ability._characterType[target.character.type])
				{
					return false;
				}
				if (!this.ability._attackType[damage.attackType])
				{
					return false;
				}
				if (!this.ability._motionType[damage.motionType])
				{
					return false;
				}
				this.owner.StartCoroutine(this.ability._operations.CRun(this.owner, target.character));
				return false;
			}

			// Token: 0x06003862 RID: 14434 RVA: 0x000A644D File Offset: 0x000A464D
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
			}
		}
	}
}
