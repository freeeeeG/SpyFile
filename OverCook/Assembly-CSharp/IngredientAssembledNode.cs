using System;
using System.Collections.Generic;
using BitStream;

// Token: 0x020009C0 RID: 2496
public class IngredientAssembledNode : AssembledDefinitionNode
{
	// Token: 0x060030DF RID: 12511 RVA: 0x000E5F5D File Offset: 0x000E435D
	public IngredientAssembledNode()
	{
	}

	// Token: 0x060030E0 RID: 12512 RVA: 0x000E5F65 File Offset: 0x000E4365
	public IngredientAssembledNode(IngredientOrderNode _orderNode)
	{
		this.m_ingriedientOrderNode = _orderNode;
	}

	// Token: 0x060030E1 RID: 12513 RVA: 0x000E5F74 File Offset: 0x000E4374
	public override void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_ingriedientOrderNode.m_uID, 32);
	}

	// Token: 0x060030E2 RID: 12514 RVA: 0x000E5F8C File Offset: 0x000E438C
	public override bool Deserialise(BitStreamReader reader)
	{
		OrderDefinitionNode orderDefinitionNode = GameUtils.GetOrderDefinitionNode((int)reader.ReadUInt32(32));
		IngredientOrderNode ingriedientOrderNode = orderDefinitionNode as IngredientOrderNode;
		this.m_ingriedientOrderNode = ingriedientOrderNode;
		return true;
	}

	// Token: 0x060030E3 RID: 12515 RVA: 0x000E5FB8 File Offset: 0x000E43B8
	public override IEnumerator<AssembledDefinitionNode> GetEnumerator()
	{
		yield return this;
		yield break;
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x000E5FD4 File Offset: 0x000E43D4
	protected override bool IsMatch(AssembledDefinitionNode _subject)
	{
		IngredientAssembledNode ingredientAssembledNode = _subject as IngredientAssembledNode;
		return ingredientAssembledNode != null && this.m_ingriedientOrderNode.m_uID == ingredientAssembledNode.m_ingriedientOrderNode.m_uID;
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x000E6008 File Offset: 0x000E4408
	public override AssembledDefinitionNode Simpilfy()
	{
		return new IngredientAssembledNode(this.m_ingriedientOrderNode);
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x000E6018 File Offset: 0x000E4418
	public override void ReplaceData(AssembledDefinitionNode _node)
	{
		IngredientAssembledNode ingredientAssembledNode = _node as IngredientAssembledNode;
		this.m_ingriedientOrderNode = ingredientAssembledNode.m_ingriedientOrderNode;
		base.ReplaceData(_node);
	}

	// Token: 0x060030E7 RID: 12519 RVA: 0x000E603F File Offset: 0x000E443F
	public override int GetNodeCount()
	{
		return 1;
	}

	// Token: 0x04002746 RID: 10054
	public IngredientOrderNode m_ingriedientOrderNode;
}
