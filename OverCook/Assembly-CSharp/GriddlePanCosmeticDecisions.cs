using System;
using UnityEngine;

// Token: 0x020003C1 RID: 961
public class GriddlePanCosmeticDecisions : OverlapModelsMealDecisions
{
	// Token: 0x060011E4 RID: 4580 RVA: 0x00065D7D File Offset: 0x0006417D
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x060011E5 RID: 4581 RVA: 0x00065D8A File Offset: 0x0006418A
	protected override void Start()
	{
		base.Start();
		this.m_container.transform.localPosition = new Vector3(0f, 0.1f, 0f);
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x00065DB8 File Offset: 0x000641B8
	protected override void UpdateAppearance(AssembledDefinitionNode _contents)
	{
		base.UpdateAppearance(_contents);
		if (_contents != null)
		{
			bool flag = _contents is CookedCompositeAssembledNode && (_contents as CookedCompositeAssembledNode).m_progress == CookedCompositeOrderNode.CookingProgress.Burnt;
			bool flag2 = _contents.Simpilfy() != AssembledDefinitionNode.NullNode && this.m_container.transform.childCount == 0;
			if ((flag || flag2) && this.m_burntPrefab != null)
			{
				this.DestroyAllRenderingInstances();
				int nodeCount = _contents.GetNodeCount();
				for (int i = 0; i < 4; i++)
				{
					GameObject gameObject = this.m_burntPrefab.InstantiateOnParent(this.m_container.transform, true);
				}
			}
			this.RepositionRenderChildren(4);
		}
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x00065E74 File Offset: 0x00064274
	protected override GameObject GetRenderPefabForNode(AssembledDefinitionNode _order)
	{
		if (_order is CookedCompositeAssembledNode)
		{
			CookedCompositeAssembledNode cookedCompositeAssembledNode = _order as CookedCompositeAssembledNode;
			if (cookedCompositeAssembledNode.m_composition.Length == 1)
			{
				return base.GetRenderPefabForNode(cookedCompositeAssembledNode.m_composition[0]);
			}
		}
		return base.GetRenderPefabForNode(_order);
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x00065EB8 File Offset: 0x000642B8
	protected virtual void RepositionRenderChildren(int _totalSections)
	{
		int childCount = this.m_container.transform.childCount;
		if (childCount == 0)
		{
			return;
		}
		Quaternion quaternion = Quaternion.Euler(0f, -360f / (float)_totalSections, 0f);
		Vector3 vector = new Vector3(this.m_modelOffsetXZ.x, 0f, this.m_modelOffsetXZ.y);
		Quaternion quaternion2 = Quaternion.identity;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = this.m_container.transform.GetChild(i);
			child.localPosition = vector;
			child.localRotation = quaternion2;
			vector = quaternion * vector;
			quaternion2 = quaternion * quaternion2;
		}
	}

	// Token: 0x04000DF4 RID: 3572
	private const int c_panSections = 4;

	// Token: 0x04000DF5 RID: 3573
	[SerializeField]
	private GameObject m_burntPrefab;

	// Token: 0x04000DF6 RID: 3574
	[SerializeField]
	private Vector2 m_modelOffsetXZ = Vector2.zero;
}
