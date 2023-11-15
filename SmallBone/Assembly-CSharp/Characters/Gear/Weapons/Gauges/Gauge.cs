using System;
using UnityEngine;

namespace Characters.Gear.Weapons.Gauges
{
	// Token: 0x0200083A RID: 2106
	public abstract class Gauge : MonoBehaviour
	{
		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06002BB7 RID: 11191
		public abstract float gaugePercent { get; }

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06002BB8 RID: 11192
		public abstract string displayText { get; }

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06002BB9 RID: 11193
		public abstract Color barColor { get; }

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06002BBA RID: 11194
		public abstract bool secondBar { get; }

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06002BBB RID: 11195
		public abstract Color secondBarColor { get; }

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06002BBC RID: 11196
		public abstract Color textColor { get; }

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06002BBD RID: 11197
		public abstract Gauge.GaugeInfo defaultBarGaugeColor { get; }

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06002BBE RID: 11198
		public abstract Gauge.GaugeInfo secondBarGaugeColor { get; }

		// Token: 0x0200083B RID: 2107
		[Serializable]
		public class GaugeInfo
		{
			// Token: 0x0400250D RID: 9485
			[SerializeField]
			[Range(0f, 1f)]
			internal float proportion;

			// Token: 0x0400250E RID: 9486
			[SerializeField]
			internal Color baseColor;

			// Token: 0x0400250F RID: 9487
			[SerializeField]
			internal bool useChargedColor;

			// Token: 0x04002510 RID: 9488
			[SerializeField]
			internal Color chargedColor;
		}
	}
}
