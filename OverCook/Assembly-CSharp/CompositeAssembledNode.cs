using System;
using System.Collections.Generic;
using BitStream;

// Token: 0x020009BB RID: 2491
public class CompositeAssembledNode : AssembledDefinitionNode
{
	// Token: 0x060030C6 RID: 12486 RVA: 0x000E5290 File Offset: 0x000E3690
	public override void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_composition.Length, 5);
		for (int i = 0; i < this.m_composition.Length; i++)
		{
			writer.Write((uint)AssembledDefinitionNodeFactory.GetNodeType(this.m_composition[i]), 4);
			this.m_composition[i].Serialise(writer);
		}
	}

	// Token: 0x060030C7 RID: 12487 RVA: 0x000E52E8 File Offset: 0x000E36E8
	public override bool Deserialise(BitStreamReader reader)
	{
		bool flag = true;
		if (this.m_composition.Length != 0)
		{
			this.m_composition = new AssembledDefinitionNode[0];
		}
		int num = (int)reader.ReadUInt32(5);
		for (int i = 0; i < num; i++)
		{
			AssembledDefinitionNode assembledDefinitionNode = AssembledDefinitionNodeFactory.CreateNode((int)reader.ReadUInt32(4));
			flag &= assembledDefinitionNode.Deserialise(reader);
			Array.Resize<AssembledDefinitionNode>(ref this.m_composition, this.m_composition.Length + 1);
			this.m_composition[this.m_composition.Length - 1] = assembledDefinitionNode;
		}
		return flag;
	}

	// Token: 0x060030C8 RID: 12488 RVA: 0x000E536C File Offset: 0x000E376C
	public IEnumerator<AssembledDefinitionNode> GetEnumeratorExhaustive()
	{
		for (int i = 0; i < this.m_composition.Length; i++)
		{
			yield return this.m_composition[i];
			foreach (AssembledDefinitionNode node in this.m_composition[i])
			{
				yield return node;
			}
		}
		yield break;
	}

	// Token: 0x060030C9 RID: 12489 RVA: 0x000E5388 File Offset: 0x000E3788
	public override IEnumerator<AssembledDefinitionNode> GetEnumerator()
	{
		for (int i = 0; i < this.m_composition.Length; i++)
		{
			foreach (AssembledDefinitionNode node in this.m_composition[i])
			{
				yield return node;
			}
		}
		yield break;
	}

	// Token: 0x060030CA RID: 12490 RVA: 0x000E53A4 File Offset: 0x000E37A4
	protected override bool IsMatch(AssembledDefinitionNode _subject)
	{
		if (_subject.GetType() == typeof(CompositeAssembledNode))
		{
			CompositeAssembledNode subject = _subject as CompositeAssembledNode;
			return this.AssumeTypeMatch(subject);
		}
		return this.IsMatch(new CompositeAssembledNode
		{
			m_composition = new AssembledDefinitionNode[]
			{
				_subject
			}
		});
	}

	// Token: 0x060030CB RID: 12491 RVA: 0x000E53F4 File Offset: 0x000E37F4
	protected bool AssumeTypeMatch(CompositeAssembledNode _subject)
	{
		return CompositeAssembledNode.Contains(this.m_composition.Union(this.m_optional), _subject.m_composition) && CompositeAssembledNode.Contains(_subject.m_composition.Union(_subject.m_optional), this.m_composition);
	}

	// Token: 0x060030CC RID: 12492 RVA: 0x000E5444 File Offset: 0x000E3844
	public override AssembledDefinitionNode Simpilfy()
	{
		CompositeAssembledNode compositeAssembledNode = new CompositeAssembledNode();
		compositeAssembledNode.m_permittedEntries = this.m_permittedEntries;
		for (int i = 0; i < this.m_composition.Length; i++)
		{
			if (this.m_composition[i] != AssembledDefinitionNode.NullNode)
			{
				AssembledDefinitionNode assembledDefinitionNode = this.m_composition[i].Simpilfy();
				if (assembledDefinitionNode != AssembledDefinitionNode.NullNode)
				{
					ArrayUtils.PushBack<AssembledDefinitionNode>(ref compositeAssembledNode.m_composition, assembledDefinitionNode);
				}
			}
		}
		for (int j = 0; j < this.m_optional.Length; j++)
		{
			if (this.m_optional[j] != AssembledDefinitionNode.NullNode)
			{
				AssembledDefinitionNode assembledDefinitionNode2 = this.m_optional[j].Simpilfy();
				if (assembledDefinitionNode2 != AssembledDefinitionNode.NullNode)
				{
					ArrayUtils.PushBack<AssembledDefinitionNode>(ref compositeAssembledNode.m_optional, assembledDefinitionNode2);
				}
			}
		}
		if (compositeAssembledNode.m_optional.Length == 0)
		{
			if (compositeAssembledNode.m_composition.Length == 1)
			{
				return compositeAssembledNode.m_composition[0];
			}
			if (compositeAssembledNode.m_composition.Length == 0)
			{
				return AssembledDefinitionNode.NullNode;
			}
		}
		return compositeAssembledNode;
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x000E5540 File Offset: 0x000E3940
	public static bool Contains(AssembledDefinitionNode[] _superSet, AssembledDefinitionNode[] _subSet)
	{
		bool[] array = new bool[_superSet.Length];
		for (int i = 0; i < _subSet.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < _superSet.Length; j++)
			{
				if (!array[j] && (_subSet[i] == null || AssembledDefinitionNode.MatchingAlreadySimple(_subSet[i], _superSet[j])))
				{
					array[j] = true;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x000E55B4 File Offset: 0x000E39B4
	public override void ReplaceData(AssembledDefinitionNode _node)
	{
		CompositeAssembledNode compositeAssembledNode = _node as CompositeAssembledNode;
		this.m_composition = compositeAssembledNode.m_composition;
		this.m_optional = compositeAssembledNode.m_optional;
		this.m_permittedEntries = compositeAssembledNode.m_permittedEntries;
		base.ReplaceData(_node);
	}

	// Token: 0x060030CF RID: 12495 RVA: 0x000E55F4 File Offset: 0x000E39F4
	public void AddOrderNode(AssembledDefinitionNode _toAdd, bool _dontUpdate)
	{
		if (this.m_freeObject != null && this.m_freeObject.RequestInterface<IHandleOrderModification>() != null && !_dontUpdate)
		{
			IHandleOrderModification handleOrderModification = this.m_freeObject.RequestInterface<IHandleOrderModification>();
			handleOrderModification.AddOrderContents(new AssembledDefinitionNode[]
			{
				_toAdd
			});
			IOrderDefinition orderDefinition = this.m_freeObject.RequireInterface<IOrderDefinition>();
			CompositeAssembledNode node = orderDefinition.GetOrderComposition() as CompositeAssembledNode;
			this.ReplaceData(node);
			return;
		}
		Array.Resize<AssembledDefinitionNode>(ref this.m_composition, this.m_composition.Length + 1);
		this.m_composition[this.m_composition.Length - 1] = _toAdd;
	}

	// Token: 0x060030D0 RID: 12496 RVA: 0x000E568C File Offset: 0x000E3A8C
	public bool CanAddOrderNode(AssembledDefinitionNode _toAdd, bool _raw = false)
	{
		if (!_raw && this.m_freeObject != null && this.m_freeObject.RequestInterface<IHandleOrderModification>() != null)
		{
			IHandleOrderModification handleOrderModification = this.m_freeObject.RequestInterface<IHandleOrderModification>();
			return handleOrderModification.CanAddOrderContents(new AssembledDefinitionNode[]
			{
				_toAdd
			});
		}
		int num = 0;
		foreach (AssembledDefinitionNode node in this.m_composition)
		{
			if (AssembledDefinitionNode.Matching(node, _toAdd))
			{
				num++;
			}
		}
		Predicate<OrderContentRestriction> match = (OrderContentRestriction _specifier) => AssembledDefinitionNode.Matching(_specifier.m_content, _toAdd);
		OrderContentRestriction orderContentRestriction = this.m_permittedEntries.Find(match);
		return orderContentRestriction != null && num < orderContentRestriction.m_amountAllowed && !this.CompositionContainsRestrictedNode(_toAdd, orderContentRestriction);
	}

	// Token: 0x060030D1 RID: 12497 RVA: 0x000E5774 File Offset: 0x000E3B74
	private bool CompositionContainsRestrictedNode(AssembledDefinitionNode _toAdd, OrderContentRestriction _specifier)
	{
		for (int i = 0; i < _specifier.m_restrictedContent.Length; i++)
		{
			OrderDefinitionNode node = _specifier.m_restrictedContent[i];
			for (int j = 0; j < this.m_composition.Length; j++)
			{
				AssembledDefinitionNode node2 = this.m_composition[j];
				if (AssembledDefinitionNode.Matching(node, node2))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060030D2 RID: 12498 RVA: 0x000E57D4 File Offset: 0x000E3BD4
	public override int GetNodeCount()
	{
		int num = 1;
		for (int i = 0; i < this.m_composition.Length; i++)
		{
			num += this.m_composition[i].GetNodeCount();
		}
		return num;
	}

	// Token: 0x04002735 RID: 10037
	public AssembledDefinitionNode[] m_composition = new AssembledDefinitionNode[0];

	// Token: 0x04002736 RID: 10038
	public AssembledDefinitionNode[] m_optional = new AssembledDefinitionNode[0];

	// Token: 0x04002737 RID: 10039
	public List<OrderContentRestriction> m_permittedEntries = new List<OrderContentRestriction>();
}
