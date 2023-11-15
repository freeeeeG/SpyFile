using System;
using UnityEngine;

// Token: 0x020003C7 RID: 967
[RequireComponent(typeof(PickupItemSpawner))]
public class ItemCrateCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000DFD RID: 3581
	[SerializeField]
	public Transform m_cosmeticItemAttachpoint;

	// Token: 0x04000DFE RID: 3582
	[SerializeField]
	public string m_crateLidMeshName = "CrateLid_mesh";

	// Token: 0x04000DFF RID: 3583
	[SerializeField]
	public int m_materialNumber = 1;

	// Token: 0x04000E00 RID: 3584
	[SerializeField]
	public Vector2 m_uvScale = new Vector2(2f, 2f);
}
