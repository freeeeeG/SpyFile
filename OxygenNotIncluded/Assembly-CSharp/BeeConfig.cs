using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class BeeConfig : IEntityConfig
{
	// Token: 0x060002DA RID: 730 RVA: 0x000173D0 File Offset: 0x000155D0
	public static GameObject CreateBee(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseBeeConfig.BaseBee(id, name, desc, anim_file, "BeeBaseTrait", DECOR.BONUS.TIER4, is_baby, null);
		Trait trait = Db.Get().CreateTrait("BeeBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 5f, name, false, false, true));
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0001746D File Offset: 0x0001566D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00017474 File Offset: 0x00015674
	public GameObject CreatePrefab()
	{
		return BeeConfig.CreateBee("Bee", STRINGS.CREATURES.SPECIES.BEE.NAME, STRINGS.CREATURES.SPECIES.BEE.DESC, "bee_kanim", false);
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0001749A File Offset: 0x0001569A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0001749C File Offset: 0x0001569C
	public void OnSpawn(GameObject inst)
	{
		BaseBeeConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040001FC RID: 508
	public const string ID = "Bee";

	// Token: 0x040001FD RID: 509
	public const string BASE_TRAIT_ID = "BeeBaseTrait";
}
