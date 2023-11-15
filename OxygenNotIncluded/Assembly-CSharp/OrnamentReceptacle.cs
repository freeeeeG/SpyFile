using System;
using UnityEngine;

// Token: 0x02000668 RID: 1640
public class OrnamentReceptacle : SingleEntityReceptacle
{
	// Token: 0x06002B6E RID: 11118 RVA: 0x000E70D8 File Offset: 0x000E52D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002B6F RID: 11119 RVA: 0x000E70E0 File Offset: 0x000E52E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("snapTo_ornament", false);
	}

	// Token: 0x06002B70 RID: 11120 RVA: 0x000E7100 File Offset: 0x000E5300
	protected override void PositionOccupyingObject()
	{
		KBatchedAnimController component = base.occupyingObject.GetComponent<KBatchedAnimController>();
		component.transform.SetLocalPosition(new Vector3(0f, 0f, -0.1f));
		this.occupyingTracker = base.occupyingObject.AddComponent<KBatchedAnimTracker>();
		this.occupyingTracker.symbol = new HashedString("snapTo_ornament");
		this.occupyingTracker.forceAlwaysVisible = true;
		this.animLink = new KAnimLink(base.GetComponent<KBatchedAnimController>(), component);
	}

	// Token: 0x06002B71 RID: 11121 RVA: 0x000E7180 File Offset: 0x000E5380
	protected override void ClearOccupant()
	{
		if (this.occupyingTracker != null)
		{
			UnityEngine.Object.Destroy(this.occupyingTracker);
			this.occupyingTracker = null;
		}
		if (this.animLink != null)
		{
			this.animLink.Unregister();
			this.animLink = null;
		}
		base.ClearOccupant();
	}

	// Token: 0x04001973 RID: 6515
	[MyCmpReq]
	private SnapOn snapOn;

	// Token: 0x04001974 RID: 6516
	private KBatchedAnimTracker occupyingTracker;

	// Token: 0x04001975 RID: 6517
	private KAnimLink animLink;
}
