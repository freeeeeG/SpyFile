using System;
using System.Collections.Generic;
using BitStream;

// Token: 0x020009C2 RID: 2498
public class ItemAssembledNode : AssembledDefinitionNode
{
	// Token: 0x060030EA RID: 12522 RVA: 0x000E60F5 File Offset: 0x000E44F5
	public ItemAssembledNode()
	{
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x000E60FD File Offset: 0x000E44FD
	public ItemAssembledNode(ItemOrderNode _orderNode)
	{
		this.m_itemOrderNode = _orderNode;
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x000E610C File Offset: 0x000E450C
	public override void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_itemOrderNode.m_uID, 32);
	}

	// Token: 0x060030ED RID: 12525 RVA: 0x000E6124 File Offset: 0x000E4524
	public override bool Deserialise(BitStreamReader reader)
	{
		OrderDefinitionNode orderDefinitionNode = GameUtils.GetOrderDefinitionNode((int)reader.ReadUInt32(32));
		ItemOrderNode itemOrderNode = orderDefinitionNode as ItemOrderNode;
		this.m_itemOrderNode = itemOrderNode;
		return true;
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x000E6150 File Offset: 0x000E4550
	public override IEnumerator<AssembledDefinitionNode> GetEnumerator()
	{
		yield return this;
		yield break;
	}

	// Token: 0x060030EF RID: 12527 RVA: 0x000E616C File Offset: 0x000E456C
	protected override bool IsMatch(AssembledDefinitionNode _subject)
	{
		ItemAssembledNode itemAssembledNode = _subject as ItemAssembledNode;
		return itemAssembledNode != null && this.m_itemOrderNode.m_uID == itemAssembledNode.m_itemOrderNode.m_uID;
	}

	// Token: 0x060030F0 RID: 12528 RVA: 0x000E61A0 File Offset: 0x000E45A0
	public override AssembledDefinitionNode Simpilfy()
	{
		return new ItemAssembledNode(this.m_itemOrderNode);
	}

	// Token: 0x060030F1 RID: 12529 RVA: 0x000E61B0 File Offset: 0x000E45B0
	public override void ReplaceData(AssembledDefinitionNode _node)
	{
		ItemAssembledNode itemAssembledNode = _node as ItemAssembledNode;
		this.m_itemOrderNode = itemAssembledNode.m_itemOrderNode;
		base.ReplaceData(_node);
	}

	// Token: 0x060030F2 RID: 12530 RVA: 0x000E61D7 File Offset: 0x000E45D7
	public override int GetNodeCount()
	{
		return 1;
	}

	// Token: 0x0400274B RID: 10059
	public ItemOrderNode m_itemOrderNode;
}
