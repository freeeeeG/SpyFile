using System;
using UnityEngine;

// Token: 0x0200038E RID: 910
[RequireComponent(typeof(AttachStation))]
[RequireComponent(typeof(HeatedCookingStation))]
public class CampfireCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000D53 RID: 3411
	[SerializeField]
	[AssignChildRecursive("High", Editorbility.NonEditable)]
	public GameObject m_highVisuals;

	// Token: 0x04000D54 RID: 3412
	[SerializeField]
	[AssignChildRecursive("Medium", Editorbility.NonEditable)]
	public GameObject m_mediumVisuals;

	// Token: 0x04000D55 RID: 3413
	[SerializeField]
	[AssignChildRecursive("Low", Editorbility.NonEditable)]
	public GameObject m_lowVisuals;
}
