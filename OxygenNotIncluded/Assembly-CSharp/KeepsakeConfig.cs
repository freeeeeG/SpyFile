using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000283 RID: 643
public class KeepsakeConfig : IMultiEntityConfig
{
	// Token: 0x06000D04 RID: 3332 RVA: 0x00048A64 File Offset: 0x00046C64
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(KeepsakeConfig.CreateKeepsake("MegaBrain", UI.KEEPSAKES.MEGA_BRAIN.NAME, UI.KEEPSAKES.MEGA_BRAIN.DESCRIPTION, "keepsake_mega_brain_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("CritterManipulator", UI.KEEPSAKES.CRITTER_MANIPULATOR.NAME, UI.KEEPSAKES.CRITTER_MANIPULATOR.DESCRIPTION, "keepsake_critter_manipulator_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("LonelyMinion", UI.KEEPSAKES.LONELY_MINION.NAME, UI.KEEPSAKES.LONELY_MINION.DESCRIPTION, "keepsake_lonelyminion_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("FossilHunt", UI.KEEPSAKES.FOSSIL_HUNT.NAME, UI.KEEPSAKES.FOSSIL_HUNT.DESCRIPTION, "keepsake_fossil_dig_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS, null, SimHashes.Creature));
		list.RemoveAll((GameObject x) => x == null);
		return list;
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x00048B94 File Offset: 0x00046D94
	public static GameObject CreateKeepsake(string id, string name, string desc, string animFile, string initial_anim = "idle", string ui_anim = "ui", string[] dlcIDs = null, KeepsakeConfig.PostInitFn postInitFn = null, SimHashes element = SimHashes.Creature)
	{
		if (dlcIDs == null)
		{
			dlcIDs = DlcManager.AVAILABLE_ALL_VERSIONS;
		}
		if (!DlcManager.IsDlcListValidForCurrentContent(dlcIDs))
		{
			return null;
		}
		GameObject gameObject = EntityTemplates.CreateLooseEntity("keepsake_" + id.ToLower(), name, desc, 25f, true, Assets.GetAnim(animFile), initial_anim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, SORTORDER.KEEPSAKES, element, new List<Tag>
		{
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.BONUS.TIER1);
		decorProvider.overrideName = gameObject.name;
		gameObject.AddOrGet<KSelectable>();
		gameObject.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;
		if (postInitFn != null)
		{
			postInitFn(gameObject);
		}
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.PedestalDisplayable, false);
		component.AddTag(GameTags.Keepsake, false);
		return gameObject;
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x00048C72 File Offset: 0x00046E72
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x00048C74 File Offset: 0x00046E74
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x02000F6F RID: 3951
	// (Invoke) Token: 0x06007217 RID: 29207
	public delegate void PostInitFn(GameObject gameObject);
}
