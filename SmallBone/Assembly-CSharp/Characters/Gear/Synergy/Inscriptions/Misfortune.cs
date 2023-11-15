using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008A1 RID: 2209
	public sealed class Misfortune : SimpleStatBonusKeyword
	{
		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06002EF2 RID: 12018 RVA: 0x0008CFF0 File Offset: 0x0008B1F0
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06002EF3 RID: 12019 RVA: 0x00088DBC File Offset: 0x00086FBC
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.PercentPoint;
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06002EF4 RID: 12020 RVA: 0x0008CFF8 File Offset: 0x0008B1F8
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.CriticalChance;
			}
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x0008CFFF File Offset: 0x0008B1FF
		protected override void Initialize()
		{
			base.Initialize();
			this._buff.Initialize();
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x0008D014 File Offset: 0x0008B214
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

		// Token: 0x06002EF7 RID: 12023 RVA: 0x0008D07B File Offset: 0x0008B27B
		public override void Detach()
		{
			base.Detach();
			base.character.ability.Remove(this._buff);
		}

		// Token: 0x040026EA RID: 9962
		[Header("2세트 효과")]
		[SerializeField]
		private double[] _statBonusByStep;

		// Token: 0x040026EB RID: 9963
		[Header("4세트 효과")]
		[SerializeField]
		private Misfortune.Buffs _buff;

		// Token: 0x020008A2 RID: 2210
		[Serializable]
		public sealed class Buffs : CooldownAbility
		{
			// Token: 0x06002EF9 RID: 12025 RVA: 0x0008D09A File Offset: 0x0008B29A
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new Misfortune.Buffs.Instance(owner, this);
			}

			// Token: 0x020008A3 RID: 2211
			public sealed class Instance : CooldownAbility.CooldownAbilityInstance
			{
				// Token: 0x06002EFB RID: 12027 RVA: 0x0008D0A3 File Offset: 0x0008B2A3
				public Instance(Character owner, Misfortune.Buffs ability) : base(owner, ability)
				{
				}

				// Token: 0x06002EFC RID: 12028 RVA: 0x0008D0AD File Offset: 0x0008B2AD
				protected override void OnAttach()
				{
					Character owner = this.owner;
					owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
					base.ChangeIconFillToBuffTime();
				}

				// Token: 0x06002EFD RID: 12029 RVA: 0x0008D0DC File Offset: 0x0008B2DC
				private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
				{
					if (!gaveDamage.critical)
					{
						return;
					}
					if (this._buffAttached)
					{
						return;
					}
					if (this._remainCooldownTime > 0f)
					{
						return;
					}
					this.OnAttachBuff();
				}

				// Token: 0x06002EFE RID: 12030 RVA: 0x0008D104 File Offset: 0x0008B304
				protected override void OnDetach()
				{
					Character owner = this.owner;
					owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
					this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
					this.OnDetachBuff();
				}

				// Token: 0x06002EFF RID: 12031 RVA: 0x0008D15B File Offset: 0x0008B35B
				protected override void OnAttachBuff()
				{
					base.OnAttachBuff();
					this.owner.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.OnGiveDamage));
				}

				// Token: 0x06002F00 RID: 12032 RVA: 0x0008D184 File Offset: 0x0008B384
				protected override void OnDetachBuff()
				{
					base.OnDetachBuff();
					this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
				}

				// Token: 0x06002F01 RID: 12033 RVA: 0x0008D1A9 File Offset: 0x0008B3A9
				private bool OnGiveDamage(ITarget target, ref Damage damage)
				{
					damage.criticalChance = 1.0;
					damage.Evaluate(false);
					return false;
				}
			}
		}
	}
}
