using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000412 RID: 1042
public class WokCosmeticDecisions : MealCosmeticDecisions
{
	// Token: 0x060012D1 RID: 4817 RVA: 0x000698FB File Offset: 0x00067CFB
	protected override void Start()
	{
		this.m_prefabLookups.CacheAssembledOrderNodes();
		base.Start();
		this.m_ingredientContainer = base.gameObject.RequestComponentUpwardsRecursive<IngredientContainer>();
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x0006991F File Offset: 0x00067D1F
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x0006992C File Offset: 0x00067D2C
	protected override void UpdateAppearance(AssembledDefinitionNode _contents)
	{
		this.DestroyContents();
		CompositeAssembledNode compositeAssembledNode = _contents as CompositeAssembledNode;
		if (compositeAssembledNode == null || compositeAssembledNode.m_composition.Length == 0)
		{
			return;
		}
		this.CreateContents(compositeAssembledNode);
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x00069964 File Offset: 0x00067D64
	private void CreateContents(CompositeAssembledNode _composite)
	{
		CookedCompositeAssembledNode cookedCompositeAssembledNode = _composite as CookedCompositeAssembledNode;
		OrderToPrefabLookup prefabLookup = this.m_prefabLookups.GetPrefabLookup((cookedCompositeAssembledNode == null) ? CookedCompositeOrderNode.CookingProgress.Raw : cookedCompositeAssembledNode.m_progress);
		if (prefabLookup == null)
		{
			return;
		}
		for (int i = 0; i < _composite.m_composition.Length; i++)
		{
			GameObject prefabForNode = prefabLookup.GetPrefabForNode(_composite.m_composition[i]);
			if (!(prefabForNode == null))
			{
				this.m_prefabsToSpawn.Add(prefabForNode);
			}
		}
		float time = Mathf.Clamp01((float)_composite.m_composition.Length / (float)this.m_ingredientContainer.m_capacity);
		this.m_container.transform.localPosition = Vector3.Lerp(this.m_contentsOffset.m_empty, this.m_contentsOffset.m_full, this.m_contentsOffset.m_curve.Evaluate(time));
		for (int j = 0; j < this.m_prefabsToSpawn.Count; j++)
		{
			GameObject gameObject = this.m_prefabsToSpawn[j];
			if (!(gameObject == null))
			{
				GameObject gameObject2 = gameObject.InstantiateOnParent(this.m_container.transform, true);
				if (this.m_rotationType != WokCosmeticDecisions.RotationType.None)
				{
					float angle = 0f;
					WokCosmeticDecisions.RotationType rotationType = this.m_rotationType;
					if (rotationType != WokCosmeticDecisions.RotationType.Ordered)
					{
						if (rotationType == WokCosmeticDecisions.RotationType.Random)
						{
							angle = UnityEngine.Random.Range(-180f, 180f);
						}
					}
					else
					{
						angle = this.m_orderedData.m_angles[j];
					}
					gameObject2.transform.localRotation = Quaternion.AngleAxis(angle, gameObject2.transform.up);
				}
			}
		}
		this.m_prefabsToSpawn.Clear();
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x00069B24 File Offset: 0x00067F24
	private void DestroyContents()
	{
		int childCount = this.m_container.transform.childCount;
		for (int i = childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.m_container.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x04000ED5 RID: 3797
	[SerializeField]
	private WokCosmeticDecisions.PrefabLookups m_prefabLookups;

	// Token: 0x04000ED6 RID: 3798
	[SerializeField]
	public WokCosmeticDecisions.ContentsOffset m_contentsOffset = new WokCosmeticDecisions.ContentsOffset();

	// Token: 0x04000ED7 RID: 3799
	[SerializeField]
	private WokCosmeticDecisions.RotationType m_rotationType;

	// Token: 0x04000ED8 RID: 3800
	[SerializeField]
	[HideInInspectorTest("m_rotationType", WokCosmeticDecisions.RotationType.Ordered)]
	private WokCosmeticDecisions.OrderedData m_orderedData;

	// Token: 0x04000ED9 RID: 3801
	private List<GameObject> m_prefabsToSpawn = new List<GameObject>();

	// Token: 0x04000EDA RID: 3802
	private IngredientContainer m_ingredientContainer;

	// Token: 0x02000413 RID: 1043
	[Serializable]
	private struct PrefabLookups
	{
		// Token: 0x060012D6 RID: 4822 RVA: 0x00069B74 File Offset: 0x00067F74
		public void CacheAssembledOrderNodes()
		{
			if (this.m_rawPrefabLookup != null)
			{
				this.m_rawPrefabLookup.CacheAssembledOrderNodes();
			}
			if (this.m_cookedPrefabLookup != null)
			{
				this.m_cookedPrefabLookup.CacheAssembledOrderNodes();
			}
			if (this.m_burntPrefabLookup != null)
			{
				this.m_burntPrefabLookup.CacheAssembledOrderNodes();
			}
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00069BD5 File Offset: 0x00067FD5
		public OrderToPrefabLookup GetPrefabLookup(CookedCompositeOrderNode.CookingProgress _progress)
		{
			if (_progress == CookedCompositeOrderNode.CookingProgress.Raw)
			{
				return this.m_rawPrefabLookup;
			}
			if (_progress == CookedCompositeOrderNode.CookingProgress.Cooked)
			{
				return this.m_cookedPrefabLookup;
			}
			if (_progress != CookedCompositeOrderNode.CookingProgress.Burnt)
			{
				return null;
			}
			return this.m_burntPrefabLookup;
		}

		// Token: 0x04000EDB RID: 3803
		[AssignResource("DLC04_LargePotCookableObjectsLookup", Editorbility.Editable)]
		[SerializeField]
		public OrderToPrefabLookup m_rawPrefabLookup;

		// Token: 0x04000EDC RID: 3804
		[AssignResource("DLC04_LargePotCookableObjectsLookup", Editorbility.Editable)]
		[SerializeField]
		public OrderToPrefabLookup m_cookedPrefabLookup;

		// Token: 0x04000EDD RID: 3805
		[SerializeField]
		public OrderToPrefabLookup m_burntPrefabLookup;
	}

	// Token: 0x02000414 RID: 1044
	[Serializable]
	public class ContentsOffset
	{
		// Token: 0x04000EDE RID: 3806
		[SerializeField]
		public Vector3 m_empty = new Vector3(0f, 0f, 0f);

		// Token: 0x04000EDF RID: 3807
		[SerializeField]
		public Vector3 m_full = new Vector3(0f, 0f, 0f);

		// Token: 0x04000EE0 RID: 3808
		[SerializeField]
		public AnimationCurve m_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	}

	// Token: 0x02000415 RID: 1045
	[Serializable]
	private enum RotationType
	{
		// Token: 0x04000EE2 RID: 3810
		None,
		// Token: 0x04000EE3 RID: 3811
		Random,
		// Token: 0x04000EE4 RID: 3812
		Ordered
	}

	// Token: 0x02000416 RID: 1046
	[Serializable]
	private struct OrderedData
	{
		// Token: 0x04000EE5 RID: 3813
		[SerializeField]
		public float[] m_angles;
	}
}
