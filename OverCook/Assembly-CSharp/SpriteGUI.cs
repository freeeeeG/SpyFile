using System;
using UnityEngine;

// Token: 0x020009B6 RID: 2486
public class SpriteGUI : MonoBehaviour
{
	// Token: 0x060030B1 RID: 12465 RVA: 0x000E4C43 File Offset: 0x000E3043
	public void SetData(GUIRect _rect, SubTexture2D _texture, bool _screenSpace)
	{
		this.m_rect = _rect;
		this.m_texture = _texture;
		this.m_screenSpace = _screenSpace;
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x000E4C5C File Offset: 0x000E305C
	private void OnGUI()
	{
		Rect rect;
		if (this.m_screenSpace)
		{
			rect = GUIUtils.ToPixels(this.m_rect, Camera.main);
		}
		else
		{
			rect = GUIUtils.ToPixels(this.m_rect, Camera.main, base.transform.position);
		}
		this.m_texture.Draw(rect, 0f);
	}

	// Token: 0x0400271C RID: 10012
	[SerializeField]
	private GUIRect m_rect;

	// Token: 0x0400271D RID: 10013
	[SerializeField]
	private SubTexture2D m_texture;

	// Token: 0x0400271E RID: 10014
	[SerializeField]
	private bool m_screenSpace;
}
