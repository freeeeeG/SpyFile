﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A7D RID: 2685
public class SelectToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x0600518D RID: 20877 RVA: 0x001CFB2C File Offset: 0x001CDD2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.overlayFilterMap.Add(OverlayModes.Oxygen.ID, delegate
		{
			int num = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
			return Grid.Element[num].IsGas;
		});
		this.overlayFilterMap.Add(OverlayModes.GasConduits.ID, delegate
		{
			int num = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
			return Grid.Element[num].IsGas;
		});
		this.overlayFilterMap.Add(OverlayModes.Radiation.ID, delegate
		{
			int i = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
			return Grid.Radiation[i] > 0f;
		});
		this.overlayFilterMap.Add(OverlayModes.LiquidConduits.ID, delegate
		{
			int num = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
			return Grid.Element[num].IsLiquid;
		});
		this.overlayFilterMap.Add(OverlayModes.Decor.ID, () => false);
		this.overlayFilterMap.Add(OverlayModes.Rooms.ID, () => false);
		this.overlayFilterMap.Add(OverlayModes.Logic.ID, () => false);
		this.overlayFilterMap.Add(OverlayModes.TileMode.ID, delegate
		{
			int num = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
			Element element = Grid.Element[num];
			foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
			{
				if (element.HasTag(search_tag))
				{
					return true;
				}
			}
			return false;
		});
	}

	// Token: 0x0600518E RID: 20878 RVA: 0x001CFCB8 File Offset: 0x001CDEB8
	public override void ConfigureHoverScreen()
	{
		base.ConfigureHoverScreen();
		HoverTextScreen instance = HoverTextScreen.Instance;
		this.iconWarning = instance.GetSprite("iconWarning");
		this.iconDash = instance.GetSprite("dash");
		this.iconHighlighted = instance.GetSprite("dash_arrow");
		this.iconActiveAutomationPort = instance.GetSprite("current_automation_state_arrow");
		this.maskOverlay = LayerMask.GetMask(new string[]
		{
			"MaskedOverlay",
			"MaskedOverlayBG"
		});
	}

	// Token: 0x0600518F RID: 20879 RVA: 0x001CFD36 File Offset: 0x001CDF36
	private bool IsStatusItemWarning(StatusItemGroup.Entry item)
	{
		return item.item.notificationType == NotificationType.Bad || item.item.notificationType == NotificationType.BadMinor || item.item.notificationType == NotificationType.DuplicantThreatening;
	}

	// Token: 0x06005190 RID: 20880 RVA: 0x001CFD68 File Offset: 0x001CDF68
	public override void UpdateHoverElements(List<KSelectable> hoverObjects)
	{
		if (this.iconWarning == null)
		{
			this.ConfigureHoverScreen();
		}
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (OverlayScreen.Instance == null || !Grid.IsValidCell(num))
		{
			return;
		}
		HoverTextDrawer hoverTextDrawer = HoverTextScreen.Instance.BeginDrawing();
		this.overlayValidHoverObjects.Clear();
		foreach (KSelectable kselectable in hoverObjects)
		{
			if (this.ShouldShowSelectableInCurrentOverlay(kselectable))
			{
				this.overlayValidHoverObjects.Add(kselectable);
			}
		}
		this.currentSelectedSelectableIndex = -1;
		if (SelectToolHoverTextCard.highlightedObjects.Count > 0)
		{
			SelectToolHoverTextCard.highlightedObjects.Clear();
		}
		HashedString mode = SimDebugView.Instance.GetMode();
		bool flag = mode == OverlayModes.Disease.ID;
		bool flag2 = true;
		if (Grid.DupePassable[num] && Grid.Solid[num])
		{
			flag2 = false;
		}
		bool flag3 = Grid.IsVisible(num);
		if ((int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			flag3 = false;
		}
		if (!flag3)
		{
			flag2 = false;
		}
		foreach (KeyValuePair<HashedString, Func<bool>> keyValuePair in this.overlayFilterMap)
		{
			if (OverlayScreen.Instance.GetMode() == keyValuePair.Key)
			{
				if (!keyValuePair.Value())
				{
					flag2 = false;
					break;
				}
				break;
			}
		}
		string text = "";
		if (mode == OverlayModes.Temperature.ID && Game.Instance.temperatureOverlayMode == Game.TemperatureOverlayModes.HeatFlow)
		{
			if (!Grid.Solid[num] && flag3)
			{
				float thermalComfort = GameUtil.GetThermalComfort(num, 0f);
				float thermalComfort2 = GameUtil.GetThermalComfort(num, -0.08368001f);
				float num2 = 0f;
				if (thermalComfort2 * 0.001f > -0.27893335f - num2 && thermalComfort2 * 0.001f < 0.27893335f + num2)
				{
					text = UI.OVERLAYS.HEATFLOW.NEUTRAL;
				}
				else if (thermalComfort2 <= ExternalTemperatureMonitor.GetExternalColdThreshold(null))
				{
					text = UI.OVERLAYS.HEATFLOW.COOLING;
				}
				else if (thermalComfort2 >= ExternalTemperatureMonitor.GetExternalWarmThreshold(null))
				{
					text = UI.OVERLAYS.HEATFLOW.HEATING;
				}
				float dtu_s = 1f * thermalComfort;
				text = text + " (" + GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic) + ")";
				hoverTextDrawer.BeginShadowBar(false);
				hoverTextDrawer.DrawText(UI.OVERLAYS.HEATFLOW.HOVERTITLE, this.Styles_Title.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawText(text, this.Styles_BodyText.Standard);
				hoverTextDrawer.EndShadowBar();
			}
		}
		else if (mode == OverlayModes.Decor.ID)
		{
			List<DecorProvider> list = new List<DecorProvider>();
			GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.decorProviderLayer, list);
			float decorAtCell = GameUtil.GetDecorAtCell(num);
			hoverTextDrawer.BeginShadowBar(false);
			hoverTextDrawer.DrawText(UI.OVERLAYS.DECOR.HOVERTITLE, this.Styles_Title.Standard);
			hoverTextDrawer.NewLine(26);
			hoverTextDrawer.DrawText(UI.OVERLAYS.DECOR.TOTAL + GameUtil.GetFormattedDecor(decorAtCell, true), this.Styles_BodyText.Standard);
			if (!Grid.Solid[num] && flag3)
			{
				List<EffectorEntry> list2 = new List<EffectorEntry>();
				List<EffectorEntry> list3 = new List<EffectorEntry>();
				foreach (DecorProvider decorProvider in list)
				{
					float decorForCell = decorProvider.GetDecorForCell(num);
					if (decorForCell != 0f)
					{
						string text2 = decorProvider.GetName();
						KMonoBehaviour component = decorProvider.GetComponent<KMonoBehaviour>();
						if (component != null && component.gameObject != null)
						{
							SelectToolHoverTextCard.highlightedObjects.Add(component.gameObject);
							if (component.GetComponent<MonumentPart>() != null && component.GetComponent<MonumentPart>().IsMonumentCompleted())
							{
								text2 = MISC.MONUMENT_COMPLETE.NAME;
								foreach (GameObject item in AttachableBuilding.GetAttachedNetwork(component.GetComponent<AttachableBuilding>()))
								{
									SelectToolHoverTextCard.highlightedObjects.Add(item);
								}
							}
						}
						bool flag4 = false;
						if (decorForCell > 0f)
						{
							for (int i = 0; i < list2.Count; i++)
							{
								if (list2[i].name == text2)
								{
									EffectorEntry value = list2[i];
									value.count++;
									value.value += decorForCell;
									list2[i] = value;
									flag4 = true;
									break;
								}
							}
							if (!flag4)
							{
								list2.Add(new EffectorEntry(text2, decorForCell));
							}
						}
						else
						{
							for (int j = 0; j < list3.Count; j++)
							{
								if (list3[j].name == text2)
								{
									EffectorEntry value2 = list3[j];
									value2.count++;
									value2.value += decorForCell;
									list3[j] = value2;
									flag4 = true;
									break;
								}
							}
							if (!flag4)
							{
								list3.Add(new EffectorEntry(text2, decorForCell));
							}
						}
					}
				}
				int lightDecorBonus = DecorProvider.GetLightDecorBonus(num);
				if (lightDecorBonus > 0)
				{
					list2.Add(new EffectorEntry(UI.OVERLAYS.DECOR.LIGHTING, (float)lightDecorBonus));
				}
				list2.Sort((EffectorEntry x, EffectorEntry y) => y.value.CompareTo(x.value));
				if (list2.Count > 0)
				{
					hoverTextDrawer.NewLine(26);
					hoverTextDrawer.DrawText(UI.OVERLAYS.DECOR.HEADER_POSITIVE, this.Styles_BodyText.Standard);
				}
				foreach (EffectorEntry effectorEntry in list2)
				{
					hoverTextDrawer.NewLine(18);
					hoverTextDrawer.DrawIcon(this.iconDash, 18);
					hoverTextDrawer.DrawText(effectorEntry.ToString(), this.Styles_BodyText.Standard);
				}
				list3.Sort((EffectorEntry x, EffectorEntry y) => Mathf.Abs(y.value).CompareTo(Mathf.Abs(x.value)));
				if (list3.Count > 0)
				{
					hoverTextDrawer.NewLine(26);
					hoverTextDrawer.DrawText(UI.OVERLAYS.DECOR.HEADER_NEGATIVE, this.Styles_BodyText.Standard);
				}
				foreach (EffectorEntry effectorEntry2 in list3)
				{
					hoverTextDrawer.NewLine(18);
					hoverTextDrawer.DrawIcon(this.iconDash, 18);
					hoverTextDrawer.DrawText(effectorEntry2.ToString(), this.Styles_BodyText.Standard);
				}
			}
			hoverTextDrawer.EndShadowBar();
		}
		else if (mode == OverlayModes.Rooms.ID)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(num);
			if (cavityForCell != null)
			{
				Room room = cavityForCell.room;
				RoomType roomType = null;
				string text3;
				if (room != null)
				{
					roomType = room.roomType;
					text3 = roomType.Name;
				}
				else
				{
					text3 = UI.OVERLAYS.ROOMS.NOROOM.HEADER;
				}
				hoverTextDrawer.BeginShadowBar(false);
				hoverTextDrawer.DrawText(text3, this.Styles_Title.Standard);
				if (room != null)
				{
					string text4 = RoomDetails.EFFECT.resolve_string_function(room);
					string text5 = RoomDetails.ASSIGNED_TO.resolve_string_function(room);
					string text6 = RoomConstraints.RoomCriteriaString(room);
					string text7 = RoomDetails.EFFECTS.resolve_string_function(room);
					if (text4 != "")
					{
						hoverTextDrawer.NewLine(26);
						hoverTextDrawer.DrawText(text4, this.Styles_BodyText.Standard);
					}
					if (text5 != "" && roomType != Db.Get().RoomTypes.Neutral)
					{
						hoverTextDrawer.NewLine(26);
						hoverTextDrawer.DrawText(text5, this.Styles_BodyText.Standard);
					}
					hoverTextDrawer.NewLine(22);
					hoverTextDrawer.DrawText(RoomDetails.RoomDetailString(room), this.Styles_BodyText.Standard);
					if (text6 != "")
					{
						hoverTextDrawer.NewLine(26);
						hoverTextDrawer.DrawText(text6, this.Styles_BodyText.Standard);
					}
					if (text7 != "")
					{
						hoverTextDrawer.NewLine(26);
						hoverTextDrawer.DrawText(text7, this.Styles_BodyText.Standard);
					}
				}
				else
				{
					string text8 = UI.OVERLAYS.ROOMS.NOROOM.DESC;
					int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
					if (cavityForCell.numCells > maxRoomSize)
					{
						text8 = text8 + "\n" + string.Format(UI.OVERLAYS.ROOMS.NOROOM.TOO_BIG, cavityForCell.numCells, maxRoomSize);
					}
					hoverTextDrawer.NewLine(26);
					hoverTextDrawer.DrawText(text8, this.Styles_BodyText.Standard);
				}
				hoverTextDrawer.EndShadowBar();
			}
		}
		else if (mode == OverlayModes.Light.ID)
		{
			if (flag3)
			{
				text = string.Concat(new string[]
				{
					text,
					string.Format(UI.OVERLAYS.LIGHTING.DESC, Grid.LightIntensity[num]),
					" (",
					GameUtil.GetLightDescription(Grid.LightIntensity[num]),
					")"
				});
				hoverTextDrawer.BeginShadowBar(false);
				hoverTextDrawer.DrawText(UI.OVERLAYS.LIGHTING.HOVERTITLE, this.Styles_Title.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawText(text, this.Styles_BodyText.Standard);
				hoverTextDrawer.EndShadowBar();
			}
		}
		else if (mode == OverlayModes.Radiation.ID)
		{
			if (flag3)
			{
				flag2 = true;
				text += UI.OVERLAYS.RADIATION.DESC.Replace("{rads}", GameUtil.GetFormattedRads(Grid.Radiation[num], GameUtil.TimeSlice.None)).Replace("{description}", GameUtil.GetRadiationDescription(Grid.Radiation[num]));
				string text9 = UI.OVERLAYS.RADIATION.SHIELDING_DESC.Replace("{radiationAbsorptionFactor}", GameUtil.GetFormattedPercent(GameUtil.GetRadiationAbsorptionPercentage(num) * 100f, GameUtil.TimeSlice.None));
				hoverTextDrawer.BeginShadowBar(false);
				hoverTextDrawer.DrawText(UI.OVERLAYS.RADIATION.HOVERTITLE, this.Styles_Title.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawText(text, this.Styles_BodyText.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawText(text9, this.Styles_BodyText.Standard);
				hoverTextDrawer.EndShadowBar();
			}
		}
		else if (mode == OverlayModes.Logic.ID)
		{
			foreach (KSelectable kselectable2 in hoverObjects)
			{
				LogicPorts component2 = kselectable2.GetComponent<LogicPorts>();
				LogicPorts.Port port;
				bool flag5;
				if (component2 != null && component2.TryGetPortAtCell(num, out port, out flag5))
				{
					bool flag6 = component2.IsPortConnected(port.id);
					hoverTextDrawer.BeginShadowBar(false);
					int num3;
					if (flag5)
					{
						string text10 = port.displayCustomName ? port.description : UI.LOGIC_PORTS.PORT_INPUT_DEFAULT_NAME.text;
						num3 = component2.GetInputValue(port.id);
						hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.LOGIC_INPUT_HOVER_FMT.Replace("{Port}", text10.ToUpper()).Replace("{Name}", kselectable2.GetProperName().ToUpper()), this.Styles_Title.Standard);
					}
					else
					{
						string text11 = port.displayCustomName ? port.description : UI.LOGIC_PORTS.PORT_OUTPUT_DEFAULT_NAME.text;
						num3 = component2.GetOutputValue(port.id);
						hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.LOGIC_OUTPUT_HOVER_FMT.Replace("{Port}", text11.ToUpper()).Replace("{Name}", kselectable2.GetProperName().ToUpper()), this.Styles_Title.Standard);
					}
					hoverTextDrawer.NewLine(26);
					TextStyleSetting style;
					if (flag6)
					{
						style = ((num3 == 1) ? this.Styles_LogicActive.Selected : this.Styles_LogicSignalInactive);
					}
					else
					{
						style = this.Styles_LogicActive.Standard;
					}
					this.DrawLogicIcon(hoverTextDrawer, (num3 == 1 && flag6) ? this.iconActiveAutomationPort : this.iconDash, style);
					this.DrawLogicText(hoverTextDrawer, port.activeDescription, style);
					hoverTextDrawer.NewLine(26);
					TextStyleSetting style2;
					if (flag6)
					{
						style2 = ((num3 == 0) ? this.Styles_LogicStandby.Selected : this.Styles_LogicSignalInactive);
					}
					else
					{
						style2 = this.Styles_LogicStandby.Standard;
					}
					this.DrawLogicIcon(hoverTextDrawer, (num3 == 0 && flag6) ? this.iconActiveAutomationPort : this.iconDash, style2);
					this.DrawLogicText(hoverTextDrawer, port.inactiveDescription, style2);
					hoverTextDrawer.EndShadowBar();
				}
				LogicGate component3 = kselectable2.GetComponent<LogicGate>();
				LogicGateBase.PortId portId;
				if (component3 != null && component3.TryGetPortAtCell(num, out portId))
				{
					int portValue = component3.GetPortValue(portId);
					bool portConnected = component3.GetPortConnected(portId);
					LogicGate.LogicGateDescriptions.Description portDescription = component3.GetPortDescription(portId);
					hoverTextDrawer.BeginShadowBar(false);
					if (portId == LogicGateBase.PortId.OutputOne)
					{
						hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.LOGIC_MULTI_OUTPUT_HOVER_FMT.Replace("{Port}", portDescription.name.ToUpper()).Replace("{Name}", kselectable2.GetProperName().ToUpper()), this.Styles_Title.Standard);
					}
					else
					{
						hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.LOGIC_MULTI_INPUT_HOVER_FMT.Replace("{Port}", portDescription.name.ToUpper()).Replace("{Name}", kselectable2.GetProperName().ToUpper()), this.Styles_Title.Standard);
					}
					hoverTextDrawer.NewLine(26);
					TextStyleSetting style3;
					if (portConnected)
					{
						style3 = ((portValue == 1) ? this.Styles_LogicActive.Selected : this.Styles_LogicSignalInactive);
					}
					else
					{
						style3 = this.Styles_LogicActive.Standard;
					}
					this.DrawLogicIcon(hoverTextDrawer, (portValue == 1 && portConnected) ? this.iconActiveAutomationPort : this.iconDash, style3);
					this.DrawLogicText(hoverTextDrawer, portDescription.active, style3);
					hoverTextDrawer.NewLine(26);
					TextStyleSetting style4;
					if (portConnected)
					{
						style4 = ((portValue == 0) ? this.Styles_LogicStandby.Selected : this.Styles_LogicSignalInactive);
					}
					else
					{
						style4 = this.Styles_LogicStandby.Standard;
					}
					this.DrawLogicIcon(hoverTextDrawer, (portValue == 0 && portConnected) ? this.iconActiveAutomationPort : this.iconDash, style4);
					this.DrawLogicText(hoverTextDrawer, portDescription.inactive, style4);
					hoverTextDrawer.EndShadowBar();
				}
			}
		}
		int num4 = 0;
		ChoreConsumer choreConsumer = null;
		if (SelectTool.Instance.selected != null)
		{
			choreConsumer = SelectTool.Instance.selected.GetComponent<ChoreConsumer>();
		}
		for (int k = 0; k < this.overlayValidHoverObjects.Count; k++)
		{
			if (this.overlayValidHoverObjects[k] != null && !CellSelectionObject.IsSelectionObject(this.overlayValidHoverObjects[k].gameObject))
			{
				KSelectable kselectable3 = this.overlayValidHoverObjects[k];
				if ((!(OverlayScreen.Instance != null) || !(OverlayScreen.Instance.mode != OverlayModes.None.ID) || (kselectable3.gameObject.layer & this.maskOverlay) == 0) && flag3)
				{
					PrimaryElement component4 = kselectable3.GetComponent<PrimaryElement>();
					bool flag7 = SelectTool.Instance.selected == this.overlayValidHoverObjects[k];
					if (flag7)
					{
						this.currentSelectedSelectableIndex = k;
					}
					num4++;
					hoverTextDrawer.BeginShadowBar(flag7);
					string text12 = GameUtil.GetUnitFormattedName(this.overlayValidHoverObjects[k].gameObject, true);
					if (component4 != null && kselectable3.GetComponent<Building>() != null)
					{
						text12 = StringFormatter.Replace(StringFormatter.Replace(UI.TOOLS.GENERIC.BUILDING_HOVER_NAME_FMT, "{Name}", text12), "{Element}", component4.Element.nameUpperCase);
					}
					hoverTextDrawer.DrawText(text12, this.Styles_Title.Standard);
					bool flag8 = false;
					string text13 = UI.OVERLAYS.DISEASE.NO_DISEASE;
					if (flag)
					{
						if (component4 != null && component4.DiseaseIdx != 255)
						{
							text13 = GameUtil.GetFormattedDisease(component4.DiseaseIdx, component4.DiseaseCount, true);
						}
						flag8 = true;
						Storage component5 = kselectable3.GetComponent<Storage>();
						if (component5 != null && component5.showInUI)
						{
							List<GameObject> items = component5.items;
							for (int l = 0; l < items.Count; l++)
							{
								GameObject gameObject = items[l];
								if (gameObject != null)
								{
									PrimaryElement component6 = gameObject.GetComponent<PrimaryElement>();
									if (component6.DiseaseIdx != 255)
									{
										text13 += string.Format(UI.OVERLAYS.DISEASE.CONTAINER_FORMAT, gameObject.GetComponent<KSelectable>().GetProperName(), GameUtil.GetFormattedDisease(component6.DiseaseIdx, component6.DiseaseCount, true));
									}
								}
							}
						}
					}
					if (flag8)
					{
						hoverTextDrawer.NewLine(26);
						hoverTextDrawer.DrawIcon(this.iconDash, 18);
						hoverTextDrawer.DrawText(text13, this.Styles_Values.Property.Standard);
					}
					int num5 = 0;
					foreach (StatusItemGroup.Entry entry in this.overlayValidHoverObjects[k].GetStatusItemGroup())
					{
						if (this.ShowStatusItemInCurrentOverlay(entry.item))
						{
							if (num5 >= SelectToolHoverTextCard.maxNumberOfDisplayedSelectableWarnings)
							{
								break;
							}
							if (entry.category != null && entry.category.Id == "Main" && num5 < SelectToolHoverTextCard.maxNumberOfDisplayedSelectableWarnings)
							{
								TextStyleSetting style5 = this.IsStatusItemWarning(entry) ? this.HoverTextStyleSettings[1] : this.Styles_BodyText.Standard;
								Sprite icon = (entry.item.sprite != null) ? entry.item.sprite.sprite : this.iconWarning;
								Color color = this.IsStatusItemWarning(entry) ? this.HoverTextStyleSettings[1].textColor : this.Styles_BodyText.Standard.textColor;
								hoverTextDrawer.NewLine(26);
								hoverTextDrawer.DrawIcon(icon, color, 18, 2);
								hoverTextDrawer.DrawText(entry.GetName(), style5);
								num5++;
							}
						}
					}
					foreach (StatusItemGroup.Entry entry2 in this.overlayValidHoverObjects[k].GetStatusItemGroup())
					{
						if (this.ShowStatusItemInCurrentOverlay(entry2.item))
						{
							if (num5 >= SelectToolHoverTextCard.maxNumberOfDisplayedSelectableWarnings)
							{
								break;
							}
							if ((entry2.category == null || entry2.category.Id != "Main") && num5 < SelectToolHoverTextCard.maxNumberOfDisplayedSelectableWarnings)
							{
								TextStyleSetting style6 = this.IsStatusItemWarning(entry2) ? this.HoverTextStyleSettings[1] : this.Styles_BodyText.Standard;
								Sprite icon2 = (entry2.item.sprite != null) ? entry2.item.sprite.sprite : this.iconWarning;
								Color color2 = this.IsStatusItemWarning(entry2) ? this.HoverTextStyleSettings[1].textColor : this.Styles_BodyText.Standard.textColor;
								hoverTextDrawer.NewLine(26);
								hoverTextDrawer.DrawIcon(icon2, color2, 18, 2);
								hoverTextDrawer.DrawText(entry2.GetName(), style6);
								num5++;
							}
						}
					}
					float temp = 0f;
					bool flag9 = true;
					bool flag10 = OverlayModes.Temperature.ID == SimDebugView.Instance.GetMode() && Game.Instance.temperatureOverlayMode != Game.TemperatureOverlayModes.HeatFlow;
					if (kselectable3.GetComponent<Constructable>())
					{
						flag9 = false;
					}
					else if (flag10 && component4)
					{
						temp = component4.Temperature;
					}
					else if (kselectable3.GetComponent<Building>() && component4)
					{
						temp = component4.Temperature;
					}
					else if (CellSelectionObject.IsSelectionObject(kselectable3.gameObject))
					{
						temp = kselectable3.GetComponent<CellSelectionObject>().temperature;
					}
					else
					{
						flag9 = false;
					}
					if (mode != OverlayModes.None.ID && mode != OverlayModes.Temperature.ID)
					{
						flag9 = false;
					}
					if (flag9)
					{
						hoverTextDrawer.NewLine(26);
						hoverTextDrawer.DrawIcon(this.iconDash, 18);
						hoverTextDrawer.DrawText(GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), this.Styles_BodyText.Standard);
					}
					BuildingComplete component7 = kselectable3.GetComponent<BuildingComplete>();
					if (component7 != null && component7.Def.IsFoundation && Grid.Element[num].IsSolid)
					{
						flag2 = false;
					}
					if (mode == OverlayModes.Light.ID && choreConsumer != null)
					{
						bool flag11 = false;
						foreach (Type type in SelectToolHoverTextCard.hiddenChoreConsumerTypes)
						{
							if (choreConsumer.gameObject.GetComponent(type) != null)
							{
								flag11 = true;
								break;
							}
						}
						if (!flag11)
						{
							choreConsumer.ShowHoverTextOnHoveredItem(kselectable3, hoverTextDrawer, this);
						}
					}
					hoverTextDrawer.EndShadowBar();
				}
			}
		}
		if (flag2)
		{
			CellSelectionObject cellSelectionObject = null;
			if (SelectTool.Instance.selected != null)
			{
				cellSelectionObject = SelectTool.Instance.selected.GetComponent<CellSelectionObject>();
			}
			bool flag12 = cellSelectionObject != null && cellSelectionObject.mouseCell == cellSelectionObject.alternateSelectionObject.mouseCell;
			if (flag12)
			{
				this.currentSelectedSelectableIndex = this.recentNumberOfDisplayedSelectables - 1;
			}
			Element element = Grid.Element[num];
			hoverTextDrawer.BeginShadowBar(flag12);
			hoverTextDrawer.DrawText(element.nameUpperCase, this.Styles_Title.Standard);
			if (Grid.DiseaseCount[num] > 0 || flag)
			{
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(this.iconDash, 18);
				hoverTextDrawer.DrawText(GameUtil.GetFormattedDisease(Grid.DiseaseIdx[num], Grid.DiseaseCount[num], true), this.Styles_Values.Property.Standard);
			}
			if (!element.IsVacuum)
			{
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(this.iconDash, 18);
				hoverTextDrawer.DrawText(ElementLoader.elements[(int)Grid.ElementIdx[num]].GetMaterialCategoryTag().ProperName(), this.Styles_BodyText.Standard);
			}
			string[] array = HoverTextHelper.MassStringsReadOnly(num);
			hoverTextDrawer.NewLine(26);
			hoverTextDrawer.DrawIcon(this.iconDash, 18);
			for (int m = 0; m < array.Length; m++)
			{
				if (m >= 3 || !element.IsVacuum)
				{
					hoverTextDrawer.DrawText(array[m], this.Styles_BodyText.Standard);
				}
			}
			if (!element.IsVacuum)
			{
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(this.iconDash, 18);
				Element element2 = Grid.Element[num];
				string formattedTemperature = this.cachedTemperatureString;
				float num6 = Grid.Temperature[num];
				if (num6 != this.cachedTemperature)
				{
					this.cachedTemperature = num6;
					formattedTemperature = GameUtil.GetFormattedTemperature(Grid.Temperature[num], GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
					this.cachedTemperatureString = formattedTemperature;
				}
				string text14 = (element2.specificHeatCapacity == 0f) ? "N/A" : formattedTemperature;
				hoverTextDrawer.DrawText(text14, this.Styles_BodyText.Standard);
			}
			if (CellSelectionObject.IsExposedToSpace(num))
			{
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(this.iconDash, 18);
				hoverTextDrawer.DrawText(MISC.STATUSITEMS.SPACE.NAME, this.Styles_BodyText.Standard);
			}
			if (Game.Instance.GetComponent<EntombedItemVisualizer>().IsEntombedItem(num))
			{
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(this.iconDash, 18);
				hoverTextDrawer.DrawText(MISC.STATUSITEMS.BURIEDITEM.NAME, this.Styles_BodyText.Standard);
			}
			int num7 = Grid.CellAbove(num);
			bool flag13 = element.IsLiquid && Grid.IsValidCell(num7) && (Grid.Element[num7].IsGas || Grid.Element[num7].IsVacuum);
			if (element.sublimateId != (SimHashes)0 && (element.IsSolid || flag13))
			{
				float mass = Grid.AccumulatedFlow[num] / 3f;
				string elementNameByElementHash = GameUtil.GetElementNameByElementHash(element.id);
				string elementNameByElementHash2 = GameUtil.GetElementNameByElementHash(element.sublimateId);
				string text15 = BUILDING.STATUSITEMS.EMITTINGGASAVG.NAME;
				text15 = text15.Replace("{FlowRate}", GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				text15 = text15.Replace("{Element}", elementNameByElementHash2);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(this.iconDash, 18);
				hoverTextDrawer.DrawText(text15, this.Styles_BodyText.Standard);
				bool flag14;
				bool flag15;
				GameUtil.IsEmissionBlocked(num, out flag14, out flag15);
				string text16 = null;
				if (flag14)
				{
					text16 = MISC.STATUSITEMS.SUBLIMATIONBLOCKED.NAME;
				}
				else if (flag15)
				{
					text16 = MISC.STATUSITEMS.SUBLIMATIONOVERPRESSURE.NAME;
				}
				if (text16 != null)
				{
					text16 = text16.Replace("{Element}", elementNameByElementHash);
					text16 = text16.Replace("{SubElement}", elementNameByElementHash2);
					hoverTextDrawer.NewLine(26);
					hoverTextDrawer.DrawIcon(this.iconDash, 18);
					hoverTextDrawer.DrawText(text16, this.Styles_BodyText.Standard);
				}
			}
			hoverTextDrawer.EndShadowBar();
		}
		else if (!flag3 && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.BeginShadowBar(false);
			hoverTextDrawer.DrawIcon(this.iconWarning, 18);
			hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.UNKNOWN, this.Styles_BodyText.Standard);
			hoverTextDrawer.EndShadowBar();
		}
		this.recentNumberOfDisplayedSelectables = num4 + 1;
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x06005191 RID: 20881 RVA: 0x001D17F4 File Offset: 0x001CF9F4
	public void DrawLogicIcon(HoverTextDrawer drawer, Sprite icon, TextStyleSetting style)
	{
		drawer.DrawIcon(icon, this.GetLogicColorFromStyle(style), 18, 2);
	}

	// Token: 0x06005192 RID: 20882 RVA: 0x001D1807 File Offset: 0x001CFA07
	public void DrawLogicText(HoverTextDrawer drawer, string text, TextStyleSetting style)
	{
		drawer.DrawText(text, style, this.GetLogicColorFromStyle(style), true);
	}

	// Token: 0x06005193 RID: 20883 RVA: 0x001D181C File Offset: 0x001CFA1C
	private Color GetLogicColorFromStyle(TextStyleSetting style)
	{
		ColorSet colorSet = GlobalAssets.Instance.colorSet;
		if (style == this.Styles_LogicActive.Selected)
		{
			return colorSet.logicOnText;
		}
		if (style == this.Styles_LogicStandby.Selected)
		{
			return colorSet.logicOffText;
		}
		return style.textColor;
	}

	// Token: 0x06005194 RID: 20884 RVA: 0x001D1878 File Offset: 0x001CFA78
	private bool ShowStatusItemInCurrentOverlay(StatusItem status)
	{
		return !(OverlayScreen.Instance == null) && (status.status_overlays & (int)StatusItem.GetStatusItemOverlayBySimViewMode(OverlayScreen.Instance.GetMode())) == (int)StatusItem.GetStatusItemOverlayBySimViewMode(OverlayScreen.Instance.GetMode());
	}

	// Token: 0x06005195 RID: 20885 RVA: 0x001D18B0 File Offset: 0x001CFAB0
	private bool ShouldShowSelectableInCurrentOverlay(KSelectable selectable)
	{
		bool result = true;
		if (OverlayScreen.Instance == null)
		{
			return result;
		}
		if (selectable == null)
		{
			return false;
		}
		if (selectable.GetComponent<KPrefabID>() == null)
		{
			return result;
		}
		HashedString mode = OverlayScreen.Instance.GetMode();
		Func<KSelectable, bool> func;
		if (this.modeFilters.TryGetValue(mode, out func))
		{
			result = func(selectable);
		}
		return result;
	}

	// Token: 0x06005196 RID: 20886 RVA: 0x001D190D File Offset: 0x001CFB0D
	private static bool ShouldShowOxygenOverlay(KSelectable selectable)
	{
		return selectable.GetComponent<AlgaeHabitat>() != null || selectable.GetComponent<Electrolyzer>() != null || selectable.GetComponent<AirFilter>() != null;
	}

	// Token: 0x06005197 RID: 20887 RVA: 0x001D193C File Offset: 0x001CFB3C
	private static bool ShouldShowLightOverlay(KSelectable selectable)
	{
		return selectable.GetComponent<Light2D>() != null;
	}

	// Token: 0x06005198 RID: 20888 RVA: 0x001D194A File Offset: 0x001CFB4A
	private static bool ShouldShowRadiationOverlay(KSelectable selectable)
	{
		return selectable.GetComponent<HighEnergyParticle>() != null || selectable.GetComponent<HighEnergyParticlePort>();
	}

	// Token: 0x06005199 RID: 20889 RVA: 0x001D1968 File Offset: 0x001CFB68
	private static bool ShouldShowGasConduitOverlay(KSelectable selectable)
	{
		return (selectable.GetComponent<Conduit>() != null && selectable.GetComponent<Conduit>().type == ConduitType.Gas) || (selectable.GetComponent<Filterable>() != null && selectable.GetComponent<Filterable>().filterElementState == Filterable.ElementState.Gas) || (selectable.GetComponent<Vent>() != null && selectable.GetComponent<Vent>().conduitType == ConduitType.Gas) || (selectable.GetComponent<Pump>() != null && selectable.GetComponent<Pump>().conduitType == ConduitType.Gas) || (selectable.GetComponent<ValveBase>() != null && selectable.GetComponent<ValveBase>().conduitType == ConduitType.Gas);
	}

	// Token: 0x0600519A RID: 20890 RVA: 0x001D1A24 File Offset: 0x001CFC24
	private static bool ShouldShowLiquidConduitOverlay(KSelectable selectable)
	{
		return (selectable.GetComponent<Conduit>() != null && selectable.GetComponent<Conduit>().type == ConduitType.Liquid) || (selectable.GetComponent<Filterable>() != null && selectable.GetComponent<Filterable>().filterElementState == Filterable.ElementState.Liquid) || (selectable.GetComponent<Vent>() != null && selectable.GetComponent<Vent>().conduitType == ConduitType.Liquid) || (selectable.GetComponent<Pump>() != null && selectable.GetComponent<Pump>().conduitType == ConduitType.Liquid) || (selectable.GetComponent<ValveBase>() != null && selectable.GetComponent<ValveBase>().conduitType == ConduitType.Liquid);
	}

	// Token: 0x0600519B RID: 20891 RVA: 0x001D1AE0 File Offset: 0x001CFCE0
	private static bool ShouldShowPowerOverlay(KSelectable selectable)
	{
		Tag prefabTag = selectable.GetComponent<KPrefabID>().PrefabTag;
		return OverlayScreen.WireIDs.Contains(prefabTag) || selectable.GetComponent<Battery>() != null || selectable.GetComponent<PowerTransformer>() != null || selectable.GetComponent<EnergyConsumer>() != null || selectable.GetComponent<EnergyGenerator>() != null;
	}

	// Token: 0x0600519C RID: 20892 RVA: 0x001D1B48 File Offset: 0x001CFD48
	private static bool ShouldShowTileOverlay(KSelectable selectable)
	{
		bool result = false;
		PrimaryElement component = selectable.GetComponent<PrimaryElement>();
		if (component != null)
		{
			Element element = component.Element;
			foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
			{
				if (element.HasTag(search_tag))
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x0600519D RID: 20893 RVA: 0x001D1BC4 File Offset: 0x001CFDC4
	private static bool ShouldShowTemperatureOverlay(KSelectable selectable)
	{
		return selectable.GetComponent<PrimaryElement>() != null;
	}

	// Token: 0x0600519E RID: 20894 RVA: 0x001D1BD4 File Offset: 0x001CFDD4
	private static bool ShouldShowLogicOverlay(KSelectable selectable)
	{
		Tag prefabTag = selectable.GetComponent<KPrefabID>().PrefabTag;
		return OverlayModes.Logic.HighlightItemIDs.Contains(prefabTag) || selectable.GetComponent<LogicPorts>() != null;
	}

	// Token: 0x0600519F RID: 20895 RVA: 0x001D1C08 File Offset: 0x001CFE08
	private static bool ShouldShowSolidConveyorOverlay(KSelectable selectable)
	{
		Tag prefabTag = selectable.GetComponent<KPrefabID>().PrefabTag;
		return OverlayScreen.SolidConveyorIDs.Contains(prefabTag);
	}

	// Token: 0x060051A0 RID: 20896 RVA: 0x001D1C2C File Offset: 0x001CFE2C
	private static bool HideInOverlay(KSelectable selectable)
	{
		return false;
	}

	// Token: 0x060051A1 RID: 20897 RVA: 0x001D1C2F File Offset: 0x001CFE2F
	private static bool ShowOverlayIfHasComponent<T>(KSelectable selectable)
	{
		return selectable.GetComponent<T>() != null;
	}

	// Token: 0x060051A2 RID: 20898 RVA: 0x001D1C3F File Offset: 0x001CFE3F
	private static bool ShouldShowCropOverlay(KSelectable selectable)
	{
		return selectable.GetComponent<Uprootable>() != null || selectable.GetComponent<PlanterBox>() != null;
	}

	// Token: 0x04003582 RID: 13698
	public static int maxNumberOfDisplayedSelectableWarnings = 10;

	// Token: 0x04003583 RID: 13699
	private Dictionary<HashedString, Func<bool>> overlayFilterMap = new Dictionary<HashedString, Func<bool>>();

	// Token: 0x04003584 RID: 13700
	public int recentNumberOfDisplayedSelectables;

	// Token: 0x04003585 RID: 13701
	public int currentSelectedSelectableIndex = -1;

	// Token: 0x04003586 RID: 13702
	[NonSerialized]
	public Sprite iconWarning;

	// Token: 0x04003587 RID: 13703
	[NonSerialized]
	public Sprite iconDash;

	// Token: 0x04003588 RID: 13704
	[NonSerialized]
	public Sprite iconHighlighted;

	// Token: 0x04003589 RID: 13705
	[NonSerialized]
	public Sprite iconActiveAutomationPort;

	// Token: 0x0400358A RID: 13706
	public HoverTextConfiguration.TextStylePair Styles_LogicActive;

	// Token: 0x0400358B RID: 13707
	public HoverTextConfiguration.TextStylePair Styles_LogicStandby;

	// Token: 0x0400358C RID: 13708
	public TextStyleSetting Styles_LogicSignalInactive;

	// Token: 0x0400358D RID: 13709
	public static List<GameObject> highlightedObjects = new List<GameObject>();

	// Token: 0x0400358E RID: 13710
	private static readonly List<Type> hiddenChoreConsumerTypes = new List<Type>
	{
		typeof(KSelectableHealthBar)
	};

	// Token: 0x0400358F RID: 13711
	private int maskOverlay;

	// Token: 0x04003590 RID: 13712
	private string cachedTemperatureString;

	// Token: 0x04003591 RID: 13713
	private float cachedTemperature = float.MinValue;

	// Token: 0x04003592 RID: 13714
	private List<KSelectable> overlayValidHoverObjects = new List<KSelectable>();

	// Token: 0x04003593 RID: 13715
	private Dictionary<HashedString, Func<KSelectable, bool>> modeFilters = new Dictionary<HashedString, Func<KSelectable, bool>>
	{
		{
			OverlayModes.Oxygen.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowOxygenOverlay)
		},
		{
			OverlayModes.Light.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowLightOverlay)
		},
		{
			OverlayModes.Radiation.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowRadiationOverlay)
		},
		{
			OverlayModes.GasConduits.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowGasConduitOverlay)
		},
		{
			OverlayModes.LiquidConduits.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowLiquidConduitOverlay)
		},
		{
			OverlayModes.SolidConveyor.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowSolidConveyorOverlay)
		},
		{
			OverlayModes.Power.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowPowerOverlay)
		},
		{
			OverlayModes.Logic.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowLogicOverlay)
		},
		{
			OverlayModes.TileMode.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowTileOverlay)
		},
		{
			OverlayModes.Disease.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShowOverlayIfHasComponent<PrimaryElement>)
		},
		{
			OverlayModes.Decor.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShowOverlayIfHasComponent<DecorProvider>)
		},
		{
			OverlayModes.Crop.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowCropOverlay)
		},
		{
			OverlayModes.Temperature.ID,
			new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowTemperatureOverlay)
		}
	};
}
