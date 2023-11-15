using System;
using UnityEngine;

// Token: 0x020007FD RID: 2045
public class IngredientPropertiesComponent : MonoBehaviour, ISpawnableItem, IOrderDefinition, IClientOrderDefinition
{
	// Token: 0x0600272A RID: 10026 RVA: 0x000B9217 File Offset: 0x000B7617
	public void Awake()
	{
	}

	// Token: 0x0600272B RID: 10027 RVA: 0x000B9219 File Offset: 0x000B7619
	public SubTexture2D GetSubTexture()
	{
		return this.m_ingredientOrderNode.m_crateLid;
	}

	// Token: 0x0600272C RID: 10028 RVA: 0x000B9226 File Offset: 0x000B7626
	public Sprite GetUIIcon()
	{
		return this.m_ingredientOrderNode.m_iconSprite;
	}

	// Token: 0x0600272D RID: 10029 RVA: 0x000B9233 File Offset: 0x000B7633
	public AssembledDefinitionNode GetOrderComposition()
	{
		return new IngredientAssembledNode(this.m_ingredientOrderNode);
	}

	// Token: 0x0600272E RID: 10030 RVA: 0x000B9240 File Offset: 0x000B7640
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
	}

	// Token: 0x0600272F RID: 10031 RVA: 0x000B9242 File Offset: 0x000B7642
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x000B9244 File Offset: 0x000B7644
	public void SetIngredientOrderNode(IngredientOrderNode ingredientOrderNode)
	{
		this.m_ingredientOrderNode = ingredientOrderNode;
	}

	// Token: 0x04001EDF RID: 7903
	[SerializeField]
	private IngredientOrderNode m_ingredientOrderNode;
}
