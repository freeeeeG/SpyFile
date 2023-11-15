using System;
using UnityEngine;

// Token: 0x020005B1 RID: 1457
public class BuildingUnderConstruction : Building
{
	// Token: 0x060023AD RID: 9133 RVA: 0x000C3378 File Offset: 0x000C1578
	protected override void OnPrefabInit()
	{
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(this.Def.SceneLayer);
		base.transform.SetPosition(position);
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Construction"));
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Rotatable component2 = base.GetComponent<Rotatable>();
		if (component != null && component2 == null)
		{
			component.Offset = this.Def.GetVisualizerOffset();
		}
		KBoxCollider2D component3 = base.GetComponent<KBoxCollider2D>();
		if (component3 != null)
		{
			Vector3 visualizerOffset = this.Def.GetVisualizerOffset();
			component3.offset += new Vector2(visualizerOffset.x, visualizerOffset.y);
		}
		base.OnPrefabInit();
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x000C3444 File Offset: 0x000C1644
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.Def.IsTilePiece)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.Def.RunOnArea(cell, base.Orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
			});
		}
		base.RegisterBlockTileRenderer();
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x000C3499 File Offset: 0x000C1699
	protected override void OnCleanUp()
	{
		base.UnregisterBlockTileRenderer();
		base.OnCleanUp();
	}

	// Token: 0x0400145B RID: 5211
	[MyCmpAdd]
	private KSelectable selectable;

	// Token: 0x0400145C RID: 5212
	[MyCmpAdd]
	private SaveLoadRoot saveLoadRoot;

	// Token: 0x0400145D RID: 5213
	[MyCmpAdd]
	private KPrefabID kPrefabID;
}
