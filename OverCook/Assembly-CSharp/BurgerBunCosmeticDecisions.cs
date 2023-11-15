using System;
using UnityEngine;

// Token: 0x02000388 RID: 904
public class BurgerBunCosmeticDecisions : MealCosmeticDecisions
{
	// Token: 0x06001119 RID: 4377 RVA: 0x000620E8 File Offset: 0x000604E8
	protected override void Start()
	{
		if (this.m_prefabLookup != null)
		{
			this.m_prefabLookup.CacheAssembledOrderNodes();
		}
		base.Start();
		Mesh mesh = this.m_base.RequireComponent<MeshFilter>().mesh;
		Vector3 position = this.m_base.transform.TransformPoint(new Vector3(0f, mesh.bounds.size.y, 0f));
		this.m_container.transform.localPosition = this.m_container.transform.parent.InverseTransformPoint(position);
		this.UpdateAppearance(this.m_iOrderDefinition.GetOrderComposition());
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x00062195 File Offset: 0x00060595
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x000621A4 File Offset: 0x000605A4
	protected override void UpdateAppearance(AssembledDefinitionNode _contents)
	{
		AssembledDefinitionNode assembledDefinitionNode = _contents.Simpilfy();
		CompositeAssembledNode compositeAssembledNode = assembledDefinitionNode as CompositeAssembledNode;
		if (compositeAssembledNode != null && compositeAssembledNode.m_composition.Length > 0)
		{
			bool active = this.ContainsBap(compositeAssembledNode);
			this.m_lid.SetActive(active);
			this.m_base.SetActive(active);
			this.m_empty.SetActive(false);
			this.LayoutContents(compositeAssembledNode);
		}
		else
		{
			this.m_lid.SetActive(false);
			this.m_base.SetActive(false);
			this.m_empty.SetActive(true);
			this.DestroyContents();
		}
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x00062238 File Offset: 0x00060638
	private bool ContainsBap(CompositeAssembledNode _composite)
	{
		for (int i = 0; i < _composite.m_composition.Length; i++)
		{
			if (AssembledDefinitionNode.Matching(_composite.m_composition[i], this.m_bapOrderDefinition))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600111D RID: 4381 RVA: 0x0006227C File Offset: 0x0006067C
	private void DestroyContents()
	{
		for (int i = 0; i < this.m_container.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.m_container.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x000622C8 File Offset: 0x000606C8
	private void LayoutContents(CompositeAssembledNode _composite)
	{
		this.DestroyContents();
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < _composite.m_composition.Length; i++)
		{
			GameObject burgerPrefab = this.GetBurgerPrefab(_composite.m_composition[i]);
			if (burgerPrefab != null)
			{
				GameObject gameObject = burgerPrefab.InstantiateOnParent(this.m_container.transform, true);
				MeshFilter meshFilter = gameObject.RequireComponentRecursive<MeshFilter>();
				gameObject.transform.localPosition = vector;
				float y = meshFilter.sharedMesh.bounds.size.y;
				vector += Vector3.up * y * gameObject.transform.localScale.y;
			}
		}
		Vector3 position = this.m_container.transform.TransformPoint(vector);
		this.m_lid.transform.localPosition = this.m_lid.transform.parent.InverseTransformPoint(position);
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x000623C4 File Offset: 0x000607C4
	private GameObject GetBurgerPrefab(AssembledDefinitionNode _providedNode)
	{
		GameObject prefabForNode = this.m_prefabLookup.GetPrefabForNode(_providedNode);
		if (prefabForNode)
		{
			return prefabForNode;
		}
		return null;
	}

	// Token: 0x04000D3E RID: 3390
	[SerializeField]
	private GameObject m_lid;

	// Token: 0x04000D3F RID: 3391
	[SerializeField]
	private GameObject m_base;

	// Token: 0x04000D40 RID: 3392
	[SerializeField]
	private GameObject m_empty;

	// Token: 0x04000D41 RID: 3393
	[SerializeField]
	private OrderToPrefabLookup m_prefabLookup;

	// Token: 0x04000D42 RID: 3394
	[SerializeField]
	private OrderDefinitionNode m_bapOrderDefinition;
}
