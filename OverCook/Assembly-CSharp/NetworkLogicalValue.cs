using System;

// Token: 0x02000208 RID: 520
public class NetworkLogicalValue : ILogicalValue, ILogicalElement
{
	// Token: 0x060008AF RID: 2223 RVA: 0x00034998 File Offset: 0x00032D98
	public NetworkLogicalValue(ILogicalValue _valueToNetwork, PlayerInputLookup.LogicalValueID _valueID)
	{
		this.m_LogicalValue = _valueToNetwork;
		this.m_ValueID = _valueID;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x000349AE File Offset: 0x00032DAE
	public virtual float GetValue()
	{
		return this.m_Value;
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x000349B6 File Offset: 0x00032DB6
	public void SetValue(float _value)
	{
		this.m_Value = _value;
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x000349BF File Offset: 0x00032DBF
	public PlayerInputLookup.LogicalValueID GetLogicalID()
	{
		return this.m_ValueID;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x000349C8 File Offset: 0x00032DC8
	public void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node node;
		this.m_LogicalValue.GetLogicTreeData(out _tree, out node);
		_tree.AddLink(this, node.m_value, new LogicalLinkInfo());
		_head = _tree.GetNode(this);
	}

	// Token: 0x040007A5 RID: 1957
	private PlayerInputLookup.LogicalValueID m_ValueID;

	// Token: 0x040007A6 RID: 1958
	private ILogicalValue m_LogicalValue;

	// Token: 0x040007A7 RID: 1959
	private float m_Value;
}
