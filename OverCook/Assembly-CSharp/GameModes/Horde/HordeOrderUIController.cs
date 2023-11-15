using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameModes.Horde
{
	// Token: 0x020007BC RID: 1980
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class HordeOrderUIController : HoverIconUIController
	{
		// Token: 0x060025E9 RID: 9705 RVA: 0x000B3628 File Offset: 0x000B1A28
		public void Setup(Transform followTransform, RecipeWidgetUIController.RecipeTileData[] guiDescriptions, HordeOrderUIController.Align uiAnchor)
		{
			base.SetFollowTransform(followTransform);
			this.m_rectTransform = base.gameObject.RequireComponent<RectTransform>();
			this.m_rectTransformExtension = base.gameObject.RequireComponent<RectTransformExtension>();
			this.OnCreateSubObjects(base.gameObject, guiDescriptions, this.m_displayConfig, uiAnchor);
			this.m_rectTransformExtension.PixelOffset += this.m_offset;
			this.m_images = base.gameObject.RequestComponentsRecursive<Image>();
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000B369F File Offset: 0x000B1A9F
		public void PlayAnimation(WidgetAnimation animation)
		{
			this.m_animation = animation;
			this.m_animation.Init(this.m_animator);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000B36BC File Offset: 0x000B1ABC
		public void Update()
		{
			if (this.m_animation != null)
			{
				Color b = Color.white;
				Vector2 scale = Vector2.one;
				Vector2 b2 = Vector2.zero;
				float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
				this.m_animation.Advance(deltaTime);
				if (!this.m_animation.IsFinished())
				{
					b = this.m_animation.GetColourModifier();
					scale = this.m_animation.GetScaleModifier();
					b2 = ((deltaTime <= 0f) ? Vector2.zero : this.m_animation.GetPosModifier());
				}
				else
				{
					this.m_animation = null;
				}
				this.m_rectTransformExtension.PixelOffset += b2;
				this.m_rectTransformExtension.scale = scale;
				if (this.m_images != null && this.m_images.Length > 0)
				{
					for (int i = 0; i < this.m_images.Length; i++)
					{
						Image image = this.m_images[i];
						if (image != null)
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
			}
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000B387B File Offset: 0x000B1C7B
		public bool IsPlayingAnimation()
		{
			return this.m_animation != null;
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000B388C File Offset: 0x000B1C8C
		private RecipeWidgetUIController.Size?[] BuildTileSizes(RecipeWidgetUIController.RecipeTileData[] tileDatas, RecipeWidgetTile.DisplayConfiguration displayConfig)
		{
			RecipeWidgetUIController.Size?[] array = new RecipeWidgetUIController.Size?[tileDatas.Length];
			for (int i = 0; i < tileDatas.Length; i++)
			{
				this.CalculateSize(tileDatas, i, array, displayConfig);
			}
			return array;
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000B38C4 File Offset: 0x000B1CC4
		private RecipeWidgetUIController.Position?[] BuildTilePositions(RecipeWidgetUIController.RecipeTileData[] tileDatas, RecipeWidgetUIController.Size?[] _sizes, RecipeWidgetTile.DisplayConfiguration displayConfig)
		{
			RecipeWidgetUIController.Position?[] array = new RecipeWidgetUIController.Position?[tileDatas.Length];
			for (int i = 0; i < tileDatas.Length; i++)
			{
				this.CalculatePosition(tileDatas, i, array, _sizes, displayConfig);
			}
			return array;
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000B38FC File Offset: 0x000B1CFC
		private RecipeWidgetUIController.Size CalculateSize(RecipeWidgetUIController.RecipeTileData[] tileDatas, int _tileIdx, RecipeWidgetUIController.Size?[] _cachedSizes, RecipeWidgetTile.DisplayConfiguration displayConfig)
		{
			RecipeWidgetUIController.RecipeTileData recipeTileData = tileDatas[_tileIdx];
			if (_cachedSizes[_tileIdx] != null)
			{
				return _cachedSizes[_tileIdx].Value;
			}
			RecipeWidgetUIController.Size size;
			if (recipeTileData.m_children.Count > 0)
			{
				size = new RecipeWidgetUIController.Size(0, 2f * displayConfig.m_levelInset + displayConfig.m_tileSpacing * (float)(recipeTileData.m_children.Count - 1));
				for (int i = 0; i < recipeTileData.m_children.Count; i++)
				{
					RecipeWidgetUIController.Size size2 = this.CalculateSize(tileDatas, recipeTileData.m_children[i], _cachedSizes, displayConfig);
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
				size = new RecipeWidgetUIController.Size(num, 2f * displayConfig.m_levelInset);
			}
			_cachedSizes[_tileIdx] = new RecipeWidgetUIController.Size?(size);
			return size;
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000B3A44 File Offset: 0x000B1E44
		private RecipeWidgetUIController.Position CalculatePosition(RecipeWidgetUIController.RecipeTileData[] tileDatas, int _tileIdx, RecipeWidgetUIController.Position?[] _cachedPositions, RecipeWidgetUIController.Size?[] _sizes, RecipeWidgetTile.DisplayConfiguration displayConfig)
		{
			RecipeWidgetUIController.RecipeTileData recipeTileData = tileDatas[_tileIdx];
			if (_cachedPositions[_tileIdx] != null)
			{
				return _cachedPositions[_tileIdx].Value;
			}
			int parentIndex = this.GetParentIndex(tileDatas, _tileIdx);
			RecipeWidgetUIController.Position position;
			if (parentIndex != -1)
			{
				position = this.CalculatePosition(tileDatas, parentIndex, _cachedPositions, _sizes, displayConfig);
				position.X.PixelOffset = position.X.PixelOffset + displayConfig.m_levelInset;
				position.Y.GridTiles = position.Y.GridTiles + this.CalculateSizeY(tileDatas, parentIndex, displayConfig).GridTiles;
				RecipeWidgetUIController.RecipeTileData recipeTileData2 = tileDatas[parentIndex];
				for (int i = 0; i < recipeTileData2.m_children.Count; i++)
				{
					int num = recipeTileData2.m_children[i];
					if (num == _tileIdx)
					{
						break;
					}
					position.X.GridTiles = position.X.GridTiles + _sizes[num].Value.GridTiles;
					position.X.PixelOffset = position.X.PixelOffset + (_sizes[num].Value.PixelOffset + displayConfig.m_tileSpacing);
				}
			}
			else
			{
				position = new RecipeWidgetUIController.Position(new RecipeWidgetUIController.Size(0, 0f), new RecipeWidgetUIController.Size(-2, 0f));
			}
			_cachedPositions[_tileIdx] = new RecipeWidgetUIController.Position?(position);
			return position;
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000B3BAC File Offset: 0x000B1FAC
		private int GetParentIndex(RecipeWidgetUIController.RecipeTileData[] tileDatas, int _childIdx)
		{
			for (int i = 0; i < tileDatas.Length; i++)
			{
				RecipeWidgetUIController.RecipeTileData recipeTileData = tileDatas[i];
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

		// Token: 0x060025F2 RID: 9714 RVA: 0x000B3C04 File Offset: 0x000B2004
		private RecipeWidgetUIController.Size CalculateSizeY(RecipeWidgetUIController.RecipeTileData[] tileDatas, int _tileIdx, RecipeWidgetTile.DisplayConfiguration displayConfig)
		{
			RecipeWidgetUIController.RecipeTileData recipeTileData = tileDatas[_tileIdx];
			bool flag = recipeTileData.m_children.Count > 0;
			bool flag2 = recipeTileData.m_tileDefinition.m_modifierPictures != null && recipeTileData.m_tileDefinition.m_modifierPictures.Count > 0;
			int grid;
			if (MaskUtils.HasFlag<RecipeWidgetTile.Axis>(displayConfig.m_fixedSizeAxis, RecipeWidgetTile.Axis.Y))
			{
				grid = displayConfig.m_fixedSizeGridCells.y;
			}
			else
			{
				grid = ((!flag && !flag2) ? 1 : 2);
			}
			return new RecipeWidgetUIController.Size(grid, displayConfig.m_yExtension);
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000B3C90 File Offset: 0x000B2090
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

		// Token: 0x060025F4 RID: 9716 RVA: 0x000B3D0C File Offset: 0x000B210C
		private void CalculateRect(RectTransform rect, out RecipeWidgetUIController.Size tileSizeX, out RecipeWidgetUIController.Size tileSizeY, out RecipeWidgetUIController.Position tilePos, RecipeWidgetUIController.RecipeTileData[] tileDatas, RecipeWidgetUIController.Size?[] tileSizes, RecipeWidgetUIController.Position?[] tilePositions, int index, RecipeWidgetTile.DisplayConfiguration displayConfig)
		{
			tileSizeX = tileSizes[index].Value;
			tileSizeY = this.CalculateSizeY(tileDatas, index, displayConfig);
			tilePos = tilePositions[index].Value;
			if (MaskUtils.HasFlag<RecipeWidgetTile.Axis>(displayConfig.m_fixedSizeAxis, RecipeWidgetTile.Axis.X))
			{
				tileSizeX = new RecipeWidgetUIController.Size(displayConfig.m_fixedSizeGridCells.x, displayConfig.m_fixedSizeOffset.x);
			}
			if (MaskUtils.HasFlag<RecipeWidgetTile.Axis>(displayConfig.m_fixedSizeAxis, RecipeWidgetTile.Axis.Y))
			{
				tileSizeY = new RecipeWidgetUIController.Size(displayConfig.m_fixedSizeGridCells.y, displayConfig.m_fixedSizeOffset.y);
			}
			if (MaskUtils.HasFlag<RecipeWidgetTile.Axis>(displayConfig.m_fixedPosAxis, RecipeWidgetTile.Axis.X))
			{
				tilePos.X = new RecipeWidgetUIController.Size(displayConfig.m_fixedPosGridCells.x, displayConfig.m_fixedPosOffset.x);
			}
			if (MaskUtils.HasFlag<RecipeWidgetTile.Axis>(displayConfig.m_fixedPosAxis, RecipeWidgetTile.Axis.Y))
			{
				tilePos.Y = new RecipeWidgetUIController.Size(displayConfig.m_fixedPosGridCells.y, displayConfig.m_fixedPosOffset.y);
			}
			rect.anchorMin = new Vector2((float)tilePos.X.GridTiles * 0.5f, (float)(-(float)(tilePos.Y.GridTiles + tileSizeY.GridTiles)) * 0.5f);
			rect.anchorMax = new Vector2((float)(tilePos.X.GridTiles + tileSizeX.GridTiles) * 0.5f, (float)(-(float)tilePos.Y.GridTiles) * 0.5f);
			rect.offsetMin = new Vector2(tilePos.X.PixelOffset, tilePos.Y.PixelOffset);
			rect.offsetMax = new Vector2(tilePos.X.PixelOffset + tileSizeX.PixelOffset, tilePos.Y.PixelOffset + tileSizeY.PixelOffset);
			rect.localScale = Vector3.one;
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000B3EF4 File Offset: 0x000B22F4
		private void OnCreateSubObjects(GameObject _container, RecipeWidgetUIController.RecipeTileData[] tileDatas, RecipeWidgetTile.DisplayConfiguration displayConfig, HordeOrderUIController.Align align)
		{
			List<RecipeWidgetTile> list = new List<RecipeWidgetTile>();
			RecipeWidgetUIController.Size?[] array = this.BuildTileSizes(tileDatas, displayConfig);
			RecipeWidgetUIController.Position?[] tilePositions = this.BuildTilePositions(tileDatas, array, displayConfig);
			RecipeWidgetUIController.RecipeTileData recipeTileData = this.FindHead(tileDatas);
			for (int i = 0; i < tileDatas.Length; i++)
			{
				RecipeWidgetUIController.RecipeTileData recipeTileData2 = tileDatas[i];
				GameObject gameObject = GameObjectUtils.CreateOnParent<RectTransform>(_container, "Tile " + i);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				recipeTileData2.m_tileDefinition.m_largeIcon = (recipeTileData2.m_children.Count > 0);
				recipeTileData2.m_tileDefinition.m_backgroundTop = false;
				RecipeWidgetTile recipeWidgetTile;
				if (recipeTileData2 == recipeTileData)
				{
					RecipeWidgetUIController.Size sizex;
					RecipeWidgetUIController.Size sizey;
					RecipeWidgetUIController.Position position;
					this.CalculateRect(component, out sizex, out sizey, out position, tileDatas, array, tilePositions, i, this.m_topDisplayConfig);
					recipeWidgetTile = gameObject.AddComponent<TopRecipeWidgetTile>();
					recipeWidgetTile.SetupTile(recipeTileData2.m_tileDefinition, this.m_topDisplayConfig, sizex, sizey);
					TopRecipeWidgetTile topRecipeWidgetTile = recipeWidgetTile as TopRecipeWidgetTile;
				}
				else
				{
					RecipeWidgetUIController.Size sizex;
					RecipeWidgetUIController.Size sizey;
					RecipeWidgetUIController.Position position;
					this.CalculateRect(component, out sizex, out sizey, out position, tileDatas, array, tilePositions, i, displayConfig);
					recipeWidgetTile = gameObject.AddComponent<RecipeWidgetTile>();
					recipeWidgetTile.SetupTile(recipeTileData2.m_tileDefinition, displayConfig, sizex, sizey);
				}
				list.Add(recipeWidgetTile);
			}
			list.Reverse();
			for (int j = 0; j < list.Count; j++)
			{
				list[j].transform.SetSiblingIndex(j);
			}
			if (Application.isPlaying)
			{
				for (int k = 0; k < list.Count; k++)
				{
					list[k].RefreshSubElements();
				}
				float num = 0f;
				float num2 = float.NegativeInfinity;
				int childCount = base.transform.childCount;
				for (int l = 0; l < list.Count; l++)
				{
					RectTransform rectTransform = base.transform.GetChild(l).transform as RectTransform;
					if (rectTransform != null)
					{
						num += rectTransform.rect.width;
						num2 = Mathf.Max(rectTransform.rect.height, num2);
					}
				}
				this.m_size = new Vector2(num, num2);
				if (align != HordeOrderUIController.Align.Left)
				{
					if (align == HordeOrderUIController.Align.Right)
					{
						this.m_offset = new Vector2(-1f, 0f).MultipliedBy(this.m_size);
					}
				}
				else
				{
					this.m_offset = new Vector2(0f, 0f).MultipliedBy(this.m_size);
				}
			}
		}

		// Token: 0x04001DD2 RID: 7634
		[SerializeField]
		private RecipeWidgetTile.DisplayConfiguration m_displayConfig = new RecipeWidgetTile.DisplayConfiguration();

		// Token: 0x04001DD3 RID: 7635
		[SerializeField]
		private TopRecipeWidgetTile.TopDisplayConfiguration m_topDisplayConfig = new TopRecipeWidgetTile.TopDisplayConfiguration();

		// Token: 0x04001DD4 RID: 7636
		[SerializeField]
		private Animator m_animator;

		// Token: 0x04001DD5 RID: 7637
		[SerializeField]
		private AnimationCurve m_appearScaleCurve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 1f);

		// Token: 0x04001DD6 RID: 7638
		private WidgetAnimation m_animation;

		// Token: 0x04001DD7 RID: 7639
		private Image[] m_images;

		// Token: 0x04001DD8 RID: 7640
		private RectTransformExtension m_rectTransformExtension;

		// Token: 0x04001DD9 RID: 7641
		private Vector2 m_offset;

		// Token: 0x04001DDA RID: 7642
		private Vector2 m_size;

		// Token: 0x020007BD RID: 1981
		public enum Align
		{
			// Token: 0x04001DDC RID: 7644
			Left,
			// Token: 0x04001DDD RID: 7645
			Right
		}
	}
}
