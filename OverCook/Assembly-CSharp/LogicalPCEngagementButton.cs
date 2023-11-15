using System;
using UnityEngine;

// Token: 0x02000218 RID: 536
public class LogicalPCEngagementButton : LogicalButtonBase
{
	// Token: 0x060008EA RID: 2282 RVA: 0x00035287 File Offset: 0x00033687
	public LogicalPCEngagementButton(ControlPadInput.PadNum _pad)
	{
		this.m_InputProvider = Singleton<PCPadInputProvider>.Get();
		this.m_Pad = _pad;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x000352A1 File Offset: 0x000336A1
	public override void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		_tree = new AcyclicGraph<ILogicalElement, LogicalLinkInfo>(this);
		_head = _tree.GetNode(this);
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x000352B8 File Offset: 0x000336B8
	public override bool IsDown()
	{
		if (!Application.isFocused)
		{
			base.Update(false);
			return false;
		}
		bool flag = this.m_InputProvider.IsEngagementDown(this.m_Pad);
		base.Update(flag);
		return flag;
	}

	// Token: 0x040007CA RID: 1994
	protected PCPadInputProvider m_InputProvider;

	// Token: 0x040007CB RID: 1995
	protected ControlPadInput.PadNum m_Pad;
}
