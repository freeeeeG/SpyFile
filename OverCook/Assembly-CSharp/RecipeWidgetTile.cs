using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AF7 RID: 2807
public class RecipeWidgetTile : UISubElementContainer
{
	// Token: 0x060038D3 RID: 14547 RVA: 0x0010C1EB File Offset: 0x0010A5EB
	public void SetupTile(RecipeWidgetTile.TileDefinition _tileDefinition, RecipeWidgetTile.DisplayConfiguration _displayConfig, RecipeWidgetUIController.Size _sizex, RecipeWidgetUIController.Size _sizey)
	{
		this.m_tileDefinition = _tileDefinition;
		this.m_displayConfig = _displayConfig;
		this.m_gridSizeX = _sizex;
		this.m_gridSizeY = _sizey;
	}

	// Token: 0x060038D4 RID: 14548 RVA: 0x0010C20C File Offset: 0x0010A60C
	protected override void OnCreateSubObjects(GameObject _container)
	{
		this.m_background = base.CreateImage(_container, "RecipeWidgetTile_Background");
		this.m_background.type = Image.Type.Sliced;
		if (this.m_tileDefinition.m_backgroundTop)
		{
			this.m_backgroundTop = base.CreateImage(this.m_background.gameObject, "RecipeWidgetTile_BackgroundTop");
			this.m_backgroundTop.type = Image.Type.Sliced;
		}
		RectTransform rectTransform = base.CreateRect(_container, "RecipeWidgetTile_GridContainer");
		UIUtils.SetupFillParentAreaRect(rectTransform);
		rectTransform.offsetMin = new Vector2(this.m_gridSizeX.PixelOffset * 0.5f, this.m_gridSizeY.PixelOffset * 0.5f);
		rectTransform.offsetMax = new Vector2(-this.m_gridSizeX.PixelOffset * 0.5f, -(this.m_gridSizeY.PixelOffset * 0.5f));
		this.m_mainImages = new Image[this.m_tileDefinition.m_mainPictures.Count];
		for (int i = 0; i < this.m_mainImages.Length; i++)
		{
			this.m_mainImages[i] = base.CreateImage(rectTransform.gameObject, "RecipeWidgetTile_MainImage_" + i);
		}
		if (this.m_tileDefinition.m_modifierPictures != null && this.m_tileDefinition.m_modifierPictures.Count > 0)
		{
			this.m_modifierImages = new Image[this.m_tileDefinition.m_modifierPictures.Count];
			for (int j = 0; j < this.m_modifierImages.Length; j++)
			{
				this.m_modifierImages[j] = base.CreateImage(rectTransform.gameObject, "RecipeWidgetTile_ModifierImage_" + j);
			}
		}
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x0010C3B4 File Offset: 0x0010A7B4
	protected override void OnRefreshSubObjectProperties(GameObject _container)
	{
		this.m_background.sprite = this.m_displayConfig.m_background;
		this.m_background.color = this.m_displayConfig.m_tint;
		for (int i = 0; i < this.m_mainImages.Length; i++)
		{
			this.m_mainImages[i].sprite = this.m_tileDefinition.m_mainPictures[i];
		}
		if (this.m_modifierImages != null)
		{
			for (int j = 0; j < this.m_modifierImages.Length; j++)
			{
				this.m_modifierImages[j].sprite = this.m_tileDefinition.m_modifierPictures[j];
			}
		}
		UIUtils.SetupFillParentAreaRect(this.m_background.gameObject.RequireComponent<RectTransform>());
		if (this.m_backgroundTop != null)
		{
			this.m_backgroundTop.sprite = this.m_displayConfig.m_background;
			this.m_backgroundTop.color = this.m_displayConfig.m_tint;
			RectTransform rectTransform = this.m_backgroundTop.gameObject.RequireComponent<RectTransform>();
			UIUtils.SetupFillParentAreaRect(rectTransform);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.localScale = new Vector3(1f, -1f, 1f);
			rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
			rectTransform.sizeDelta = this.m_displayConfig.m_BackgroundStitchSize;
			rectTransform.offsetMax = new Vector2(0f, rectTransform.offsetMax.y);
			rectTransform.offsetMin = new Vector2(0f, rectTransform.offsetMin.y);
		}
		int num = (!this.m_tileDefinition.m_largeIcon) ? 1 : 2;
		for (int k = 0; k < this.m_mainImages.Length; k++)
		{
			float x = (float)k + (float)(this.m_gridSizeX.GridTiles - this.m_mainImages.Length) * 0.5f - 0.5f * (float)(num - 1);
			this.SetRectForGridPos(this.m_mainImages[k].gameObject.RequireComponent<RectTransform>(), x, (float)(this.m_gridSizeY.GridTiles - num), (float)num, (float)num);
		}
		if (this.m_modifierImages != null)
		{
			for (int l = 0; l < this.m_modifierImages.Length; l++)
			{
				float x2 = (float)l + (float)(this.m_gridSizeX.GridTiles - this.m_modifierImages.Length) * 0.5f;
				this.SetRectForGridPos(this.m_modifierImages[l].gameObject.RequireComponent<RectTransform>(), x2, 0f, 1f, 1f);
			}
		}
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x0010C69C File Offset: 0x0010AA9C
	private void SetRectForGridPos(RectTransform _rect, float _x, float _y, float _sizeX, float _sizeY)
	{
		Vector2 vector = new Vector2(1f / (float)this.m_gridSizeX.GridTiles, 1f / (float)this.m_gridSizeY.GridTiles);
		_x = Mathf.Clamp(_x, 0f, (float)this.m_gridSizeX.GridTiles - 1f);
		_y = Mathf.Clamp(_y, 0f, (float)this.m_gridSizeY.GridTiles - 1f);
		_rect.anchorMin = new Vector2(vector.x * _x, vector.y * _y);
		_rect.anchorMax = _rect.anchorMin + new Vector2(_sizeX * vector.x, _sizeY * vector.y);
		_rect.pivot = new Vector2(0f, 1f);
		float iconInsetBorder = this.m_displayConfig.m_iconInsetBorder;
		_rect.offsetMin = new Vector2(iconInsetBorder, iconInsetBorder);
		_rect.offsetMax = new Vector2(-iconInsetBorder, -iconInsetBorder);
		_rect.localScale = Vector3.one;
	}

	// Token: 0x04002D7C RID: 11644
	[SerializeField]
	[HideInInspector]
	protected Image m_background;

	// Token: 0x04002D7D RID: 11645
	[SerializeField]
	[HideInInspector]
	protected Image m_backgroundTop;

	// Token: 0x04002D7E RID: 11646
	[SerializeField]
	[HideInInspector]
	protected Image[] m_mainImages;

	// Token: 0x04002D7F RID: 11647
	[SerializeField]
	[HideInInspector]
	protected Image[] m_modifierImages;

	// Token: 0x04002D80 RID: 11648
	protected RecipeWidgetTile.TileDefinition m_tileDefinition;

	// Token: 0x04002D81 RID: 11649
	protected RecipeWidgetTile.DisplayConfiguration m_displayConfig;

	// Token: 0x04002D82 RID: 11650
	private RecipeWidgetUIController.Size m_gridSizeX;

	// Token: 0x04002D83 RID: 11651
	private RecipeWidgetUIController.Size m_gridSizeY;

	// Token: 0x02000AF8 RID: 2808
	[Serializable]
	public class TileDefinition
	{
		// Token: 0x04002D84 RID: 11652
		public List<Sprite> m_mainPictures = new List<Sprite>();

		// Token: 0x04002D85 RID: 11653
		public List<Sprite> m_modifierPictures = new List<Sprite>();

		// Token: 0x04002D86 RID: 11654
		[HideInInspector]
		public bool m_largeIcon;

		// Token: 0x04002D87 RID: 11655
		[HideInInspector]
		public bool m_backgroundTop = true;
	}

	// Token: 0x02000AF9 RID: 2809
	public enum Axis
	{
		// Token: 0x04002D89 RID: 11657
		X,
		// Token: 0x04002D8A RID: 11658
		Y
	}

	// Token: 0x02000AFA RID: 2810
	[Serializable]
	public class DisplayConfiguration
	{
		// Token: 0x04002D8B RID: 11659
		public Sprite m_background;

		// Token: 0x04002D8C RID: 11660
		public Color m_tint = Color.white;

		// Token: 0x04002D8D RID: 11661
		public float m_iconInsetBorder = 5f;

		// Token: 0x04002D8E RID: 11662
		public float m_levelInset = 10f;

		// Token: 0x04002D8F RID: 11663
		public float m_tileSpacing = 1f;

		// Token: 0x04002D90 RID: 11664
		public float m_yExtension = 5f;

		// Token: 0x04002D91 RID: 11665
		public Vector2 m_BackgroundStitchSize = new Vector2(0f, 53f);

		// Token: 0x04002D92 RID: 11666
		[Header("Fixed")]
		[Mask(typeof(RecipeWidgetTile.Axis))]
		public int m_fixedSizeAxis;

		// Token: 0x04002D93 RID: 11667
		public Vector2Int m_fixedSizeGridCells = Vector2Int.zero;

		// Token: 0x04002D94 RID: 11668
		public Vector2 m_fixedSizeOffset = Vector2.zero;

		// Token: 0x04002D95 RID: 11669
		[Mask(typeof(RecipeWidgetTile.Axis))]
		public int m_fixedPosAxis;

		// Token: 0x04002D96 RID: 11670
		public Vector2Int m_fixedPosGridCells = Vector2Int.zero;

		// Token: 0x04002D97 RID: 11671
		public Vector2 m_fixedPosOffset = Vector2.zero;
	}
}
