using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000288 RID: 648
public class LonelyMinionConfig : IEntityConfig
{
	// Token: 0x06000D23 RID: 3363 RVA: 0x00048E1A File Offset: 0x0004701A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D24 RID: 3364 RVA: 0x00048E24 File Offset: 0x00047024
	public GameObject CreatePrefab()
	{
		string name = DUPLICANTS.MODIFIERS.BASEDUPLICANT.NAME;
		GameObject gameObject = EntityTemplates.CreateEntity(LonelyMinionConfig.ID, name, true);
		gameObject.AddComponent<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		gameObject.AddComponent<Storage>().doDiseaseTransfer = false;
		gameObject.AddComponent<StateMachineController>();
		LonelyMinion.Def def = gameObject.AddOrGetDef<LonelyMinion.Def>();
		def.Personality = Db.Get().Personalities.Get("JORGE");
		def.Personality.Disabled = true;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.defaultAnim = "idle_default";
		kbatchedAnimController.initialAnim = "idle_default";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_interacts_lonely_dupe_kanim")
		};
		this.ConfigurePackageOverride(gameObject);
		SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		symbolOverrideController.applySymbolOverridesEveryFrame = true;
		symbolOverrideController.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol(string.Format("cheek_00{0}", def.Personality.headShape)), 1);
		MinionConfig.ConfigureSymbols(gameObject, true);
		return gameObject;
	}

	// Token: 0x06000D25 RID: 3365 RVA: 0x00048F65 File Offset: 0x00047165
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x00048F67 File Offset: 0x00047167
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x00048F6C File Offset: 0x0004716C
	private void ConfigurePackageOverride(GameObject go)
	{
		GameObject gameObject = new GameObject("PackageSnapPoint");
		gameObject.transform.SetParent(go.transform);
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.transform.position = Vector3.forward * -0.1f;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mushbar_kanim")
		};
		kbatchedAnimController.initialAnim = "object";
		component.SetSymbolVisiblity(LonelyMinionConfig.PARCEL_SNAPTO, false);
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kbatchedAnimTracker.controller = component;
		kbatchedAnimTracker.symbol = LonelyMinionConfig.PARCEL_SNAPTO;
	}

	// Token: 0x04000797 RID: 1943
	public static string ID = "LonelyMinion";

	// Token: 0x04000798 RID: 1944
	public const int VOICE_IDX = -2;

	// Token: 0x04000799 RID: 1945
	public const int STARTING_SKILL_POINTS = 3;

	// Token: 0x0400079A RID: 1946
	public const int BASE_ATTRIBUTE_LEVEL = 7;

	// Token: 0x0400079B RID: 1947
	public const int AGE_MIN = 2190;

	// Token: 0x0400079C RID: 1948
	public const int AGE_MAX = 3102;

	// Token: 0x0400079D RID: 1949
	public const float MIN_IDLE_DELAY = 20f;

	// Token: 0x0400079E RID: 1950
	public const float MAX_IDLE_DELAY = 40f;

	// Token: 0x0400079F RID: 1951
	public const string IDLE_PREFIX = "idle_blinds";

	// Token: 0x040007A0 RID: 1952
	public static readonly HashedString GreetingCriteraId = "Neighbor";

	// Token: 0x040007A1 RID: 1953
	public static readonly HashedString FoodCriteriaId = "FoodQuality";

	// Token: 0x040007A2 RID: 1954
	public static readonly HashedString DecorCriteriaId = "Decor";

	// Token: 0x040007A3 RID: 1955
	public static readonly HashedString PowerCriteriaId = "SuppliedPower";

	// Token: 0x040007A4 RID: 1956
	public static readonly HashedString CHECK_MAIL = "mail_pre";

	// Token: 0x040007A5 RID: 1957
	public static readonly HashedString CHECK_MAIL_SUCCESS = "mail_success_pst";

	// Token: 0x040007A6 RID: 1958
	public static readonly HashedString CHECK_MAIL_FAILURE = "mail_failure_pst";

	// Token: 0x040007A7 RID: 1959
	public static readonly HashedString CHECK_MAIL_DUPLICATE = "mail_duplicate_pst";

	// Token: 0x040007A8 RID: 1960
	public static readonly HashedString FOOD_SUCCESS = "food_like_loop";

	// Token: 0x040007A9 RID: 1961
	public static readonly HashedString FOOD_FAILURE = "food_dislike_loop";

	// Token: 0x040007AA RID: 1962
	public static readonly HashedString FOOD_DUPLICATE = "food_duplicate_loop";

	// Token: 0x040007AB RID: 1963
	public static readonly HashedString FOOD_IDLE = "idle_food_quest";

	// Token: 0x040007AC RID: 1964
	public static readonly HashedString DECOR_IDLE = "idle_decor_quest";

	// Token: 0x040007AD RID: 1965
	public static readonly HashedString POWER_IDLE = "idle_power_quest";

	// Token: 0x040007AE RID: 1966
	public static readonly HashedString BLINDS_IDLE_0 = "idle_blinds_0";

	// Token: 0x040007AF RID: 1967
	public static readonly HashedString PARCEL_SNAPTO = "parcel_snapTo";

	// Token: 0x040007B0 RID: 1968
	public const string PERSONALITY_ID = "JORGE";

	// Token: 0x040007B1 RID: 1969
	public const string BODY_ANIM_FILE = "body_lonelyminion_kanim";
}
