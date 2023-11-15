using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200011E RID: 286
	public class StatModTransformScale : MonoBehaviour
	{
		// Token: 0x060007DF RID: 2015 RVA: 0x00021983 File Offset: 0x0001FB83
		private void Start()
		{
			this.stats[this.statType].ChangedEvent += this.OnStatChanged;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000219A7 File Offset: 0x0001FBA7
		private void OnDestroy()
		{
			this.stats[this.statType].ChangedEvent -= this.OnStatChanged;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000219CB File Offset: 0x0001FBCB
		private void OnStatChanged(object sender, EventArgs e)
		{
			base.transform.localScale = Vector3.one * Mathf.Min(this.maxScale, this.stats[this.statType].Modify(1f));
		}

		// Token: 0x040005B9 RID: 1465
		[SerializeField]
		private StatType statType;

		// Token: 0x040005BA RID: 1466
		[SerializeField]
		private StatsHolder stats;

		// Token: 0x040005BB RID: 1467
		[SerializeField]
		private float maxScale = float.PositiveInfinity;
	}
}
