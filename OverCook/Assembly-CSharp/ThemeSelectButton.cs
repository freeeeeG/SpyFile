using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ADB RID: 2779
[RequireComponent(typeof(T17Button))]
public class ThemeSelectButton : CarouselButton
{
	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06003831 RID: 14385 RVA: 0x00108889 File Offset: 0x00106C89
	public Sprite ThemeSprite
	{
		get
		{
			if (this.m_themeImage != null)
			{
				return this.m_themeImage.sprite;
			}
			return null;
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06003832 RID: 14386 RVA: 0x001088A9 File Offset: 0x00106CA9
	public SceneDirectoryData.LevelTheme Theme
	{
		get
		{
			return this.m_theme;
		}
	}

	// Token: 0x04002CF0 RID: 11504
	[SerializeField]
	private Image m_themeImage;

	// Token: 0x04002CF1 RID: 11505
	[SerializeField]
	private SceneDirectoryData.LevelTheme m_theme;
}
