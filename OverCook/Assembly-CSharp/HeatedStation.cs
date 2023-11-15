using System;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public class HeatedStation : MonoBehaviour
{
	// Token: 0x0600166D RID: 5741 RVA: 0x00076B0A File Offset: 0x00074F0A
	public HeatRange GetHeat(float _heat)
	{
		if (_heat >= 0.66f)
		{
			return HeatRange.High;
		}
		if (_heat >= 0.33f)
		{
			return HeatRange.Moderate;
		}
		return HeatRange.Low;
	}

	// Token: 0x040010EB RID: 4331
	[SerializeField]
	public float m_dissipationRate;

	// Token: 0x040010EC RID: 4332
	[SerializeField]
	public StatValidationList m_burnAchievementFilter;

	// Token: 0x040010ED RID: 4333
	[AssignResource("DLC07_Coal", Editorbility.NonEditable)]
	[SerializeField]
	public ItemOrderNode m_coalOrderNode;

	// Token: 0x040010EE RID: 4334
	private const float c_heatThresholdHigh = 0.66f;

	// Token: 0x040010EF RID: 4335
	private const float c_heatThresholdModerate = 0.33f;
}
