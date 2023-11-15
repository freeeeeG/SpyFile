using System;
using UnityEngine;

// Token: 0x0200039D RID: 925
public class ComboCosmeticDecisions : MealCosmeticDecisions
{
	// Token: 0x06001161 RID: 4449 RVA: 0x000627C4 File Offset: 0x00060BC4
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequireInterface<IClientOrderDefinition>();
	}

	// Token: 0x06001162 RID: 4450 RVA: 0x000627D1 File Offset: 0x00060BD1
	protected override void UpdateAppearance(AssembledDefinitionNode _contents)
	{
		this.DestroyContents();
		if (_contents != null)
		{
			this.CreateContents(_contents);
		}
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x000627E6 File Offset: 0x00060BE6
	protected override void Start()
	{
		if (this.m_comboPrefabLookup != null)
		{
			this.m_comboPrefabLookup.CacheAssembledOrderNodes();
		}
		base.Start();
	}

	// Token: 0x06001164 RID: 4452 RVA: 0x0006280C File Offset: 0x00060C0C
	private void CreateContents(AssembledDefinitionNode _contents)
	{
		GameObject prefabForNode = this.m_comboPrefabLookup.GetPrefabForNode(_contents);
		if (prefabForNode != null)
		{
			prefabForNode.InstantiateOnParent(this.m_container.transform, this.m_maintainScale);
		}
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x0006284C File Offset: 0x00060C4C
	private void DestroyContents()
	{
		for (int i = this.m_container.transform.childCount - 1; i >= 0; i--)
		{
			Transform child = this.m_container.transform.GetChild(i);
			child.transform.SetParent(null);
			UnityEngine.Object.Destroy(child.gameObject);
		}
	}

	// Token: 0x04000D81 RID: 3457
	[SerializeField]
	private ComboOrderToPrefabLookup m_comboPrefabLookup;

	// Token: 0x04000D82 RID: 3458
	[SerializeField]
	public bool m_maintainScale = true;
}
