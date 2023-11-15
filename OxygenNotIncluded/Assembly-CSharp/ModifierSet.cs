using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003ED RID: 1005
public class ModifierSet : ScriptableObject
{
	// Token: 0x06001528 RID: 5416 RVA: 0x0006FC6C File Offset: 0x0006DE6C
	public virtual void Initialize()
	{
		this.ResourceTable = new List<Resource>();
		this.Root = new ResourceSet<Resource>("Root", null);
		this.modifierInfos = new ModifierSet.ModifierInfos();
		this.modifierInfos.Load(this.modifiersFile);
		this.Attributes = new Database.Attributes(this.Root);
		this.BuildingAttributes = new BuildingAttributes(this.Root);
		this.CritterAttributes = new CritterAttributes(this.Root);
		this.PlantAttributes = new PlantAttributes(this.Root);
		this.effects = new ResourceSet<Effect>("Effects", this.Root);
		this.traits = new ModifierSet.TraitSet();
		this.traitGroups = new ModifierSet.TraitGroupSet();
		this.FertilityModifiers = new FertilityModifiers();
		this.Amounts = new Database.Amounts();
		this.Amounts.Load();
		this.AttributeConverters = new Database.AttributeConverters();
		this.LoadEffects();
		this.LoadFertilityModifiers();
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x0006FD59 File Offset: 0x0006DF59
	public static float ConvertValue(float value, Units units)
	{
		if (Units.PerDay == units)
		{
			return value * 0.0016666667f;
		}
		return value;
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x0006FD68 File Offset: 0x0006DF68
	private void LoadEffects()
	{
		foreach (ModifierSet.ModifierInfo modifierInfo in this.modifierInfos)
		{
			if (!this.effects.Exists(modifierInfo.Id) && (modifierInfo.Type == "Effect" || modifierInfo.Type == "Base" || modifierInfo.Type == "Need"))
			{
				string text = Strings.Get(string.Format("STRINGS.DUPLICANTS.MODIFIERS.{0}.NAME", modifierInfo.Id.ToUpper()));
				string description = Strings.Get(string.Format("STRINGS.DUPLICANTS.MODIFIERS.{0}.TOOLTIP", modifierInfo.Id.ToUpper()));
				Effect effect = new Effect(modifierInfo.Id, text, description, modifierInfo.Duration * 600f, modifierInfo.ShowInUI && modifierInfo.Type != "Need", modifierInfo.TriggerFloatingText, modifierInfo.IsBad, modifierInfo.EmoteAnim, modifierInfo.EmoteCooldown, modifierInfo.StompGroup, modifierInfo.CustomIcon);
				effect.stompPriority = modifierInfo.StompPriority;
				foreach (ModifierSet.ModifierInfo modifierInfo2 in this.modifierInfos)
				{
					if (modifierInfo2.Id == modifierInfo.Id)
					{
						effect.Add(new AttributeModifier(modifierInfo2.Attribute, ModifierSet.ConvertValue(modifierInfo2.Value, modifierInfo2.Units), text, modifierInfo2.Multiplier, false, true));
					}
				}
				this.effects.Add(effect);
			}
		}
		Effect effect2 = new Effect("Ranched", STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, STRINGS.CREATURES.MODIFIERS.RANCHED.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect2.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, false, false, true));
		effect2.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -0.09166667f, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, false, false, true));
		this.effects.Add(effect2);
		Effect effect3 = new Effect("HadMilk", STRINGS.CREATURES.MODIFIERS.GOTMILK.NAME, STRINGS.CREATURES.MODIFIERS.GOTMILK.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect3.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.GOTMILK.NAME, false, false, true));
		this.effects.Add(effect3);
		Effect effect4 = new Effect("EggSong", STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.TOOLTIP, 600f, true, false, false, null, -1f, 0f, null, "");
		effect4.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 4f, STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, true, false, true));
		this.effects.Add(effect4);
		Effect effect5 = new Effect("EggHug", STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, STRINGS.CREATURES.MODIFIERS.EGGHUG.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect5.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 1f, STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, true, false, true));
		this.effects.Add(effect5);
		Effect resource = new Effect("HuggingFrenzy", STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.NAME, STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.TOOLTIP, 600f, true, false, false, null, -1f, 0f, null, "");
		this.effects.Add(resource);
		Reactable.ReactablePrecondition precon = delegate(GameObject go, Navigator.ActiveTransition n)
		{
			int cell = Grid.PosToCell(go);
			return Grid.IsValidCell(cell) && Grid.IsGas(cell);
		};
		this.effects.Get("WetFeet").AddEmotePrecondition(precon);
		this.effects.Get("SoakingWet").AddEmotePrecondition(precon);
		Effect effect6 = new Effect("DivergentCropTended", STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect6.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.05f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, true, false, true));
		this.effects.Add(effect6);
		Effect effect7 = new Effect("DivergentCropTendedWorm", STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect7.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.5f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, true, false, true));
		this.effects.Add(effect7);
		Effect effect8 = new Effect("MooWellFed", STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect8.Add(new AttributeModifier(Db.Get().Amounts.Beckoning.deltaAttribute.Id, MooTuning.WELLFED_EFFECT, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, false, false, true));
		effect8.Add(new AttributeModifier(Db.Get().Amounts.MilkProduction.deltaAttribute.Id, MooTuning.MILK_PRODUCTION_PERCENTAGE_PER_SECOND, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, false, false, true));
		this.effects.Add(effect8);
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x000703CC File Offset: 0x0006E5CC
	public Trait CreateTrait(string id, string name, string description, string group_name, bool should_save, ChoreGroup[] disabled_chore_groups, bool positive_trait, bool is_valid_starter_trait)
	{
		Trait trait = new Trait(id, name, description, 0f, should_save, disabled_chore_groups, positive_trait, is_valid_starter_trait);
		this.traits.Add(trait);
		if (group_name == "" || group_name == null)
		{
			group_name = "Default";
		}
		TraitGroup traitGroup = this.traitGroups.TryGet(group_name);
		if (traitGroup == null)
		{
			traitGroup = new TraitGroup(group_name, group_name, group_name != "Default");
			this.traitGroups.Add(traitGroup);
		}
		traitGroup.Add(trait);
		return trait;
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x00070454 File Offset: 0x0006E654
	public FertilityModifier CreateFertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction)
	{
		FertilityModifier fertilityModifier = new FertilityModifier(id, targetTag, name, description, tooltipCB, applyFunction);
		this.FertilityModifiers.Add(fertilityModifier);
		return fertilityModifier;
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x0007047E File Offset: 0x0006E67E
	protected void LoadTraits()
	{
		TRAITS.TRAIT_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x000704A9 File Offset: 0x0006E6A9
	protected void LoadFertilityModifiers()
	{
		TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}

	// Token: 0x04000B7F RID: 2943
	public TextAsset modifiersFile;

	// Token: 0x04000B80 RID: 2944
	public ModifierSet.ModifierInfos modifierInfos;

	// Token: 0x04000B81 RID: 2945
	public ModifierSet.TraitSet traits;

	// Token: 0x04000B82 RID: 2946
	public ResourceSet<Effect> effects;

	// Token: 0x04000B83 RID: 2947
	public ModifierSet.TraitGroupSet traitGroups;

	// Token: 0x04000B84 RID: 2948
	public FertilityModifiers FertilityModifiers;

	// Token: 0x04000B85 RID: 2949
	public Database.Attributes Attributes;

	// Token: 0x04000B86 RID: 2950
	public BuildingAttributes BuildingAttributes;

	// Token: 0x04000B87 RID: 2951
	public CritterAttributes CritterAttributes;

	// Token: 0x04000B88 RID: 2952
	public PlantAttributes PlantAttributes;

	// Token: 0x04000B89 RID: 2953
	public Database.Amounts Amounts;

	// Token: 0x04000B8A RID: 2954
	public Database.AttributeConverters AttributeConverters;

	// Token: 0x04000B8B RID: 2955
	public ResourceSet Root;

	// Token: 0x04000B8C RID: 2956
	public List<Resource> ResourceTable;

	// Token: 0x02001079 RID: 4217
	public class ModifierInfo : Resource
	{
		// Token: 0x04005927 RID: 22823
		public string Type;

		// Token: 0x04005928 RID: 22824
		public string Attribute;

		// Token: 0x04005929 RID: 22825
		public float Value;

		// Token: 0x0400592A RID: 22826
		public Units Units;

		// Token: 0x0400592B RID: 22827
		public bool Multiplier;

		// Token: 0x0400592C RID: 22828
		public float Duration;

		// Token: 0x0400592D RID: 22829
		public bool ShowInUI;

		// Token: 0x0400592E RID: 22830
		public string StompGroup;

		// Token: 0x0400592F RID: 22831
		public int StompPriority;

		// Token: 0x04005930 RID: 22832
		public bool IsBad;

		// Token: 0x04005931 RID: 22833
		public string CustomIcon;

		// Token: 0x04005932 RID: 22834
		public bool TriggerFloatingText;

		// Token: 0x04005933 RID: 22835
		public string EmoteAnim;

		// Token: 0x04005934 RID: 22836
		public float EmoteCooldown;
	}

	// Token: 0x0200107A RID: 4218
	[Serializable]
	public class ModifierInfos : ResourceLoader<ModifierSet.ModifierInfo>
	{
	}

	// Token: 0x0200107B RID: 4219
	[Serializable]
	public class TraitSet : ResourceSet<Trait>
	{
	}

	// Token: 0x0200107C RID: 4220
	[Serializable]
	public class TraitGroupSet : ResourceSet<TraitGroup>
	{
	}
}
