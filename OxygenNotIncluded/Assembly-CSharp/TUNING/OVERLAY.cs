using System;
using STRINGS;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000D87 RID: 3463
	public class OVERLAY
	{
		// Token: 0x02001C74 RID: 7284
		public class TEMPERATURE_LEGEND
		{
			// Token: 0x04008114 RID: 33044
			public static readonly LegendEntry MAXHOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x04008115 RID: 33045
			public static readonly LegendEntry EXTREMEHOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x04008116 RID: 33046
			public static readonly LegendEntry VERYHOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0f, 0f), null, null, true);

			// Token: 0x04008117 RID: 33047
			public static readonly LegendEntry HOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 1f, 0f), null, null, true);

			// Token: 0x04008118 RID: 33048
			public static readonly LegendEntry TEMPERATE = new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x04008119 RID: 33049
			public static readonly LegendEntry COLD = new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 1f), null, null, true);

			// Token: 0x0400811A RID: 33050
			public static readonly LegendEntry VERYCOLD = new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 1f), null, null, true);

			// Token: 0x0400811B RID: 33051
			public static readonly LegendEntry EXTREMECOLD = new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);
		}

		// Token: 0x02001C75 RID: 7285
		public class HEATFLOW_LEGEND
		{
			// Token: 0x0400811C RID: 33052
			public static readonly LegendEntry HEATING = new LegendEntry(UI.OVERLAYS.HEATFLOW.HEATING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.HEATING, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x0400811D RID: 33053
			public static readonly LegendEntry NEUTRAL = new LegendEntry(UI.OVERLAYS.HEATFLOW.NEUTRAL, UI.OVERLAYS.HEATFLOW.TOOLTIPS.NEUTRAL, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x0400811E RID: 33054
			public static readonly LegendEntry COOLING = new LegendEntry(UI.OVERLAYS.HEATFLOW.COOLING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.COOLING, new Color(0f, 0f, 0f), null, null, true);
		}

		// Token: 0x02001C76 RID: 7286
		public class POWER_LEGEND
		{
			// Token: 0x0400811F RID: 33055
			public const float WATTAGE_WARNING_THRESHOLD = 0.75f;
		}
	}
}
