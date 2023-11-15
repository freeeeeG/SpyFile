using System;
using UnityEngine;

// Token: 0x02000934 RID: 2356
[AddComponentMenu("KMonoBehaviour/scripts/Reservoir")]
public class Reservoir : KMonoBehaviour
{
	// Token: 0x06004469 RID: 17513 RVA: 0x0017FC80 File Offset: 0x0017DE80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_fill",
			"meter_OL"
		});
		base.Subscribe<Reservoir>(-1697596308, Reservoir.OnStorageChangeDelegate);
		this.OnStorageChange(null);
	}

	// Token: 0x0600446A RID: 17514 RVA: 0x0017FCDF File Offset: 0x0017DEDF
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
	}

	// Token: 0x04002D56 RID: 11606
	private MeterController meter;

	// Token: 0x04002D57 RID: 11607
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002D58 RID: 11608
	private static readonly EventSystem.IntraObjectHandler<Reservoir> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Reservoir>(delegate(Reservoir component, object data)
	{
		component.OnStorageChange(data);
	});
}
