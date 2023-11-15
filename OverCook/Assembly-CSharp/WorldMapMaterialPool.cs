using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BEA RID: 3050
public static class WorldMapMaterialPool
{
	// Token: 0x06003E49 RID: 15945 RVA: 0x0012A480 File Offset: 0x00128880
	public static void RegisterMaterial(Material material)
	{
		if (WorldMapMaterialPool.m_MaterialPool == null)
		{
			WorldMapMaterialPool.m_MaterialPool = new FastList<Material>();
		}
		if (WorldMapMaterialPool.m_FoldedMaterial == null && material.shader.name.Contains("lerp"))
		{
			WorldMapMaterialPool.m_FoldedMaterial = new Material(material);
			WorldMapMaterialPool.m_FoldedMaterial.SetFloat("_Blend", 0f);
		}
		if (WorldMapMaterialPool.m_MaterialPool.Find((Material x) => x.name.GetHashCode() == material.name.GetHashCode()) == null)
		{
			Material material2 = new Material(material);
			material2.SetFloat("_Blend", 1f);
			WorldMapMaterialPool.m_MaterialPool.Add(material2);
		}
	}

	// Token: 0x06003E4A RID: 15946 RVA: 0x0012A54C File Offset: 0x0012894C
	public static Material GetSharedMaterialForState(Material orignalMaterial, WorldMapMaterialPool.MapState state)
	{
		if (WorldMapMaterialPool.m_MaterialPool == null)
		{
			WorldMapMaterialPool.RegisterMaterial(orignalMaterial);
		}
		if (state != WorldMapMaterialPool.MapState.Folded)
		{
			int count = WorldMapMaterialPool.m_MaterialPool.Count;
			for (int i = 0; i < count; i++)
			{
				Material material = WorldMapMaterialPool.m_MaterialPool._items[i];
				if (material.name.GetHashCode() == orignalMaterial.name.GetHashCode())
				{
					return material;
				}
			}
			WorldMapMaterialPool.RegisterMaterial(orignalMaterial);
			count = WorldMapMaterialPool.m_MaterialPool.Count;
			for (int j = 0; j < count; j++)
			{
				Material material2 = WorldMapMaterialPool.m_MaterialPool._items[j];
				if (material2.name.GetHashCode() == orignalMaterial.name.GetHashCode())
				{
					return material2;
				}
			}
			return null;
		}
		if (!orignalMaterial.shader.name.Contains("lerp"))
		{
			return orignalMaterial;
		}
		return WorldMapMaterialPool.m_FoldedMaterial;
	}

	// Token: 0x0400320B RID: 12811
	private static FastList<Material> m_MaterialPool;

	// Token: 0x0400320C RID: 12812
	private static Material m_FoldedMaterial;

	// Token: 0x02000BEB RID: 3051
	public enum MapState
	{
		// Token: 0x0400320E RID: 12814
		Folded,
		// Token: 0x0400320F RID: 12815
		UnFolded
	}
}
