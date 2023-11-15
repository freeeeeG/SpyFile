using System;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class InstanceMaterialScroller : MonoBehaviour
{
	// Token: 0x060007D9 RID: 2009 RVA: 0x00030DA8 File Offset: 0x0002F1A8
	private void Awake()
	{
	}

	// Token: 0x0400063B RID: 1595
	[SerializeField]
	private int m_materialIndex;

	// Token: 0x0400063C RID: 1596
	[SerializeField]
	private Vector2 m_scrollSpeed;

	// Token: 0x0400063D RID: 1597
	private MeshRenderer m_meshRenderer;

	// Token: 0x0400063E RID: 1598
	private Material m_material;
}
