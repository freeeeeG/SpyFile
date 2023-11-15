using System;
using UnityEngine;

// Token: 0x0200092E RID: 2350
public class TechInstance
{
	// Token: 0x0600443E RID: 17470 RVA: 0x0017EEC8 File Offset: 0x0017D0C8
	public TechInstance(Tech tech)
	{
		this.tech = tech;
	}

	// Token: 0x0600443F RID: 17471 RVA: 0x0017EEE2 File Offset: 0x0017D0E2
	public bool IsComplete()
	{
		return this.complete;
	}

	// Token: 0x06004440 RID: 17472 RVA: 0x0017EEEA File Offset: 0x0017D0EA
	public void Purchased()
	{
		if (!this.complete)
		{
			this.complete = true;
		}
	}

	// Token: 0x06004441 RID: 17473 RVA: 0x0017EEFB File Offset: 0x0017D0FB
	public float PercentageCompleteResearchType(string type)
	{
		if (!this.tech.RequiresResearchType(type))
		{
			return 1f;
		}
		return Mathf.Clamp01(this.progressInventory.PointsByTypeID[type] / this.tech.costsByResearchTypeID[type]);
	}

	// Token: 0x06004442 RID: 17474 RVA: 0x0017EF3C File Offset: 0x0017D13C
	public TechInstance.SaveData Save()
	{
		string[] array = new string[this.progressInventory.PointsByTypeID.Count];
		this.progressInventory.PointsByTypeID.Keys.CopyTo(array, 0);
		float[] array2 = new float[this.progressInventory.PointsByTypeID.Count];
		this.progressInventory.PointsByTypeID.Values.CopyTo(array2, 0);
		return new TechInstance.SaveData
		{
			techId = this.tech.Id,
			complete = this.complete,
			inventoryIDs = array,
			inventoryValues = array2
		};
	}

	// Token: 0x06004443 RID: 17475 RVA: 0x0017EFDC File Offset: 0x0017D1DC
	public void Load(TechInstance.SaveData save_data)
	{
		this.complete = save_data.complete;
		for (int i = 0; i < save_data.inventoryIDs.Length; i++)
		{
			this.progressInventory.AddResearchPoints(save_data.inventoryIDs[i], save_data.inventoryValues[i]);
		}
	}

	// Token: 0x04002D40 RID: 11584
	public Tech tech;

	// Token: 0x04002D41 RID: 11585
	private bool complete;

	// Token: 0x04002D42 RID: 11586
	public ResearchPointInventory progressInventory = new ResearchPointInventory();

	// Token: 0x02001772 RID: 6002
	public struct SaveData
	{
		// Token: 0x04006EC6 RID: 28358
		public string techId;

		// Token: 0x04006EC7 RID: 28359
		public bool complete;

		// Token: 0x04006EC8 RID: 28360
		public string[] inventoryIDs;

		// Token: 0x04006EC9 RID: 28361
		public float[] inventoryValues;
	}
}
