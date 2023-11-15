using System;

// Token: 0x0200050D RID: 1293
public class Storable : KMonoBehaviour
{
	// Token: 0x06001E94 RID: 7828 RVA: 0x000A3156 File Offset: 0x000A1356
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Storable>(856640610, Storable.OnStoreDelegate);
		base.Subscribe<Storable>(-778359855, Storable.RefreshStorageTagsDelegate);
	}

	// Token: 0x06001E95 RID: 7829 RVA: 0x000A3180 File Offset: 0x000A1380
	public void OnStore(object data)
	{
		this.RefreshStorageTags(data);
	}

	// Token: 0x06001E96 RID: 7830 RVA: 0x000A318C File Offset: 0x000A138C
	private void RefreshStorageTags(object data = null)
	{
		bool flag = data is Storage || (data != null && (bool)data);
		Storage storage = (Storage)data;
		if (storage != null && storage.gameObject == base.gameObject)
		{
			return;
		}
		KPrefabID component = base.GetComponent<KPrefabID>();
		SaveLoadRoot component2 = base.GetComponent<SaveLoadRoot>();
		KSelectable component3 = base.GetComponent<KSelectable>();
		if (component3)
		{
			component3.IsSelectable = !flag;
		}
		if (flag)
		{
			component.AddTag(GameTags.Stored, false);
			if (storage == null || !storage.allowItemRemoval)
			{
				component.AddTag(GameTags.StoredPrivate, false);
			}
			else
			{
				component.RemoveTag(GameTags.StoredPrivate);
			}
			if (component2 != null)
			{
				component2.SetRegistered(false);
				return;
			}
		}
		else
		{
			component.RemoveTag(GameTags.Stored);
			component.RemoveTag(GameTags.StoredPrivate);
			if (component2 != null)
			{
				component2.SetRegistered(true);
			}
		}
	}

	// Token: 0x0400112B RID: 4395
	private static readonly EventSystem.IntraObjectHandler<Storable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Storable>(delegate(Storable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x0400112C RID: 4396
	private static readonly EventSystem.IntraObjectHandler<Storable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Storable>(delegate(Storable component, object data)
	{
		component.RefreshStorageTags(data);
	});
}
