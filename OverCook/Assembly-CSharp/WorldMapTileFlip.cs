using System;
using UnityEngine;

// Token: 0x02000BF7 RID: 3063
public class WorldMapTileFlip : WorldMapFlipperBase
{
	// Token: 0x06003E8C RID: 16012 RVA: 0x0012B530 File Offset: 0x00129930
	protected override void Awake()
	{
		base.Awake();
		if (this.m_flipOwnerData != null)
		{
			if (base.gameObject.GetComponent<GridAutoParenting>() == null)
			{
				if (this.m_flipOwnerData.m_levelMapNode != null)
				{
					this.m_startFlipDelay = 0f;
					this.m_flipOwnerData.m_levelMapNode.AddToFlipSet(this.GetOrder(), this);
				}
			}
			else
			{
				this.m_startFlipDelay += 0.2f * (float)base.transform.GetSiblingIndex();
			}
		}
	}

	// Token: 0x06003E8D RID: 16013 RVA: 0x0012B5C0 File Offset: 0x001299C0
	private int GetOrder()
	{
		GridManager gridManager = GameUtils.GetGridManager(base.transform);
		GridIndex unclampedGridLocationFromPos = gridManager.GetUnclampedGridLocationFromPos(this.m_flipOwnerData.m_levelMapNode.transform.position);
		GridIndex unclampedGridLocationFromPos2 = gridManager.GetUnclampedGridLocationFromPos(base.transform.position);
		return HexGridManager.ComputeDistanceHexGrid(unclampedGridLocationFromPos, unclampedGridLocationFromPos2);
	}

	// Token: 0x06003E8E RID: 16014 RVA: 0x0012B60E File Offset: 0x00129A0E
	public MapNode GetMapNode()
	{
		if (this.m_flipOwnerData != null)
		{
			return this.m_flipOwnerData.m_levelMapNode;
		}
		return null;
	}

	// Token: 0x06003E8F RID: 16015 RVA: 0x0012B628 File Offset: 0x00129A28
	public void DrawBoundingHex()
	{
	}

	// Token: 0x0400323D RID: 12861
	[SerializeField]
	public WorldMapTileFlip.FlipOwnerData m_flipOwnerData = new WorldMapTileFlip.FlipOwnerData();

	// Token: 0x02000BF8 RID: 3064
	[Serializable]
	public class FlipOwnerData
	{
		// Token: 0x0400323E RID: 12862
		[SerializeField]
		public MapNode m_levelMapNode;
	}
}
