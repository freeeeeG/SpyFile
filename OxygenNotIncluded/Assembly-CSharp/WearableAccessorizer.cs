using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x0200053B RID: 1339
[AddComponentMenu("KMonoBehaviour/scripts/WearableAccessorizer")]
public class WearableAccessorizer : KMonoBehaviour
{
	// Token: 0x06002006 RID: 8198 RVA: 0x000AB952 File Offset: 0x000A9B52
	public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> GetCustomClothingItems()
	{
		return this.customOutfitItems;
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06002007 RID: 8199 RVA: 0x000AB95A File Offset: 0x000A9B5A
	public Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> Wearables
	{
		get
		{
			return this.wearables;
		}
	}

	// Token: 0x06002008 RID: 8200 RVA: 0x000AB964 File Offset: 0x000A9B64
	public string[] GetClothingItemsIds(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			string[] array = new string[this.customOutfitItems[outfitType].Count];
			for (int i = 0; i < this.customOutfitItems[outfitType].Count; i++)
			{
				array[i] = this.customOutfitItems[outfitType][i].Get().Id;
			}
			return array;
		}
		return new string[0];
	}

	// Token: 0x06002009 RID: 8201 RVA: 0x000AB9D9 File Offset: 0x000A9BD9
	public Option<string> GetJoyResponseId()
	{
		return this.joyResponsePermitId;
	}

	// Token: 0x0600200A RID: 8202 RVA: 0x000AB9E6 File Offset: 0x000A9BE6
	public void SetJoyResponseId(Option<string> joyResponsePermitId)
	{
		this.joyResponsePermitId = joyResponsePermitId.UnwrapOr(null, null);
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x000AB9F8 File Offset: 0x000A9BF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		base.Subscribe(-448952673, new Action<object>(this.EquippedItem));
		base.Subscribe(-1285462312, new Action<object>(this.UnequippedItem));
	}

