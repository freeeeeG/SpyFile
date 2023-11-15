using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Debuffs
{
	// Token: 0x02000BA3 RID: 2979
	[Serializable]
	public sealed class DamageMission : Ability
	{
		// Token: 0x06003D96 RID: 15766 RVA: 0x000B3055 File Offset: 0x000B1255
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new DamageMission.Instance(owner, this);
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x000B305E File Offset: 0x000B125E
		public override void Initialize()
		{
			base.Initialize();
			this._onSuccess.Initialize();
			this._onFailed.Initialize();
		}

		// Token: 0x04002F99 RID: 12185
		[SerializeField]
		private AttackTypeBoolArray _attackType;

		// Token: 0x04002F9A RID: 12186
		[SerializeField]
		private MotionTypeBoolArray _motionType;

		// Token: 0x04002F9B RID: 12187
		[SerializeField]
		private CharacterTypeBoolArray _targetType;

		// Token: 0x04002F9C RID: 12188
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onSuccess;

		// Token: 0x04002F9D RID: 12189
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onFailed;

		// Token: 0x02000BA4 RID: 2980
		public class Instance : AbilityInstance<DamageMission>
		{
			// Token: 0x06003D99 RID: 15769 RVA: 0x000B307C File Offset: 0x000B127C
			public Instance(Character owner, DamageMission ability) : base(owner, ability)
			{
			}

			// Token: 0x06003D9A RID: 15770 RVA: 0x000B3086 File Offset: 0x000B1286
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x06003D9B RID: 15771 RVA: 0x00002191 File Offset: 0x00000391
			public override void Refresh()
			{
			}

			// Token: 0x06003D9C RID: 15772 RVA: 0x000B30B0 File Offset: 0x000B12B0
			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (!this.ability._motionType[gaveDamage.motionType])
				{
					return;
				}
				if (!this.ability._attackType[gaveDamage.attackType])
				{
					return;
				}
				if (!this.ability._targetType[character.type])
				{
					return;
				}
				if (gaveDamage.@null)
				{
					return;
				}
				if (damageDealt == 0.0)
				{
					return;
				}
				this._success = true;
				this.owner.ability.Remove(this);
			}

			// Token: 0x06003D9D RID: 15773 RVA: 0x000B314C File Offset: 0x000B134C
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				if (this._success)
				{
					this.owner.StartCoroutine(this.ability._onSuccess.CRun(this.owner));
					return;
				}
				this.owner.StartCoroutine(this.ability._onFailed.CRun(this.owner));
			}

			// Token: 0x04002F9E RID: 12190
			private bool _success;
		}
	}
}
