using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000106 RID: 262
[CreateAssetMenu(menuName = "Factory/ShapeFactory", fileName = "ShapeFactory")]
public class TileShapeFactory : ScriptableObject
{
	// Token: 0x0600068A RID: 1674 RVA: 0x00011BD0 File Offset: 0x0000FDD0
	public void Initialize()
	{
		this.ShapeDIC = new Dictionary<ShapeType, TileShape>();
		foreach (TileShape tileShape in this.ShapePrefabs)
		{
			this.ShapeDIC.Add(tileShape.shapeType, tileShape);
		}
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x00011C14 File Offset: 0x0000FE14
	public TileShape GetRandomShape()
	{
		int type = StaticData.RandomNumber(this.RandomShapeChance);
		return this.GetShape((ShapeType)type);
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x00011C34 File Offset: 0x0000FE34
	public TileShape GetDShape()
	{
		return this.GetShape(ShapeType.D);
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x00011C3D File Offset: 0x0000FE3D
	public TileShape GetShape(ShapeType type)
	{
		if (this.ShapeDIC.ContainsKey(type))
		{
			return Object.Instantiate<TileShape>(this.ShapeDIC[type]);
		}
		return null;
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x00011C60 File Offset: 0x0000FE60
	public TileShape GetShape(ShapeInfo shapeInfo)
	{
		if (this.ShapeDIC.ContainsKey((ShapeType)shapeInfo.ShapeType))
		{
			TileShape tileShape = Object.Instantiate<TileShape>(this.ShapeDIC[(ShapeType)shapeInfo.ShapeType]);
			tileShape.m_ShapeInfo = shapeInfo;
			return tileShape;
		}
		return null;
	}

	// Token: 0x04000306 RID: 774
	[SerializeField]
	private TileShape[] ShapePrefabs;

	// Token: 0x04000307 RID: 775
	private Dictionary<ShapeType, TileShape> ShapeDIC;

	// Token: 0x04000308 RID: 776
	[SerializeField]
	private float[] RandomShapeChance = new float[7];
}
