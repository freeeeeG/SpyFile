using System;
using UnityEngine;

// Token: 0x02000503 RID: 1283
[AddComponentMenu("KMonoBehaviour/scripts/SimpleVent")]
public class SimpleVent : KMonoBehaviour
{
	// Token: 0x06001E2F RID: 7727 RVA: 0x000A1155 File Offset: 0x0009F355
	protected override void OnPrefabInit()
	{
		base.Subscribe<SimpleVent>(-592767678, SimpleVent.OnChangedDelegate);
		base.Subscribe<SimpleVent>(-111137758, SimpleVent.OnChangedDelegate);
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x000A1179 File Offset: 0x0009F379
	protected override void OnSpawn()
	{
		this.OnChanged(null);
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x000A1184 File Offset: 0x0009F384
	private void OnChanged(object data)
	{
		if (this.operational.IsFunctional)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, this);
			return;
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
	}

	// Token: 0x040010E6 RID: 4326
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040010E7 RID: 4327
	private static readonly EventSystem.IntraObjectHandler<SimpleVent> OnChangedDelegate = new EventSystem.IntraObjectHandler<SimpleVent>(delegate(SimpleVent component, object data)
	{
		component.OnChanged(data);
	});
}
