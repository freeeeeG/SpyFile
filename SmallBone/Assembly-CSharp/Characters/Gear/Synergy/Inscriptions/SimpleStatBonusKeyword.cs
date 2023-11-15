using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008B0 RID: 2224
	public abstract class SimpleStatBonusKeyword : InscriptionInstance
	{
		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06002F43 RID: 12099
		protected abstract double[] statBonusByStep { get; }

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06002F44 RID: 12100
		protected abstract Stat.Category statCategory { get; }

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06002F45 RID: 12101
		protected abstract Stat.Kind statKind { get; }

		// Token: 0x06002F46 RID: 12102 RVA: 0x0008DE11 File Offset: 0x0008C011
		protected override void Initialize()
		{
			this._statBonus = new SimpleStatBonusKeyword.StatBonus(base.character);
			this._statBonus.Initialize();
			this._statBonus.icon = this._icon;
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x0008DE40 File Offset: 0x0008C040
		protected void UpdateStat()
		{
			this._statBonus.currentStatBonus = this.statBonusByStep[this.keyword.step];
			if (this.statCategory.index == Stat.Category.Percent.index)
			{
				this._statBonus.currentStatBonus = this._statBonus.currentStatBonus * 0.01 + 1.0;
			}
			else if (this.statCategory.index == Stat.Category.PercentPoint.index)
			{
				this._statBonus.currentStatBonus *= 0.01;
			}
			this._statBonus.stat.values[0].categoryIndex = this.statCategory.index;
			this._statBonus.stat.values[0].kindIndex = this.statKind.index;
			this._statBonus.UpdateStat();
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x0008DF2E File Offset: 0x0008C12E
		public override void Attach()
		{
			base.character.ability.Add(this._statBonus);
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x0008DF47 File Offset: 0x0008C147
		public override void Detach()
		{
			base.character.ability.Remove(this._statBonus);
			this.UpdateStat();
		}

		// Token: 0x0400271A RID: 10010
		[SerializeField]
		private Sprite _icon;

		// Token: 0x0400271B RID: 10011
		protected SimpleStatBonusKeyword.StatBonus _statBonus;

		// Token: 0x020008B1 RID: 2225
		protected class StatBonus : IAbility, IAbilityInstance
		{
			// Token: 0x17000A1E RID: 2590
			// (get) Token: 0x06002F4B RID: 12107 RVA: 0x0008DF66 File Offset: 0x0008C166
			Character IAbilityInstance.owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x17000A1F RID: 2591
			// (get) Token: 0x06002F4C RID: 12108 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbility ability
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000A20 RID: 2592
			// (get) Token: 0x06002F4D RID: 12109 RVA: 0x0008DF6E File Offset: 0x0008C16E
			// (set) Token: 0x06002F4E RID: 12110 RVA: 0x0008DF76 File Offset: 0x0008C176
			public float remainTime { get; set; }

			// Token: 0x17000A21 RID: 2593
			// (get) Token: 0x06002F4F RID: 12111 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool attached
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000A22 RID: 2594
			// (get) Token: 0x06002F50 RID: 12112 RVA: 0x0008DF7F File Offset: 0x0008C17F
			// (set) Token: 0x06002F51 RID: 12113 RVA: 0x0008DF87 File Offset: 0x0008C187
			public Sprite icon { get; set; }

			// Token: 0x17000A23 RID: 2595
			// (get) Token: 0x06002F52 RID: 12114 RVA: 0x00071719 File Offset: 0x0006F919
			public float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x17000A24 RID: 2596
			// (get) Token: 0x06002F53 RID: 12115 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillInversed
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000A25 RID: 2597
			// (get) Token: 0x06002F54 RID: 12116 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillFlipped
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000A26 RID: 2598
			// (get) Token: 0x06002F55 RID: 12117 RVA: 0x0008DF90 File Offset: 0x0008C190
			public int iconStacks
			{
				get
				{
					return (int)(this.currentStatBonus * 100.0);
				}
			}

			// Token: 0x17000A27 RID: 2599
			// (get) Token: 0x06002F56 RID: 12118 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool expired
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000A28 RID: 2600
			// (get) Token: 0x06002F57 RID: 12119 RVA: 0x0008DFA3 File Offset: 0x0008C1A3
			// (set) Token: 0x06002F58 RID: 12120 RVA: 0x0008DFAB File Offset: 0x0008C1AB
			public float duration { get; set; }

			// Token: 0x17000A29 RID: 2601
			// (get) Token: 0x06002F59 RID: 12121 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconPriority
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17000A2A RID: 2602
			// (get) Token: 0x06002F5A RID: 12122 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool removeOnSwapWeapon
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06002F5B RID: 12123 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbilityInstance CreateInstance(Character owner)
			{
				return this;
			}

			// Token: 0x06002F5C RID: 12124 RVA: 0x0008DFB4 File Offset: 0x0008C1B4
			public StatBonus(Character owner)
			{
				this._owner = owner;
			}

			// Token: 0x06002F5D RID: 12125 RVA: 0x00002191 File Offset: 0x00000391
			public void Initialize()
			{
			}

			// Token: 0x06002F5E RID: 12126 RVA: 0x00002191 File Offset: 0x00000391
			public void UpdateTime(float deltaTime)
			{
			}

			// Token: 0x06002F5F RID: 12127 RVA: 0x00002191 File Offset: 0x00000391
			public void Refresh()
			{
			}

			// Token: 0x06002F60 RID: 12128 RVA: 0x0008DFE7 File Offset: 0x0008C1E7
			void IAbilityInstance.Attach()
			{
				this._owner.stat.AttachValues(this.stat);
			}

			// Token: 0x06002F61 RID: 12129 RVA: 0x0008DFFF File Offset: 0x0008C1FF
			void IAbilityInstance.Detach()
			{
				this._owner.stat.DetachValues(this.stat);
			}

			// Token: 0x06002F62 RID: 12130 RVA: 0x0008E017 File Offset: 0x0008C217
			public void UpdateStat()
			{
				this.stat.values[0].value = this.currentStatBonus;
				this._owner.stat.SetNeedUpdate();
			}

			// Token: 0x0400271C RID: 10012
			[NonSerialized]
			public double currentStatBonus;

			// Token: 0x0400271D RID: 10013
			[NonSerialized]
			public Stat.Values stat = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(0, 0, 0.0)
			});

			// Token: 0x0400271E RID: 10014
			private Character _owner;
		}
	}
}
