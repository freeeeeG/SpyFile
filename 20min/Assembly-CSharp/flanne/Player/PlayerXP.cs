using System;
using UnityEngine;

namespace flanne.Player
{
	// Token: 0x0200015F RID: 351
	public class PlayerXP : MonoBehaviour
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x000253CC File Offset: 0x000235CC
		// (set) Token: 0x060008EE RID: 2286 RVA: 0x000253D4 File Offset: 0x000235D4
		public int level { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x000253E0 File Offset: 0x000235E0
		private float xpToLevel
		{
			get
			{
				float num = (float)(this.level + 1);
				if (num < 20f)
				{
					return num * 10f - 5f;
				}
				if (num < 40f)
				{
					return num * 13f - 6f;
				}
				if (num < 60f)
				{
					return num * 16f - 8f;
				}
				return num * num;
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0002543C File Offset: 0x0002363C
		private void Awake()
		{
			this.xp = 0f;
			this.level = 1;
			this.OnXPToLevelChanged.Invoke(this.xpToLevel);
			this.xpMultiplier = new StatMod();
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002546C File Offset: 0x0002366C
		public void GainXP(float amount)
		{
			this.xp += this.xpMultiplier.Modify(amount);
			if (this.xp > this.xpToLevel)
			{
				this.xp -= this.xpToLevel;
				int level = this.level;
				this.level = level + 1;
				this.OnLevelChanged.Invoke(this.level);
				this.OnXPToLevelChanged.Invoke(this.xpToLevel);
			}
			this.OnXPChanged.Invoke(this.xp);
		}

		// Token: 0x040006A0 RID: 1696
		public UnityFloatEvent OnXPChanged;

		// Token: 0x040006A1 RID: 1697
		public UnityFloatEvent OnXPToLevelChanged;

		// Token: 0x040006A2 RID: 1698
		public UnityIntEvent OnLevelChanged;

		// Token: 0x040006A3 RID: 1699
		public StatMod xpMultiplier;

		// Token: 0x040006A5 RID: 1701
		private float xp;
	}
}
