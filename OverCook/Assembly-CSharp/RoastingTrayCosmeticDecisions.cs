using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003EC RID: 1004
public class RoastingTrayCosmeticDecisions : OverlapModelsMealDecisions
{
	// Token: 0x0600126D RID: 4717 RVA: 0x00067CD2 File Offset: 0x000660D2
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x00067CE0 File Offset: 0x000660E0
	protected override void Start()
	{
		base.Start();
		this.m_container.transform.localPosition = new Vector3(0f, 0f, 0f);
		if (this.m_repositionLookup != null)
		{
			this.m_repositionLookup.CacheAssembledOrderNodes();
		}
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x00067D34 File Offset: 0x00066134
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
		}
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x00067DE8 File Offset: 0x000661E8
	protected override void CreateRenderChildren(AssembledDefinitionNode _contents)
	{
		this.m_nodesToReposition.Clear();
		IEnumerator<AssembledDefinitionNode> enumerator = base.ShallowIteration(_contents);
		while (enumerator.MoveNext())
		{
			AssembledDefinitionNode assembledDefinitionNode = enumerator.Current;
			GameObject renderPefabForNode = this.GetRenderPefabForNode(assembledDefinitionNode);
			if (renderPefabForNode != null)
			{
				GameObject item = renderPefabForNode.InstantiateOnParent(this.m_container.transform, true);
				if (this.m_repositionLookup != null && this.m_repositionLookup.GetPrefabForNode(assembledDefinitionNode) != null)
				{
					this.m_nodesToReposition.Add(item);
				}
			}
		}
		this.RepositionRenderChildren(this.m_nodesToReposition, Mathf.Min(this.m_nodesToReposition.Count, 4));
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x00067E98 File Offset: 0x00066298
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

	// Token: 0x06001272 RID: 4722 RVA: 0x00067EDC File Offset: 0x000662DC
	protected virtual void RepositionRenderChildren(List<GameObject> nodes, int _totalSections)
	{
		if (nodes.Count <= 1)
		{
			return;
		}
		Quaternion quaternion = Quaternion.Euler(0f, -360f / (float)_totalSections, 0f);
		Vector3 vector = new Vector3(this.m_modelOffsetXZ.x, 0f, this.m_modelOffsetXZ.y);
		Quaternion quaternion2 = Quaternion.identity;
		for (int i = 0; i < nodes.Count; i++)
		{
			Transform child = this.m_container.transform.GetChild(i);
			child.localPosition = vector;
			child.localRotation = quaternion2;
			vector = quaternion * vector;
			quaternion2 = quaternion * quaternion2;
		}
	}

	// Token: 0x04000E70 RID: 3696
	private const int c_panSections = 4;

	// Token: 0x04000E71 RID: 3697
	[SerializeField]
	private OrderToPrefabLookup m_repositionLookup;

	// Token: 0x04000E72 RID: 3698
	[SerializeField]
	private GameObject m_burntPrefab;

	// Token: 0x04000E73 RID: 3699
	[SerializeField]
	private Vector2 m_modelOffsetXZ = Vector2.zero;

	// Token: 0x04000E74 RID: 3700
	private List<GameObject> m_nodesToReposition = new List<GameObject>();
}
