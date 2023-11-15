using System;

// Token: 0x02000205 RID: 517
public class GateLogicalValue : ILogicalValue, ILogicalElement
{
	// Token: 0x060008A5 RID: 2213 RVA: 0x0003489D File Offset: 0x00032C9D
	public GateLogicalValue(ILogicalValue _childNode, Generic<bool> _callback)
	{
		this.m_callback = _callback;
		this.m_childValue = _childNode;
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x000348B3 File Offset: 0x00032CB3
	public virtual float GetValue()
	{
		if (this.m_callback())
		{
			return this.m_childValue.GetValue();
		}
		return 0f;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x000348D8 File Offset: 0x00032CD8
	public void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node node;
		this.m_childValue.GetLogicTreeData(out _tree, out node);
		_tree.AddLink(this, node.m_value, new LogicalLinkInfo());
		_head = _tree.GetNode(this);
	}

	// Token: 0x0400079F RID: 1951
	private Generic<bool> m_callback;

	// Token: 0x040007A0 RID: 1952
	private ILogicalValue m_childValue;
}
