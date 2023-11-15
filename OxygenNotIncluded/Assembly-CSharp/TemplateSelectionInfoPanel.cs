using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C77 RID: 3191
[AddComponentMenu("KMonoBehaviour/scripts/TemplateSelectionInfoPanel")]
public class TemplateSelectionInfoPanel : KMonoBehaviour, IRender1000ms
{
	// Token: 0x060065C4 RID: 26052 RVA: 0x0025F1F4 File Offset: 0x0025D3F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.details.Length; i++)
		{
			Util.KInstantiateUI(this.prefab_detail_label, this.current_detail_container, true);
		}
		this.RefreshDetails();
		this.save_button.onClick += this.SaveCurrentDetails;
	}

	// Token: 0x060065C5 RID: 26053 RVA: 0x0025F24C File Offset: 0x0025D44C
	public void SaveCurrentDetails()
	{
		string text = "";
		for (int i = 0; i < this.details.Length; i++)
		{
			text = text + this.details[i](DebugBaseTemplateButton.Instance.SelectedCells) + "\n";
		}
		text += "\n\n";
		text += this.saved_detail_label.text;
		this.saved_detail_label.text = text;
	}

	// Token: 0x060065C6 RID: 26054 RVA: 0x0025F2BF File Offset: 0x0025D4BF
	public void Render1000ms(float dt)
	{
		this.RefreshDetails();
	}

	// Token: 0x060065C7 RID: 26055 RVA: 0x0025F2C8 File Offset: 0x0025D4C8
	public void RefreshDetails()
	{
		for (int i = 0; i < this.details.Length; i++)
		{
			this.current_detail_container.transform.GetChild(i).GetComponent<LocText>().text = this.details[i](DebugBaseTemplateButton.Instance.SelectedCells);
		}
	}

	// Token: 0x060065C8 RID: 26056 RVA: 0x0025F31C File Offset: 0x0025D51C
	private static string TotalMass(List<int> cells)
	{
		float num = 0f;
		foreach (int i in cells)
		{
			num += Grid.Mass[i];
		}
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_MASS, GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x060065C9 RID: 26057 RVA: 0x0025F394 File Offset: 0x0025D594
	private static string AverageMass(List<int> cells)
	{
		float num = 0f;
		foreach (int i in cells)
		{
			num += Grid.Mass[i];
		}
		num /= (float)cells.Count;
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_MASS, GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x060065CA RID: 26058 RVA: 0x0025F418 File Offset: 0x0025D618
	private static string AverageTemperature(List<int> cells)
	{
		float num = 0f;
		foreach (int i in cells)
		{
			num += Grid.Temperature[i];
		}
		num /= (float)cells.Count;
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_TEMPERATURE, GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x060065CB RID: 26059 RVA: 0x0025F498 File Offset: 0x0025D698
	private static string TotalJoules(List<int> cells)
	{
		List<GameObject> list = new List<GameObject>();
		float num = 0f;
		foreach (int num2 in cells)
		{
			num += Grid.Element[num2].specificHeatCapacity * Grid.Temperature[num2] * (Grid.Mass[num2] * 1000f);
			num += TemplateSelectionInfoPanel.GetCellEntityEnergy(num2, ref list);
		}
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_JOULES, GameUtil.GetFormattedJoules(num, "F5", GameUtil.TimeSlice.None));
	}

	// Token: 0x060065CC RID: 26060 RVA: 0x0025F540 File Offset: 0x0025D740
	private static float GetCellEntityEnergy(int cell, ref List<GameObject> ignoreObjects)
	{
		float num = 0f;
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (!(gameObject == null) && !ignoreObjects.Contains(gameObject))
			{
				ignoreObjects.Add(gameObject);
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Element != null)
				{
					float num2 = component.Mass;
					Building component2 = gameObject.GetComponent<Building>();
					if (component2 != null)
					{
						num2 = component2.Def.MassForTemperatureModification;
					}
					float num3 = num2 * 1000f * component.Element.specificHeatCapacity * component.Temperature;
					num += num3;
					Storage[] components = gameObject.GetComponents<Storage>();
					if (components != null)
					{
						float num4 = 0f;
						Storage[] array = components;
						for (int j = 0; j < array.Length; j++)
						{
							foreach (GameObject gameObject2 in array[j].items)
							{
								PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
								if (!(component3 == null))
								{
									num4 += component3.Mass * 1000f * component3.Element.specificHeatCapacity * component3.Temperature;
								}
							}
						}
						num += num4;
					}
					Conduit component4 = gameObject.GetComponent<Conduit>();
					if (component4 != null)
					{
						ConduitFlow.ConduitContents contents = component4.GetFlowManager().GetContents(cell);
						if (contents.mass > 0f)
						{
							Element element = ElementLoader.FindElementByHash(contents.element);
							float num5 = contents.mass * 1000f * element.specificHeatCapacity * contents.temperature;
							num += num5;
						}
					}
					if (gameObject.GetComponent<SolidConduit>() != null)
					{
						SolidConduitFlow solidConduitFlow = Game.Instance.solidConduitFlow;
						SolidConduitFlow.ConduitContents contents2 = solidConduitFlow.GetContents(cell);
						if (contents2.pickupableHandle.IsValid())
						{
							Pickupable pickupable = solidConduitFlow.GetPickupable(contents2.pickupableHandle);
							if (pickupable)
							{
								PrimaryElement component5 = pickupable.GetComponent<PrimaryElement>();
								if (component5.Mass > 0f)
								{
									float num6 = component5.Mass * 1000f * component5.Element.specificHeatCapacity * component5.Temperature;
									num += num6;
								}
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x060065CD RID: 26061 RVA: 0x0025F798 File Offset: 0x0025D998
	private static string JoulesPerKilogram(List<int> cells)
	{
		float num = 0f;
		float num2 = 0f;
		foreach (int num3 in cells)
		{
			num += Grid.Element[num3].specificHeatCapacity * Grid.Temperature[num3] * (Grid.Mass[num3] * 1000f);
			num2 += Grid.Mass[num3];
		}
		num /= num2;
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.JOULES_PER_KILOGRAM, GameUtil.GetFormattedJoules(num, "F1", GameUtil.TimeSlice.None));
	}

	// Token: 0x060065CE RID: 26062 RVA: 0x0025F848 File Offset: 0x0025DA48
	private static string TotalRadiation(List<int> cells)
	{
		float num = 0f;
		foreach (int i in cells)
		{
			num += Grid.Radiation[i];
		}
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_RADS, GameUtil.GetFormattedRads(num, GameUtil.TimeSlice.None));
	}

	// Token: 0x060065CF RID: 26063 RVA: 0x0025F8BC File Offset: 0x0025DABC
	private static string AverageRadiation(List<int> cells)
	{
		float num = 0f;
		foreach (int i in cells)
		{
			num += Grid.Radiation[i];
		}
		num /= (float)cells.Count;
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_RADS, GameUtil.GetFormattedRads(num, GameUtil.TimeSlice.None));
	}

	// Token: 0x060065D0 RID: 26064 RVA: 0x0025F938 File Offset: 0x0025DB38
	private static string MassPerElement(List<int> cells)
	{
		TemplateSelectionInfoPanel.mass_per_element.Clear();
		foreach (int num in cells)
		{
			bool flag = false;
			for (int i = 0; i < TemplateSelectionInfoPanel.mass_per_element.Count; i++)
			{
				if (TemplateSelectionInfoPanel.mass_per_element[i].first == Grid.Element[num])
				{
					TemplateSelectionInfoPanel.mass_per_element[i].second += Grid.Mass[num];
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TemplateSelectionInfoPanel.mass_per_element.Add(new global::Tuple<Element, float>(Grid.Element[num], Grid.Mass[num]));
			}
		}
		TemplateSelectionInfoPanel.mass_per_element.Sort(delegate(global::Tuple<Element, float> a, global::Tuple<Element, float> b)
		{
			if (a.second > b.second)
			{
				return -1;
			}
			if (b.second > a.second)
			{
				return 1;
			}
			return 0;
		});
		string text = "";
		foreach (global::Tuple<Element, float> tuple in TemplateSelectionInfoPanel.mass_per_element)
		{
			text = string.Concat(new string[]
			{
				text,
				tuple.first.name,
				": ",
				GameUtil.GetFormattedMass(tuple.second, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
				"\n"
			});
		}
		return text;
	}

	// Token: 0x0400460D RID: 17933
	[SerializeField]
	private GameObject prefab_detail_label;

	// Token: 0x0400460E RID: 17934
	[SerializeField]
	private GameObject current_detail_container;

	// Token: 0x0400460F RID: 17935
	[SerializeField]
	private LocText saved_detail_label;

	// Token: 0x04004610 RID: 17936
	[SerializeField]
	private KButton save_button;

	// Token: 0x04004611 RID: 17937
	private Func<List<int>, string>[] details = new Func<List<int>, string>[]
	{
		new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalMass),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageMass),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageTemperature),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalJoules),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.JoulesPerKilogram),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.MassPerElement),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalRadiation),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageRadiation)
	};

	// Token: 0x04004612 RID: 17938
	private static List<global::Tuple<Element, float>> mass_per_element = new List<global::Tuple<Element, float>>();
}
