using System;
using UnityEngine;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B71 RID: 2929
	public class Poison : CharacterStatusAbility, IAbility, IAbilityInstance
	{
		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06003B0E RID: 15118 RVA: 0x000AE381 File Offset: 0x000AC581
		// (set) Token: 0x06003B0F RID: 15119 RVA: 0x000AE389 File Offset: 0x000AC589
		public Character owner { get; private set; }

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06003B10 RID: 15120 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x000AE392 File Offset: 0x000AC592
		// (set) Token: 0x06003B12 RID: 15122 RVA: 0x000AE39A File Offset: 0x000AC59A
		public float remainTime { get; set; }

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06003B13 RID: 15123 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06003B14 RID: 15124 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06003B15 RID: 15125 RVA: 0x000AE3A3 File Offset: 0x000AC5A3
		public float iconFillAmount
		{
			get
			{
				return this.remainTime / this.duration;
			}
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06003B16 RID: 15126 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06003B17 RID: 15127 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06003B19 RID: 15129 RVA: 0x000AE3B2 File Offset: 0x000AC5B2
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06003B1A RID: 15130 RVA: 0x000AE3C4 File Offset: 0x000AC5C4
		public float duration
		{
			get
			{
				return CharacterStatusSetting.instance.poison.duration * base.durationMultiplier;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06003B1B RID: 15131 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06003B1C RID: 15132 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06003B1D RID: 15133 RVA: 0x000AE3DC File Offset: 0x000AC5DC
		public double tickInterval
		{
			get
			{
				return (double)CharacterStatusSetting.instance.poison.tickFrequency - base.attacker.stat.GetFinal(Stat.Kind.PoisonTickFrequency);
			}
		}

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06003B1E RID: 15134 RVA: 0x000AE404 File Offset: 0x000AC604
		// (remove) Token: 0x06003B1F RID: 15135 RVA: 0x000AE43C File Offset: 0x000AC63C
		public override event CharacterStatus.OnTimeDelegate onAttachEvents;

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06003B20 RID: 15136 RVA: 0x000AE474 File Offset: 0x000AC674
		// (remove) Token: 0x06003B21 RID: 15137 RVA: 0x000AE4AC File Offset: 0x000AC6AC
		public override event CharacterStatus.OnTimeDelegate onRefreshEvents;

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06003B22 RID: 15138 RVA: 0x000AE4E4 File Offset: 0x000AC6E4
		// (remove) Token: 0x06003B23 RID: 15139 RVA: 0x000AE51C File Offset: 0x000AC71C
		public override event CharacterStatus.OnTimeDelegate onDetachEvents;

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06003B24 RID: 15140 RVA: 0x000AE551 File Offset: 0x000AC751
		// (set) Token: 0x06003B25 RID: 15141 RVA: 0x000AE559 File Offset: 0x000AC759
		public new StatusEffect.PoisonHandler effectHandler { get; set; }

		// Token: 0x06003B26 RID: 15142 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x000AE562 File Offset: 0x000AC762
		public Poison(Character owner)
		{
			this.owner = owner;
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x000AE574 File Offset: 0x000AC774
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
			this._remainTimeToNextTick -= (double)deltaTime;
			this.effectHandler.UpdateTime(deltaTime);
			if (this._remainTimeToNextTick <= 0.0)
			{
				this._remainTimeToNextTick += this.tickInterval;
				this.GiveDamage(base.attacker);
			}
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x000AE5DC File Offset: 0x000AC7DC
		private void GiveDamage(Character attacker)
		{
			if (attacker == null)
			{
				return;
			}
			Damage damage = attacker.stat.GetDamage((double)CharacterStatusSetting.instance.poison.baseTickDamage, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), CharacterStatusSetting.instance.poison.hitInfo);
			damage.canCritical = false;
			damage.stoppingPower = (this.stoppingPower ? 1f : 0f);
			damage.multiplier -= damage.multiplier * (1.0 - this.owner.stat.GetStatusResistacneFor(CharacterStatus.Kind.Poison));
			attacker.Attack(this.owner, ref damage);
			CharacterStatus.OnTimeDelegate onTimeDelegate = this.onTookPoisonTickDamage;
			if (onTimeDelegate != null)
			{
				onTimeDelegate(attacker, this.owner);
			}
			this.effectHandler.HandleOnTookPoisonTickDamage(attacker, this.owner);
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x000AE6BC File Offset: 0x000AC8BC
		public void Refresh()
		{
			this.remainTime = this.duration;
			CharacterStatus.OnTimeDelegate onRefreshed = this.onRefreshed;
			if (onRefreshed != null)
			{
				onRefreshed(base.attacker, this.owner);
			}
			this.effectHandler.HandleOnRefresh(base.attacker, this.owner);
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x000AE70C File Offset: 0x000AC90C
		public void Attach()
		{
			this.remainTime = this.duration;
			CharacterStatus.OnTimeDelegate onAttached = this.onAttached;
			if (onAttached != null)
			{
				onAttached(base.attacker, this.owner);
			}
			this.effectHandler.HandleOnAttach(base.attacker, this.owner);
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x000AE759 File Offset: 0x000AC959
		public void Detach()
		{
			CharacterStatus.OnTimeDelegate onDetached = this.onDetached;
			if (onDetached != null)
			{
				onDetached(base.attacker, this.owner);
			}
			this.effectHandler.HandleOnDetach(base.attacker, this.owner);
			base.attacker = null;
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x000AE796 File Offset: 0x000AC996
		public void Initialize()
		{
			this.effectHandler = StatusEffect.GetDefaultPoisonEffectHanlder(this.owner);
		}

		// Token: 0x04002EB9 RID: 11961
		private double _remainTimeToNextTick;

		// Token: 0x04002EBA RID: 11962
		public bool stoppingPower;

		// Token: 0x04002EBB RID: 11963
		public CharacterStatus.OnTimeDelegate onTookPoisonTickDamage;
	}
}
