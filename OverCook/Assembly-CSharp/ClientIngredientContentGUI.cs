using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020009AB RID: 2475
public class ClientIngredientContentGUI : ClientSynchroniserBase
{
	// Token: 0x06003070 RID: 12400 RVA: 0x000E3C7C File Offset: 0x000E207C
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_ingredientContentGUI = (IngredientContentGUI)synchronisedObject;
		this.m_iOrderDefinitions = base.gameObject.RequestInterfaces<IClientOrderDefinition>();
		for (int i = 0; i < this.m_iOrderDefinitions.Length; i++)
		{
			this.m_iOrderDefinitions[i].RegisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderChanged));
			this.OnOrderChanged(this.m_iOrderDefinitions[i].GetOrderComposition());
		}
		if (this.m_iOrderDefinitions.Length == 0)
		{
		}
	}

	// Token: 0x06003071 RID: 12401 RVA: 0x000E3CF8 File Offset: 0x000E20F8
	private void OnOrderChanged(AssembledDefinitionNode _orderDefinition)
	{
		this.m_mostRecentChange = _orderDefinition;
		if (base.enabled && base.gameObject.activeInHierarchy)
		{
			if (_orderDefinition.Simpilfy() != AssembledDefinitionNode.NullNode || this.m_ingredientContentGUI.m_displayEmptyElements)
			{
				if (this.m_uiInstance == null)
				{
					GameObject obj = GameUtils.InstantiateHoverIconUIController(this.m_ingredientContentGUI.m_ingredientContentUIPrefab.gameObject, NetworkUtils.FindVisualRoot(base.gameObject), "HoverIconCanvas", this.m_ingredientContentGUI.m_Offset);
					this.m_uiInstance = obj.RequireComponent<IngredientContentsUIContainer>();
				}
				else
				{
					this.m_uiInstance.gameObject.SetActive(true);
				}
				if (this.m_ingredientContentGUI.m_displayEmptyElements)
				{
					IngredientContainer ingredientContainer = base.gameObject.RequestComponent<IngredientContainer>();
					if (ingredientContainer != null)
					{
						int minimumSize = ingredientContainer.GetCapacity();
						if (_orderDefinition.Simpilfy() == AssembledDefinitionNode.NullNode)
						{
							minimumSize = 1;
						}
						this.m_uiInstance.SetMinimumSize(minimumSize);
					}
				}
			}
			else if (this.m_uiInstance != null)
			{
				this.m_uiInstance.gameObject.SetActive(false);
			}
			if (this.m_uiInstance != null)
			{
				this.m_uiInstance.SetOrder(_orderDefinition);
			}
		}
	}

	// Token: 0x06003072 RID: 12402 RVA: 0x000E3E3C File Offset: 0x000E223C
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.m_iOrderDefinitions != null)
		{
			if (this.m_mostRecentChange != null)
			{
				this.OnOrderChanged(this.m_mostRecentChange);
			}
			else
			{
				for (int i = 0; i < this.m_iOrderDefinitions.Length; i++)
				{
					this.OnOrderChanged(this.m_iOrderDefinitions[i].GetOrderComposition());
				}
			}
		}
	}

	// Token: 0x06003073 RID: 12403 RVA: 0x000E3EA2 File Offset: 0x000E22A2
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_uiInstance != null)
		{
			this.m_uiInstance.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003074 RID: 12404 RVA: 0x000E3ECC File Offset: 0x000E22CC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_uiInstance != null)
		{
			UnityEngine.Object.Destroy(this.m_uiInstance.gameObject);
			this.m_uiInstance = null;
		}
	}

	// Token: 0x040026DF RID: 9951
	private IngredientContentGUI m_ingredientContentGUI;

	// Token: 0x040026E0 RID: 9952
	private IClientOrderDefinition[] m_iOrderDefinitions;

	// Token: 0x040026E1 RID: 9953
	private IngredientContentsUIContainer m_uiInstance;

	// Token: 0x040026E2 RID: 9954
	private AssembledDefinitionNode m_mostRecentChange;
}
