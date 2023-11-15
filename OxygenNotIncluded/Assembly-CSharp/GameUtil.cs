﻿using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007D4 RID: 2004
public static class GameUtil
{
	// Token: 0x060037E9 RID: 14313 RVA: 0x001358F0 File Offset: 0x00133AF0
	public static string GetTemperatureUnitSuffix()
	{
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		string result;
		if (temperatureUnit != GameUtil.TemperatureUnit.Celsius)
		{
			if (temperatureUnit != GameUtil.TemperatureUnit.Fahrenheit)
			{
				result = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
			}
			else
			{
				result = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
			}
		}
		else
		{
			result = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
		}
		return result;
	}

	// Token: 0x060037EA RID: 14314 RVA: 0x00135932 File Offset: 0x00133B32
	private static string AddTemperatureUnitSuffix(string text)
	{
		return text + GameUtil.GetTemperatureUnitSuffix();
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x0013593F File Offset: 0x00133B3F
	public static float GetTemperatureConvertedFromKelvin(float temperature, GameUtil.TemperatureUnit targetUnit)
	{
		if (targetUnit == GameUtil.TemperatureUnit.Celsius)
		{
			return temperature - 273.15f;
		}
		if (targetUnit != GameUtil.TemperatureUnit.Fahrenheit)
		{
			return temperature;
		}
		return temperature * 1.8f - 459.67f;
	}

	// Token: 0x060037EC RID: 14316 RVA: 0x00135964 File Offset: 0x00133B64
	public static float GetConvertedTemperature(float temperature, bool roundOutput = false)
	{
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		if (temperatureUnit != GameUtil.TemperatureUnit.Celsius)
		{
			if (temperatureUnit != GameUtil.TemperatureUnit.Fahrenheit)
			{
				if (!roundOutput)
				{
					return temperature;
				}
				return Mathf.Round(temperature);
			}
			else
			{
				float num = temperature * 1.8f - 459.67f;
				if (!roundOutput)
				{
					return num;
				}
				return Mathf.Round(num);
			}
		}
		else
		{
			float num = temperature - 273.15f;
			if (!roundOutput)
			{
				return num;
			}
			return Mathf.Round(num);
		}
	}

	// Token: 0x060037ED RID: 14317 RVA: 0x001359BF File Offset: 0x00133BBF
	public static float GetTemperatureConvertedToKelvin(float temperature, GameUtil.TemperatureUnit fromUnit)
	{
		if (fromUnit == GameUtil.TemperatureUnit.Celsius)
		{
			return temperature + 273.15f;
		}
		if (fromUnit != GameUtil.TemperatureUnit.Fahrenheit)
		{
			return temperature;
		}
		return (temperature + 459.67f) * 5f / 9f;
	}

	// Token: 0x060037EE RID: 14318 RVA: 0x001359E8 File Offset: 0x00133BE8
	public static float GetTemperatureConvertedToKelvin(float temperature)
	{
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		if (temperatureUnit == GameUtil.TemperatureUnit.Celsius)
		{
			return temperature + 273.15f;
		}
		if (temperatureUnit != GameUtil.TemperatureUnit.Fahrenheit)
		{
			return temperature;
		}
		return (temperature + 459.67f) * 5f / 9f;
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x00135A24 File Offset: 0x00133C24
	private static float GetConvertedTemperatureDelta(float kelvin_delta)
	{
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			return kelvin_delta;
		case GameUtil.TemperatureUnit.Fahrenheit:
			return kelvin_delta * 1.8f;
		case GameUtil.TemperatureUnit.Kelvin:
			return kelvin_delta;
		default:
			return kelvin_delta;
		}
	}

	// Token: 0x060037F0 RID: 14320 RVA: 0x00135A58 File Offset: 0x00133C58
	public static float ApplyTimeSlice(float val, GameUtil.TimeSlice timeSlice)
	{
		if (timeSlice == GameUtil.TimeSlice.PerCycle)
		{
			return val * 600f;
		}
		return val;
	}

	// Token: 0x060037F1 RID: 14321 RVA: 0x00135A67 File Offset: 0x00133C67
	public static float ApplyTimeSlice(int val, GameUtil.TimeSlice timeSlice)
	{
		if (timeSlice == GameUtil.TimeSlice.PerCycle)
		{
			return (float)val * 600f;
		}
		return (float)val;
	}

	// Token: 0x060037F2 RID: 14322 RVA: 0x00135A78 File Offset: 0x00133C78
	public static string AddTimeSliceText(string text, GameUtil.TimeSlice timeSlice)
	{
		switch (timeSlice)
		{
		case GameUtil.TimeSlice.PerSecond:
			return text + UI.UNITSUFFIXES.PERSECOND;
		case GameUtil.TimeSlice.PerCycle:
			return text + UI.UNITSUFFIXES.PERCYCLE;
		}
		return text;
	}

	// Token: 0x060037F3 RID: 14323 RVA: 0x00135AB5 File Offset: 0x00133CB5
	public static string AddPositiveSign(string text, bool positive)
	{
		if (positive)
		{
			return string.Format(UI.POSITIVE_FORMAT, text);
		}
		return text;
	}

	// Token: 0x060037F4 RID: 14324 RVA: 0x00135ACC File Offset: 0x00133CCC
	public static float AttributeSkillToAlpha(AttributeInstance attributeInstance)
	{
		return Mathf.Min(attributeInstance.GetTotalValue() / 10f, 1f);
	}

	// Token: 0x060037F5 RID: 14325 RVA: 0x00135AE4 File Offset: 0x00133CE4
	public static float AttributeSkillToAlpha(float attributeSkill)
	{
		return Mathf.Min(attributeSkill / 10f, 1f);
	}

	// Token: 0x060037F6 RID: 14326 RVA: 0x00135AF7 File Offset: 0x00133CF7
	public static float AptitudeToAlpha(float aptitude)
	{
		return Mathf.Min(aptitude / 10f, 1f);
	}

	// Token: 0x060037F7 RID: 14327 RVA: 0x00135B0A File Offset: 0x00133D0A
	public static float GetThermalEnergy(PrimaryElement pe)
	{
		return pe.Temperature * pe.Mass * pe.Element.specificHeatCapacity;
	}

	// Token: 0x060037F8 RID: 14328 RVA: 0x00135B25 File Offset: 0x00133D25
	public static float CalculateTemperatureChange(float shc, float mass, float kilowatts)
	{
		return kilowatts / (shc * mass);
	}

	// Token: 0x060037F9 RID: 14329 RVA: 0x00135B2C File Offset: 0x00133D2C
	public static void DeltaThermalEnergy(PrimaryElement pe, float kilowatts, float targetTemperature)
	{
		float num = GameUtil.CalculateTemperatureChange(pe.Element.specificHeatCapacity, pe.Mass, kilowatts);
		float num2 = pe.Temperature + num;
		if (targetTemperature > pe.Temperature)
		{
			num2 = Mathf.Clamp(num2, pe.Temperature, targetTemperature);
		}
		else
		{
			num2 = Mathf.Clamp(num2, targetTemperature, pe.Temperature);
		}
		pe.Temperature = num2;
	}

	// Token: 0x060037FA RID: 14330 RVA: 0x00135B88 File Offset: 0x00133D88
	public static BindingEntry ActionToBinding(global::Action action)
	{
		foreach (BindingEntry bindingEntry in GameInputMapping.KeyBindings)
		{
			if (bindingEntry.mAction == action)
			{
				return bindingEntry;
			}
		}
		throw new ArgumentException(action.ToString() + " is not bound in GameInputBindings");
	}

	// Token: 0x060037FB RID: 14331 RVA: 0x00135BD8 File Offset: 0x00133DD8
	public static string GetIdentityDescriptor(GameObject go, GameUtil.IdentityDescriptorTense tense = GameUtil.IdentityDescriptorTense.Normal)
	{
		if (go.GetComponent<MinionIdentity>())
		{
			switch (tense)
			{
			case GameUtil.IdentityDescriptorTense.Normal:
				return DUPLICANTS.STATS.SUBJECTS.DUPLICANT;
			case GameUtil.IdentityDescriptorTense.Possessive:
				return DUPLICANTS.STATS.SUBJECTS.DUPLICANT_POSSESSIVE;
			case GameUtil.IdentityDescriptorTense.Plural:
				return DUPLICANTS.STATS.SUBJECTS.DUPLICANT_PLURAL;
			}
		}
		else if (go.GetComponent<CreatureBrain>())
		{
			switch (tense)
			{
			case GameUtil.IdentityDescriptorTense.Normal:
				return DUPLICANTS.STATS.SUBJECTS.CREATURE;
			case GameUtil.IdentityDescriptorTense.Possessive:
				return DUPLICANTS.STATS.SUBJECTS.CREATURE_POSSESSIVE;
			case GameUtil.IdentityDescriptorTense.Plural:
				return DUPLICANTS.STATS.SUBJECTS.CREATURE_PLURAL;
			}
		}
		else
		{
			switch (tense)
			{
			case GameUtil.IdentityDescriptorTense.Normal:
				return DUPLICANTS.STATS.SUBJECTS.PLANT;
			case GameUtil.IdentityDescriptorTense.Possessive:
				return DUPLICANTS.STATS.SUBJECTS.PLANT_POSESSIVE;
			case GameUtil.IdentityDescriptorTense.Plural:
				return DUPLICANTS.STATS.SUBJECTS.PLANT_PLURAL;
			}
		}
		return "";
	}

	// Token: 0x060037FC RID: 14332 RVA: 0x00135CA6 File Offset: 0x00133EA6
	public static float GetEnergyInPrimaryElement(PrimaryElement element)
	{
		return 0.001f * (element.Temperature * (element.Mass * 1000f * element.Element.specificHeatCapacity));
	}

	// Token: 0x060037FD RID: 14333 RVA: 0x00135CD0 File Offset: 0x00133ED0
	public static float EnergyToTemperatureDelta(float kilojoules, PrimaryElement element)
	{
		global::Debug.Assert(element.Mass > 0f);
		float num = Mathf.Max(GameUtil.GetEnergyInPrimaryElement(element) - kilojoules, 1f);
		float temperature = element.Temperature;
		return num / (0.001f * (element.Mass * (element.Element.specificHeatCapacity * 1000f))) - temperature;
	}

	// Token: 0x060037FE RID: 14334 RVA: 0x00135D29 File Offset: 0x00133F29
	public static float CalculateEnergyDeltaForElement(PrimaryElement element, float startTemp, float endTemp)
	{
		return GameUtil.CalculateEnergyDeltaForElementChange(element.Mass, element.Element.specificHeatCapacity, startTemp, endTemp);
	}

	// Token: 0x060037FF RID: 14335 RVA: 0x00135D43 File Offset: 0x00133F43
	public static float CalculateEnergyDeltaForElementChange(float mass, float shc, float startTemp, float endTemp)
	{
		return (endTemp - startTemp) * mass * shc;
	}

	// Token: 0x06003800 RID: 14336 RVA: 0x00135D4C File Offset: 0x00133F4C
	public static float GetFinalTemperature(float t1, float m1, float t2, float m2)
	{
		float num = m1 + m2;
		float num2 = (t1 * m1 + t2 * m2) / num;
		float num3 = Mathf.Min(t1, t2);
		float num4 = Mathf.Max(t1, t2);
		num2 = Mathf.Clamp(num2, num3, num4);
		if (float.IsNaN(num2) || float.IsInfinity(num2))
		{
			global::Debug.LogError(string.Format("Calculated an invalid temperature: t1={0}, m1={1}, t2={2}, m2={3}, min_temp={4}, max_temp={5}", new object[]
			{
				t1,
				m1,
				t2,
				m2,
				num3,
				num4
			}));
		}
		return num2;
	}

	// Token: 0x06003801 RID: 14337 RVA: 0x00135DDC File Offset: 0x00133FDC
	public static void ForceConduction(PrimaryElement a, PrimaryElement b, float dt)
	{
		float num = a.Temperature * a.Element.specificHeatCapacity * a.Mass;
		float num2 = b.Temperature * b.Element.specificHeatCapacity * b.Mass;
		float num3 = Math.Min(a.Element.thermalConductivity, b.Element.thermalConductivity);
		float num4 = Math.Min(a.Mass, b.Mass);
		float num5 = (b.Temperature - a.Temperature) * (num3 * num4) * dt;
		float num6 = (num + num2) / (a.Element.specificHeatCapacity * a.Mass + b.Element.specificHeatCapacity * b.Mass);
		float val = Math.Abs((num6 - a.Temperature) * a.Element.specificHeatCapacity * a.Mass);
		float val2 = Math.Abs((num6 - b.Temperature) * b.Element.specificHeatCapacity * b.Mass);
		float num7 = Math.Min(val, val2);
		num5 = Math.Min(num5, num7);
		num5 = Math.Max(num5, -num7);
		a.Temperature = (num + num5) / a.Element.specificHeatCapacity / a.Mass;
		b.Temperature = (num2 - num5) / b.Element.specificHeatCapacity / b.Mass;
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x00135F28 File Offset: 0x00134128
	public static string FloatToString(float f, string format = null)
	{
		if (float.IsPositiveInfinity(f))
		{
			return UI.POS_INFINITY;
		}
		if (float.IsNegativeInfinity(f))
		{
			return UI.NEG_INFINITY;
		}
		return f.ToString(format);
	}

	// Token: 0x06003803 RID: 14339 RVA: 0x00135F58 File Offset: 0x00134158
	public static string GetFloatWithDecimalPoint(float f)
	{
		string format;
		if (f == 0f)
		{
			format = "0";
		}
		else if (Mathf.Abs(f) < 1f)
		{
			format = "#,##0.#";
		}
		else
		{
			format = "#,###.#";
		}
		return GameUtil.FloatToString(f, format);
	}

	// Token: 0x06003804 RID: 14340 RVA: 0x00135FA0 File Offset: 0x001341A0
	public static string GetStandardFloat(float f)
	{
		string format;
		if (f == 0f)
		{
			format = "0";
		}
		else if (Mathf.Abs(f) < 1f)
		{
			format = "#,##0.#";
		}
		else if (Mathf.Abs(f) < 10f)
		{
			format = "#,###.#";
		}
		else
		{
			format = "#,###";
		}
		return GameUtil.FloatToString(f, format);
	}

	// Token: 0x06003805 RID: 14341 RVA: 0x00135FFC File Offset: 0x001341FC
	public static string GetStandardPercentageFloat(float f, bool allowHundredths = false)
	{
		string format;
		if (Mathf.Abs(f) == 0f)
		{
			format = "0";
		}
		else if (Mathf.Abs(f) < 0.1f && allowHundredths)
		{
			format = "##0.##";
		}
		else if (Mathf.Abs(f) < 1f)
		{
			format = "##0.#";
		}
		else
		{
			format = "##0";
		}
		return GameUtil.FloatToString(f, format);
	}

	// Token: 0x06003806 RID: 14342 RVA: 0x00136060 File Offset: 0x00134260
	public static string GetUnitFormattedName(GameObject go, bool upperName = false)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		if (component != null && Assets.IsTagCountable(component.PrefabTag))
		{
			PrimaryElement component2 = go.GetComponent<PrimaryElement>();
			return GameUtil.GetUnitFormattedName(go.GetProperName(), component2.Units, upperName);
		}
		if (!upperName)
		{
			return go.GetProperName();
		}
		return StringFormatter.ToUpper(go.GetProperName());
	}

	// Token: 0x06003807 RID: 14343 RVA: 0x001360B9 File Offset: 0x001342B9
	public static string GetUnitFormattedName(string name, float count, bool upperName = false)
	{
		if (upperName)
		{
			name = name.ToUpper();
		}
		return StringFormatter.Replace(UI.NAME_WITH_UNITS, "{0}", name).Replace("{1}", string.Format("{0:0.##}", count));
	}

	// Token: 0x06003808 RID: 14344 RVA: 0x001360F8 File Offset: 0x001342F8
	public static string GetFormattedUnits(float units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool displaySuffix = true, string floatFormatOverride = "")
	{
		string str = (units == 1f) ? UI.UNITSUFFIXES.UNIT : UI.UNITSUFFIXES.UNITS;
		units = GameUtil.ApplyTimeSlice(units, timeSlice);
		string text = GameUtil.GetStandardFloat(units);
		if (!floatFormatOverride.IsNullOrWhiteSpace())
		{
			text = string.Format(floatFormatOverride, units);
		}
		if (displaySuffix)
		{
			text += str;
		}
		return GameUtil.AddTimeSliceText(text, timeSlice);
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x00136158 File Offset: 0x00134358
	public static string GetFormattedRocketRange(float range, GameUtil.TimeSlice timeSlice, bool displaySuffix = true)
	{
		if (timeSlice == GameUtil.TimeSlice.PerCycle)
		{
			return range.ToString("N1") + (displaySuffix ? (" " + UI.CLUSTERMAP.TILES_PER_CYCLE) : "");
		}
		float num = range / 600f;
		if (Mathf.Approximately(num, Mathf.Round(num)))
		{
			num = Mathf.Round(num);
		}
		return Mathf.Floor(num).ToString() + (displaySuffix ? (" " + UI.CLUSTERMAP.TILES) : "");
	}

	// Token: 0x0600380A RID: 14346 RVA: 0x001361E7 File Offset: 0x001343E7
	public static string ApplyBoldString(string source)
	{
		return "<b>" + source + "</b>";
	}

	// Token: 0x0600380B RID: 14347 RVA: 0x001361FC File Offset: 0x001343FC
	public static float GetRoundedTemperatureInKelvin(float kelvin)
	{
		float result = 0f;
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			result = GameUtil.GetTemperatureConvertedToKelvin(Mathf.Round(GameUtil.GetConvertedTemperature(Mathf.Round(kelvin), true)));
			break;
		case GameUtil.TemperatureUnit.Fahrenheit:
			result = GameUtil.GetTemperatureConvertedToKelvin((float)Mathf.RoundToInt(GameUtil.GetTemperatureConvertedFromKelvin(kelvin, GameUtil.TemperatureUnit.Fahrenheit)), GameUtil.TemperatureUnit.Fahrenheit);
			break;
		case GameUtil.TemperatureUnit.Kelvin:
			result = (float)Mathf.RoundToInt(kelvin);
			break;
		}
		return result;
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x00136264 File Offset: 0x00134464
	public static string GetFormattedTemperature(float temp, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation interpretation = GameUtil.TemperatureInterpretation.Absolute, bool displayUnits = true, bool roundInDestinationFormat = false)
	{
		if (interpretation != GameUtil.TemperatureInterpretation.Absolute)
		{
			if (interpretation != GameUtil.TemperatureInterpretation.Relative)
			{
			}
			temp = GameUtil.GetConvertedTemperatureDelta(temp);
		}
		else
		{
			temp = GameUtil.GetConvertedTemperature(temp, roundInDestinationFormat);
		}
		temp = GameUtil.ApplyTimeSlice(temp, timeSlice);
		string text;
		if (Mathf.Abs(temp) < 0.1f)
		{
			text = GameUtil.FloatToString(temp, "##0.####");
		}
		else
		{
			text = GameUtil.FloatToString(temp, "##0.#");
		}
		if (displayUnits)
		{
			text = GameUtil.AddTemperatureUnitSuffix(text);
		}
		return GameUtil.AddTimeSliceText(text, timeSlice);
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x001362D8 File Offset: 0x001344D8
	public static string GetFormattedCaloriesForItem(Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
		return GameUtil.GetFormattedCalories((foodInfo != null) ? (foodInfo.CaloriesPerUnit * amount) : -1f, timeSlice, forceKcal);
	}

	// Token: 0x0600380E RID: 14350 RVA: 0x0013630C File Offset: 0x0013450C
	public static string GetFormattedCalories(float calories, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		string str = UI.UNITSUFFIXES.CALORIES.CALORIE;
		if (Mathf.Abs(calories) >= 1000f || forceKcal)
		{
			calories /= 1000f;
			str = UI.UNITSUFFIXES.CALORIES.KILOCALORIE;
		}
		calories = GameUtil.ApplyTimeSlice(calories, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.GetStandardFloat(calories) + str, timeSlice);
	}

	// Token: 0x0600380F RID: 14351 RVA: 0x00136367 File Offset: 0x00134567
	public static string GetFormattedPlantGrowth(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		percent = GameUtil.ApplyTimeSlice(percent, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.GetStandardPercentageFloat(percent, true) + UI.UNITSUFFIXES.PERCENT + " " + UI.UNITSUFFIXES.GROWTH, timeSlice);
	}

	// Token: 0x06003810 RID: 14352 RVA: 0x0013639D File Offset: 0x0013459D
	public static string GetFormattedPercent(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		percent = GameUtil.ApplyTimeSlice(percent, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.GetStandardPercentageFloat(percent, true) + UI.UNITSUFFIXES.PERCENT, timeSlice);
	}

	// Token: 0x06003811 RID: 14353 RVA: 0x001363C4 File Offset: 0x001345C4
	public static string GetFormattedRoundedJoules(float joules)
	{
		if (Mathf.Abs(joules) > 1000f)
		{
			return GameUtil.FloatToString(joules / 1000f, "F1") + UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE;
		}
		return GameUtil.FloatToString(joules, "F1") + UI.UNITSUFFIXES.ELECTRICAL.JOULE;
	}

	// Token: 0x06003812 RID: 14354 RVA: 0x0013641C File Offset: 0x0013461C
	public static string GetFormattedJoules(float joules, string floatFormat = "F1", GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		if (timeSlice == GameUtil.TimeSlice.PerSecond)
		{
			return GameUtil.GetFormattedWattage(joules, GameUtil.WattageFormatterUnit.Automatic, true);
		}
		joules = GameUtil.ApplyTimeSlice(joules, timeSlice);
		string text;
		if (Math.Abs(joules) > 1000000f)
		{
			text = GameUtil.FloatToString(joules / 1000000f, floatFormat) + UI.UNITSUFFIXES.ELECTRICAL.MEGAJOULE;
		}
		else if (Mathf.Abs(joules) > 1000f)
		{
			text = GameUtil.FloatToString(joules / 1000f, floatFormat) + UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE;
		}
		else
		{
			text = GameUtil.FloatToString(joules, floatFormat) + UI.UNITSUFFIXES.ELECTRICAL.JOULE;
		}
		return GameUtil.AddTimeSliceText(text, timeSlice);
	}

	// Token: 0x06003813 RID: 14355 RVA: 0x001364B5 File Offset: 0x001346B5
	public static string GetFormattedRads(float rads, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		rads = GameUtil.ApplyTimeSlice(rads, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.GetStandardFloat(rads) + UI.UNITSUFFIXES.RADIATION.RADS, timeSlice);
	}

	// Token: 0x06003814 RID: 14356 RVA: 0x001364DC File Offset: 0x001346DC
	public static string GetFormattedHighEnergyParticles(float units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool displayUnits = true)
	{
		string str = (units == 1f) ? UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLE : UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		units = GameUtil.ApplyTimeSlice(units, timeSlice);
		return GameUtil.AddTimeSliceText(displayUnits ? (GameUtil.GetFloatWithDecimalPoint(units) + str) : GameUtil.GetFloatWithDecimalPoint(units), timeSlice);
	}

	// Token: 0x06003815 RID: 14357 RVA: 0x0013652C File Offset: 0x0013472C
	public static string GetFormattedWattage(float watts, GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Automatic, bool displayUnits = true)
	{
		LocString loc_string = "";
		switch (unit)
		{
		case GameUtil.WattageFormatterUnit.Watts:
			loc_string = UI.UNITSUFFIXES.ELECTRICAL.WATT;
			break;
		case GameUtil.WattageFormatterUnit.Kilowatts:
			watts /= 1000f;
			loc_string = UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
			break;
		case GameUtil.WattageFormatterUnit.Automatic:
			if (Mathf.Abs(watts) > 1000f)
			{
				watts /= 1000f;
				loc_string = UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
			}
			else
			{
				loc_string = UI.UNITSUFFIXES.ELECTRICAL.WATT;
			}
			break;
		}
		if (displayUnits)
		{
			return GameUtil.FloatToString(watts, "###0.##") + loc_string;
		}
		return GameUtil.FloatToString(watts, "###0.##");
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x001365BC File Offset: 0x001347BC
	public static string GetFormattedHeatEnergy(float dtu, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
	{
		LocString loc_string = "";
		string format;
		switch (unit)
		{
		case GameUtil.HeatEnergyFormatterUnit.DTU_S:
			loc_string = UI.UNITSUFFIXES.HEAT.DTU;
			format = "###0.";
			break;
		case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
			dtu /= 1000f;
			loc_string = UI.UNITSUFFIXES.HEAT.KDTU;
			format = "###0.##";
			break;
		default:
			if (Mathf.Abs(dtu) > 1000f)
			{
				dtu /= 1000f;
				loc_string = UI.UNITSUFFIXES.HEAT.KDTU;
				format = "###0.##";
			}
			else
			{
				loc_string = UI.UNITSUFFIXES.HEAT.DTU;
				format = "###0.";
			}
			break;
		}
		return GameUtil.FloatToString(dtu, format) + loc_string;
	}

	// Token: 0x06003817 RID: 14359 RVA: 0x00136650 File Offset: 0x00134850
	public static string GetFormattedHeatEnergyRate(float dtu_s, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
	{
		LocString loc_string = "";
		switch (unit)
		{
		case GameUtil.HeatEnergyFormatterUnit.DTU_S:
			loc_string = UI.UNITSUFFIXES.HEAT.DTU_S;
			break;
		case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
			dtu_s /= 1000f;
			loc_string = UI.UNITSUFFIXES.HEAT.KDTU_S;
			break;
		case GameUtil.HeatEnergyFormatterUnit.Automatic:
			if (Mathf.Abs(dtu_s) > 1000f)
			{
				dtu_s /= 1000f;
				loc_string = UI.UNITSUFFIXES.HEAT.KDTU_S;
			}
			else
			{
				loc_string = UI.UNITSUFFIXES.HEAT.DTU_S;
			}
			break;
		}
		return GameUtil.FloatToString(dtu_s, "###0.##") + loc_string;
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x001366CF File Offset: 0x001348CF
	public static string GetFormattedInt(float num, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		num = GameUtil.ApplyTimeSlice(num, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.FloatToString(num, "F0"), timeSlice);
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x001366EC File Offset: 0x001348EC
	public static string GetFormattedSimple(float num, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, string formatString = null)
	{
		num = GameUtil.ApplyTimeSlice(num, timeSlice);
		string text;
		if (formatString != null)
		{
			text = GameUtil.FloatToString(num, formatString);
		}
		else if (num == 0f)
		{
			text = "0";
		}
		else if (Mathf.Abs(num) < 1f)
		{
			text = GameUtil.FloatToString(num, "#,##0.##");
		}
		else if (Mathf.Abs(num) < 10f)
		{
			text = GameUtil.FloatToString(num, "#,###.##");
		}
		else
		{
			text = GameUtil.FloatToString(num, "#,###.##");
		}
		return GameUtil.AddTimeSliceText(text, timeSlice);
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x0013676E File Offset: 0x0013496E
	public static string GetFormattedLux(int lux)
	{
		return lux.ToString() + UI.UNITSUFFIXES.LIGHT.LUX;
	}

	// Token: 0x0600381B RID: 14363 RVA: 0x00136788 File Offset: 0x00134988
	public static string GetLightDescription(int lux)
	{
		if (lux == 0)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.NO_LIGHT;
		}
		if (lux < 100)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.VERY_LOW_LIGHT;
		}
		if (lux < 1000)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.LOW_LIGHT;
		}
		if (lux < 10000)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.MEDIUM_LIGHT;
		}
		if (lux < 50000)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.HIGH_LIGHT;
		}
		if (lux < 100000)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.VERY_HIGH_LIGHT;
		}
		return UI.OVERLAYS.LIGHTING.RANGES.MAX_LIGHT;
	}

	// Token: 0x0600381C RID: 14364 RVA: 0x0013680C File Offset: 0x00134A0C
	public static string GetRadiationDescription(float radsPerCycle)
	{
		if (radsPerCycle == 0f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.NONE;
		}
		if (radsPerCycle < 100f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.VERY_LOW;
		}
		if (radsPerCycle < 200f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.LOW;
		}
		if (radsPerCycle < 400f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.MEDIUM;
		}
		if (radsPerCycle < 2000f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.HIGH;
		}
		if (radsPerCycle < 4000f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.VERY_HIGH;
		}
		return UI.OVERLAYS.RADIATION.RANGES.MAX;
	}

	// Token: 0x0600381D RID: 14365 RVA: 0x00136898 File Offset: 0x00134A98
	public static string GetFormattedByTag(Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		if (GameTags.DisplayAsCalories.Contains(tag))
		{
			return GameUtil.GetFormattedCaloriesForItem(tag, amount, timeSlice, true);
		}
		if (GameTags.DisplayAsUnits.Contains(tag))
		{
			return GameUtil.GetFormattedUnits(amount, timeSlice, true, "");
		}
		return GameUtil.GetFormattedMass(amount, timeSlice, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x001368E8 File Offset: 0x00134AE8
	public static string GetFormattedFoodQuality(int quality)
	{
		if (GameUtil.adjectives == null)
		{
			GameUtil.adjectives = LocString.GetStrings(typeof(DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVES));
		}
		LocString loc_string = (quality >= 0) ? DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_POSITIVE : DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_NEGATIVE;
		int num = quality - DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_INDEX_OFFSET;
		num = Mathf.Clamp(num, 0, GameUtil.adjectives.Length);
		return string.Format(loc_string, GameUtil.adjectives[num], GameUtil.AddPositiveSign(quality.ToString(), quality > 0));
	}

	// Token: 0x0600381F RID: 14367 RVA: 0x00136958 File Offset: 0x00134B58
	public static string GetFormattedBytes(ulong amount)
	{
		string[] array = new string[]
		{
			UI.UNITSUFFIXES.INFORMATION.BYTE,
			UI.UNITSUFFIXES.INFORMATION.KILOBYTE,
			UI.UNITSUFFIXES.INFORMATION.MEGABYTE,
			UI.UNITSUFFIXES.INFORMATION.GIGABYTE,
			UI.UNITSUFFIXES.INFORMATION.TERABYTE
		};
		int num = (amount == 0UL) ? 0 : ((int)Math.Floor(Math.Floor(Math.Log(amount)) / Math.Log(1024.0)));
		double num2 = amount / Math.Pow(1024.0, (double)num);
		global::Debug.Assert(num >= 0 && num < array.Length);
		return string.Format("{0:F} {1}", num2, array[num]);
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x00136A10 File Offset: 0x00134C10
	public static string GetFormattedInfomation(float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		amount = GameUtil.ApplyTimeSlice(amount, timeSlice);
		string str = "";
		if (amount < 1024f)
		{
			str = UI.UNITSUFFIXES.INFORMATION.KILOBYTE;
		}
		else if (amount < 1048576f)
		{
			amount /= 1000f;
			str = UI.UNITSUFFIXES.INFORMATION.MEGABYTE;
		}
		else if (amount < 1.0737418E+09f)
		{
			amount /= 1048576f;
			str = UI.UNITSUFFIXES.INFORMATION.GIGABYTE;
		}
		return GameUtil.AddTimeSliceText(amount.ToString() + str, timeSlice);
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x00136A90 File Offset: 0x00134C90
	public static LocString GetCurrentMassUnit(bool useSmallUnit = false)
	{
		LocString result = null;
		GameUtil.MassUnit massUnit = GameUtil.massUnit;
		if (massUnit != GameUtil.MassUnit.Kilograms)
		{
			if (massUnit == GameUtil.MassUnit.Pounds)
			{
				result = UI.UNITSUFFIXES.MASS.POUND;
			}
		}
		else if (useSmallUnit)
		{
			result = UI.UNITSUFFIXES.MASS.GRAM;
		}
		else
		{
			result = UI.UNITSUFFIXES.MASS.KILOGRAM;
		}
		return result;
	}

	// Token: 0x06003822 RID: 14370 RVA: 0x00136AC8 File Offset: 0x00134CC8
	public static string GetFormattedMass(float mass, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.UseThreshold, bool includeSuffix = true, string floatFormat = "{0:0.#}")
	{
		if (mass == -3.4028235E+38f)
		{
			return UI.CALCULATING;
		}
		if (float.IsPositiveInfinity(mass))
		{
			return UI.POS_INFINITY + UI.UNITSUFFIXES.MASS.TONNE;
		}
		if (float.IsNegativeInfinity(mass))
		{
			return UI.NEG_INFINITY + UI.UNITSUFFIXES.MASS.TONNE;
		}
		mass = GameUtil.ApplyTimeSlice(mass, timeSlice);
		string str;
		if (GameUtil.massUnit == GameUtil.MassUnit.Kilograms)
		{
			str = UI.UNITSUFFIXES.MASS.TONNE;
			if (massFormat == GameUtil.MetricMassFormat.UseThreshold)
			{
				float num = Mathf.Abs(mass);
				if (0f < num)
				{
					if (num < 5E-06f)
					{
						str = UI.UNITSUFFIXES.MASS.MICROGRAM;
						mass = Mathf.Floor(mass * 1E+09f);
					}
					else if (num < 0.005f)
					{
						mass *= 1000000f;
						str = UI.UNITSUFFIXES.MASS.MILLIGRAM;
					}
					else if (Mathf.Abs(mass) < 5f)
					{
						mass *= 1000f;
						str = UI.UNITSUFFIXES.MASS.GRAM;
					}
					else if (Mathf.Abs(mass) < 5000f)
					{
						str = UI.UNITSUFFIXES.MASS.KILOGRAM;
					}
					else
					{
						mass /= 1000f;
						str = UI.UNITSUFFIXES.MASS.TONNE;
					}
				}
				else
				{
					str = UI.UNITSUFFIXES.MASS.KILOGRAM;
				}
			}
			else if (massFormat == GameUtil.MetricMassFormat.Kilogram)
			{
				str = UI.UNITSUFFIXES.MASS.KILOGRAM;
			}
			else if (massFormat == GameUtil.MetricMassFormat.Gram)
			{
				mass *= 1000f;
				str = UI.UNITSUFFIXES.MASS.GRAM;
			}
			else if (massFormat == GameUtil.MetricMassFormat.Tonne)
			{
				mass /= 1000f;
				str = UI.UNITSUFFIXES.MASS.TONNE;
			}
		}
		else
		{
			mass /= 2.2f;
			str = UI.UNITSUFFIXES.MASS.POUND;
			if (massFormat == GameUtil.MetricMassFormat.UseThreshold)
			{
				float num2 = Mathf.Abs(mass);
				if (num2 < 5f && num2 > 0.001f)
				{
					mass *= 256f;
					str = UI.UNITSUFFIXES.MASS.DRACHMA;
				}
				else
				{
					mass *= 7000f;
					str = UI.UNITSUFFIXES.MASS.GRAIN;
				}
			}
		}
		if (!includeSuffix)
		{
			str = "";
			timeSlice = GameUtil.TimeSlice.None;
		}
		return GameUtil.AddTimeSliceText(string.Format(floatFormat, mass) + str, timeSlice);
	}

	// Token: 0x06003823 RID: 14371 RVA: 0x00136CDA File Offset: 0x00134EDA
	public static string GetFormattedTime(float seconds, string floatFormat = "F0")
	{
		return string.Format(UI.FORMATSECONDS, seconds.ToString(floatFormat));
	}

	// Token: 0x06003824 RID: 14372 RVA: 0x00136CF3 File Offset: 0x00134EF3
	public static string GetFormattedEngineEfficiency(float amount)
	{
		return amount.ToString() + " km /" + UI.UNITSUFFIXES.MASS.KILOGRAM;
	}

	// Token: 0x06003825 RID: 14373 RVA: 0x00136D10 File Offset: 0x00134F10
	public static string GetFormattedDistance(float meters)
	{
		if (Mathf.Abs(meters) < 1f)
		{
			string text = (meters * 100f).ToString();
			string text2 = text.Substring(0, text.LastIndexOf('.') + Mathf.Min(3, text.Length - text.LastIndexOf('.')));
			if (text2 == "-0.0")
			{
				text2 = "0";
			}
			return text2 + " cm";
		}
		if (meters < 1000f)
		{
			return meters.ToString() + " m";
		}
		return Util.FormatOneDecimalPlace(meters / 1000f) + " km";
	}

	// Token: 0x06003826 RID: 14374 RVA: 0x00136DAF File Offset: 0x00134FAF
	public static string GetFormattedCycles(float seconds, string formatString = "F1", bool forceCycles = false)
	{
		if (forceCycles || Mathf.Abs(seconds) > 100f)
		{
			return string.Format(UI.FORMATDAY, GameUtil.FloatToString(seconds / 600f, formatString));
		}
		return GameUtil.GetFormattedTime(seconds, "F0");
	}

	// Token: 0x06003827 RID: 14375 RVA: 0x00136DE9 File Offset: 0x00134FE9
	public static float GetDisplaySHC(float shc)
	{
		if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
		{
			shc /= 1.8f;
		}
		return shc;
	}

	// Token: 0x06003828 RID: 14376 RVA: 0x00136DFD File Offset: 0x00134FFD
	public static string GetSHCSuffix()
	{
		return string.Format("(DTU/g)/{0}", GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x06003829 RID: 14377 RVA: 0x00136E0E File Offset: 0x0013500E
	public static string GetFormattedSHC(float shc)
	{
		shc = GameUtil.GetDisplaySHC(shc);
		return string.Format("{0} (DTU/g)/{1}", shc.ToString("0.000"), GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x0600382A RID: 14378 RVA: 0x00136E33 File Offset: 0x00135033
	public static float GetDisplayThermalConductivity(float tc)
	{
		if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
		{
			tc /= 1.8f;
		}
		return tc;
	}

	// Token: 0x0600382B RID: 14379 RVA: 0x00136E47 File Offset: 0x00135047
	public static string GetThermalConductivitySuffix()
	{
		return string.Format("(DTU/(m*s))/{0}", GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x00136E58 File Offset: 0x00135058
	public static string GetFormattedThermalConductivity(float tc)
	{
		tc = GameUtil.GetDisplayThermalConductivity(tc);
		return string.Format("{0} (DTU/(m*s))/{1}", tc.ToString("0.000"), GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x0600382D RID: 14381 RVA: 0x00136E7D File Offset: 0x0013507D
	public static string GetElementNameByElementHash(SimHashes elementHash)
	{
		return ElementLoader.FindElementByHash(elementHash).tag.ProperName();
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x00136E90 File Offset: 0x00135090
	public static bool HasTrait(GameObject go, string traitName)
	{
		Traits component = go.GetComponent<Traits>();
		return !(component == null) && component.HasTrait(traitName);
	}

	// Token: 0x0600382F RID: 14383 RVA: 0x00136EB8 File Offset: 0x001350B8
	public static HashSet<int> GetFloodFillCavity(int startCell, bool allowLiquid)
	{
		HashSet<int> result = new HashSet<int>();
		if (allowLiquid)
		{
			result = GameUtil.FloodCollectCells(startCell, (int cell) => !Grid.Solid[cell], 300, null, true);
		}
		else
		{
			result = GameUtil.FloodCollectCells(startCell, (int cell) => Grid.Element[cell].IsVacuum || Grid.Element[cell].IsGas, 300, null, true);
		}
		return result;
	}

	// Token: 0x06003830 RID: 14384 RVA: 0x00136F2C File Offset: 0x0013512C
	public static float GetRadiationAbsorptionPercentage(int cell)
	{
		if (Grid.IsValidCell(cell))
		{
			return GameUtil.GetRadiationAbsorptionPercentage(Grid.Element[cell], Grid.Mass[cell], Grid.IsSolidCell(cell) && (Grid.Properties[cell] & 128) == 128);
		}
		return 0f;
	}

	// Token: 0x06003831 RID: 14385 RVA: 0x00136F84 File Offset: 0x00135184
	public static float GetRadiationAbsorptionPercentage(Element elem, float mass, bool isConstructed)
	{
		float num = 2000f;
		float num2 = 0.3f;
		float num3 = 0.7f;
		float num4 = 0.8f;
		float value;
		if (isConstructed)
		{
			value = elem.radiationAbsorptionFactor * num4;
		}
		else
		{
			value = elem.radiationAbsorptionFactor * num2 + mass / num * elem.radiationAbsorptionFactor * num3;
		}
		return Mathf.Clamp(value, 0f, 1f);
	}

	// Token: 0x06003832 RID: 14386 RVA: 0x00136FE8 File Offset: 0x001351E8
	public static HashSet<int> CollectCellsBreadthFirst(int start_cell, Func<int, bool> test_func, int max_depth = 10)
	{
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> hashSet2 = new HashSet<int>();
		HashSet<int> hashSet3 = new HashSet<int>();
		hashSet3.Add(start_cell);
		Vector2Int[] array = new Vector2Int[]
		{
			new Vector2Int(1, 0),
			new Vector2Int(-1, 0),
			new Vector2Int(0, 1),
			new Vector2Int(0, -1)
		};
		for (int i = 0; i < max_depth; i++)
		{
			List<int> list = new List<int>();
			foreach (int cell in hashSet3)
			{
				foreach (Vector2Int vector2Int in array)
				{
					int num = Grid.OffsetCell(cell, vector2Int.x, vector2Int.y);
					if (!hashSet2.Contains(num) && !hashSet.Contains(num))
					{
						if (Grid.IsValidCell(num) && test_func(num))
						{
							hashSet.Add(num);
							list.Add(num);
						}
						else
						{
							hashSet2.Add(num);
						}
					}
				}
			}
			hashSet3.Clear();
			foreach (int item in list)
			{
				hashSet3.Add(item);
			}
			list.Clear();
			if (hashSet3.Count == 0)
			{
				break;
			}
		}
		return hashSet;
	}

	// Token: 0x06003833 RID: 14387 RVA: 0x00137184 File Offset: 0x00135384
	public static HashSet<int> FloodCollectCells(int start_cell, Func<int, bool> is_valid, int maxSize = 300, HashSet<int> AddInvalidCellsToSet = null, bool clearOversizedResults = true)
	{
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> hashSet2 = new HashSet<int>();
		GameUtil.probeFromCell(start_cell, is_valid, hashSet, hashSet2, maxSize);
		if (AddInvalidCellsToSet != null)
		{
			AddInvalidCellsToSet.UnionWith(hashSet2);
			if (hashSet.Count > maxSize)
			{
				AddInvalidCellsToSet.UnionWith(hashSet);
			}
		}
		if (hashSet.Count > maxSize && clearOversizedResults)
		{
			hashSet.Clear();
		}
		return hashSet;
	}

	// Token: 0x06003834 RID: 14388 RVA: 0x001371D8 File Offset: 0x001353D8
	public static HashSet<int> FloodCollectCells(HashSet<int> results, int start_cell, Func<int, bool> is_valid, int maxSize = 300, HashSet<int> AddInvalidCellsToSet = null, bool clearOversizedResults = true)
	{
		HashSet<int> hashSet = new HashSet<int>();
		GameUtil.probeFromCell(start_cell, is_valid, results, hashSet, maxSize);
		if (AddInvalidCellsToSet != null)
		{
			AddInvalidCellsToSet.UnionWith(hashSet);
			if (results.Count > maxSize)
			{
				AddInvalidCellsToSet.UnionWith(results);
			}
		}
		if (results.Count > maxSize && clearOversizedResults)
		{
			results.Clear();
		}
		return results;
	}

	// Token: 0x06003835 RID: 14389 RVA: 0x00137228 File Offset: 0x00135428
	private static void probeFromCell(int start_cell, Func<int, bool> is_valid, HashSet<int> cells, HashSet<int> invalidCells, int maxSize = 300)
	{
		if (cells.Count > maxSize || !Grid.IsValidCell(start_cell) || invalidCells.Contains(start_cell) || cells.Contains(start_cell) || !is_valid(start_cell))
		{
			invalidCells.Add(start_cell);
			return;
		}
		cells.Add(start_cell);
		GameUtil.probeFromCell(Grid.CellLeft(start_cell), is_valid, cells, invalidCells, maxSize);
		GameUtil.probeFromCell(Grid.CellRight(start_cell), is_valid, cells, invalidCells, maxSize);
		GameUtil.probeFromCell(Grid.CellAbove(start_cell), is_valid, cells, invalidCells, maxSize);
		GameUtil.probeFromCell(Grid.CellBelow(start_cell), is_valid, cells, invalidCells, maxSize);
	}

	// Token: 0x06003836 RID: 14390 RVA: 0x001372B3 File Offset: 0x001354B3
	public static bool FloodFillCheck<ArgType>(Func<int, ArgType, bool> fn, ArgType arg, int start_cell, int max_depth, bool stop_at_solid, bool stop_at_liquid)
	{
		return GameUtil.FloodFillFind<ArgType>(fn, arg, start_cell, max_depth, stop_at_solid, stop_at_liquid) != -1;
	}

	// Token: 0x06003837 RID: 14391 RVA: 0x001372C8 File Offset: 0x001354C8
	public static int FloodFillFind<ArgType>(Func<int, ArgType, bool> fn, ArgType arg, int start_cell, int max_depth, bool stop_at_solid, bool stop_at_liquid)
	{
		GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = start_cell,
			depth = 0
		});
		int result = -1;
		while (GameUtil.FloodFillNext.Count > 0)
		{
			GameUtil.FloodFillInfo floodFillInfo = GameUtil.FloodFillNext.Dequeue();
			if (floodFillInfo.depth < max_depth && Grid.IsValidCell(floodFillInfo.cell))
			{
				Element element = Grid.Element[floodFillInfo.cell];
				if ((!stop_at_solid || !element.IsSolid) && (!stop_at_liquid || !element.IsLiquid) && !GameUtil.FloodFillVisited.Contains(floodFillInfo.cell))
				{
					GameUtil.FloodFillVisited.Add(floodFillInfo.cell);
					if (fn(floodFillInfo.cell, arg))
					{
						result = floodFillInfo.cell;
						break;
					}
					GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellLeft(floodFillInfo.cell),
						depth = floodFillInfo.depth + 1
					});
					GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellRight(floodFillInfo.cell),
						depth = floodFillInfo.depth + 1
					});
					GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellAbove(floodFillInfo.cell),
						depth = floodFillInfo.depth + 1
					});
					GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellBelow(floodFillInfo.cell),
						depth = floodFillInfo.depth + 1
					});
				}
			}
		}
		GameUtil.FloodFillVisited.Clear();
		GameUtil.FloodFillNext.Clear();
		return result;
	}

	// Token: 0x06003838 RID: 14392 RVA: 0x00137488 File Offset: 0x00135688
	public static void FloodFillConditional(int start_cell, Func<int, bool> condition, ICollection<int> visited_cells, ICollection<int> valid_cells = null)
	{
		GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = start_cell,
			depth = 0
		});
		GameUtil.FloodFillConditional(GameUtil.FloodFillNext, condition, visited_cells, valid_cells, 10000);
	}

	// Token: 0x06003839 RID: 14393 RVA: 0x001374CC File Offset: 0x001356CC
	public static void FloodFillConditional(Queue<GameUtil.FloodFillInfo> queue, Func<int, bool> condition, ICollection<int> visited_cells, ICollection<int> valid_cells = null, int max_depth = 10000)
	{
		while (queue.Count > 0)
		{
			GameUtil.FloodFillInfo floodFillInfo = queue.Dequeue();
			if (floodFillInfo.depth < max_depth && Grid.IsValidCell(floodFillInfo.cell) && !visited_cells.Contains(floodFillInfo.cell))
			{
				visited_cells.Add(floodFillInfo.cell);
				if (condition(floodFillInfo.cell))
				{
					if (valid_cells != null)
					{
						valid_cells.Add(floodFillInfo.cell);
					}
					int depth = floodFillInfo.depth + 1;
					queue.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellLeft(floodFillInfo.cell),
						depth = depth
					});
					queue.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellRight(floodFillInfo.cell),
						depth = depth
					});
					queue.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellAbove(floodFillInfo.cell),
						depth = depth
					});
					queue.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellBelow(floodFillInfo.cell),
						depth = depth
					});
				}
			}
		}
		queue.Clear();
	}

	// Token: 0x0600383A RID: 14394 RVA: 0x00137600 File Offset: 0x00135800
	public static string GetHardnessString(Element element, bool addColor = true)
	{
		if (!element.IsSolid)
		{
			return ELEMENTS.HARDNESS.NA;
		}
		Color c = GameUtil.Hardness.firmColor;
		string text;
		if (element.hardness >= 255)
		{
			c = GameUtil.Hardness.ImpenetrableColor;
			text = string.Format(ELEMENTS.HARDNESS.IMPENETRABLE, element.hardness);
		}
		else if (element.hardness >= 150)
		{
			c = GameUtil.Hardness.nearlyImpenetrableColor;
			text = string.Format(ELEMENTS.HARDNESS.NEARLYIMPENETRABLE, element.hardness);
		}
		else if (element.hardness >= 50)
		{
			c = GameUtil.Hardness.veryFirmColor;
			text = string.Format(ELEMENTS.HARDNESS.VERYFIRM, element.hardness);
		}
		else if (element.hardness >= 25)
		{
			c = GameUtil.Hardness.firmColor;
			text = string.Format(ELEMENTS.HARDNESS.FIRM, element.hardness);
		}
		else if (element.hardness >= 10)
		{
			c = GameUtil.Hardness.softColor;
			text = string.Format(ELEMENTS.HARDNESS.SOFT, element.hardness);
		}
		else
		{
			c = GameUtil.Hardness.verySoftColor;
			text = string.Format(ELEMENTS.HARDNESS.VERYSOFT, element.hardness);
		}
		if (addColor)
		{
			text = string.Format("<color=#{0}>{1}</color>", c.ToHexString(), text);
		}
		return text;
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x00137750 File Offset: 0x00135950
	public static string GetGermResistanceModifierString(float modifier, bool addColor = true)
	{
		Color c = Color.black;
		string text = "";
		if (modifier > 0f)
		{
			if (modifier >= 5f)
			{
				c = GameUtil.GermResistanceValues.PositiveLargeColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_LARGE, modifier);
			}
			else if (modifier >= 2f)
			{
				c = GameUtil.GermResistanceValues.PositiveMediumColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_MEDIUM, modifier);
			}
			else if (modifier > 0f)
			{
				c = GameUtil.GermResistanceValues.PositiveSmallColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_SMALL, modifier);
			}
		}
		else if (modifier < 0f)
		{
			if (modifier <= -5f)
			{
				c = GameUtil.GermResistanceValues.NegativeLargeColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_LARGE, modifier);
			}
			else if (modifier <= -2f)
			{
				c = GameUtil.GermResistanceValues.NegativeMediumColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_MEDIUM, modifier);
			}
			else if (modifier < 0f)
			{
				c = GameUtil.GermResistanceValues.NegativeSmallColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_SMALL, modifier);
			}
		}
		else
		{
			addColor = false;
			text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NONE, modifier);
		}
		if (addColor)
		{
			text = string.Format("<color=#{0}>{1}</color>", c.ToHexString(), text);
		}
		return text;
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x00137898 File Offset: 0x00135A98
	public static string GetThermalConductivityString(Element element, bool addColor = true, bool addValue = true)
	{
		Color c = GameUtil.ThermalConductivityValues.mediumConductivityColor;
		string text;
		if (element.thermalConductivity >= 50f)
		{
			c = GameUtil.ThermalConductivityValues.veryHighConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_HIGH_CONDUCTIVITY;
		}
		else if (element.thermalConductivity >= 10f)
		{
			c = GameUtil.ThermalConductivityValues.highConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.HIGH_CONDUCTIVITY;
		}
		else if (element.thermalConductivity >= 2f)
		{
			c = GameUtil.ThermalConductivityValues.mediumConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.MEDIUM_CONDUCTIVITY;
		}
		else if (element.thermalConductivity >= 1f)
		{
			c = GameUtil.ThermalConductivityValues.lowConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.LOW_CONDUCTIVITY;
		}
		else
		{
			c = GameUtil.ThermalConductivityValues.veryLowConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_LOW_CONDUCTIVITY;
		}
		if (addColor)
		{
			text = string.Format("<color=#{0}>{1}</color>", c.ToHexString(), text);
		}
		if (addValue)
		{
			text = string.Format(UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VALUE_WITH_ADJECTIVE, element.thermalConductivity.ToString(), text);
		}
		return text;
	}

	// Token: 0x0600383D RID: 14397 RVA: 0x00137978 File Offset: 0x00135B78
	public static string GetBreathableString(Element element, float Mass)
	{
		if (!element.IsGas && !element.IsVacuum)
		{
			return "";
		}
		Color c = GameUtil.BreathableValues.positiveColor;
		SimHashes id = element.id;
		LocString arg;
		if (id != SimHashes.Oxygen)
		{
			if (id != SimHashes.ContaminatedOxygen)
			{
				c = GameUtil.BreathableValues.negativeColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND4;
			}
			else if (Mass >= SimDebugView.optimallyBreathable)
			{
				c = GameUtil.BreathableValues.positiveColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND1;
			}
			else if (Mass >= SimDebugView.minimumBreathable + (SimDebugView.optimallyBreathable - SimDebugView.minimumBreathable) / 2f)
			{
				c = GameUtil.BreathableValues.positiveColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND2;
			}
			else if (Mass >= SimDebugView.minimumBreathable)
			{
				c = GameUtil.BreathableValues.warningColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND3;
			}
			else
			{
				c = GameUtil.BreathableValues.negativeColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND4;
			}
		}
		else if (Mass >= SimDebugView.optimallyBreathable)
		{
			c = GameUtil.BreathableValues.positiveColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND1;
		}
		else if (Mass >= SimDebugView.minimumBreathable + (SimDebugView.optimallyBreathable - SimDebugView.minimumBreathable) / 2f)
		{
			c = GameUtil.BreathableValues.positiveColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND2;
		}
		else if (Mass >= SimDebugView.minimumBreathable)
		{
			c = GameUtil.BreathableValues.warningColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND3;
		}
		else
		{
			c = GameUtil.BreathableValues.negativeColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND4;
		}
		return string.Format(ELEMENTS.BREATHABLEDESC, c.ToHexString(), arg);
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x00137AAC File Offset: 0x00135CAC
	public static string GetWireLoadColor(float load, float maxLoad, float potentialLoad)
	{
		Color c;
		if (load > maxLoad + POWER.FLOAT_FUDGE_FACTOR)
		{
			c = GameUtil.WireLoadValues.negativeColor;
		}
		else if (potentialLoad > maxLoad && load / maxLoad >= 0.75f)
		{
			c = GameUtil.WireLoadValues.warningColor;
		}
		else
		{
			c = Color.white;
		}
		return c.ToHexString();
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x00137AED File Offset: 0x00135CED
	public static string GetHotkeyString(global::Action action)
	{
		if (KInputManager.currentControllerIsGamepad)
		{
			return UI.FormatAsHotkey(GameUtil.GetActionString(action));
		}
		return UI.FormatAsHotkey("[" + GameUtil.GetActionString(action) + "]");
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x00137B1C File Offset: 0x00135D1C
	public static string ReplaceHotkeyString(string template, global::Action action)
	{
		return template.Replace("{Hotkey}", GameUtil.GetHotkeyString(action));
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x00137B2F File Offset: 0x00135D2F
	public static string ReplaceHotkeyString(string template, global::Action action1, global::Action action2)
	{
		return template.Replace("{Hotkey}", GameUtil.GetHotkeyString(action1) + GameUtil.GetHotkeyString(action2));
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x00137B50 File Offset: 0x00135D50
	public static string GetKeycodeLocalized(KKeyCode key_code)
	{
		string result = key_code.ToString();
		if (key_code <= KKeyCode.Slash)
		{
			if (key_code <= KKeyCode.Tab)
			{
				if (key_code == KKeyCode.None)
				{
					return result;
				}
				if (key_code == KKeyCode.Backspace)
				{
					return INPUT.BACKSPACE;
				}
				if (key_code == KKeyCode.Tab)
				{
					return INPUT.TAB;
				}
			}
			else if (key_code <= KKeyCode.Escape)
			{
				if (key_code == KKeyCode.Return)
				{
					return INPUT.ENTER;
				}
				if (key_code == KKeyCode.Escape)
				{
					return INPUT.ESCAPE;
				}
			}
			else
			{
				if (key_code == KKeyCode.Space)
				{
					return INPUT.SPACE;
				}
				switch (key_code)
				{
				case KKeyCode.Plus:
					return "+";
				case KKeyCode.Comma:
					return ",";
				case KKeyCode.Minus:
					return "-";
				case KKeyCode.Period:
					return INPUT.PERIOD;
				case KKeyCode.Slash:
					return "/";
				}
			}
		}
		else if (key_code <= KKeyCode.Insert)
		{
			switch (key_code)
			{
			case KKeyCode.Colon:
				return ":";
			case KKeyCode.Semicolon:
				return ";";
			case KKeyCode.Less:
				break;
			case KKeyCode.Equals:
				return "=";
			default:
				switch (key_code)
				{
				case KKeyCode.LeftBracket:
					return "[";
				case KKeyCode.Backslash:
					return "\\";
				case KKeyCode.RightBracket:
					return "]";
				case KKeyCode.Caret:
				case KKeyCode.Underscore:
					break;
				case KKeyCode.BackQuote:
					return INPUT.BACKQUOTE;
				default:
					switch (key_code)
					{
					case KKeyCode.Keypad0:
						return INPUT.NUM + " 0";
					case KKeyCode.Keypad1:
						return INPUT.NUM + " 1";
					case KKeyCode.Keypad2:
						return INPUT.NUM + " 2";
					case KKeyCode.Keypad3:
						return INPUT.NUM + " 3";
					case KKeyCode.Keypad4:
						return INPUT.NUM + " 4";
					case KKeyCode.Keypad5:
						return INPUT.NUM + " 5";
					case KKeyCode.Keypad6:
						return INPUT.NUM + " 6";
					case KKeyCode.Keypad7:
						return INPUT.NUM + " 7";
					case KKeyCode.Keypad8:
						return INPUT.NUM + " 8";
					case KKeyCode.Keypad9:
						return INPUT.NUM + " 9";
					case KKeyCode.KeypadPeriod:
						return INPUT.NUM + " " + INPUT.PERIOD;
					case KKeyCode.KeypadDivide:
						return INPUT.NUM + " /";
					case KKeyCode.KeypadMultiply:
						return INPUT.NUM + " *";
					case KKeyCode.KeypadMinus:
						return INPUT.NUM + " -";
					case KKeyCode.KeypadPlus:
						return INPUT.NUM + " +";
					case KKeyCode.KeypadEnter:
						return INPUT.NUM + " " + INPUT.ENTER;
					case KKeyCode.Insert:
						return INPUT.INSERT;
					}
					break;
				}
				break;
			}
		}
		else if (key_code <= KKeyCode.Mouse6)
		{
			switch (key_code)
			{
			case KKeyCode.RightShift:
				return INPUT.RIGHT_SHIFT;
			case KKeyCode.LeftShift:
				return INPUT.LEFT_SHIFT;
			case KKeyCode.RightControl:
				return INPUT.RIGHT_CTRL;
			case KKeyCode.LeftControl:
				return INPUT.LEFT_CTRL;
			case KKeyCode.RightAlt:
				return INPUT.RIGHT_ALT;
			case KKeyCode.LeftAlt:
				return INPUT.LEFT_ALT;
			default:
				switch (key_code)
				{
				case KKeyCode.Mouse0:
					return INPUT.MOUSE + " 0";
				case KKeyCode.Mouse1:
					return INPUT.MOUSE + " 1";
				case KKeyCode.Mouse2:
					return INPUT.MOUSE + " 2";
				case KKeyCode.Mouse3:
					return INPUT.MOUSE + " 3";
				case KKeyCode.Mouse4:
					return INPUT.MOUSE + " 4";
				case KKeyCode.Mouse5:
					return INPUT.MOUSE + " 5";
				case KKeyCode.Mouse6:
					return INPUT.MOUSE + " 6";
				}
				break;
			}
		}
		else
		{
			if (key_code == KKeyCode.MouseScrollDown)
			{
				return INPUT.MOUSE_SCROLL_DOWN;
			}
			if (key_code == KKeyCode.MouseScrollUp)
			{
				return INPUT.MOUSE_SCROLL_UP;
			}
		}
		if (KKeyCode.A <= key_code && key_code <= KKeyCode.Z)
		{
			result = ((char)(65 + (key_code - KKeyCode.A))).ToString();
		}
		else if (KKeyCode.Alpha0 <= key_code && key_code <= KKeyCode.Alpha9)
		{
			result = ((char)(48 + (key_code - KKeyCode.Alpha0))).ToString();
		}
		else if (KKeyCode.F1 <= key_code && key_code <= KKeyCode.F12)
		{
			result = "F" + (key_code - KKeyCode.F1 + 1).ToString();
		}
		else
		{
			global::Debug.LogWarning("Unable to find proper string for KKeyCode: " + key_code.ToString() + " using key_code.ToString()");
		}
		return result;
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x00138158 File Offset: 0x00136358
	public static string GetActionString(global::Action action)
	{
		string result = "";
		if (action == global::Action.NumActions)
		{
			return result;
		}
		BindingEntry bindingEntry = GameUtil.ActionToBinding(action);
		KKeyCode mKeyCode = bindingEntry.mKeyCode;
		if (KInputManager.currentControllerIsGamepad)
		{
			return KInputManager.steamInputInterpreter.GetActionGlyph(action);
		}
		if (bindingEntry.mModifier == global::Modifier.None)
		{
			return GameUtil.GetKeycodeLocalized(mKeyCode).ToUpper();
		}
		string str = "";
		global::Modifier mModifier = bindingEntry.mModifier;
		switch (mModifier)
		{
		case global::Modifier.Alt:
			str = GameUtil.GetKeycodeLocalized(KKeyCode.LeftAlt).ToUpper();
			break;
		case global::Modifier.Ctrl:
			str = GameUtil.GetKeycodeLocalized(KKeyCode.LeftControl).ToUpper();
			break;
		case (global::Modifier)3:
			break;
		case global::Modifier.Shift:
			str = GameUtil.GetKeycodeLocalized(KKeyCode.LeftShift).ToUpper();
			break;
		default:
			if (mModifier != global::Modifier.CapsLock)
			{
				if (mModifier == global::Modifier.Backtick)
				{
					str = GameUtil.GetKeycodeLocalized(KKeyCode.BackQuote).ToUpper();
				}
			}
			else
			{
				str = GameUtil.GetKeycodeLocalized(KKeyCode.CapsLock).ToUpper();
			}
			break;
		}
		return str + " + " + GameUtil.GetKeycodeLocalized(mKeyCode).ToUpper();
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x0013824C File Offset: 0x0013644C
	public static void CreateExplosion(Vector3 explosion_pos)
	{
		Vector2 b = new Vector2(explosion_pos.x, explosion_pos.y);
		float num = 5f;
		float num2 = num * num;
		foreach (Health health in Components.Health.Items)
		{
			Vector3 position = health.transform.GetPosition();
			float sqrMagnitude = (new Vector2(position.x, position.y) - b).sqrMagnitude;
			if (num2 >= sqrMagnitude && health != null)
			{
				health.Damage(health.maxHitPoints);
			}
		}
	}

	// Token: 0x06003845 RID: 14405 RVA: 0x00138304 File Offset: 0x00136504
	private static void GetNonSolidCells(int x, int y, List<int> cells, int min_x, int min_y, int max_x, int max_y)
	{
		int num = Grid.XYToCell(x, y);
		if (Grid.IsValidCell(num) && !Grid.Solid[num] && !Grid.DupePassable[num] && x >= min_x && x <= max_x && y >= min_y && y <= max_y && !cells.Contains(num))
		{
			cells.Add(num);
			GameUtil.GetNonSolidCells(x + 1, y, cells, min_x, min_y, max_x, max_y);
			GameUtil.GetNonSolidCells(x - 1, y, cells, min_x, min_y, max_x, max_y);
			GameUtil.GetNonSolidCells(x, y + 1, cells, min_x, min_y, max_x, max_y);
			GameUtil.GetNonSolidCells(x, y - 1, cells, min_x, min_y, max_x, max_y);
		}
	}

	// Token: 0x06003846 RID: 14406 RVA: 0x001383A8 File Offset: 0x001365A8
	public static void GetNonSolidCells(int cell, int radius, List<int> cells)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		GameUtil.GetNonSolidCells(num, num2, cells, num - radius, num2 - radius, num + radius, num2 + radius);
	}

	// Token: 0x06003847 RID: 14407 RVA: 0x001383D8 File Offset: 0x001365D8
	public static float GetMaxStressInActiveWorld()
	{
		if (Components.LiveMinionIdentities.Count <= 0)
		{
			return 0f;
		}
		float num = 0f;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Stress.Lookup(minionIdentity);
				if (amountInstance != null)
				{
					num = Mathf.Max(num, amountInstance.value);
				}
			}
		}
		return num;
	}

	// Token: 0x06003848 RID: 14408 RVA: 0x00138484 File Offset: 0x00136684
	public static float GetAverageStressInActiveWorld()
	{
		if (Components.LiveMinionIdentities.Count <= 0)
		{
			return 0f;
		}
		float num = 0f;
		int num2 = 0;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
			{
				num += Db.Get().Amounts.Stress.Lookup(minionIdentity).value;
				num2++;
			}
		}
		return num / (float)num2;
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x00138530 File Offset: 0x00136730
	public static string MigrateFMOD(FMODAsset asset)
	{
		if (asset == null)
		{
			return null;
		}
		if (asset.path == null)
		{
			return asset.name;
		}
		return asset.path;
	}

	// Token: 0x0600384A RID: 14410 RVA: 0x00138552 File Offset: 0x00136752
	private static void SortGameObjectDescriptors(List<IGameObjectEffectDescriptor> descriptorList)
	{
		descriptorList.Sort(delegate(IGameObjectEffectDescriptor e1, IGameObjectEffectDescriptor e2)
		{
			int num = TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e1.GetType());
			int value = TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e2.GetType());
			return num.CompareTo(value);
		});
	}

	// Token: 0x0600384B RID: 14411 RVA: 0x0013857C File Offset: 0x0013677C
	public static void IndentListOfDescriptors(List<Descriptor> list, int indentCount = 1)
	{
		for (int i = 0; i < list.Count; i++)
		{
			Descriptor value = list[i];
			for (int j = 0; j < indentCount; j++)
			{
				value.IncreaseIndent();
			}
			list[i] = value;
		}
	}

	// Token: 0x0600384C RID: 14412 RVA: 0x001385C0 File Offset: 0x001367C0
	public static List<Descriptor> GetAllDescriptors(GameObject go, bool simpleInfoScreen = false)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<IGameObjectEffectDescriptor> list2 = new List<IGameObjectEffectDescriptor>(go.GetComponents<IGameObjectEffectDescriptor>());
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			list2.AddRange(component.GetDescriptors());
		}
		GameUtil.SortGameObjectDescriptors(list2);
		foreach (IGameObjectEffectDescriptor gameObjectEffectDescriptor in list2)
		{
			List<Descriptor> descriptors = gameObjectEffectDescriptor.GetDescriptors(go);
			if (descriptors != null)
			{
				foreach (Descriptor descriptor in descriptors)
				{
					if (!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen)
					{
						list.Add(descriptor);
					}
				}
			}
		}
		KPrefabID component2 = go.GetComponent<KPrefabID>();
		if (component2 != null && component2.AdditionalRequirements != null)
		{
			foreach (Descriptor descriptor2 in component2.AdditionalRequirements)
			{
				if (!descriptor2.onlyForSimpleInfoScreen || simpleInfoScreen)
				{
					list.Add(descriptor2);
				}
			}
		}
		if (component2 != null && component2.AdditionalEffects != null)
		{
			foreach (Descriptor descriptor3 in component2.AdditionalEffects)
			{
				if (!descriptor3.onlyForSimpleInfoScreen || simpleInfoScreen)
				{
					list.Add(descriptor3);
				}
			}
		}
		return list;
	}

	// Token: 0x0600384D RID: 14413 RVA: 0x00138760 File Offset: 0x00136960
	public static List<Descriptor> GetDetailDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Detail)
			{
				list.Add(descriptor);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x0600384E RID: 14414 RVA: 0x001387C8 File Offset: 0x001369C8
	public static List<Descriptor> GetRequirementDescriptors(List<Descriptor> descriptors)
	{
		return GameUtil.GetRequirementDescriptors(descriptors, true);
	}

	// Token: 0x0600384F RID: 14415 RVA: 0x001387D4 File Offset: 0x001369D4
	public static List<Descriptor> GetRequirementDescriptors(List<Descriptor> descriptors, bool indent)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Requirement)
			{
				list.Add(descriptor);
			}
		}
		if (indent)
		{
			GameUtil.IndentListOfDescriptors(list, 1);
		}
		return list;
	}

	// Token: 0x06003850 RID: 14416 RVA: 0x0013883C File Offset: 0x00136A3C
	public static List<Descriptor> GetEffectDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource)
			{
				list.Add(descriptor);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x06003851 RID: 14417 RVA: 0x001388AC File Offset: 0x00136AAC
	public static List<Descriptor> GetInformationDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Lifecycle)
			{
				list.Add(descriptor);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x06003852 RID: 14418 RVA: 0x00138914 File Offset: 0x00136B14
	public static List<Descriptor> GetCropOptimumConditionDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Lifecycle)
			{
				Descriptor descriptor2 = descriptor;
				descriptor2.text = "• " + descriptor2.text;
				list.Add(descriptor2);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x06003853 RID: 14419 RVA: 0x00138994 File Offset: 0x00136B94
	public static List<Descriptor> GetGameObjectRequirements(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<IGameObjectEffectDescriptor> list2 = new List<IGameObjectEffectDescriptor>(go.GetComponents<IGameObjectEffectDescriptor>());
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			list2.AddRange(component.GetDescriptors());
		}
		GameUtil.SortGameObjectDescriptors(list2);
		foreach (IGameObjectEffectDescriptor gameObjectEffectDescriptor in list2)
		{
			List<Descriptor> descriptors = gameObjectEffectDescriptor.GetDescriptors(go);
			if (descriptors != null)
			{
				foreach (Descriptor descriptor in descriptors)
				{
					if (descriptor.type == Descriptor.DescriptorType.Requirement)
					{
						list.Add(descriptor);
					}
				}
			}
		}
		KPrefabID component2 = go.GetComponent<KPrefabID>();
		if (component2.AdditionalRequirements != null)
		{
			list.AddRange(component2.AdditionalRequirements);
		}
		return list;
	}

	// Token: 0x06003854 RID: 14420 RVA: 0x00138A84 File Offset: 0x00136C84
	public static List<Descriptor> GetGameObjectEffects(GameObject go, bool simpleInfoScreen = false)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<IGameObjectEffectDescriptor> list2 = new List<IGameObjectEffectDescriptor>(go.GetComponents<IGameObjectEffectDescriptor>());
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			list2.AddRange(component.GetDescriptors());
		}
		GameUtil.SortGameObjectDescriptors(list2);
		foreach (IGameObjectEffectDescriptor gameObjectEffectDescriptor in list2)
		{
			List<Descriptor> descriptors = gameObjectEffectDescriptor.GetDescriptors(go);
			if (descriptors != null)
			{
				foreach (Descriptor descriptor in descriptors)
				{
					if ((!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen) && (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource))
					{
						list.Add(descriptor);
					}
				}
			}
		}
		KPrefabID component2 = go.GetComponent<KPrefabID>();
		if (component2 != null && component2.AdditionalEffects != null)
		{
			foreach (Descriptor descriptor2 in component2.AdditionalEffects)
			{
				if (!descriptor2.onlyForSimpleInfoScreen || simpleInfoScreen)
				{
					list.Add(descriptor2);
				}
			}
		}
		return list;
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x00138BD8 File Offset: 0x00136DD8
	public static List<Descriptor> GetPlantRequirementDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(GameUtil.GetAllDescriptors(go, false));
		if (requirementDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTREQUIREMENTS, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTREQUIREMENTS, Descriptor.DescriptorType.Requirement);
			list.Add(item);
			list.AddRange(requirementDescriptors);
		}
		return list;
	}

	// Token: 0x06003856 RID: 14422 RVA: 0x00138C34 File Offset: 0x00136E34
	public static List<Descriptor> GetPlantLifeCycleDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<Descriptor> informationDescriptors = GameUtil.GetInformationDescriptors(GameUtil.GetAllDescriptors(go, false));
		if (informationDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.LIFECYCLE, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTLIFECYCLE, Descriptor.DescriptorType.Lifecycle);
			list.Add(item);
			list.AddRange(informationDescriptors);
		}
		return list;
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x00138C90 File Offset: 0x00136E90
	public static List<Descriptor> GetPlantEffectDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (go.GetComponent<Growing>() == null)
		{
			return list;
		}
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(go, false);
		List<Descriptor> list2 = new List<Descriptor>();
		list2.AddRange(GameUtil.GetEffectDescriptors(allDescriptors));
		if (list2.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTEFFECTS, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTEFFECTS, Descriptor.DescriptorType.Effect);
			list.Add(item);
			list.AddRange(list2);
		}
		return list;
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x00138D0C File Offset: 0x00136F0C
	public static string GetGameObjectEffectsTooltipString(GameObject go)
	{
		string text = "";
		List<Descriptor> gameObjectEffects = GameUtil.GetGameObjectEffects(go, false);
		if (gameObjectEffects.Count > 0)
		{
			text = text + UI.BUILDINGEFFECTS.OPERATIONEFFECTS + "\n";
		}
		foreach (Descriptor descriptor in gameObjectEffects)
		{
			text = text + descriptor.IndentedText() + "\n";
		}
		return text;
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x00138D94 File Offset: 0x00136F94
	public static List<Descriptor> GetEquipmentEffects(EquipmentDef def)
	{
		global::Debug.Assert(def != null);
		List<Descriptor> list = new List<Descriptor>();
		List<AttributeModifier> attributeModifiers = def.AttributeModifiers;
		if (attributeModifiers != null)
		{
			foreach (AttributeModifier attributeModifier in attributeModifiers)
			{
				string name = Db.Get().Attributes.Get(attributeModifier.AttributeId).Name;
				string formattedString = attributeModifier.GetFormattedString();
				string newValue = (attributeModifier.Value >= 0f) ? "produced" : "consumed";
				string text = UI.GAMEOBJECTEFFECTS.EQUIPMENT_MODS.text.Replace("{Attribute}", name).Replace("{Style}", newValue).Replace("{Value}", formattedString);
				list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
			}
		}
		return list;
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x00138E84 File Offset: 0x00137084
	public static string GetRecipeDescription(Recipe recipe)
	{
		string text = null;
		if (recipe != null)
		{
			text = recipe.recipeDescription;
		}
		if (text == null)
		{
			text = RESEARCH.TYPES.MISSINGRECIPEDESC;
			global::Debug.LogWarning("Missing recipeDescription");
		}
		return text;
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x00138EB6 File Offset: 0x001370B6
	public static int GetCurrentCycle()
	{
		return GameClock.Instance.GetCycle() + 1;
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x00138EC4 File Offset: 0x001370C4
	public static float GetCurrentTimeInCycles()
	{
		return GameClock.Instance.GetTimeInCycles() + 1f;
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x00138ED8 File Offset: 0x001370D8
	public static GameObject GetActiveTelepad()
	{
		GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.activeWorldId);
		if (telepad == null)
		{
			telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
		}
		return telepad;
	}

	// Token: 0x0600385E RID: 14430 RVA: 0x00138F14 File Offset: 0x00137114
	public static GameObject GetTelepad(int worldId)
	{
		if (Components.Telepads.Count > 0)
		{
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				if (Components.Telepads[i].GetMyWorldId() == worldId)
				{
					return Components.Telepads[i].gameObject;
				}
			}
		}
		return null;
	}

	// Token: 0x0600385F RID: 14431 RVA: 0x00138F68 File Offset: 0x00137168
	public static GameObject KInstantiate(GameObject original, Vector3 position, Grid.SceneLayer sceneLayer, string name = null, int gameLayer = 0)
	{
		return GameUtil.KInstantiate(original, position, sceneLayer, null, name, gameLayer);
	}

	// Token: 0x06003860 RID: 14432 RVA: 0x00138F76 File Offset: 0x00137176
	public static GameObject KInstantiate(GameObject original, Vector3 position, Grid.SceneLayer sceneLayer, GameObject parent, string name = null, int gameLayer = 0)
	{
		position.z = Grid.GetLayerZ(sceneLayer);
		return Util.KInstantiate(original, position, Quaternion.identity, parent, name, true, gameLayer);
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x00138F97 File Offset: 0x00137197
	public static GameObject KInstantiate(GameObject original, Grid.SceneLayer sceneLayer, string name = null, int gameLayer = 0)
	{
		return GameUtil.KInstantiate(original, Vector3.zero, sceneLayer, name, gameLayer);
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x00138FA7 File Offset: 0x001371A7
	public static GameObject KInstantiate(Component original, Grid.SceneLayer sceneLayer, string name = null, int gameLayer = 0)
	{
		return GameUtil.KInstantiate(original.gameObject, Vector3.zero, sceneLayer, name, gameLayer);
	}

	// Token: 0x06003863 RID: 14435 RVA: 0x00138FBC File Offset: 0x001371BC
	public unsafe static void IsEmissionBlocked(int cell, out bool all_not_gaseous, out bool all_over_pressure)
	{
		int* ptr = stackalloc int[(UIntPtr)16];
		*ptr = Grid.CellBelow(cell);
		ptr[1] = Grid.CellLeft(cell);
		ptr[2] = Grid.CellRight(cell);
		ptr[3] = Grid.CellAbove(cell);
		all_not_gaseous = true;
		all_over_pressure = true;
		for (int i = 0; i < 4; i++)
		{
			int num = ptr[i];
			if (Grid.IsValidCell(num))
			{
				Element element = Grid.Element[num];
				all_not_gaseous = (all_not_gaseous && !element.IsGas && !element.IsVacuum);
				all_over_pressure = (all_over_pressure && ((!element.IsGas && !element.IsVacuum) || Grid.Mass[num] >= 1.8f));
			}
		}
	}

	// Token: 0x06003864 RID: 14436 RVA: 0x00139074 File Offset: 0x00137274
	public static float GetDecorAtCell(int cell)
	{
		float num = 0f;
		if (!Grid.Solid[cell])
		{
			num = Grid.Decor[cell];
			num += (float)DecorProvider.GetLightDecorBonus(cell);
		}
		return num;
	}

	// Token: 0x06003865 RID: 14437 RVA: 0x001390A8 File Offset: 0x001372A8
	public static string GetUnitTypeMassOrUnit(GameObject go)
	{
		string result = UI.UNITSUFFIXES.UNITS;
		KPrefabID component = go.GetComponent<KPrefabID>();
		if (component != null)
		{
			result = (component.Tags.Contains(GameTags.Seed) ? UI.UNITSUFFIXES.UNITS : UI.UNITSUFFIXES.MASS.KILOGRAM);
		}
		return result;
	}

	// Token: 0x06003866 RID: 14438 RVA: 0x001390F8 File Offset: 0x001372F8
	public static string GetKeywordStyle(Tag tag)
	{
		Element element = ElementLoader.GetElement(tag);
		string result;
		if (element != null)
		{
			result = GameUtil.GetKeywordStyle(element);
		}
		else if (GameUtil.foodTags.Contains(tag))
		{
			result = "food";
		}
		else if (GameUtil.solidTags.Contains(tag))
		{
			result = "solid";
		}
		else
		{
			result = null;
		}
		return result;
	}

	// Token: 0x06003867 RID: 14439 RVA: 0x00139148 File Offset: 0x00137348
	public static string GetKeywordStyle(SimHashes hash)
	{
		Element element = ElementLoader.FindElementByHash(hash);
		if (element != null)
		{
			return GameUtil.GetKeywordStyle(element);
		}
		return null;
	}

	// Token: 0x06003868 RID: 14440 RVA: 0x00139168 File Offset: 0x00137368
	public static string GetKeywordStyle(Element element)
	{
		if (element.id == SimHashes.Oxygen)
		{
			return "oxygen";
		}
		if (element.IsSolid)
		{
			return "solid";
		}
		if (element.IsLiquid)
		{
			return "liquid";
		}
		if (element.IsGas)
		{
			return "gas";
		}
		if (element.IsVacuum)
		{
			return "vacuum";
		}
		return null;
	}

	// Token: 0x06003869 RID: 14441 RVA: 0x001391C4 File Offset: 0x001373C4
	public static string GetKeywordStyle(GameObject go)
	{
		string result = "";
		UnityEngine.Object component = go.GetComponent<Edible>();
		Equippable component2 = go.GetComponent<Equippable>();
		MedicinalPill component3 = go.GetComponent<MedicinalPill>();
		ResearchPointObject component4 = go.GetComponent<ResearchPointObject>();
		if (component != null)
		{
			result = "food";
		}
		else if (component2 != null)
		{
			result = "equipment";
		}
		else if (component3 != null)
		{
			result = "medicine";
		}
		else if (component4 != null)
		{
			result = "research";
		}
		return result;
	}

	// Token: 0x0600386A RID: 14442 RVA: 0x00139234 File Offset: 0x00137434
	public static Sprite GetBiomeSprite(string id)
	{
		string text = "biomeIcon" + char.ToUpper(id[0]).ToString() + id.Substring(1).ToLower();
		Sprite sprite = Assets.GetSprite(text);
		if (sprite != null)
		{
			return new global::Tuple<Sprite, Color>(sprite, Color.white).first;
		}
		global::Debug.LogWarning("Missing codex biome icon: " + text);
		return null;
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x001392A4 File Offset: 0x001374A4
	public static string GenerateRandomDuplicantName()
	{
		string text = "";
		string text2 = "";
		bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
		List<string> list = new List<string>(LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.NAME.NB)));
		list.AddRange(flag ? LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.NAME.MALE)) : LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.NAME.FEMALE)));
		string random = list.GetRandom<string>();
		if (UnityEngine.Random.Range(0f, 1f) > 0.7f)
		{
			List<string> list2 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.PREFIX.NB)));
			list2.AddRange(flag ? LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.PREFIX.MALE)) : LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.PREFIX.FEMALE)));
			text = list2.GetRandom<string>();
		}
		if (!string.IsNullOrEmpty(text))
		{
			text += " ";
		}
		if (UnityEngine.Random.Range(0f, 1f) >= 0.9f)
		{
			List<string> list3 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.SUFFIX.NB)));
			list3.AddRange(flag ? LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.SUFFIX.MALE)) : LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.SUFFIX.FEMALE)));
			text2 = list3.GetRandom<string>();
		}
		if (!string.IsNullOrEmpty(text2))
		{
			text2 = " " + text2;
		}
		return text + random + text2;
	}

	// Token: 0x0600386C RID: 14444 RVA: 0x0013940C File Offset: 0x0013760C
	public static string GenerateRandomLaunchPadName()
	{
		return NAMEGEN.LAUNCHPAD.FORMAT.Replace("{Name}", UnityEngine.Random.Range(1, 1000).ToString());
	}

	// Token: 0x0600386D RID: 14445 RVA: 0x0013943C File Offset: 0x0013763C
	public static string GenerateRandomRocketName()
	{
		string newValue = "";
		string newValue2 = "";
		string newValue3 = "";
		int num = 1;
		int num2 = 2;
		int num3 = 4;
		string random = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.NOUN))).GetRandom<string>();
		int num4 = 0;
		if (UnityEngine.Random.value > 0.7f)
		{
			newValue = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.PREFIX))).GetRandom<string>();
			num4 |= num;
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			newValue2 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.ADJECTIVE))).GetRandom<string>();
			num4 |= num2;
		}
		if (UnityEngine.Random.value > 0.1f)
		{
			newValue3 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.SUFFIX))).GetRandom<string>();
			num4 |= num3;
		}
		string text;
		if (num4 == (num | num2 | num3))
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN_SUFFIX;
		}
		else if (num4 == (num2 | num3))
		{
			text = NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN_SUFFIX;
		}
		else if (num4 == (num | num3))
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_NOUN_SUFFIX;
		}
		else if (num4 == num3)
		{
			text = NAMEGEN.ROCKET.FMT_NOUN_SUFFIX;
		}
		else if (num4 == (num | num2))
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN;
		}
		else if (num4 == num)
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_NOUN;
		}
		else if (num4 == num2)
		{
			text = NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN;
		}
		else
		{
			text = NAMEGEN.ROCKET.FMT_NOUN;
		}
		DebugUtil.LogArgs(new object[]
		{
			"Rocket name bits:",
			Convert.ToString(num4, 2)
		});
		return text.Replace("{Prefix}", newValue).Replace("{Adjective}", newValue2).Replace("{Noun}", random).Replace("{Suffix}", newValue3);
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x00139604 File Offset: 0x00137804
	public static string GenerateRandomWorldName(string[] nameTables)
	{
		if (nameTables == null)
		{
			global::Debug.LogWarning("No name tables provided to generate world name. Using GENERIC");
			nameTables = new string[]
			{
				"GENERIC"
			};
		}
		string text = "";
		foreach (string text2 in nameTables)
		{
			text += Strings.Get("STRINGS.NAMEGEN.WORLD.ROOTS." + text2.ToUpper());
		}
		string text3 = GameUtil.RandomValueFromSeparatedString(text, "\n");
		if (string.IsNullOrEmpty(text3))
		{
			text3 = GameUtil.RandomValueFromSeparatedString(Strings.Get(NAMEGEN.WORLD.ROOTS.GENERIC), "\n");
		}
		string str = GameUtil.RandomValueFromSeparatedString(NAMEGEN.WORLD.SUFFIXES.GENERICLIST, "\n");
		return text3 + str;
	}

	// Token: 0x0600386F RID: 14447 RVA: 0x001396C0 File Offset: 0x001378C0
	public static float GetThermalComfort(int cell, float tolerance = -0.08368001f)
	{
		float num = 0f;
		Element element = ElementLoader.FindElementByHash(SimHashes.Creature);
		if (Grid.Element[cell].thermalConductivity != 0f)
		{
			num = SimUtil.CalculateEnergyFlowCreatures(cell, 310.15f, element.specificHeatCapacity, element.thermalConductivity, 1f, 0.0045f);
		}
		num -= tolerance;
		return num * 1000f;
	}

	// Token: 0x06003870 RID: 14448 RVA: 0x00139724 File Offset: 0x00137924
	public static string RandomValueFromSeparatedString(string source, string separator = "\n")
	{
		int num = 0;
		int num2 = 0;
		for (;;)
		{
			num = source.IndexOf(separator, num);
			if (num == -1)
			{
				break;
			}
			num += separator.Length;
			num2++;
		}
		if (num2 == 0)
		{
			return "";
		}
		int num3 = UnityEngine.Random.Range(0, num2);
		num = 0;
		for (int i = 0; i < num3; i++)
		{
			num = source.IndexOf(separator, num) + separator.Length;
		}
		int num4 = source.IndexOf(separator, num);
		return source.Substring(num, (num4 == -1) ? (source.Length - num) : (num4 - num));
	}

	// Token: 0x06003871 RID: 14449 RVA: 0x001397A8 File Offset: 0x001379A8
	public static string GetFormattedDiseaseName(byte idx, bool color = false)
	{
		Disease disease = Db.Get().Diseases[(int)idx];
		if (color)
		{
			return string.Format(UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT, disease.Name, GameUtil.ColourToHex(GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName)));
		}
		return string.Format(UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT_NO_COLOR, disease.Name);
	}

	// Token: 0x06003872 RID: 14450 RVA: 0x00139810 File Offset: 0x00137A10
	public static string GetFormattedDisease(byte idx, int units, bool color = false)
	{
		if (idx == 255 || units <= 0)
		{
			return UI.OVERLAYS.DISEASE.NO_DISEASE;
		}
		Disease disease = Db.Get().Diseases[(int)idx];
		if (color)
		{
			return string.Format(UI.OVERLAYS.DISEASE.DISEASE_FORMAT, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None), GameUtil.ColourToHex(GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName)));
		}
		return string.Format(UI.OVERLAYS.DISEASE.DISEASE_FORMAT_NO_COLOR, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None));
	}

	// Token: 0x06003873 RID: 14451 RVA: 0x0013989B File Offset: 0x00137A9B
	public static string GetFormattedDiseaseAmount(int units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		GameUtil.ApplyTimeSlice(units, timeSlice);
		return GameUtil.AddTimeSliceText(units.ToString("#,##0") + UI.UNITSUFFIXES.DISEASE.UNITS, timeSlice);
	}

	// Token: 0x06003874 RID: 14452 RVA: 0x001398C6 File Offset: 0x00137AC6
	public static string ColourizeString(Color32 colour, string str)
	{
		return string.Format("<color=#{0}>{1}</color>", GameUtil.ColourToHex(colour), str);
	}

	// Token: 0x06003875 RID: 14453 RVA: 0x001398DC File Offset: 0x00137ADC
	public static string ColourToHex(Color32 colour)
	{
		return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[]
		{
			colour.r,
			colour.g,
			colour.b,
			colour.a
		});
	}

	// Token: 0x06003876 RID: 14454 RVA: 0x00139934 File Offset: 0x00137B34
	public static string GetFormattedDecor(float value, bool enforce_max = false)
	{
		string arg = "";
		LocString loc_string = (value > DecorMonitor.MAXIMUM_DECOR_VALUE && enforce_max) ? UI.OVERLAYS.DECOR.MAXIMUM_DECOR : UI.OVERLAYS.DECOR.VALUE;
		if (enforce_max)
		{
			value = Math.Min(value, DecorMonitor.MAXIMUM_DECOR_VALUE);
		}
		if (value > 0f)
		{
			arg = "+";
		}
		else if (value >= 0f)
		{
			loc_string = UI.OVERLAYS.DECOR.VALUE_ZERO;
		}
		return string.Format(loc_string, arg, value);
	}

	// Token: 0x06003877 RID: 14455 RVA: 0x001399A0 File Offset: 0x00137BA0
	public static Color GetDecorColourFromValue(int decor)
	{
		Color result = Color.black;
		float num = (float)decor / 100f;
		if (num > 0f)
		{
			result = Color.Lerp(new Color(0.15f, 0f, 0f), new Color(0f, 1f, 0f), Mathf.Abs(num));
		}
		else
		{
			result = Color.Lerp(new Color(0.15f, 0f, 0f), new Color(1f, 0f, 0f), Mathf.Abs(num));
		}
		return result;
	}

	// Token: 0x06003878 RID: 14456 RVA: 0x00139A30 File Offset: 0x00137C30
	public static List<Descriptor> GetMaterialDescriptors(Element element)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (element.attributeModifiers.Count > 0)
		{
			foreach (AttributeModifier attributeModifier in element.attributeModifiers)
			{
				string txt = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
				string tooltip = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
				Descriptor item = default(Descriptor);
				item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
				item.IncreaseIndent();
				list.Add(item);
			}
		}
		list.AddRange(GameUtil.GetSignificantMaterialPropertyDescriptors(element));
		return list;
	}

	// Token: 0x06003879 RID: 14457 RVA: 0x00139B2C File Offset: 0x00137D2C
	public static string GetMaterialTooltips(Element element)
	{
		string text = element.tag.ProperName();
		foreach (AttributeModifier attributeModifier in element.attributeModifiers)
		{
			string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
			string formattedString = attributeModifier.GetFormattedString();
			text = text + "\n    • " + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name, formattedString);
		}
		text += GameUtil.GetSignificantMaterialPropertyTooltips(element);
		return text;
	}

	// Token: 0x0600387A RID: 14458 RVA: 0x00139BD4 File Offset: 0x00137DD4
	public static string GetSignificantMaterialPropertyTooltips(Element element)
	{
		string text = "";
		List<Descriptor> significantMaterialPropertyDescriptors = GameUtil.GetSignificantMaterialPropertyDescriptors(element);
		if (significantMaterialPropertyDescriptors.Count > 0)
		{
			text += "\n";
			for (int i = 0; i < significantMaterialPropertyDescriptors.Count; i++)
			{
				text = text + "    • " + Util.StripTextFormatting(significantMaterialPropertyDescriptors[i].text) + "\n";
			}
		}
		return text;
	}

	// Token: 0x0600387B RID: 14459 RVA: 0x00139C38 File Offset: 0x00137E38
	public static List<Descriptor> GetSignificantMaterialPropertyDescriptors(Element element)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (element.thermalConductivity > 10f)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(ELEMENTS.MATERIAL_MODIFIERS.HIGH_THERMAL_CONDUCTIVITY, GameUtil.GetThermalConductivityString(element, false, false)), string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_THERMAL_CONDUCTIVITY, element.name, element.thermalConductivity.ToString("0.#####")), Descriptor.DescriptorType.Effect);
			item.IncreaseIndent();
			list.Add(item);
		}
		if (element.thermalConductivity < 1f)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(string.Format(ELEMENTS.MATERIAL_MODIFIERS.LOW_THERMAL_CONDUCTIVITY, GameUtil.GetThermalConductivityString(element, false, false)), string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_THERMAL_CONDUCTIVITY, element.name, element.thermalConductivity.ToString("0.#####")), Descriptor.DescriptorType.Effect);
			item2.IncreaseIndent();
			list.Add(item2);
		}
		if (element.specificHeatCapacity <= 0.2f)
		{
			Descriptor item3 = default(Descriptor);
			item3.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.LOW_SPECIFIC_HEAT_CAPACITY, string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_SPECIFIC_HEAT_CAPACITY, element.name, element.specificHeatCapacity * 1f), Descriptor.DescriptorType.Effect);
			item3.IncreaseIndent();
			list.Add(item3);
		}
		if (element.specificHeatCapacity >= 1f)
		{
			Descriptor item4 = default(Descriptor);
			item4.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.HIGH_SPECIFIC_HEAT_CAPACITY, string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_SPECIFIC_HEAT_CAPACITY, element.name, element.specificHeatCapacity * 1f), Descriptor.DescriptorType.Effect);
			item4.IncreaseIndent();
			list.Add(item4);
		}
		if (Sim.IsRadiationEnabled() && element.radiationAbsorptionFactor >= 0.8f)
		{
			Descriptor item5 = default(Descriptor);
			item5.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EXCELLENT_RADIATION_SHIELD, string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EXCELLENT_RADIATION_SHIELD, element.name, element.radiationAbsorptionFactor), Descriptor.DescriptorType.Effect);
			item5.IncreaseIndent();
			list.Add(item5);
		}
		return list;
	}

	// Token: 0x0600387C RID: 14460 RVA: 0x00139E33 File Offset: 0x00138033
	public static int NaturalBuildingCell(this KMonoBehaviour cmp)
	{
		return Grid.PosToCell(cmp.transform.GetPosition());
	}

	// Token: 0x0600387D RID: 14461 RVA: 0x00139E48 File Offset: 0x00138048
	public static List<Descriptor> GetMaterialDescriptors(Tag tag)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.GetElement(tag);
		if (element != null)
		{
			if (element.attributeModifiers.Count > 0)
			{
				foreach (AttributeModifier attributeModifier in element.attributeModifiers)
				{
					string txt = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
					string tooltip = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
					Descriptor item = default(Descriptor);
					item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
					item.IncreaseIndent();
					list.Add(item);
				}
			}
			list.AddRange(GameUtil.GetSignificantMaterialPropertyDescriptors(element));
		}
		else
		{
			GameObject gameObject = Assets.TryGetPrefab(tag);
			if (gameObject != null)
			{
				PrefabAttributeModifiers component = gameObject.GetComponent<PrefabAttributeModifiers>();
				if (component != null)
				{
					foreach (AttributeModifier attributeModifier2 in component.descriptors)
					{
						string txt2 = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier2.AttributeId.ToUpper())), attributeModifier2.GetFormattedString());
						string tooltip2 = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier2.AttributeId.ToUpper())), attributeModifier2.GetFormattedString());
						Descriptor item2 = default(Descriptor);
						item2.SetupDescriptor(txt2, tooltip2, Descriptor.DescriptorType.Effect);
						item2.IncreaseIndent();
						list.Add(item2);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x0600387E RID: 14462 RVA: 0x0013A050 File Offset: 0x00138250
	public static string GetMaterialTooltips(Tag tag)
	{
		string text = tag.ProperName();
		Element element = ElementLoader.GetElement(tag);
		if (element != null)
		{
			foreach (AttributeModifier attributeModifier in element.attributeModifiers)
			{
				string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
				string formattedString = attributeModifier.GetFormattedString();
				text = text + "\n    • " + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name, formattedString);
			}
			text += GameUtil.GetSignificantMaterialPropertyTooltips(element);
		}
		else
		{
			GameObject gameObject = Assets.TryGetPrefab(tag);
			if (gameObject != null)
			{
				PrefabAttributeModifiers component = gameObject.GetComponent<PrefabAttributeModifiers>();
				if (component != null)
				{
					foreach (AttributeModifier attributeModifier2 in component.descriptors)
					{
						string name2 = Db.Get().BuildingAttributes.Get(attributeModifier2.AttributeId).Name;
						string formattedString2 = attributeModifier2.GetFormattedString();
						text = text + "\n    • " + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name2, formattedString2);
					}
				}
			}
		}
		return text;
	}

	// Token: 0x0600387F RID: 14463 RVA: 0x0013A1B0 File Offset: 0x001383B0
	public static bool AreChoresUIMergeable(Chore.Precondition.Context choreA, Chore.Precondition.Context choreB)
	{
		if (choreA.chore.target.isNull || choreB.chore.target.isNull)
		{
			return false;
		}
		ChoreType choreType = choreB.chore.choreType;
		ChoreType choreType2 = choreA.chore.choreType;
		return (choreA.chore.choreType == choreB.chore.choreType && choreA.chore.target.GetComponent<KPrefabID>().PrefabTag == choreB.chore.target.GetComponent<KPrefabID>().PrefabTag) || (choreA.chore.choreType == Db.Get().ChoreTypes.Dig && choreB.chore.choreType == Db.Get().ChoreTypes.Dig) || (choreA.chore.choreType == Db.Get().ChoreTypes.Relax && choreB.chore.choreType == Db.Get().ChoreTypes.Relax) || ((choreType2 == Db.Get().ChoreTypes.ReturnSuitIdle || choreType2 == Db.Get().ChoreTypes.ReturnSuitUrgent) && (choreType == Db.Get().ChoreTypes.ReturnSuitIdle || choreType == Db.Get().ChoreTypes.ReturnSuitUrgent)) || (choreA.chore.target.gameObject == choreB.chore.target.gameObject && choreA.chore.choreType == choreB.chore.choreType);
	}

	// Token: 0x06003880 RID: 14464 RVA: 0x0013A348 File Offset: 0x00138548
	public static string GetChoreName(Chore chore, object choreData)
	{
		string result = "";
		if (chore.choreType == Db.Get().ChoreTypes.Fetch || chore.choreType == Db.Get().ChoreTypes.MachineFetch || chore.choreType == Db.Get().ChoreTypes.FabricateFetch || chore.choreType == Db.Get().ChoreTypes.FetchCritical || chore.choreType == Db.Get().ChoreTypes.PowerFetch)
		{
			result = chore.GetReportName(chore.gameObject.GetProperName());
		}
		else if (chore.choreType == Db.Get().ChoreTypes.StorageFetch || chore.choreType == Db.Get().ChoreTypes.FoodFetch)
		{
			FetchChore fetchChore = chore as FetchChore;
			FetchAreaChore fetchAreaChore = chore as FetchAreaChore;
			if (fetchAreaChore != null)
			{
				GameObject getFetchTarget = fetchAreaChore.GetFetchTarget;
				KMonoBehaviour kmonoBehaviour = choreData as KMonoBehaviour;
				if (getFetchTarget != null)
				{
					result = chore.GetReportName(getFetchTarget.GetProperName());
				}
				else if (kmonoBehaviour != null)
				{
					result = chore.GetReportName(kmonoBehaviour.GetProperName());
				}
				else
				{
					result = chore.GetReportName(null);
				}
			}
			else if (fetchChore != null)
			{
				Pickupable fetchTarget = fetchChore.fetchTarget;
				KMonoBehaviour kmonoBehaviour2 = choreData as KMonoBehaviour;
				if (fetchTarget != null)
				{
					result = chore.GetReportName(fetchTarget.GetProperName());
				}
				else if (kmonoBehaviour2 != null)
				{
					result = chore.GetReportName(kmonoBehaviour2.GetProperName());
				}
				else
				{
					result = chore.GetReportName(null);
				}
			}
		}
		else
		{
			result = chore.GetReportName(null);
		}
		return result;
	}

	// Token: 0x06003881 RID: 14465 RVA: 0x0013A4CC File Offset: 0x001386CC
	public static string ChoreGroupsForChoreType(ChoreType choreType)
	{
		if (choreType.groups == null || choreType.groups.Length == 0)
		{
			return null;
		}
		string text = "";
		for (int i = 0; i < choreType.groups.Length; i++)
		{
			if (i != 0)
			{
				text += UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CHORE_GROUP_SEPARATOR;
			}
			text += choreType.groups[i].Name;
		}
		return text;
	}

	// Token: 0x06003882 RID: 14466 RVA: 0x0013A52E File Offset: 0x0013872E
	public static bool IsCapturingTimeLapse()
	{
		return Game.Instance != null && Game.Instance.timelapser != null && Game.Instance.timelapser.CapturingTimelapseScreenshot;
	}

	// Token: 0x06003883 RID: 14467 RVA: 0x0013A560 File Offset: 0x00138760
	public static ExposureType GetExposureTypeForDisease(Disease disease)
	{
		for (int i = 0; i < GERM_EXPOSURE.TYPES.Length; i++)
		{
			if (disease.id == GERM_EXPOSURE.TYPES[i].germ_id)
			{
				return GERM_EXPOSURE.TYPES[i];
			}
		}
		return null;
	}

	// Token: 0x06003884 RID: 14468 RVA: 0x0013A5A8 File Offset: 0x001387A8
	public static Sickness GetSicknessForDisease(Disease disease)
	{
		int i = 0;
		while (i < GERM_EXPOSURE.TYPES.Length)
		{
			if (disease.id == GERM_EXPOSURE.TYPES[i].germ_id)
			{
				if (GERM_EXPOSURE.TYPES[i].sickness_id == null)
				{
					return null;
				}
				return Db.Get().Sicknesses.Get(GERM_EXPOSURE.TYPES[i].sickness_id);
			}
			else
			{
				i++;
			}
		}
		return null;
	}

	// Token: 0x06003885 RID: 14469 RVA: 0x0013A612 File Offset: 0x00138812
	public static void SubscribeToTags<T>(T target, EventSystem.IntraObjectHandler<T> handler, bool triggerImmediately) where T : KMonoBehaviour
	{
		if (triggerImmediately)
		{
			handler.Trigger(target.gameObject, new TagChangedEventData(Tag.Invalid, false));
		}
		target.Subscribe<T>(-1582839653, handler);
	}

	// Token: 0x06003886 RID: 14470 RVA: 0x0013A64A File Offset: 0x0013884A
	public static void UnsubscribeToTags<T>(T target, EventSystem.IntraObjectHandler<T> handler) where T : KMonoBehaviour
	{
		target.Unsubscribe<T>(-1582839653, handler, false);
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x0013A65E File Offset: 0x0013885E
	public static EventSystem.IntraObjectHandler<T> CreateHasTagHandler<T>(Tag tag, Action<T, object> callback) where T : KMonoBehaviour
	{
		return new EventSystem.IntraObjectHandler<T>(delegate(T component, object data)
		{
			TagChangedEventData tagChangedEventData = (TagChangedEventData)data;
			if (tagChangedEventData.tag == Tag.Invalid)
			{
				KPrefabID component2 = component.GetComponent<KPrefabID>();
				tagChangedEventData = new TagChangedEventData(tag, component2.HasTag(tag));
			}
			if (tagChangedEventData.tag == tag && tagChangedEventData.added)
			{
				callback(component, data);
			}
		});
	}

	// Token: 0x0400257E RID: 9598
	public static GameUtil.TemperatureUnit temperatureUnit;

	// Token: 0x0400257F RID: 9599
	public static GameUtil.MassUnit massUnit;

	// Token: 0x04002580 RID: 9600
	private static string[] adjectives;

	// Token: 0x04002581 RID: 9601
	[ThreadStatic]
	public static Queue<GameUtil.FloodFillInfo> FloodFillNext = new Queue<GameUtil.FloodFillInfo>();

	// Token: 0x04002582 RID: 9602
	[ThreadStatic]
	public static HashSet<int> FloodFillVisited = new HashSet<int>();

	// Token: 0x04002583 RID: 9603
	public static TagSet foodTags = new TagSet(new string[]
	{
		"BasicPlantFood",
		"MushBar",
		"ColdWheatSeed",
		"ColdWheatSeed",
		"SpiceNut",
		"PrickleFruit",
		"Meat",
		"Mushroom",
		"ColdWheat",
		GameTags.Compostable.Name
	});

	// Token: 0x04002584 RID: 9604
	public static TagSet solidTags = new TagSet(new string[]
	{
		"Filter",
		"Coal",
		"BasicFabric",
		"SwampLilyFlower",
		"RefinedMetal"
	});

	// Token: 0x02001569 RID: 5481
	public enum UnitClass
	{
		// Token: 0x0400685C RID: 26716
		SimpleFloat,
		// Token: 0x0400685D RID: 26717
		SimpleInteger,
		// Token: 0x0400685E RID: 26718
		Temperature,
		// Token: 0x0400685F RID: 26719
		Mass,
		// Token: 0x04006860 RID: 26720
		Calories,
		// Token: 0x04006861 RID: 26721
		Percent,
		// Token: 0x04006862 RID: 26722
		Distance,
		// Token: 0x04006863 RID: 26723
		Disease,
		// Token: 0x04006864 RID: 26724
		Radiation,
		// Token: 0x04006865 RID: 26725
		Energy,
		// Token: 0x04006866 RID: 26726
		Power,
		// Token: 0x04006867 RID: 26727
		Lux,
		// Token: 0x04006868 RID: 26728
		Time,
		// Token: 0x04006869 RID: 26729
		Seconds,
		// Token: 0x0400686A RID: 26730
		Cycles
	}

	// Token: 0x0200156A RID: 5482
	public enum TemperatureUnit
	{
		// Token: 0x0400686C RID: 26732
		Celsius,
		// Token: 0x0400686D RID: 26733
		Fahrenheit,
		// Token: 0x0400686E RID: 26734
		Kelvin
	}

	// Token: 0x0200156B RID: 5483
	public enum MassUnit
	{
		// Token: 0x04006870 RID: 26736
		Kilograms,
		// Token: 0x04006871 RID: 26737
		Pounds
	}

	// Token: 0x0200156C RID: 5484
	public enum MetricMassFormat
	{
		// Token: 0x04006873 RID: 26739
		UseThreshold,
		// Token: 0x04006874 RID: 26740
		Kilogram,
		// Token: 0x04006875 RID: 26741
		Gram,
		// Token: 0x04006876 RID: 26742
		Tonne
	}

	// Token: 0x0200156D RID: 5485
	public enum TemperatureInterpretation
	{
		// Token: 0x04006878 RID: 26744
		Absolute,
		// Token: 0x04006879 RID: 26745
		Relative
	}

	// Token: 0x0200156E RID: 5486
	public enum TimeSlice
	{
		// Token: 0x0400687B RID: 26747
		None,
		// Token: 0x0400687C RID: 26748
		ModifyOnly,
		// Token: 0x0400687D RID: 26749
		PerSecond,
		// Token: 0x0400687E RID: 26750
		PerCycle
	}

	// Token: 0x0200156F RID: 5487
	public enum MeasureUnit
	{
		// Token: 0x04006880 RID: 26752
		mass,
		// Token: 0x04006881 RID: 26753
		kcal,
		// Token: 0x04006882 RID: 26754
		quantity
	}

	// Token: 0x02001570 RID: 5488
	public enum IdentityDescriptorTense
	{
		// Token: 0x04006884 RID: 26756
		Normal,
		// Token: 0x04006885 RID: 26757
		Possessive,
		// Token: 0x04006886 RID: 26758
		Plural
	}

	// Token: 0x02001571 RID: 5489
	public enum WattageFormatterUnit
	{
		// Token: 0x04006888 RID: 26760
		Watts,
		// Token: 0x04006889 RID: 26761
		Kilowatts,
		// Token: 0x0400688A RID: 26762
		Automatic
	}

	// Token: 0x02001572 RID: 5490
	public enum HeatEnergyFormatterUnit
	{
		// Token: 0x0400688C RID: 26764
		DTU_S,
		// Token: 0x0400688D RID: 26765
		KDTU_S,
		// Token: 0x0400688E RID: 26766
		Automatic
	}

	// Token: 0x02001573 RID: 5491
	public struct FloodFillInfo
	{
		// Token: 0x0400688F RID: 26767
		public int cell;

		// Token: 0x04006890 RID: 26768
		public int depth;
	}

	// Token: 0x02001574 RID: 5492
	public static class Hardness
	{
		// Token: 0x04006891 RID: 26769
		public const int VERY_SOFT = 0;

		// Token: 0x04006892 RID: 26770
		public const int SOFT = 10;

		// Token: 0x04006893 RID: 26771
		public const int FIRM = 25;

		// Token: 0x04006894 RID: 26772
		public const int VERY_FIRM = 50;

		// Token: 0x04006895 RID: 26773
		public const int NEARLY_IMPENETRABLE = 150;

		// Token: 0x04006896 RID: 26774
		public const int SUPER_DUPER_HARD = 200;

		// Token: 0x04006897 RID: 26775
		public const int RADIOACTIVE_MATERIALS = 251;

		// Token: 0x04006898 RID: 26776
		public const int IMPENETRABLE = 255;

		// Token: 0x04006899 RID: 26777
		public static Color ImpenetrableColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);

		// Token: 0x0400689A RID: 26778
		public static Color nearlyImpenetrableColor = new Color(0.7411765f, 0.34901962f, 0.49803922f);

		// Token: 0x0400689B RID: 26779
		public static Color veryFirmColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x0400689C RID: 26780
		public static Color firmColor = new Color(0.5254902f, 0.41960785f, 0.64705884f);

		// Token: 0x0400689D RID: 26781
		public static Color softColor = new Color(0.42745098f, 0.48235294f, 0.75686276f);

		// Token: 0x0400689E RID: 26782
		public static Color verySoftColor = new Color(0.44313726f, 0.67058825f, 0.8117647f);
	}

	// Token: 0x02001575 RID: 5493
	public static class GermResistanceValues
	{
		// Token: 0x0400689F RID: 26783
		public const float MEDIUM = 2f;

		// Token: 0x040068A0 RID: 26784
		public const float LARGE = 5f;

		// Token: 0x040068A1 RID: 26785
		public static Color NegativeLargeColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);

		// Token: 0x040068A2 RID: 26786
		public static Color NegativeMediumColor = new Color(0.7411765f, 0.34901962f, 0.49803922f);

		// Token: 0x040068A3 RID: 26787
		public static Color NegativeSmallColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x040068A4 RID: 26788
		public static Color PositiveSmallColor = new Color(0.5254902f, 0.41960785f, 0.64705884f);

		// Token: 0x040068A5 RID: 26789
		public static Color PositiveMediumColor = new Color(0.42745098f, 0.48235294f, 0.75686276f);

		// Token: 0x040068A6 RID: 26790
		public static Color PositiveLargeColor = new Color(0.44313726f, 0.67058825f, 0.8117647f);
	}

	// Token: 0x02001576 RID: 5494
	public static class ThermalConductivityValues
	{
		// Token: 0x040068A7 RID: 26791
		public const float VERY_HIGH = 50f;

		// Token: 0x040068A8 RID: 26792
		public const float HIGH = 10f;

		// Token: 0x040068A9 RID: 26793
		public const float MEDIUM = 2f;

		// Token: 0x040068AA RID: 26794
		public const float LOW = 1f;

		// Token: 0x040068AB RID: 26795
		public static Color veryLowConductivityColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);

		// Token: 0x040068AC RID: 26796
		public static Color lowConductivityColor = new Color(0.7411765f, 0.34901962f, 0.49803922f);

		// Token: 0x040068AD RID: 26797
		public static Color mediumConductivityColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x040068AE RID: 26798
		public static Color highConductivityColor = new Color(0.5254902f, 0.41960785f, 0.64705884f);

		// Token: 0x040068AF RID: 26799
		public static Color veryHighConductivityColor = new Color(0.42745098f, 0.48235294f, 0.75686276f);
	}

	// Token: 0x02001577 RID: 5495
	public static class BreathableValues
	{
		// Token: 0x040068B0 RID: 26800
		public static Color positiveColor = new Color(0.44313726f, 0.67058825f, 0.8117647f);

		// Token: 0x040068B1 RID: 26801
		public static Color warningColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x040068B2 RID: 26802
		public static Color negativeColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);
	}

	// Token: 0x02001578 RID: 5496
	public static class WireLoadValues
	{
		// Token: 0x040068B3 RID: 26803
		public static Color warningColor = new Color(0.9843137f, 0.6901961f, 0.23137255f);

		// Token: 0x040068B4 RID: 26804
		public static Color negativeColor = new Color(1f, 0.19215687f, 0.19215687f);
	}
}
