﻿using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;

namespace Database
{
	// Token: 0x02000D14 RID: 3348
	public class PlantMutations : ResourceSet<PlantMutation>
	{
		// Token: 0x060069E6 RID: 27110 RVA: 0x00291540 File Offset: 0x0028F740
		public PlantMutation AddPlantMutation(string id)
		{
			StringEntry entry = Strings.Get(new StringKey("STRINGS.CREATURES.PLANT_MUTATIONS." + id.ToUpper() + ".NAME"));
			StringEntry entry2 = Strings.Get(new StringKey("STRINGS.CREATURES.PLANT_MUTATIONS." + id.ToUpper() + ".DESCRIPTION"));
			PlantMutation plantMutation = new PlantMutation(id, entry, entry2);
			base.Add(plantMutation);
			return plantMutation;
		}

		// Token: 0x060069E7 RID: 27111 RVA: 0x002915AC File Offset: 0x0028F7AC
		public PlantMutations(ResourceSet parent) : base("PlantMutations", parent)
		{
			this.moderatelyLoose = this.AddPlantMutation("moderatelyLoose").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, 0.5f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, -0.25f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, -0.5f, true).VisualTint(-0.4f, -0.4f, -0.4f);
			this.moderatelyTight = this.AddPlantMutation("moderatelyTight").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, -0.5f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 0.5f, true).VisualTint(0.2f, 0.2f, 0.2f);
			this.extremelyTight = this.AddPlantMutation("extremelyTight").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, -0.8f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 1f, true).VisualTint(0.3f, 0.3f, 0.3f).VisualBGFX("mutate_glow_fx_kanim");
			this.bonusLice = this.AddPlantMutation("bonusLice").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).BonusCrop("BasicPlantFood", 1f).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "meal_lice_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "meal_lice_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_MealLice", false));
			this.sunnySpeed = this.AddPlantMutation("sunnySpeed").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.MinLightLux, 1000f, false).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, -0.5f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "leaf_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "leaf_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_Leaf", false));
			this.slowBurn = this.AddPlantMutation("slowBurn").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, -0.9f, true).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, 3.5f, true).VisualTint(-0.3f, -0.3f, -0.5f);
			this.blooms = this.AddPlantMutation("blooms").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().BuildingAttributes.Decor, 20f, false).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "blossom_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "blossom_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_PrickleFlower", false));
			this.loadedWithFruit = this.AddPlantMutation("loadedWithFruit").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 1f, true).AttributeModifier(Db.Get().PlantAttributes.HarvestTime, 4f, true).AttributeModifier(Db.Get().PlantAttributes.MinLightLux, 200f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.2f, true).VisualSymbolScale("swap_crop01", 1.3f).VisualSymbolScale("swap_crop02", 1.3f);
			this.rottenHeaps = this.AddPlantMutation("rottenHeaps").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, -0.75f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.5f, true).BonusCrop(RotPileConfig.ID, 4f).AddDiseaseToHarvest(Db.Get().Diseases.GetIndex(Db.Get().Diseases.FoodGerms.Id), 10000).ForcePrefersDarkness().VisualFGFX("mutate_stink_fx_kanim").VisualSymbolTint("swap_crop01", -0.2f, -0.1f, -0.5f).VisualSymbolTint("swap_crop02", -0.2f, -0.1f, -0.5f);
			this.heavyFruit = this.AddPlantMutation("heavyFruit").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).ForceSelfHarvestOnGrown().VisualSymbolTint("swap_crop01", -0.1f, -0.5f, -0.5f).VisualSymbolTint("swap_crop02", -0.1f, -0.5f, -0.5f);
		}

		// Token: 0x060069E8 RID: 27112 RVA: 0x00291B98 File Offset: 0x0028FD98
		public List<string> GetNamesForMutations(List<string> mutationIDs)
		{
			List<string> list = new List<string>(mutationIDs.Count);
			foreach (string id in mutationIDs)
			{
				list.Add(base.Get(id).Name);
			}
			return list;
		}

		// Token: 0x060069E9 RID: 27113 RVA: 0x00291C00 File Offset: 0x0028FE00
		public PlantMutation GetRandomMutation(string targetPlantPrefabID)
		{
			return (from m in this.resources
			where !m.originalMutation && !m.restrictedPrefabIDs.Contains(targetPlantPrefabID) && (m.requiredPrefabIDs.Count == 0 || m.requiredPrefabIDs.Contains(targetPlantPrefabID))
			select m).ToList<PlantMutation>().GetRandom<PlantMutation>();
		}

		// Token: 0x04004CA1 RID: 19617
		public PlantMutation moderatelyLoose;

		// Token: 0x04004CA2 RID: 19618
		public PlantMutation moderatelyTight;

		// Token: 0x04004CA3 RID: 19619
		public PlantMutation extremelyTight;

		// Token: 0x04004CA4 RID: 19620
		public PlantMutation bonusLice;

		// Token: 0x04004CA5 RID: 19621
		public PlantMutation sunnySpeed;

		// Token: 0x04004CA6 RID: 19622
		public PlantMutation slowBurn;

		// Token: 0x04004CA7 RID: 19623
		public PlantMutation blooms;

		// Token: 0x04004CA8 RID: 19624
		public PlantMutation loadedWithFruit;

		// Token: 0x04004CA9 RID: 19625
		public PlantMutation heavyFruit;

		// Token: 0x04004CAA RID: 19626
		public PlantMutation rottenHeaps;
	}
}
