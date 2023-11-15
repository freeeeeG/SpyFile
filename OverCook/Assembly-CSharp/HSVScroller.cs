using System;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class HSVScroller : MonoBehaviour
{
	// Token: 0x060007D7 RID: 2007 RVA: 0x00030CDC File Offset: 0x0002F0DC
	private void Update()
	{
		if (this.m_material == null)
		{
			Renderer renderer = base.gameObject.RequireComponent<Renderer>();
			this.m_material = renderer.material;
		}
		else
		{
			Color.RGBToHSV(this.m_material.color, out this.m_hsvValue.x, out this.m_hsvValue.y, out this.m_hsvValue.z);
			this.m_hsvValue += this.m_scrollSpeed * TimeManager.GetDeltaTime(base.gameObject);
			this.m_material.color = Color.HSVToRGB(this.m_hsvValue.x, this.m_hsvValue.y, this.m_hsvValue.z);
		}
	}

	// Token: 0x04000638 RID: 1592
	[SerializeField]
	private Vector3 m_scrollSpeed;

	// Token: 0x04000639 RID: 1593
	private Material m_material;

	// Token: 0x0400063A RID: 1594
	private Vector3 m_hsvValue;
}
