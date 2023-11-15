using System;
using UnityEngine;

// Token: 0x0200054B RID: 1355
[AddComponentMenu("Scripts/Game/Environment/PlateReturnStation")]
[RequireComponent(typeof(AttachStation))]
public class PlateReturnStation : MonoBehaviour
{
	// Token: 0x04001449 RID: 5193
	[SerializeField]
	public GameObject m_stackPrefab;

	// Token: 0x0400144A RID: 5194
	[SerializeField]
	public int m_startingPlateNumber;
}
