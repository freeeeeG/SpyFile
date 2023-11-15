using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B91 RID: 2961
	public sealed class RageBuff : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06003D46 RID: 15686 RVA: 0x000B257A File Offset: 0x000B077A
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06003D47 RID: 15687 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06003D48 RID: 15688 RVA: 0x000B2582 File Offset: 0x000B0782
		// (set) Token: 0x06003D49 RID: 15689 RVA: 0x000B258A File Offset: 0x000B078A
		public float remainTime { get; set; }

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06003D4A RID: 15690 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06003D4B RID: 15691 RVA: 0x000B2593 File Offset: 0x000B0793
		public Sprite icon
		{
			get
			{
				return Singleton<DarktechManager>.Instance.resource.rageBuffIcon;
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06003D4C RID: 15692 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06003D4D RID: 15693 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x06003D4E RID: 15694 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06003D4F RID: 15695 RVA: 0x000B25A4 File Offset: 0x000B07A4
		public int iconStacks
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06003D50 RID: 15696 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06003D51 RID: 15697 RVA: 0x000B25AC File Offset: 0x000B07AC
		// (set) Token: 0x06003D52 RID: 15698 RVA: 0x000B25B4 File Offset: 0x000B07B4
		public float duration { get; set; }

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06003D53 RID: 15699 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06003D54 RID: 15700 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06003D55 RID: 15701 RVA: 0x000B25BD File Offset: 0x000B07BD
		// (set) Token: 0x06003D56 RID: 15702 RVA: 0x000B25C6 File Offset: 0x000B07C6
		public float stack
		{
			get
			{
				return (float)this._level;
			}
			set
			{
				this._level = (int)value;
			}
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x000B25D0 File Offset: 0x000B07D0
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x000B2658 File Offset: 0x000B0858
		void IAbilityInstance.Attach()
		{
			this._attached = this._buff.Clone();
			this.remainTime = (float)Singleton<DarktechManager>.Instance.setting.품목순환장치버프맵카운트;
			this._owner.stat.AttachValues(this._attached);
			for (int i = 0; i < this._attached.values.Length; i++)
			{
				this._attached.values[i].value = this._buff.values[i].GetStackedValue((double)this.stack);
			}
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x000B26E8 File Offset: 0x000B08E8
		void IAbilityInstance.Refresh()
		{
			this.remainTime = (float)Singleton<DarktechManager>.Instance.setting.품목순환장치버프맵카운트;
			float stack = this.stack;
			this.stack = stack + 1f;
			for (int i = 0; i < this._attached.values.Length; i++)
			{
				this._attached.values[i].value = this._buff.values[i].GetStackedValue((double)this.stack);
			}
			this._owner.stat.SetNeedUpdate();
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x000B2772 File Offset: 0x000B0972
		void IAbilityInstance.Detach()
		{
			this._owner.stat.DetachValues(this._buff);
		}

		// Token: 0x04002F78 RID: 12152
		private readonly Stat.Values _buff = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.10000000149011612),
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.10000000149011612),
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, 0.10000000149011612)
		});

		// Token: 0x04002F79 RID: 12153
		private Stat.Values _attached;

		// Token: 0x04002F7A RID: 12154
		private Character _owner;

		// Token: 0x04002F7D RID: 12157
		private int _level = 1;
	}
}
