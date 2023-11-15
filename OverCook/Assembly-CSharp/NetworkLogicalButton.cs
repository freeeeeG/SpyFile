using System;

// Token: 0x02000207 RID: 519
public class NetworkLogicalButton : LogicalButtonBase
{
	// Token: 0x060008A9 RID: 2217 RVA: 0x00034911 File Offset: 0x00032D11
	public NetworkLogicalButton(ILogicalButton _button, PlayerInputLookup.LogicalButtonID _buttonID, bool _buttonRequiresAppFocus)
	{
		this.m_Button = _button;
		this.m_ButtonID = _buttonID;
		this.m_ButtonRequiresAppFocus = _buttonRequiresAppFocus;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00034930 File Offset: 0x00032D30
	public override void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node node;
		this.m_Button.GetLogicTreeData(out _tree, out node);
		_tree.AddLink(this, node.m_value, new LogicalLinkInfo());
		_head = _tree.GetNode(this);
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00034969 File Offset: 0x00032D69
	public override bool IsDown()
	{
		return this.m_IsDown;
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x00034971 File Offset: 0x00032D71
	protected override bool CanProcessInput()
	{
		return !this.m_ButtonRequiresAppFocus || base.CanProcessInput();
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00034987 File Offset: 0x00032D87
	public PlayerInputLookup.LogicalButtonID GetButtonID()
	{
		return this.m_ButtonID;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0003498F File Offset: 0x00032D8F
	public void SetIsDown(bool _isDown)
	{
		this.m_IsDown = _isDown;
	}

	// Token: 0x040007A1 RID: 1953
	private PlayerInputLookup.LogicalButtonID m_ButtonID;

	// Token: 0x040007A2 RID: 1954
	private ILogicalButton m_Button;

	// Token: 0x040007A3 RID: 1955
	private bool m_IsDown;

	// Token: 0x040007A4 RID: 1956
	private bool m_ButtonRequiresAppFocus;
}
