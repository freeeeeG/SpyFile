using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B96 RID: 2966
[AddComponentMenu("KMonoBehaviour/scripts/MinionVitalsPanel")]
public class MinionVitalsPanel : KMonoBehaviour
{
	// Token: 0x06005C3B RID: 23611 RVA: 0x0021C9BC File Offset: 0x0021ABBC
	public void Init()
	{
		this.AddAmountLine(Db.Get().Amounts.HitPoints, null);
		this.AddAttributeLine(Db.Get().CritterAttributes.Happiness, null);
		this.AddAmountLine(Db.Get().Amounts.Wildness, null);
		this.AddAmountLine(Db.Get().Amounts.Incubation, null);
		this.AddAmountLine(Db.Get().Amounts.Viability, null);
		this.AddAmountLine(Db.Get().Amounts.PowerCharge, null);
		this.AddAmountLine(Db.Get().Amounts.Fertility, null);
		this.AddAmountLine(Db.Get().Amounts.Beckoning, null);
		this.AddAmountLine(Db.Get().Amounts.Age, null);
		this.AddAmountLine(Db.Get().Amounts.Stress, null);
		this.AddAttributeLine(Db.Get().Attributes.QualityOfLife, null);
		this.AddAmountLine(Db.Get().Amounts.Bladder, null);
		this.AddAmountLine(Db.Get().Amounts.Breath, null);
		this.AddAmountLine(Db.Get().Amounts.Stamina, null);
		this.AddAmountLine(Db.Get().Amounts.Calories, null);
		this.AddAmountLine(Db.Get().Amounts.ScaleGrowth, null);
		this.AddAmountLine(Db.Get().Amounts.MilkProduction, null);
		this.AddAmountLine(Db.Get().Amounts.ElementGrowth, null);
		this.AddAmountLine(Db.Get().Amounts.Temperature, null);
		this.AddAmountLine(Db.Get().Amounts.Decor, null);
		this.AddAmountLine(Db.Get().Amounts.InternalBattery, null);
		this.AddAmountLine(Db.Get().Amounts.InternalChemicalBattery, null);
		if (DlcManager.FeatureRadiationEnabled())
		{
			this.AddAmountLine(Db.Get().Amounts.RadiationBalance, null);
		}
		this.AddCheckboxLine(Db.Get().Amounts.AirPressure, this.conditionsContainerNormal, (GameObject go) => this.GetAirPressureLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<PressureVulnerable>() != null && go.GetComponent<PressureVulnerable>().pressure_sensitive)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_pressure(go), (GameObject go) => this.GetAirPressureTooltip(go));
		this.AddCheckboxLine(null, this.conditionsContainerNormal, (GameObject go) => this.GetAtmosphereLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<PressureVulnerable>() != null && go.GetComponent<PressureVulnerable>().safe_atmospheres.Count > 0)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_atmosphere(go), (GameObject go) => this.GetAtmosphereTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Temperature, this.conditionsContainerNormal, (GameObject go) => this.GetInternalTemperatureLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<TemperatureVulnerable>() != null)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_temperature(go), (GameObject go) => this.GetInternalTemperatureTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Fertilization, this.conditionsContainerAdditional, (GameObject go) => this.GetFertilizationLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<ReceptacleMonitor>() == null)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
			}
			if (go.GetComponent<ReceptacleMonitor>().Replanted)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
		}, (GameObject go) => this.check_fertilizer(go), (GameObject go) => this.GetFertilizationTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Irrigation, this.conditionsContainerAdditional, (GameObject go) => this.GetIrrigationLabel(go), delegate(GameObject go)
		{
			ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
			if (!(component != null) || !component.Replanted)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
		}, (GameObject go) => this.check_irrigation(go), (GameObject go) => this.GetIrrigationTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Illumination, this.conditionsContainerNormal, (GameObject go) => this.GetIlluminationLabel(go), (GameObject go) => MinionVitalsPanel.CheckboxLineDisplayType.Normal, (GameObject go) => this.check_illumination(go), (GameObject go) => this.GetIlluminationTooltip(go));
		this.AddCheckboxLine(null, this.conditionsContainerNormal, (GameObject go) => this.GetRadiationLabel(go), delegate(GameObject go)
		{
			AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
			if (attributeInstance != null && attributeInstance.GetTotalValue() > 0f)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_radiation(go), (GameObject go) => this.GetRadiationTooltip(go));
	}

	// Token: 0x06005C3C RID: 23612 RVA: 0x0021CE40 File Offset: 0x0021B040
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x06005C3D RID: 23613 RVA: 0x0021CE54 File Offset: 0x0021B054
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06005C3E RID: 23614 RVA: 0x0021CE68 File Offset: 0x0021B068
	private void AddAmountLine(Amount amount, Func<AmountInstance, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, base.gameObject, false);
		gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite(amount.uiSprite);
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.AmountLine item = default(MinionVitalsPanel.AmountLine);
		item.amount = amount;
		item.go = gameObject;
		item.locText = gameObject.GetComponentInChildren<LocText>();
		item.toolTip = gameObject.GetComponentInChildren<ToolTip>();
		item.imageToggle = gameObject.GetComponentInChildren<ValueTrendImageToggle>();
		item.toolTipFunc = ((tooltip_func != null) ? tooltip_func : new Func<AmountInstance, string>(amount.GetTooltip));
		this.amountsLines.Add(item);
	}

	// Token: 0x06005C3F RID: 23615 RVA: 0x0021CF1C File Offset: 0x0021B11C
	private void AddAttributeLine(Klei.AI.Attribute attribute, Func<AttributeInstance, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, base.gameObject, false);
		gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite(attribute.uiSprite);
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.AttributeLine item = default(MinionVitalsPanel.AttributeLine);
		item.attribute = attribute;
		item.go = gameObject;
		item.locText = gameObject.GetComponentInChildren<LocText>();
		item.toolTip = gameObject.GetComponentInChildren<ToolTip>();
		gameObject.GetComponentInChildren<ValueTrendImageToggle>().gameObject.SetActive(false);
		item.toolTipFunc = ((tooltip_func != null) ? tooltip_func : new Func<AttributeInstance, string>(attribute.GetTooltip));
		this.attributesLines.Add(item);
	}

	// Token: 0x06005C40 RID: 23616 RVA: 0x0021CFD4 File Offset: 0x0021B1D4
	private void AddCheckboxLine(Amount amount, Transform parentContainer, Func<GameObject, string> label_text_func, Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition, Func<GameObject, bool> checkbox_value_func, Func<GameObject, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.CheckboxLinePrefab, base.gameObject, false);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.CheckboxLine checkboxLine = default(MinionVitalsPanel.CheckboxLine);
		checkboxLine.go = gameObject;
		checkboxLine.parentContainer = parentContainer;
		checkboxLine.amount = amount;
		checkboxLine.locText = (component.GetReference("Label") as LocText);
		checkboxLine.get_value = checkbox_value_func;
		checkboxLine.display_condition = display_condition;
		checkboxLine.label_text_func = label_text_func;
		checkboxLine.go.name = "Checkbox_";
		if (amount != null)
		{
			GameObject go = checkboxLine.go;
			go.name += amount.Name;
		}
		else
		{
			GameObject go2 = checkboxLine.go;
			go2.name += "Unnamed";
		}
		if (tooltip_func != null)
		{
			checkboxLine.tooltip = tooltip_func;
			ToolTip tt = checkboxLine.go.GetComponent<ToolTip>();
			tt.refreshWhileHovering = true;
			tt.OnToolTip = delegate()
			{
				tt.ClearMultiStringTooltip();
				tt.AddMultiStringTooltip(tooltip_func(this.selectedEntity), null);
				return "";
			};
		}
		this.checkboxLines.Add(checkboxLine);
	}

	// Token: 0x06005C41 RID: 23617 RVA: 0x0021D118 File Offset: 0x0021B318
	public void Refresh()
	{
		if (this.selectedEntity == null)
		{
			return;
		}
		if (this.selectedEntity.gameObject == null)
		{
			return;
		}
		Amounts amounts = this.selectedEntity.GetAmounts();
		Attributes attributes = this.selectedEntity.GetAttributes();
		if (amounts == null || attributes == null)
		{
			return;
		}
		WiltCondition component = this.selectedEntity.GetComponent<WiltCondition>();
		if (component == null)
		{
			this.conditionsContainerNormal.gameObject.SetActive(false);
			this.conditionsContainerAdditional.gameObject.SetActive(false);
			foreach (MinionVitalsPanel.AmountLine amountLine in this.amountsLines)
			{
				bool flag = amountLine.TryUpdate(amounts);
				if (amountLine.go.activeSelf != flag)
				{
					amountLine.go.SetActive(flag);
				}
			}
			foreach (MinionVitalsPanel.AttributeLine attributeLine in this.attributesLines)
			{
				bool flag2 = attributeLine.TryUpdate(attributes);
				if (attributeLine.go.activeSelf != flag2)
				{
					attributeLine.go.SetActive(flag2);
				}
			}
		}
		bool flag3 = false;
		for (int i = 0; i < this.checkboxLines.Count; i++)
		{
			MinionVitalsPanel.CheckboxLine checkboxLine = this.checkboxLines[i];
			MinionVitalsPanel.CheckboxLineDisplayType checkboxLineDisplayType = MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
			if (this.checkboxLines[i].amount != null)
			{
				for (int j = 0; j < amounts.Count; j++)
				{
					AmountInstance amountInstance = amounts[j];
					if (checkboxLine.amount == amountInstance.amount)
					{
						checkboxLineDisplayType = checkboxLine.display_condition(this.selectedEntity.gameObject);
						break;
					}
				}
			}
			else
			{
				checkboxLineDisplayType = checkboxLine.display_condition(this.selectedEntity.gameObject);
			}
			if (checkboxLineDisplayType != MinionVitalsPanel.CheckboxLineDisplayType.Hidden)
			{
				checkboxLine.locText.SetText(checkboxLine.label_text_func(this.selectedEntity.gameObject));
				if (!checkboxLine.go.activeSelf)
				{
					checkboxLine.go.SetActive(true);
				}
				GameObject gameObject = checkboxLine.go.GetComponent<HierarchyReferences>().GetReference("Check").gameObject;
				gameObject.SetActive(checkboxLine.get_value(this.selectedEntity.gameObject));
				if (checkboxLine.go.transform.parent != checkboxLine.parentContainer)
				{
					checkboxLine.go.transform.SetParent(checkboxLine.parentContainer);
					checkboxLine.go.transform.localScale = Vector3.one;
				}
				if (checkboxLine.parentContainer == this.conditionsContainerAdditional)
				{
					flag3 = true;
				}
				if (checkboxLineDisplayType == MinionVitalsPanel.CheckboxLineDisplayType.Normal)
				{
					if (checkboxLine.get_value(this.selectedEntity.gameObject))
					{
						checkboxLine.locText.color = Color.black;
						gameObject.transform.parent.GetComponent<Image>().color = Color.black;
					}
					else
					{
						Color color = new Color(0.99215686f, 0f, 0.101960786f);
						checkboxLine.locText.color = color;
						gameObject.transform.parent.GetComponent<Image>().color = color;
					}
				}
				else
				{
					checkboxLine.locText.color = Color.grey;
					gameObject.transform.parent.GetComponent<Image>().color = Color.grey;
				}
			}
			else if (checkboxLine.go.activeSelf)
			{
				checkboxLine.go.SetActive(false);
			}
		}
		if (component != null)
		{
			UnityEngine.Object component2 = component.GetComponent<Growing>();
			bool flag4 = component.HasTag(GameTags.Decoration);
			this.conditionsContainerNormal.gameObject.SetActive(true);
			this.conditionsContainerAdditional.gameObject.SetActive(!flag4);
			if (component2 == null)
			{
				float num = 1f;
				LocText reference = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference.text = "";
				reference.text = (flag4 ? string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_DECOR.BASE, Array.Empty<object>()) : string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.BASE, Util.FormatTwoDecimalPlace(num * 0.25f * 100f)));
				reference.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.TOOLTIP, Array.Empty<object>()));
				LocText reference2 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				ReceptacleMonitor component3 = this.selectedEntity.GetComponent<ReceptacleMonitor>();
				reference2.color = ((component3 == null || component3.Replanted) ? Color.black : Color.grey);
				reference2.text = string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.BASE, Util.FormatTwoDecimalPlace(num * 100f));
				reference2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.TOOLTIP, Array.Empty<object>()));
			}
			else
			{
				LocText reference3 = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference3.text = "";
				reference3.text = string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.BASE, GameUtil.GetFormattedCycles(component.GetComponent<Growing>().WildGrowthTime(), "F1", false));
				reference3.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.TOOLTIP, GameUtil.GetFormattedCycles(component.GetComponent<Growing>().WildGrowthTime(), "F1", false)));
				LocText reference4 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference4.color = (this.selectedEntity.GetComponent<ReceptacleMonitor>().Replanted ? Color.black : Color.grey);
				reference4.text = "";
				reference4.text = (flag3 ? string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.BASE, GameUtil.GetFormattedCycles(component.GetComponent<Growing>().DomesticGrowthTime(), "F1", false)) : string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.DOMESTIC.BASE, GameUtil.GetFormattedCycles(component.GetComponent<Growing>().DomesticGrowthTime(), "F1", false)));
				reference4.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.TOOLTIP, GameUtil.GetFormattedCycles(component.GetComponent<Growing>().DomesticGrowthTime(), "F1", false)));
			}
			foreach (MinionVitalsPanel.AmountLine amountLine2 in this.amountsLines)
			{
				amountLine2.go.SetActive(false);
			}
			foreach (MinionVitalsPanel.AttributeLine attributeLine2 in this.attributesLines)
			{
				attributeLine2.go.SetActive(false);
			}
		}
	}

	// Token: 0x06005C42 RID: 23618 RVA: 0x0021D80C File Offset: 0x0021BA0C
	private string GetAirPressureTooltip(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		if (component == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_PRESSURE.text.Replace("{pressure}", GameUtil.GetFormattedMass(component.GetExternalPressure(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06005C43 RID: 23619 RVA: 0x0021D858 File Offset: 0x0021BA58
	private string GetInternalTemperatureTooltip(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		if (component == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_TEMPERATURE.text.Replace("{temperature}", GameUtil.GetFormattedTemperature(component.InternalTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x06005C44 RID: 23620 RVA: 0x0021D8A0 File Offset: 0x0021BAA0
	private string GetFertilizationTooltip(GameObject go)
	{
		FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
		if (smi == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_FERTILIZER.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06005C45 RID: 23621 RVA: 0x0021D8E4 File Offset: 0x0021BAE4
	private string GetIrrigationTooltip(GameObject go)
	{
		IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
		if (smi == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_IRRIGATION.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06005C46 RID: 23622 RVA: 0x0021D928 File Offset: 0x0021BB28
	private string GetIlluminationTooltip(GameObject go)
	{
		IlluminationVulnerable component = go.GetComponent<IlluminationVulnerable>();
		if (component == null)
		{
			return "";
		}
		if ((component.prefersDarkness && component.IsComfortable()) || (!component.prefersDarkness && !component.IsComfortable()))
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_DARK;
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_LIGHT;
	}

	// Token: 0x06005C47 RID: 23623 RVA: 0x0021D980 File Offset: 0x0021BB80
	private string GetRadiationTooltip(GameObject go)
	{
		int num = Grid.PosToCell(go);
		float rads = Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f;
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		AttributeInstance attributeInstance2 = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
		MutantPlant component = go.GetComponent<MutantPlant>();
		bool flag = component != null && component.IsOriginal;
		string text;
		if (attributeInstance.GetTotalValue() == 0f)
		{
			text = UI.TOOLTIPS.VITALS_CHECKBOX_RADIATION_NO_MIN.Replace("{rads}", GameUtil.GetFormattedRads(rads, GameUtil.TimeSlice.None)).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		else
		{
			text = UI.TOOLTIPS.VITALS_CHECKBOX_RADIATION.Replace("{rads}", GameUtil.GetFormattedRads(rads, GameUtil.TimeSlice.None)).Replace("{minRads}", attributeInstance.GetFormattedValue()).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		if (flag)
		{
			text += UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_SEED_TOOLTIP;
		}
		return text;
	}

	// Token: 0x06005C48 RID: 23624 RVA: 0x0021DA88 File Offset: 0x0021BC88
	private string GetReceptacleTooltip(GameObject go)
	{
		ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
		if (component == null)
		{
			return "";
		}
		if (component.HasOperationalReceptacle())
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL;
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL;
	}

	// Token: 0x06005C49 RID: 23625 RVA: 0x0021DAC8 File Offset: 0x0021BCC8
	private string GetAtmosphereTooltip(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		if (component != null && component.currentAtmoElement != null)
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE.text.Replace("{element}", component.currentAtmoElement.name);
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE;
	}

	// Token: 0x06005C4A RID: 23626 RVA: 0x0021DB18 File Offset: 0x0021BD18
	private string GetAirPressureLabel(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return string.Concat(new string[]
		{
			Db.Get().Amounts.AirPressure.Name,
			"\n    • ",
			GameUtil.GetFormattedMass(component.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Gram, false, "{0:0.#}"),
			" - ",
			GameUtil.GetFormattedMass(component.pressureWarning_High, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}")
		});
	}

	// Token: 0x06005C4B RID: 23627 RVA: 0x0021DB8C File Offset: 0x0021BD8C
	private string GetInternalTemperatureLabel(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		return string.Concat(new string[]
		{
			Db.Get().Amounts.Temperature.Name,
			"\n    • ",
			GameUtil.GetFormattedTemperature(component.TemperatureWarningLow, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false),
			" - ",
			GameUtil.GetFormattedTemperature(component.TemperatureWarningHigh, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)
		});
	}

	// Token: 0x06005C4C RID: 23628 RVA: 0x0021DBF8 File Offset: 0x0021BDF8
	private string GetFertilizationLabel(GameObject go)
	{
		StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.GenericInstance smi = go.GetSMI<FertilizationMonitor.Instance>();
		string text = Db.Get().Amounts.Fertilization.Name;
		float totalValue = go.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
		foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in smi.def.consumedElements)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n    • ",
				ElementLoader.GetElement(consumeInfo.tag).name,
				" ",
				GameUtil.GetFormattedMass(consumeInfo.massConsumptionRate * totalValue, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
			});
		}
		return text;
	}

	// Token: 0x06005C4D RID: 23629 RVA: 0x0021DCB0 File Offset: 0x0021BEB0
	private string GetIrrigationLabel(GameObject go)
	{
		StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.GenericInstance smi = go.GetSMI<IrrigationMonitor.Instance>();
		string text = Db.Get().Amounts.Irrigation.Name;
		float totalValue = go.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
		foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in smi.def.consumedElements)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n    • ",
				ElementLoader.GetElement(consumeInfo.tag).name,
				": ",
				GameUtil.GetFormattedMass(consumeInfo.massConsumptionRate * totalValue, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
			});
		}
		return text;
	}

	// Token: 0x06005C4E RID: 23630 RVA: 0x0021DD68 File Offset: 0x0021BF68
	private string GetIlluminationLabel(GameObject go)
	{
		IlluminationVulnerable component = go.GetComponent<IlluminationVulnerable>();
		return Db.Get().Amounts.Illumination.Name + "\n    • " + (component.prefersDarkness ? UI.GAMEOBJECTEFFECTS.DARKNESS.ToString() : GameUtil.GetFormattedLux(component.LightIntensityThreshold));
	}

	// Token: 0x06005C4F RID: 23631 RVA: 0x0021DDBC File Offset: 0x0021BFBC
	private string GetAtmosphereLabel(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		string text = UI.VITALSSCREEN.ATMOSPHERE_CONDITION;
		foreach (Element element in component.safe_atmospheres)
		{
			text = text + "\n    • " + element.name;
		}
		return text;
	}

	// Token: 0x06005C50 RID: 23632 RVA: 0x0021DE2C File Offset: 0x0021C02C
	private string GetRadiationLabel(GameObject go)
	{
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		AttributeInstance attributeInstance2 = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
		if (attributeInstance.GetTotalValue() == 0f)
		{
			return UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION + "\n    • " + UI.GAMEOBJECTEFFECTS.AMBIENT_NO_MIN_RADIATION_FMT.Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		return UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION + "\n    • " + UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION_FMT.Replace("{minRads}", attributeInstance.GetFormattedValue()).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
	}

	// Token: 0x06005C51 RID: 23633 RVA: 0x0021DEE0 File Offset: 0x0021C0E0
	private bool check_pressure(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return !(component != null) || component.ExternalPressureState == PressureVulnerable.PressureState.Normal;
	}

	// Token: 0x06005C52 RID: 23634 RVA: 0x0021DF08 File Offset: 0x0021C108
	private bool check_temperature(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		return !(component != null) || component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
	}

	// Token: 0x06005C53 RID: 23635 RVA: 0x0021DF30 File Offset: 0x0021C130
	private bool check_irrigation(GameObject go)
	{
		IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
		return smi == null || (!smi.IsInsideState(smi.sm.replanted.starved) && !smi.IsInsideState(smi.sm.wild));
	}

	// Token: 0x06005C54 RID: 23636 RVA: 0x0021DF78 File Offset: 0x0021C178
	private bool check_illumination(GameObject go)
	{
		IlluminationVulnerable component = go.GetComponent<IlluminationVulnerable>();
		return !(component != null) || component.IsComfortable();
	}

	// Token: 0x06005C55 RID: 23637 RVA: 0x0021DFA0 File Offset: 0x0021C1A0
	private bool check_radiation(GameObject go)
	{
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		if (attributeInstance != null && attributeInstance.GetTotalValue() != 0f)
		{
			int num = Grid.PosToCell(go);
			return (Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f) >= attributeInstance.GetTotalValue();
		}
		return true;
	}

	// Token: 0x06005C56 RID: 23638 RVA: 0x0021E008 File Offset: 0x0021C208
	private bool check_receptacle(GameObject go)
	{
		ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
		return !(component == null) && component.HasOperationalReceptacle();
	}

	// Token: 0x06005C57 RID: 23639 RVA: 0x0021E030 File Offset: 0x0021C230
	private bool check_fertilizer(GameObject go)
	{
		FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
		return smi == null || smi.sm.hasCorrectFertilizer.Get(smi);
	}

	// Token: 0x06005C58 RID: 23640 RVA: 0x0021E05C File Offset: 0x0021C25C
	private bool check_atmosphere(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return !(component != null) || component.testAreaElementSafe;
	}

	// Token: 0x04003E29 RID: 15913
	public GameObject LineItemPrefab;

	// Token: 0x04003E2A RID: 15914
	public GameObject CheckboxLinePrefab;

	// Token: 0x04003E2B RID: 15915
	public GameObject selectedEntity;

	// Token: 0x04003E2C RID: 15916
	public List<MinionVitalsPanel.AmountLine> amountsLines = new List<MinionVitalsPanel.AmountLine>();

	// Token: 0x04003E2D RID: 15917
	public List<MinionVitalsPanel.AttributeLine> attributesLines = new List<MinionVitalsPanel.AttributeLine>();

	// Token: 0x04003E2E RID: 15918
	public List<MinionVitalsPanel.CheckboxLine> checkboxLines = new List<MinionVitalsPanel.CheckboxLine>();

	// Token: 0x04003E2F RID: 15919
	public Transform conditionsContainerNormal;

	// Token: 0x04003E30 RID: 15920
	public Transform conditionsContainerAdditional;

	// Token: 0x02001AC0 RID: 6848
	[DebuggerDisplay("{amount.Name}")]
	public struct AmountLine
	{
		// Token: 0x060097F8 RID: 38904 RVA: 0x003403EC File Offset: 0x0033E5EC
		public bool TryUpdate(Amounts amounts)
		{
			foreach (AmountInstance amountInstance in amounts)
			{
				if (this.amount == amountInstance.amount && !amountInstance.hide)
				{
					this.locText.SetText(this.amount.GetDescription(amountInstance));
					this.toolTip.toolTip = this.toolTipFunc(amountInstance);
					this.imageToggle.SetValue(amountInstance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04007A4A RID: 31306
		public Amount amount;

		// Token: 0x04007A4B RID: 31307
		public GameObject go;

		// Token: 0x04007A4C RID: 31308
		public ValueTrendImageToggle imageToggle;

		// Token: 0x04007A4D RID: 31309
		public LocText locText;

		// Token: 0x04007A4E RID: 31310
		public ToolTip toolTip;

		// Token: 0x04007A4F RID: 31311
		public Func<AmountInstance, string> toolTipFunc;
	}

	// Token: 0x02001AC1 RID: 6849
	[DebuggerDisplay("{attribute.Name}")]
	public struct AttributeLine
	{
		// Token: 0x060097F9 RID: 38905 RVA: 0x00340484 File Offset: 0x0033E684
		public bool TryUpdate(Attributes attributes)
		{
			foreach (AttributeInstance attributeInstance in attributes)
			{
				if (this.attribute == attributeInstance.modifier && !attributeInstance.hide)
				{
					this.locText.SetText(this.attribute.GetDescription(attributeInstance));
					this.toolTip.toolTip = this.toolTipFunc(attributeInstance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04007A50 RID: 31312
		public Klei.AI.Attribute attribute;

		// Token: 0x04007A51 RID: 31313
		public GameObject go;

		// Token: 0x04007A52 RID: 31314
		public LocText locText;

		// Token: 0x04007A53 RID: 31315
		public ToolTip toolTip;

		// Token: 0x04007A54 RID: 31316
		public Func<AttributeInstance, string> toolTipFunc;
	}

	// Token: 0x02001AC2 RID: 6850
	public struct CheckboxLine
	{
		// Token: 0x04007A55 RID: 31317
		public Amount amount;

		// Token: 0x04007A56 RID: 31318
		public GameObject go;

		// Token: 0x04007A57 RID: 31319
		public LocText locText;

		// Token: 0x04007A58 RID: 31320
		public Func<GameObject, string> tooltip;

		// Token: 0x04007A59 RID: 31321
		public Func<GameObject, bool> get_value;

		// Token: 0x04007A5A RID: 31322
		public Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition;

		// Token: 0x04007A5B RID: 31323
		public Func<GameObject, string> label_text_func;

		// Token: 0x04007A5C RID: 31324
		public Transform parentContainer;
	}

	// Token: 0x02001AC3 RID: 6851
	public enum CheckboxLineDisplayType
	{
		// Token: 0x04007A5E RID: 31326
		Normal,
		// Token: 0x04007A5F RID: 31327
		Diminished,
		// Token: 0x04007A60 RID: 31328
		Hidden
	}
}
