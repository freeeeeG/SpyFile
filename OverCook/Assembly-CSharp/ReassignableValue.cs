using System;

// Token: 0x0200020A RID: 522
public class ReassignableValue : ILogicalValue, IReassignable<ILogicalValue>, ILogicalElement
{
	// Token: 0x060008B9 RID: 2233 RVA: 0x00034A75 File Offset: 0x00032E75
	public ReassignableValue()
	{
		this.m_childValue = new LogicalKeycodeValue();
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00034A88 File Offset: 0x00032E88
	public ReassignableValue(ILogicalValue _childNode)
	{
		this.m_childValue = _childNode;
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x00034A97 File Offset: 0x00032E97
	public void Reassign(ILogicalValue _childNode)
	{
		this.m_childValue = _childNode;
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00034AA0 File Offset: 0x00032EA0
	public virtual float GetValue()
	{
		return this.m_childValue.GetValue();
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00034AB0 File Offset: 0x00032EB0
	public void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node node;
		this.m_childValue.GetLogicTreeData(out _tree, out node);
		_tree.AddLink(this, node.m_value, new LogicalLinkInfo());
		_head = _tree.GetNode(this);
	}

	// Token: 0x040007A9 RID: 1961
	private ILogicalValue m_childValue;
}
