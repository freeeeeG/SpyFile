using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AB9 RID: 2745
public class ClusterNameDisplayScreen : KScreen
{
	// Token: 0x06005405 RID: 21509 RVA: 0x001E4AE2 File Offset: 0x001E2CE2
	public static void DestroyInstance()
	{
		ClusterNameDisplayScreen.Instance = null;
	}

	// Token: 0x06005406 RID: 21510 RVA: 0x001E4AEA File Offset: 0x001E2CEA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClusterNameDisplayScreen.Instance = this;
	}

	// Token: 0x06005407 RID: 21511 RVA: 0x001E4AF8 File Offset: 0x001E2CF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005408 RID: 21512 RVA: 0x001E4B00 File Offset: 0x001E2D00
	public void AddNewEntry(ClusterGridEntity representedObject)
	{
		if (this.GetEntry(representedObject) != null)
		{
			return;
		}
		ClusterNameDisplayScreen.Entry entry = new ClusterNameDisplayScreen.Entry();
		entry.grid_entity = representedObject;
		GameObject gameObject = Util.KInstantiateUI(this.nameAndBarsPrefab, base.gameObject, true);
		entry.display_go = gameObject;
		gameObject.name = representedObject.name + " cluster overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		entry.bars_go = entry.refs.GetReference<RectTransform>("Bars").gameObject;
		this.m_entries.Add(entry);
		if (representedObject.GetComponent<KSelectable>() != null)
		{
			this.UpdateName(representedObject);
			this.UpdateBars(representedObject);
		}
	}

	// Token: 0x06005409 RID: 21513 RVA: 0x001E4BB0 File Offset: 0x001E2DB0
	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		int num = this.m_entries.Count;
		int i = 0;
		while (i < num)
		{
			if (this.m_entries[i].grid_entity != null && ClusterMapScreen.GetRevealLevel(this.m_entries[i].grid_entity) == ClusterRevealLevel.Visible)
			{
				Transform gridEntityNameTarget = ClusterMapScreen.Instance.GetGridEntityNameTarget(this.m_entries[i].grid_entity);
				if (gridEntityNameTarget != null)
				{
					Vector3 position = gridEntityNameTarget.GetPosition();
					this.m_entries[i].display_go.GetComponent<RectTransform>().SetPositionAndRotation(position, Quaternion.identity);
					this.m_entries[i].display_go.SetActive(this.m_entries[i].grid_entity.IsVisible && this.m_entries[i].grid_entity.ShowName());
				}
				else if (this.m_entries[i].display_go.activeSelf)
				{
					this.m_entries[i].display_go.SetActive(false);
				}
				this.UpdateBars(this.m_entries[i].grid_entity);
				if (this.m_entries[i].bars_go != null)
				{
					this.m_entries[i].bars_go.GetComponentsInChildren<KCollider2D>(false, this.workingList);
					foreach (KCollider2D kcollider2D in this.workingList)
					{
						kcollider2D.MarkDirty(false);
					}
				}
				i++;
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_entries[i].display_go);
				num--;
				this.m_entries[i] = this.m_entries[num];
			}
		}
		this.m_entries.RemoveRange(num, this.m_entries.Count - num);
	}

	// Token: 0x0600540A RID: 21514 RVA: 0x001E4DC8 File Offset: 0x001E2FC8
	public void UpdateName(ClusterGridEntity representedObject)
	{
		ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		KSelectable component = representedObject.GetComponent<KSelectable>();
		entry.display_go.name = component.GetProperName() + " cluster overlay";
		LocText componentInChildren = entry.display_go.GetComponentInChildren<LocText>();
		if (componentInChildren != null)
		{
			componentInChildren.text = component.GetProperName();
		}
	}

	// Token: 0x0600540B RID: 21515 RVA: 0x001E4E24 File Offset: 0x001E3024
	private void UpdateBars(ClusterGridEntity representedObject)
	{
		ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		GenericUIProgressBar componentInChildren = entry.bars_go.GetComponentInChildren<GenericUIProgressBar>(true);
		if (entry.grid_entity.ShowProgressBar())
		{
			if (!componentInChildren.gameObject.activeSelf)
			{
				componentInChildren.gameObject.SetActive(true);
			}
			componentInChildren.SetFillPercentage(entry.grid_entity.GetProgress());
			return;
		}
		if (componentInChildren.gameObject.activeSelf)
		{
			componentInChildren.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600540C RID: 21516 RVA: 0x001E4E9C File Offset: 0x001E309C
	private ClusterNameDisplayScreen.Entry GetEntry(ClusterGridEntity entity)
	{
		return this.m_entries.Find((ClusterNameDisplayScreen.Entry entry) => entry.grid_entity == entity);
	}

	// Token: 0x0400383C RID: 14396
	public static ClusterNameDisplayScreen Instance;

	// Token: 0x0400383D RID: 14397
	public GameObject nameAndBarsPrefab;

	// Token: 0x0400383E RID: 14398
	[SerializeField]
	private Color selectedColor;

	// Token: 0x0400383F RID: 14399
	[SerializeField]
	private Color defaultColor;

	// Token: 0x04003840 RID: 14400
	private List<ClusterNameDisplayScreen.Entry> m_entries = new List<ClusterNameDisplayScreen.Entry>();

	// Token: 0x04003841 RID: 14401
	private List<KCollider2D> workingList = new List<KCollider2D>();

	// Token: 0x020019CE RID: 6606
	private class Entry
	{
		// Token: 0x0400776A RID: 30570
		public string Name;

		// Token: 0x0400776B RID: 30571
		public ClusterGridEntity grid_entity;

		// Token: 0x0400776C RID: 30572
		public GameObject display_go;

		// Token: 0x0400776D RID: 30573
		public GameObject bars_go;

		// Token: 0x0400776E RID: 30574
		public HierarchyReferences refs;
	}
}
