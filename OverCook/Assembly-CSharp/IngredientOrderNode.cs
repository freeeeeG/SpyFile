using System;
using UnityEngine;

// Token: 0x020009BF RID: 2495
[Serializable]
public class IngredientOrderNode : OrderDefinitionNode
{
	// Token: 0x060030DE RID: 12510 RVA: 0x000E5F48 File Offset: 0x000E4348
	public override AssembledDefinitionNode Convert()
	{
		return new IngredientAssembledNode(this);
	}

	// Token: 0x04002743 RID: 10051
	public Sprite m_iconSprite;

	// Token: 0x04002744 RID: 10052
	public SubTexture2D m_crateLid;

	// Token: 0x04002745 RID: 10053
	public Color m_colour = Color.white;
}
