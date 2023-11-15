using System;
using System.Collections.Generic;
using BitStream;

// Token: 0x020009C8 RID: 2504
public class NullAssembledNode : AssembledDefinitionNode
{
	// Token: 0x06003112 RID: 12562 RVA: 0x000E65AE File Offset: 0x000E49AE
	public override void Serialise(BitStreamWriter writer)
	{
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x000E65B0 File Offset: 0x000E49B0
	public override bool Deserialise(BitStreamReader reader)
	{
		return true;
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x000E65B4 File Offset: 0x000E49B4
	public override IEnumerator<AssembledDefinitionNode> GetEnumerator()
	{
		yield return this;
		yield break;
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x000E65CF File Offset: 0x000E49CF
	protected override bool IsMatch(AssembledDefinitionNode _node)
	{
		return _node.GetType() == typeof(NullAssembledNode);
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x000E65E3 File Offset: 0x000E49E3
	public override AssembledDefinitionNode Simpilfy()
	{
		return AssembledDefinitionNode.NullNode;
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x000E65EA File Offset: 0x000E49EA
	public override int GetNodeCount()
	{
		return 1;
	}
}
