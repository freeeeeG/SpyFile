using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008C8 RID: 2248
	public sealed class Wisdom : SimpleStatBonusKeyword
	{
		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06002FD9 RID: 12249 RVA: 0x0008F804 File Offset: 0x0008DA04
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06002FDA RID: 12250 RVA: 0x00089044 File Offset: 0x00087244
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.Percent;
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06002FDB RID: 12251 RVA: 0x000894A1 File Offset: 0x000876A1
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.MagicAttackDamage;
			}
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x0008F80C File Offset: 0x0008DA0C
		protected override void Initialize()
		{
			base.Initialize();
			this._buff.Initialize();
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x0008F820 File Offset: 0x0008DA20
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

		// Token: 0x06002FDE RID: 12254 RVA: 0x0008F887 File Offset: 0x0008DA87
		public override void Detach()
		{
			base.Detach();
			base.character.ability.Remove(this._buff);
		}

		// Token: 0x04002787 RID: 10119
		[Header("2세트 효과")]
		[SerializeField]
		private double[] _statBonusByStep;

		// Token: 0x04002788 RID: 10120
		[Header("4세트 효과")]
		[SerializeField]
		private Wisdom.Buffs _buff;

		// Token: 0x020008C9 RID: 2249
		[Serializable]
		public sealed class Buffs : CooldownAbility
		{
			// Token: 0x06002FE0 RID: 12256 RVA: 0x0008F8A6 File Offset: 0x0008DAA6
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new Wisdom.Buffs.Instance(owner, this);
			}

			// Token: 0x04002789 RID: 10121
			[Header("4세트 효과")]
			[SerializeField]
			private Stat.Values _statValues;

			// Token: 0x020008CA RID: 2250
			public sealed class Instance : CooldownAbility.CooldownAbilityInstance
			{
				// Token: 0x06002FE2 RID: 12258 RVA: 0x0008F8AF File Offset: 0x0008DAAF
				public Instance(Character owner, Wisdom.Buffs ability) : base(owner, ability)
				{
					this._buffs = ability;
				}

				// Token: 0x06002FE3 RID: 12259 RVA: 0x0008F8C0 File Offset: 0x0008DAC0
				protected override void OnAttach()
				{
					this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
					base.ChangeIconFillToBuffTime();
				}

				// Token: 0x06002FE4 RID: 12260 RVA: 0x0008F8EC File Offset: 0x0008DAEC
				private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
				{
					if (damage.attribute != Damage.Attribute.Magic)
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

				// Token: 0x06002FE5 RID: 12261 RVA: 0x0008F95A File Offset: 0x0008DB5A
				protected override void OnDetach()
				{
					this.owner.stat.DetachValues(this._buffs._statValues);
					this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
					this.OnDetachBuff();
				}

				// Token: 0x06002FE6 RID: 12262 RVA: 0x0008F99A File Offset: 0x0008DB9A
				protected override void OnDetachBuff()
				{
					base.OnDetachBuff();
					this.owner.stat.DetachValues(this._buffs._statValues);
				}

				// Token: 0x0400278A RID: 10122
				private Wisdom.Buffs _buffs;
			}
		}
	}
}
