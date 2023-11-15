﻿using System;
using System.Collections.Generic;
using ImGuiNET;
using STRINGS;
using UnityEngine;

// Token: 0x0200055C RID: 1372
public class DevToolGeyserModifiers : DevTool
{
	// Token: 0x1700018E RID: 398
	// (get) Token: 0x0600216A RID: 8554 RVA: 0x000B3937 File Offset: 0x000B1B37
	private float GraphHeight
	{
		get
		{
			return 26f;
		}
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x000B3940 File Offset: 0x000B1B40
	private void DrawGeyserVariable(string variableTitle, float currentValue, float modifier, string modifierFormating = "+0.##; -0.##; +0", string unit = "", string modifierUnit = "", float altValue = 0f, string altUnit = "")
	{
		ImGui.BulletText(variableTitle + ": " + currentValue.ToString() + unit);
		if (modifier != 0f)
		{
			ImGui.SameLine();
			ImGui.TextColored(this.MODFIED_VALUE_TEXT_COLOR, "(" + modifier.ToString(modifierFormating) + modifierUnit + ")");
		}
		if (!altUnit.IsNullOrWhiteSpace())
		{
			ImGui.SameLine();
			ImGui.TextColored(this.ALT_COLOR, "(" + altValue.ToString() + altUnit + ")");
		}
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x000B39C9 File Offset: 0x000B1BC9
	public static uint Color(byte r, byte g, byte b, byte a)
	{
		return (uint)((int)a << 24 | (int)b << 16 | (int)g << 8 | (int)r);
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x000B39DC File Offset: 0x000B1BDC
	private void DrawYearAndIterationsGraph(Geyser geyser)
	{
		Vector2 windowContentRegionMin = ImGui.GetWindowContentRegionMin();
		Vector2 windowContentRegionMax = ImGui.GetWindowContentRegionMax();
		float num = windowContentRegionMax.x - windowContentRegionMin.x;
		ImGui.Dummy(new Vector2(num, this.GraphHeight));
		if (!ImGui.IsItemVisible())
		{
			return;
		}
		Vector2 itemRectMin = ImGui.GetItemRectMin();
		Vector2 itemRectMax = ImGui.GetItemRectMax();
		windowContentRegionMin.x += ImGui.GetWindowPos().x;
		windowContentRegionMin.y += ImGui.GetWindowPos().y;
		windowContentRegionMax.x += ImGui.GetWindowPos().x;
		windowContentRegionMax.y += ImGui.GetWindowPos().y;
		Vector2 vector = windowContentRegionMin;
		Vector2 vector2 = windowContentRegionMax;
		vector.y = itemRectMin.y;
		vector2.y = itemRectMax.y;
		float iterationLength = this.selectedGeyser.configuration.GetIterationLength();
		float iterationPercent = this.selectedGeyser.configuration.GetIterationPercent();
		float yearLength = this.selectedGeyser.configuration.GetYearLength();
		float yearPercent = this.selectedGeyser.configuration.GetYearPercent();
		Vector2 vector3 = vector;
		Vector2 vector4 = vector2;
		vector4.x = vector.x + num * yearPercent;
		vector4.y = vector3.y + 10f;
		ImGui.GetForegroundDrawList().AddRectFilled(vector3, vector4, this.YEAR_ACTIVE_COLOR);
		vector3.x = vector4.x;
		vector4.x = vector2.x;
		ImGui.GetForegroundDrawList().AddRectFilled(vector3, vector4, this.YEAR_DORMANT_COLOR);
		float f = yearLength / iterationLength;
		float num2 = iterationLength / yearLength;
		vector3.y = vector4.y + 2f;
		vector4.y = vector3.y + 10f;
		float num3 = (float)Mathf.FloorToInt(geyser.GetCurrentLifeTime() / yearLength) * yearLength % iterationLength / iterationLength;
		int num4 = Mathf.CeilToInt(f) + 1;
		for (int i = 0; i < num4; i++)
		{
			float x = vector.x - num2 * num3 * num + num2 * (float)i * num;
			vector3.x = x;
			vector4.x = vector3.x + iterationPercent * num2 * num;
			Vector2 vector5 = vector3;
			Vector2 vector6 = vector4;
			vector5.x = Mathf.Clamp(vector5.x, vector.x, vector2.x);
			vector6.x = Mathf.Clamp(vector6.x, vector.x, vector2.x);
			ImGui.GetForegroundDrawList().AddRectFilled(vector5, vector6, this.ITERATION_ERUPTION_COLOR);
			vector3.x = vector4.x;
			vector4.x += (1f - iterationPercent) * num2 * num;
			vector5 = vector3;
			vector6 = vector4;
			vector5.x = Mathf.Clamp(vector5.x, vector.x, vector2.x);
			vector6.x = Mathf.Clamp(vector6.x, vector.x, vector2.x);
			ImGui.GetForegroundDrawList().AddRectFilled(vector5, vector6, this.ITERATION_QUIET_COLOR);
		}
		float num5 = this.selectedGeyser.RemainingActiveTime();
		float num6 = this.selectedGeyser.RemainingDormantTime();
		float num7 = ((num6 > 0f) ? (yearLength - num6) : (yearLength * yearPercent - num5)) / yearLength;
		vector3.x = vector.x + num7 * num - 1f;
		vector4.x = vector.x + num7 * num + 1f;
		vector3.y = vector.y - 2f;
		vector4.y += 2f;
		ImGui.GetForegroundDrawList().AddRectFilled(vector3, vector4, this.CURRENT_TIME_COLOR);
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x000B3D9C File Offset: 0x000B1F9C
	protected override void RenderTo(DevPanel panel)
	{
		this.Update();
		string fmt = (this.selectedGeyser == null) ? "No Geyser Selected" : (UI.StripLinkFormatting(this.selectedGeyser.gameObject.GetProperName()) + " -");
		uint num = 0U;
		ImGui.AlignTextToFramePadding();
		ImGui.Text(fmt);
		if (this.selectedGeyser != null)
		{
			StateMachine.BaseState currentState = this.selectedGeyser.smi.GetCurrentState();
			string fmt2 = "zZ";
			string tooltip = "Current State: " + currentState.name;
			Vector4 col = this.SUBTITLE_SLEEP_COLOR;
			if (currentState == this.selectedGeyser.smi.sm.erupt.erupting)
			{
				fmt2 = "Erupting";
				col = this.SUBTITLE_ERUPTING_COLOR;
			}
			else if (currentState == this.selectedGeyser.smi.sm.erupt.overpressure)
			{
				fmt2 = "Overpressure";
				col = this.SUBTITLE_OVERPRESSURE_COLOR;
			}
			ImGui.SameLine();
			ImGui.TextColored(col, fmt2);
			if (ImGui.IsItemHovered())
			{
				ImGui.SetTooltip(tooltip);
			}
			ImGui.Separator();
			ImGui.Spacing();
			Geyser.GeyserModification modifier = this.selectedGeyser.configuration.GetModifier();
			this.PrepareSummaryForModification(this.selectedGeyser.configuration.GetModifier());
			float currentLifeTime = this.selectedGeyser.GetCurrentLifeTime();
			float yearLength = this.selectedGeyser.configuration.GetYearLength();
			ImGui.Text("Time Settings: \t");
			ImGui.SameLine();
			bool flag = ImGui.Button("Active");
			ImGui.SameLine();
			bool flag2 = ImGui.Button("Dormant");
			ImGui.SameLine();
			bool flag3 = ImGui.Button("<");
			ImGui.SameLine();
			bool flag4 = ImGui.Button(">");
			ImGui.SameLine();
			ImGui.Text(string.Concat(new string[]
			{
				"\tLifetime: ",
				currentLifeTime.ToString("00.0"),
				" sec (",
				(currentLifeTime / yearLength).ToString("0.00"),
				" Years)\t"
			}));
			bool flag5 = false;
			if (this.selectedGeyser.timeShift != 0f)
			{
				ImGui.SameLine();
				flag5 = ImGui.Button("Restore");
				if (ImGui.IsItemHovered())
				{
					ImGui.SetTooltip("Restore lifetime to match with current game time");
				}
			}
			ImGui.SliderFloat("rateRoll", ref this.selectedGeyser.configuration.rateRoll, 0f, 1f);
			ImGui.SliderFloat("iterationLengthRoll", ref this.selectedGeyser.configuration.iterationLengthRoll, 0f, 1f);
			ImGui.SliderFloat("iterationPercentRoll", ref this.selectedGeyser.configuration.iterationPercentRoll, 0f, 1f);
			ImGui.SliderFloat("yearLengthRoll", ref this.selectedGeyser.configuration.yearLengthRoll, 0f, 1f);
			ImGui.SliderFloat("yearPercentRoll", ref this.selectedGeyser.configuration.yearPercentRoll, 0f, 1f);
			this.selectedGeyser.configuration.Init(true);
			if (flag)
			{
				this.selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.ActiveState);
			}
			if (flag2)
			{
				this.selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.DormantState);
			}
			if (flag3)
			{
				this.selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.PreviousIteration);
			}
			if (flag4)
			{
				this.selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.NextIteration);
			}
			if (flag5)
			{
				this.selectedGeyser.AlterTime(0f);
			}
			this.DrawYearAndIterationsGraph(this.selectedGeyser);
			ImGui.Indent();
			bool flag6 = true;
			float num2 = (float)(flag6 ? 100 : 1);
			string modifierUnit = flag6 ? "%%" : "";
			float convertedTemperature = GameUtil.GetConvertedTemperature(this.selectedGeyser.configuration.GetTemperature(), false);
			string temperatureUnitSuffix = GameUtil.GetTemperatureUnitSuffix();
			Element element = ElementLoader.FindElementByHash(this.selectedGeyser.configuration.GetElement());
			Element element2 = ElementLoader.FindElementByHash(this.selectedGeyser.configuration.geyserType.element);
			string text = (element2.lowTempTransitionTarget == (SimHashes)0) ? "" : (GameUtil.GetConvertedTemperature(element2.lowTemp, false).ToString() + " -> " + element2.lowTempTransitionTarget.ToString());
			string text2 = (element2.highTempTransitionTarget == (SimHashes)0) ? "" : (GameUtil.GetConvertedTemperature(element2.highTemp, false).ToString() + " -> " + element2.highTempTransitionTarget.ToString());
			ImGui.BulletText("Element:");
			ImGui.SameLine();
			if (element2 != element)
			{
				string text3 = (element.lowTempTransitionTarget == (SimHashes)0) ? "" : (GameUtil.GetConvertedTemperature(element.lowTemp, false).ToString() + " " + element.lowTempTransitionTarget.ToString());
				string text4 = (element.highTempTransitionTarget == (SimHashes)0) ? "" : (GameUtil.GetConvertedTemperature(element.highTemp, false).ToString() + " " + element.highTempTransitionTarget.ToString());
				ImGui.TextColored(this.MODFIED_VALUE_TEXT_COLOR, element.ToString());
				ImGui.SameLine();
				ImGui.TextColored(this.MODFIED_VALUE_TEXT_COLOR, "(Original element: " + element2.id.ToString() + ")");
				ImGui.SameLine();
				ImGui.Text(string.Concat(new string[]
				{
					" [original low: ",
					text,
					", ",
					text2,
					", current low: ",
					text3,
					", ",
					text4,
					"]"
				}));
			}
			else
			{
				ImGui.Text(string.Format("{0} [low: {1}, high: {2}]", element2.id, text, text2));
			}
			float altValue = Mathf.Max(0f, GameUtil.GetConvertedTemperature(element2.highTemp, false) - convertedTemperature);
			this.DrawGeyserVariable("Emit Rate", this.selectedGeyser.configuration.GetEmitRate(), 0f, "+0.##; -0.##; +0", " Kg/s", "", 0f, "");
			this.DrawGeyserVariable("Average Output", this.selectedGeyser.configuration.GetAverageEmission(), 0f, "+0.##; -0.##; +0", " Kg/s", "", 0f, "");
			this.DrawGeyserVariable("Mass per cycle", this.selectedGeyser.configuration.GetMassPerCycle(), modifier.massPerCycleModifier * num2, "+0.##; -0.##; +0", "", modifierUnit, 0f, "");
			this.DrawGeyserVariable("Temperature", convertedTemperature, modifier.temperatureModifier, "+0.##; -0.##; +0", temperatureUnitSuffix, temperatureUnitSuffix, altValue, temperatureUnitSuffix + " before state change");
			this.DrawGeyserVariable("Max Pressure", this.selectedGeyser.configuration.GetMaxPressure(), modifier.maxPressureModifier * num2, "+0.##; -0.##; +0", " Kg", modifierUnit, 0f, "");
			this.DrawGeyserVariable("Iteration duration", this.selectedGeyser.configuration.GetIterationLength(), modifier.iterationDurationModifier * num2, "+0.##; -0.##; +0", " sec", modifierUnit, 0f, "");
			this.DrawGeyserVariable("Iteration percentage", this.selectedGeyser.configuration.GetIterationPercent(), modifier.iterationPercentageModifier * num2, "+0.##; -0.##; +0", "", modifierUnit, this.selectedGeyser.configuration.GetIterationLength() * this.selectedGeyser.configuration.GetIterationPercent(), " sec");
			this.DrawGeyserVariable("Year duration", this.selectedGeyser.configuration.GetYearLength(), modifier.yearDurationModifier * num2, "+0.##; -0.##; +0", " sec", modifierUnit, this.selectedGeyser.configuration.GetYearLength() / 600f, " cycles");
			this.DrawGeyserVariable("Year percentage", this.selectedGeyser.configuration.GetYearPercent(), modifier.yearPercentageModifier * num2, "+0.##; -0.##; +0", "", modifierUnit, this.selectedGeyser.configuration.GetYearPercent() * this.selectedGeyser.configuration.GetYearLength() / 600f, " cycles");
			ImGui.Unindent();
			ImGui.Spacing();
			ImGui.Separator();
			ImGui.Spacing();
			ImGui.Text("Create Modification");
			ImGui.SameLine();
			bool flag7 = ImGui.Button("Clear");
			if (flag6)
			{
				ImGui.TextColored(this.COMMENT_COLOR, "Units specified in the inputs bellow are percentages E.g. 0.1 represents 10%%\nTemperature is measured in kelvins and percentages affect the kelvin value");
				ImGui.Spacing();
			}
			if (flag7)
			{
				this.dev_modification.Clear();
			}
			ImGui.Indent();
			ImGui.BeginGroup();
			this.dev_modification.newElement.ToString();
			float num3 = 0.05f;
			float num4 = 0.15f;
			string text5 = "%.2f";
			ImGui.InputFloat(this.modifiers_FormatedList_Titles[0], ref this.dev_modification.massPerCycleModifier, flag6 ? num3 : 1f, flag6 ? num4 : 5f, flag6 ? text5 : "%.0f");
			ImGui.InputFloat(this.modifiers_FormatedList_Titles[1], ref this.dev_modification.temperatureModifier, flag6 ? num3 : 1f, flag6 ? num4 : 5f, flag6 ? text5 : "%.0f");
			ImGui.InputFloat(this.modifiers_FormatedList_Titles[2], ref this.dev_modification.maxPressureModifier, flag6 ? num3 : 0.1f, flag6 ? num4 : 0.5f, flag6 ? text5 : "%.1f");
			ImGui.InputFloat(this.modifiers_FormatedList_Titles[3], ref this.dev_modification.iterationDurationModifier, flag6 ? num3 : 1f, flag6 ? num4 : 5f, flag6 ? text5 : "%.0f");
			ImGui.InputFloat(this.modifiers_FormatedList_Titles[4], ref this.dev_modification.iterationPercentageModifier, flag6 ? num3 : 0.01f, flag6 ? num4 : 0.1f, flag6 ? text5 : "%.2f");
			ImGui.InputFloat(this.modifiers_FormatedList_Titles[5], ref this.dev_modification.yearDurationModifier, flag6 ? num3 : 1f, flag6 ? num4 : 5f, flag6 ? text5 : "%.0f");
			ImGui.InputFloat(this.modifiers_FormatedList_Titles[6], ref this.dev_modification.yearPercentageModifier, flag6 ? num3 : 0.01f, flag6 ? num4 : 0.1f, flag6 ? text5 : "%.2f");
			ImGui.Checkbox(this.modifiers_FormatedList_Titles[7], ref this.dev_modification.modifyElement);
			string text6 = "None";
			string text7 = (this.dev_modification.modifyElement && this.dev_modification.newElement != (SimHashes)0) ? this.dev_modification.newElement.ToString() : text6;
			if (ImGui.BeginCombo(this.modifiers_FormatedList_Titles[8], text7))
			{
				for (int i = 0; i < this.AllSimHashesValues.Length; i++)
				{
					bool flag8 = this.dev_modification.newElement.ToString() == text7;
					if (ImGui.Selectable(this.AllSimHashesValues[i], flag8))
					{
						text7 = this.AllSimHashesValues[i];
						this.dev_modification.newElement = (SimHashes)Enum.Parse(typeof(SimHashes), text7);
					}
					if (flag8)
					{
						ImGui.SetItemDefaultFocus();
					}
				}
				ImGui.EndCombo();
			}
			if (ImGui.Button("Add Modification"))
			{
				string str = "DEV MODIFIER#";
				int devModifierID = this.DevModifierID;
				this.DevModifierID = devModifierID + 1;
				this.dev_modification.originID = str + devModifierID.ToString();
				this.selectedGeyser.AddModification(this.dev_modification);
			}
			ImGui.SameLine();
			if (ImGui.Button("Remove Last") && this.selectedGeyser.modifications.Count > 0)
			{
				int num5 = -1;
				for (int j = this.selectedGeyser.modifications.Count - 1; j >= 0; j--)
				{
					if (this.selectedGeyser.modifications[j].originID.Contains("DEV MODIFIER"))
					{
						num5 = j;
						break;
					}
				}
				if (num5 >= 0)
				{
					this.selectedGeyser.RemoveModification(this.selectedGeyser.modifications[num5]);
				}
			}
			ImGui.EndGroup();
			ImGui.Unindent();
			ImGui.Spacing();
			ImGui.Separator();
			ImGui.Spacing();
			while (this.modificationListUnfold.Count < this.selectedGeyser.modifications.Count)
			{
				this.modificationListUnfold.Add(false);
			}
			ImGui.Text("Modifications: " + this.selectedGeyser.modifications.Count.ToString());
			ImGui.Indent();
			for (int k = 0; k < this.selectedGeyser.modifications.Count; k++)
			{
				bool flag9 = this.selectedGeyser.modifications[k].originID.Contains("DEV MODIFIER");
				bool flag10 = false;
				bool flag11 = false;
				if (this.modificationListUnfold[k] = ImGui.CollapsingHeader(k.ToString() + ". " + this.selectedGeyser.modifications[k].originID, ImGuiTreeNodeFlags.SpanAvailWidth))
				{
					this.PrepareSummaryForModification(this.selectedGeyser.modifications[k]);
					Vector2 itemRectSize = ImGui.GetItemRectSize();
					itemRectSize.y *= (float)Mathf.Max(this.modifiers_FormatedList.Length + (flag9 ? 1 : 0) + 1, 1);
					if (ImGui.BeginChild(num += 1U, itemRectSize, false, ImGuiWindowFlags.NoBackground))
					{
						ImGui.Indent();
						for (int l = 0; l < this.modifiers_FormatedList.Length; l++)
						{
							ImGui.Text(this.modifiers_FormatedList[l]);
							if (ImGui.IsItemHovered())
							{
								this.modifierSelected = l;
								ImGui.SetTooltip(this.modifiers_FormatedList_Tooltip[this.modifierSelected]);
							}
						}
						flag11 = ImGui.Button("Copy");
						if (flag9)
						{
							flag10 = ImGui.Button("Remove");
						}
						ImGui.Unindent();
					}
					ImGui.EndChild();
				}
				if (flag11)
				{
					this.dev_modification = this.selectedGeyser.modifications[k];
				}
				if (flag10)
				{
					this.selectedGeyser.RemoveModification(this.selectedGeyser.modifications[k]);
					break;
				}
			}
			ImGui.Unindent();
		}
	}

