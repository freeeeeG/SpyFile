using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200011D RID: 285
	public class StatModHealth : MonoBehaviour
	{
		// Token: 0x060007DB RID: 2011 RVA: 0x000218F8 File Offset: 0x0001FAF8
		private void Start()
		{
			this.stats[StatType.MaxHP].ChangedEvent += this.OnStatChanged;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00021918 File Offset: 0x0001FB18
		private void OnDestroy()
		{
			this.stats[StatType.MaxHP].ChangedEvent -= this.OnStatChanged;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00021938 File Offset: 0x0001FB38
		private void OnStatChanged(object sender, EventArgs e)
		{
			this.health.maxHP = Mathf.Min(this.maxHP, Mathf.CeilToInt(this.stats[StatType.MaxHP].Modify((float)this.health.baseMaxHP)));
		}

		// Token: 0x040005B6 RID: 1462
		[SerializeField]
		private StatsHolder stats;

		// Token: 0x040005B7 RID: 1463
		[SerializeField]
		private PlayerHealth health;

		// Token: 0x040005B8 RID: 1464
		[SerializeField]
		private int maxHP = 20;
	}
}
