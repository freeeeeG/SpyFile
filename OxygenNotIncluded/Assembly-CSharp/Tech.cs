using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x0200092D RID: 2349
public class Tech : Resource
{
	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06004430 RID: 17456 RVA: 0x0017EB64 File Offset: 0x0017CD64
	public bool FoundNode
	{
		get
		{
			return this.node != null;
		}
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06004431 RID: 17457 RVA: 0x0017EB6F File Offset: 0x0017CD6F
	public Vector2 center
	{
		get
		{
			return this.node.center;
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06004432 RID: 17458 RVA: 0x0017EB7C File Offset: 0x0017CD7C
	public float width
	{
		get
		{
			return this.node.width;
		}
	}

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06004433 RID: 17459 RVA: 0x0017EB89 File Offset: 0x0017CD89
	public float height
	{
		get
		{
			return this.node.height;
		}
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06004434 RID: 17460 RVA: 0x0017EB96 File Offset: 0x0017CD96
	public List<ResourceTreeNode.Edge> edges
	{
		get
		{
			return this.node.edges;
		}
	}

	// Token: 0x06004435 RID: 17461 RVA: 0x0017EBA4 File Offset: 0x0017CDA4
	public Tech(string id, List<string> unlockedItemIDs, Techs techs, Dictionary<string, float> overrideDefaultCosts = null) : base(id, techs, Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".NAME"))
	{
		this.desc = Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".DESC");
		this.unlockedItemIDs = unlockedItemIDs;
		if (overrideDefaultCosts != null && DlcManager.IsExpansion1Active())
		{
			foreach (KeyValuePair<string, float> keyValuePair in overrideDefaultCosts)
			{
				this.costsByResearchTypeID.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}

	// Token: 0x06004436 RID: 17462 RVA: 0x0017EC9C File Offset: 0x0017CE9C
	public void AddUnlockedItemIDs(params string[] ids)
	{
		foreach (string item in ids)
		{
			this.unlockedItemIDs.Add(item);
		}
	}

	// Token: 0x06004437 RID: 17463 RVA: 0x0017ECCC File Offset: 0x0017CECC
	public void RemoveUnlockedItemIDs(params string[] ids)
	{
		foreach (string text in ids)
		{
			if (!this.unlockedItemIDs.Remove(text))
			{
				DebugUtil.DevLogError("Tech item '" + text + "' does not exist to remove");
			}
		}
	}

	// Token: 0x06004438 RID: 17464 RVA: 0x0017ED10 File Offset: 0x0017CF10
	public bool RequiresResearchType(string type)
	{
		return this.costsByResearchTypeID.ContainsKey(type);
	}

	// Token: 0x06004439 RID: 17465 RVA: 0x0017ED1E File Offset: 0x0017CF1E
	public void SetNode(ResourceTreeNode node, string categoryID)
	{
		this.node = node;
		this.category = categoryID;
	}

	// Token: 0x0600443A RID: 17466 RVA: 0x0017ED30 File Offset: 0x0017CF30
	public bool CanAfford(ResearchPointInventory pointInventory)
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
		{
			if (pointInventory.PointsByTypeID[keyValuePair.Key] < keyValuePair.Value)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600443B RID: 17467 RVA: 0x0017EDA0 File Offset: 0x0017CFA0
	public string CostString(ResearchTypes types)
	{
		string text = "";
		foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
		{
			text += string.Format("{0}:{1}", types.GetResearchType(keyValuePair.Key).name.ToString(), keyValuePair.Value.ToString());
			text += "\n";
		}
		return text;
	}

	// Token: 0x0600443C RID: 17468 RVA: 0x0017EE38 File Offset: 0x0017D038
	public bool IsComplete()
	{
		if (Research.Instance != null)
		{
			TechInstance techInstance = Research.Instance.Get(this);
			return techInstance != null && techInstance.IsComplete();
		}
		return false;
	}

	// Token: 0x0600443D RID: 17469 RVA: 0x0017EE6C File Offset: 0x0017D06C
	public bool ArePrerequisitesComplete()
	{
		using (List<Tech>.Enumerator enumerator = this.requiredTech.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsComplete())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x04002D36 RID: 11574
	public List<Tech> requiredTech = new List<Tech>();

	// Token: 0x04002D37 RID: 11575
	public List<Tech> unlockedTech = new List<Tech>();

	// Token: 0x04002D38 RID: 11576
	public List<TechItem> unlockedItems = new List<TechItem>();

	// Token: 0x04002D39 RID: 11577
	public List<string> unlockedItemIDs = new List<string>();

	// Token: 0x04002D3A RID: 11578
	public int tier;

	// Token: 0x04002D3B RID: 11579
	public Dictionary<string, float> costsByResearchTypeID = new Dictionary<string, float>();

	// Token: 0x04002D3C RID: 11580
	public string desc;

	// Token: 0x04002D3D RID: 11581
	public string category;

	// Token: 0x04002D3E RID: 11582
	public Tag[] tags;

	// Token: 0x04002D3F RID: 11583
	private ResourceTreeNode node;
}
