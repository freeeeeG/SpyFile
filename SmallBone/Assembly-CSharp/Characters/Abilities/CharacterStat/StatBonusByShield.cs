using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C62 RID: 3170
	[Serializable]
	public class StatBonusByShield : Ability
	{
		// Token: 0x060040DC RID: 16604 RVA: 0x000BC71D File Offset: 0x000BA91D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByShield.Instance(owner, this);
		}

		// Token: 0x040031CF RID: 12751
		[SerializeField]
		private int _maxStack;

		// Token: 0x040031D0 RID: 12752
		[SerializeField]
		[Tooltip("실드량에 이 값을 곱한 숫자가 스택이 됨")]
		private double _stackMultiplier = 1.0;

		// Token: 0x040031D1 RID: 12753
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		[SerializeField]
		private double _iconStacksPerStack = 1.0;

		// Token: 0x040031D2 RID: 12754
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x02000C63 RID: 3171
		public class Instance : AbilityInstance<StatBonusByShield>
		{
			// Token: 0x17000DAB RID: 3499
			// (get) Token: 0x060040DE RID: 16606 RVA: 0x000BC74C File Offset: 0x000BA94C
			public override Sprite icon
			{
				get
				{
					if (this._iconStacks <= 0)
					{
						return null;
					}
					return this.ability.defaultIcon;
				}
			}

			// Token: 0x17000DAC RID: 3500
			// (get) Token: 0x060040DF RID: 16607 RVA: 0x000BC764 File Offset: 0x000BA964
			public override int iconStacks
			{
				get
				{
					return this._iconStacks;
				}
			}

			// Token: 0x060040E0 RID: 16608 RVA: 0x000BC76C File Offset: 0x000BA96C
			public Instance(Character owner, StatBonusByShield ability) : base(owner, ability)
			{
				this._stat = ability._statPerStack.Clone();
			}

			// Token: 0x060040E1 RID: 16609 RVA: 0x000BC788 File Offset: 0x000BA988
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this._stat);
				this.owner.health.onTookDamage += new TookDamageDelegate(this.OnOwnerTookDamage);
				this.owner.health.shield.onAdd += this.OnShieldChanged;
				this.owner.health.shield.onUpdate += this.OnShieldChanged;
				this.owner.health.shield.onRemove += this.OnShieldChanged;
				this.UpdateStat();
			}

			// Token: 0x060040E2 RID: 16610 RVA: 0x000BC830 File Offset: 0x000BAA30
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnOwnerTookDamage);
				this.owner.health.shield.onAdd -= this.OnShieldChanged;
				this.owner.health.shield.onUpdate -= this.OnShieldChanged;
				this.owner.health.shield.onRemove -= this.OnShieldChanged;
			}

			// Token: 0x060040E3 RID: 16611 RVA: 0x000BC8D2 File Offset: 0x000BAAD2
			private void OnShieldChanged(Shield.Instance shieldInstance)
			{
				this.UpdateStat();
			}

			// Token: 0x060040E4 RID: 16612 RVA: 0x000BC8DA File Offset: 0x000BAADA
			private void OnOwnerTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (!this.owner.health.shield.hasAny)
				{
					return;
				}
				if (tookDamage.attackType == Damage.AttackType.None)
				{
					return;
				}
				this.UpdateStat();
			}

			// Token: 0x060040E5 RID: 16613 RVA: 0x000BC904 File Offset: 0x000BAB04
			public void UpdateStat()
			{
				double num = this.owner.health.shield.amount * this.ability._stackMultiplier;
				num = (double)Mathf.Min((int)num, this.ability._maxStack);
				this._iconStacks = (int)(num * this.ability._iconStacksPerStack);
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue(num);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040031D3 RID: 12755
			private Stat.Values _stat;

			// Token: 0x040031D4 RID: 12756
			private int _iconStacks;
		}
	}
}
