using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B31 RID: 2865
[RequireComponent(typeof(HoverIconUIController))]
public class IngredientContentsUIContainer : UISubElementContainer
{
	// Token: 0x06003A0B RID: 14859 RVA: 0x00114505 File Offset: 0x00112905
	private void Awake()
	{
		this.m_runtimeOrderDefinition = this.m_orderDefinitionNode.Simpilfy();
		this.m_uiMove = base.gameObject.RequestComponent<UI_Move>();
		this.TryGetExistingContainer();
		this.TryGetExistingImages();
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x00114538 File Offset: 0x00112938
	private void TryGetExistingContainer()
	{
		Transform transform = base.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.name.Equals(this.c_containerName))
			{
				this.m_container = child.gameObject;
				return;
			}
		}
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x00114590 File Offset: 0x00112990
	private void TryGetExistingImages()
	{
		if (this.m_container == null)
		{
			return;
		}
		Transform transform = this.m_container.transform;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			GameObject gameObject = transform.GetChild(i).gameObject;
			Image image = gameObject.RequestComponent<Image>();
			if (image != null)
			{
				this.m_icons.Add(image);
			}
			i++;
		}
	}

	// Token: 0x06003A0E RID: 14862 RVA: 0x00114603 File Offset: 0x00112A03
	public void SetOrder(AssembledDefinitionNode _orderDefinition)
	{
		if (AssembledDefinitionNode.Matching(this.m_runtimeOrderDefinition, _orderDefinition))
		{
			return;
		}
		this.m_runtimeOrderDefinition = _orderDefinition;
		base.RefreshSubElements();
	}

	// Token: 0x06003A0F RID: 14863 RVA: 0x00114624 File Offset: 0x00112A24
	protected override void EnsureImagesExist()
	{
		if (this.m_container != null)
		{
			return;
		}
		this.m_container = GameObjectUtils.CreateOnParent<RectTransform>(base.gameObject, this.c_containerName);
		AnchorGridLayoutGroup anchorGridLayoutGroup = this.m_container.AddComponent<AnchorGridLayoutGroup>();
		anchorGridLayoutGroup.cellSize = new Vector2(0.5f, 0.5f);
		anchorGridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
		this.OnCreateSubObjects(this.m_container);
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x0011468E File Offset: 0x00112A8E
	protected override void OnRefreshSubObjectProperties(GameObject _container)
	{
		this.RefreshIcons(_container);
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x00114697 File Offset: 0x00112A97
	private void RefreshIcons(GameObject _container)
	{
		this.SetIconsForOrder(_container, this.m_runtimeOrderDefinition);
		this.UpdateIconVisibility();
		if (this.m_uiMove != null)
		{
			this.m_uiMove.UpdateGraphics();
		}
	}

	// Token: 0x06003A12 RID: 14866 RVA: 0x001146C8 File Offset: 0x00112AC8
	protected void SetIconsForOrder(GameObject _container, AssembledDefinitionNode _order)
	{
		this.m_iconsInUse = 0;
		if (_order == null || _order.Simpilfy() == AssembledDefinitionNode.NullNode)
		{
			this.SetRemainingIcons(_container, this.m_emptySprite);
			return;
		}
		CookedCompositeAssembledNode cookedCompositeAssembledNode = _order as CookedCompositeAssembledNode;
		if (cookedCompositeAssembledNode != null && cookedCompositeAssembledNode.m_progress == CookedCompositeOrderNode.CookingProgress.Burnt)
		{
			this.SetNextIcon(_container, this.m_burntSprite);
			return;
		}
		MixedCompositeAssembledNode mixedCompositeAssembledNode = _order as MixedCompositeAssembledNode;
		if (mixedCompositeAssembledNode != null && mixedCompositeAssembledNode.m_progress == MixedCompositeOrderNode.MixingProgress.OverMixed)
		{
			this.SetNextIcon(_container, this.m_overmixedSprite);
			return;
		}
		foreach (AssembledDefinitionNode assembledDefinitionNode in _order)
		{
			IngredientAssembledNode ingredientAssembledNode = assembledDefinitionNode as IngredientAssembledNode;
			if (ingredientAssembledNode != null && ingredientAssembledNode.m_ingriedientOrderNode != null)
			{
				this.SetNextIcon(_container, ingredientAssembledNode.m_ingriedientOrderNode.m_iconSprite);
			}
			ItemAssembledNode itemAssembledNode = assembledDefinitionNode as ItemAssembledNode;
			if (itemAssembledNode != null && itemAssembledNode.m_itemOrderNode != null)
			{
				this.SetNextIcon(_container, itemAssembledNode.m_itemOrderNode.m_iconSprite);
			}
		}
		this.SetRemainingIcons(_container, this.m_emptySprite);
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x00114804 File Offset: 0x00112C04
	private void SetRemainingIcons(GameObject _container, Sprite _sprite)
	{
		while (this.m_iconsInUse < this.m_minimumSize)
		{
			this.SetNextIcon(_container, _sprite);
		}
	}

	// Token: 0x06003A14 RID: 14868 RVA: 0x00114824 File Offset: 0x00112C24
	private void SetNextIcon(GameObject _container, Sprite _sprite)
	{
		if (this.m_icons.Count <= this.m_iconsInUse)
		{
			this.InstantiateIcon(_container);
		}
		this.m_icons[this.m_iconsInUse++].sprite = _sprite;
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x00114870 File Offset: 0x00112C70
	private void InstantiateIcon(GameObject _container)
	{
		GameObject obj = GameObjectUtils.CreateOnParent<Image>(_container, "Icon");
		Image image = obj.RequireComponent<Image>();
		image.sprite = this.m_emptySprite;
		this.m_icons.Add(image);
		UIUtils.SetupFillParentAreaRect(image.transform as RectTransform);
	}

	// Token: 0x06003A16 RID: 14870 RVA: 0x001148B8 File Offset: 0x00112CB8
	private void UpdateIconVisibility()
	{
		for (int i = 0; i < this.m_iconsInUse; i++)
		{
			this.m_icons[i].gameObject.SetActive(true);
		}
		for (int j = this.m_iconsInUse; j < this.m_icons.Count; j++)
		{
			this.m_icons[j].gameObject.SetActive(false);
		}
	}

	// Token: 0x06003A17 RID: 14871 RVA: 0x0011492B File Offset: 0x00112D2B
	public void SetMinimumSize(int _size)
	{
		this.m_minimumSize = _size;
		base.RefreshSubElements();
	}

	// Token: 0x04002F02 RID: 12034
	[SerializeField]
	private OrderDefinitionNode m_orderDefinitionNode;

	// Token: 0x04002F03 RID: 12035
	[SerializeField]
	private Sprite m_burntSprite;

	// Token: 0x04002F04 RID: 12036
	[SerializeField]
	private Sprite m_overmixedSprite;

	// Token: 0x04002F05 RID: 12037
	[SerializeField]
	private Sprite m_emptySprite;

	// Token: 0x04002F06 RID: 12038
	private AssembledDefinitionNode m_runtimeOrderDefinition;

	// Token: 0x04002F07 RID: 12039
	private int m_minimumSize;

	// Token: 0x04002F08 RID: 12040
	private List<Image> m_icons = new List<Image>();

	// Token: 0x04002F09 RID: 12041
	private int m_iconsInUse;

	// Token: 0x04002F0A RID: 12042
	private UI_Move m_uiMove;
}
