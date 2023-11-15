using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C1C RID: 3100
public class FlatTagFilterSideScreen : SideScreenContent
{
	// Token: 0x0600621A RID: 25114 RVA: 0x002437C5 File Offset: 0x002419C5
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<FlatTagFilterable>() != null;
	}

	// Token: 0x0600621B RID: 25115 RVA: 0x002437D3 File Offset: 0x002419D3
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.tagFilterable = target.GetComponent<FlatTagFilterable>();
		this.Build();
	}

	// Token: 0x0600621C RID: 25116 RVA: 0x002437F0 File Offset: 0x002419F0
	private void Build()
	{
		this.headerLabel.SetText(this.tagFilterable.GetHeaderText());
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
		foreach (Tag tag in this.tagFilterable.tagOptions)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
			gameObject.gameObject.name = tag.ProperName();
			this.rows.Add(tag, gameObject);
		}
		this.Refresh();
	}

	// Token: 0x0600621D RID: 25117 RVA: 0x002438E4 File Offset: 0x00241AE4
	private void Refresh()
	{
		using (Dictionary<Tag, GameObject>.Enumerator enumerator = this.rows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Tag, GameObject> kvp = enumerator.Current;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.ProperNameStripLink());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(kvp.Key, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(kvp.Key, "ui", false).second;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.tagFilterable.ToggleTag(kvp.Key);
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.tagFilterable.selectedTags.Contains(kvp.Key) ? 1 : 0);
				kvp.Value.SetActive(!this.tagFilterable.displayOnlyDiscoveredTags || DiscoveredResources.Instance.IsDiscovered(kvp.Key));
			}
		}
	}

	// Token: 0x0600621E RID: 25118 RVA: 0x00243AA4 File Offset: 0x00241CA4
	public override string GetTitle()
	{
		return this.tagFilterable.gameObject.GetProperName();
	}

	// Token: 0x040042DC RID: 17116
	private FlatTagFilterable tagFilterable;

	// Token: 0x040042DD RID: 17117
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x040042DE RID: 17118
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x040042DF RID: 17119
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x040042E0 RID: 17120
	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
}
