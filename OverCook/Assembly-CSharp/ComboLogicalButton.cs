using System;

// Token: 0x020001FF RID: 511
public class ComboLogicalButton : LogicalButtonBase
{
	// Token: 0x06000895 RID: 2197 RVA: 0x000345E0 File Offset: 0x000329E0
	public ComboLogicalButton(ILogicalButton[] _buttons, ComboLogicalButton.ComboType _comboType = ComboLogicalButton.ComboType.Or)
	{
		this.m_baseButtons = _buttons;
		this.m_comboType = _comboType;
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x000345F8 File Offset: 0x000329F8
	public override bool IsDown()
	{
		bool flag = false;
		ComboLogicalButton.ComboType comboType = this.m_comboType;
		if (comboType != ComboLogicalButton.ComboType.And)
		{
			if (comboType == ComboLogicalButton.ComboType.Or)
			{
				flag = false;
				for (int i = 0; i < this.m_baseButtons.Length; i++)
				{
					flag = (flag || this.m_baseButtons[i].IsDown());
				}
			}
		}
		else
		{
			flag = true;
			for (int j = 0; j < this.m_baseButtons.Length; j++)
			{
				flag = (flag && this.m_baseButtons[j].IsDown());
			}
		}
		base.Update(flag);
		return flag;
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x00034698 File Offset: 0x00032A98
	public override void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
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

	// Token: 0x04000793 RID: 1939
	private ILogicalButton[] m_baseButtons;

	// Token: 0x04000794 RID: 1940
	private ComboLogicalButton.ComboType m_comboType;

	// Token: 0x02000200 RID: 512
	public enum ComboType
	{
		// Token: 0x04000796 RID: 1942
		And,
		// Token: 0x04000797 RID: 1943
		Or
	}
}
