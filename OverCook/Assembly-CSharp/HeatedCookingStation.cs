using System;
using UnityEngine;

// Token: 0x020004A9 RID: 1193
public class HeatedCookingStation : CookingStation
{
	// Token: 0x040010CD RID: 4301
	[Space]
	[AssignComponent(Editorbility.Editable)]
	[SerializeField]
	public HeatedStation m_heatSource;

	// Token: 0x040010CE RID: 4302
	[Range(0f, 1f)]
	[SerializeField]
	public float m_cookingSpeedHigh = 1f;

	// Token: 0x040010CF RID: 4303
	[Range(0f, 1f)]
	[SerializeField]
	public float m_cookingSpeedModerate = 0.66f;

	// Token: 0x040010D0 RID: 4304
	[Range(0f, 1f)]
	[SerializeField]
	public float m_cookingSpeedLow = 0.33f;
}
