using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B90 RID: 2960
	public sealed class FortitudeBuff : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06003D2E RID: 15662 RVA: 0x000B2382 File Offset: 0x000B0582
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06003D2F RID: 15663 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06003D30 RID: 15664 RVA: 0x000B238A File Offset: 0x000B058A
		// (set) Token: 0x06003D31 RID: 15665 RVA: 0x000B2392 File Offset: 0x000B0592
		public float remainTime { get; set; }

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06003D32 RID: 15666 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06003D33 RID: 15667 RVA: 0x000B239B File Offset: 0x000B059B
		public Sprite icon
		{
			get
			{
				return Singleton<DarktechManager>.Instance.resource.fortitudeBuffIcon;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06003D34 RID: 15668 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06003D35 RID: 15669 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06003D36 RID: 15670 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06003D37 RID: 15671 RVA: 0x000B23AC File Offset: 0x000B05AC
		public int iconStacks
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06003D38 RID: 15672 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06003D39 RID: 15673 RVA: 0x000B23B4 File Offset: 0x000B05B4
		// (set) Token: 0x06003D3A RID: 15674 RVA: 0x000B23BC File Offset: 0x000B05BC
		public float duration { get; set; }

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06003D3B RID: 15675 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06003D3C RID: 15676 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06003D3D RID: 15677 RVA: 0x000B23C5 File Offset: 0x000B05C5
		// (set) Token: 0x06003D3E RID: 15678 RVA: 0x000B23CE File Offset: 0x000B05CE
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

		// Token: 0x06003D3F RID: 15679 RVA: 0x000B23D8 File Offset: 0x000B05D8
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x000B2448 File Offset: 0x000B0648
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

		// Token: 0x06003D44 RID: 15684 RVA: 0x000B24D8 File Offset: 0x000B06D8
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

		// Token: 0x06003D45 RID: 15685 RVA: 0x000B2562 File Offset: 0x000B0762
		void IAbilityInstance.Detach()
		{
			this._owner.stat.DetachValues(this._buff);
		}

		// Token: 0x04002F72 RID: 12146
		private readonly Stat.Values _buff = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.10000000149011612),
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.10000000149011612)
		});

		// Token: 0x04002F73 RID: 12147
		private Stat.Values _attached;

		// Token: 0x04002F74 RID: 12148
		private Character _owner;

		// Token: 0x04002F77 RID: 12151
		private int _level = 1;
	}
}
