using System;
using Rendering;
using UnityEngine;

// Token: 0x020004C0 RID: 1216
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGridTileVisualizer")]
public class KAnimGridTileVisualizer : KMonoBehaviour, IBlockTileInfo
{
	// Token: 0x06001BA0 RID: 7072 RVA: 0x00093E03 File Offset: 0x00092003
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<KAnimGridTileVisualizer>(-1503271301, KAnimGridTileVisualizer.OnSelectionChangedDelegate);
		base.Subscribe<KAnimGridTileVisualizer>(-1201923725, KAnimGridTileVisualizer.OnHighlightChangedDelegate);
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x00093E30 File Offset: 0x00092030
	protected override void OnCleanUp()
	{
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			ObjectLayer tileLayer = component.Def.TileLayer;
			if (Grid.Objects[cell, (int)tileLayer] == base.gameObject)
			{
				Grid.Objects[cell, (int)tileLayer] = null;
			}
			TileVisualizer.RefreshCell(cell, tileLayer, component.Def.ReplacementLayer);
		}
		base.OnCleanUp();
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x00093EA8 File Offset: 0x000920A8
	private void OnSelectionChanged(object data)
	{
		bool enabled = (bool)data;
		World.Instance.blockTileRenderer.SelectCell(Grid.PosToCell(base.transform.GetPosition()), enabled);
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x00093EDC File Offset: 0x000920DC
	private void OnHighlightChanged(object data)
	{
		bool enabled = (bool)data;
		World.Instance.blockTileRenderer.HighlightCell(Grid.PosToCell(base.transform.GetPosition()), enabled);
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x00093F10 File Offset: 0x00092110
	public int GetBlockTileConnectorID()
	{
		return this.blockTileConnectorID;
	}

	// Token: 0x04000F5E RID: 3934
	[SerializeField]
	public int blockTileConnectorID;

	// Token: 0x04000F5F RID: 3935
	private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnSelectionChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>(delegate(KAnimGridTileVisualizer component, object data)
	{
		component.OnSelectionChanged(data);
	});

	// Token: 0x04000F60 RID: 3936
	private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnHighlightChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>(delegate(KAnimGridTileVisualizer component, object data)
	{
		component.OnHighlightChanged(data);
	});
}
