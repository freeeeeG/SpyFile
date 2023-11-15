using System;
using KSerialization;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public class FoodStorage : KMonoBehaviour
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06001B5F RID: 7007 RVA: 0x00093273 File Offset: 0x00091473
	// (set) Token: 0x06001B60 RID: 7008 RVA: 0x0009327B File Offset: 0x0009147B
	public FilteredStorage FilteredStorage { get; set; }

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06001B61 RID: 7009 RVA: 0x00093284 File Offset: 0x00091484
	// (set) Token: 0x06001B62 RID: 7010 RVA: 0x0009328C File Offset: 0x0009148C
	public bool SpicedFoodOnly
	{
		get
		{
			return this.onlyStoreSpicedFood;
		}
		set
		{
			this.onlyStoreSpicedFood = value;
			base.Trigger(1163645216, this.onlyStoreSpicedFood);
			if (this.onlyStoreSpicedFood)
			{
				this.FilteredStorage.AddForbiddenTag(GameTags.UnspicedFood);
				this.storage.DropUnlessHasTag(GameTags.SpicedFood);
				return;
			}
			this.FilteredStorage.RemoveForbiddenTag(GameTags.UnspicedFood);
		}
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x000932EF File Offset: 0x000914EF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<FoodStorage>(-905833192, FoodStorage.OnCopySettingsDelegate);
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x00093308 File Offset: 0x00091508
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x00093310 File Offset: 0x00091510
	private void OnCopySettings(object data)
	{
		FoodStorage component = ((GameObject)data).GetComponent<FoodStorage>();
		if (component != null)
		{
			this.SpicedFoodOnly = component.SpicedFoodOnly;
		}
	}

	// Token: 0x04000F46 RID: 3910
	[Serialize]
	private bool onlyStoreSpicedFood;

	// Token: 0x04000F47 RID: 3911
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04000F49 RID: 3913
	private static readonly EventSystem.IntraObjectHandler<FoodStorage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FoodStorage>(delegate(FoodStorage component, object data)
	{
		component.OnCopySettings(data);
	});
}
