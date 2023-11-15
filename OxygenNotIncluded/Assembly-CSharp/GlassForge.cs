using System;
using UnityEngine;

// Token: 0x0200060F RID: 1551
public class GlassForge : ComplexFabricator
{
	// Token: 0x06002708 RID: 9992 RVA: 0x000D3F68 File Offset: 0x000D2168
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<GlassForge>(-2094018600, GlassForge.CheckPipesDelegate);
	}

	// Token: 0x06002709 RID: 9993 RVA: 0x000D3F84 File Offset: 0x000D2184
	private void CheckPipes(object data)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		int cell = Grid.OffsetCell(Grid.PosToCell(this), GlassForgeConfig.outPipeOffset);
		GameObject gameObject = Grid.Objects[cell, 16];
		if (!(gameObject != null))
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		if (gameObject.GetComponent<PrimaryElement>().Element.highTemp > ElementLoader.FindElementByHash(SimHashes.MoltenGlass).lowTemp)
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		this.statusHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.PipeMayMelt, null);
	}

	// Token: 0x04001661 RID: 5729
	private Guid statusHandle;

	// Token: 0x04001662 RID: 5730
	private static readonly EventSystem.IntraObjectHandler<GlassForge> CheckPipesDelegate = new EventSystem.IntraObjectHandler<GlassForge>(delegate(GlassForge component, object data)
	{
		component.CheckPipes(data);
	});
}
