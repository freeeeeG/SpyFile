using System;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B86 RID: 2950
	public sealed class EssenceSpirit : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06003C30 RID: 15408 RVA: 0x000B1309 File Offset: 0x000AF509
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06003C31 RID: 15409 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06003C32 RID: 15410 RVA: 0x000B1311 File Offset: 0x000AF511
		// (set) Token: 0x06003C33 RID: 15411 RVA: 0x000B1319 File Offset: 0x000AF519
		public float remainTime { get; set; }

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06003C34 RID: 15412 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06003C35 RID: 15413 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06003C36 RID: 15414 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06003C37 RID: 15415 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06003C38 RID: 15416 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06003C39 RID: 15417 RVA: 0x000B1322 File Offset: 0x000AF522
		public int iconStacks
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06003C3A RID: 15418 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06003C3B RID: 15419 RVA: 0x000B132A File Offset: 0x000AF52A
		// (set) Token: 0x06003C3C RID: 15420 RVA: 0x000B1332 File Offset: 0x000AF532
		public float duration { get; set; }

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06003C3E RID: 15422 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x000B133B File Offset: 0x000AF53B
		// (set) Token: 0x06003C40 RID: 15424 RVA: 0x000B1344 File Offset: 0x000AF544
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

		// Token: 0x06003C41 RID: 15425 RVA: 0x000B134E File Offset: 0x000AF54E
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003C45 RID: 15429 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x000B138C File Offset: 0x000AF58C
		void IAbilityInstance.Attach()
		{
			this._stat = this._buff.Clone();
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				this._stat.values[i].value = (double)this._level;
			}
			this._owner.stat.AttachValues(this._stat);
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x000B13F4 File Offset: 0x000AF5F4
		void IAbilityInstance.Refresh()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				this._stat.values[i].value = (double)this._level;
			}
			this._owner.stat.SetNeedUpdate();
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x000B1442 File Offset: 0x000AF642
		void IAbilityInstance.Detach()
		{
			this._owner.stat.DetachValues(this._stat);
		}

		// Token: 0x04002F34 RID: 12084
		private readonly Stat.Values _buff = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 1.0)
		});

		// Token: 0x04002F35 RID: 12085
		private Character _owner;

		// Token: 0x04002F38 RID: 12088
		private int _level;

		// Token: 0x04002F39 RID: 12089
		private Stat.Values _stat;
	}
}
