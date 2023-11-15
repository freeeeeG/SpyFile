using System;
using System.Collections;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Data;
using GameResources;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000040 RID: 64
public class GameResourceLoader
{
	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600010D RID: 269 RVA: 0x0000669F File Offset: 0x0000489F
	// (set) Token: 0x0600010E RID: 270 RVA: 0x000066A6 File Offset: 0x000048A6
	public static GameResourceLoader instance { get; private set; }

	// Token: 0x0600010F RID: 271 RVA: 0x000066AE File Offset: 0x000048AE
	public static void Load()
	{
		GameResourceLoader.instance = new GameResourceLoader();
		GameResourceLoader.instance.LoadInternal();
	}

	// Token: 0x06000110 RID: 272 RVA: 0x000066C4 File Offset: 0x000048C4
	private void LoadInternal()
	{
		this.PreloadStrings();
	}

	// Token: 0x06000111 RID: 273 RVA: 0x000066CC File Offset: 0x000048CC
	public void WaitForStrings()
	{
		this._localizationStringHandle.WaitForCompletion();
	}

	// Token: 0x06000112 RID: 274 RVA: 0x000066DC File Offset: 0x000048DC
	public void WaitForCompletion()
	{
		this.PreloadStrings();
		this.PreloadLevel();
		this.PreloadGear();
		this.PreloadCommon();
		this.PreloadMaterial();
		this.PreloadHUD();
		this._localizationStringHandle.WaitForCompletion();
		this._gearHandle.WaitForCompletion();
		this._levelHandle.WaitForCompletion();
		this._commonHandle.WaitForCompletion();
		this._materialHandle.WaitForCompletion();
		this._hudHandle.WaitForCompletion();
		Singleton<Service>.Instance.gearManager.Initialize();
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00006764 File Offset: 0x00004964
	public void Clear()
	{
		GameResourceLoader.instance = null;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x0000676C File Offset: 0x0000496C
	private void PreloadStrings()
	{
		if (this._localizationStringHandle.IsValid())
		{
			return;
		}
		this._localizationStringHandle = Addressables.LoadAssetAsync<LocalizationStringResource>("LocalizationStringResource");
		this._localizationStringHandle.Completed += this.OnStringLoaded;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000067A3 File Offset: 0x000049A3
	private void OnStringLoaded(AsyncOperationHandle<LocalizationStringResource> handle)
	{
		handle.Result.Initialize();
		this.PreloadLevel();
		this.PreloadGear();
		this.PreloadCommon();
		this.PreloadMaterial();
		this.PreloadHUD();
	}

	// Token: 0x06000116 RID: 278 RVA: 0x000067CF File Offset: 0x000049CF
	private void PreloadLevel()
	{
		if (this._levelHandle.IsValid())
		{
			return;
		}
		this._levelHandle = Addressables.LoadAssetAsync<LevelResource>("LevelResource");
		this._levelHandle.Completed += this.OnLevelLoaded;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00006806 File Offset: 0x00004A06
	private void OnLevelLoaded(AsyncOperationHandle<LevelResource> handle)
	{
		handle.Result.Initialize();
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00006814 File Offset: 0x00004A14
	private void PreloadGear()
	{
		if (this._gearHandle.IsValid())
		{
			return;
		}
		this._gearHandle = Addressables.LoadAssetAsync<GearResource>("GearResource");
		this._gearHandle.Completed += this.OnGearLoaded;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x0000684B File Offset: 0x00004A4B
	private void OnGearLoaded(AsyncOperationHandle<GearResource> handle)
	{
		handle.Result.Initialize();
		this.PreloadSavedGear();
	}

	// Token: 0x0600011A RID: 282 RVA: 0x0000685F File Offset: 0x00004A5F
	private void PreloadCommon()
	{
		if (this._commonHandle.IsValid())
		{
			return;
		}
		this._commonHandle = Addressables.LoadAssetAsync<CommonResource>("CommonResource");
		this._commonHandle.Completed += this.OnResourceLoaded;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00006896 File Offset: 0x00004A96
	private void OnResourceLoaded(AsyncOperationHandle<CommonResource> handle)
	{
		handle.Result.Initialize();
	}

	// Token: 0x0600011C RID: 284 RVA: 0x000068A4 File Offset: 0x00004AA4
	private void PreloadMaterial()
	{
		if (this._materialHandle.IsValid())
		{
			return;
		}
		this._materialHandle = Addressables.LoadAssetAsync<MaterialResource>("MaterialResource");
		this._materialHandle.Completed += this.OnMaterialsLoaded;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x000068DB File Offset: 0x00004ADB
	private void OnMaterialsLoaded(AsyncOperationHandle<MaterialResource> handle)
	{
		handle.Result.Initialize();
	}

	// Token: 0x0600011E RID: 286 RVA: 0x000068E9 File Offset: 0x00004AE9
	private void PreloadHUD()
	{
		if (this._hudHandle.IsValid())
		{
			return;
		}
		this._hudHandle = Addressables.LoadAssetAsync<HUDResource>("HUDResource");
		this._hudHandle.Completed += this.OnHUDResourceLoaded;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00006920 File Offset: 0x00004B20
	private void OnHUDResourceLoaded(AsyncOperationHandle<HUDResource> handle)
	{
		handle.Result.Initialize();
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0000692E File Offset: 0x00004B2E
	public void PreloadSavedGear()
	{
		CoroutineProxy.instance.StartCoroutine(this.CPreloadSavedGear());
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00006941 File Offset: 0x00004B41
	private IEnumerator CPreloadSavedGear()
	{
		GameData.Save save = GameData.Save.instance;
		while (!save.initilaized)
		{
			yield return null;
		}
		if (!save.hasSave)
		{
			yield break;
		}
		this._gearHandle.WaitForCompletion();
		this._itemRequests = new ItemRequest[save.items.length];
		GearResource instance = GearResource.instance;
		WeaponReference weaponReference;
		if (instance.TryGetWeaponReferenceByName(save.nextWeapon, out weaponReference))
		{
			this._weaponRequest2 = weaponReference.LoadAsync();
		}
		WeaponReference weaponReference2;
		if (instance.TryGetWeaponReferenceByName(save.currentWeapon, out weaponReference2))
		{
			this._weaponRequest1 = weaponReference2.LoadAsync();
		}
		EssenceReference essenceReference;
		if (instance.TryGetEssenceReferenceByName(save.essence, out essenceReference))
		{
			this._essenceRequest = essenceReference.LoadAsync();
		}
		for (int i = 0; i < save.items.length; i++)
		{
			ItemReference itemReference;
			if (instance.TryGetItemReferenceByName(save.items[i], out itemReference))
			{
				this._itemRequests[i] = itemReference.LoadAsync();
			}
		}
		yield break;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00006950 File Offset: 0x00004B50
	public Weapon TakeWeapon1()
	{
		this._weaponRequest1.WaitForCompletion();
		Weapon result = Singleton<Service>.Instance.levelManager.DropWeapon(this._weaponRequest1, Vector3.zero);
		this._weaponRequest1 = null;
		return result;
	}

	// Token: 0x06000123 RID: 291 RVA: 0x0000697E File Offset: 0x00004B7E
	public Weapon TakeWeapon2()
	{
		if (this._weaponRequest2 == null)
		{
			return null;
		}
		this._weaponRequest2.WaitForCompletion();
		Weapon result = Singleton<Service>.Instance.levelManager.DropWeapon(this._weaponRequest2, Vector3.zero);
		this._weaponRequest2 = null;
		return result;
	}

	// Token: 0x06000124 RID: 292 RVA: 0x000069B6 File Offset: 0x00004BB6
	public Quintessence TakeEssence()
	{
		if (this._essenceRequest == null)
		{
			return null;
		}
		this._essenceRequest.WaitForCompletion();
		Quintessence result = Singleton<Service>.Instance.levelManager.DropQuintessence(this._essenceRequest, Vector3.zero);
		this._essenceRequest = null;
		return result;
	}

	// Token: 0x06000125 RID: 293 RVA: 0x000069F0 File Offset: 0x00004BF0
	public Item TakeItem(int index)
	{
		ref ItemRequest ptr = ref this._itemRequests[index];
		if (ptr == null)
		{
			return null;
		}
		ptr.WaitForCompletion();
		Item result = Singleton<Service>.Instance.levelManager.DropItem(ptr, Vector3.zero);
		ptr = null;
		return result;
	}

	// Token: 0x040000F5 RID: 245
	private AsyncOperationHandle<LocalizationStringResource> _localizationStringHandle;

	// Token: 0x040000F6 RID: 246
	private AsyncOperationHandle<GearResource> _gearHandle;

	// Token: 0x040000F7 RID: 247
	private AsyncOperationHandle<LevelResource> _levelHandle;

	// Token: 0x040000F8 RID: 248
	private AsyncOperationHandle<CommonResource> _commonHandle;

	// Token: 0x040000F9 RID: 249
	private AsyncOperationHandle<MaterialResource> _materialHandle;

	// Token: 0x040000FA RID: 250
	private AsyncOperationHandle<HUDResource> _hudHandle;

	// Token: 0x040000FB RID: 251
	private WeaponRequest _weaponRequest1;

	// Token: 0x040000FC RID: 252
	private WeaponRequest _weaponRequest2;

	// Token: 0x040000FD RID: 253
	private EssenceRequest _essenceRequest;

	// Token: 0x040000FE RID: 254
	private ItemRequest[] _itemRequests;
}
