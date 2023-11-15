using System;
using UnityEngine;

// Token: 0x02000836 RID: 2102
public class ItemPropertiesComponent : MonoBehaviour, ISpawnableItem, IOrderDefinition, IClientOrderDefinition
{
	// Token: 0x0600286F RID: 10351 RVA: 0x000BE115 File Offset: 0x000BC515
	public SubTexture2D GetSubTexture()
	{
		return this.m_itemDefinition.m_crateLid;
	}

	// Token: 0x06002870 RID: 10352 RVA: 0x000BE122 File Offset: 0x000BC522
	public Sprite GetUIIcon()
	{
		return this.m_itemDefinition.m_iconSprite;
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x000BE12F File Offset: 0x000BC52F
	public AssembledDefinitionNode GetOrderComposition()
	{
		return new ItemAssembledNode(this.m_itemDefinition);
	}

	// Token: 0x06002872 RID: 10354 RVA: 0x000BE13C File Offset: 0x000BC53C
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
	}

	// Token: 0x06002873 RID: 10355 RVA: 0x000BE13E File Offset: 0x000BC53E
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
	}

	// Token: 0x04001FFB RID: 8187
	[SerializeField]
	private ItemOrderNode m_itemDefinition;
}
