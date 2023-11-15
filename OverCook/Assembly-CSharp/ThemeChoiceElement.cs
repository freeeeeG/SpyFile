using System;
using UnityEngine;

// Token: 0x02000ADA RID: 2778
public class ThemeChoiceElement : MonoBehaviour
{
	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x0600382B RID: 14379 RVA: 0x001089D1 File Offset: 0x00106DD1
	// (set) Token: 0x0600382C RID: 14380 RVA: 0x001089D9 File Offset: 0x00106DD9
	public SceneDirectoryData.LevelTheme Theme
	{
		get
		{
			return this.m_themeInfo;
		}
		set
		{
			this.m_themeInfo = value;
		}
	}

	// Token: 0x170003D2 RID: 978
	// (set) Token: 0x0600382D RID: 14381 RVA: 0x001089E2 File Offset: 0x00106DE2
	public Sprite Sprite
	{
		set
		{
			this.m_image.sprite = value;
			this.m_image.enabled = (value != null);
		}
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x00108A02 File Offset: 0x00106E02
	private void Start()
	{
		this.m_image.enabled = (this.m_image.sprite != null);
	}

	// Token: 0x0600382F RID: 14383 RVA: 0x00108A20 File Offset: 0x00106E20
	public void ShowAsSelected(bool _isSelected)
	{
		this.m_frameImage.sprite = ((!_isSelected) ? this.m_notSelectedSprite : this.m_selectedSprite);
	}

	// Token: 0x04002CEA RID: 11498
	[SerializeField]
	private T17Image m_image;

	// Token: 0x04002CEB RID: 11499
	[SerializeField]
	private T17Image m_frameImage;

	// Token: 0x04002CEC RID: 11500
	private SceneDirectoryData.LevelTheme m_themeInfo;

	// Token: 0x04002CED RID: 11501
	[SerializeField]
	private Sprite m_notSelectedSprite;

	// Token: 0x04002CEE RID: 11502
	[SerializeField]
	private Sprite m_selectedSprite;

	// Token: 0x04002CEF RID: 11503
	protected bool m_bIsInitialised;
}
