using System;
using UnityEngine;

// Token: 0x0200020F RID: 527
public abstract class LogicalKeycodeButtonBase<ButtonId> : LogicalButtonBase
{
	// Token: 0x060008CC RID: 2252 RVA: 0x00034E59 File Offset: 0x00033259
	public LogicalKeycodeButtonBase(KeyCode? _code, ButtonId _button)
	{
		this.m_keyCode = _code;
		this.m_button = _button;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00034E6F File Offset: 0x0003326F
	public LogicalKeycodeButtonBase()
	{
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x00034E78 File Offset: 0x00033278
	public override bool IsDown()
	{
		if (!this.CanProcessInput())
		{
			base.Update(false);
			return false;
		}
		bool flag = false;
		KeyCode? keyCode = this.m_keyCode;
		if (keyCode != null)
		{
			KeyCode? keyCode2 = this.m_keyCode;
			flag = Input.GetKey((keyCode2 == null) ? KeyCode.A : keyCode2.Value);
		}
		base.Update(flag);
		return flag;
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00034EDD File Offset: 0x000332DD
	public override void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		_tree = new AcyclicGraph<ILogicalElement, LogicalLinkInfo>(this);
		_head = _tree.GetNode(this);
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00034EF1 File Offset: 0x000332F1
	public ButtonId GetControlButton()
	{
		return this.m_button;
	}

	// Token: 0x040007B9 RID: 1977
	protected KeyCode? m_keyCode;

	// Token: 0x040007BA RID: 1978
	protected ButtonId m_button;
}
