using System;
using UnityEngine;

// Token: 0x0200037B RID: 891
[RequireComponent(typeof(Backpack))]
public class BackpackCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000D10 RID: 3344
	[SerializeField]
	[AssignResource("HoverIconUI", Editorbility.Editable)]
	public GameObject m_contentsHoverIconPrefab;

	// Token: 0x04000D11 RID: 3345
	[SerializeField]
	public Transform m_hoverIconTarget;

	// Token: 0x04000D12 RID: 3346
	[SerializeField]
	public Vector3 m_offset = Vector3.zero;
}
