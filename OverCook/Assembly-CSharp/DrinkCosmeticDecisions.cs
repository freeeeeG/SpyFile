using System;
using UnityEngine;

// Token: 0x020003AE RID: 942
public class DrinkCosmeticDecisions : MealCosmeticDecisions
{
	// Token: 0x060011A5 RID: 4517 RVA: 0x00064F10 File Offset: 0x00063310
	protected override void Start()
	{
		this.m_renderers = base.gameObject.GetComponentsInChildren<Renderer>();
		base.Start();
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x00064F29 File Offset: 0x00063329
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x00064F38 File Offset: 0x00063338
	protected override void UpdateAppearance(AssembledDefinitionNode _contents)
	{
		AssembledDefinitionNode assembledDefinitionNode = _contents.Simpilfy();
		CompositeAssembledNode compositeAssembledNode = assembledDefinitionNode as CompositeAssembledNode;
		Material material = null;
		if (compositeAssembledNode != null)
		{
			for (int i = 0; i < compositeAssembledNode.m_composition.Length; i++)
			{
				material = this.GetDrinkMaterial(compositeAssembledNode.m_composition[i]);
				if (material)
				{
					break;
				}
			}
		}
		else
		{
			material = this.GetDrinkMaterial(_contents);
		}
		if (material)
		{
			for (int j = 0; j < this.m_renderers.Length; j++)
			{
				this.m_renderers[j].material = material;
			}
		}
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x00064FD8 File Offset: 0x000633D8
	private Material GetDrinkMaterial(AssembledDefinitionNode _node)
	{
		for (int i = 0; i < this.m_materialLookup.Length; i++)
		{
			if (AssembledDefinitionNode.Matching(_node, this.m_materialLookup[i].m_content))
			{
				return this.m_materialLookup[i].m_material;
			}
		}
		return null;
	}

	// Token: 0x04000DBC RID: 3516
	[SerializeField]
	private DrinkCosmeticDecisions.OrderToMaterial[] m_materialLookup = new DrinkCosmeticDecisions.OrderToMaterial[0];

	// Token: 0x04000DBD RID: 3517
	private Renderer[] m_renderers;

	// Token: 0x020003AF RID: 943
	[Serializable]
	public class OrderToMaterial
	{
		// Token: 0x04000DBE RID: 3518
		public OrderDefinitionNode m_content;

		// Token: 0x04000DBF RID: 3519
		public Material m_material;
	}
}
