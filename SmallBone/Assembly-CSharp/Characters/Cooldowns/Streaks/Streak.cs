using System;
using System.Collections;

namespace Characters.Cooldowns.Streaks
{
	// Token: 0x02000910 RID: 2320
	public class Streak : IStreak
	{
		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x060031B6 RID: 12726 RVA: 0x00093F3F File Offset: 0x0009213F
		// (set) Token: 0x060031B7 RID: 12727 RVA: 0x00093F47 File Offset: 0x00092147
		public int count { get; set; }

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x060031B8 RID: 12728 RVA: 0x00093F50 File Offset: 0x00092150
		// (set) Token: 0x060031B9 RID: 12729 RVA: 0x00093F58 File Offset: 0x00092158
		public float timeout { get; set; }

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x060031BA RID: 12730 RVA: 0x00093F61 File Offset: 0x00092161
		// (set) Token: 0x060031BB RID: 12731 RVA: 0x00093F69 File Offset: 0x00092169
		public int remains { get; private set; }

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060031BC RID: 12732 RVA: 0x00093F72 File Offset: 0x00092172
		public float remainPercent
		{
			get
			{
				return this._remainTime / this.timeout;
			}
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x00093F81 File Offset: 0x00092181
		public Streak(int count, float timeout)
		{
			this.count = count;
			this.timeout = timeout;
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x00093F98 File Offset: 0x00092198
		public bool Consume()
		{
			if (this.remains > 0)
			{
				int remains = this.remains;
				this.remains = remains - 1;
				return true;
			}
			return false;
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x00093FC1 File Offset: 0x000921C1
		public void Start()
		{
			if (this.count == 0)
			{
				return;
			}
			this._update.Stop();
			this._update = CoroutineProxy.instance.StartCoroutineWithReference(this.CUpdate());
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x00093FED File Offset: 0x000921ED
		private IEnumerator CUpdate()
		{
			this.remains = this.count;
			this._remainTime = this.timeout;
			Chronometer.Global chronometer = Chronometer.global;
			while (this._remainTime > 0f)
			{
				yield return null;
				this._remainTime -= chronometer.deltaTime;
			}
			this.remains = 0;
			yield break;
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x00093FFC File Offset: 0x000921FC
		public void Expire()
		{
			this._update.Stop();
			this.remains = 0;
			this._remainTime = 0f;
		}

		// Token: 0x040028B8 RID: 10424
		private float _remainTime;

		// Token: 0x040028B9 RID: 10425
		private CoroutineReference _update;
	}
}
