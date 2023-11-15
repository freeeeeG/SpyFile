using System;
using System.Collections.Generic;

// Token: 0x020005AF RID: 1455
public class BuildingInventory : KMonoBehaviour
{
	// Token: 0x060023A4 RID: 9124 RVA: 0x000C3205 File Offset: 0x000C1405
	public static void DestroyInstance()
	{
		BuildingInventory.Instance = null;
	}

	// Token: 0x060023A5 RID: 9125 RVA: 0x000C320D File Offset: 0x000C140D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		BuildingInventory.Instance = this;
	}

	// Token: 0x060023A6 RID: 9126 RVA: 0x000C321B File Offset: 0x000C141B
	public HashSet<BuildingComplete> GetBuildings(Tag tag)
	{
		return this.Buildings[tag];
	}

	// Token: 0x060023A7 RID: 9127 RVA: 0x000C3229 File Offset: 0x000C1429
	public int BuildingCount(Tag tag)
	{
		if (!this.Buildings.ContainsKey(tag))
		{
			return 0;
		}
		return this.Buildings[tag].Count;
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x000C324C File Offset: 0x000C144C
	public int BuildingCountForWorld_BAD_PERF(Tag tag, int worldId)
	{
		if (!this.Buildings.ContainsKey(tag))
		{
			return 0;
		}
		int num = 0;
		using (HashSet<BuildingComplete>.Enumerator enumerator = this.Buildings[tag].GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetMyWorldId() == worldId)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x060023A9 RID: 9129 RVA: 0x000C32BC File Offset: 0x000C14BC
	public void RegisterBuilding(BuildingComplete building)
	{
		Tag prefabTag = building.prefabid.PrefabTag;
		HashSet<BuildingComplete> hashSet;
		if (!this.Buildings.TryGetValue(prefabTag, out hashSet))
		{
			hashSet = new HashSet<BuildingComplete>();
			this.Buildings[prefabTag] = hashSet;
		}
		hashSet.Add(building);
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x000C3300 File Offset: 0x000C1500
	public void UnregisterBuilding(BuildingComplete building)
	{
		Tag prefabTag = building.prefabid.PrefabTag;
		HashSet<BuildingComplete> hashSet;
		if (!this.Buildings.TryGetValue(prefabTag, out hashSet))
		{
			DebugUtil.DevLogError(string.Format("Unregistering building {0} before it was registered.", prefabTag));
			return;
		}
		DebugUtil.DevAssert(hashSet.Remove(building), string.Format("Building {0} was not found to be removed", prefabTag), null);
	}

	// Token: 0x04001459 RID: 5209
	public static BuildingInventory Instance;

	// Token: 0x0400145A RID: 5210
	private Dictionary<Tag, HashSet<BuildingComplete>> Buildings = new Dictionary<Tag, HashSet<BuildingComplete>>();
}
