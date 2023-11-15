using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006DB RID: 1755
public static class ClusterUtil
{
	// Token: 0x0600301B RID: 12315 RVA: 0x000FE5B9 File Offset: 0x000FC7B9
	public static WorldContainer GetMyWorld(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorld();
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x000FE5C6 File Offset: 0x000FC7C6
	public static WorldContainer GetMyWorld(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorld();
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x000FE5D4 File Offset: 0x000FC7D4
	public static WorldContainer GetMyWorld(this GameObject gameObject)
	{
		int num = Grid.PosToCell(gameObject);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] != 255)
		{
			return ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
		}
		return null;
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x000FE611 File Offset: 0x000FC811
	public static int GetMyWorldId(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorldId();
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x000FE61E File Offset: 0x000FC81E
	public static int GetMyWorldId(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorldId();
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x000FE62C File Offset: 0x000FC82C
	public static int GetMyWorldId(this GameObject gameObject)
	{
		int num = Grid.PosToCell(gameObject);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] != 255)
		{
			return (int)Grid.WorldIdx[num];
		}
		return -1;
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x000FE65F File Offset: 0x000FC85F
	public static int GetMyParentWorldId(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyParentWorldId();
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x000FE66C File Offset: 0x000FC86C
	public static int GetMyParentWorldId(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyParentWorldId();
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x000FE67C File Offset: 0x000FC87C
	public static int GetMyParentWorldId(this GameObject gameObject)
	{
		WorldContainer myWorld = gameObject.GetMyWorld();
		if (myWorld == null)
		{
			return gameObject.GetMyWorldId();
		}
		return myWorld.ParentWorldId;
	}

	// Token: 0x06003024 RID: 12324 RVA: 0x000FE6A6 File Offset: 0x000FC8A6
	public static AxialI GetMyWorldLocation(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorldLocation();
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x000FE6B3 File Offset: 0x000FC8B3
	public static AxialI GetMyWorldLocation(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorldLocation();
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x000FE6C0 File Offset: 0x000FC8C0
	public static AxialI GetMyWorldLocation(this GameObject gameObject)
	{
		ClusterGridEntity component = gameObject.GetComponent<ClusterGridEntity>();
		if (component != null)
		{
			return component.Location;
		}
		WorldContainer myWorld = gameObject.GetMyWorld();
		DebugUtil.DevAssertArgs(myWorld != null, new object[]
		{
			"GetMyWorldLocation called on object with no world",
			gameObject
		});
		return myWorld.GetComponent<ClusterGridEntity>().Location;
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x000FE714 File Offset: 0x000FC914
	public static bool IsMyWorld(this GameObject go, GameObject otherGo)
	{
		int otherCell = Grid.PosToCell(otherGo);
		return go.IsMyWorld(otherCell);
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x000FE730 File Offset: 0x000FC930
	public static bool IsMyWorld(this GameObject go, int otherCell)
	{
		int num = Grid.PosToCell(go);
		return Grid.IsValidCell(num) && Grid.IsValidCell(otherCell) && Grid.WorldIdx[num] == Grid.WorldIdx[otherCell];
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x000FE768 File Offset: 0x000FC968
	public static bool IsMyParentWorld(this GameObject go, GameObject otherGo)
	{
		int otherCell = Grid.PosToCell(otherGo);
		return go.IsMyParentWorld(otherCell);
	}

	// Token: 0x0600302A RID: 12330 RVA: 0x000FE784 File Offset: 0x000FC984
	public static bool IsMyParentWorld(this GameObject go, int otherCell)
	{
		int num = Grid.PosToCell(go);
		if (Grid.IsValidCell(num) && Grid.IsValidCell(otherCell))
		{
			if (Grid.WorldIdx[num] == Grid.WorldIdx[otherCell])
			{
				return true;
			}
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
			WorldContainer world2 = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[otherCell]);
			if (world == null)
			{
				DebugUtil.DevLogError(string.Format("{0} at {1} has a valid cell but no world", go, num));
			}
			if (world2 == null)
			{
				DebugUtil.DevLogError(string.Format("{0} is a valid cell but no world", otherCell));
			}
			if (world != null && world2 != null && world.ParentWorldId == world2.ParentWorldId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x000FE844 File Offset: 0x000FCA44
	public static int GetAsteroidWorldIdAtLocation(AxialI location)
	{
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.cellContents[location])
		{
			if (clusterGridEntity.Layer == EntityLayer.Asteroid)
			{
				WorldContainer component = clusterGridEntity.GetComponent<WorldContainer>();
				if (component != null)
				{
					return component.id;
				}
			}
		}
		return -1;
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x000FE8C0 File Offset: 0x000FCAC0
	public static bool ActiveWorldIsRocketInterior()
	{
		return ClusterManager.Instance.activeWorld.IsModuleInterior;
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x000FE8D1 File Offset: 0x000FCAD1
	public static bool ActiveWorldHasPrinter()
	{
		return ClusterManager.Instance.activeWorld.IsModuleInterior || Components.Telepads.GetWorldItems(ClusterManager.Instance.activeWorldId, false).Count > 0;
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x000FE904 File Offset: 0x000FCB04
	public static float GetAmountFromRelatedWorlds(WorldInventory worldInventory, Tag element)
	{
		WorldContainer worldContainer = worldInventory.WorldContainer;
		float num = 0f;
		int parentWorldId = worldContainer.ParentWorldId;
		foreach (WorldContainer worldContainer2 in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer2.ParentWorldId == parentWorldId)
			{
				num += worldContainer2.worldInventory.GetAmount(element, false);
			}
		}
		return num;
	}

	// Token: 0x0600302F RID: 12335 RVA: 0x000FE97C File Offset: 0x000FCB7C
	public static List<Pickupable> GetPickupablesFromRelatedWorlds(WorldInventory worldInventory, Tag tag)
	{
		List<Pickupable> list = new List<Pickupable>();
		int parentWorldId = worldInventory.GetComponent<WorldContainer>().ParentWorldId;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.ParentWorldId == parentWorldId)
			{
				ICollection<Pickupable> pickupables = worldContainer.worldInventory.GetPickupables(tag, false);
				if (pickupables != null)
				{
					list.AddRange(pickupables);
				}
			}
		}
		return list;
	}

	// Token: 0x06003030 RID: 12336 RVA: 0x000FEA00 File Offset: 0x000FCC00
	public static string DebugGetMyWorldName(this GameObject gameObject)
	{
		WorldContainer myWorld = gameObject.GetMyWorld();
		if (myWorld != null)
		{
			return myWorld.worldName;
		}
		return string.Format("InvalidWorld(pos={0})", gameObject.transform.GetPosition());
	}

	// Token: 0x06003031 RID: 12337 RVA: 0x000FEA40 File Offset: 0x000FCC40
	public static ClusterGridEntity ClosestVisibleAsteroidToLocation(AxialI location)
	{
		foreach (AxialI cell in AxialUtil.SpiralOut(location, ClusterGrid.Instance.numRings))
		{
			if (ClusterGrid.Instance.IsValidCell(cell) && ClusterGrid.Instance.IsCellVisible(cell))
			{
				ClusterGridEntity asteroidAtCell = ClusterGrid.Instance.GetAsteroidAtCell(cell);
				if (asteroidAtCell != null)
				{
					return asteroidAtCell;
				}
			}
		}
		return null;
	}
}
