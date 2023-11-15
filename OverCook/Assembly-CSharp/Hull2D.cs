using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000BBE RID: 3006
[Serializable]
public class Hull2D
{
	// Token: 0x06003D88 RID: 15752 RVA: 0x00125844 File Offset: 0x00123C44
	public static Hull2D GenerateHull(List<Vector2> _points)
	{
		if (_points == null)
		{
			return null;
		}
		if (_points.Count < 3)
		{
			return null;
		}
		List<Vector2> list = new List<Vector2>(_points);
		List<Vector2> list2 = list;
		if (Hull2D.<>f__mg$cache0 == null)
		{
			Hull2D.<>f__mg$cache0 = new Comparison<Vector2>(Hull2D.SortPoints);
		}
		list2.Sort(Hull2D.<>f__mg$cache0);
		return Hull2D.GenerateHullFromSorted(list);
	}

	// Token: 0x06003D89 RID: 15753 RVA: 0x00125898 File Offset: 0x00123C98
	public static Hull2D GenerateHull(Vector2[] _points)
	{
		if (_points.Length < 3)
		{
			return null;
		}
		List<Vector2> list = new List<Vector2>(_points);
		List<Vector2> list2 = list;
		if (Hull2D.<>f__mg$cache1 == null)
		{
			Hull2D.<>f__mg$cache1 = new Comparison<Vector2>(Hull2D.SortPoints);
		}
		list2.Sort(Hull2D.<>f__mg$cache1);
		return Hull2D.GenerateHullFromSorted(list);
	}

	// Token: 0x06003D8A RID: 15754 RVA: 0x001258E0 File Offset: 0x00123CE0
	private static Hull2D GenerateHullFromSorted(List<Vector2> _sortedPoints)
	{
		int[] array = new int[_sortedPoints.Count * 2];
		int num = 0;
		for (int i = 0; i < _sortedPoints.Count; i++)
		{
			while (num >= 2 && Hull2D.TriangleArea(_sortedPoints[array[num - 2]], _sortedPoints[array[num - 1]], _sortedPoints[i]) <= 0f)
			{
				num--;
			}
			array[num++] = i;
		}
		int j = _sortedPoints.Count - 2;
		int num2 = num + 1;
		while (j >= 0)
		{
			while (num >= num2 && Hull2D.TriangleArea(_sortedPoints[array[num - 2]], _sortedPoints[array[num - 1]], _sortedPoints[j]) <= 0f)
			{
				num--;
			}
			array[num++] = j;
			j--;
		}
		num--;
		Vector2[] array2 = new Vector2[num];
		for (int k = 0; k < num; k++)
		{
			array2[k] = _sortedPoints[array[k]];
		}
		return new Hull2D
		{
			m_points = array2,
			m_bounds = Hull2D.CalculateBounds(array2)
		};
	}

	// Token: 0x06003D8B RID: 15755 RVA: 0x00125A18 File Offset: 0x00123E18
	private static Rect CalculateBounds(Vector2[] _points)
	{
		if (_points.Length == 0)
		{
			return default(Rect);
		}
		Vector2 vector = _points[0];
		float num = vector.x;
		float num2 = vector.x;
		float num3 = vector.y;
		float num4 = vector.y;
		for (int i = 1; i < _points.Length; i++)
		{
			Vector2 vector2 = _points[i];
			num = ((vector2.x >= num) ? num : vector2.x);
			num2 = ((vector2.x <= num2) ? num2 : vector2.x);
			num3 = ((vector2.y >= num3) ? num3 : vector2.y);
			num4 = ((vector2.y <= num4) ? num4 : vector2.y);
		}
		return Rect.MinMaxRect(num, num3, num2, num4);
	}

	// Token: 0x06003D8C RID: 15756 RVA: 0x00125B0A File Offset: 0x00123F0A
	private static int SortPoints(Vector2 _v0, Vector2 _v1)
	{
		return _v0.x.CompareTo(_v1.x);
	}

	// Token: 0x06003D8D RID: 15757 RVA: 0x00125B20 File Offset: 0x00123F20
	public static float TriangleArea(Vector2 _a, Vector2 _b, Vector2 _c)
	{
		return (_b.x - _a.x) * (_c.y - _a.y) - (_c.x - _a.x) * (_b.y - _a.y);
	}

	// Token: 0x06003D8E RID: 15758 RVA: 0x00125B6C File Offset: 0x00123F6C
	public bool ContainsPoint(Vector2 _point)
	{
		if (this.m_points.Length < 3)
		{
			return false;
		}
		if (this.m_points.Length > 4 && !this.m_bounds.Contains(_point))
		{
			return false;
		}
		for (int i = 1; i < this.m_points.Length; i++)
		{
			if (Hull2D.TriangleArea(this.m_points[i - 1], this.m_points[i], _point) < 0f)
			{
				return false;
			}
		}
		return Hull2D.TriangleArea(this.m_points[this.m_points.Length - 1], this.m_points[0], _point) >= 0f;
	}

	// Token: 0x04003169 RID: 12649
	[SerializeField]
	[ReadOnly]
	public Vector2[] m_points;

	// Token: 0x0400316A RID: 12650
	[SerializeField]
	[ReadOnly]
	public Rect m_bounds;

	// Token: 0x0400316B RID: 12651
	[CompilerGenerated]
	private static Comparison<Vector2> <>f__mg$cache0;

	// Token: 0x0400316C RID: 12652
	[CompilerGenerated]
	private static Comparison<Vector2> <>f__mg$cache1;
}
