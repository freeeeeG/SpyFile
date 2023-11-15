using System;
using UnityEngine;

// Token: 0x020001CD RID: 461
public class MaterialScroller : MonoBehaviour
{
	// Token: 0x060007E4 RID: 2020 RVA: 0x00030EB0 File Offset: 0x0002F2B0
	private void Update()
	{
		this.m_scrollingMaterial.mainTextureOffset += TimeManager.GetDeltaTime(base.gameObject) * this.m_scrollSpeed;
	}

	// Token: 0x04000648 RID: 1608
	[SerializeField]
	private Material m_scrollingMaterial;

	// Token: 0x04000649 RID: 1609
	[SerializeField]
	private Vector2 m_scrollSpeed;
}