	// Token: 0x0600200C RID: 8204 RVA: 0x000ABA58 File Offset: 0x000A9C58
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		List<WearableAccessorizer.WearableType> list = new List<WearableAccessorizer.WearableType>();
		foreach (KeyValuePair<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> keyValuePair in this.wearables)
		{
			keyValuePair.Value.Deserialize();
			if (keyValuePair.Value.BuildAnims == null)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (WearableAccessorizer.WearableType key in list)
		{
			this.wearables.Remove(key);
		}
		if (this.clothingItems.Count > 0)
		{
			this.customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>(this.clothingItems);
			this.clothingItems.Clear();
			if (!this.wearables.ContainsKey(WearableAccessorizer.WearableType.CustomClothing))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing])
				{
					this.ApplyClothingItem(ClothingOutfitUtility.OutfitType.Clothing, resourceRef.Get());
				}
			}
		}
		this.ApplyWearable();
	}

	// Token: 0x0600200D RID: 8205 RVA: 0x000ABBAC File Offset: 0x000A9DAC
	public void EquippedItem(object data)
	{
		KPrefabID kprefabID = data as KPrefabID;
		if (kprefabID != null)
		{
			Equippable component = kprefabID.GetComponent<Equippable>();
			this.ApplyEquipment(component, component.GetBuildOverride());
		}
	}

	// Token: 0x0600200E RID: 8206 RVA: 0x000ABBE0 File Offset: 0x000A9DE0
	public void ApplyEquipment(Equippable equippable, KAnimFile animFile)
	{
		WearableAccessorizer.WearableType key;
		if (equippable != null && animFile != null && Enum.TryParse<WearableAccessorizer.WearableType>(equippable.def.Slot, out key))
		{
			if (this.wearables.ContainsKey(key))
			{
				this.RemoveAnimBuild(this.wearables[key].BuildAnims[0], this.wearables[key].buildOverridePriority);
			}
			ClothingOutfitUtility.OutfitType key2;
			if (this.TryGetEquippableClothingType(equippable.def, out key2) && this.customOutfitItems.ContainsKey(key2))
			{
				this.wearables[WearableAccessorizer.WearableType.CustomSuit] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);
				this.wearables[WearableAccessorizer.WearableType.CustomSuit].AddCustomItems(this.customOutfitItems[key2]);
			}
			else
			{
				this.wearables[key] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);
			}
			this.ApplyWearable();
		}
	}

	// Token: 0x0600200F RID: 8207 RVA: 0x000ABCD5 File Offset: 0x000A9ED5
	private bool TryGetEquippableClothingType(EquipmentDef equipment, out ClothingOutfitUtility.OutfitType outfitType)
	{
		if (equipment.Id == "Atmo_Suit")
		{
			outfitType = ClothingOutfitUtility.OutfitType.AtmoSuit;
			return true;
		}
		outfitType = ClothingOutfitUtility.OutfitType.LENGTH;
		return false;
	}

	// Token: 0x06002010 RID: 8208 RVA: 0x000ABCF4 File Offset: 0x000A9EF4
	private bool IsWearingSuitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		Equippable suitEquippable = this.GetSuitEquippable();
		if (suitEquippable != null)
		{
			ClothingOutfitUtility.OutfitType outfitType2;
			this.TryGetEquippableClothingType(suitEquippable.def, out outfitType2);
			return outfitType2 == outfitType;
		}
		return false;
	}

	// Token: 0x06002011 RID: 8209 RVA: 0x000ABD28 File Offset: 0x000A9F28
	private Equippable GetSuitEquippable()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null && component.assignableProxy.Get() != null)
		{
			Equipment equipment = component.GetEquipment();
			Assignable assignable = (equipment != null) ? equipment.GetAssignable(Db.Get().AssignableSlots.Suit) : null;
			if (assignable != null)
			{
				return assignable.GetComponent<Equippable>();
			}
		}
		return null;
	}

	// Token: 0x06002012 RID: 8210 RVA: 0x000ABD94 File Offset: 0x000A9F94
	private WearableAccessorizer.WearableType GetHighestAccessory()
	{
		WearableAccessorizer.WearableType wearableType = WearableAccessorizer.WearableType.Basic;
		foreach (WearableAccessorizer.WearableType wearableType2 in this.wearables.Keys)
		{
			if (wearableType2 > wearableType)
			{
				wearableType = wearableType2;
			}
		}
		return wearableType;
	}

	// Token: 0x06002013 RID: 8211 RVA: 0x000ABDF0 File Offset: 0x000A9FF0
	private void ApplyWearable()
	{
		if (this.animController == null)
		{
			global::Debug.LogWarning("Missing animcontroller for WearableAccessorizer, bailing early to prevent a crash!");
			return;
		}
		SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
		WearableAccessorizer.WearableType highestAccessory = this.GetHighestAccessory();
		foreach (object obj in Enum.GetValues(typeof(WearableAccessorizer.WearableType)))
		{
			WearableAccessorizer.WearableType wearableType = (WearableAccessorizer.WearableType)obj;
			if (this.wearables.ContainsKey(wearableType))
			{
				WearableAccessorizer.Wearable wearable = this.wearables[wearableType];
				int buildOverridePriority = wearable.buildOverridePriority;
				foreach (KAnimFile kanimFile in wearable.BuildAnims)
				{
					KAnim.Build build = kanimFile.GetData().build;
					if (build != null)
					{
						for (int i = 0; i < build.symbols.Length; i++)
						{
							string text = HashCache.Get().Get(build.symbols[i].hash);
							if (wearableType == highestAccessory)
							{
								component.AddSymbolOverride(text, build.symbols[i], buildOverridePriority);
								this.animController.SetSymbolVisiblity(text, true);
							}
							else
							{
								component.RemoveSymbolOverride(text, buildOverridePriority);
							}
						}
					}
				}
			}
		}
		this.UpdateVisibleSymbols(highestAccessory);
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x000ABF70 File Offset: 0x000AA170
	public void UpdateVisibleSymbols(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		this.UpdateVisibleSymbols(this.ConvertOutfitTypeToWearableType(outfitType));
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x000ABF9C File Offset: 0x000AA19C
	private void UpdateVisibleSymbols(WearableAccessorizer.WearableType wearableType)
	{
		bool flag = wearableType == WearableAccessorizer.WearableType.Basic;
		bool hasHat = base.GetComponent<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Hat) != null;
		bool flag2 = false;
		bool is_visible = false;
		bool is_visible2 = true;
		bool is_visible3 = wearableType == WearableAccessorizer.WearableType.Basic;
		bool flag3 = wearableType == WearableAccessorizer.WearableType.Basic;
		if (this.wearables.ContainsKey(wearableType))
		{
			List<KAnimHashedString> list = this.wearables[wearableType].BuildAnims.SelectMany((KAnimFile x) => from s in x.GetData().build.symbols
			select s.hash).ToList<KAnimHashedString>();
			flag = (flag || list.Contains(Db.Get().AccessorySlots.Belt.targetSymbolId));
			flag2 = list.Contains(Db.Get().AccessorySlots.Skirt.targetSymbolId);
			is_visible = list.Contains(Db.Get().AccessorySlots.Necklace.targetSymbolId);
			is_visible2 = list.Contains(Db.Get().AccessorySlots.ArmLower.targetSymbolId);
			is_visible3 = (list.Contains(Db.Get().AccessorySlots.Arm.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops)));
			flag3 = (list.Contains(Db.Get().AccessorySlots.Leg.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeBottoms)));
		}
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Belt.targetSymbolId, flag);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, is_visible);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.ArmLower.targetSymbolId, is_visible2);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Arm.targetSymbolId, is_visible3);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, flag3 && !flag2);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, flag2);
		if (flag2)
		{
			this.SkirtHACK(wearableType);
		}
		WearableAccessorizer.UpdateHairBasedOnHat(this.animController, hasHat);
	}

	// Token: 0x06002016 RID: 8214 RVA: 0x000AC1EC File Offset: 0x000AA3EC
	private void SkirtHACK(WearableAccessorizer.WearableType wearable_type)
	{
		if (this.wearables.ContainsKey(wearable_type))
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			WearableAccessorizer.Wearable wearable = this.wearables[wearable_type];
			int buildOverridePriority = wearable.buildOverridePriority;
			foreach (KAnimFile kanimFile in wearable.BuildAnims)
			{
				foreach (KAnim.Build.Symbol symbol in kanimFile.GetData().build.symbols)
				{
					if (HashCache.Get().Get(symbol.hash).EndsWith(WearableAccessorizer.cropped))
					{
						component.AddSymbolOverride(WearableAccessorizer.torso, symbol, buildOverridePriority);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x000AC2BC File Offset: 0x000AA4BC
	public static void UpdateHairBasedOnHat(KAnimControllerBase kbac, bool hasHat)
	{
		if (hasHat)
		{
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, true);
			return;
		}
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x000AC36F File Offset: 0x000AA56F
	public static void SkirtAccessory(KAnimControllerBase kbac, bool show_skirt)
	{
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, show_skirt);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, !show_skirt);
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x000AC3AC File Offset: 0x000AA5AC
	private void RemoveAnimBuild(KAnimFile animFile, int override_priority)
	{
		SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
		KAnim.Build build = (animFile != null) ? animFile.GetData().build : null;
		if (build != null)
		{
			for (int i = 0; i < build.symbols.Length; i++)
			{
				string s = HashCache.Get().Get(build.symbols[i].hash);
				component.RemoveSymbolOverride(s, override_priority);
			}
		}
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x000AC414 File Offset: 0x000AA614
	private void UnequippedItem(object data)
	{
		KPrefabID kprefabID = data as KPrefabID;
		if (kprefabID != null)
		{
			Equippable component = kprefabID.GetComponent<Equippable>();
			this.RemoveEquipment(component);
		}
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x000AC440 File Offset: 0x000AA640
	public void RemoveEquipment(Equippable equippable)
	{
		WearableAccessorizer.WearableType key;
		if (equippable != null && Enum.TryParse<WearableAccessorizer.WearableType>(equippable.def.Slot, out key))
		{
			ClothingOutfitUtility.OutfitType key2;
			if (this.TryGetEquippableClothingType(equippable.def, out key2) && this.customOutfitItems.ContainsKey(key2) && this.wearables.ContainsKey(WearableAccessorizer.WearableType.CustomSuit))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[key2])
				{
					this.RemoveAnimBuild(resourceRef.Get().AnimFile, this.wearables[WearableAccessorizer.WearableType.CustomSuit].buildOverridePriority);
				}
				this.RemoveAnimBuild(equippable.GetBuildOverride(), this.wearables[WearableAccessorizer.WearableType.CustomSuit].buildOverridePriority);
				this.wearables.Remove(WearableAccessorizer.WearableType.CustomSuit);
			}
			if (this.wearables.ContainsKey(key))
			{
				this.RemoveAnimBuild(equippable.GetBuildOverride(), this.wearables[key].buildOverridePriority);
				this.wearables.Remove(key);
			}
			this.ApplyWearable();
		}
	}

	// Token: 0x0600201C RID: 8220 RVA: 0x000AC574 File Offset: 0x000AA774
	public void AddCustomClothingOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> items)
	{
		if (outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			this.ApplyClothingItems(outfitType, items);
			return;
		}
		if (this.IsWearingSuitType(outfitType))
		{
			this.ApplyClothingItems(outfitType, items);
			Equippable suitEquippable = this.GetSuitEquippable();
			if (suitEquippable != null)
			{
				this.ApplyEquipment(suitEquippable, suitEquippable.GetBuildOverride());
				return;
			}
		}
		else
		{
			if (!this.customOutfitItems.ContainsKey(outfitType))
			{
				this.customOutfitItems.Add(outfitType, new List<ResourceRef<ClothingItemResource>>());
			}
			using (IEnumerator<ClothingItemResource> enumerator = items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClothingItemResource clothingItem = enumerator.Current;
					if (!this.customOutfitItems[outfitType].Exists((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothingItem.IdHash))
					{
						foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[outfitType].FindAll((ResourceRef<ClothingItemResource> x) => x.Get().Category == clothingItem.Category))
						{
							this.RemoveClothingItem(outfitType, resourceRef.Get());
						}
						this.customOutfitItems[outfitType].Add(new ResourceRef<ClothingItemResource>(clothingItem));
					}
				}
			}
		}
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000AC6C0 File Offset: 0x000AA8C0
	public void ApplyClothingItem(ClothingOutfitUtility.OutfitType outfitType, ClothingItemResource clothingItem)
	{
		WearableAccessorizer.WearableType wearableType = this.ConvertOutfitTypeToWearableType(outfitType);
		if (!this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems.Add(outfitType, new List<ResourceRef<ClothingItemResource>>());
		}
		if (!this.customOutfitItems[outfitType].Exists((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothingItem.IdHash))
		{
			if (this.wearables.ContainsKey(wearableType))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[outfitType].FindAll((ResourceRef<ClothingItemResource> x) => x.Get().Category == clothingItem.Category))
				{
					this.RemoveClothingItem(outfitType, resourceRef.Get());
				}
			}
			this.customOutfitItems[outfitType].Add(new ResourceRef<ClothingItemResource>(clothingItem));
		}
		if (!this.wearables.ContainsKey(wearableType))
		{
			int num = (wearableType == WearableAccessorizer.WearableType.CustomClothing) ? 4 : 6;
			if (clothingItem.Category == PermitCategory.DupeBottoms && clothingItem.AnimFile.name.Contains("skirt"))
			{
				num--;
			}
			this.wearables[wearableType] = new WearableAccessorizer.Wearable(new List<KAnimFile>(), num);
		}
		this.wearables[wearableType].AddAnim(clothingItem.AnimFile);
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x000AC82C File Offset: 0x000AAA2C
	public void RemoveClothingItem(ClothingOutfitUtility.OutfitType outfitType, ClothingItemResource clothing_item)
	{
		WearableAccessorizer.WearableType key = this.ConvertOutfitTypeToWearableType(outfitType);
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems[outfitType].RemoveAll((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothing_item.IdHash);
		}
		if (this.wearables.ContainsKey(key))
		{
			if (this.wearables[key].RemoveAnim(clothing_item.AnimFile))
			{
				this.RemoveAnimBuild(clothing_item.AnimFile, this.wearables[key].buildOverridePriority);
			}
			if (this.wearables[key].BuildAnims.Count <= 0)
			{
				this.wearables.Remove(key);
			}
		}
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x000AC8F0 File Offset: 0x000AAAF0
	public void ApplyClothingOutfit(ClothingOutfitResource outfit)
	{
		IEnumerable<ClothingItemResource> items = from itemId in outfit.itemsInOutfit
		select Db.Get().Permits.ClothingItems.Get(itemId);
		this.ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, items);
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x000AC930 File Offset: 0x000AAB30
	public void ClearAllOutfitItems(ClothingOutfitUtility.OutfitType? forOutfitType = null)
	{
		foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> self in this.customOutfitItems)
		{
			ClothingOutfitUtility.OutfitType outfitType;
			List<ResourceRef<ClothingItemResource>> list;
			self.Deconstruct(out outfitType, out list);
			ClothingOutfitUtility.OutfitType outfitType2 = outfitType;
			if (forOutfitType != null)
			{
				ClothingOutfitUtility.OutfitType? outfitType3 = forOutfitType;
				outfitType = outfitType2;
				if (!(outfitType3.GetValueOrDefault() == outfitType & outfitType3 != null))
				{
					continue;
				}
			}
			this.ApplyClothingItems(outfitType2, Enumerable.Empty<ClothingItemResource>());
		}
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x000AC9B4 File Offset: 0x000AABB4
	public void ApplyClothingItems(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> items)
	{
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems[outfitType].Clear();
		}
		WearableAccessorizer.WearableType key = this.ConvertOutfitTypeToWearableType(outfitType);
		if (this.wearables.ContainsKey(key))
		{
			foreach (KAnimFile animFile in this.wearables[key].BuildAnims)
			{
				this.RemoveAnimBuild(animFile, this.wearables[key].buildOverridePriority);
			}
			this.wearables[key].ClearAnims();
			if (items.Count<ClothingItemResource>() <= 0)
			{
				this.wearables.Remove(key);
			}
		}
		foreach (ClothingItemResource clothingItem in items)
		{
			this.ApplyClothingItem(outfitType, clothingItem);
		}
		this.ApplyWearable();
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000ACAC0 File Offset: 0x000AACC0
	private WearableAccessorizer.WearableType ConvertOutfitTypeToWearableType(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			return WearableAccessorizer.WearableType.CustomClothing;
		}
		if (outfitType != ClothingOutfitUtility.OutfitType.AtmoSuit)
		{
			global::Debug.LogWarning("Add a wearable type for clothing outfit type " + outfitType.ToString());
			return WearableAccessorizer.WearableType.Basic;
		}
		return WearableAccessorizer.WearableType.CustomSuit;
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x000ACAEC File Offset: 0x000AACEC
	public void RestoreWearables(Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> stored_wearables, Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> clothing)
	{
		if (stored_wearables != null)
		{
			this.wearables = stored_wearables;
			foreach (KeyValuePair<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> keyValuePair in this.wearables)
			{
				keyValuePair.Value.Deserialize();
			}
		}
		if (clothing != null)
		{
			foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> keyValuePair2 in clothing)
			{
				this.ApplyClothingItems(keyValuePair2.Key, from i in keyValuePair2.Value
				select i.Get());
			}
		}
		this.ApplyWearable();
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x000ACBC8 File Offset: 0x000AADC8
	public void AddCustomOutfit(Option<ClothingOutfitTarget> outfit)
	{
		this.customOutfitItems[outfit.Value.OutfitType] = new List<ResourceRef<ClothingItemResource>>();
		foreach (ClothingItemResource resource in outfit.Value.ReadItemValues())
		{
			this.customOutfitItems[outfit.Value.OutfitType].Add(new ResourceRef<ClothingItemResource>(resource));
		}
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x000ACC5C File Offset: 0x000AAE5C
	public bool HasPermitCategoryItem(ClothingOutfitUtility.OutfitType wearable_type, PermitCategory category)
	{
		bool result = false;
		if (this.customOutfitItems.ContainsKey(wearable_type))
		{
			result = this.customOutfitItems[wearable_type].Exists((ResourceRef<ClothingItemResource> resource) => resource.Get().Category == category);
		}
		return result;
	}

	// Token: 0x040011E7 RID: 4583
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x040011E8 RID: 4584
	[Obsolete("Deprecated, use customOufitItems[ClothingOutfitUtility.OutfitType.Clothing]")]
	[Serialize]
	private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	// Token: 0x040011E9 RID: 4585
	[Serialize]
	private string joyResponsePermitId;

	// Token: 0x040011EA RID: 4586
	[Serialize]
	private Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customOutfitItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();

	// Token: 0x040011EB RID: 4587
	[Serialize]
	private Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearables = new Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable>();

	// Token: 0x040011EC RID: 4588
	private static string torso = "torso";

	// Token: 0x040011ED RID: 4589
	private static string cropped = "_cropped";

	// Token: 0x020011D5 RID: 4565
	public enum WearableType
	{
		// Token: 0x04005DC8 RID: 24008
		Basic,
		// Token: 0x04005DC9 RID: 24009
		CustomClothing,
		// Token: 0x04005DCA RID: 24010
		Outfit,
		// Token: 0x04005DCB RID: 24011
		Suit,
		// Token: 0x04005DCC RID: 24012
		CustomSuit
	}

	// Token: 0x020011D6 RID: 4566
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Wearable
	{
		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06007AF8 RID: 31480 RVA: 0x002DD232 File Offset: 0x002DB432
		public List<KAnimFile> BuildAnims
		{
			get
			{
				return this.buildAnims;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06007AF9 RID: 31481 RVA: 0x002DD23A File Offset: 0x002DB43A
		public List<string> AnimNames
		{
			get
			{
				return this.animNames;
			}
		}

		// Token: 0x06007AFA RID: 31482 RVA: 0x002DD244 File Offset: 0x002DB444
		public Wearable(List<KAnimFile> buildAnims, int buildOverridePriority)
		{
			this.buildAnims = buildAnims;
			this.animNames = (from animFile in buildAnims
			select animFile.name).ToList<string>();
			this.buildOverridePriority = buildOverridePriority;
		}

		// Token: 0x06007AFB RID: 31483 RVA: 0x002DD295 File Offset: 0x002DB495
		public Wearable(KAnimFile buildAnim, int buildOverridePriority)
		{
			this.buildAnims = new List<KAnimFile>
			{
				buildAnim
			};
			this.animNames = new List<string>
			{
				buildAnim.name
			};
			this.buildOverridePriority = buildOverridePriority;
		}

		// Token: 0x06007AFC RID: 31484 RVA: 0x002DD2D0 File Offset: 0x002DB4D0
		public Wearable(List<ResourceRef<ClothingItemResource>> items, int buildOverridePriority)
		{
			this.buildAnims = new List<KAnimFile>();
			this.animNames = new List<string>();
			this.buildOverridePriority = buildOverridePriority;
			foreach (ResourceRef<ClothingItemResource> resourceRef in items)
			{
				ClothingItemResource clothingItemResource = resourceRef.Get();
				this.buildAnims.Add(clothingItemResource.AnimFile);
				this.animNames.Add(clothingItemResource.animFilename);
			}
		}

		// Token: 0x06007AFD RID: 31485 RVA: 0x002DD364 File Offset: 0x002DB564
		public void AddCustomItems(List<ResourceRef<ClothingItemResource>> items)
		{
			foreach (ResourceRef<ClothingItemResource> resourceRef in items)
			{
				ClothingItemResource clothingItemResource = resourceRef.Get();
				this.buildAnims.Add(clothingItemResource.AnimFile);
				this.animNames.Add(clothingItemResource.animFilename);
			}
		}

		// Token: 0x06007AFE RID: 31486 RVA: 0x002DD3D4 File Offset: 0x002DB5D4
		public void Deserialize()
		{
			if (this.animNames != null)
			{
				this.buildAnims = new List<KAnimFile>();
				for (int i = 0; i < this.animNames.Count; i++)
				{
					this.buildAnims.Add(Assets.GetAnim(this.animNames[i]));
				}
			}
		}

		// Token: 0x06007AFF RID: 31487 RVA: 0x002DD42B File Offset: 0x002DB62B
		public void AddAnim(KAnimFile animFile)
		{
			this.buildAnims.Add(animFile);
			this.animNames.Add(animFile.name);
		}

		// Token: 0x06007B00 RID: 31488 RVA: 0x002DD44A File Offset: 0x002DB64A
		public bool RemoveAnim(KAnimFile animFile)
		{
			return this.buildAnims.Remove(animFile) | this.animNames.Remove(animFile.name);
		}

		// Token: 0x06007B01 RID: 31489 RVA: 0x002DD46A File Offset: 0x002DB66A
		public void ClearAnims()
		{
			this.buildAnims.Clear();
			this.animNames.Clear();
		}

		// Token: 0x04005DCD RID: 24013
		private List<KAnimFile> buildAnims;

		// Token: 0x04005DCE RID: 24014
		[Serialize]
		private List<string> animNames;

		// Token: 0x04005DCF RID: 24015
		[Serialize]
		public int buildOverridePriority;
	}
}
