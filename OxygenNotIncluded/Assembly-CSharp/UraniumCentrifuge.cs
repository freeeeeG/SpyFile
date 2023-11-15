using System;
using UnityEngine;

// Token: 0x020006B2 RID: 1714
public class UraniumCentrifuge : ComplexFabricator
{
	// Token: 0x06002E9B RID: 11931 RVA: 0x000F64B6 File Offset: 0x000F46B6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<UraniumCentrifuge>(-1697596308, UraniumCentrifuge.DropEnrichedProductDelegate);
		base.Subscribe<UraniumCentrifuge>(-2094018600, UraniumCentrifuge.CheckPipesDelegate);
	}

	// Token: 0x06002E9C RID: 11932 RVA: 0x000F64E0 File Offset: 0x000F46E0
	private void DropEnrichedProducts(object data)
	{
		Storage[] components = base.GetComponents<Storage>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].Drop(ElementLoader.FindElementByHash(SimHashes.EnrichedUranium).tag);
		}
	}

	// Token: 0x06002E9D RID: 11933 RVA: 0x000F651C File Offset: 0x000F471C
	private void CheckPipes(object data)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		int cell = Grid.OffsetCell(Grid.PosToCell(this), UraniumCentrifugeConfig.outPipeOffset);
		GameObject gameObject = Grid.Objects[cell, 16];
		if (!(gameObject != null))
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		if (gameObject.GetComponent<PrimaryElement>().Element.highTemp > ElementLoader.FindElementByHash(SimHashes.MoltenUranium).lowTemp)
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		this.statusHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.PipeMayMelt, null);
	}

	// Token: 0x04001B96 RID: 7062
	private Guid statusHandle;

	// Token: 0x04001B97 RID: 7063
	private static readonly EventSystem.IntraObjectHandler<UraniumCentrifuge> CheckPipesDelegate = new EventSystem.IntraObjectHandler<UraniumCentrifuge>(delegate(UraniumCentrifuge component, object data)
	{
		component.CheckPipes(data);
	});

	// Token: 0x04001B98 RID: 7064
	private static readonly EventSystem.IntraObjectHandler<UraniumCentrifuge> DropEnrichedProductDelegate = new EventSystem.IntraObjectHandler<UraniumCentrifuge>(delegate(UraniumCentrifuge component, object data)
	{
		component.DropEnrichedProducts(data);
	});
}
