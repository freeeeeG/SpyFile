using System;
using UnityEngine;

// Token: 0x020009A8 RID: 2472
public class HeatedStationGUI : MonoBehaviour
{
	// Token: 0x040026D9 RID: 9945
	[SerializeField]
	public HeatValueUIController m_heatUIPrefab;

	// Token: 0x040026DA RID: 9946
	[SerializeField]
	public bool m_displayWhenCold;

	// Token: 0x040026DB RID: 9947
	[SerializeField]
	public Vector3 m_Offset = Vector3.zero;
}
