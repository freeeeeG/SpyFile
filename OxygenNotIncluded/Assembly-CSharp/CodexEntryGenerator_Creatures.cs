using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000ABF RID: 2751
public class CodexEntryGenerator_Creatures
{
	// Token: 0x060054AE RID: 21678 RVA: 0x001EBCCC File Offset: 0x001E9ECC
	public static Dictionary<string, CodexEntry> GenerateEntries()
	{
		CodexEntryGenerator_Creatures.<>c__DisplayClass6_0 CS$<>8__locals1;
		CS$<>8__locals1.results = new Dictionary<string, CodexEntry>();
		CS$<>8__locals1.brains = Assets.GetPrefabsWithComponent<CreatureBrain>();
		CS$<>8__locals1.critterEntries = new List<ValueTuple<string, CodexEntry>>();
		CodexEntryGenerator_Creatures.<GenerateEntries>g__AddEntry|6_0("CREATURES::GUIDE", CodexEntryGenerator_Creatures.GenerateFieldGuideEntry(), "CREATURES", ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.PuftSpecies, CREATURES.FAMILY_PLURAL.PUFTSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.PacuSpecies, CREATURES.FAMILY_PLURAL.PACUSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.OilFloaterSpecies, CREATURES.FAMILY_PLURAL.OILFLOATERSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.LightBugSpecies, CREATURES.FAMILY_PLURAL.LIGHTBUGSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.HatchSpecies, CREATURES.FAMILY_PLURAL.HATCHSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.GlomSpecies, CREATURES.FAMILY_PLURAL.GLOMSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.DreckoSpecies, CREATURES.FAMILY_PLURAL.DRECKOSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.MooSpecies, CREATURES.FAMILY_PLURAL.MOOSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.MoleSpecies, CREATURES.FAMILY_PLURAL.MOLESPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.SquirrelSpecies, CREATURES.FAMILY_PLURAL.SQUIRRELSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.CrabSpecies, CREATURES.FAMILY_PLURAL.CRABSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Robots.Models.ScoutRover, CREATURES.FAMILY_PLURAL.SCOUTROVER, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.StaterpillarSpecies, CREATURES.FAMILY_PLURAL.STATERPILLARSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.BeetaSpecies, CREATURES.FAMILY_PLURAL.BEETASPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Creatures.Species.DivergentSpecies, CREATURES.FAMILY_PLURAL.DIVERGENTSPECIES, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PushCritterEntry|6_1(GameTags.Robots.Models.SweepBot, CREATURES.FAMILY_PLURAL.SWEEPBOT, ref CS$<>8__locals1);
		CodexEntryGenerator_Creatures.<GenerateEntries>g__PopAndAddAllCritterEntries|6_2(ref CS$<>8__locals1);
		return CS$<>8__locals1.results;
	}

	// Token: 0x060054AF RID: 21679 RVA: 0x001EBE80 File Offset: 0x001EA080
	private static CodexEntry GenerateFieldGuideEntry()
	{
		CodexEntryGenerator_Creatures.<>c__DisplayClass7_0 CS$<>8__locals1;
		CS$<>8__locals1.generalInfoEntry = new CodexEntry("CREATURES", new List<ContentContainer>(), CODEX.CRITTERSTATUS.CRITTERSTATUS_TITLE);
		CS$<>8__locals1.generalInfoEntry.icon = Assets.GetSprite("codex_critter_emotions");
		CodexEntryGenerator_Creatures.<>c__DisplayClass7_1 CS$<>8__locals2;
		CS$<>8__locals2.subEntryContents = null;
		CS$<>8__locals2.subEntry = null;
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubEntry|7_0("CREATURES::GUIDE::METABOLISM", CODEX.CRITTERSTATUS.METABOLISM.TITLE, ref CS$<>8__locals1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddImage|7_1(Assets.GetSprite("codex_metabolism"), ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.METABOLISM.BODY.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.METABOLISM.HUNGRY.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.METABOLISM.HUNGRY.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.METABOLISM.STARVING.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(string.Format(DlcManager.IsExpansion1Active() ? CODEX.CRITTERSTATUS.METABOLISM.STARVING.CONTAINER1_DLC1 : CODEX.CRITTERSTATUS.METABOLISM.STARVING.CONTAINER1_VANILLA, 10), ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSpacer|7_4(ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubEntry|7_0("CREATURES::GUIDE::MOOD", CODEX.CRITTERSTATUS.MOOD.TITLE, ref CS$<>8__locals1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddImage|7_1(Assets.GetSprite("codex_mood"), ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.BODY.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.MOOD.HAPPY.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.HAPPY.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.HAPPY.SUBTITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBulletPoint|7_5(CODEX.CRITTERSTATUS.MOOD.HAPPY.HAPPY_METABOLISM, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.MOOD.GLUM.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.GLUM.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.GLUM.SUBTITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBulletPoint|7_5(CODEX.CRITTERSTATUS.MOOD.GLUM.GLUMWILD_METABOLISM, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.MOOD.HOSTILE.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(DlcManager.IsExpansion1Active() ? CODEX.CRITTERSTATUS.MOOD.HOSTILE.CONTAINER1_DLC1 : CODEX.CRITTERSTATUS.MOOD.HOSTILE.CONTAINER1_VANILLA, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.MOOD.CONFINED.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.CONFINED.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.CONFINED.SUBTITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBulletPoint|7_5(CODEX.CRITTERSTATUS.MOOD.CONFINED.CONFINED_FERTILITY, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBulletPoint|7_5(CODEX.CRITTERSTATUS.MOOD.CONFINED.CONFINED_HAPPINESS, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.MOOD.OVERCROWDED.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.OVERCROWDED.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.OVERCROWDED.SUBTITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBulletPoint|7_5(CODEX.CRITTERSTATUS.MOOD.OVERCROWDED.OVERCROWDED_HAPPY1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.MOOD.CRAMPED.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.CRAMPED.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.MOOD.CRAMPED.SUBTITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBulletPoint|7_5(CODEX.CRITTERSTATUS.MOOD.CRAMPED.CRAMPED_FERTILITY, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSpacer|7_4(ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubEntry|7_0("CREATURES::GUIDE::FERTILITY", CODEX.CRITTERSTATUS.FERTILITY.TITLE, ref CS$<>8__locals1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddImage|7_1(Assets.GetSprite("codex_reproduction"), ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.FERTILITY.BODY.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.FERTILITY.FERTILITYRATE.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.FERTILITY.FERTILITYRATE.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.FERTILITY.EGGCHANCES.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.FERTILITY.EGGCHANCES.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.FERTILITY.INCUBATION.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.FERTILITY.INCUBATION.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.FERTILITY.MAXAGE.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(DlcManager.IsExpansion1Active() ? CODEX.CRITTERSTATUS.FERTILITY.MAXAGE.CONTAINER1_DLC1 : CODEX.CRITTERSTATUS.FERTILITY.MAXAGE.CONTAINER1_VANILLA, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSpacer|7_4(ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubEntry|7_0("CREATURES::GUIDE::DOMESTICATION", CODEX.CRITTERSTATUS.DOMESTICATION.TITLE, ref CS$<>8__locals1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddImage|7_1(Assets.GetSprite("codex_domestication"), ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.DOMESTICATION.BODY.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.DOMESTICATION.WILD.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.DOMESTICATION.WILD.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.DOMESTICATION.WILD.SUBTITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBulletPoint|7_5(CODEX.CRITTERSTATUS.DOMESTICATION.WILD.WILD_METABOLISM, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBulletPoint|7_5(CODEX.CRITTERSTATUS.DOMESTICATION.WILD.WILD_POOP, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSubtitle|7_2(CODEX.CRITTERSTATUS.DOMESTICATION.TAME.TITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.DOMESTICATION.TAME.CONTAINER1, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBody|7_3(CODEX.CRITTERSTATUS.DOMESTICATION.TAME.SUBTITLE, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBulletPoint|7_5(CODEX.CRITTERSTATUS.DOMESTICATION.TAME.TAME_HAPPINESS, ref CS$<>8__locals2);
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddBulletPoint|7_5(CODEX.CRITTERSTATUS.DOMESTICATION.TAME.TAME_METABOLISM, ref CS$<>8__locals2);
		return CS$<>8__locals1.generalInfoEntry;
	}

	// Token: 0x060054B0 RID: 21680 RVA: 0x001EC324 File Offset: 0x001EA524
	private static CodexEntry GenerateCritterEntry(Tag speciesTag, string name, List<GameObject> brains)
	{
		CodexEntry codexEntry = null;
		List<ContentContainer> list = new List<ContentContainer>();
		foreach (GameObject gameObject in brains)
		{
			if (gameObject.GetDef<BabyMonitor.Def>() == null)
			{
				Sprite sprite = null;
				CreatureBrain component = gameObject.GetComponent<CreatureBrain>();
				if (!(component.species != speciesTag))
				{
					if (codexEntry == null)
					{
						codexEntry = new CodexEntry("CREATURES", list, name);
						codexEntry.sortString = name;
						list.Add(new ContentContainer(new List<ICodexWidget>
						{
							new CodexSpacer(),
							new CodexSpacer()
						}, ContentContainer.ContentLayout.Vertical));
					}
					List<ContentContainer> list2 = new List<ContentContainer>();
					string symbolPrefix = component.symbolPrefix;
					Sprite first = Def.GetUISprite(gameObject, symbolPrefix + "ui", false).first;
					GameObject gameObject2 = Assets.TryGetPrefab(gameObject.PrefabID().ToString() + "Baby");
					if (gameObject2 != null)
					{
						sprite = Def.GetUISprite(gameObject2, "ui", false).first;
					}
					if (sprite)
					{
						CodexEntryGenerator.GenerateImageContainers(new Sprite[]
						{
							first,
							sprite
						}, list2, ContentContainer.ContentLayout.Horizontal);
					}
					else
					{
						CodexEntryGenerator.GenerateImageContainers(first, list2);
					}
					CodexEntryGenerator_Creatures.GenerateCreatureDescriptionContainers(gameObject, list2);
					SubEntry subEntry = new SubEntry(component.PrefabID().ToString(), speciesTag.ToString(), list2, component.GetProperName());
					subEntry.icon = first;
					subEntry.iconColor = Color.white;
					codexEntry.subEntries.Add(subEntry);
				}
			}
		}
		return codexEntry;
	}

	// Token: 0x060054B1 RID: 21681 RVA: 0x001EC4EC File Offset: 0x001EA6EC
	private static void GenerateCreatureDescriptionContainers(GameObject creature, List<ContentContainer> containers)
	{
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(creature.GetComponent<InfoDescription>().description, CodexTextStyle.Body, null)
		}, ContentContainer.ContentLayout.Vertical));
		RobotBatteryMonitor.Def def = creature.GetDef<RobotBatteryMonitor.Def>();
		if (def != null)
		{
			Amount batteryAmount = Db.Get().Amounts.Get(def.batteryAmountId);
			float value = Db.Get().traits.Get(creature.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers.Find((AttributeModifier match) => match.AttributeId == batteryAmount.maxAttribute.Id).Value;
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(CODEX.HEADERS.INTERNALBATTERY, CodexTextStyle.Subtitle, null),
				new CodexText("    • " + string.Format(CODEX.ROBOT_DESCRIPTORS.BATTERY.CAPACITY, value), CodexTextStyle.Body, null)
			}, ContentContainer.ContentLayout.Vertical));
		}
		if (creature.GetDef<StorageUnloadMonitor.Def>() != null)
		{
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(CODEX.HEADERS.INTERNALSTORAGE, CodexTextStyle.Subtitle, null),
				new CodexText("    • " + string.Format(CODEX.ROBOT_DESCRIPTORS.STORAGE.CAPACITY, creature.GetComponents<Storage>()[1].Capacity()), CodexTextStyle.Body, null)
			}, ContentContainer.ContentLayout.Vertical));
		}
		List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag((creature.PrefabID().ToString() + "Egg").ToTag());
		if (prefabsWithTag != null && prefabsWithTag.Count > 0)
		{
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(CODEX.HEADERS.HATCHESFROMEGG, CodexTextStyle.Subtitle, null)
			}, ContentContainer.ContentLayout.Vertical));
			foreach (GameObject gameObject in prefabsWithTag)
			{
				containers.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexIndentedLabelWithIcon(gameObject.GetProperName(), CodexTextStyle.Body, Def.GetUISprite(gameObject, "ui", false))
				}, ContentContainer.ContentLayout.Horizontal));
			}
		}
		TemperatureVulnerable component = creature.GetComponent<TemperatureVulnerable>();
		if (component != null)
		{
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(CODEX.HEADERS.COMFORTRANGE, CodexTextStyle.Subtitle, null),
				new CodexText("    • " + string.Format(CODEX.CREATURE_DESCRIPTORS.TEMPERATURE.COMFORT_RANGE, GameUtil.GetFormattedTemperature(component.TemperatureWarningLow, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(component.TemperatureWarningHigh, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), CodexTextStyle.Body, null),
				new CodexText("    • " + string.Format(CODEX.CREATURE_DESCRIPTORS.TEMPERATURE.NON_LETHAL_RANGE, GameUtil.GetFormattedTemperature(component.TemperatureLethalLow, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(component.TemperatureLethalHigh, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), CodexTextStyle.Body, null)
			}, ContentContainer.ContentLayout.Vertical));
		}
		Modifiers component2 = creature.GetComponent<Modifiers>();
		if (component2 != null)
		{
			Klei.AI.Attribute maxAttribute = Db.Get().Amounts.Age.maxAttribute;
			float totalValue = AttributeInstance.GetTotalValue(maxAttribute, component2.GetPreModifiers(maxAttribute));
			string text;
			if (Mathf.Approximately(totalValue, 0f))
			{
				text = null;
			}
			else
			{
				text = string.Format(CODEX.CREATURE_DESCRIPTORS.MAXAGE, maxAttribute.formatter.GetFormattedValue(totalValue, GameUtil.TimeSlice.None));
			}
			if (text != null)
			{
				containers.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexSpacer(),
					new CodexText(CODEX.HEADERS.CRITTERMAXAGE, CodexTextStyle.Subtitle, null),
					new CodexText(text, CodexTextStyle.Body, null)
				}, ContentContainer.ContentLayout.Vertical));
			}
		}
		OvercrowdingMonitor.Def def2 = creature.GetDef<OvercrowdingMonitor.Def>();
		if (def2 != null && def2.spaceRequiredPerCreature > 0)
		{
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(CODEX.HEADERS.CRITTEROVERCROWDING, CodexTextStyle.Subtitle, null),
				new CodexText("    • " + string.Format(CODEX.CREATURE_DESCRIPTORS.OVERCROWDING, def2.spaceRequiredPerCreature), CodexTextStyle.Body, null),
				new CodexText("    • " + string.Format(CODEX.CREATURE_DESCRIPTORS.CONFINED, def2.spaceRequiredPerCreature), CodexTextStyle.Body, null)
			}, ContentContainer.ContentLayout.Vertical));
		}
		int num = 0;
		string item = null;
		Tag tag = default(Tag);
		Butcherable component3 = creature.GetComponent<Butcherable>();
		if (component3 != null && component3.drops != null)
		{
			num = component3.drops.Length;
			if (num > 0)
			{
				item = (tag.Name = component3.drops[0]);
			}
		}
		string text2 = null;
		string text3 = null;
		if (tag.IsValid)
		{
			text2 = TagManager.GetProperName(tag, false);
			text3 = "\t" + GameUtil.GetFormattedByTag(tag, (float)num, GameUtil.TimeSlice.None);
		}
		if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
		{
			ContentContainer item2 = new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(CODEX.HEADERS.CRITTERDROPS, CodexTextStyle.Subtitle, null)
			}, ContentContainer.ContentLayout.Vertical);
			ContentContainer item3 = new ContentContainer(new List<ICodexWidget>
			{
				new CodexIndentedLabelWithIcon(text2, CodexTextStyle.Body, Def.GetUISprite(item, "ui", false)),
				new CodexText(text3, CodexTextStyle.Body, null)
			}, ContentContainer.ContentLayout.Vertical);
			containers.Add(item2);
			containers.Add(item3);
		}
		new List<Tag>();
		Diet.Info[] array = null;
		CreatureCalorieMonitor.Def def3 = creature.GetDef<CreatureCalorieMonitor.Def>();
		BeehiveCalorieMonitor.Def def4 = creature.GetDef<BeehiveCalorieMonitor.Def>();
		if (def3 != null)
		{
			array = def3.diet.infos;
		}
		else if (def4 != null)
		{
			array = def4.diet.infos;
		}
		if (array != null && array.Length != 0)
		{
			float num2 = 0f;
			foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(creature.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
			{
				if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
				{
					num2 = attributeModifier.Value;
				}
			}
			List<ICodexWidget> list = new List<ICodexWidget>();
			foreach (Diet.Info info in array)
			{
				if (info.consumedTags.Count != 0)
				{
					foreach (Tag tag2 in info.consumedTags)
					{
						Element element = ElementLoader.FindElementByHash(ElementLoader.GetElementID(tag2));
						if ((element.id != SimHashes.Vacuum && element.id != SimHashes.Void) || !(Assets.GetPrefab(tag2) == null))
						{
							float num3 = -num2 / info.caloriesPerKg;
							float outputAmount = num3 * info.producedConversionRate;
							list.Add(new CodexConversionPanel(tag2.ProperName(), tag2, num3, true, info.producedElement, outputAmount, true, creature));
						}
					}
				}
			}
			ContentContainer contentContainer = new ContentContainer(list, ContentContainer.ContentLayout.Vertical);
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexCollapsibleHeader(CODEX.HEADERS.DIET, contentContainer)
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(contentContainer);
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical));
		}
	}

	// Token: 0x060054B3 RID: 21683 RVA: 0x001ECCC0 File Offset: 0x001EAEC0
	[CompilerGenerated]
	internal static void <GenerateEntries>g__AddEntry|6_0(string entryId, CodexEntry entry, string parentEntryId, ref CodexEntryGenerator_Creatures.<>c__DisplayClass6_0 A_3)
	{
		if (entry == null)
		{
			return;
		}
		entry.parentId = parentEntryId;
		CodexCache.AddEntry(entryId, entry, null);
		A_3.results.Add(entryId, entry);
	}

	// Token: 0x060054B4 RID: 21684 RVA: 0x001ECCE4 File Offset: 0x001EAEE4
	[CompilerGenerated]
	internal static void <GenerateEntries>g__PushCritterEntry|6_1(Tag speciesTag, string name, ref CodexEntryGenerator_Creatures.<>c__DisplayClass6_0 A_2)
	{
		CodexEntry codexEntry = CodexEntryGenerator_Creatures.GenerateCritterEntry(speciesTag, name, A_2.brains);
		if (codexEntry != null)
		{
			A_2.critterEntries.Add(new ValueTuple<string, CodexEntry>(speciesTag.ToString(), codexEntry));
		}
	}

	// Token: 0x060054B5 RID: 21685 RVA: 0x001ECD20 File Offset: 0x001EAF20
	[CompilerGenerated]
	internal static void <GenerateEntries>g__PopAndAddAllCritterEntries|6_2(ref CodexEntryGenerator_Creatures.<>c__DisplayClass6_0 A_0)
	{
		foreach (ValueTuple<string, CodexEntry> valueTuple in A_0.critterEntries.StableSort((ValueTuple<string, CodexEntry> pair) => UI.StripLinkFormatting(pair.Item2.name)))
		{
			string item = valueTuple.Item1;
			CodexEntry item2 = valueTuple.Item2;
			CodexEntryGenerator_Creatures.<GenerateEntries>g__AddEntry|6_0(item, item2, "CREATURES", ref A_0);
		}
	}

	// Token: 0x060054B6 RID: 21686 RVA: 0x001ECDA4 File Offset: 0x001EAFA4
	[CompilerGenerated]
	internal static void <GenerateFieldGuideEntry>g__AddSubEntry|7_0(string id, string name, ref CodexEntryGenerator_Creatures.<>c__DisplayClass7_0 A_2, ref CodexEntryGenerator_Creatures.<>c__DisplayClass7_1 A_3)
	{
		A_3.subEntryContents = new List<ICodexWidget>();
		A_3.subEntryContents.Add(new CodexText(name, CodexTextStyle.Title, null));
		A_3.subEntry = new SubEntry(id, "CREATURES::GUIDE", new List<ContentContainer>
		{
			new ContentContainer(A_3.subEntryContents, ContentContainer.ContentLayout.Vertical)
		}, name);
		A_2.generalInfoEntry.subEntries.Add(A_3.subEntry);
	}

	// Token: 0x060054B7 RID: 21687 RVA: 0x001ECE0E File Offset: 0x001EB00E
	[CompilerGenerated]
	internal static void <GenerateFieldGuideEntry>g__AddImage|7_1(Sprite sprite, ref CodexEntryGenerator_Creatures.<>c__DisplayClass7_1 A_1)
	{
		A_1.subEntryContents.Add(new CodexImage(432, 1, sprite));
	}

	// Token: 0x060054B8 RID: 21688 RVA: 0x001ECE27 File Offset: 0x001EB027
	[CompilerGenerated]
	internal static void <GenerateFieldGuideEntry>g__AddSubtitle|7_2(string text, ref CodexEntryGenerator_Creatures.<>c__DisplayClass7_1 A_1)
	{
		CodexEntryGenerator_Creatures.<GenerateFieldGuideEntry>g__AddSpacer|7_4(ref A_1);
		A_1.subEntryContents.Add(new CodexText(text, CodexTextStyle.Subtitle, null));
	}

	// Token: 0x060054B9 RID: 21689 RVA: 0x001ECE42 File Offset: 0x001EB042
	[CompilerGenerated]
	internal static void <GenerateFieldGuideEntry>g__AddBody|7_3(string text, ref CodexEntryGenerator_Creatures.<>c__DisplayClass7_1 A_1)
	{
		A_1.subEntryContents.Add(new CodexText(text, CodexTextStyle.Body, null));
	}

	// Token: 0x060054BA RID: 21690 RVA: 0x001ECE57 File Offset: 0x001EB057
	[CompilerGenerated]
	internal static void <GenerateFieldGuideEntry>g__AddSpacer|7_4(ref CodexEntryGenerator_Creatures.<>c__DisplayClass7_1 A_0)
	{
		A_0.subEntryContents.Add(new CodexSpacer());
	}

	// Token: 0x060054BB RID: 21691 RVA: 0x001ECE6C File Offset: 0x001EB06C
	[CompilerGenerated]
	internal static void <GenerateFieldGuideEntry>g__AddBulletPoint|7_5(string text, ref CodexEntryGenerator_Creatures.<>c__DisplayClass7_1 A_1)
	{
		if (text.StartsWith("    • "))
		{
			text = text.Substring("    • ".Length);
		}
		text = "<indent=13px>•<indent=21px>" + text + "</indent></indent>";
		A_1.subEntryContents.Add(new CodexText(text, CodexTextStyle.Body, null));
	}

	// Token: 0x04003876 RID: 14454
	public const string CATEGORY_ID = "CREATURES";

	// Token: 0x04003877 RID: 14455
	public const string GUIDE_ID = "CREATURES::GUIDE";

	// Token: 0x04003878 RID: 14456
	public const string GUIDE_METABOLISM_ID = "CREATURES::GUIDE::METABOLISM";

	// Token: 0x04003879 RID: 14457
	public const string GUIDE_MOOD_ID = "CREATURES::GUIDE::MOOD";

	// Token: 0x0400387A RID: 14458
	public const string GUIDE_FERTILITY_ID = "CREATURES::GUIDE::FERTILITY";

	// Token: 0x0400387B RID: 14459
	public const string GUIDE_DOMESTICATION_ID = "CREATURES::GUIDE::DOMESTICATION";
}
