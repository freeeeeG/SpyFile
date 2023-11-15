using System;
using KSerialization;

// Token: 0x0200066F RID: 1647
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantAirConditioner : AirConditioner
{
	// Token: 0x06002B8E RID: 11150 RVA: 0x000E7B88 File Offset: 0x000E5D88
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<PlantAirConditioner>(-1396791468, PlantAirConditioner.OnFertilizedDelegate);
		base.Subscribe<PlantAirConditioner>(-1073674739, PlantAirConditioner.OnUnfertilizedDelegate);
	}

	// Token: 0x06002B8F RID: 11151 RVA: 0x000E7BB2 File Offset: 0x000E5DB2
	private void OnFertilized(object data)
	{
		this.operational.SetFlag(PlantAirConditioner.fertilizedFlag, true);
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x000E7BC5 File Offset: 0x000E5DC5
	private void OnUnfertilized(object data)
	{
		this.operational.SetFlag(PlantAirConditioner.fertilizedFlag, false);
	}

	// Token: 0x04001994 RID: 6548
	private static readonly Operational.Flag fertilizedFlag = new Operational.Flag("fertilized", Operational.Flag.Type.Requirement);

	// Token: 0x04001995 RID: 6549
	private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnFertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>(delegate(PlantAirConditioner component, object data)
	{
		component.OnFertilized(data);
	});

	// Token: 0x04001996 RID: 6550
	private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnUnfertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>(delegate(PlantAirConditioner component, object data)
	{
		component.OnUnfertilized(data);
	});
}
