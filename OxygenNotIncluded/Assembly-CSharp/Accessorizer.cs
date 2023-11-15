using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x02000479 RID: 1145
[AddComponentMenu("KMonoBehaviour/scripts/Accessorizer")]
public class Accessorizer : KMonoBehaviour
{
	// Token: 0x06001907 RID: 6407 RVA: 0x00082CF6 File Offset: 0x00080EF6
	public List<ResourceRef<Accessory>> GetAccessories()
	{
		return this.accessories;
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x00082CFE File Offset: 0x00080EFE
	public void SetAccessories(List<ResourceRef<Accessory>> data)
	{
		this.accessories = data;
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06001909 RID: 6409 RVA: 0x00082D07 File Offset: 0x00080F07
	// (set) Token: 0x0600190A RID: 6410 RVA: 0x00082D0F File Offset: 0x00080F0F
	public KCompBuilder.BodyData bodyData { get; set; }

	// Token: 0x0600190B RID: 6411 RVA: 0x00082D18 File Offset: 0x00080F18
	[OnDeserialized]
	private void OnDeserialized()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (this.clothingItems.Count > 0 || (component != null && component.nameStringKey == "JORGE") || SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
		{
			if (component != null)
			{
				this.bodyData = Accessorizer.UpdateAccessorySlots(component.nameStringKey, ref this.accessories);
			}
			this.accessories.RemoveAll((ResourceRef<Accessory> x) => x.Get() == null);
		}
		if (this.clothingItems.Count > 0)
		{
			base.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, from i in this.clothingItems
			select i.Get());
			this.clothingItems.Clear();
		}
		this.ApplyAccessories();
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x00082E14 File Offset: 0x00081014
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null)
		{
			this.bodyData = MinionStartingStats.CreateBodyData(Db.Get().Personalities.Get(component.personalityResourceId));
		}
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x00082E58 File Offset: 0x00081058
	public void AddAccessory(Accessory accessory)
	{
		if (accessory != null)
		{
			if (this.animController == null)
			{
				this.animController = base.GetComponent<KAnimControllerBase>();
			}
			this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol, accessory.slot.overrideLayer);
			if (!this.HasAccessory(accessory))
			{
				ResourceRef<Accessory> resourceRef = new ResourceRef<Accessory>(accessory);
				if (resourceRef != null)
				{
					this.accessories.Add(resourceRef);
				}
			}
		}
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x00082ED4 File Offset: 0x000810D4
	public void RemoveAccessory(Accessory accessory)
	{
		this.accessories.RemoveAll((ResourceRef<Accessory> x) => x.Get() == accessory);
		this.animController.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride(accessory.slot.targetSymbolId, accessory.slot.overrideLayer);
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x00082F3C File Offset: 0x0008113C
	public void ApplyAccessories()
	{
		foreach (ResourceRef<Accessory> resourceRef in this.accessories)
		{
			Accessory accessory = resourceRef.Get();
			if (accessory != null)
			{
				this.AddAccessory(accessory);
			}
		}
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x00082F98 File Offset: 0x00081198
	public static KCompBuilder.BodyData UpdateAccessorySlots(string nameString, ref List<ResourceRef<Accessory>> accessories)
	{
		accessories.RemoveAll((ResourceRef<Accessory> acc) => acc.Get() == null);
		Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(nameString);
		if (personalityFromNameStringKey != null)
		{
			KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personalityFromNameStringKey);
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				if (accessorySlot.accessories.Count != 0)
				{
					Accessory accessory = null;
					if (accessorySlot == Db.Get().AccessorySlots.Body)
					{
						accessory = accessorySlot.Lookup(bodyData.body);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Arm)
					{
						accessory = accessorySlot.Lookup(bodyData.arms);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmLower)
					{
						accessory = accessorySlot.Lookup(bodyData.armslower);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmLowerSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.armLowerSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmUpperSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.armUpperSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.LegSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.legSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Leg)
					{
						accessory = accessorySlot.Lookup(bodyData.legs);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Belt)
					{
						accessory = accessorySlot.Lookup(bodyData.belt);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Neck)
					{
						accessory = accessorySlot.Lookup(bodyData.neck);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Pelvis)
					{
						accessory = accessorySlot.Lookup(bodyData.pelvis);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Foot)
					{
						accessory = accessorySlot.Lookup(bodyData.foot);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Cuff)
					{
						accessory = accessorySlot.Lookup(bodyData.cuff);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Hand)
					{
						accessory = accessorySlot.Lookup(bodyData.hand);
					}
					if (accessory != null)
					{
						ResourceRef<Accessory> item = new ResourceRef<Accessory>(accessory);
						accessories.RemoveAll((ResourceRef<Accessory> old_acc) => old_acc.Get().slot == accessory.slot);
						accessories.Add(item);
					}
				}
			}
			return bodyData;
		}
		return default(KCompBuilder.BodyData);
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x000832B8 File Offset: 0x000814B8
	public bool HasAccessory(Accessory accessory)
	{
		return this.accessories.Exists((ResourceRef<Accessory> x) => x.Get() == accessory);
	}

	// Token: 0x06001912 RID: 6418 RVA: 0x000832EC File Offset: 0x000814EC
	public Accessory GetAccessory(AccessorySlot slot)
	{
		for (int i = 0; i < this.accessories.Count; i++)
		{
			if (this.accessories[i].Get() != null && this.accessories[i].Get().slot == slot)
			{
				return this.accessories[i].Get();
			}
		}
		return null;
	}

	// Token: 0x06001913 RID: 6419 RVA: 0x00083350 File Offset: 0x00081550
	public void ApplyMinionPersonality(Personality personality)
	{
		this.bodyData = MinionStartingStats.CreateBodyData(personality);
		this.accessories.Clear();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		foreach (string text in new string[]
		{
			"snapTo_hat",
			"snapTo_hat_hair",
			"snapTo_goggles",
			"snapTo_headFX",
			"snapTo_neck",
			"snapTo_chest",
			"snapTo_pivot",
			"skirt",
			"necklace"
		})
		{
			this.animController.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(text, 0);
			this.animController.SetSymbolVisiblity(text, false);
		}
		this.AddAccessory(Db.Get().AccessorySlots.Eyes.Lookup(this.bodyData.eyes));
		this.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(this.bodyData.hair));
		this.AddAccessory(Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(this.bodyData.hair)));
		this.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(this.bodyData.headShape));
		this.AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(this.bodyData.mouth));
		this.AddAccessory(Db.Get().AccessorySlots.Body.Lookup(this.bodyData.body));
		this.AddAccessory(Db.Get().AccessorySlots.Arm.Lookup(this.bodyData.arms));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLower.Lookup(this.bodyData.armslower));
		this.AddAccessory(Db.Get().AccessorySlots.Neck.Lookup(this.bodyData.neck));
		this.AddAccessory(Db.Get().AccessorySlots.Pelvis.Lookup(this.bodyData.pelvis));
		this.AddAccessory(Db.Get().AccessorySlots.Leg.Lookup(this.bodyData.legs));
		this.AddAccessory(Db.Get().AccessorySlots.Foot.Lookup(this.bodyData.foot));
		this.AddAccessory(Db.Get().AccessorySlots.Hand.Lookup(this.bodyData.hand));
		this.AddAccessory(Db.Get().AccessorySlots.Cuff.Lookup(this.bodyData.cuff));
		this.AddAccessory(Db.Get().AccessorySlots.Belt.Lookup(this.bodyData.belt));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLowerSkin.Lookup(this.bodyData.armLowerSkin));
		this.AddAccessory(Db.Get().AccessorySlots.ArmUpperSkin.Lookup(this.bodyData.armUpperSkin));
		this.AddAccessory(Db.Get().AccessorySlots.LegSkin.Lookup(this.bodyData.legSkin));
		this.UpdateHairBasedOnHat();
	}

	// Token: 0x06001914 RID: 6420 RVA: 0x000836D0 File Offset: 0x000818D0
	public void UpdateHairBasedOnHat()
	{
		if (!this.GetAccessory(Db.Get().AccessorySlots.Hat).IsNullOrDestroyed())
		{
			this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
			return;
		}
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
	}

	// Token: 0x06001915 RID: 6421 RVA: 0x000837A0 File Offset: 0x000819A0
	public void GetBodySlots(ref KCompBuilder.BodyData fd)
	{
		fd.eyes = HashedString.Invalid;
		fd.hair = HashedString.Invalid;
		fd.headShape = HashedString.Invalid;
		fd.mouth = HashedString.Invalid;
		fd.neck = HashedString.Invalid;
		fd.body = HashedString.Invalid;
		fd.arms = HashedString.Invalid;
		fd.armslower = HashedString.Invalid;
		fd.hat = HashedString.Invalid;
		fd.faceFX = HashedString.Invalid;
		fd.armLowerSkin = HashedString.Invalid;
		fd.armUpperSkin = HashedString.Invalid;
		fd.legSkin = HashedString.Invalid;
		fd.belt = HashedString.Invalid;
		fd.pelvis = HashedString.Invalid;
		fd.foot = HashedString.Invalid;
		fd.skirt = HashedString.Invalid;
		fd.necklace = HashedString.Invalid;
		fd.cuff = HashedString.Invalid;
		fd.hand = HashedString.Invalid;
		for (int i = 0; i < this.accessories.Count; i++)
		{
			Accessory accessory = this.accessories[i].Get();
			if (accessory != null)
			{
				if (accessory.slot.Id == "Eyes")
				{
					fd.eyes = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hair")
				{
					fd.hair = accessory.IdHash;
				}
				else if (accessory.slot.Id == "HeadShape")
				{
					fd.headShape = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Mouth")
				{
					fd.mouth = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Neck")
				{
					fd.neck = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Torso")
				{
					fd.body = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Arm_Sleeve")
				{
					fd.arms = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Arm_Lower_Sleeve")
				{
					fd.armslower = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hat")
				{
					fd.hat = HashedString.Invalid;
				}
				else if (accessory.slot.Id == "FaceEffect")
				{
					fd.faceFX = HashedString.Invalid;
				}
				else if (accessory.slot.Id == "Arm_Lower")
				{
					fd.armLowerSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Arm_Upper")
				{
					fd.armUpperSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Leg_Skin")
				{
					fd.legSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Leg")
				{
					fd.legs = accessory.Id;
				}
				else if (accessory.slot.Id == "Belt")
				{
					fd.belt = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Pelvis")
				{
					fd.pelvis = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Foot")
				{
					fd.foot = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Cuff")
				{
					fd.cuff = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Skirt")
				{
					fd.skirt = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hand")
				{
					fd.hand = accessory.IdHash;
				}
			}
		}
	}

	// Token: 0x04000DD3 RID: 3539
	[Serialize]
	private List<ResourceRef<Accessory>> accessories = new List<ResourceRef<Accessory>>();

	// Token: 0x04000DD4 RID: 3540
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04000DD5 RID: 3541
	[Serialize]
	private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();
}
