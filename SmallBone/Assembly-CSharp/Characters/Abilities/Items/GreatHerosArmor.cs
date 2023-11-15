using System;
using Characters.Operations;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CCC RID: 3276
	[Serializable]
	public sealed class GreatHerosArmor : Ability
	{
		// Token: 0x0600425F RID: 16991 RVA: 0x000C1438 File Offset: 0x000BF638
		public override void Initialize()
		{
			this._debuff.Initialize();
			base.Initialize();
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x000C144B File Offset: 0x000BF64B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GreatHerosArmor.Instance(owner, this);
		}

		// Token: 0x040032D1 RID: 13009
		[SerializeField]
		[Header("스턴 대상")]
		private GreatHerosArmor.Debuff _debuff;

		// Token: 0x02000CCD RID: 3277
		[Serializable]
		public sealed class Debuff : Ability
		{
			// Token: 0x06004262 RID: 16994 RVA: 0x000C1454 File Offset: 0x000BF654
			public override void Initialize()
			{
				base.Initialize();
				this._operations.Initialize();
				this._signOperations.Initialize();
			}

			// Token: 0x06004263 RID: 16995 RVA: 0x000C1472 File Offset: 0x000BF672
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new GreatHerosArmor.Debuff.Instance(owner, this);
			}

			// Token: 0x040032D2 RID: 13010
			[SerializeField]
			private GreatHerosArmorComponent _component;

			// Token: 0x040032D3 RID: 13011
			[SerializeField]
			private AttackTypeBoolArray _stackDamageType;

			// Token: 0x040032D4 RID: 13012
			[SerializeField]
			private float _signTime;

			// Token: 0x040032D5 RID: 13013
			[SerializeField]
			private CustomFloat _baseDamage;

			// Token: 0x040032D6 RID: 13014
			[SerializeField]
			private double _damageConversionRatio;

			// Token: 0x040032D7 RID: 13015
			[SerializeField]
			private CustomFloat _maxBaseDamage;

			// Token: 0x040032D8 RID: 13016
			[SerializeField]
			[CharacterOperation.SubcomponentAttribute]
			private CharacterOperation.Subcomponents _signOperations;

			// Token: 0x040032D9 RID: 13017
			[SerializeField]
			[CharacterOperation.SubcomponentAttribute]
			private CharacterOperation.Subcomponents _operations;

			// Token: 0x040032DA RID: 13018
			[SerializeField]
			private Transform _targetPoint;

			// Token: 0x02000CCE RID: 3278
			public sealed class Instance : AbilityInstance<GreatHerosArmor.Debuff>
			{
				// Token: 0x06004265 RID: 16997 RVA: 0x000C147B File Offset: 0x000BF67B
				public Instance(Character owner, GreatHerosArmor.Debuff ability) : base(owner, ability)
				{
				}

				// Token: 0x06004266 RID: 16998 RVA: 0x000C1485 File Offset: 0x000BF685
				protected override void OnAttach()
				{
					this._stackedDamage = 0.0;
					this._beforeSignTime = 0f;
					this.owner.health.onTookDamage += new TookDamageDelegate(this.HandleOnTookDamage);
				}

				// Token: 0x06004267 RID: 16999 RVA: 0x000C14BD File Offset: 0x000BF6BD
				protected override void OnDetach()
				{
					this.owner.health.onTookDamage -= new TookDamageDelegate(this.HandleOnTookDamage);
				}

				// Token: 0x06004268 RID: 17000 RVA: 0x000C14DC File Offset: 0x000BF6DC
				public override void UpdateTime(float deltaTime)
				{
					base.UpdateTime(deltaTime);
					if (!this.owner.status.stuned)
					{
						this.Attack();
					}
					if (this.owner.status.stun.remainTime < this.ability._signTime)
					{
						if (this.owner.status.stun.remainTime < this._beforeSignTime)
						{
							return;
						}
						this.ability._targetPoint.position = this.owner.collider.bounds.center;
						this.ability._signOperations.Run(this.owner);
						this._beforeSignTime = this.owner.status.stun.remainTime;
					}
				}

				// Token: 0x06004269 RID: 17001 RVA: 0x000C15A1 File Offset: 0x000BF7A1
				private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
				{
					if (!this.ability._stackDamageType[tookDamage.attackType])
					{
						return;
					}
					this._stackedDamage += damageDealt;
				}

				// Token: 0x0600426A RID: 17002 RVA: 0x000C15CC File Offset: 0x000BF7CC
				private void Attack()
				{
					this.ability._component.amount = Mathf.Min((float)(this._stackedDamage * this.ability._damageConversionRatio), this.ability._maxBaseDamage.value);
					this.ability._targetPoint.position = this.owner.collider.bounds.center;
					Character player = Singleton<Service>.Instance.levelManager.player;
					this.ability._operations.Run(player);
					this.owner.ability.Remove(this);
				}

				// Token: 0x0600426B RID: 17003 RVA: 0x000C166C File Offset: 0x000BF86C
				private void HandleTargetOnReleaseStun(Character attacker, Character target)
				{
					double num = this._stackedDamage * this.ability._damageConversionRatio;
					Damage damage = new Damage(attacker, (double)this.ability._baseDamage.value * num, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Item, 1.0, 0f, 0.0, 1.0, 1.0, false, false, 0.0, 0.0, 0, null, 1.0);
					this.ability._targetPoint.position = this.owner.collider.bounds.center;
					attacker.Attack(this.owner, ref damage);
					this.ability._operations.Run(this.owner);
					this.owner.ability.Remove(this);
				}

				// Token: 0x040032DB RID: 13019
				private double _stackedDamage;

				// Token: 0x040032DC RID: 13020
				private float _beforeSignTime;
			}
		}

		// Token: 0x02000CCF RID: 3279
		public class Instance : AbilityInstance<GreatHerosArmor>
		{
			// Token: 0x0600426C RID: 17004 RVA: 0x000C176A File Offset: 0x000BF96A
			public Instance(Character owner, GreatHerosArmor ability) : base(owner, ability)
			{
			}

			// Token: 0x0600426D RID: 17005 RVA: 0x000C1774 File Offset: 0x000BF974
			protected override void OnAttach()
			{
				this.owner.status.onApplyStun += this.HandleOnApplyStun;
			}

			// Token: 0x0600426E RID: 17006 RVA: 0x000C1792 File Offset: 0x000BF992
			private void HandleOnApplyStun(Character attacker, Character target)
			{
				target.ability.Add(this.ability._debuff);
			}

			// Token: 0x0600426F RID: 17007 RVA: 0x000C17AB File Offset: 0x000BF9AB
			protected override void OnDetach()
			{
				this.owner.status.onApplyStun -= this.HandleOnApplyStun;
			}
		}
	}
}
