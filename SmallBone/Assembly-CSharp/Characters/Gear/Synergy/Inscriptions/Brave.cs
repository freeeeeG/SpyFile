using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000873 RID: 2163
	public sealed class Brave : SimpleStatBonusKeyword
	{
		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06002D63 RID: 11619 RVA: 0x00089E71 File Offset: 0x00088071
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06002D64 RID: 11620 RVA: 0x00089044 File Offset: 0x00087244
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.Percent;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06002D65 RID: 11621 RVA: 0x00088DC3 File Offset: 0x00086FC3
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.PhysicalAttackDamage;
			}
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x00089E79 File Offset: 0x00088079
		protected override void Initialize()
		{
			base.Initialize();
			this._buff.Initialize();
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x00089E8C File Offset: 0x0008808C
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.isMaxStep)
			{
				if (!base.character.ability.Contains(this._buff))
				{
					base.character.ability.Add(this._buff);
					return;
				}
			}
			else
			{
				base.character.ability.Remove(this._buff);
			}
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x00089EF3 File Offset: 0x000880F3
		public override void Detach()
		{
			base.Detach();
			base.character.ability.Remove(this._buff);
		}

		// Token: 0x04002601 RID: 9729
		[SerializeField]
		[Header("2세트 효과")]
		private double[] _statBonusByStep;

		// Token: 0x04002602 RID: 9730
		[Header("4세트 효과")]
		[SerializeField]
		private Brave.Buffs _buff;

		// Token: 0x02000874 RID: 2164
		[Serializable]
		public sealed class Buffs : CooldownAbility
		{
			// Token: 0x06002D6A RID: 11626 RVA: 0x00089F12 File Offset: 0x00088112
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new Brave.Buffs.Instance(owner, this);
			}

			// Token: 0x04002603 RID: 9731
			[SerializeField]
			private Stat.Values _statValues;

			// Token: 0x02000875 RID: 2165
			public sealed class Instance : CooldownAbility.CooldownAbilityInstance
			{
				// Token: 0x06002D6C RID: 11628 RVA: 0x00089F23 File Offset: 0x00088123
				public Instance(Character owner, Brave.Buffs ability) : base(owner, ability)
				{
					this._buffs = ability;
				}

				// Token: 0x06002D6D RID: 11629 RVA: 0x00089F34 File Offset: 0x00088134
				protected override void OnAttach()
				{
					this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
					base.ChangeIconFillToBuffTime();
				}

				// Token: 0x06002D6E RID: 11630 RVA: 0x00089F60 File Offset: 0x00088160
				private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
				{
					if (damage.attribute != Damage.Attribute.Physical)
					{
						return false;
					}
					if (this._remainBuffTime > 0f)
					{
						return false;
					}
					if (this._remainCooldownTime > 0f)
					{
						return false;
					}
					this.owner.stat.AttachValues(this._buffs._statValues);
					this.ability._onAttached.Run(this.owner);
					this.OnAttachBuff();
					return false;
				}

				// Token: 0x06002D6F RID: 11631 RVA: 0x00089FCD File Offset: 0x000881CD
				protected override void OnDetach()
				{
					this.OnDetachBuff();
					this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
				}

				// Token: 0x06002D70 RID: 11632 RVA: 0x00089FF2 File Offset: 0x000881F2
				protected override void OnDetachBuff()
				{
					base.OnDetachBuff();
					this.owner.stat.DetachValues(this._buffs._statValues);
				}

				// Token: 0x04002604 RID: 9732
				private Brave.Buffs _buffs;
			}
		}
	}
}
