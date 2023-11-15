using System;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000C9D RID: 3229
	[Serializable]
	public sealed class BigBlood : Ability
	{
		// Token: 0x060041AA RID: 16810 RVA: 0x000BEE55 File Offset: 0x000BD055
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new BigBlood.Instance(owner, this);
		}

		// Token: 0x04003250 RID: 12880
		[SerializeField]
		[Range(0f, 100f)]
		private int _additionalAttackChance;

		// Token: 0x04003251 RID: 12881
		[SerializeField]
		[Range(0f, 100f)]
		private int _additionalAttackHealthPercent;

		// Token: 0x04003252 RID: 12882
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x02000C9E RID: 3230
		public class Instance : AbilityInstance<BigBlood>
		{
			// Token: 0x060041AC RID: 16812 RVA: 0x000BEE5E File Offset: 0x000BD05E
			public Instance(Character owner, BigBlood ability) : base(owner, ability)
			{
			}

			// Token: 0x060041AD RID: 16813 RVA: 0x000BEE68 File Offset: 0x000BD068
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.OnStartAction;
				this.owner.status.onApplyBleed += this.AdditionalAttack;
			}

			// Token: 0x060041AE RID: 16814 RVA: 0x000BEE9D File Offset: 0x000BD09D
			private void OnStartAction(Characters.Actions.Action action)
			{
				if (action.type == Characters.Actions.Action.Type.Swap)
				{
					this._canUse = true;
					return;
				}
				if (this._canUse && action.type == Characters.Actions.Action.Type.Skill)
				{
					this._canUse = false;
					this.Attack();
				}
			}

			// Token: 0x060041AF RID: 16815 RVA: 0x000BEECE File Offset: 0x000BD0CE
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.OnStartAction;
				this.owner.status.onApplyBleed -= this.AdditionalAttack;
			}

			// Token: 0x060041B0 RID: 16816 RVA: 0x000BEF03 File Offset: 0x000BD103
			public void Attack()
			{
				this.owner.StartCoroutine(this.ability._operations.CRun(this.owner));
			}

			// Token: 0x060041B1 RID: 16817 RVA: 0x000BEF28 File Offset: 0x000BD128
			public void AdditionalAttack(Character giver, Character target)
			{
				if (!MMMaths.PercentChance(this.ability._additionalAttackChance))
				{
					return;
				}
				if (target.health.dead)
				{
					return;
				}
				double @base = target.health.maximumHealth * (double)this.ability._additionalAttackHealthPercent / 100.0;
				Damage damage = new Damage(giver, @base, MMMaths.RandomPointWithinBounds(target.collider.bounds), Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Item, 1.0, 0f, 0.0, 1.0, 1.0, false, false, 0.0, 0.0, 0, null, 1.0);
				giver.Attack(target, ref damage);
				giver.GiveStatus(target, new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Stun));
			}

			// Token: 0x04003253 RID: 12883
			private bool _canUse;
		}
	}
}
