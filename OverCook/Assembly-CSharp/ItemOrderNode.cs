using System;
using UnityEngine;

// Token: 0x020009C1 RID: 2497
[Serializable]
public class ItemOrderNode : OrderDefinitionNode
{
	// Token: 0x060030E9 RID: 12521 RVA: 0x000E60E0 File Offset: 0x000E44E0
	public override AssembledDefinitionNode Convert()
	{
		return new ItemAssembledNode(this);
	}

	// Token: 0x04002747 RID: 10055
	public Sprite m_iconSprite;

	// Token: 0x04002748 RID: 10056
	public SubTexture2D m_crateLid;

	// Token: 0x04002749 RID: 10057
	public Color m_colour = Color.white;

	// Token: 0x0400274A RID: 10058
	[Range(0f, 1f)]
	public float m_heatValue;
}
