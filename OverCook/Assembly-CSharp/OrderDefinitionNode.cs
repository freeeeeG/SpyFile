using System;
using UnityEngine;

// Token: 0x020009C6 RID: 2502
[Serializable]
public abstract class OrderDefinitionNode : ScriptableObject
{
	// Token: 0x060030FE RID: 12542 RVA: 0x000E5024 File Offset: 0x000E3424
	public AssembledDefinitionNode Simpilfy()
	{
		AssembledDefinitionNode assembledDefinitionNode = this.Convert();
		return assembledDefinitionNode.Simpilfy();
	}

	// Token: 0x060030FF RID: 12543
	public abstract AssembledDefinitionNode Convert();

	// Token: 0x06003100 RID: 12544 RVA: 0x000E503E File Offset: 0x000E343E
	public override bool Equals(object _node)
	{
		return AssembledDefinitionNode.Matching(this, _node as OrderDefinitionNode);
	}

	// Token: 0x04002755 RID: 10069
	public RecipeWidgetUIController.RecipeTileData[] m_orderGuiDescription;

	// Token: 0x04002756 RID: 10070
	public PlatingStepData m_platingStep;

	// Token: 0x04002757 RID: 10071
	public GameObject m_platingPrefab;

	// Token: 0x04002758 RID: 10072
	[SelfAssignID(true)]
	public int m_uID;
}
