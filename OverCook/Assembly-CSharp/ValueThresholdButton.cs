using System;
using UnityEngine;

// Token: 0x0200020D RID: 525
public class ValueThresholdButton : LogicalButtonBase
{
	// Token: 0x060008C9 RID: 2249 RVA: 0x00034D61 File Offset: 0x00033161
	public ValueThresholdButton(ILogicalValue _logicalValue, ValueThresholdButton.ThresholdType _type, float _thldValue)
	{
		this.m_baseValue = _logicalValue;
		this.m_thldType = _type;
		this.m_thldValue = _thldValue;
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00034D80 File Offset: 0x00033180
	public override bool IsDown()
	{
		if (!this.CanProcessInput())
		{
			base.Update(false);
			return false;
		}
		bool flag = true;
		float value = this.m_baseValue.GetValue();
		switch (this.m_thldType)
		{
		case ValueThresholdButton.ThresholdType.Greater:
			flag = (value > this.m_thldValue);
			break;
		case ValueThresholdButton.ThresholdType.LessThan:
			flag = (value < this.m_thldValue);
			break;
		case ValueThresholdButton.ThresholdType.AbsGreater:
			flag = (Mathf.Abs(value) > this.m_thldValue);
			break;
		case ValueThresholdButton.ThresholdType.AbsLessThan:
			flag = (Mathf.Abs(value) < this.m_thldValue);
			break;
		}
		base.Update(flag);
		return flag;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x00034E20 File Offset: 0x00033220
	public override void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node node;
		this.m_baseValue.GetLogicTreeData(out _tree, out node);
		_tree.AddLink(this, node.m_value, new LogicalLinkInfo());
		_head = _tree.GetNode(this);
	}

	// Token: 0x040007B1 RID: 1969
	public ILogicalValue m_baseValue;

	// Token: 0x040007B2 RID: 1970
	public float m_thldValue;

	// Token: 0x040007B3 RID: 1971
	public ValueThresholdButton.ThresholdType m_thldType;

	// Token: 0x0200020E RID: 526
	public enum ThresholdType
	{
		// Token: 0x040007B5 RID: 1973
		Greater,
		// Token: 0x040007B6 RID: 1974
		LessThan,
		// Token: 0x040007B7 RID: 1975
		AbsGreater,
		// Token: 0x040007B8 RID: 1976
		AbsLessThan
	}
}
