using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000C8A RID: 3210
[Serializable]
public class UnitConfigurationScreen
{
	// Token: 0x0600664E RID: 26190 RVA: 0x002628B8 File Offset: 0x00260AB8
	public void Init()
	{
		this.celsiusToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.celsiusToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.CELSIUS_TOOLTIP;
		this.celsiusToggle.GetComponentInChildren<KButton>().onClick += this.OnCelsiusClicked;
		this.celsiusToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.CELSIUS;
		this.kelvinToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.kelvinToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.KELVIN_TOOLTIP;
		this.kelvinToggle.GetComponentInChildren<KButton>().onClick += this.OnKelvinClicked;
		this.kelvinToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.KELVIN;
		this.fahrenheitToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.fahrenheitToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.FAHRENHEIT_TOOLTIP;
		this.fahrenheitToggle.GetComponentInChildren<KButton>().onClick += this.OnFahrenheitClicked;
		this.fahrenheitToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.FAHRENHEIT;
		this.DisplayCurrentUnit();
	}

	// Token: 0x0600664F RID: 26191 RVA: 0x00262A04 File Offset: 0x00260C04
	private void DisplayCurrentUnit()
	{
		GameUtil.TemperatureUnit @int = (GameUtil.TemperatureUnit)KPlayerPrefs.GetInt(UnitConfigurationScreen.TemperatureUnitKey, 0);
		if (@int == GameUtil.TemperatureUnit.Celsius)
		{
			this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
			this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			return;
		}
		if (@int != GameUtil.TemperatureUnit.Kelvin)
		{
			this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
			return;
		}
		this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
		this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
		this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
	}

	// Token: 0x06006650 RID: 26192 RVA: 0x00262B4C File Offset: 0x00260D4C
	private void OnCelsiusClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Celsius;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(999382396, GameUtil.TemperatureUnit.Celsius);
		}
	}

	// Token: 0x06006651 RID: 26193 RVA: 0x00262BA4 File Offset: 0x00260DA4
	private void OnKelvinClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Kelvin;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(999382396, GameUtil.TemperatureUnit.Kelvin);
		}
	}

	// Token: 0x06006652 RID: 26194 RVA: 0x00262BFC File Offset: 0x00260DFC
	private void OnFahrenheitClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Fahrenheit;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(999382396, GameUtil.TemperatureUnit.Fahrenheit);
		}
	}

	// Token: 0x04004672 RID: 18034
	[SerializeField]
	private GameObject toggleUnitPrefab;

	// Token: 0x04004673 RID: 18035
	[SerializeField]
	private GameObject toggleGroup;

	// Token: 0x04004674 RID: 18036
	private GameObject celsiusToggle;

	// Token: 0x04004675 RID: 18037
	private GameObject kelvinToggle;

	// Token: 0x04004676 RID: 18038
	private GameObject fahrenheitToggle;

	// Token: 0x04004677 RID: 18039
	public static readonly string TemperatureUnitKey = "TemperatureUnit";

	// Token: 0x04004678 RID: 18040
	public static readonly string MassUnitKey = "MassUnit";
}
