using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class CoolVestConfig : IEquipmentConfig
{
	// Token: 0x0600022A RID: 554 RVA: 0x0000F818 File Offset: 0x0000DA18
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000F820 File Offset: 0x0000DA20
	public EquipmentDef CreateEquipmentDef()
	{
		new Dictionary<string, float>().Add("BasicFabric", (float)TUNING.EQUIPMENT.VESTS.COOL_VEST_MASS);
		ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.COOL_CLOTHING;
		List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Cool_Vest", TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float)TUNING.EQUIPMENT.VESTS.COOL_VEST_MASS, TUNING.EQUIPMENT.VESTS.COOL_VEST_ICON0, TUNING.EQUIPMENT.VESTS.SNAPON0, TUNING.EQUIPMENT.VESTS.COOL_VEST_ANIM0, 4, attributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, null, null);
		Descriptor item = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.COOL_CLOTHING.conductivityMod)), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.COOL_CLOTHING.conductivityMod)), Descriptor.DescriptorType.Effect, false);
		Descriptor item2 = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, ClothingWearer.ClothingInfo.COOL_CLOTHING.decorMod), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, ClothingWearer.ClothingInfo.COOL_CLOTHING.decorMod), Descriptor.DescriptorType.Effect, false);
		equipmentDef.additionalDescriptors.Add(item);
		equipmentDef.additionalDescriptors.Add(item2);
		equipmentDef.OnEquipCallBack = delegate(Equippable eq)
		{
			CoolVestConfig.OnEquipVest(eq, clothingInfo);
		};
		equipmentDef.OnUnequipCallBack = new Action<Equippable>(CoolVestConfig.OnUnequipVest);
		equipmentDef.RecipeDescription = STRINGS.EQUIPMENT.PREFABS.COOL_VEST.RECIPE_DESC;
		return equipmentDef;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000F970 File Offset: 0x0000DB70
	public static void OnEquipVest(Equippable eq, ClothingWearer.ClothingInfo clothingInfo)
	{
		if (eq == null || eq.assignee == null)
		{
			return;
		}
		Ownables soleOwner = eq.assignee.GetSoleOwner();
		if (soleOwner == null)
		{
			return;
		}
		ClothingWearer component = (soleOwner.GetComponent<MinionAssignablesProxy>().target as KMonoBehaviour).GetComponent<ClothingWearer>();
		if (component != null)
		{
			component.ChangeClothes(clothingInfo);
			return;
		}
		global::Debug.LogWarning("Clothing item cannot be equipped to assignee because they lack ClothingWearer component");
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000F9D8 File Offset: 0x0000DBD8
	public static void OnUnequipVest(Equippable eq)
	{
		if (eq != null && eq.assignee != null)
		{
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				ClothingWearer component = soleOwner.GetComponent<ClothingWearer>();
				if (component != null)
				{
					component.ChangeToDefaultClothes();
				}
			}
		}
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000FA24 File Offset: 0x0000DC24
	public static void SetupVest(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
		Equippable equippable = go.GetComponent<Equippable>();
		if (equippable == null)
		{
			equippable = go.AddComponent<Equippable>();
		}
		equippable.SetQuality(global::QualityLevel.Poor);
		go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000FA6D File Offset: 0x0000DC6D
	public void DoPostConfigure(GameObject go)
	{
		CoolVestConfig.SetupVest(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.PedestalDisplayable, false);
	}

	// Token: 0x04000140 RID: 320
	public const string ID = "Cool_Vest";

	// Token: 0x04000141 RID: 321
	public static ComplexRecipe recipe;
}
