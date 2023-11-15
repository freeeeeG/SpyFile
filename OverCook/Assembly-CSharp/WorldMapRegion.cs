using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BBD RID: 3005
public class WorldMapRegion : MonoBehaviour
{
	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x06003D7E RID: 15742 RVA: 0x001256F7 File Offset: 0x00123AF7
	[SerializeField]
	public int Priority
	{
		get
		{
			return this.m_priority;
		}
	}

	// Token: 0x06003D7F RID: 15743 RVA: 0x00125700 File Offset: 0x00123B00
	private void Awake()
	{
		if (WorldMapRegion.s_allRegions == null)
		{
			WorldMapRegion.s_allRegions = new List<WorldMapRegion>(1);
		}
		WorldMapRegion.s_allRegions.Add(this);
		WorldMapRegion.s_allRegions.Sort((WorldMapRegion x, WorldMapRegion y) => y.m_priority - x.m_priority);
	}

	// Token: 0x06003D80 RID: 15744 RVA: 0x00125754 File Offset: 0x00123B54
	private void OnDestroy()
	{
		WorldMapRegion.s_allRegions.Remove(this);
		if (WorldMapRegion.s_allRegions.Count == 0)
		{
			WorldMapRegion.s_allRegions = null;
		}
	}

	// Token: 0x06003D81 RID: 15745 RVA: 0x00125777 File Offset: 0x00123B77
	public static WorldMapRegion FindRegionForPoint(Vector3 _point)
	{
		return WorldMapRegion.FindRegionForPoint(new Vector2(_point.x, _point.z));
	}

	// Token: 0x06003D82 RID: 15746 RVA: 0x00125794 File Offset: 0x00123B94
	public static WorldMapRegion FindRegionForPoint(Vector2 _point)
	{
		if (WorldMapRegion.s_allRegions == null)
		{
			return null;
		}
		for (int i = 0; i < WorldMapRegion.s_allRegions.Count; i++)
		{
			WorldMapRegion worldMapRegion = WorldMapRegion.s_allRegions[i];
			if (worldMapRegion.ContainsPoint(_point))
			{
				return worldMapRegion;
			}
		}
		return null;
	}

	// Token: 0x06003D83 RID: 15747 RVA: 0x001257E3 File Offset: 0x00123BE3
	public bool ContainsPoint(Vector3 _point)
	{
		return this.m_hull != null && this.m_hull.ContainsPoint(new Vector2(_point.x, _point.z));
	}

	// Token: 0x06003D84 RID: 15748 RVA: 0x00125810 File Offset: 0x00123C10
	public bool ContainsPoint(Vector2 _point)
	{
		return this.m_hull != null && this.m_hull.ContainsPoint(_point);
	}

	// Token: 0x04003164 RID: 12644
	[SerializeField]
	private int m_priority;

	// Token: 0x04003165 RID: 12645
	[SerializeField]
	public Hull2D m_hull;

	// Token: 0x04003166 RID: 12646
	[SerializeField]
	public WorldMapRegionData m_data;

	// Token: 0x04003167 RID: 12647
	private static List<WorldMapRegion> s_allRegions;
}
