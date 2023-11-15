using System;
using UnityEngine;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B7F RID: 2943
	public class Wound : CharacterStatusAbility, IAbility, IAbilityInstance
	{
		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06003BB9 RID: 15289 RVA: 0x000B051F File Offset: 0x000AE71F
		// (set) Token: 0x06003BBA RID: 15290 RVA: 0x000B0527 File Offset: 0x000AE727
		public Character owner { get; private set; }

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06003BBB RID: 15291 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06003BBC RID: 15292 RVA: 0x000B0530 File Offset: 0x000AE730
		// (set) Token: 0x06003BBD RID: 15293 RVA: 0x000B0538 File Offset: 0x000AE738
		public float remainTime { get; set; }

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06003BBE RID: 15294 RVA: 0x000B0541 File Offset: 0x000AE741
		// (set) Token: 0x06003BBF RID: 15295 RVA: 0x000B0549 File Offset: 0x000AE749
		public bool attached { get; set; }

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06003BC0 RID: 15296 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06003BC1 RID: 15297 RVA: 0x000B0552 File Offset: 0x000AE752
		public float iconFillAmount
		{
			get
			{
				return this.remainTime / this.duration;
			}
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06003BC2 RID: 15298 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06003BC3 RID: 15299 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06003BC4 RID: 15300 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06003BC5 RID: 15301 RVA: 0x000B0561 File Offset: 0x000AE761
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06003BC6 RID: 15302 RVA: 0x000B0573 File Offset: 0x000AE773
		public float duration
		{
			get
			{
				return 2.1474836E+09f;
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06003BC7 RID: 15303 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06003BC8 RID: 15304 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06003BC9 RID: 15305 RVA: 0x000B057A File Offset: 0x000AE77A
		// (set) Token: 0x06003BCA RID: 15306 RVA: 0x000B0582 File Offset: 0x000AE782
		public bool critical { get; set; }

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06003BCB RID: 15307 RVA: 0x000B058C File Offset: 0x000AE78C
		// (remove) Token: 0x06003BCC RID: 15308 RVA: 0x000B05C4 File Offset: 0x000AE7C4
		public override event CharacterStatus.OnTimeDelegate onAttachEvents;

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x06003BCD RID: 15309 RVA: 0x000B05FC File Offset: 0x000AE7FC
		// (remove) Token: 0x06003BCE RID: 15310 RVA: 0x000B0634 File Offset: 0x000AE834
		public override event CharacterStatus.OnTimeDelegate onRefreshEvents;

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x06003BCF RID: 15311 RVA: 0x000B066C File Offset: 0x000AE86C
		// (remove) Token: 0x06003BD0 RID: 15312 RVA: 0x000B06A4 File Offset: 0x000AE8A4
		public override event CharacterStatus.OnTimeDelegate onDetachEvents;

		// Token: 0x06003BD1 RID: 15313 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x000B06D9 File Offset: 0x000AE8D9
		public Wound(Character owner)
		{
			this.owner = owner;
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x000B06E8 File Offset: 0x000AE8E8
		public void UpdateTime(float deltaTime)
		{
			this.attached = true;
			this.remainTime -= deltaTime;
			base.effectHandler.UpdateTime(deltaTime);
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x000B070C File Offset: 0x000AE90C
		private void GiveDamage()
		{
			if (base.attacker == null)
			{
				return;
			}
			if (this.owner == null)
			{
				return;
			}
			Damage damage = base.attacker.stat.GetDamage((double)CharacterStatusSetting.instance.bleed.baseDamage, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), CharacterStatusSetting.instance.bleed.hitInfo);
			damage.criticalChance = base.attacker.stat.GetFinal(Stat.Kind.CriticalChance) - 1.0;
			damage.criticalDamageMultiplier = base.attacker.stat.GetFinal(Stat.Kind.CriticalDamage);
			damage.canCritical = this.critical;
			damage.multiplier *= base.attacker.stat.GetFinal(Stat.Kind.BleedDamage);
			damage.multiplier -= damage.multiplier * (1.0 - this.owner.stat.GetStatusResistacneFor(CharacterStatus.Kind.Wound));
			base.attacker.Attack(this.owner, ref damage);
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x000B082C File Offset: 0x000AEA2C
		public void Refresh()
		{
			this.GiveDamage();
			this.remainTime = 0f;
			this.attached = false;
			CharacterStatus.OnTimeDelegate onDetached = this.onDetached;
			if (onDetached != null)
			{
				onDetached(base.attacker, this.owner);
			}
			base.effectHandler.HandleOnDetach(base.attacker, this.owner);
			this.owner.ability.Remove(this);
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x000B0898 File Offset: 0x000AEA98
		public void Attach()
		{
			this.remainTime = this.duration;
			CharacterStatus.OnTimeDelegate onAttached = this.onAttached;
			if (onAttached != null)
			{
				onAttached(base.attacker, this.owner);
			}
			base.effectHandler.HandleOnAttach(base.attacker, this.owner);
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x000B08E5 File Offset: 0x000AEAE5
		public void Detach()
		{
			this.remainTime = 0f;
			this.attached = false;
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x000B08F9 File Offset: 0x000AEAF9
		public void Initialize()
		{
			base.effectHandler = StatusEffect.GetDefaultWoundEffectHanlder(this.owner);
		}
	}
}
