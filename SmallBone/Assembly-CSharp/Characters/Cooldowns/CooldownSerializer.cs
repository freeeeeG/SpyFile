using System;
using Characters.Cooldowns.Streaks;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Cooldowns
{
	// Token: 0x02000904 RID: 2308
	[Serializable]
	public class CooldownSerializer : ICooldown
	{
		// Token: 0x14000085 RID: 133
		// (add) Token: 0x0600313A RID: 12602 RVA: 0x0009372A File Offset: 0x0009192A
		// (remove) Token: 0x0600313B RID: 12603 RVA: 0x00093738 File Offset: 0x00091938
		public event Action onReady
		{
			add
			{
				this.cooldown.onReady += value;
			}
			remove
			{
				this.cooldown.onReady -= value;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x0600313C RID: 12604 RVA: 0x00093746 File Offset: 0x00091946
		public CooldownSerializer.Type type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x0600313D RID: 12605 RVA: 0x0009374E File Offset: 0x0009194E
		// (set) Token: 0x0600313E RID: 12606 RVA: 0x00093756 File Offset: 0x00091956
		private ICooldown cooldown { get; set; }

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x0600313F RID: 12607 RVA: 0x0009375F File Offset: 0x0009195F
		// (set) Token: 0x06003140 RID: 12608 RVA: 0x00093767 File Offset: 0x00091967
		public None none { get; private set; }

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06003141 RID: 12609 RVA: 0x00093770 File Offset: 0x00091970
		// (set) Token: 0x06003142 RID: 12610 RVA: 0x00093778 File Offset: 0x00091978
		public Time time { get; private set; }

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06003143 RID: 12611 RVA: 0x00093781 File Offset: 0x00091981
		// (set) Token: 0x06003144 RID: 12612 RVA: 0x00093789 File Offset: 0x00091989
		public Gauge gauge { get; private set; }

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06003145 RID: 12613 RVA: 0x00093792 File Offset: 0x00091992
		// (set) Token: 0x06003146 RID: 12614 RVA: 0x0009379A File Offset: 0x0009199A
		public Custom custom { get; private set; }

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06003147 RID: 12615 RVA: 0x000937A3 File Offset: 0x000919A3
		public int maxStack
		{
			get
			{
				return this.cooldown.maxStack;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06003148 RID: 12616 RVA: 0x000937B0 File Offset: 0x000919B0
		// (set) Token: 0x06003149 RID: 12617 RVA: 0x000937BD File Offset: 0x000919BD
		public int stacks
		{
			get
			{
				return this.cooldown.stacks;
			}
			set
			{
				this.cooldown.stacks = value;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x0600314A RID: 12618 RVA: 0x000937CB File Offset: 0x000919CB
		public bool canUse
		{
			get
			{
				return this.cooldown.canUse;
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x0600314B RID: 12619 RVA: 0x000937D8 File Offset: 0x000919D8
		public float remainPercent
		{
			get
			{
				return this.cooldown.remainPercent;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x0600314C RID: 12620 RVA: 0x000937E5 File Offset: 0x000919E5
		public bool usedByStreak
		{
			get
			{
				return this.streak.remains < this.streak.count;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x0600314D RID: 12621 RVA: 0x000937FF File Offset: 0x000919FF
		public IStreak streak
		{
			get
			{
				return this.cooldown.streak;
			}
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x0009380C File Offset: 0x00091A0C
		public void Serialize()
		{
			if (this.cooldown != null)
			{
				return;
			}
			switch (this._type)
			{
			case CooldownSerializer.Type.None:
				this.none = new None();
				this.cooldown = this.none;
				return;
			case CooldownSerializer.Type.Time:
				this.time = new Time(this._maxStack, this._streakCount, this._streakTimeout, this._cooldownTime);
				this.cooldown = this.time;
				return;
			case CooldownSerializer.Type.Gauge:
				this.gauge = new Gauge(this._gauge, this._requiredAmount, this._streakCount, this._streakTimeout);
				this.cooldown = this.gauge;
				return;
			case CooldownSerializer.Type.Custom:
				this.custom = new Custom();
				this.cooldown = this.custom;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x000938CF File Offset: 0x00091ACF
		public void CopyCooldown(CooldownSerializer other)
		{
			if (this._type != other._type)
			{
				Debug.LogError("CooldownSerializer type missmatching.");
				return;
			}
			if (this._type == CooldownSerializer.Type.Time)
			{
				this.time.Copy(other.time);
			}
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x00093904 File Offset: 0x00091B04
		public bool Consume()
		{
			return this.cooldown.Consume();
		}

		// Token: 0x0400288D RID: 10381
		[SerializeField]
		private CooldownSerializer.Type _type;

		// Token: 0x0400288E RID: 10382
		[SerializeField]
		private int _maxStack = 1;

		// Token: 0x0400288F RID: 10383
		[SerializeField]
		private int _streakCount;

		// Token: 0x04002890 RID: 10384
		[SerializeField]
		private float _streakTimeout;

		// Token: 0x04002891 RID: 10385
		[SerializeField]
		private float _cooldownTime = 1f;

		// Token: 0x04002892 RID: 10386
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04002893 RID: 10387
		[SerializeField]
		private int _requiredAmount;

		// Token: 0x02000905 RID: 2309
		public enum Type
		{
			// Token: 0x0400289A RID: 10394
			None,
			// Token: 0x0400289B RID: 10395
			Time,
			// Token: 0x0400289C RID: 10396
			Gauge,
			// Token: 0x0400289D RID: 10397
			Custom,
			// Token: 0x0400289E RID: 10398
			Damage,
			// Token: 0x0400289F RID: 10399
			Kill
		}
	}
}
