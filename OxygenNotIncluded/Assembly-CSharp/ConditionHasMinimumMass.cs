using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009D3 RID: 2515
public class ConditionHasMinimumMass : ProcessCondition
{
	// Token: 0x06004B3E RID: 19262 RVA: 0x001A73EB File Offset: 0x001A55EB
	public ConditionHasMinimumMass(CommandModule command)
	{
		this.commandModule = command;
	}

	// Token: 0x06004B3F RID: 19263 RVA: 0x001A73FC File Offset: 0x001A55FC
	public override ProcessCondition.Status EvaluateCondition()
	{
		int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
		if (spacecraftDestination != null && SpacecraftManager.instance.GetDestinationAnalysisState(spacecraftDestination) == SpacecraftManager.DestinationAnalysisState.Complete && spacecraftDestination.AvailableMass >= ConditionHasMinimumMass.CargoCapacity(spacecraftDestination, this.commandModule))
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Warning;
	}

	// Token: 0x06004B40 RID: 19264 RVA: 0x001A7458 File Offset: 0x001A5658
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
		if (spacecraftDestination == null)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.NO_DESTINATION;
		}
		if (SpacecraftManager.instance.GetDestinationAnalysisState(spacecraftDestination) == SpacecraftManager.DestinationAnalysisState.Complete)
		{
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.MINIMUM_MASS, GameUtil.GetFormattedMass(spacecraftDestination.AvailableMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"));
		}
		return string.Format(UI.STARMAP.LAUNCHCHECKLIST.MINIMUM_MASS, UI.STARMAP.COMPOSITION_UNDISCOVERED_AMOUNT);
	}

	// Token: 0x06004B41 RID: 19265 RVA: 0x001A74E0 File Offset: 0x001A56E0
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
		bool flag = spacecraftDestination != null && SpacecraftManager.instance.GetDestinationAnalysisState(spacecraftDestination) == SpacecraftManager.DestinationAnalysisState.Complete;
		string text = "";
		if (flag)
		{
			if (spacecraftDestination.AvailableMass <= ConditionHasMinimumMass.CargoCapacity(spacecraftDestination, this.commandModule))
			{
				text = text + UI.STARMAP.LAUNCHCHECKLIST.INSUFFICENT_MASS_TOOLTIP + "\n\n";
			}
			text = text + string.Format(UI.STARMAP.LAUNCHCHECKLIST.RESOURCE_MASS_TOOLTIP, spacecraftDestination.GetDestinationType().Name, GameUtil.GetFormattedMass(spacecraftDestination.AvailableMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), GameUtil.GetFormattedMass(ConditionHasMinimumMass.CargoCapacity(spacecraftDestination, this.commandModule), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")) + "\n\n";
		}
		float num = (spacecraftDestination != null) ? spacecraftDestination.AvailableMass : 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			CargoBay component = gameObject.GetComponent<CargoBay>();
			if (component != null)
			{
				if (flag)
				{
					float availableResourcesPercentage = spacecraftDestination.GetAvailableResourcesPercentage(component.storageType);
					float num2 = Mathf.Min(component.storage.Capacity(), availableResourcesPercentage * num);
					num -= num2;
					text = string.Concat(new string[]
					{
						text,
						component.gameObject.GetProperName(),
						" ",
						string.Format(UI.STARMAP.STORAGESTATS.STORAGECAPACITY, GameUtil.GetFormattedMass(Mathf.Min(num2, component.storage.Capacity()), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), GameUtil.GetFormattedMass(component.storage.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")),
						"\n"
					});
				}
				else
				{
					text = string.Concat(new string[]
					{
						text,
						component.gameObject.GetProperName(),
						" ",
						string.Format(UI.STARMAP.STORAGESTATS.STORAGECAPACITY, GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), GameUtil.GetFormattedMass(component.storage.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")),
						"\n"
					});
				}
			}
		}
		return text;
	}

	// Token: 0x06004B42 RID: 19266 RVA: 0x001A774C File Offset: 0x001A594C
	public static float CargoCapacity(SpaceDestination destination, CommandModule module)
	{
		if (module == null)
		{
			return 0f;
		}
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(module.GetComponent<AttachableBuilding>()))
		{
			CargoBay component = gameObject.GetComponent<CargoBay>();
			if (component != null && destination.HasElementType(component.storageType))
			{
				Storage component2 = component.GetComponent<Storage>();
				num += component2.capacityKg;
			}
		}
		return num;
	}

	// Token: 0x06004B43 RID: 19267 RVA: 0x001A77E0 File Offset: 0x001A59E0
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003143 RID: 12611
	private CommandModule commandModule;
}
