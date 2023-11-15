using System;
using UnityEngine;

// Token: 0x02000105 RID: 261
[CreateAssetMenu(menuName = "Factory/TileFactory", fileName = "TileFactory")]
public class TileFactory : GameObjectFactory
{
	// Token: 0x06000685 RID: 1669 RVA: 0x00011B98 File Offset: 0x0000FD98
	public void Initialize()
	{
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00011B9A File Offset: 0x0000FD9A
	public GameTile GetBasicTile()
	{
		return base.CreateInstance(this.basicTilePrefab) as BasicTile;
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00011BAD File Offset: 0x0000FDAD
	public GroundTile GetGroundTile()
	{
		return base.CreateInstance(this.groundTile) as GroundTile;
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x00011BC0 File Offset: 0x0000FDC0
	public GameObject GetTutorialPrefab()
	{
		return this.tutorialPrefab;
	}

	// Token: 0x04000303 RID: 771
	[SerializeField]
	private GroundTile groundTile;

	// Token: 0x04000304 RID: 772
	[SerializeField]
	private GameTile basicTilePrefab;

	// Token: 0x04000305 RID: 773
	[SerializeField]
	private GameObject tutorialPrefab;
}
