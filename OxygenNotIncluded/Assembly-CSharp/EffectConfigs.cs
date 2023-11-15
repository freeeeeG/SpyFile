using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class EffectConfigs : IMultiEntityConfig
{
	// Token: 0x060001E8 RID: 488 RVA: 0x0000D404 File Offset: 0x0000B604
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		var array = new <>f__AnonymousType0<string, string[], string, KAnim.PlayMode, bool>[]
		{
			new
			{
				id = EffectConfigs.EffectTemplateId,
				animFiles = new string[0],
				initialAnim = "",
				initialMode = KAnim.PlayMode.Once,
				destroyOnAnimComplete = false
			},
			new
			{
				id = EffectConfigs.EffectTemplateOverrideId,
				animFiles = new string[0],
				initialAnim = "",
				initialMode = KAnim.PlayMode.Once,
				destroyOnAnimComplete = false
			},
			new
			{
				id = EffectConfigs.AttackSplashId,
				animFiles = new string[]
				{
					"attack_beam_contact_fx_kanim"
				},
				initialAnim = "loop",
				initialMode = KAnim.PlayMode.Loop,
				destroyOnAnimComplete = false
			},
			new
			{
				id = EffectConfigs.OreAbsorbId,
				animFiles = new string[]
				{
					"ore_collision_kanim"
				},
				initialAnim = "idle",
				initialMode = KAnim.PlayMode.Once,
				destroyOnAnimComplete = true
			},
			new
			{
				id = EffectConfigs.PlantDeathId,
				animFiles = new string[]
				{
					"plant_death_fx_kanim"
				},
				initialAnim = "plant_death",
				initialMode = KAnim.PlayMode.Once,
				destroyOnAnimComplete = true
			},
			new
			{
				id = EffectConfigs.BuildSplashId,
				animFiles = new string[]
				{
					"sparks_radial_build_kanim"
				},
				initialAnim = "loop",
				initialMode = KAnim.PlayMode.Loop,
				destroyOnAnimComplete = false
			},
			new
			{
				id = EffectConfigs.DemolishSplashId,
				animFiles = new string[]
				{
					"poi_demolish_impact_kanim"
				},
				initialAnim = "POI_demolish_impact",
				initialMode = KAnim.PlayMode.Loop,
				destroyOnAnimComplete = false
			}
		};
		for (int i = 0; i < array.Length; i++)
		{
			var <>f__AnonymousType = array[i];
			GameObject gameObject = EntityTemplates.CreateEntity(<>f__AnonymousType.id, <>f__AnonymousType.id, false);
			KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
			kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.Simple;
			kbatchedAnimController.initialAnim = <>f__AnonymousType.initialAnim;
			kbatchedAnimController.initialMode = <>f__AnonymousType.initialMode;
			kbatchedAnimController.isMovable = true;
			kbatchedAnimController.destroyOnAnimComplete = <>f__AnonymousType.destroyOnAnimComplete;
			if (<>f__AnonymousType.id == EffectConfigs.EffectTemplateOverrideId)
			{
				SymbolOverrideControllerUtil.AddToPrefab(gameObject);
			}
			if (<>f__AnonymousType.animFiles.Length != 0)
			{
				KAnimFile[] array2 = new KAnimFile[<>f__AnonymousType.animFiles.Length];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = Assets.GetAnim(<>f__AnonymousType.animFiles[j]);
				}
				kbatchedAnimController.AnimFiles = array2;
			}
			gameObject.AddOrGet<LoopingSounds>();
			list.Add(gameObject);
		}
		return list;
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000D5DF File Offset: 0x0000B7DF
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000D5E1 File Offset: 0x0000B7E1
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000129 RID: 297
	public static string EffectTemplateId = "EffectTemplateFx";

	// Token: 0x0400012A RID: 298
	public static string EffectTemplateOverrideId = "EffectTemplateOverrideFx";

	// Token: 0x0400012B RID: 299
	public static string AttackSplashId = "AttackSplashFx";

	// Token: 0x0400012C RID: 300
	public static string OreAbsorbId = "OreAbsorbFx";

	// Token: 0x0400012D RID: 301
	public static string PlantDeathId = "PlantDeathFx";

	// Token: 0x0400012E RID: 302
	public static string BuildSplashId = "BuildSplashFx";

	// Token: 0x0400012F RID: 303
	public static string DemolishSplashId = "DemolishSplashFx";
}
