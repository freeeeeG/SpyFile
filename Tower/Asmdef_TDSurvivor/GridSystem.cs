using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class GridSystem : Singleton<GridSystem>
{
	// Token: 0x0600032B RID: 811 RVA: 0x0000C3E9 File Offset: 0x0000A5E9
	private new void Awake()
	{
		base.Awake();
		this.dic_TetrisPosition = new Dictionary<Vector3Int, Obj_TetrisBlock>();
		this.dic_GridObjects = new Dictionary<Vector3Int, APowerGrid>();
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0000C407 File Offset: 0x0000A607
	public Vector3 GetGridPos(Vector3 position)
	{
		return this.grid.GetCellCenterWorld(this.grid.WorldToCell(position));
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0000C420 File Offset: 0x0000A620
	public Vector3Int GetGridCell(Vector3 position)
	{
		return this.grid.WorldToCell(position);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x0000C430 File Offset: 0x0000A630
	public void RegisterTetris(Obj_TetrisBlock block)
	{
		foreach (Collider collider in ((IPlaceable)block).GetCollisionColliders())
		{
			this.dic_TetrisPosition.Add(collider.transform.position.WithY(0f).ToVector3Int(), block);
		}
	}

	// Token: 0x0600032F RID: 815 RVA: 0x0000C4A4 File Offset: 0x0000A6A4
	public void UnregisterTetris(Obj_TetrisBlock block)
	{
		foreach (Collider collider in ((IPlaceable)block).GetCollisionColliders())
		{
			this.dic_TetrisPosition.Remove(collider.transform.position.WithY(0f).ToVector3Int());
		}
	}

	// Token: 0x06000330 RID: 816 RVA: 0x0000C518 File Offset: 0x0000A718
	public Obj_TetrisBlock GetTetrisAtPosition(Vector3 position)
	{
		Vector3Int key = position.WithY(0f).ToVector3Int();
		if (this.dic_TetrisPosition.ContainsKey(key))
		{
			return this.dic_TetrisPosition[key];
		}
		return null;
	}

	// Token: 0x06000331 RID: 817 RVA: 0x0000C552 File Offset: 0x0000A752
	public void RegisterPowerGridObject(APowerGrid powerGrid)
	{
		this.dic_GridObjects.Add(powerGrid.transform.position.WithY(0f).ToVector3Int(), powerGrid);
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0000C57A File Offset: 0x0000A77A
	public void UnregisterPowerGridObject(APowerGrid powerGrid)
	{
		this.dic_GridObjects.Remove(powerGrid.transform.position.WithY(0f).ToVector3Int());
	}

	// Token: 0x06000333 RID: 819 RVA: 0x0000C5A4 File Offset: 0x0000A7A4
	public bool IsHavePowerGridAtPosition(Vector3 position)
	{
		Vector3Int key = position.WithY(0f).ToVector3Int();
		return this.dic_GridObjects.ContainsKey(key);
	}

	// Token: 0x06000334 RID: 820 RVA: 0x0000C5D0 File Offset: 0x0000A7D0
	public APowerGrid GetPowerGridObjectAtPosition(Vector3 position)
	{
		Vector3Int vector3Int = position.WithY(0f).ToVector3Int();
		Debug.Log(string.Format("取得位置: {0}", vector3Int));
		if (this.dic_GridObjects.ContainsKey(vector3Int))
		{
			return this.dic_GridObjects[vector3Int];
		}
		return null;
	}

	// Token: 0x04000364 RID: 868
	[SerializeField]
	private Grid grid;

	// Token: 0x04000365 RID: 869
	[SerializeField]
	private Dictionary<Vector3Int, Obj_TetrisBlock> dic_TetrisPosition;

	// Token: 0x04000366 RID: 870
	[SerializeField]
	private Dictionary<Vector3Int, APowerGrid> dic_GridObjects;
}
