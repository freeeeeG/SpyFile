using System;
using System.Collections;
using Characters.Cooldowns.Streaks;

namespace Characters.Cooldowns
{
	// Token: 0x0200090B RID: 2315
	public class Time : ICooldown
	{
		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003187 RID: 12679 RVA: 0x00093C14 File Offset: 0x00091E14
		// (set) Token: 0x06003188 RID: 12680 RVA: 0x00093C1C File Offset: 0x00091E1C
		public int stacks
		{
			get
			{
				return this._stacks;
			}
			set
			{
				if (this._stacks == 0 && value > 0 && this._onReady != null)
				{
					this._onReady();
				}
				this._stacks = value;
			}
		}

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06003189 RID: 12681 RVA: 0x00093C44 File Offset: 0x00091E44
		// (remove) Token: 0x0600318A RID: 12682 RVA: 0x00093C5D File Offset: 0x00091E5D
		public event Action onReady
		{
			add
			{
				this._onReady = (Action)Delegate.Combine(this._onReady, value);
			}
			remove
			{
				this._onReady = (Action)Delegate.Remove(this._onReady, value);
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x0600318B RID: 12683 RVA: 0x00093C76 File Offset: 0x00091E76
		// (set) Token: 0x0600318C RID: 12684 RVA: 0x00093C7E File Offset: 0x00091E7E
		public int maxStack { get; protected set; }

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x0600318D RID: 12685 RVA: 0x00093C87 File Offset: 0x00091E87
		public bool canUse
		{
			get
			{
				return this.stacks > 0 || this.streak.remains > 0;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x0600318E RID: 12686 RVA: 0x00093CA2 File Offset: 0x00091EA2
		// (set) Token: 0x0600318F RID: 12687 RVA: 0x00093CAA File Offset: 0x00091EAA
		public IStreak streak { get; protected set; }

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003190 RID: 12688 RVA: 0x00093CB3 File Offset: 0x00091EB3
		public float remainPercent
		{
			get
			{
				if (this.streak.remains > 0)
				{
					return 0f;
				}
				if (this.stacks != this.maxStack)
				{
					return this.remainTime / this.cooldownTime;
				}
				return 0f;
			}
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x00093CEC File Offset: 0x00091EEC
		public Time(int maxStack, int streakCount, float streakTimeout, float cooldownTime)
		{
			this.cooldownTime = cooldownTime;
			this.maxStack = maxStack;
			this.streak = new Streak(streakCount, streakTimeout);
			this._updateReference.Stop();
			this._updateReference = CoroutineProxy.instance.StartCoroutineWithReference(this.CUpdate());
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x00093D47 File Offset: 0x00091F47
		private void OnDestroy()
		{
			this._updateReference.Stop();
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x00093D54 File Offset: 0x00091F54
		private IEnumerator CUpdate()
		{
			for (;;)
			{
				yield return null;
				if (this.stacks < this.maxStack && this.streak.remains <= 0)
				{
					if (this.remainTime > this.cooldownTime)
					{
						this.remainTime = this.cooldownTime;
					}
					this.remainTime -= Chronometer.global.deltaTime * this.GetCooldownSpeed();
					if (this.remainTime <= 0f)
					{
						this.remainTime = this.cooldownTime;
						int stacks = this.stacks;
						this.stacks = stacks + 1;
					}
				}
			}
			yield break;
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x00093D64 File Offset: 0x00091F64
		public bool Consume()
		{
			if (this.streak.Consume())
			{
				return true;
			}
			if (this.stacks > 0)
			{
				int stacks = this.stacks;
				this.stacks = stacks - 1;
				this.streak.Start();
				return true;
			}
			return false;
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x00093DA7 File Offset: 0x00091FA7
		public void ReduceCooldown(float time)
		{
			if (this.stacks == this.maxStack)
			{
				return;
			}
			this.remainTime -= time;
			if (this.remainTime < 0f)
			{
				this.remainTime = 0f;
			}
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x00093DDE File Offset: 0x00091FDE
		public void ReduceCooldownPercent(float percent)
		{
			if (this.stacks == this.maxStack)
			{
				return;
			}
			this.remainTime -= this.cooldownTime * percent;
			if (this.remainTime < 0f)
			{
				this.remainTime = 0f;
			}
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x00093E1C File Offset: 0x0009201C
		public void Copy(Time time)
		{
			this._stacks = time._stacks;
			this.remainTime = time.remainTime;
		}

		// Token: 0x040028A9 RID: 10409
		private int _stacks;

		// Token: 0x040028AA RID: 10410
		private Action _onReady;

		// Token: 0x040028AD RID: 10413
		public static readonly Func<float> GetDefaultCooldownSpeed = () => 1f;

		// Token: 0x040028AE RID: 10414
		public Func<float> GetCooldownSpeed = Time.GetDefaultCooldownSpeed;

		// Token: 0x040028AF RID: 10415
		public readonly float cooldownTime;

		// Token: 0x040028B0 RID: 10416
		private CoroutineReference _updateReference;

		// Token: 0x040028B1 RID: 10417
		public float remainTime;
	}
}
