using System;
using UnityEngine;

// Token: 0x020003EE RID: 1006
public class SkewerCosmeticDecisions : MealCosmeticDecisions
{
	// Token: 0x06001275 RID: 4725 RVA: 0x00067FB8 File Offset: 0x000663B8
	protected override void Start()
	{
		if (this.m_prefabLookup != null)
		{
			this.m_prefabLookup.CacheAssembledOrderNodes();
		}
		if (this.m_cookedPrefabLookup != null)
		{
			this.m_cookedPrefabLookup.CacheAssembledOrderNodes();
		}
		base.Start();
		this.m_container.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
		this.m_container.transform.localPosition = this.m_container.transform.localPosition.WithX(this.m_contentsOffset);
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x0006804D File Offset: 0x0006644D
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x0006805C File Offset: 0x0006645C
	protected override void UpdateAppearance(AssembledDefinitionNode _contents)
	{
		bool flag = false;
		CookedCompositeAssembledNode cookedCompositeAssembledNode = _contents as CookedCompositeAssembledNode;
		if (cookedCompositeAssembledNode != null)
		{
			flag = (cookedCompositeAssembledNode.m_progress == CookedCompositeOrderNode.CookingProgress.Burnt);
		}
		if (flag)
		{
			this.DestroyContents();
		}
		else
		{
			this.CreateContents(_contents);
		}
		if ((flag || (_contents.Simpilfy() != AssembledDefinitionNode.NullNode && this.m_container.transform.childCount == 0)) && this.m_burntPrefab != null)
		{
			CompositeAssembledNode compositeAssembledNode = _contents as CompositeAssembledNode;
			if (compositeAssembledNode != null)
			{
				float prefabLength = this.GetPrefabLength(this.m_burntPrefab);
				int b = Mathf.FloorToInt(this.m_contentsBounds / prefabLength);
				int num = this.m_maxMeshesPerIngredient * compositeAssembledNode.m_composition.Length;
				num = Mathf.Min(num, b);
				float num2 = 0f;
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = this.m_burntPrefab.InstantiateOnParent(this.m_container.transform, true);
					this.PlacePrefabAtOffset(gameObject.transform, prefabLength, num2);
					num2 += prefabLength;
				}
			}
		}
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x00068168 File Offset: 0x00066568
	private void CreateContents(AssembledDefinitionNode _contents)
	{
		this.DestroyContents();
		CompositeAssembledNode compositeAssembledNode = _contents as CompositeAssembledNode;
		if (compositeAssembledNode == null || compositeAssembledNode.m_composition.Length == 0)
		{
			return;
		}
		CookedCompositeAssembledNode cookedCompositeAssembledNode = compositeAssembledNode as CookedCompositeAssembledNode;
		OrderToPrefabLookup orderToPrefabLookup = (cookedCompositeAssembledNode == null || cookedCompositeAssembledNode.m_progress == CookedCompositeOrderNode.CookingProgress.Raw) ? this.m_prefabLookup : this.m_cookedPrefabLookup;
		int num = 0;
		GameObject[] array = new GameObject[compositeAssembledNode.m_composition.Length];
		for (int i = 0; i < array.Length; i++)
		{
			GameObject prefabForNode = orderToPrefabLookup.GetPrefabForNode(compositeAssembledNode.m_composition[i]);
			if (prefabForNode != null)
			{
				array[num++] = prefabForNode;
			}
		}
		float[] array2 = new float[num];
		for (int j = 0; j < num; j++)
		{
			array2[j] = Mathf.Max(this.m_minimumPrefabWidth, this.GetPrefabLength(array[j]));
		}
		float num2 = 0f;
		int num3 = 0;
		while (num3 < this.m_maxMeshesPerIngredient && num2 < this.m_contentsBounds)
		{
			int num4 = 0;
			while (num4 < num && num2 < this.m_contentsBounds)
			{
				GameObject gameObject = array[num4].InstantiateOnParent(this.m_container.transform, true);
				this.PlacePrefabAtOffset(gameObject.transform, array2[num4], num2);
				num2 += array2[num4];
				num4++;
			}
			num3++;
		}
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x000682D4 File Offset: 0x000666D4
	private void DestroyContents()
	{
		int childCount = this.m_container.transform.childCount;
		for (int i = childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.m_container.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x00068324 File Offset: 0x00066724
	private void PlacePrefabAtOffset(Transform _element, float _size, float _offset)
	{
		_element.localRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.forward);
		_element.localPosition += Vector3.forward * (_offset + _size / 2f);
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x00068374 File Offset: 0x00066774
	private float GetPrefabLength(GameObject _gameObject)
	{
		Bounds bounds = default(Bounds);
		MeshFilter[] array = _gameObject.RequestComponentsRecursive<MeshFilter>();
		for (int i = 0; i < array.Length; i++)
		{
			Mesh sharedMesh = array[i].sharedMesh;
			if (sharedMesh != null)
			{
				bounds.Encapsulate(sharedMesh.bounds);
			}
		}
		return bounds.size.z;
	}

	// Token: 0x04000E75 RID: 3701
	[SerializeField]
	[AssignResource("KebobCosmeticPrefabs", Editorbility.Editable)]
	private OrderToPrefabLookup m_prefabLookup;

	// Token: 0x04000E76 RID: 3702
	[SerializeField]
	[AssignResource("CookedKebobCosmeticPrefabs", Editorbility.Editable)]
	private OrderToPrefabLookup m_cookedPrefabLookup;

	// Token: 0x04000E77 RID: 3703
	[SerializeField]
	public GameObject m_burntPrefab;

	// Token: 0x04000E78 RID: 3704
	[SerializeField]
	public float m_minimumPrefabWidth = 0.15f;

	// Token: 0x04000E79 RID: 3705
	[SerializeField]
	public float m_contentsOffset = -0.4f;

	// Token: 0x04000E7A RID: 3706
	[SerializeField]
	public float m_contentsBounds = 1f;

	// Token: 0x04000E7B RID: 3707
	[SerializeField]
	public int m_maxMeshesPerIngredient = 2;
}
