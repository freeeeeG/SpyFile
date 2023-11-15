using System;
using UnityEngine;

// Token: 0x02000211 RID: 529
public class LogicalKeycodeValue : ILogicalValue, ILogicalElement
{
	// Token: 0x060008D3 RID: 2259 RVA: 0x00034F0B File Offset: 0x0003330B
	public LogicalKeycodeValue(ValueCode? _code, ILogicalButton _nveButton, ILogicalButton _pveButton)
	{
		this.m_valueCode = _code;
		this.m_negativeButton = _nveButton;
		this.m_positiveButton = _pveButton;
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x00034F3E File Offset: 0x0003333E
	public LogicalKeycodeValue(ValueCode? _code)
	{
		this.m_valueCode = _code;
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x00034F63 File Offset: 0x00033363
	public LogicalKeycodeValue()
	{
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00034F84 File Offset: 0x00033384
	public virtual float GetValue()
	{
		if (this.m_positiveButton.IsDown())
		{
			return 1f;
		}
		if (this.m_negativeButton.IsDown())
		{
			return -1f;
		}
		ValueCode? valueCode = this.m_valueCode;
		if (valueCode != null)
		{
			ValueCode? valueCode2 = this.m_valueCode;
			return Input.GetAxis(((valueCode2 == null) ? ValueCode.Joystick1Axis1 : valueCode2.Value).ToString());
		}
		return 0f;
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x00035009 File Offset: 0x00033409
	public virtual void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		_tree = new AcyclicGraph<ILogicalElement, LogicalLinkInfo>(this);
		_head = _tree.GetNode(this);
	}

	// Token: 0x040007BB RID: 1979
	private ValueCode? m_valueCode;

	// Token: 0x040007BC RID: 1980
	private ILogicalButton m_positiveButton = new LogicalKeycodeButton();

	// Token: 0x040007BD RID: 1981
	private ILogicalButton m_negativeButton = new LogicalKeycodeButton();
}
