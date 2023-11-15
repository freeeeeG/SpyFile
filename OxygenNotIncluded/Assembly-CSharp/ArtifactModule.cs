using System;
using UnityEngine;

// Token: 0x0200058E RID: 1422
public class ArtifactModule : SingleEntityReceptacle, IRenderEveryTick
{
	// Token: 0x06002264 RID: 8804 RVA: 0x000BD068 File Offset: 0x000BB268
	protected override void OnSpawn()
	{
		this.craft = this.module.CraftInterface.GetComponent<Clustercraft>();
		if (this.craft.Status == Clustercraft.CraftStatus.InFlight && base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(false);
		}
		base.OnSpawn();
		base.Subscribe(705820818, new Action<object>(this.OnEnterSpace));
		base.Subscribe(-1165815793, new Action<object>(this.OnExitSpace));
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x000BD0E9 File Offset: 0x000BB2E9
	public void RenderEveryTick(float dt)
	{
		this.ArtifactTrackModulePosition();
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x000BD0F4 File Offset: 0x000BB2F4
	private void ArtifactTrackModulePosition()
	{
		this.occupyingObjectRelativePosition = this.animController.Offset + Vector3.up * 0.5f + new Vector3(0f, 0f, -1f);
		if (base.occupyingObject != null)
		{
			this.PositionOccupyingObject();
		}
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x000BD153 File Offset: 0x000BB353
	private void OnEnterSpace(object data)
	{
		if (base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(false);
		}
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x000BD16F File Offset: 0x000BB36F
	private void OnExitSpace(object data)
	{
		if (base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(true);
		}
	}

	// Token: 0x040013A4 RID: 5028
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x040013A5 RID: 5029
	[MyCmpReq]
	private RocketModuleCluster module;

	// Token: 0x040013A6 RID: 5030
	private Clustercraft craft;
}
