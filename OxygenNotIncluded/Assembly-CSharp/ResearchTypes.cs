using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200092C RID: 2348
public class ResearchTypes
{
	// Token: 0x0600442E RID: 17454 RVA: 0x0017E82C File Offset: 0x0017CA2C
	public ResearchTypes()
	{
		ResearchType item = new ResearchType("basic", RESEARCH.TYPES.ALPHA.NAME, RESEARCH.TYPES.ALPHA.DESC, Assets.GetSprite("research_type_alpha_icon"), new Color(0.59607846f, 0.6666667f, 0.9137255f), new Recipe.Ingredient[]
		{
			new Recipe.Ingredient("Dirt".ToTag(), 100f)
		}, 600f, "research_center_kanim", new string[]
		{
			"ResearchCenter"
		}, RESEARCH.TYPES.ALPHA.RECIPEDESC);
		this.Types.Add(item);
		ResearchType item2 = new ResearchType("advanced", RESEARCH.TYPES.BETA.NAME, RESEARCH.TYPES.BETA.DESC, Assets.GetSprite("research_type_beta_icon"), new Color(0.6f, 0.38431373f, 0.5686275f), new Recipe.Ingredient[]
		{
			new Recipe.Ingredient("Water".ToTag(), 25f)
		}, 1200f, "research_center_kanim", new string[]
		{
			"AdvancedResearchCenter"
		}, RESEARCH.TYPES.BETA.RECIPEDESC);
		this.Types.Add(item2);
		ResearchType item3 = new ResearchType("space", RESEARCH.TYPES.GAMMA.NAME, RESEARCH.TYPES.GAMMA.DESC, Assets.GetSprite("research_type_gamma_icon"), new Color32(240, 141, 44, byte.MaxValue), null, 2400f, "research_center_kanim", new string[]
		{
			"CosmicResearchCenter"
		}, RESEARCH.TYPES.GAMMA.RECIPEDESC);
		this.Types.Add(item3);
		ResearchType item4 = new ResearchType("nuclear", RESEARCH.TYPES.DELTA.NAME, RESEARCH.TYPES.DELTA.DESC, Assets.GetSprite("research_type_delta_icon"), new Color32(231, 210, 17, byte.MaxValue), null, 2400f, "research_center_kanim", new string[]
		{
			"NuclearResearchCenter"
		}, RESEARCH.TYPES.DELTA.RECIPEDESC);
		this.Types.Add(item4);
		ResearchType item5 = new ResearchType("orbital", RESEARCH.TYPES.ORBITAL.NAME, RESEARCH.TYPES.ORBITAL.DESC, Assets.GetSprite("research_type_orbital_icon"), new Color32(240, 141, 44, byte.MaxValue), null, 2400f, "research_center_kanim", new string[]
		{
			"OrbitalResearchCenter",
			"DLC1CosmicResearchCenter"
		}, RESEARCH.TYPES.ORBITAL.RECIPEDESC);
		this.Types.Add(item5);
	}

	// Token: 0x0600442F RID: 17455 RVA: 0x0017EAF0 File Offset: 0x0017CCF0
	public ResearchType GetResearchType(string id)
	{
		foreach (ResearchType researchType in this.Types)
		{
			if (id == researchType.id)
			{
				return researchType;
			}
		}
		global::Debug.LogWarning(string.Format("No research with type id {0} found", id));
		return null;
	}

	// Token: 0x04002D35 RID: 11573
	public List<ResearchType> Types = new List<ResearchType>();

	// Token: 0x02001771 RID: 6001
	public class ID
	{
		// Token: 0x04006EC1 RID: 28353
		public const string BASIC = "basic";

		// Token: 0x04006EC2 RID: 28354
		public const string ADVANCED = "advanced";

		// Token: 0x04006EC3 RID: 28355
		public const string SPACE = "space";

		// Token: 0x04006EC4 RID: 28356
		public const string NUCLEAR = "nuclear";

		// Token: 0x04006EC5 RID: 28357
		public const string ORBITAL = "orbital";
	}
}
