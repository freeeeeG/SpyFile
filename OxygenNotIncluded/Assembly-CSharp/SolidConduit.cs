using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200068C RID: 1676
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduit")]
public class SolidConduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr
{
	// Token: 0x06002CE1 RID: 11489 RVA: 0x000EE7CC File Offset: 0x000EC9CC
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06002CE2 RID: 11490 RVA: 0x000EE7E2 File Offset: 0x000EC9E2
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002CE3 RID: 11491 RVA: 0x000EE7F1 File Offset: 0x000EC9F1
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.solidConduitSystem;
	}

	// Token: 0x06002CE4 RID: 11492 RVA: 0x000EE7FD File Offset: 0x000EC9FD
	public UtilityNetwork GetNetwork()
	{
		return this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
	}

	// Token: 0x06002CE5 RID: 11493 RVA: 0x000EE810 File Offset: 0x000ECA10
	public static SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06002CE6 RID: 11494 RVA: 0x000EE81C File Offset: 0x000ECA1C
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x06002CE7 RID: 11495 RVA: 0x000EE829 File Offset: 0x000ECA29
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Conveyor, this);
	}

	// Token: 0x06002CE8 RID: 11496 RVA: 0x000EE85C File Offset: 0x000ECA5C
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(this);
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
			SolidConduit.GetFlowManager().EmptyConduit(cell);
		}
		base.OnCleanUp();
	}

	// Token: 0x04001A70 RID: 6768
	[MyCmpReq]
	private KAnimGraphTileVisualizer graphTileDependency;

	// Token: 0x04001A71 RID: 6769
	private System.Action firstFrameCallback;
}
