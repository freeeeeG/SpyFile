using System;

// Token: 0x02000204 RID: 516
public class GateLogicalButton : LogicalButtonBase
{
	// Token: 0x060008A2 RID: 2210 RVA: 0x0003482C File Offset: 0x00032C2C
	public GateLogicalButton(ILogicalButton _childNode, Generic<bool> _callback)
	{
		this.m_callback = _callback;
		this.m_childButton = _childNode;
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x00034842 File Offset: 0x00032C42
	public override bool IsDown()
	{
		return this.m_callback() && this.m_childButton.IsDown();
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x00034864 File Offset: 0x00032C64
	public override void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node node;
		this.m_childButton.GetLogicTreeData(out _tree, out node);
		_tree.AddLink(this, node.m_value, new LogicalLinkInfo());
		_head = _tree.GetNode(this);
	}

	// Token: 0x0400079D RID: 1949
	private Generic<bool> m_callback;

	// Token: 0x0400079E RID: 1950
	private ILogicalButton m_childButton;
}