	// Token: 0x0600216F RID: 8559 RVA: 0x000B4BE4 File Offset: 0x000B2DE4
	private void PrepareSummaryForModification(Geyser.GeyserModification modification)
	{
		float num = (float)((Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? 100 : 1);
		float num2 = (float)((Geyser.temperatureModificationMethod == Geyser.ModificationMethod.Percentages) ? 100 : 1);
		float num3 = (float)((Geyser.maxPressureModificationMethod == Geyser.ModificationMethod.Percentages) ? 100 : 1);
		float num4 = (float)((Geyser.IterationDurationModificationMethod == Geyser.ModificationMethod.Percentages) ? 100 : 1);
		float num5 = (float)((Geyser.IterationPercentageModificationMethod == Geyser.ModificationMethod.Percentages) ? 100 : 1);
		float num6 = (float)((Geyser.yearDurationModificationMethod == Geyser.ModificationMethod.Percentages) ? 100 : 1);
		float num7 = (float)((Geyser.yearPercentageModificationMethod == Geyser.ModificationMethod.Percentages) ? 100 : 1);
		string str = (num == 100f) ? "%%" : "";
		string str2 = (num2 == 100f) ? "%%" : "";
		string text = (num3 == 100f) ? "%%" : "";
		string text2 = (num4 == 100f) ? "%%" : "";
		string str3 = (num5 == 100f) ? "%%" : "";
		string text3 = (num6 == 100f) ? "%%" : "";
		string str4 = (num7 == 100f) ? "%%" : "";
		this.modifiers_FormatedList[0] = this.modifiers_FormatedList_Titles[0] + ": " + (modification.massPerCycleModifier * num).ToString("+0.##; -0.##; +0") + str;
		this.modifiers_FormatedList[1] = this.modifiers_FormatedList_Titles[1] + ": " + (modification.temperatureModifier * num2).ToString("+0.##; -0.##; +0") + str2;
		this.modifiers_FormatedList[2] = this.modifiers_FormatedList_Titles[2] + ": " + (modification.maxPressureModifier * num3).ToString("+0.##; -0.##; +0") + ((num3 == 100f) ? text : "Kg");
		this.modifiers_FormatedList[3] = this.modifiers_FormatedList_Titles[3] + ": " + (modification.iterationDurationModifier * num4).ToString("+0.##; -0.##; +0") + ((num4 == 100f) ? text2 : "s");
		this.modifiers_FormatedList[4] = this.modifiers_FormatedList_Titles[4] + ": " + (modification.iterationPercentageModifier * num5).ToString("+0.##; -0.##; +0") + str3;
		this.modifiers_FormatedList[5] = this.modifiers_FormatedList_Titles[5] + ": " + (modification.yearDurationModifier * num6).ToString("+0.##; -0.##; +0") + ((num6 == 100f) ? text3 : "s");
		this.modifiers_FormatedList[6] = this.modifiers_FormatedList_Titles[6] + ": " + (modification.yearPercentageModifier * num7).ToString("+0.##; -0.##; +0") + str4;
		this.modifiers_FormatedList[7] = this.modifiers_FormatedList_Titles[7] + ": " + modification.modifyElement.ToString();
		this.modifiers_FormatedList[8] = this.modifiers_FormatedList_Titles[8] + ": " + (modification.IsNewElementInUse() ? modification.newElement.ToString() : "None");
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x000B4EEC File Offset: 0x000B30EC
	private void Update()
	{
		this.Setup();
		SelectTool instance = SelectTool.Instance;
		GameObject gameObject;
		if (instance == null)
		{
			gameObject = null;
		}
		else
		{
			KSelectable selected = instance.selected;
			gameObject = ((selected != null) ? selected.gameObject : null);
		}
		GameObject gameObject2 = gameObject;
		if (this.lastSelectedGameObject != gameObject2 && gameObject2 != null)
		{
			Geyser component = gameObject2.GetComponent<Geyser>();
			this.selectedGeyser = ((component == null) ? this.selectedGeyser : component);
		}
		this.lastSelectedGameObject = gameObject2;
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x000B4F5C File Offset: 0x000B315C
	private void Setup()
	{
		if (this.AllSimHashesValues == null)
		{
			this.AllSimHashesValues = Enum.GetNames(typeof(SimHashes));
		}
		if (this.modifierFormatting_ValuePadding < 0)
		{
			for (int i = 0; i < this.modifiers_FormatedList_Titles.Length; i++)
			{
				this.modifierFormatting_ValuePadding = Mathf.Max(this.modifierFormatting_ValuePadding, this.modifiers_FormatedList_Titles[i].Length);
			}
		}
		if (string.IsNullOrEmpty(this.modifiers_FormatedList_Tooltip[0]))
		{
			this.modifiers_FormatedList_Tooltip[0] = "Mass per cycle is not mass per iteration, mass per iteration gets calculated out of this";
			this.modifiers_FormatedList_Tooltip[1] = "Temperature modifier of the emitted element, does not refer to the temperature of the geyser itself";
			this.modifiers_FormatedList_Tooltip[2] = "Refering to the max pressure allowed in the environment surrounding the geyser before it stops emitting";
			this.modifiers_FormatedList_Tooltip[3] = "An iteration is a chunk of time that has 2 sections, one section is the erupting time while the other is the non erupting time";
			this.modifiers_FormatedList_Tooltip[4] = "Represents what percentage out of the iteration duration will be used for 'Erupting' period and the remaining will be the 'Quiet' period";
			this.modifiers_FormatedList_Tooltip[5] = "A year is a chunk of time that has 2 sections, one section is the Active section while the other is the Dormant section. While active, there could be many Iterations. While Dormant, there is no activity at all.";
			this.modifiers_FormatedList_Tooltip[6] = "Represents what percentage out of the year duration will be used for 'Active' period and the remaining will be the 'Dormant' period";
			this.modifiers_FormatedList_Tooltip[7] = "Whether to use or not to use the specified element";
			this.modifiers_FormatedList_Tooltip[8] = "Extra element to emit";
		}
	}

	// Token: 0x040012D1 RID: 4817
	private const string DEV_MODIFIER_ID = "DEV MODIFIER";

	// Token: 0x040012D2 RID: 4818
	private const string NO_SELECTED_STR = "No Geyser Selected";

	// Token: 0x040012D3 RID: 4819
	private int DevModifierID;

	// Token: 0x040012D4 RID: 4820
	private const float ITERATION_BAR_HEIGHT = 10f;

	// Token: 0x040012D5 RID: 4821
	private const float YEAR_BAR_HEIGHT = 10f;

	// Token: 0x040012D6 RID: 4822
	private const float BAR_SPACING = 2f;

	// Token: 0x040012D7 RID: 4823
	private const float CURRENT_TIME_PADDING = 2f;

	// Token: 0x040012D8 RID: 4824
	private const float CURRENT_TIME_LINE_WIDTH = 2f;

	// Token: 0x040012D9 RID: 4825
	private uint YEAR_ACTIVE_COLOR = DevToolGeyserModifiers.Color(220, 15, 65, 175);

	// Token: 0x040012DA RID: 4826
	private uint YEAR_DORMANT_COLOR = DevToolGeyserModifiers.Color(byte.MaxValue, 0, 65, 60);

	// Token: 0x040012DB RID: 4827
	private uint ITERATION_ERUPTION_COLOR = DevToolGeyserModifiers.Color(60, 80, byte.MaxValue, 200);

	// Token: 0x040012DC RID: 4828
	private uint ITERATION_QUIET_COLOR = DevToolGeyserModifiers.Color(60, 80, byte.MaxValue, 80);

	// Token: 0x040012DD RID: 4829
	private uint CURRENT_TIME_COLOR = DevToolGeyserModifiers.Color(byte.MaxValue, 0, 0, byte.MaxValue);

	// Token: 0x040012DE RID: 4830
	private Vector4 MODFIED_VALUE_TEXT_COLOR = new Vector4(0.8f, 0.7f, 0.1f, 1f);

	// Token: 0x040012DF RID: 4831
	private Vector4 COMMENT_COLOR = new Vector4(0.1f, 0.5f, 0.1f, 1f);

	// Token: 0x040012E0 RID: 4832
	private Vector4 SUBTITLE_SLEEP_COLOR = new Vector4(0.15f, 0.35f, 0.7f, 1f);

	// Token: 0x040012E1 RID: 4833
	private Vector4 SUBTITLE_OVERPRESSURE_COLOR = new Vector4(0.7f, 0f, 0f, 1f);

	// Token: 0x040012E2 RID: 4834
	private Vector4 SUBTITLE_ERUPTING_COLOR = new Vector4(1f, 0.7f, 0f, 1f);

	// Token: 0x040012E3 RID: 4835
	private Vector4 ALT_COLOR = new Vector4(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x040012E4 RID: 4836
	private List<bool> modificationListUnfold = new List<bool>();

	// Token: 0x040012E5 RID: 4837
	private GameObject lastSelectedGameObject;

	// Token: 0x040012E6 RID: 4838
	private Geyser selectedGeyser;

	// Token: 0x040012E7 RID: 4839
	private Geyser.GeyserModification dev_modification;

	// Token: 0x040012E8 RID: 4840
	private string[] modifiers_FormatedList_Titles = new string[]
	{
		"Mass per cycle",
		"Temperature",
		"Max Pressure",
		"Iteration duration",
		"Iteration percentage",
		"Year duration",
		"Year percentage",
		"Using secondary element",
		"Secondary element"
	};

	// Token: 0x040012E9 RID: 4841
	private string[] modifiers_FormatedList = new string[9];

	// Token: 0x040012EA RID: 4842
	private string[] modifiers_FormatedList_Tooltip = new string[9];

	// Token: 0x040012EB RID: 4843
	private string[] AllSimHashesValues;

	// Token: 0x040012EC RID: 4844
	private int modifierSelected = -1;

	// Token: 0x040012ED RID: 4845
	private int modifierFormatting_ValuePadding = -1;
}
