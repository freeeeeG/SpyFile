﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000D89 RID: 3465
	public class TRAITS
	{
		// Token: 0x06006BFF RID: 27647 RVA: 0x002A79A8 File Offset: 0x002A5BA8
		private static void OnAddStressVomiter(GameObject go)
		{
			Notification notification = new Notification(DUPLICANTS.STATUSITEMS.STRESSVOMITING.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.STRESSVOMITING.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
			StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalVomiter", DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_VOMITER.NAME, DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_VOMITER.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			Func<StatusItem> <>9__3;
			new StressBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(), delegate(ChoreProvider chore_provider)
			{
				ChoreType stressEmote = Db.Get().ChoreTypes.StressEmote;
				HashedString emote_kanim = "anim_interrupt_vomiter_kanim";
				HashedString[] emote_anims = new HashedString[]
				{
					"interrupt_vomiter"
				};
				KAnim.PlayMode play_mode = KAnim.PlayMode.Once;
				Func<StatusItem> get_status_item;
				if ((get_status_item = <>9__3) == null)
				{
					get_status_item = (<>9__3 = (() => tierOneBehaviourStatusItem));
				}
				return new StressEmoteChore(chore_provider, stressEmote, emote_kanim, emote_anims, play_mode, get_status_item);
			}, (ChoreProvider chore_provider) => new VomitChore(Db.Get().ChoreTypes.StressVomit, chore_provider, Db.Get().DuplicantStatusItems.Vomiting, notification, null), "anim_loco_vomiter_kanim", 3f).StartSM();
		}

		// Token: 0x06006C00 RID: 27648 RVA: 0x002A7A68 File Offset: 0x002A5C68
		private static void OnAddBanshee(GameObject go)
		{
			Notification notification = new Notification(DUPLICANTS.MODIFIERS.BANSHEE_WAILING.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.MODIFIERS.BANSHEE_WAILING.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
			StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalBanshee", DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_BANSHEE.NAME, DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_BANSHEE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			Func<StatusItem> <>9__3;
			new StressBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(), delegate(ChoreProvider chore_provider)
			{
				ChoreType stressEmote = Db.Get().ChoreTypes.StressEmote;
				HashedString emote_kanim = "anim_interrupt_banshee_kanim";
				HashedString[] emote_anims = new HashedString[]
				{
					"interrupt_banshee"
				};
				KAnim.PlayMode play_mode = KAnim.PlayMode.Once;
				Func<StatusItem> get_status_item;
				if ((get_status_item = <>9__3) == null)
				{
					get_status_item = (<>9__3 = (() => tierOneBehaviourStatusItem));
				}
				return new StressEmoteChore(chore_provider, stressEmote, emote_kanim, emote_anims, play_mode, get_status_item);
			}, (ChoreProvider chore_provider) => new BansheeChore(Db.Get().ChoreTypes.BansheeWail, chore_provider, notification, null), "anim_loco_banshee_60_kanim", 3f).StartSM();
		}

		// Token: 0x06006C01 RID: 27649 RVA: 0x002A7B28 File Offset: 0x002A5D28
		private static void OnAddAggressive(GameObject go)
		{
			StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalAggresive", DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_AGGRESIVE.NAME, DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_AGGRESIVE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			Func<StatusItem> <>9__2;
			new StressBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(), delegate(ChoreProvider chore_provider)
			{
				ChoreType stressEmote = Db.Get().ChoreTypes.StressEmote;
				HashedString emote_kanim = "anim_interrupt_destructive_kanim";
				HashedString[] emote_anims = new HashedString[]
				{
					"interrupt_destruct"
				};
				KAnim.PlayMode play_mode = KAnim.PlayMode.Once;
				Func<StatusItem> get_status_item;
				if ((get_status_item = <>9__2) == null)
				{
					get_status_item = (<>9__2 = (() => tierOneBehaviourStatusItem));
				}
				return new StressEmoteChore(chore_provider, stressEmote, emote_kanim, emote_anims, play_mode, get_status_item);
			}, (ChoreProvider chore_provider) => new AggressiveChore(chore_provider, null), "anim_loco_destructive_kanim", 3f).StartSM();
		}

		// Token: 0x06006C02 RID: 27650 RVA: 0x002A7BB8 File Offset: 0x002A5DB8
		private static void OnAddUglyCrier(GameObject go)
		{
			StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalUglyCrier", DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_UGLY_CRIER.NAME, DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_UGLY_CRIER.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			Func<StatusItem> <>9__2;
			new StressBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(), delegate(ChoreProvider chore_provider)
			{
				ChoreType stressEmote = Db.Get().ChoreTypes.StressEmote;
				HashedString emote_kanim = "anim_cry_kanim";
				HashedString[] emote_anims = new HashedString[]
				{
					"working_pre",
					"working_loop",
					"working_pst"
				};
				KAnim.PlayMode play_mode = KAnim.PlayMode.Once;
				Func<StatusItem> get_status_item;
				if ((get_status_item = <>9__2) == null)
				{
					get_status_item = (<>9__2 = (() => tierOneBehaviourStatusItem));
				}
				return new StressEmoteChore(chore_provider, stressEmote, emote_kanim, emote_anims, play_mode, get_status_item);
			}, (ChoreProvider chore_provider) => new UglyCryChore(Db.Get().ChoreTypes.UglyCry, chore_provider, null), "anim_loco_cry_kanim", 3f).StartSM();
		}

		// Token: 0x06006C03 RID: 27651 RVA: 0x002A7C48 File Offset: 0x002A5E48
		private static void OnAddBingeEater(GameObject go)
		{
			StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalBingeEater", DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_BINGE_EAT.NAME, DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_BINGE_EAT.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			Func<StatusItem> <>9__2;
			new StressBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(), delegate(ChoreProvider chore_provider)
			{
				ChoreType stressEmote = Db.Get().ChoreTypes.StressEmote;
				HashedString emote_kanim = "anim_interrupt_binge_eat_kanim";
				HashedString[] emote_anims = new HashedString[]
				{
					"interrupt_binge_eat"
				};
				KAnim.PlayMode play_mode = KAnim.PlayMode.Once;
				Func<StatusItem> get_status_item;
				if ((get_status_item = <>9__2) == null)
				{
					get_status_item = (<>9__2 = (() => tierOneBehaviourStatusItem));
				}
				return new StressEmoteChore(chore_provider, stressEmote, emote_kanim, emote_anims, play_mode, get_status_item);
			}, (ChoreProvider chore_provider) => new BingeEatChore(chore_provider, null), "anim_loco_binge_eat_kanim", 8f).StartSM();
		}

		// Token: 0x06006C04 RID: 27652 RVA: 0x002A7CD8 File Offset: 0x002A5ED8
		private static void OnAddBalloonArtist(GameObject go)
		{
			new BalloonArtist.Instance(go.GetComponent<KMonoBehaviour>()).StartSM();
			new JoyBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(), "anim_loco_happy_balloon_kanim", null, Db.Get().Expressions.Balloon).StartSM();
		}

		// Token: 0x06006C05 RID: 27653 RVA: 0x002A7D0F File Offset: 0x002A5F0F
		private static void OnAddSparkleStreaker(GameObject go)
		{
			new SparkleStreaker.Instance(go.GetComponent<KMonoBehaviour>()).StartSM();
			new JoyBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(), "anim_loco_sparkle_kanim", null, Db.Get().Expressions.Sparkle).StartSM();
		}

		// Token: 0x06006C06 RID: 27654 RVA: 0x002A7D46 File Offset: 0x002A5F46
		private static void OnAddStickerBomber(GameObject go)
		{
			new StickerBomber.Instance(go.GetComponent<KMonoBehaviour>()).StartSM();
			new JoyBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(), "anim_loco_stickers_kanim", null, Db.Get().Expressions.Sticker).StartSM();
		}

		// Token: 0x06006C07 RID: 27655 RVA: 0x002A7D7D File Offset: 0x002A5F7D
		private static void OnAddSuperProductive(GameObject go)
		{
			new SuperProductive.Instance(go.GetComponent<KMonoBehaviour>()).StartSM();
			new JoyBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(), "anim_loco_productive_kanim", "anim_loco_walk_productive_kanim", Db.Get().Expressions.Productive).StartSM();
		}

		// Token: 0x06006C08 RID: 27656 RVA: 0x002A7DB8 File Offset: 0x002A5FB8
		private static void OnAddHappySinger(GameObject go)
		{
			new HappySinger.Instance(go.GetComponent<KMonoBehaviour>()).StartSM();
			new JoyBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(), "anim_loco_singer_kanim", null, Db.Get().Expressions.Music).StartSM();
		}

		// Token: 0x04004F3E RID: 20286
		public static float EARLYBIRD_MODIFIER = 2f;

		// Token: 0x04004F3F RID: 20287
		public static int EARLYBIRD_SCHEDULEBLOCK = 5;

		// Token: 0x04004F40 RID: 20288
		public static float NIGHTOWL_MODIFIER = 3f;

		// Token: 0x04004F41 RID: 20289
		public static float METEORPHILE_MODIFIER = 3f;

		// Token: 0x04004F42 RID: 20290
		public const float FLATULENCE_EMIT_MASS = 0.1f;

		// Token: 0x04004F43 RID: 20291
		public static float FLATULENCE_EMIT_INTERVAL_MIN = 10f;

		// Token: 0x04004F44 RID: 20292
		public static float FLATULENCE_EMIT_INTERVAL_MAX = 40f;

		// Token: 0x04004F45 RID: 20293
		public static float STINKY_EMIT_INTERVAL_MIN = 10f;

		// Token: 0x04004F46 RID: 20294
		public static float STINKY_EMIT_INTERVAL_MAX = 30f;

		// Token: 0x04004F47 RID: 20295
		public static float NARCOLEPSY_INTERVAL_MIN = 300f;

		// Token: 0x04004F48 RID: 20296
		public static float NARCOLEPSY_INTERVAL_MAX = 600f;

		// Token: 0x04004F49 RID: 20297
		public static float NARCOLEPSY_SLEEPDURATION_MIN = 15f;

		// Token: 0x04004F4A RID: 20298
		public static float NARCOLEPSY_SLEEPDURATION_MAX = 30f;

		// Token: 0x04004F4B RID: 20299
		public const float INTERRUPTED_SLEEP_STRESS_DELTA = 10f;

		// Token: 0x04004F4C RID: 20300
		public const float INTERRUPTED_SLEEP_ATHLETICS_DELTA = -2f;

		// Token: 0x04004F4D RID: 20301
		public static int NO_ATTRIBUTE_BONUS = 0;

		// Token: 0x04004F4E RID: 20302
		public static int GOOD_ATTRIBUTE_BONUS = 3;

		// Token: 0x04004F4F RID: 20303
		public static int GREAT_ATTRIBUTE_BONUS = 5;

		// Token: 0x04004F50 RID: 20304
		public static int BAD_ATTRIBUTE_PENALTY = -3;

		// Token: 0x04004F51 RID: 20305
		public static int HORRIBLE_ATTRIBUTE_PENALTY = -5;

		// Token: 0x04004F52 RID: 20306
		public static float GLOWSTICK_RADIATION_RESISTANCE = 0.33f;

		// Token: 0x04004F53 RID: 20307
		public static float RADIATION_EATER_RECOVERY = -0.25f;

		// Token: 0x04004F54 RID: 20308
		public static float RADS_TO_CALS = 333.33f;

		// Token: 0x04004F55 RID: 20309
		public static readonly List<System.Action> TRAIT_CREATORS = new List<System.Action>
		{
			TraitUtil.CreateAttributeEffectTrait("None", DUPLICANTS.CONGENITALTRAITS.NONE.NAME, DUPLICANTS.CONGENITALTRAITS.NONE.DESC, "", (float)TRAITS.NO_ATTRIBUTE_BONUS, false, null, true),
			TraitUtil.CreateComponentTrait<Stinky>("Stinky", DUPLICANTS.CONGENITALTRAITS.STINKY.NAME, DUPLICANTS.CONGENITALTRAITS.STINKY.DESC, false, null),
			TraitUtil.CreateAttributeEffectTrait("Ellie", DUPLICANTS.CONGENITALTRAITS.ELLIE.NAME, DUPLICANTS.CONGENITALTRAITS.ELLIE.DESC, "AirConsumptionRate", -0.044999998f, "DecorExpectation", -5f, false),
			TraitUtil.CreateDisabledTaskTrait("Joshua", DUPLICANTS.CONGENITALTRAITS.JOSHUA.NAME, DUPLICANTS.CONGENITALTRAITS.JOSHUA.DESC, "Combat", true),
			TraitUtil.CreateComponentTrait<Stinky>("Liam", DUPLICANTS.CONGENITALTRAITS.LIAM.NAME, DUPLICANTS.CONGENITALTRAITS.LIAM.DESC, false, null),
			TraitUtil.CreateNamedTrait("AncientKnowledge", DUPLICANTS.TRAITS.ANCIENTKNOWLEDGE.NAME, DUPLICANTS.TRAITS.ANCIENTKNOWLEDGE.DESC, true),
			TraitUtil.CreateComponentTrait<Chatty>("Chatty", DUPLICANTS.TRAITS.CHATTY.NAME, DUPLICANTS.TRAITS.CHATTY.DESC, true, null),
			TraitUtil.CreateDisabledTaskTrait("CantResearch", DUPLICANTS.TRAITS.CANTRESEARCH.NAME, DUPLICANTS.TRAITS.CANTRESEARCH.DESC, "Research", true),
			TraitUtil.CreateDisabledTaskTrait("CantBuild", DUPLICANTS.TRAITS.CANTBUILD.NAME, DUPLICANTS.TRAITS.CANTBUILD.DESC, "Build", false),
			TraitUtil.CreateDisabledTaskTrait("CantCook", DUPLICANTS.TRAITS.CANTCOOK.NAME, DUPLICANTS.TRAITS.CANTCOOK.DESC, "Cook", true),
			TraitUtil.CreateDisabledTaskTrait("CantDig", DUPLICANTS.TRAITS.CANTDIG.NAME, DUPLICANTS.TRAITS.CANTDIG.DESC, "Dig", false),
			TraitUtil.CreateDisabledTaskTrait("Hemophobia", DUPLICANTS.TRAITS.HEMOPHOBIA.NAME, DUPLICANTS.TRAITS.HEMOPHOBIA.DESC, "MedicalAid", true),
			TraitUtil.CreateDisabledTaskTrait("ScaredyCat", DUPLICANTS.TRAITS.SCAREDYCAT.NAME, DUPLICANTS.TRAITS.SCAREDYCAT.DESC, "Combat", true),
			TraitUtil.CreateNamedTrait("Allergies", DUPLICANTS.TRAITS.ALLERGIES.NAME, DUPLICANTS.TRAITS.ALLERGIES.DESC, false),
			TraitUtil.CreateNamedTrait("NightLight", DUPLICANTS.TRAITS.NIGHTLIGHT.NAME, DUPLICANTS.TRAITS.NIGHTLIGHT.DESC, false),
			TraitUtil.CreateAttributeEffectTrait("MouthBreather", DUPLICANTS.TRAITS.MOUTHBREATHER.NAME, DUPLICANTS.TRAITS.MOUTHBREATHER.DESC, "AirConsumptionRate", 0.1f, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("CalorieBurner", DUPLICANTS.TRAITS.CALORIEBURNER.NAME, DUPLICANTS.TRAITS.CALORIEBURNER.DESC, "CaloriesDelta", -833.3333f, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("SmallBladder", DUPLICANTS.TRAITS.SMALLBLADDER.NAME, DUPLICANTS.TRAITS.SMALLBLADDER.DESC, "BladderDelta", 0.00027777778f, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("Anemic", DUPLICANTS.TRAITS.ANEMIC.NAME, DUPLICANTS.TRAITS.ANEMIC.DESC, "Athletics", (float)TRAITS.HORRIBLE_ATTRIBUTE_PENALTY, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("SlowLearner", DUPLICANTS.TRAITS.SLOWLEARNER.NAME, DUPLICANTS.TRAITS.SLOWLEARNER.DESC, "Learning", (float)TRAITS.BAD_ATTRIBUTE_PENALTY, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("NoodleArms", DUPLICANTS.TRAITS.NOODLEARMS.NAME, DUPLICANTS.TRAITS.NOODLEARMS.DESC, "Strength", (float)TRAITS.BAD_ATTRIBUTE_PENALTY, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("ConstructionDown", DUPLICANTS.TRAITS.CONSTRUCTIONDOWN.NAME, DUPLICANTS.TRAITS.CONSTRUCTIONDOWN.DESC, "Construction", (float)TRAITS.BAD_ATTRIBUTE_PENALTY, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("RanchingDown", DUPLICANTS.TRAITS.RANCHINGDOWN.NAME, DUPLICANTS.TRAITS.RANCHINGDOWN.DESC, "Ranching", (float)TRAITS.BAD_ATTRIBUTE_PENALTY, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("DiggingDown", DUPLICANTS.TRAITS.DIGGINGDOWN.NAME, DUPLICANTS.TRAITS.DIGGINGDOWN.DESC, "Digging", (float)TRAITS.BAD_ATTRIBUTE_PENALTY, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("MachineryDown", DUPLICANTS.TRAITS.MACHINERYDOWN.NAME, DUPLICANTS.TRAITS.MACHINERYDOWN.DESC, "Machinery", (float)TRAITS.BAD_ATTRIBUTE_PENALTY, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("CookingDown", DUPLICANTS.TRAITS.COOKINGDOWN.NAME, DUPLICANTS.TRAITS.COOKINGDOWN.DESC, "Cooking", (float)TRAITS.BAD_ATTRIBUTE_PENALTY, "FoodExpectation", 1f, false),
			TraitUtil.CreateAttributeEffectTrait("ArtDown", DUPLICANTS.TRAITS.ARTDOWN.NAME, DUPLICANTS.TRAITS.ARTDOWN.DESC, "Art", (float)TRAITS.BAD_ATTRIBUTE_PENALTY, "DecorExpectation", 5f, false),
			TraitUtil.CreateAttributeEffectTrait("CaringDown", DUPLICANTS.TRAITS.CARINGDOWN.NAME, DUPLICANTS.TRAITS.CARINGDOWN.DESC, "Caring", (float)TRAITS.BAD_ATTRIBUTE_PENALTY, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("BotanistDown", DUPLICANTS.TRAITS.BOTANISTDOWN.NAME, DUPLICANTS.TRAITS.BOTANISTDOWN.DESC, "Botanist", (float)TRAITS.BAD_ATTRIBUTE_PENALTY, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("DecorDown", DUPLICANTS.TRAITS.DECORDOWN.NAME, DUPLICANTS.TRAITS.DECORDOWN.DESC, "Decor", (float)BUILDINGS.DECOR.PENALTY.TIER2.amount, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("Regeneration", DUPLICANTS.TRAITS.REGENERATION.NAME, DUPLICANTS.TRAITS.REGENERATION.DESC, "HitPointsDelta", 0.033333335f, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("DeeperDiversLungs", DUPLICANTS.TRAITS.DEEPERDIVERSLUNGS.NAME, DUPLICANTS.TRAITS.DEEPERDIVERSLUNGS.DESC, "AirConsumptionRate", -0.05f, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("SunnyDisposition", DUPLICANTS.TRAITS.SUNNYDISPOSITION.NAME, DUPLICANTS.TRAITS.SUNNYDISPOSITION.DESC, "StressDelta", -0.033333335f, false, delegate(GameObject go)
			{
				go.GetComponent<KBatchedAnimController>().AddAnimOverrides(Assets.GetAnim("anim_loco_happy_kanim"), 0f);
			}, true),
			TraitUtil.CreateAttributeEffectTrait("RockCrusher", DUPLICANTS.TRAITS.ROCKCRUSHER.NAME, DUPLICANTS.TRAITS.ROCKCRUSHER.DESC, "Strength", 10f, false, null, true),
			TraitUtil.CreateTrait("Uncultured", DUPLICANTS.TRAITS.UNCULTURED.NAME, DUPLICANTS.TRAITS.UNCULTURED.DESC, "DecorExpectation", 20f, new string[]
			{
				"Art"
			}, true),
			TraitUtil.CreateNamedTrait("Archaeologist", DUPLICANTS.TRAITS.ARCHAEOLOGIST.NAME, DUPLICANTS.TRAITS.ARCHAEOLOGIST.DESC, false),
			TraitUtil.CreateAttributeEffectTrait("WeakImmuneSystem", DUPLICANTS.TRAITS.WEAKIMMUNESYSTEM.NAME, DUPLICANTS.TRAITS.WEAKIMMUNESYSTEM.DESC, "GermResistance", -1f, false, null, true),
			TraitUtil.CreateAttributeEffectTrait("IrritableBowel", DUPLICANTS.TRAITS.IRRITABLEBOWEL.NAME, DUPLICANTS.TRAITS.IRRITABLEBOWEL.DESC, "ToiletEfficiency", -0.5f, false, null, true),
			TraitUtil.CreateComponentTrait<Flatulence>("Flatulence", DUPLICANTS.TRAITS.FLATULENCE.NAME, DUPLICANTS.TRAITS.FLATULENCE.DESC, false, null),
			TraitUtil.CreateComponentTrait<Snorer>("Snorer", DUPLICANTS.TRAITS.SNORER.NAME, DUPLICANTS.TRAITS.SNORER.DESC, false, null),
			TraitUtil.CreateComponentTrait<Narcolepsy>("Narcolepsy", DUPLICANTS.TRAITS.NARCOLEPSY.NAME, DUPLICANTS.TRAITS.NARCOLEPSY.DESC, false, null),
			TraitUtil.CreateComponentTrait<Thriver>("Thriver", DUPLICANTS.TRAITS.THRIVER.NAME, DUPLICANTS.TRAITS.THRIVER.DESC, true, null),
			TraitUtil.CreateComponentTrait<Loner>("Loner", DUPLICANTS.TRAITS.LONER.NAME, DUPLICANTS.TRAITS.LONER.DESC, true, null),
			TraitUtil.CreateComponentTrait<StarryEyed>("StarryEyed", DUPLICANTS.TRAITS.STARRYEYED.NAME, DUPLICANTS.TRAITS.STARRYEYED.DESC, true, null),
			TraitUtil.CreateComponentTrait<GlowStick>("GlowStick", DUPLICANTS.TRAITS.GLOWSTICK.NAME, DUPLICANTS.TRAITS.GLOWSTICK.DESC, true, null),
			TraitUtil.CreateComponentTrait<RadiationEater>("RadiationEater", DUPLICANTS.TRAITS.RADIATIONEATER.NAME, DUPLICANTS.TRAITS.RADIATIONEATER.DESC, true, null),
			TraitUtil.CreateAttributeEffectTrait("Twinkletoes", DUPLICANTS.TRAITS.TWINKLETOES.NAME, DUPLICANTS.TRAITS.TWINKLETOES.DESC, "Athletics", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("Greasemonkey", DUPLICANTS.TRAITS.GREASEMONKEY.NAME, DUPLICANTS.TRAITS.GREASEMONKEY.DESC, "Machinery", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("MoleHands", DUPLICANTS.TRAITS.MOLEHANDS.NAME, DUPLICANTS.TRAITS.MOLEHANDS.DESC, "Digging", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("FastLearner", DUPLICANTS.TRAITS.FASTLEARNER.NAME, DUPLICANTS.TRAITS.FASTLEARNER.DESC, "Learning", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("DiversLung", DUPLICANTS.TRAITS.DIVERSLUNG.NAME, DUPLICANTS.TRAITS.DIVERSLUNG.DESC, "AirConsumptionRate", -0.025f, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("StrongArm", DUPLICANTS.TRAITS.STRONGARM.NAME, DUPLICANTS.TRAITS.STRONGARM.DESC, "Strength", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("DecorUp", DUPLICANTS.TRAITS.DECORUP.NAME, DUPLICANTS.TRAITS.DECORUP.DESC, "Decor", (float)BUILDINGS.DECOR.BONUS.TIER4.amount, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("GreenThumb", DUPLICANTS.TRAITS.GREENTHUMB.NAME, DUPLICANTS.TRAITS.GREENTHUMB.DESC, "Botanist", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("InteriorDecorator", DUPLICANTS.TRAITS.INTERIORDECORATOR.NAME, DUPLICANTS.TRAITS.INTERIORDECORATOR.DESC, "Art", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, "DecorExpectation", -5f, true),
			TraitUtil.CreateAttributeEffectTrait("SimpleTastes", DUPLICANTS.TRAITS.SIMPLETASTES.NAME, DUPLICANTS.TRAITS.SIMPLETASTES.DESC, "FoodExpectation", 1f, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("Foodie", DUPLICANTS.TRAITS.FOODIE.NAME, DUPLICANTS.TRAITS.FOODIE.DESC, "Cooking", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, "FoodExpectation", -1f, true),
			TraitUtil.CreateAttributeEffectTrait("BedsideManner", DUPLICANTS.TRAITS.BEDSIDEMANNER.NAME, DUPLICANTS.TRAITS.BEDSIDEMANNER.DESC, "Caring", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("ConstructionUp", DUPLICANTS.TRAITS.CONSTRUCTIONUP.NAME, DUPLICANTS.TRAITS.CONSTRUCTIONUP.DESC, "Construction", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, true, null, true),
			TraitUtil.CreateAttributeEffectTrait("RanchingUp", DUPLICANTS.TRAITS.RANCHINGUP.NAME, DUPLICANTS.TRAITS.RANCHINGUP.DESC, "Ranching", (float)TRAITS.GOOD_ATTRIBUTE_BONUS, true, null, true),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Mining1", DUPLICANTS.TRAITS.GRANTSKILL_MINING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_MINING1.DESC, "Mining1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Mining2", DUPLICANTS.TRAITS.GRANTSKILL_MINING2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_MINING2.DESC, "Mining2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Mining3", DUPLICANTS.TRAITS.GRANTSKILL_MINING3.NAME, DUPLICANTS.TRAITS.GRANTSKILL_MINING3.DESC, "Mining3"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Mining4", DUPLICANTS.TRAITS.GRANTSKILL_MINING4.NAME, DUPLICANTS.TRAITS.GRANTSKILL_MINING4.DESC, "Mining4"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Building1", DUPLICANTS.TRAITS.GRANTSKILL_BUILDING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_BUILDING1.DESC, "Building1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Building2", DUPLICANTS.TRAITS.GRANTSKILL_BUILDING2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_BUILDING2.DESC, "Building2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Building3", DUPLICANTS.TRAITS.GRANTSKILL_BUILDING3.NAME, DUPLICANTS.TRAITS.GRANTSKILL_BUILDING3.DESC, "Building3"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Farming1", DUPLICANTS.TRAITS.GRANTSKILL_FARMING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_FARMING1.DESC, "Farming1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Farming2", DUPLICANTS.TRAITS.GRANTSKILL_FARMING2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_FARMING2.DESC, "Farming2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Farming3", DUPLICANTS.TRAITS.GRANTSKILL_FARMING3.NAME, DUPLICANTS.TRAITS.GRANTSKILL_FARMING3.DESC, "Farming3"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Ranching1", DUPLICANTS.TRAITS.GRANTSKILL_RANCHING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_RANCHING1.DESC, "Ranching1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Ranching2", DUPLICANTS.TRAITS.GRANTSKILL_RANCHING2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_RANCHING2.DESC, "Ranching2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Researching1", DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING1.DESC, "Researching1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Researching2", DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING2.DESC, "Researching2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Researching3", DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING3.NAME, DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING3.DESC, "Researching3"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Researching4", DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING4.NAME, DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING4.DESC, "Researching4"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Cooking1", DUPLICANTS.TRAITS.GRANTSKILL_COOKING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_COOKING1.DESC, "Cooking1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Cooking2", DUPLICANTS.TRAITS.GRANTSKILL_COOKING2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_COOKING2.DESC, "Cooking2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Arting1", DUPLICANTS.TRAITS.GRANTSKILL_ARTING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_ARTING1.DESC, "Arting1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Arting2", DUPLICANTS.TRAITS.GRANTSKILL_ARTING2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_ARTING2.DESC, "Arting2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Arting3", DUPLICANTS.TRAITS.GRANTSKILL_ARTING3.NAME, DUPLICANTS.TRAITS.GRANTSKILL_ARTING3.DESC, "Arting3"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Hauling1", DUPLICANTS.TRAITS.GRANTSKILL_HAULING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_HAULING1.DESC, "Hauling1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Hauling2", DUPLICANTS.TRAITS.GRANTSKILL_HAULING2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_HAULING2.DESC, "Hauling2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Suits1", DUPLICANTS.TRAITS.GRANTSKILL_SUITS1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_SUITS1.DESC, "Suits1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Technicals1", DUPLICANTS.TRAITS.GRANTSKILL_TECHNICALS1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_TECHNICALS1.DESC, "Technicals1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Technicals2", DUPLICANTS.TRAITS.GRANTSKILL_TECHNICALS2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_TECHNICALS2.DESC, "Technicals2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Engineering1", DUPLICANTS.TRAITS.GRANTSKILL_ENGINEERING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_ENGINEERING1.DESC, "Engineering1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Basekeeping1", DUPLICANTS.TRAITS.GRANTSKILL_BASEKEEPING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_BASEKEEPING1.DESC, "Basekeeping1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Basekeeping2", DUPLICANTS.TRAITS.GRANTSKILL_BASEKEEPING2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_BASEKEEPING2.DESC, "Basekeeping2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Astronauting1", DUPLICANTS.TRAITS.GRANTSKILL_ASTRONAUTING1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_ASTRONAUTING1.DESC, "Astronauting1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Astronauting2", DUPLICANTS.TRAITS.GRANTSKILL_ASTRONAUTING2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_ASTRONAUTING2.DESC, "Astronauting2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Medicine1", DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE1.NAME, DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE1.DESC, "Medicine1"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Medicine2", DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE2.NAME, DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE2.DESC, "Medicine2"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Medicine3", DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE3.NAME, DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE3.DESC, "Medicine3"),
			TraitUtil.CreateSkillGrantingTrait("GrantSkill_Pyrotechnics", DUPLICANTS.TRAITS.GRANTSKILL_PYROTECHNICS.NAME, DUPLICANTS.TRAITS.GRANTSKILL_PYROTECHNICS.DESC, "Pyrotechnics"),
			TraitUtil.CreateNamedTrait("IronGut", DUPLICANTS.TRAITS.IRONGUT.NAME, DUPLICANTS.TRAITS.IRONGUT.DESC, true),
			TraitUtil.CreateAttributeEffectTrait("StrongImmuneSystem", DUPLICANTS.TRAITS.STRONGIMMUNESYSTEM.NAME, DUPLICANTS.TRAITS.STRONGIMMUNESYSTEM.DESC, "GermResistance", 1f, true, null, true),
			TraitUtil.CreateTrait("Aggressive", DUPLICANTS.TRAITS.AGGRESSIVE.NAME, DUPLICANTS.TRAITS.AGGRESSIVE.DESC, new Action<GameObject>(TRAITS.OnAddAggressive), null, false, () => DUPLICANTS.TRAITS.AGGRESSIVE.NOREPAIR),
			TraitUtil.CreateTrait("UglyCrier", DUPLICANTS.TRAITS.UGLYCRIER.NAME, DUPLICANTS.TRAITS.UGLYCRIER.DESC, new Action<GameObject>(TRAITS.OnAddUglyCrier), null, false, null),
			TraitUtil.CreateTrait("BingeEater", DUPLICANTS.TRAITS.BINGEEATER.NAME, DUPLICANTS.TRAITS.BINGEEATER.DESC, new Action<GameObject>(TRAITS.OnAddBingeEater), null, false, null),
			TraitUtil.CreateTrait("StressVomiter", DUPLICANTS.TRAITS.STRESSVOMITER.NAME, DUPLICANTS.TRAITS.STRESSVOMITER.DESC, new Action<GameObject>(TRAITS.OnAddStressVomiter), null, false, null),
			TraitUtil.CreateTrait("Banshee", DUPLICANTS.TRAITS.BANSHEE.NAME, DUPLICANTS.TRAITS.BANSHEE.DESC, new Action<GameObject>(TRAITS.OnAddBanshee), null, false, null),
			TraitUtil.CreateTrait("BalloonArtist", DUPLICANTS.TRAITS.BALLOONARTIST.NAME, DUPLICANTS.TRAITS.BALLOONARTIST.DESC, new Action<GameObject>(TRAITS.OnAddBalloonArtist), null, false, null),
			TraitUtil.CreateTrait("SparkleStreaker", DUPLICANTS.TRAITS.SPARKLESTREAKER.NAME, DUPLICANTS.TRAITS.SPARKLESTREAKER.DESC, new Action<GameObject>(TRAITS.OnAddSparkleStreaker), null, false, null),
			TraitUtil.CreateTrait("StickerBomber", DUPLICANTS.TRAITS.STICKERBOMBER.NAME, DUPLICANTS.TRAITS.STICKERBOMBER.DESC, new Action<GameObject>(TRAITS.OnAddStickerBomber), null, false, null),
			TraitUtil.CreateTrait("SuperProductive", DUPLICANTS.TRAITS.SUPERPRODUCTIVE.NAME, DUPLICANTS.TRAITS.SUPERPRODUCTIVE.DESC, new Action<GameObject>(TRAITS.OnAddSuperProductive), null, false, null),
			TraitUtil.CreateTrait("HappySinger", DUPLICANTS.TRAITS.HAPPYSINGER.NAME, DUPLICANTS.TRAITS.HAPPYSINGER.DESC, new Action<GameObject>(TRAITS.OnAddHappySinger), null, false, null),
			TraitUtil.CreateComponentTrait<EarlyBird>("EarlyBird", DUPLICANTS.TRAITS.EARLYBIRD.NAME, DUPLICANTS.TRAITS.EARLYBIRD.DESC, true, () => string.Format(DUPLICANTS.TRAITS.EARLYBIRD.EXTENDED_DESC, GameUtil.AddPositiveSign(TRAITS.EARLYBIRD_MODIFIER.ToString(), true))),
			TraitUtil.CreateComponentTrait<NightOwl>("NightOwl", DUPLICANTS.TRAITS.NIGHTOWL.NAME, DUPLICANTS.TRAITS.NIGHTOWL.DESC, true, () => string.Format(DUPLICANTS.TRAITS.NIGHTOWL.EXTENDED_DESC, GameUtil.AddPositiveSign(TRAITS.NIGHTOWL_MODIFIER.ToString(), true))),
			TraitUtil.CreateComponentTrait<Meteorphile>("Meteorphile", DUPLICANTS.TRAITS.METEORPHILE.NAME, DUPLICANTS.TRAITS.METEORPHILE.DESC, true, () => string.Format(DUPLICANTS.TRAITS.METEORPHILE.EXTENDED_DESC, GameUtil.AddPositiveSign(TRAITS.METEORPHILE_MODIFIER.ToString(), true))),
			TraitUtil.CreateComponentTrait<Claustrophobic>("Claustrophobic", DUPLICANTS.TRAITS.NEEDS.CLAUSTROPHOBIC.NAME, DUPLICANTS.TRAITS.NEEDS.CLAUSTROPHOBIC.DESC, false, null),
			TraitUtil.CreateComponentTrait<PrefersWarmer>("PrefersWarmer", DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.DESC, false, null),
			TraitUtil.CreateComponentTrait<PrefersColder>("PrefersColder", DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.DESC, false, null),
			TraitUtil.CreateComponentTrait<SensitiveFeet>("SensitiveFeet", DUPLICANTS.TRAITS.NEEDS.SENSITIVEFEET.NAME, DUPLICANTS.TRAITS.NEEDS.SENSITIVEFEET.DESC, false, null),
			TraitUtil.CreateComponentTrait<Fashionable>("Fashionable", DUPLICANTS.TRAITS.NEEDS.FASHIONABLE.NAME, DUPLICANTS.TRAITS.NEEDS.FASHIONABLE.DESC, false, null),
			TraitUtil.CreateComponentTrait<Climacophobic>("Climacophobic", DUPLICANTS.TRAITS.NEEDS.CLIMACOPHOBIC.NAME, DUPLICANTS.TRAITS.NEEDS.CLIMACOPHOBIC.DESC, false, null),
			TraitUtil.CreateComponentTrait<SolitarySleeper>("SolitarySleeper", DUPLICANTS.TRAITS.NEEDS.SOLITARYSLEEPER.NAME, DUPLICANTS.TRAITS.NEEDS.SOLITARYSLEEPER.DESC, false, null),
			TraitUtil.CreateComponentTrait<Workaholic>("Workaholic", DUPLICANTS.TRAITS.NEEDS.WORKAHOLIC.NAME, DUPLICANTS.TRAITS.NEEDS.WORKAHOLIC.DESC, false, null)
		};

		// Token: 0x02001C79 RID: 7289
		public class JOY_REACTIONS
		{
			// Token: 0x0400813A RID: 33082
			public static float MIN_MORALE_EXCESS = 8f;

			// Token: 0x0400813B RID: 33083
			public static float MAX_MORALE_EXCESS = 20f;

			// Token: 0x0400813C RID: 33084
			public static float MIN_REACTION_CHANCE = 2f;

			// Token: 0x0400813D RID: 33085
			public static float MAX_REACTION_CHANCE = 5f;

			// Token: 0x0400813E RID: 33086
			public static float JOY_REACTION_DURATION = 570f;

			// Token: 0x0400813F RID: 33087
			public const float CHARISMATIC_CHANCE = 1f;

			// Token: 0x02002248 RID: 8776
			public class SUPER_PRODUCTIVE
			{
				// Token: 0x04009942 RID: 39234
				public static float INSTANT_SUCCESS_CHANCE = 10f;
			}

			// Token: 0x02002249 RID: 8777
			public class BALLOON_ARTIST
			{
				// Token: 0x04009943 RID: 39235
				public static float MINIMUM_BALLOON_MOVESPEED = 5f;

				// Token: 0x04009944 RID: 39236
				public static int NUM_BALLOONS_TO_GIVE = 4;
			}

			// Token: 0x0200224A RID: 8778
			public class STICKER_BOMBER
			{
				// Token: 0x04009945 RID: 39237
				public static float TIME_PER_STICKER_BOMB = 150f;

				// Token: 0x04009946 RID: 39238
				public static float STICKER_DURATION = 12000f;
			}
		}
	}
}
