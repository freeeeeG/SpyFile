using System;
using System.Collections;
using System.Collections.Generic;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020009C7 RID: 2503
public abstract class AssembledDefinitionNode : IEnumerable<AssembledDefinitionNode>, Serialisable, IEnumerable
{
	// Token: 0x06003102 RID: 12546
	public abstract void Serialise(BitStreamWriter writer);

	// Token: 0x06003103 RID: 12547
	public abstract bool Deserialise(BitStreamReader reader);

	// Token: 0x06003104 RID: 12548 RVA: 0x000E50F4 File Offset: 0x000E34F4
	public static bool MatchingAlreadySimple(AssembledDefinitionNode _simpleNode1, AssembledDefinitionNode _simpleNode2)
	{
		if (_simpleNode1 == null || _simpleNode2 == null)
		{
			return _simpleNode1 == null && _simpleNode2 == null;
		}
		return _simpleNode1.IsMatch(_simpleNode2);
	}

	// Token: 0x06003105 RID: 12549 RVA: 0x000E5118 File Offset: 0x000E3518
	public static bool Matching(OrderDefinitionNode _node1, OrderDefinitionNode _node2)
	{
		if (_node1 == null || _node2 == null)
		{
			return _node1 == null && _node2 == null;
		}
		AssembledDefinitionNode assembledDefinitionNode = _node1.Simpilfy();
		AssembledDefinitionNode node = _node2.Simpilfy();
		return assembledDefinitionNode.IsMatch(node);
	}

	// Token: 0x06003106 RID: 12550 RVA: 0x000E516C File Offset: 0x000E356C
	public static bool Matching(AssembledDefinitionNode _node1, AssembledDefinitionNode _node2)
	{
		if (_node1 == null || _node2 == null)
		{
			return _node1 == null && _node2 == null;
		}
		AssembledDefinitionNode assembledDefinitionNode = _node1.Simpilfy();
		AssembledDefinitionNode node = _node2.Simpilfy();
		return assembledDefinitionNode.IsMatch(node);
	}

	// Token: 0x06003107 RID: 12551 RVA: 0x000E51A8 File Offset: 0x000E35A8
	public static bool Matching(OrderDefinitionNode _node1, AssembledDefinitionNode _node2)
	{
		if (_node1 == null || _node2 == null)
		{
			return _node1 == null && _node2 == null;
		}
		AssembledDefinitionNode assembledDefinitionNode = _node1.Simpilfy();
		AssembledDefinitionNode node = _node2.Simpilfy();
		return assembledDefinitionNode.IsMatch(node);
	}

	// Token: 0x06003108 RID: 12552 RVA: 0x000E51F0 File Offset: 0x000E35F0
	public static bool Matching(AssembledDefinitionNode _node1, OrderDefinitionNode _node2)
	{
		if (_node1 == null || _node2 == null)
		{
			return _node1 == null && _node2 == null;
		}
		AssembledDefinitionNode assembledDefinitionNode = _node1.Simpilfy();
		AssembledDefinitionNode node = _node2.Simpilfy();
		return assembledDefinitionNode.IsMatch(node);
	}

	// Token: 0x06003109 RID: 12553
	public abstract IEnumerator<AssembledDefinitionNode> GetEnumerator();

	// Token: 0x0600310A RID: 12554 RVA: 0x000E5235 File Offset: 0x000E3635
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600310B RID: 12555 RVA: 0x000E523D File Offset: 0x000E363D
	public override bool Equals(object _node)
	{
		return AssembledDefinitionNode.Matching(this, _node as AssembledDefinitionNode);
	}

	// Token: 0x0600310C RID: 12556
	protected abstract bool IsMatch(AssembledDefinitionNode _node);

	// Token: 0x0600310D RID: 12557
	public abstract int GetNodeCount();

	// Token: 0x0600310E RID: 12558
	public abstract AssembledDefinitionNode Simpilfy();

	// Token: 0x0600310F RID: 12559 RVA: 0x000E524B File Offset: 0x000E364B
	public virtual void ReplaceData(AssembledDefinitionNode _node)
	{
		this.m_freeObject = _node.m_freeObject;
	}

	// Token: 0x04002759 RID: 10073
	public static AssembledDefinitionNode NullNode = new NullAssembledNode();

	// Token: 0x0400275A RID: 10074
	public GameObject m_freeObject;
}
