using System;
using UnityEngine;

// Token: 0x0200015C RID: 348
[ExecuteInEditMode]
public class TextScale : MonoBehaviour
{
	// Token: 0x0600062A RID: 1578 RVA: 0x0002C1F6 File Offset: 0x0002A5F6
	private void Awake()
	{
		this.UpdateSize();
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0002C200 File Offset: 0x0002A600
	private void UpdateSize()
	{
		RectTransform rectTransform = base.transform.parent as RectTransform;
		Vector3[] array = new Vector3[4];
		float num = (float)Camera.main.pixelWidth * (rectTransform.anchorMax - rectTransform.anchorMin).x;
		float d = num / this.m_baseParentSize;
		base.transform.localScale = this.m_baseScale * d;
	}

	// Token: 0x0400051E RID: 1310
	[SerializeField]
	private Vector3 m_baseScale = new Vector3(1f, 1f, 1f);

	// Token: 0x0400051F RID: 1311
	[SerializeField]
	private float m_baseParentSize = 0.3f;

	// Token: 0x04000520 RID: 1312
	private RectTransform m_rectTransform;
}
