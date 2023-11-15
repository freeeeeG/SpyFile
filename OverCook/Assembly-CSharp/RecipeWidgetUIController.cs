using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B45 RID: 2885
[RequireComponent(typeof(RectTransform))]
public class RecipeWidgetUIController : UIControllerAndContainer
{
	// Token: 0x06003AA0 RID: 15008 RVA: 0x0011713B File Offset: 0x0011553B
	public void PlayAnimation(WidgetAnimation _animation)
	{
		this.m_animation = _animation;
		_animation.Init(this.m_widgetAnimator);
	}

	// Token: 0x06003AA1 RID: 15009 RVA: 0x00117150 File Offset: 0x00115550
	public bool IsPlayingAnimation()
	{
		return this.m_animation != null;
	}

	// Token: 0x06003AA2 RID: 15010 RVA: 0x0011715E File Offset: 0x0011555E
	public int GetTableNumber()
	{
		return this.m_tableNumber;
	}

	// Token: 0x06003AA3 RID: 15011 RVA: 0x00117168 File Offset: 0x00115568
	public void SetTimePropRemaining(float _propLeft)
	{
		if (this.m_topWidgetTile != null)
		{
			this.m_topWidgetTile.SetProgress(_propLeft);
		}
		if (this.m_widgetAnimator != null)
		{
			this.m_widgetAnimator.SetBool(RecipeWidgetUIController.m_iWarning, _propLeft < 0.2f);
		}
	}

	// Token: 0x06003AA4 RID: 15012 RVA: 0x001171BB File Offset: 0x001155BB
	public float GetTimePropRemaining()
	{
		if (this.m_topWidgetTile != null)
		{
			return this.m_topWidgetTile.GetProgress();
		}
		return 0f;
	}

	// Token: 0x06003AA5 RID: 15013 RVA: 0x001171DF File Offset: 0x001155DF
	public void SetIsStuck()
	{
	}

	// Token: 0x06003AA6 RID: 15014 RVA: 0x001171E4 File Offset: 0x001155E4
	public Rect GetBounds()
	{
		if (this.m_recipeTiles.Count > 0)
		{
			RecipeWidgetTile recipeWidgetTile = this.m_recipeTiles[this.m_recipeTiles.Count - 1];
			if (recipeWidgetTile != null)
			{
				RectTransform rectTransform = recipeWidgetTile.gameObject.RequestComponent<RectTransform>();
				if (rectTransform != null)
				{
					return rectTransform.rect;
				}
			}
		}
		return default(Rect);
	}

