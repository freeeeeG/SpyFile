using System;

// Token: 0x02000209 RID: 521
public class ReassignableButton : LogicalButtonBase, IReassignable<ILogicalButton>
{
	// Token: 0x060008B4 RID: 2228 RVA: 0x00034A01 File Offset: 0x00032E01
	public ReassignableButton()
	{
		this.m_childButton = new LogicalKeycodeButton();
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00034A14 File Offset: 0x00032E14
	public ReassignableButton(ILogicalButton _childNode)
	{
		this.m_childButton = _childNode;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00034A23 File Offset: 0x00032E23
	public void Reassign(ILogicalButton _childNode)
	{
		this.m_childButton = _childNode;
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00034A2C File Offset: 0x00032E2C
	public override bool IsDown()
	{
		return this.m_childButton.IsDown();
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00034A3C File Offset: 0x00032E3C
	public override void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node node;
		this.m_childButton.GetLogicTreeData(out _tree, out node);
		_tree.AddLink(this, node.m_value, new LogicalLinkInfo());
		_head = _tree.GetNode(this);
	}

	// Token: 0x040007A8 RID: 1960
	private ILogicalButton m_childButton;
}
