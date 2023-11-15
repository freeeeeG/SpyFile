using System;
using UnityEngine;

// Token: 0x02000400 RID: 1024
public class ToastingContentsCosmeticDecisions : OverlapModelsMealDecisions
{
	// Token: 0x060012A2 RID: 4770 RVA: 0x00068A40 File Offset: 0x00066E40
	protected override void Start()
	{
		base.Start();
		if (this.m_cookedPrefabLookup != null)
		{
			this.m_cookedPrefabLookup.CacheAssembledOrderNodes();
		}
		this.m_container.transform.localPosition = new Vector3(this.m_contentsOffset, 0f, 0f);
		this.m_container.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x00068AB8 File Offset: 0x00066EB8
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x00068AC8 File Offset: 0x00066EC8
	protected override void CreateRenderChildren(AssembledDefinitionNode _contents)
	{
		base.CreateRenderChildren(_contents);
		if (_contents.Simpilfy() != AssembledDefinitionNode.NullNode && this.m_container.transform.childCount == 0 && this.m_burntPrefab != null)
		{
			this.m_burntPrefab.InstantiateOnParent(this.m_container.transform, true);
		}
	}

	// Token: 0x04000E9F RID: 3743
	[SerializeField]
	private OrderToPrefabLookup m_cookedPrefabLookup;

	// Token: 0x04000EA0 RID: 3744
	[SerializeField]
	private GameObject m_burntPrefab;

	// Token: 0x04000EA1 RID: 3745
	[SerializeField]
	private float m_contentsOffset = 0.666f;
}
