using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200079C RID: 1948
[AddComponentMenu("KMonoBehaviour/scripts/EquipmentConfigManager")]
public class EquipmentConfigManager : KMonoBehaviour
{
	// Token: 0x0600362C RID: 13868 RVA: 0x00124E33 File Offset: 0x00123033
	public static void DestroyInstance()
	{
		EquipmentConfigManager.Instance = null;
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x00124E3B File Offset: 0x0012303B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		EquipmentConfigManager.Instance = this;
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x00124E4C File Offset: 0x0012304C
	public void RegisterEquipment(IEquipmentConfig config)
	{
		if (!DlcManager.IsDlcListValidForCurrentContent(config.GetDlcIds()))
		{
			return;
		}
		EquipmentDef equipmentDef = config.CreateEquipmentDef();
		GameObject gameObject = EntityTemplates.CreateLooseEntity(equipmentDef.Id, equipmentDef.Name, equipmentDef.RecipeDescription, equipmentDef.Mass, true, equipmentDef.Anim, "object", Grid.SceneLayer.Ore, equipmentDef.CollisionShape, equipmentDef.width, equipmentDef.height, true, 0, equipmentDef.OutputElement, null);
		Equippable equippable = gameObject.AddComponent<Equippable>();
		equippable.def = equipmentDef;
		global::Debug.Assert(equippable.def != null);
		equippable.slotID = equipmentDef.Slot;
		global::Debug.Assert(equippable.slot != null);
		config.DoPostConfigure(gameObject);
		Assets.AddPrefab(gameObject.GetComponent<KPrefabID>());
		if (equipmentDef.wornID != null)
		{
			GameObject gameObject2 = EntityTemplates.CreateLooseEntity(equipmentDef.wornID, equipmentDef.WornName, equipmentDef.WornDesc, equipmentDef.Mass, true, equipmentDef.Anim, "worn_out", Grid.SceneLayer.Ore, equipmentDef.CollisionShape, equipmentDef.width, equipmentDef.height, true, 0, SimHashes.Creature, null);
			RepairableEquipment repairableEquipment = gameObject2.AddComponent<RepairableEquipment>();
			repairableEquipment.def = equipmentDef;
			global::Debug.Assert(repairableEquipment.def != null);
			SymbolOverrideControllerUtil.AddToPrefab(gameObject2);
			foreach (Tag tag in equipmentDef.AdditionalTags)
			{
				gameObject2.GetComponent<KPrefabID>().AddTag(tag, false);
			}
			Assets.AddPrefab(gameObject2.GetComponent<KPrefabID>());
		}
	}

	// Token: 0x0600362F RID: 13871 RVA: 0x00124FB0 File Offset: 0x001231B0
	private void LoadRecipe(EquipmentDef def, Equippable equippable)
	{
		Recipe recipe = new Recipe(def.Id, 1f, (SimHashes)0, null, def.RecipeDescription, 0);
		recipe.SetFabricator(def.FabricatorId, def.FabricationTime);
		recipe.TechUnlock = def.RecipeTechUnlock;
		foreach (KeyValuePair<string, float> keyValuePair in def.InputElementMassMap)
		{
			recipe.AddIngredient(new Recipe.Ingredient(keyValuePair.Key, keyValuePair.Value));
		}
	}

	// Token: 0x04002104 RID: 8452
	public static EquipmentConfigManager Instance;
}
