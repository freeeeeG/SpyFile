using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x02000929 RID: 2345
public class ResearchPointInventory
{
	// Token: 0x0600441C RID: 17436 RVA: 0x0017E4C4 File Offset: 0x0017C6C4
	public ResearchPointInventory()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.PointsByTypeID.Add(researchType.id, 0f);
		}
	}

	// Token: 0x0600441D RID: 17437 RVA: 0x0017E540 File Offset: 0x0017C740
	public void AddResearchPoints(string researchTypeID, float points)
	{
		if (!this.PointsByTypeID.ContainsKey(researchTypeID))
		{
			Debug.LogWarning("Research inventory is missing research point key " + researchTypeID);
			return;
		}
		Dictionary<string, float> pointsByTypeID = this.PointsByTypeID;
		pointsByTypeID[researchTypeID] += points;
	}

	// Token: 0x0600441E RID: 17438 RVA: 0x0017E585 File Offset: 0x0017C785
	public void RemoveResearchPoints(string researchTypeID, float points)
	{
		this.AddResearchPoints(researchTypeID, -points);
	}

	// Token: 0x0600441F RID: 17439 RVA: 0x0017E590 File Offset: 0x0017C790
	[OnDeserialized]
	private void OnDeserialized()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			if (!this.PointsByTypeID.ContainsKey(researchType.id))
			{
				this.PointsByTypeID.Add(researchType.id, 0f);
			}
		}
	}

	// Token: 0x04002D2D RID: 11565
	public Dictionary<string, float> PointsByTypeID = new Dictionary<string, float>();
}
