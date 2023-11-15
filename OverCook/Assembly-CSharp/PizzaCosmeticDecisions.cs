using System;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public class PizzaCosmeticDecisions : MealCosmeticDecisions
{
	// Token: 0x0600125C RID: 4700 RVA: 0x00067796 File Offset: 0x00065B96
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x000677A3 File Offset: 0x00065BA3
	protected override void Start()
	{
		if (this.m_cookedPrefabLookup != null)
		{
			this.m_cookedPrefabLookup.CacheAssembledOrderNodes();
		}
		if (this.m_uncookedPrefabLookup != null)
		{
			this.m_uncookedPrefabLookup.CacheAssembledOrderNodes();
		}
		base.Start();
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x000677E3 File Offset: 0x00065BE3
	protected override void UpdateAppearance(AssembledDefinitionNode _contents)
	{
		this.DestroyAllRenderChildren();
		if (_contents != null)
		{
			this.CreateRenderChildren(_contents);
		}
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x000677F8 File Offset: 0x00065BF8
	private void DestroyAllRenderChildren()
	{
		for (int i = this.m_container.transform.childCount - 1; i >= 0; i--)
		{
			Transform child = this.m_container.transform.GetChild(i);
			child.transform.SetParent(null);
			UnityEngine.Object.Destroy(child.gameObject);
		}
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x00067854 File Offset: 0x00065C54
	private void CreateRenderChildren(AssembledDefinitionNode _order)
	{
		CookedCompositeAssembledNode cookedCompositeAssembledNode = _order as CookedCompositeAssembledNode;
		this.m_cooked = (cookedCompositeAssembledNode != null && cookedCompositeAssembledNode.m_progress != CookedCompositeOrderNode.CookingProgress.Raw);
		this.m_burnt = (cookedCompositeAssembledNode != null && cookedCompositeAssembledNode.m_progress == CookedCompositeOrderNode.CookingProgress.Burnt);
		bool flag = false;
		CompositeAssembledNode compositeAssembledNode = _order as CompositeAssembledNode;
		if (compositeAssembledNode != null)
		{
			flag = (compositeAssembledNode.m_composition.FindIndex_Predicate((AssembledDefinitionNode x) => AssembledDefinitionNode.Matching(x, this.m_doughIngredient)) != -1);
		}
		this.m_uncookedBase.SetActive(flag && !this.m_cooked);
		this.m_cookedBase.SetActive(flag && this.m_cooked);
		this.m_steamPFX.gameObject.SetActive(this.m_cooked);
		if (this.m_cooked)
		{
			this.m_steamPFX.Play();
		}
		else if (this.m_cooked)
		{
			this.m_steamPFX.Stop();
		}
		ParticleSystem.MainModule main = this.m_steamPFX.main;
		if (this.m_burnt)
		{
			main.startColor = Color.black;
		}
		else
		{
			main.startColor = Color.white;
		}
		if (compositeAssembledNode != null)
		{
			for (int i = 0; i < compositeAssembledNode.m_composition.Length; i++)
			{
				GameObject prefabForNode = this.GetPrefabForNode(compositeAssembledNode.m_composition[i]);
				if (prefabForNode != null)
				{
					prefabForNode.InstantiateOnParent(this.m_container.transform, true);
				}
			}
		}
		Vector3 zero = Vector3.zero;
		this.m_container.transform.localPosition = zero;
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x000679F2 File Offset: 0x00065DF2
	private GameObject GetPrefabForNode(AssembledDefinitionNode _order)
	{
		if (AssembledDefinitionNode.Matching(_order, this.m_doughIngredient))
		{
			return null;
		}
		if (this.m_cooked)
		{
			return this.m_cookedPrefabLookup.GetPrefabForNode(_order);
		}
		return this.m_uncookedPrefabLookup.GetPrefabForNode(_order);
	}

	// Token: 0x04000E5B RID: 3675
	[SerializeField]
	private OrderToPrefabLookup m_cookedPrefabLookup;

	// Token: 0x04000E5C RID: 3676
	[SerializeField]
	private OrderToPrefabLookup m_uncookedPrefabLookup;

	// Token: 0x04000E5D RID: 3677
	[SerializeField]
	private ParticleSystem m_steamPFX;

	// Token: 0x04000E5E RID: 3678
	[SerializeField]
	[AssignResource("Dough", Editorbility.NonEditable)]
	private IngredientOrderNode m_doughIngredient;

	// Token: 0x04000E5F RID: 3679
	[SerializeField]
	private GameObject m_uncookedBase;

	// Token: 0x04000E60 RID: 3680
	[SerializeField]
	private GameObject m_cookedBase;

	// Token: 0x04000E61 RID: 3681
	[SerializeField]
	private float m_cookedContentsOffset = 0.04f;

	// Token: 0x04000E62 RID: 3682
	private bool m_cooked;

	// Token: 0x04000E63 RID: 3683
	private bool m_burnt;
}
