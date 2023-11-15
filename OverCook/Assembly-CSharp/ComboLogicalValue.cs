using System;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class ComboLogicalValue : ILogicalValue, ILogicalElement
{
	// Token: 0x06000898 RID: 2200 RVA: 0x00034705 File Offset: 0x00032B05
	public ComboLogicalValue(ILogicalValue[] _buttons, ComboLogicalValue.ComboType _comboType = ComboLogicalValue.ComboType.AbsMax)
	{
		this.m_baseButtons = _buttons;
		this.m_valueFn = this.GetValueFunction(_comboType);
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x00034721 File Offset: 0x00032B21
	public virtual float GetValue()
	{
		return this.m_valueFn();
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00034730 File Offset: 0x00032B30
	private float GetAbsMaxValue()
	{
		float num = 0f;
		for (int i = 0; i < this.m_baseButtons.Length; i++)
		{
			float value = this.m_baseButtons[i].GetValue();
			if (Mathf.Abs(value) > Mathf.Abs(num))
			{
				num = value;
			}
		}
		return num;
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0003477E File Offset: 0x00032B7E
	private ComboLogicalValue.Void2Float GetValueFunction(ComboLogicalValue.ComboType _comboType)
	{
		if (_comboType != ComboLogicalValue.ComboType.AbsMax)
		{
			return () => 0f;
		}
		return new ComboLogicalValue.Void2Float(this.GetAbsMaxValue);
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x000347B8 File Offset: 0x00032BB8
	public void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		_tree = new AcyclicGraph<ILogicalElement, LogicalLinkInfo>(this);
		_head = _tree.GetNode(this);
		for (int i = 0; i < this.m_baseButtons.Length; i++)
		{
			AcyclicGraph<ILogicalElement, LogicalLinkInfo> graph;
			AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node node;
			this.m_baseButtons[i].GetLogicTreeData(out graph, out node);
			_tree = AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Merge(_tree, graph);
			_tree.AddLink(node.m_value, _head.m_value, new LogicalLinkInfo());
		}
	}

	// Token: 0x04000798 RID: 1944
	private ILogicalValue[] m_baseButtons;

	// Token: 0x04000799 RID: 1945
	private ComboLogicalValue.Void2Float m_valueFn;

	// Token: 0x02000202 RID: 514
	// (Invoke) Token: 0x0600089F RID: 2207
	private delegate float Void2Float();

	// Token: 0x02000203 RID: 515
	public enum ComboType
	{
		// Token: 0x0400079C RID: 1948
		AbsMax
	}
}
