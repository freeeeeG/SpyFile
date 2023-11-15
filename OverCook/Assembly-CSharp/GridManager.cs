using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000506 RID: 1286
public abstract class GridManager : Manager
{
	// Token: 0x17000250 RID: 592
	// (get) Token: 0x060017FE RID: 6142 RVA: 0x0007A354 File Offset: 0x00078754
	// (set) Token: 0x060017FF RID: 6143 RVA: 0x0007A35C File Offset: 0x0007875C
	public Point3 AccessGridHalfSize
	{
		get
		{
			return this.m_gridHalfSize;
		}
		set
		{
			this.m_gridHalfSize = value;
		}
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x0007A365 File Offset: 0x00078765
	private void OnEnable()
	{
		GridManager.s_activeGrids.Add(this);
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x0007A372 File Offset: 0x00078772
	private void OnDisable()
	{
		GridManager.s_activeGrids.RemoveAll(new Predicate<GridManager>(this.Equals));
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x0007A38C File Offset: 0x0007878C
	public static int GetActiveCount()
	{
		return GridManager.s_activeGrids.Count;
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x0007A398 File Offset: 0x00078798
	public static GridManager GetActive(int _index)
	{
		return GridManager.s_activeGrids[_index];
	}

	// Token: 0x06001804 RID: 6148
	public abstract Vector3 GetPosFromGridLocation(GridIndex _index);

	// Token: 0x06001805 RID: 6149
	public abstract GridIndex GetUnclampedGridLocationFromPos(Vector3 _pos);

	// Token: 0x06001806 RID: 6150 RVA: 0x0007A3A8 File Offset: 0x000787A8
	public GridIndex GetGridLocationFromPos(Vector3 _pos)
	{
		GridIndex unclampedGridLocationFromPos = this.GetUnclampedGridLocationFromPos(_pos);
		Point3 gridHalfSize = this.GetGridHalfSize();
		int x = Mathf.Clamp(unclampedGridLocationFromPos.X, -gridHalfSize.X, gridHalfSize.X);
		int y = Mathf.Clamp(unclampedGridLocationFromPos.Y, -gridHalfSize.Y, gridHalfSize.Y);
		int z = Mathf.Clamp(unclampedGridLocationFromPos.Z, -gridHalfSize.Z, gridHalfSize.Z);
		return new GridIndex(x, y, z);
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x0007A41C File Offset: 0x0007881C
	public Point3 GetGridHalfSize()
	{
		return this.m_gridHalfSize;
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x0007A424 File Offset: 0x00078824
	public void OccupyGrid(GameObject _object, GridIndex _index)
	{
		if (this.m_gridOccupancy.ContainsKey(_index))
		{
			GameObject obj = this.m_gridOccupancy[_index];
			IHandleGridTransfer handleGridTransfer = obj.RequestInterface<IHandleGridTransfer>();
			if (handleGridTransfer != null && handleGridTransfer.CanHandleTransfer(_index, _object))
			{
				handleGridTransfer.HandleTransfer(_index, _object);
				this.m_gridOccupancy[_index] = _object;
			}
		}
		else
		{
			this.m_gridOccupancy.Add(_index, _object);
		}
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x0007A498 File Offset: 0x00078898
	public GameObject GetGridOccupant(GridIndex _index)
	{
		GameObject result = null;
		this.m_gridOccupancy.TryGetValue(_index, out result);
		return result;
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x0007A4B7 File Offset: 0x000788B7
	public void DeoccupyGrid(GridIndex _index)
	{
		this.m_gridOccupancy.Remove(_index);
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x0007A4C8 File Offset: 0x000788C8
	public bool TryOccupyGridRegion(GridIndex min, GridIndex max, GameObject go)
	{
		for (int i = min.Z; i <= max.Z; i++)
		{
			for (int j = min.Y; j <= max.Y; j++)
			{
				for (int k = min.X; k <= max.X; k++)
				{
					GridIndex gridIndex = new GridIndex(k, j, i);
					if (this.m_gridOccupancy.ContainsKey(gridIndex))
					{
						GameObject gridOccupant = this.GetGridOccupant(gridIndex);
						return false;
					}
				}
			}
		}
		for (int l = min.Z; l <= max.Z; l++)
		{
			for (int m = min.Y; m <= max.Y; m++)
			{
				for (int n = min.X; n <= max.X; n++)
				{
					GridIndex index = new GridIndex(n, m, l);
					this.OccupyGrid(go, index);
				}
			}
		}
		return true;
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x0007A5D4 File Offset: 0x000789D4
	public void DeoccupyGridRegion(GridIndex min, GridIndex max)
	{
		for (int i = min.Z; i <= max.Z; i++)
		{
			for (int j = min.Y; j <= max.Y; j++)
			{
				for (int k = min.X; k <= max.X; k++)
				{
					GridIndex key = new GridIndex(k, j, i);
					this.m_gridOccupancy.Remove(key);
				}
			}
		}
	}

	// Token: 0x0400135C RID: 4956
	[SerializeField]
	private Point3 m_gridHalfSize = new Point3(20, 1, 20);

	// Token: 0x0400135D RID: 4957
	private Dictionary<GridIndex, GameObject> m_gridOccupancy = new Dictionary<GridIndex, GameObject>(default(GridIndex));

	// Token: 0x0400135E RID: 4958
	private static List<GridManager> s_activeGrids = new List<GridManager>();
}
