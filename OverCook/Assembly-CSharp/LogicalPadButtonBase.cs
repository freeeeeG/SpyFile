using System;

// Token: 0x02000215 RID: 533
public abstract class LogicalPadButtonBase<ButtonId> : LogicalButtonBase
{
	// Token: 0x060008DF RID: 2271 RVA: 0x0003501D File Offset: 0x0003341D
	public LogicalPadButtonBase(ControlPadInput.PadNum _pad, ButtonId _button)
	{
		this.m_pad = _pad;
		this.m_button = _button;
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00035033 File Offset: 0x00033433
	public override void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		_tree = new AcyclicGraph<ILogicalElement, LogicalLinkInfo>(this);
		_head = _tree.GetNode(this);
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x00035047 File Offset: 0x00033447
	public ButtonId GetControlpadButton()
	{
		return this.m_button;
	}

	// Token: 0x040007C3 RID: 1987
	protected ControlPadInput.PadNum m_pad;

	// Token: 0x040007C4 RID: 1988
	protected ButtonId m_button;
}
