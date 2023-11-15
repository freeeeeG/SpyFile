using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;
using UnityEngine;

// Token: 0x02000AF4 RID: 2804
public class DiseaseInfoScreen : TargetScreen
{
	// Token: 0x06005682 RID: 22146 RVA: 0x001F84A5 File Offset: 0x001F66A5
	public override bool IsValidForTarget(GameObject target)
	{
		return CellSelectionObject.IsSelectionObject(target) || target.GetComponent<PrimaryElement>() != null;
	}

	// Token: 0x06005683 RID: 22147 RVA: 0x001F84C0 File Offset: 0x001F66C0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.diseaseSourcePanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false).GetComponent<CollapsibleDetailContentPanel>();
		this.diseaseSourcePanel.SetTitle(UI.DETAILTABS.DISEASE.DISEASE_SOURCE);
		this.immuneSystemPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false).GetComponent<CollapsibleDetailContentPanel>();
		this.immuneSystemPanel.SetTitle(UI.DETAILTABS.DISEASE.IMMUNE_SYSTEM);
		this.currentGermsPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false).GetComponent<CollapsibleDetailContentPanel>();
		this.currentGermsPanel.SetTitle(UI.DETAILTABS.DISEASE.CURRENT_GERMS);
		this.infoPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false).GetComponent<CollapsibleDetailContentPanel>();
		this.infoPanel.SetTitle(UI.DETAILTABS.DISEASE.GERMS_INFO);
		this.infectionPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false).GetComponent<CollapsibleDetailContentPanel>();
		this.infectionPanel.SetTitle(UI.DETAILTABS.DISEASE.INFECTION_INFO);
		base.Subscribe<DiseaseInfoScreen>(-1514841199, DiseaseInfoScreen.OnRefreshDataDelegate);
	}

	// Token: 0x06005684 RID: 22148 RVA: 0x001F85F2 File Offset: 0x001F67F2
	private void LateUpdate()
	{
		this.Refresh();
	}

	// Token: 0x06005685 RID: 22149 RVA: 0x001F85FA File Offset: 0x001F67FA
	private void OnRefreshData(object obj)
	{
		this.Refresh();
	}

	// Token: 0x06005686 RID: 22150 RVA: 0x001F8604 File Offset: 0x001F6804
	private void Refresh()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		List<Descriptor> list = GameUtil.GetAllDescriptors(this.selectedTarget, true);
		Sicknesses sicknesses = this.selectedTarget.GetSicknesses();
		if (sicknesses != null)
		{
			for (int i = 0; i < sicknesses.Count; i++)
			{
				list.AddRange(sicknesses[i].GetDescriptors());
			}
		}
		list = list.FindAll((Descriptor e) => e.type == Descriptor.DescriptorType.DiseaseSource);
		if (list.Count > 0)
		{
			for (int j = 0; j < list.Count; j++)
			{
				this.diseaseSourcePanel.SetLabel("source_" + j.ToString(), list[j].text, list[j].tooltipText);
			}
		}
		this.CreateImmuneInfo();
		if (!this.CreateDiseaseInfo())
		{
			this.currentGermsPanel.SetTitle(UI.DETAILTABS.DISEASE.NO_CURRENT_GERMS);
			this.currentGermsPanel.SetLabel("nodisease", UI.DETAILTABS.DISEASE.DETAILS.NODISEASE, UI.DETAILTABS.DISEASE.DETAILS.NODISEASE_TOOLTIP);
		}
		this.diseaseSourcePanel.Commit();
		this.immuneSystemPanel.Commit();
		this.currentGermsPanel.Commit();
		this.infoPanel.Commit();
		this.infectionPanel.Commit();
	}

	// Token: 0x06005687 RID: 22151 RVA: 0x001F8754 File Offset: 0x001F6954
	private bool CreateImmuneInfo()
	{
		GermExposureMonitor.Instance smi = this.selectedTarget.GetSMI<GermExposureMonitor.Instance>();
		if (smi != null)
		{
			this.immuneSystemPanel.SetTitle(UI.DETAILTABS.DISEASE.CONTRACTION_RATES);
			this.immuneSystemPanel.SetLabel("germ_resistance", Db.Get().Attributes.GermResistance.Name + ": " + smi.GetGermResistance().ToString(), DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.DESC);
			for (int i = 0; i < Db.Get().Diseases.Count; i++)
			{
				Disease disease = Db.Get().Diseases[i];
				ExposureType exposureTypeForDisease = GameUtil.GetExposureTypeForDisease(disease);
				Sickness sicknessForDisease = GameUtil.GetSicknessForDisease(disease);
				if (sicknessForDisease != null)
				{
					bool flag = true;
					List<string> list = new List<string>();
					if (exposureTypeForDisease.required_traits != null && exposureTypeForDisease.required_traits.Count > 0)
					{
						for (int j = 0; j < exposureTypeForDisease.required_traits.Count; j++)
						{
							if (!this.selectedTarget.GetComponent<Traits>().HasTrait(exposureTypeForDisease.required_traits[j]))
							{
								list.Add(exposureTypeForDisease.required_traits[j]);
							}
						}
						if (list.Count > 0)
						{
							flag = false;
						}
					}
					bool flag2 = false;
					List<string> list2 = new List<string>();
					if (exposureTypeForDisease.excluded_effects != null && exposureTypeForDisease.excluded_effects.Count > 0)
					{
						for (int k = 0; k < exposureTypeForDisease.excluded_effects.Count; k++)
						{
							if (this.selectedTarget.GetComponent<Effects>().HasEffect(exposureTypeForDisease.excluded_effects[k]))
							{
								list2.Add(exposureTypeForDisease.excluded_effects[k]);
							}
						}
						if (list2.Count > 0)
						{
							flag2 = true;
						}
					}
					bool flag3 = false;
					List<string> list3 = new List<string>();
					if (exposureTypeForDisease.excluded_traits != null && exposureTypeForDisease.excluded_traits.Count > 0)
					{
						for (int l = 0; l < exposureTypeForDisease.excluded_traits.Count; l++)
						{
							if (this.selectedTarget.GetComponent<Traits>().HasTrait(exposureTypeForDisease.excluded_traits[l]))
							{
								list3.Add(exposureTypeForDisease.excluded_traits[l]);
							}
						}
						if (list3.Count > 0)
						{
							flag3 = true;
						}
					}
					string text = "";
					float num;
					if (!flag)
					{
						num = 0f;
						string text2 = "";
						for (int m = 0; m < list.Count; m++)
						{
							if (text2 != "")
							{
								text2 += ", ";
							}
							text2 += Db.Get().traits.Get(list[m]).Name;
						}
						text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_MISSING_REQUIRED_TRAIT, text2);
					}
					else if (flag3)
					{
						num = 0f;
						string text3 = "";
						for (int n = 0; n < list3.Count; n++)
						{
							if (text3 != "")
							{
								text3 += ", ";
							}
							text3 += Db.Get().traits.Get(list3[n]).Name;
						}
						if (text != "")
						{
							text += "\n";
						}
						text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXLCLUDED_TRAIT, text3);
					}
					else if (flag2)
					{
						num = 0f;
						string text4 = "";
						for (int num2 = 0; num2 < list2.Count; num2++)
						{
							if (text4 != "")
							{
								text4 += ", ";
							}
							text4 += Db.Get().effects.Get(list2[num2]).Name;
						}
						if (text != "")
						{
							text += "\n";
						}
						text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXCLUDED_EFFECT, text4);
					}
					else if (exposureTypeForDisease.infect_immediately)
					{
						num = 1f;
					}
					else
					{
						num = GermExposureMonitor.GetContractionChance(smi.GetResistanceToExposureType(exposureTypeForDisease, 3f));
					}
					string arg = (text != "") ? text : string.Format(DUPLICANTS.DISEASES.CONTRACTION_PROBABILITY, GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None), this.selectedTarget.GetProperName(), sicknessForDisease.Name);
					this.immuneSystemPanel.SetLabel("disease_" + disease.Id, "    • " + disease.Name + ": " + GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None), string.Format(DUPLICANTS.DISEASES.RESISTANCES_PANEL_TOOLTIP, arg, sicknessForDisease.Name));
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06005688 RID: 22152 RVA: 0x001F8C38 File Offset: 0x001F6E38
	private bool CreateDiseaseInfo()
	{
		if (this.selectedTarget.GetComponent<PrimaryElement>() != null)
		{
			return this.CreateDiseaseInfo_PrimaryElement();
		}
		CellSelectionObject component = this.selectedTarget.GetComponent<CellSelectionObject>();
		return component != null && this.CreateDiseaseInfo_CellSelectionObject(component);
	}

	// Token: 0x06005689 RID: 22153 RVA: 0x001F8C7D File Offset: 0x001F6E7D
	private string GetFormattedHalfLife(float hl)
	{
		return this.GetFormattedGrowthRate(Disease.HalfLifeToGrowthRate(hl, 600f));
	}

	// Token: 0x0600568A RID: 22154 RVA: 0x001F8C90 File Offset: 0x001F6E90
	private string GetFormattedGrowthRate(float rate)
	{
		if (rate < 1f)
		{
			return string.Format(UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT, GameUtil.GetFormattedPercent(100f * (1f - rate), GameUtil.TimeSlice.None), UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT_TOOLTIP);
		}
		if (rate > 1f)
		{
			return string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT, GameUtil.GetFormattedPercent(100f * (rate - 1f), GameUtil.TimeSlice.None), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT_TOOLTIP);
		}
		return string.Format(UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT, UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT_TOOLTIP);
	}

	// Token: 0x0600568B RID: 22155 RVA: 0x001F8D14 File Offset: 0x001F6F14
	private string GetFormattedGrowthEntry(string name, float halfLife, string dyingFormat, string growingFormat, string neutralFormat)
	{
		string format;
		if (halfLife == float.PositiveInfinity)
		{
			format = neutralFormat;
		}
		else if (halfLife > 0f)
		{
			format = dyingFormat;
		}
		else
		{
			format = growingFormat;
		}
		return string.Format(format, name, this.GetFormattedHalfLife(halfLife));
	}

	// Token: 0x0600568C RID: 22156 RVA: 0x001F8D50 File Offset: 0x001F6F50
	private void BuildFactorsStrings(int diseaseCount, ushort elementIdx, int environmentCell, float environmentMass, float temperature, HashSet<Tag> tags, Disease disease, bool isCell = false)
	{
		this.currentGermsPanel.SetTitle(string.Format(UI.DETAILTABS.DISEASE.CURRENT_GERMS, disease.Name.ToUpper()));
		this.currentGermsPanel.SetLabel("currentgerms", string.Format(UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT, disease.Name, GameUtil.GetFormattedDiseaseAmount(diseaseCount, GameUtil.TimeSlice.None)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT_TOOLTIP, GameUtil.GetFormattedDiseaseAmount(diseaseCount, GameUtil.TimeSlice.None)));
		Element e = ElementLoader.elements[(int)elementIdx];
		CompositeGrowthRule growthRuleForElement = disease.GetGrowthRuleForElement(e);
		float tags_multiplier_base = 1f;
		if (tags != null && tags.Count > 0)
		{
			tags_multiplier_base = disease.GetGrowthRateForTags(tags, (float)diseaseCount > growthRuleForElement.maxCountPerKG * environmentMass);
		}
		float num = DiseaseContainers.CalculateDelta(diseaseCount, elementIdx, environmentMass, environmentCell, temperature, tags_multiplier_base, disease, 1f, Sim.IsRadiationEnabled());
		this.currentGermsPanel.SetLabel("finaldelta", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE, GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.PerSecond, "F0")), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE_TOOLTIP, GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.PerSecond, "F0")));
		float num2 = Disease.GrowthRateToHalfLife(1f - num / (float)diseaseCount);
		if (num2 > 0f)
		{
			this.currentGermsPanel.SetLabel("finalhalflife", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG, GameUtil.GetFormattedCycles(num2, "F1", false)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG_TOOLTIP, GameUtil.GetFormattedCycles(num2, "F1", false)));
		}
		else if (num2 < 0f)
		{
			this.currentGermsPanel.SetLabel("finalhalflife", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS, GameUtil.GetFormattedCycles(-num2, "F1", false)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS_TOOLTIP, GameUtil.GetFormattedCycles(num2, "F1", false)));
		}
		else
		{
			this.currentGermsPanel.SetLabel("finalhalflife", UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL_TOOLTIP);
		}
		this.currentGermsPanel.SetLabel("factors", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TITLE, Array.Empty<object>()), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TOOLTIP);
		bool flag = false;
		if ((float)diseaseCount < growthRuleForElement.minCountPerKG * environmentMass)
		{
			this.currentGermsPanel.SetLabel("critical_status", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TITLE, this.GetFormattedGrowthRate(-growthRuleForElement.underPopulationDeathRate)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TOOLTIP, GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement.minCountPerKG * environmentMass), GameUtil.TimeSlice.None), GameUtil.GetFormattedMass(environmentMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), growthRuleForElement.minCountPerKG));
			flag = true;
		}
		else if ((float)diseaseCount > growthRuleForElement.maxCountPerKG * environmentMass)
		{
			this.currentGermsPanel.SetLabel("critical_status", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TITLE, this.GetFormattedHalfLife(growthRuleForElement.overPopulationHalfLife)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TOOLTIP, GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement.maxCountPerKG * environmentMass), GameUtil.TimeSlice.None), GameUtil.GetFormattedMass(environmentMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), growthRuleForElement.maxCountPerKG));
			flag = true;
		}
		if (!flag)
		{
			this.currentGermsPanel.SetLabel("substrate", this.GetFormattedGrowthEntry(growthRuleForElement.Name(), growthRuleForElement.populationHalfLife, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL), this.GetFormattedGrowthEntry(growthRuleForElement.Name(), growthRuleForElement.populationHalfLife, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL_TOOLTIP));
		}
		int num3 = 0;
		if (tags != null)
		{
			foreach (Tag t in tags)
			{
				TagGrowthRule growthRuleForTag = disease.GetGrowthRuleForTag(t);
				if (growthRuleForTag != null)
				{
					this.currentGermsPanel.SetLabel("tag_" + num3.ToString(), this.GetFormattedGrowthEntry(growthRuleForTag.Name(), growthRuleForTag.populationHalfLife.Value, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL), this.GetFormattedGrowthEntry(growthRuleForTag.Name(), growthRuleForTag.populationHalfLife.Value, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL_TOOLTIP));
				}
				num3++;
			}
		}
		if (Grid.IsValidCell(environmentCell))
		{
			if (!isCell)
			{
				CompositeExposureRule exposureRuleForElement = disease.GetExposureRuleForElement(Grid.Element[environmentCell]);
				if (exposureRuleForElement != null && exposureRuleForElement.populationHalfLife != float.PositiveInfinity)
				{
					if (exposureRuleForElement.GetHalfLifeForCount(diseaseCount) > 0f)
					{
						this.currentGermsPanel.SetLabel("environment", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.TITLE, exposureRuleForElement.Name(), this.GetFormattedHalfLife(exposureRuleForElement.GetHalfLifeForCount(diseaseCount))), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.DIE_TOOLTIP);
					}
					else
					{
						this.currentGermsPanel.SetLabel("environment", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.TITLE, exposureRuleForElement.Name(), this.GetFormattedHalfLife(exposureRuleForElement.GetHalfLifeForCount(diseaseCount))), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.GROW_TOOLTIP);
					}
				}
			}
			if (Sim.IsRadiationEnabled())
			{
				float num4 = Grid.Radiation[environmentCell];
				if (num4 > 0f)
				{
					float num5 = disease.radiationKillRate * num4;
					float hl = (float)diseaseCount * 0.5f / num5;
					this.currentGermsPanel.SetLabel("radiation", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RADIATION.TITLE, Mathf.RoundToInt(num4), this.GetFormattedHalfLife(hl)), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RADIATION.DIE_TOOLTIP);
				}
			}
		}
		float num6 = disease.CalculateTemperatureHalfLife(temperature);
		if (num6 != float.PositiveInfinity)
		{
			if (num6 > 0f)
			{
				this.currentGermsPanel.SetLabel("temperature", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE, GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), this.GetFormattedHalfLife(num6)), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.DIE_TOOLTIP);
				return;
			}
			this.currentGermsPanel.SetLabel("temperature", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE, GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), this.GetFormattedHalfLife(num6)), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.GROW_TOOLTIP);
		}
	}

	// Token: 0x0600568D RID: 22157 RVA: 0x001F93A0 File Offset: 0x001F75A0
	private bool CreateDiseaseInfo_PrimaryElement()
	{
		if (this.selectedTarget == null)
		{
			return false;
		}
		PrimaryElement component = this.selectedTarget.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return false;
		}
		if (component.DiseaseIdx != 255 && component.DiseaseCount > 0)
		{
			Disease disease = Db.Get().Diseases[(int)component.DiseaseIdx];
			int environmentCell = Grid.PosToCell(component.transform.GetPosition());
			KPrefabID component2 = component.GetComponent<KPrefabID>();
			this.BuildFactorsStrings(component.DiseaseCount, component.Element.idx, environmentCell, component.Mass, component.Temperature, component2.Tags, disease, false);
			return true;
		}
		return false;
	}

	// Token: 0x0600568E RID: 22158 RVA: 0x001F9448 File Offset: 0x001F7648
	private bool CreateDiseaseInfo_CellSelectionObject(CellSelectionObject cso)
	{
		if (cso.diseaseIdx != 255 && cso.diseaseCount > 0)
		{
			Disease disease = Db.Get().Diseases[(int)cso.diseaseIdx];
			this.BuildFactorsStrings(cso.diseaseCount, cso.element.idx, cso.SelectedCell, cso.Mass, cso.temperature, null, disease, true);
			return true;
		}
		return false;
	}

	// Token: 0x04003A34 RID: 14900
	private CollapsibleDetailContentPanel infectionPanel;

	// Token: 0x04003A35 RID: 14901
	private CollapsibleDetailContentPanel immuneSystemPanel;

	// Token: 0x04003A36 RID: 14902
	private CollapsibleDetailContentPanel diseaseSourcePanel;

	// Token: 0x04003A37 RID: 14903
	private CollapsibleDetailContentPanel currentGermsPanel;

	// Token: 0x04003A38 RID: 14904
	private CollapsibleDetailContentPanel infoPanel;

	// Token: 0x04003A39 RID: 14905
	private static readonly EventSystem.IntraObjectHandler<DiseaseInfoScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<DiseaseInfoScreen>(delegate(DiseaseInfoScreen component, object data)
	{
		component.OnRefreshData(data);
	});
}
