using System;
using UnityEngine;

// Token: 0x020007E5 RID: 2021
public class AssignableOrderDefinition : MonoBehaviour, IClientOrderDefinition, IAssignOrderDefinition
{
	// Token: 0x060026E2 RID: 9954 RVA: 0x000B896C File Offset: 0x000B6D6C
	public void Awake()
	{
		if (this.m_orderData != null)
		{
			this.m_assembledOrderData = this.m_orderData.Simpilfy();
		}
		else
		{
			this.m_assembledOrderData = AssembledDefinitionNode.NullNode;
		}
	}

	// Token: 0x060026E3 RID: 9955 RVA: 0x000B89A0 File Offset: 0x000B6DA0
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.m_assembledOrderData;
	}

	// Token: 0x060026E4 RID: 9956 RVA: 0x000B89A8 File Offset: 0x000B6DA8
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x060026E5 RID: 9957 RVA: 0x000B89C1 File Offset: 0x000B6DC1
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x060026E6 RID: 9958 RVA: 0x000B89DA File Offset: 0x000B6DDA
	public void SetOrderComposition(AssembledDefinitionNode _data)
	{
		this.m_assembledOrderData = _data;
		this.m_orderCompositionChangedCallbacks(this.m_assembledOrderData);
	}

	// Token: 0x04001EC5 RID: 7877
	[SerializeField]
	private OrderDefinitionNode m_orderData;

	// Token: 0x04001EC6 RID: 7878
	private AssembledDefinitionNode m_assembledOrderData;

	// Token: 0x04001EC7 RID: 7879
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};
}
