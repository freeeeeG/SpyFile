using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class MushroomConfig : IEntityConfig
{
	// Token: 0x0600076B RID: 1899 RVA: 0x0002E10C File Offset: 0x0002C30C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x0002E114 File Offset: 0x0002C314
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(MushroomConfig.ID, ITEMS.FOOD.MUSHROOM.NAME, ITEMS.FOOD.MUSHROOM.DESC, 1f, false, Assets.GetAnim("funguscap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.77f, 0.48f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHROOM);
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x0002E178 File Offset: 0x0002C378
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x0002E17A File Offset: 0x0002C37A
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, MushroomConfig.OnEatCompleteDelegate);
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0002E190 File Offset: 0x0002C390
	private static void OnEatComplete(Edible edible)
	{
		if (edible != null)
		{
			int num = 0;
			float unitsConsumed = edible.unitsConsumed;
			int num2 = Mathf.FloorToInt(unitsConsumed);
			float num3 = unitsConsumed % 1f;
			if (UnityEngine.Random.value < num3)
			{
				num2++;
			}
			for (int i = 0; i < num2; i++)
			{
				if (UnityEngine.Random.value < MushroomConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("MushroomSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04000503 RID: 1283
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x04000504 RID: 1284
	public static string ID = "Mushroom";

	// Token: 0x04000505 RID: 1285
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		MushroomConfig.OnEatComplete(component);
	});
}
