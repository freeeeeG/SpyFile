using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class PrickleFruitConfig : IEntityConfig
{
	// Token: 0x0600078B RID: 1931 RVA: 0x0002E50A File Offset: 0x0002C70A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x0002E514 File Offset: 0x0002C714
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(PrickleFruitConfig.ID, ITEMS.FOOD.PRICKLEFRUIT.NAME, ITEMS.FOOD.PRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim("bristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PRICKLEFRUIT);
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0002E578 File Offset: 0x0002C778
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x0002E57A File Offset: 0x0002C77A
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, PrickleFruitConfig.OnEatCompleteDelegate);
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x0002E590 File Offset: 0x0002C790
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
				if (UnityEngine.Random.value < PrickleFruitConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("PrickleFlowerSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0400050E RID: 1294
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x0400050F RID: 1295
	public static string ID = "PrickleFruit";

	// Token: 0x04000510 RID: 1296
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		PrickleFruitConfig.OnEatComplete(component);
	});
}
