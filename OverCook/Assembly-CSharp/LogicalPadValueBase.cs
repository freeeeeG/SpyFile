using System;

// Token: 0x02000217 RID: 535
public abstract class LogicalPadValueBase<ValueID> : ILogicalValue, ILogicalElement
{
	// Token: 0x060008E6 RID: 2278 RVA: 0x00035097 File Offset: 0x00033497
	public LogicalPadValueBase(ControlPadInput.PadNum _pad, ValueID _value)
	{
		this.m_pad = _pad;
		this.m_value = _value;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x000350AD File Offset: 0x000334AD
	public virtual void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		_tree = new AcyclicGraph<ILogicalElement, LogicalLinkInfo>(this);
		_head = _tree.GetNode(this);
	}

	// Token: 0x060008E8 RID: 2280
	public abstract float GetValue();

	// Token: 0x060008E9 RID: 2281 RVA: 0x000350C1 File Offset: 0x000334C1
	public ValueID GetControlpadValue()
	{
		return this.m_value;
	}

	// Token: 0x040007C8 RID: 1992
	protected ControlPadInput.PadNum m_pad;

	// Token: 0x040007C9 RID: 1993
	protected ValueID m_value;
}
