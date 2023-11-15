using System;

// Token: 0x0200065E RID: 1630
public class ModuleBattery : Battery
{
	// Token: 0x06002AFF RID: 11007 RVA: 0x000E57D4 File Offset: 0x000E39D4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.connectedTags = new Tag[0];
		base.IsVirtual = true;
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x000E57F0 File Offset: 0x000E39F0
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		base.OnSpawn();
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
	}
}
