using System;

// Token: 0x020009E4 RID: 2532
public readonly struct SpaceScannerTarget
{
	// Token: 0x06004B9E RID: 19358 RVA: 0x001A871B File Offset: 0x001A691B
	private SpaceScannerTarget(string id)
	{
		this.id = id;
	}

	// Token: 0x06004B9F RID: 19359 RVA: 0x001A8724 File Offset: 0x001A6924
	public static SpaceScannerTarget MeteorShower()
	{
		return new SpaceScannerTarget("meteor_shower");
	}

	// Token: 0x06004BA0 RID: 19360 RVA: 0x001A8730 File Offset: 0x001A6930
	public static SpaceScannerTarget BallisticObject()
	{
		return new SpaceScannerTarget("ballistic_object");
	}

	// Token: 0x06004BA1 RID: 19361 RVA: 0x001A873C File Offset: 0x001A693C
	public static SpaceScannerTarget RocketBaseGame(LaunchConditionManager rocket)
	{
		return new SpaceScannerTarget(string.Format("rocket_base_game::{0}", rocket.GetComponent<KPrefabID>().InstanceID));
	}

	// Token: 0x06004BA2 RID: 19362 RVA: 0x001A875D File Offset: 0x001A695D
	public static SpaceScannerTarget RocketDlc1(Clustercraft rocket)
	{
		return new SpaceScannerTarget(string.Format("rocket_dlc1::{0}", rocket.GetComponent<KPrefabID>().InstanceID));
	}

	// Token: 0x04003161 RID: 12641
	public readonly string id;
}
