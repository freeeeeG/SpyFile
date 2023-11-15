using System;
using UnityEngine;

// Token: 0x020005F7 RID: 1527
public class BellowsSpray : SprayingUtensil
{
	// Token: 0x0400169C RID: 5788
	[SerializeField]
	public float m_heatIncrease;

	// Token: 0x0400169D RID: 5789
	[SerializeField]
	public BellowsSpray.KnockbackData m_knockback = new BellowsSpray.KnockbackData();

	// Token: 0x020005F8 RID: 1528
	[Serializable]
	public class KnockbackData
	{
		// Token: 0x0400169E RID: 5790
		[SerializeField]
		public float Force = 10f;

		// Token: 0x0400169F RID: 5791
		[SerializeField]
		public float Duration = 0.2f;
	}
}
