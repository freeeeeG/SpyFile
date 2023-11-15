using System;
using System.Collections.Generic;
using AStar;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class GridNavSpace : Manager
{
	// Token: 0x06000487 RID: 1159 RVA: 0x000274BC File Offset: 0x000258BC
	public Point2 GetNavPoint(Vector3 _position)
	{
		GridIndex gridLocationFromPos = this.m_gridManager.GetGridLocationFromPos(_position);
		return this.GetPointFromIndex(gridLocationFromPos);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x000274DF File Offset: 0x000258DF
	private Point2 GetPointFromIndex(GridIndex _index)
	{
		return new Point2(_index.X + this.m_mapOffset.X, _index.Z + this.m_mapOffset.Y);
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0002750C File Offset: 0x0002590C
	private GridIndex GetIndexFromPoint(Point2 _point)
	{
		return new GridIndex(_point.X - this.m_mapOffset.X, 0, _point.Y - this.m_mapOffset.Y);
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00027538 File Offset: 0x00025938
	public List<Vector3> FindPath(Point2 _start, Point2 _end)
	{
		SearchParameters searchParameters = new SearchParameters(_start, _end, this.m_nodeMap);
		PathFinder pathFinder = new PathFinder(searchParameters);
		Converter<Point2, Vector3> converter = (Point2 _input) => this.m_gridManager.GetPosFromGridLocation(this.GetIndexFromPoint(_input));
		List<Point2> list = pathFinder.FindPath();
		return list.ConvertAll<Vector3>(converter);
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00027575 File Offset: 0x00025975
	private void Awake()
	{
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00027588 File Offset: 0x00025988
	private void Start()
	{
		Point3 gridHalfSize = this.m_gridManager.GetGridHalfSize();
		this.m_nodeMap = new bool[2 * gridHalfSize.X + 1, 2 * gridHalfSize.Z + 1];
		this.m_mapOffset = new Point2(gridHalfSize.X, gridHalfSize.Z);
		for (int i = 0; i < this.m_nodeMap.GetLength(0); i++)
		{
			for (int j = 0; j < this.m_nodeMap.GetLength(1); j++)
			{
				this.m_nodeMap[i, j] = (this.m_gridManager.GetGridOccupant(this.GetIndexFromPoint(new Point2(i, j))) == null);
			}
		}
	}

	// Token: 0x040003FE RID: 1022
	private GridManager m_gridManager;

	// Token: 0x040003FF RID: 1023
	private bool[,] m_nodeMap;

	// Token: 0x04000400 RID: 1024
	private Point2 m_mapOffset;
}