	// Token: 0x06003AA7 RID: 15015 RVA: 0x00117250 File Offset: 0x00115650
	protected void OnPauseMenuVisibilityChange(BaseMenuBehaviour _menu)
	{
		Image[] array = base.gameObject.RequestComponentsRecursive<Image>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = !_menu.isActiveAndEnabled;
		}
	}

	// Token: 0x06003AA8 RID: 15016 RVA: 0x00117290 File Offset: 0x00115690
	public void SetupFromOrderDefinition(OrderDefinitionNode _data, int _tableNumber)
	{
		this.m_recipeTree = _data.m_orderGuiDescription;
		this.m_tableNumber = _tableNumber;
		base.StartCoroutine(this.RefreshSubObjectsAtEndOfFrame());
		if (T17InGameFlow.Instance != null)
		{
			T17InGameFlow.Instance.RegisterOnPauseMenuVisibilityChanged(new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnPauseMenuVisibilityChange));
		}
	}

	// Token: 0x06003AA9 RID: 15017 RVA: 0x001172E4 File Offset: 0x001156E4
	private IEnumerator RefreshSubObjectsAtEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		base.RefreshSubElements();
		yield break;
	}

	// Token: 0x06003AAA RID: 15018 RVA: 0x00117300 File Offset: 0x00115700
	protected override void OnCreateSubObjects(GameObject _container)
	{
		this.m_widgetAnimator = _container.AddComponent<Animator>();
		this.m_widgetAnimator.runtimeAnimatorController = this.m_widgetAnimatorController;
		this.m_recipeTiles.Clear();
		RecipeWidgetUIController.Size?[] array = this.BuildTileSizes();
		RecipeWidgetUIController.Position?[] array2 = this.BuildTilePositions(array);
		RecipeWidgetUIController.RecipeTileData recipeTileData = this.FindHead(this.m_recipeTree);
		for (int i = 0; i < this.m_recipeTree.Length; i++)
		{
			RecipeWidgetUIController.RecipeTileData recipeTileData2 = this.m_recipeTree[i];
			GameObject gameObject = GameObjectUtils.CreateOnParent<RectTransform>(_container, "Tile " + i);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			RecipeWidgetUIController.Size value = array[i].Value;
			RecipeWidgetUIController.Position value2 = array2[i].Value;
			RecipeWidgetUIController.Size sizey = this.CalculateSizeY(i);
			component.anchorMin = new Vector2((float)value2.X.GridTiles * 0.5f, -(float)(value2.Y.GridTiles + sizey.GridTiles) * 0.5f);
			component.anchorMax = new Vector2((float)(value2.X.GridTiles + value.GridTiles) * 0.5f, -(float)value2.Y.GridTiles * 0.5f);
			component.pivot = new Vector2(0f, 1f);
			component.offsetMin = new Vector2(value2.X.PixelOffset, 0f);
			component.offsetMax = new Vector2(value2.X.PixelOffset + value.PixelOffset, sizey.PixelOffset);
			component.localScale = Vector3.one;
			recipeTileData2.m_tileDefinition.m_largeIcon = (recipeTileData2.m_children.Count > 0);
			RecipeWidgetTile recipeWidgetTile;
			if (recipeTileData2 == recipeTileData)
			{
				recipeWidgetTile = gameObject.AddComponent<TopRecipeWidgetTile>();
				recipeWidgetTile.SetupTile(recipeTileData2.m_tileDefinition, this.m_topDisplayConfig, value, sizey);
				this.m_topWidgetTile = (recipeWidgetTile as TopRecipeWidgetTile);
				RectTransform rectTransform = _container.transform as RectTransform;
				rectTransform.pivot = VectorUtils.Select(0.5f * (component.anchorMin + component.anchorMax), component.anchorMax, true, false);
			}
			else
			{
				recipeWidgetTile = gameObject.AddComponent<RecipeWidgetTile>();
				recipeWidgetTile.SetupTile(recipeTileData2.m_tileDefinition, this.m_displayConfig, value, sizey);
			}
			this.m_recipeTiles.Add(recipeWidgetTile);
		}
		this.m_recipeTiles.Sort(delegate(RecipeWidgetTile _t1, RecipeWidgetTile _t2)
		{
			RectTransform rectTransform2 = _t1.transform as RectTransform;
			RectTransform rectTransform3 = _t2.transform as RectTransform;
			if (rectTransform3.anchorMin.y < rectTransform2.anchorMin.y)
			{
				return 1;
			}
			if (rectTransform3.anchorMin.y == rectTransform2.anchorMin.y)
			{
				return 0;
			}
			return -1;
		});
		for (int j = 0; j < this.m_recipeTiles.Count; j++)
		{
			this.m_recipeTiles[j].transform.SetSiblingIndex(j);
		}
		if (Application.isPlaying)
		{
			for (int k = 0; k < this.m_recipeTiles.Count; k++)
			{
				this.m_recipeTiles[k].RefreshSubElements();
			}
		}
		UI_Move ui_Move = _container.AddComponent<UI_Move>();
		ui_Move.m_UIMaterial = this.m_UIMoveMaterial;
		ui_Move.UpdateGraphics();
	}

	// Token: 0x06003AAB RID: 15019 RVA: 0x00117614 File Offset: 0x00115A14
	public void Update()
	{
		if (this.m_animation != null)
		{
			Color b = Color.white;
			this.m_animation.Advance(TimeManager.GetDeltaTime(base.gameObject));
			if (!this.m_animation.IsFinished())
			{
				b = this.m_animation.GetColourModifier();
			}
			else
			{
				this.m_animation = null;
			}
			foreach (Image image in base.gameObject.RequestComponentsRecursive<Image>())
			{
				Color a = Color.white;
				if (image.sprite == this.m_topDisplayConfig.m_background)
				{
					a = this.m_topDisplayConfig.m_tint;
				}
				else if (image.sprite == this.m_displayConfig.m_background)
				{
					a = this.m_displayConfig.m_tint;
				}
				else if (image.sprite == this.m_topDisplayConfig.lowTipSprite || image.sprite == this.m_topDisplayConfig.highTipSprite)
				{
					a = this.m_topDisplayConfig.notchColor;
				}
				image.color = a * b;
			}
		}
	}

	// Token: 0x06003AAC RID: 15020 RVA: 0x00117748 File Offset: 0x00115B48
	private RecipeWidgetUIController.Size?[] BuildTileSizes()
	{
		RecipeWidgetUIController.Size?[] array = new RecipeWidgetUIController.Size?[this.m_recipeTree.Length];
		for (int i = 0; i < this.m_recipeTree.Length; i++)
		{
			this.CalculateSize(i, array);
		}
		return array;
	}

	// Token: 0x06003AAD RID: 15021 RVA: 0x00117788 File Offset: 0x00115B88
	private RecipeWidgetUIController.Position?[] BuildTilePositions(RecipeWidgetUIController.Size?[] _sizes)
	{
		RecipeWidgetUIController.Position?[] array = new RecipeWidgetUIController.Position?[this.m_recipeTree.Length];
		for (int i = 0; i < this.m_recipeTree.Length; i++)
		{
			this.CalculatePosition(i, array, _sizes);
		}
		return array;
	}

	// Token: 0x06003AAE RID: 15022 RVA: 0x001177C8 File Offset: 0x00115BC8
	private RecipeWidgetUIController.Size CalculateSize(int _tileIdx, RecipeWidgetUIController.Size?[] _cachedSizes)
	{
		RecipeWidgetUIController.RecipeTileData recipeTileData = this.m_recipeTree[_tileIdx];
		if (_cachedSizes[_tileIdx] != null)
		{
			return _cachedSizes[_tileIdx].Value;
		}
		RecipeWidgetUIController.Size size;
		if (recipeTileData.m_children.Count > 0)
		{
			size = new RecipeWidgetUIController.Size(0, 2f * this.m_displayConfig.m_levelInset + this.m_displayConfig.m_tileSpacing * (float)(recipeTileData.m_children.Count - 1));
			for (int i = 0; i < recipeTileData.m_children.Count; i++)
			{
				RecipeWidgetUIController.Size size2 = this.CalculateSize(recipeTileData.m_children[i], _cachedSizes);
				size.GridTiles += size2.GridTiles;
				size.PixelOffset += size2.PixelOffset;
			}
			size.GridTiles = Mathf.Max(size.GridTiles, 2);
		}
		else
		{
			int num = recipeTileData.m_tileDefinition.m_mainPictures.Count;
			if (recipeTileData.m_tileDefinition.m_modifierPictures != null)
			{
				num = Mathf.Max(num, recipeTileData.m_tileDefinition.m_modifierPictures.Count);
			}
			size = new RecipeWidgetUIController.Size(num, 2f * this.m_displayConfig.m_levelInset);
		}
		_cachedSizes[_tileIdx] = new RecipeWidgetUIController.Size?(size);
		return size;
	}

	// Token: 0x06003AAF RID: 15023 RVA: 0x0011791C File Offset: 0x00115D1C
	private RecipeWidgetUIController.Position CalculatePosition(int _tileIdx, RecipeWidgetUIController.Position?[] _cachedPositions, RecipeWidgetUIController.Size?[] _sizes)
	{
		RecipeWidgetUIController.RecipeTileData recipeTileData = this.m_recipeTree[_tileIdx];
		if (_cachedPositions[_tileIdx] != null)
		{
			return _cachedPositions[_tileIdx].Value;
		}
		int parentIndex = this.GetParentIndex(_tileIdx);
		RecipeWidgetUIController.Position position;
		if (parentIndex != -1)
		{
			position = this.CalculatePosition(parentIndex, _cachedPositions, _sizes);
			position.X.PixelOffset = position.X.PixelOffset + this.m_displayConfig.m_levelInset;
			position.Y.GridTiles = position.Y.GridTiles + this.CalculateSizeY(parentIndex).GridTiles;
			RecipeWidgetUIController.RecipeTileData recipeTileData2 = this.m_recipeTree[parentIndex];
			for (int i = 0; i < recipeTileData2.m_children.Count; i++)
			{
				int num = recipeTileData2.m_children[i];
				if (num == _tileIdx)
				{
					break;
				}
				position.X.GridTiles = position.X.GridTiles + _sizes[num].Value.GridTiles;
				position.X.PixelOffset = position.X.PixelOffset + (_sizes[num].Value.PixelOffset + this.m_displayConfig.m_tileSpacing);
			}
		}
		else
		{
			position = new RecipeWidgetUIController.Position(new RecipeWidgetUIController.Size(0, 0f), new RecipeWidgetUIController.Size(-2, 0f));
		}
		_cachedPositions[_tileIdx] = new RecipeWidgetUIController.Position?(position);
		return position;
	}

	// Token: 0x06003AB0 RID: 15024 RVA: 0x00117A8C File Offset: 0x00115E8C
	private RecipeWidgetUIController.RecipeTileData FindHead(RecipeWidgetUIController.RecipeTileData[] _tileData)
	{
		bool[] array = new bool[_tileData.Length];
		for (int i = 0; i < _tileData.Length; i++)
		{
			for (int j = 0; j < _tileData[i].m_children.Count; j++)
			{
				array[_tileData[i].m_children[j]] = true;
			}
		}
		for (int k = 0; k < array.Length; k++)
		{
			if (!array[k])
			{
				return _tileData[k];
			}
		}
		return null;
	}

	// Token: 0x06003AB1 RID: 15025 RVA: 0x00117B08 File Offset: 0x00115F08
	private RecipeWidgetUIController.Size CalculateSizeY(int _tileIdx)
	{
		RecipeWidgetUIController.RecipeTileData recipeTileData = this.m_recipeTree[_tileIdx];
		bool flag = recipeTileData.m_children.Count > 0;
		bool flag2 = recipeTileData.m_tileDefinition.m_modifierPictures != null && recipeTileData.m_tileDefinition.m_modifierPictures.Count > 0;
		return new RecipeWidgetUIController.Size((!flag && !flag2) ? 1 : 2, this.m_displayConfig.m_yExtension);
	}

	// Token: 0x06003AB2 RID: 15026 RVA: 0x00117B78 File Offset: 0x00115F78
	private int GetParentIndex(int _childIdx)
	{
		for (int i = 0; i < this.m_recipeTree.Length; i++)
		{
			RecipeWidgetUIController.RecipeTileData recipeTileData = this.m_recipeTree[i];
			for (int j = 0; j < recipeTileData.m_children.Count; j++)
			{
				if (recipeTileData.m_children[j] == _childIdx)
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x00117BD8 File Offset: 0x00115FD8
	protected override void OnRefreshSubObjectProperties(GameObject _container)
	{
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x00117BDA File Offset: 0x00115FDA
	protected virtual void OnDestroy()
	{
		if (T17InGameFlow.Instance != null)
		{
			T17InGameFlow.Instance.UnRegisterOnPauseMenuVisibilityChanged(new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnPauseMenuVisibilityChange));
		}
	}

	// Token: 0x04002F84 RID: 12164
	[SerializeField]
	private RecipeWidgetUIController.RecipeTileData[] m_recipeTree = new RecipeWidgetUIController.RecipeTileData[0];

	// Token: 0x04002F85 RID: 12165
	[SerializeField]
	private RecipeWidgetTile.DisplayConfiguration m_displayConfig = new RecipeWidgetTile.DisplayConfiguration();

	// Token: 0x04002F86 RID: 12166
	[SerializeField]
	private TopRecipeWidgetTile.TopDisplayConfiguration m_topDisplayConfig = new TopRecipeWidgetTile.TopDisplayConfiguration();

	// Token: 0x04002F87 RID: 12167
	[SerializeField]
	[HideInInspector]
	private List<RecipeWidgetTile> m_recipeTiles = new List<RecipeWidgetTile>();

	// Token: 0x04002F88 RID: 12168
	[SerializeField]
	private RuntimeAnimatorController m_widgetAnimatorController;

	// Token: 0x04002F89 RID: 12169
	[SerializeField]
	[AssignResource("UI_Move", Editorbility.Editable)]
	private Material m_UIMoveMaterial;

	// Token: 0x04002F8A RID: 12170
	private Animator m_widgetAnimator;

	// Token: 0x04002F8B RID: 12171
	private WidgetAnimation m_animation;

	// Token: 0x04002F8C RID: 12172
	private int m_tableNumber;

	// Token: 0x04002F8D RID: 12173
	private TopRecipeWidgetTile m_topWidgetTile;

	// Token: 0x04002F8E RID: 12174
	private static readonly int m_iWarning = Animator.StringToHash("Warning");

	// Token: 0x02000B46 RID: 2886
	[Serializable]
	public class RecipeTileData
	{
		// Token: 0x04002F90 RID: 12176
		public RecipeWidgetTile.TileDefinition m_tileDefinition;

		// Token: 0x04002F91 RID: 12177
		public List<int> m_children = new List<int>();
	}

	// Token: 0x02000B47 RID: 2887
	public struct Size
	{
		// Token: 0x06003AB8 RID: 15032 RVA: 0x00117C95 File Offset: 0x00116095
		public Size(int _grid, float _pixels)
		{
			this.GridTiles = _grid;
			this.PixelOffset = _pixels;
		}

		// Token: 0x04002F92 RID: 12178
		public int GridTiles;

		// Token: 0x04002F93 RID: 12179
		public float PixelOffset;
	}

	// Token: 0x02000B48 RID: 2888
	public struct Position
	{
		// Token: 0x06003AB9 RID: 15033 RVA: 0x00117CA5 File Offset: 0x001160A5
		public Position(RecipeWidgetUIController.Size _x, RecipeWidgetUIController.Size _y)
		{
			this.X = _x;
			this.Y = _y;
		}

		// Token: 0x04002F94 RID: 12180
		public RecipeWidgetUIController.Size X;

		// Token: 0x04002F95 RID: 12181
		public RecipeWidgetUIController.Size Y;
	}
}
