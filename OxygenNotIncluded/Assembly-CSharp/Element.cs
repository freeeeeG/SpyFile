using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using STRINGS;

// Token: 0x02000784 RID: 1924
[DebuggerDisplay("{name}")]
[Serializable]
public class Element : IComparable<Element>
{
	// Token: 0x06003520 RID: 13600 RVA: 0x0011FDC4 File Offset: 0x0011DFC4
	public float PressureToMass(float pressure)
	{
		return pressure / this.defaultValues.pressure;
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06003521 RID: 13601 RVA: 0x0011FDD3 File Offset: 0x0011DFD3
	public bool IsUnstable
	{
		get
		{
			return this.HasTag(GameTags.Unstable);
		}
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06003522 RID: 13602 RVA: 0x0011FDE0 File Offset: 0x0011DFE0
	public bool IsLiquid
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Liquid;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06003523 RID: 13603 RVA: 0x0011FDED File Offset: 0x0011DFED
	public bool IsGas
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Gas;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x06003524 RID: 13604 RVA: 0x0011FDFA File Offset: 0x0011DFFA
	public bool IsSolid
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Solid;
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06003525 RID: 13605 RVA: 0x0011FE07 File Offset: 0x0011E007
	public bool IsVacuum
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Vacuum;
		}
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06003526 RID: 13606 RVA: 0x0011FE14 File Offset: 0x0011E014
	public bool IsTemperatureInsulated
	{
		get
		{
			return (this.state & Element.State.TemperatureInsulated) > Element.State.Vacuum;
		}
	}

	// Token: 0x06003527 RID: 13607 RVA: 0x0011FE22 File Offset: 0x0011E022
	public bool IsState(Element.State expected_state)
	{
		return (this.state & Element.State.Solid) == expected_state;
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06003528 RID: 13608 RVA: 0x0011FE2F File Offset: 0x0011E02F
	public bool HasTransitionUp
	{
		get
		{
			return this.highTempTransitionTarget != (SimHashes)0 && this.highTempTransitionTarget != SimHashes.Unobtanium && this.highTempTransition != null && this.highTempTransition != this;
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06003529 RID: 13609 RVA: 0x0011FE5C File Offset: 0x0011E05C
	// (set) Token: 0x0600352A RID: 13610 RVA: 0x0011FE64 File Offset: 0x0011E064
	public string name { get; set; }

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x0600352B RID: 13611 RVA: 0x0011FE6D File Offset: 0x0011E06D
	// (set) Token: 0x0600352C RID: 13612 RVA: 0x0011FE75 File Offset: 0x0011E075
	public string nameUpperCase { get; set; }

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x0600352D RID: 13613 RVA: 0x0011FE7E File Offset: 0x0011E07E
	// (set) Token: 0x0600352E RID: 13614 RVA: 0x0011FE86 File Offset: 0x0011E086
	public string description { get; set; }

	// Token: 0x0600352F RID: 13615 RVA: 0x0011FE8F File Offset: 0x0011E08F
	public string GetStateString()
	{
		return Element.GetStateString(this.state);
	}

	// Token: 0x06003530 RID: 13616 RVA: 0x0011FE9C File Offset: 0x0011E09C
	public static string GetStateString(Element.State state)
	{
		if ((state & Element.State.Solid) == Element.State.Solid)
		{
			return ELEMENTS.STATE.SOLID;
		}
		if ((state & Element.State.Solid) == Element.State.Liquid)
		{
			return ELEMENTS.STATE.LIQUID;
		}
		if ((state & Element.State.Solid) == Element.State.Gas)
		{
			return ELEMENTS.STATE.GAS;
		}
		return ELEMENTS.STATE.VACUUM;
	}

	// Token: 0x06003531 RID: 13617 RVA: 0x0011FEDC File Offset: 0x0011E0DC
	public string FullDescription(bool addHardnessColor = true)
	{
		string text = this.Description();
		if (this.IsSolid)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCSOLID, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetHardnessString(this, addHardnessColor));
		}
		else if (this.IsLiquid)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCLIQUID, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		}
		else if (!this.IsVacuum)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCGAS, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		}
		string text2 = ELEMENTS.THERMALPROPERTIES;
		text2 = text2.Replace("{SPECIFIC_HEAT_CAPACITY}", GameUtil.GetFormattedSHC(this.specificHeatCapacity));
		text2 = text2.Replace("{THERMAL_CONDUCTIVITY}", GameUtil.GetFormattedThermalConductivity(this.thermalConductivity));
		text = text + "\n" + text2;
		if (DlcManager.FeatureRadiationEnabled())
		{
			text = text + "\n" + string.Format(ELEMENTS.RADIATIONPROPERTIES, this.radiationAbsorptionFactor, GameUtil.GetFormattedRads(this.radiationPer1000Mass * 1.1f / 600f, GameUtil.TimeSlice.PerCycle));
		}
		if (this.oreTags.Length != 0 && !this.IsVacuum)
		{
			text += "\n\n";
			string text3 = "";
			for (int i = 0; i < this.oreTags.Length; i++)
			{
				Tag tag = new Tag(this.oreTags[i]);
				text3 += tag.ProperName();
				if (i < this.oreTags.Length - 1)
				{
					text3 += ", ";
				}
			}
			text += string.Format(ELEMENTS.ELEMENTPROPERTIES, text3);
		}
		if (this.attributeModifiers.Count > 0)
		{
			foreach (AttributeModifier attributeModifier in this.attributeModifiers)
			{
				string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
				string formattedString = attributeModifier.GetFormattedString();
				text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name, formattedString);
			}
		}
		return text;
	}

	// Token: 0x06003532 RID: 13618 RVA: 0x00120180 File Offset: 0x0011E380
	public string Description()
	{
		return this.description;
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x00120188 File Offset: 0x0011E388
	public bool HasTag(Tag search_tag)
	{
		return this.tag == search_tag || Array.IndexOf<Tag>(this.oreTags, search_tag) != -1;
	}

	// Token: 0x06003534 RID: 13620 RVA: 0x001201AC File Offset: 0x0011E3AC
	public Tag GetMaterialCategoryTag()
	{
		return this.materialCategory;
	}

	// Token: 0x06003535 RID: 13621 RVA: 0x001201B4 File Offset: 0x0011E3B4
	public int CompareTo(Element other)
	{
		return this.id - other.id;
	}

	// Token: 0x04002053 RID: 8275
	public const int INVALID_ID = 0;

	// Token: 0x04002054 RID: 8276
	public SimHashes id;

	// Token: 0x04002055 RID: 8277
	public Tag tag;

	// Token: 0x04002056 RID: 8278
	public ushort idx;

	// Token: 0x04002057 RID: 8279
	public float specificHeatCapacity;

	// Token: 0x04002058 RID: 8280
	public float thermalConductivity = 1f;

	// Token: 0x04002059 RID: 8281
	public float molarMass = 1f;

	// Token: 0x0400205A RID: 8282
	public float strength;

	// Token: 0x0400205B RID: 8283
	public float flow;

	// Token: 0x0400205C RID: 8284
	public float maxCompression;

	// Token: 0x0400205D RID: 8285
	public float viscosity;

	// Token: 0x0400205E RID: 8286
	public float minHorizontalFlow = float.PositiveInfinity;

	// Token: 0x0400205F RID: 8287
	public float minVerticalFlow = float.PositiveInfinity;

	// Token: 0x04002060 RID: 8288
	public float maxMass = 10000f;

	// Token: 0x04002061 RID: 8289
	public float solidSurfaceAreaMultiplier;

	// Token: 0x04002062 RID: 8290
	public float liquidSurfaceAreaMultiplier;

	// Token: 0x04002063 RID: 8291
	public float gasSurfaceAreaMultiplier;

	// Token: 0x04002064 RID: 8292
	public Element.State state;

	// Token: 0x04002065 RID: 8293
	public byte hardness;

	// Token: 0x04002066 RID: 8294
	public float lowTemp;

	// Token: 0x04002067 RID: 8295
	public SimHashes lowTempTransitionTarget;

	// Token: 0x04002068 RID: 8296
	public Element lowTempTransition;

	// Token: 0x04002069 RID: 8297
	public float highTemp;

	// Token: 0x0400206A RID: 8298
	public SimHashes highTempTransitionTarget;

	// Token: 0x0400206B RID: 8299
	public Element highTempTransition;

	// Token: 0x0400206C RID: 8300
	public SimHashes highTempTransitionOreID = SimHashes.Vacuum;

	// Token: 0x0400206D RID: 8301
	public float highTempTransitionOreMassConversion;

	// Token: 0x0400206E RID: 8302
	public SimHashes lowTempTransitionOreID = SimHashes.Vacuum;

	// Token: 0x0400206F RID: 8303
	public float lowTempTransitionOreMassConversion;

	// Token: 0x04002070 RID: 8304
	public SimHashes sublimateId;

	// Token: 0x04002071 RID: 8305
	public SimHashes convertId;

	// Token: 0x04002072 RID: 8306
	public SpawnFXHashes sublimateFX;

	// Token: 0x04002073 RID: 8307
	public float sublimateRate;

	// Token: 0x04002074 RID: 8308
	public float sublimateEfficiency;

	// Token: 0x04002075 RID: 8309
	public float sublimateProbability;

	// Token: 0x04002076 RID: 8310
	public float offGasPercentage;

	// Token: 0x04002077 RID: 8311
	public float lightAbsorptionFactor;

	// Token: 0x04002078 RID: 8312
	public float radiationAbsorptionFactor;

	// Token: 0x04002079 RID: 8313
	public float radiationPer1000Mass;

	// Token: 0x0400207A RID: 8314
	public Sim.PhysicsData defaultValues;

	// Token: 0x0400207B RID: 8315
	public float toxicity;

	// Token: 0x0400207C RID: 8316
	public Substance substance;

	// Token: 0x0400207D RID: 8317
	public Tag materialCategory;

	// Token: 0x0400207E RID: 8318
	public int buildMenuSort;

	// Token: 0x0400207F RID: 8319
	public ElementLoader.ElementComposition[] elementComposition;

	// Token: 0x04002080 RID: 8320
	public Tag[] oreTags = new Tag[0];

	// Token: 0x04002081 RID: 8321
	public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();

	// Token: 0x04002082 RID: 8322
	public bool disabled;

	// Token: 0x04002083 RID: 8323
	public string dlcId;

	// Token: 0x04002084 RID: 8324
	public const byte StateMask = 3;

	// Token: 0x02001506 RID: 5382
	[Serializable]
	public enum State : byte
	{
		// Token: 0x04006714 RID: 26388
		Vacuum,
		// Token: 0x04006715 RID: 26389
		Gas,
		// Token: 0x04006716 RID: 26390
		Liquid,
		// Token: 0x04006717 RID: 26391
		Solid,
		// Token: 0x04006718 RID: 26392
		Unbreakable,
		// Token: 0x04006719 RID: 26393
		Unstable = 8,
		// Token: 0x0400671A RID: 26394
		TemperatureInsulated = 16
	}
}
