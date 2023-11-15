using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000949 RID: 2377
public class CavityInfo
{
	// Token: 0x06004545 RID: 17733 RVA: 0x00186DD8 File Offset: 0x00184FD8
	public CavityInfo()
	{
		this.handle = HandleVector<int>.InvalidHandle;
		this.dirty = true;
	}

	// Token: 0x06004546 RID: 17734 RVA: 0x00186E29 File Offset: 0x00185029
	public void AddBuilding(KPrefabID bc)
	{
		this.buildings.Add(bc);
		this.dirty = true;
	}

	// Token: 0x06004547 RID: 17735 RVA: 0x00186E3E File Offset: 0x0018503E
	public void AddPlants(KPrefabID plant)
	{
		this.plants.Add(plant);
		this.dirty = true;
	}

	// Token: 0x06004548 RID: 17736 RVA: 0x00186E54 File Offset: 0x00185054
	public void RemoveFromCavity(KPrefabID id, List<KPrefabID> listToRemove)
	{
		int num = -1;
		for (int i = 0; i < listToRemove.Count; i++)
		{
			if (id.InstanceID == listToRemove[i].InstanceID)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			listToRemove.RemoveAt(num);
		}
	}

	// Token: 0x06004549 RID: 17737 RVA: 0x00186E98 File Offset: 0x00185098
	public void OnEnter(object data)
	{
		foreach (KPrefabID kprefabID in this.buildings)
		{
			if (kprefabID != null)
			{
				kprefabID.Trigger(-832141045, data);
			}
		}
	}

	// Token: 0x0600454A RID: 17738 RVA: 0x00186EFC File Offset: 0x001850FC
	public Vector3 GetCenter()
	{
		return new Vector3((float)(this.minX + (this.maxX - this.minX) / 2), (float)(this.minY + (this.maxY - this.minY) / 2));
	}

	// Token: 0x04002DF5 RID: 11765
	public HandleVector<int>.Handle handle;

	// Token: 0x04002DF6 RID: 11766
	public bool dirty;

	// Token: 0x04002DF7 RID: 11767
	public int numCells;

	// Token: 0x04002DF8 RID: 11768
	public int maxX;

	// Token: 0x04002DF9 RID: 11769
	public int maxY;

	// Token: 0x04002DFA RID: 11770
	public int minX;

	// Token: 0x04002DFB RID: 11771
	public int minY;

	// Token: 0x04002DFC RID: 11772
	public Room room;

	// Token: 0x04002DFD RID: 11773
	public List<KPrefabID> buildings = new List<KPrefabID>();

	// Token: 0x04002DFE RID: 11774
	public List<KPrefabID> plants = new List<KPrefabID>();

	// Token: 0x04002DFF RID: 11775
	public List<KPrefabID> creatures = new List<KPrefabID>();

	// Token: 0x04002E00 RID: 11776
	public List<KPrefabID> eggs = new List<KPrefabID>();
}
