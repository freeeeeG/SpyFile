using System;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B87 RID: 2951
	public sealed class FogWolfBuff : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06003C49 RID: 15433 RVA: 0x000B145A File Offset: 0x000AF65A
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06003C4A RID: 15434 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06003C4B RID: 15435 RVA: 0x000B1462 File Offset: 0x000AF662
		// (set) Token: 0x06003C4C RID: 15436 RVA: 0x000B146A File Offset: 0x000AF66A
		public float remainTime { get; set; }

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06003C4D RID: 15437 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06003C4E RID: 15438 RVA: 0x000B1473 File Offset: 0x000AF673
		public Sprite icon
		{
			get
			{
				return SavableAbilityResource.instance.fogWolfBuffIcons[this._buffIndex];
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06003C4F RID: 15439 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06003C50 RID: 15440 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06003C51 RID: 15441 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06003C52 RID: 15442 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06003C53 RID: 15443 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06003C54 RID: 15444 RVA: 0x000B1486 File Offset: 0x000AF686
		// (set) Token: 0x06003C55 RID: 15445 RVA: 0x000B148E File Offset: 0x000AF68E
		public float duration { get; set; }

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06003C56 RID: 15446 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06003C57 RID: 15447 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x06003C58 RID: 15448 RVA: 0x000B1497 File Offset: 0x000AF697
		// (set) Token: 0x06003C59 RID: 15449 RVA: 0x000B14A0 File Offset: 0x000AF6A0
		public float stack
		{
			get
			{
				return (float)this._buffIndex;
			}
			set
			{
				this._buffIndex = (int)value;
			}
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x000B14AA File Offset: 0x000AF6AA
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x000B14B4 File Offset: 0x000AF6B4
		public FogWolfBuff()
		{
			this._buffs = new Stat.Values[]
			{
				this.buff1,
				this.buff2,
				this.buff3,
				this.buff4,
				this.buff5
			};
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x000B15F7 File Offset: 0x000AF7F7
		void IAbilityInstance.Attach()
		{
			this._owner.stat.AttachValues(this._buffs[this._buffIndex]);
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x000B1616 File Offset: 0x000AF816
		void IAbilityInstance.Detach()
		{
			this._owner.stat.DetachValues(this._buffs[this._buffIndex]);
		}

		// Token: 0x04002F3A RID: 12090
		public static int buffCount = 5;

		// Token: 0x04002F3B RID: 12091
		private readonly Stat.Values buff1 = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 1.2)
		});

		// Token: 0x04002F3C RID: 12092
		private readonly Stat.Values buff2 = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 1.3)
		});

		// Token: 0x04002F3D RID: 12093
		private readonly Stat.Values buff3 = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.3),
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.3)
		});

		// Token: 0x04002F3E RID: 12094
		private readonly Stat.Values buff4 = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.15)
		});

		// Token: 0x04002F3F RID: 12095
		private readonly Stat.Values buff5 = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 50.0)
		});

		// Token: 0x04002F40 RID: 12096
		private Character _owner;

		// Token: 0x04002F43 RID: 12099
		private Stat.Values[] _buffs;

		// Token: 0x04002F44 RID: 12100
		private int _buffIndex;
	}
}
