using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000CF1 RID: 3313
	public class BuildingStatusItems : StatusItems
	{
		// Token: 0x06006966 RID: 26982 RVA: 0x0027F98E File Offset: 0x0027DB8E
		public BuildingStatusItems(ResourceSet parent) : base("BuildingStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x0027F9A4 File Offset: 0x0027DBA4
		private StatusItem CreateStatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 129022)
		{
			return base.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, null));
		}

		// Token: 0x06006968 RID: 26984 RVA: 0x0027F9CC File Offset: 0x0027DBCC
		private StatusItem CreateStatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 129022)
		{
			return base.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays, true, null));
		}

		// Token: 0x06006969 RID: 26985 RVA: 0x0027F9F8 File Offset: 0x0027DBF8
		private void CreateStatusItems()
		{
			this.AngerDamage = this.CreateStatusItem("AngerDamage", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.AssignedTo = this.CreateStatusItem("AssignedTo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.AssignedTo.resolveStringCallback = delegate(string str, object data)
			{
				IAssignableIdentity assignee = ((Assignable)data).assignee;
				if (!assignee.IsNullOrDestroyed())
				{
					string properName = assignee.GetProperName();
					str = str.Replace("{Assignee}", properName);
				}
				return str;
			};
			this.AssignedToRoom = this.CreateStatusItem("AssignedToRoom", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.AssignedToRoom.resolveStringCallback = delegate(string str, object data)
			{
				IAssignableIdentity assignee = ((Assignable)data).assignee;
				if (!assignee.IsNullOrDestroyed())
				{
					string properName = assignee.GetProperName();
					str = str.Replace("{Assignee}", properName);
				}
				return str;
			};
			this.Broken = this.CreateStatusItem("Broken", "BUILDING", "status_item_broken", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Broken.resolveStringCallback = ((string str, object data) => str.Replace("{DamageInfo}", ((BuildingHP.SMInstance)data).master.GetDamageSourceInfo().ToString()));
			this.Broken.conditionalOverlayCallback = new Func<HashedString, object, bool>(BuildingStatusItems.ShowInUtilityOverlay);
			this.ChangeDoorControlState = this.CreateStatusItem("ChangeDoorControlState", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ChangeDoorControlState.resolveStringCallback = delegate(string str, object data)
			{
				Door door = (Door)data;
				return str.Replace("{ControlState}", door.RequestedState.ToString());
			};
			this.CurrentDoorControlState = this.CreateStatusItem("CurrentDoorControlState", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CurrentDoorControlState.resolveStringCallback = delegate(string str, object data)
			{
				Door door = (Door)data;
				string newValue = Strings.Get("STRINGS.BUILDING.STATUSITEMS.CURRENTDOORCONTROLSTATE." + door.CurrentState.ToString().ToUpper());
				return str.Replace("{ControlState}", newValue);
			};
			this.ClinicOutsideHospital = this.CreateStatusItem("ClinicOutsideHospital", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
			this.ConduitBlocked = this.CreateStatusItem("ConduitBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.OutputPipeFull = this.CreateStatusItem("OutputPipeFull", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.OutputTileBlocked = this.CreateStatusItem("OutputTileBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ConstructionUnreachable = this.CreateStatusItem("ConstructionUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ConduitBlockedMultiples = this.CreateStatusItem("ConduitBlockedMultiples", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID, true, 129022);
			this.SolidConduitBlockedMultiples = this.CreateStatusItem("SolidConduitBlockedMultiples", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID, true, 129022);
			this.DigUnreachable = this.CreateStatusItem("DigUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MopUnreachable = this.CreateStatusItem("MopUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.StorageUnreachable = this.CreateStatusItem("StorageUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.PassengerModuleUnreachable = this.CreateStatusItem("PassengerModuleUnreachable", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DirectionControl = this.CreateStatusItem("DirectionControl", BUILDING.STATUSITEMS.DIRECTION_CONTROL.NAME, BUILDING.STATUSITEMS.DIRECTION_CONTROL.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.DirectionControl.resolveStringCallback = delegate(string str, object data)
			{
				DirectionControl directionControl = (DirectionControl)data;
				string newValue = BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.BOTH;
				WorkableReactable.AllowedDirection allowedDirection = directionControl.allowedDirection;
				if (allowedDirection != WorkableReactable.AllowedDirection.Left)
				{
					if (allowedDirection == WorkableReactable.AllowedDirection.Right)
					{
						newValue = BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.RIGHT;
					}
				}
				else
				{
					newValue = BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.LEFT;
				}
				str = str.Replace("{Direction}", newValue);
				return str;
			};
			this.DeadReactorCoolingOff = this.CreateStatusItem("DeadReactorCoolingOff", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DeadReactorCoolingOff.resolveStringCallback = delegate(string str, object data)
			{
				Reactor.StatesInstance smi = (Reactor.StatesInstance)data;
				float num = ((Reactor.StatesInstance)data).sm.timeSinceMeltdown.Get(smi);
				str = str.Replace("{CyclesRemaining}", Util.FormatOneDecimalPlace(Mathf.Max(0f, 3000f - num) / 600f));
				return str;
			};
			this.ConstructableDigUnreachable = this.CreateStatusItem("ConstructableDigUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Entombed = this.CreateStatusItem("Entombed", "BUILDING", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Entombed.AddNotification(null, null, null);
			this.Flooded = this.CreateStatusItem("Flooded", "BUILDING", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Flooded.AddNotification(null, null, null);
			this.GasVentObstructed = this.CreateStatusItem("GasVentObstructed", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
			this.GasVentOverPressure = this.CreateStatusItem("GasVentOverPressure", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
			this.GeneShuffleCompleted = this.CreateStatusItem("GeneShuffleCompleted", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.GeneticAnalysisCompleted = this.CreateStatusItem("GeneticAnalysisCompleted", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.InvalidBuildingLocation = this.CreateStatusItem("InvalidBuildingLocation", "BUILDING", "status_item_missing_foundation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.LiquidVentObstructed = this.CreateStatusItem("LiquidVentObstructed", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.LiquidVentOverPressure = this.CreateStatusItem("LiquidVentOverPressure", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.MaterialsUnavailable = new MaterialsStatusItem("MaterialsUnavailable", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.None.ID);
			this.MaterialsUnavailable.AddNotification(null, null, null);
			this.MaterialsUnavailable.resolveStringCallback = delegate(string str, object data)
			{
				string text = "";
				Dictionary<Tag, float> dictionary = null;
				if (data is IFetchList)
				{
					dictionary = ((IFetchList)data).GetRemainingMinimum();
				}
				else if (data is Dictionary<Tag, float>)
				{
					dictionary = (data as Dictionary<Tag, float>);
				}
				if (dictionary.Count > 0)
				{
					bool flag = true;
					foreach (KeyValuePair<Tag, float> keyValuePair in dictionary)
					{
						if (keyValuePair.Value != 0f)
						{
							if (!flag)
							{
								text += "\n";
							}
							if (Assets.IsTagCountable(keyValuePair.Key))
							{
								text += string.Format(BUILDING.STATUSITEMS.MATERIALSUNAVAILABLE.LINE_ITEM_UNITS, GameUtil.GetUnitFormattedName(keyValuePair.Key.ProperName(), keyValuePair.Value, false));
							}
							else
							{
								text += string.Format(BUILDING.STATUSITEMS.MATERIALSUNAVAILABLE.LINE_ITEM_MASS, keyValuePair.Key.ProperName(), GameUtil.GetFormattedMass(keyValuePair.Value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							flag = false;
						}
					}
				}
				str = str.Replace("{ItemsRemaining}", text);
				return str;
			};
			this.MaterialsUnavailableForRefill = new MaterialsStatusItem("MaterialsUnavailableForRefill", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID);
			this.MaterialsUnavailableForRefill.resolveStringCallback = delegate(string str, object data)
			{
				IFetchList fetchList = (IFetchList)data;
				string text = "";
				Dictionary<Tag, float> remaining = fetchList.GetRemaining();
				if (remaining.Count > 0)
				{
					bool flag = true;
					foreach (KeyValuePair<Tag, float> keyValuePair in remaining)
					{
						if (keyValuePair.Value != 0f)
						{
							if (!flag)
							{
								text += "\n";
							}
							text += string.Format(BUILDING.STATUSITEMS.MATERIALSUNAVAILABLEFORREFILL.LINE_ITEM, keyValuePair.Key.ProperName());
							flag = false;
						}
					}
				}
				str = str.Replace("{ItemsRemaining}", text);
				return str;
			};
			Func<string, object, string> resolveStringCallback = delegate(string str, object data)
			{
				RoomType roomType = Db.Get().RoomTypes.Get((string)data);
				if (roomType != null)
				{
					return str.Replace("{0}", roomType.Name);
				}
				return str;
			};
			this.NoCoolant = this.CreateStatusItem("NoCoolant", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NotInAnyRoom = this.CreateStatusItem("NotInAnyRoom", "BUILDING", "status_item_room_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NotInRequiredRoom = this.CreateStatusItem("NotInRequiredRoom", "BUILDING", "status_item_room_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NotInRequiredRoom.resolveStringCallback = resolveStringCallback;
			this.NotInRecommendedRoom = this.CreateStatusItem("NotInRecommendedRoom", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NotInRecommendedRoom.resolveStringCallback = resolveStringCallback;
			this.WaitingForRepairMaterials = this.CreateStatusItem("WaitingForRepairMaterials", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Exclamation, NotificationType.Neutral, true, OverlayModes.None.ID, false, 129022);
			this.WaitingForRepairMaterials.resolveStringCallback = delegate(string str, object data)
			{
				KeyValuePair<Tag, float> keyValuePair = (KeyValuePair<Tag, float>)data;
				if (keyValuePair.Value != 0f)
				{
					string newValue = string.Format(BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_MASS, keyValuePair.Key.ProperName(), GameUtil.GetFormattedMass(keyValuePair.Value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
					str = str.Replace("{ItemsRemaining}", newValue);
				}
				return str;
			};
			this.WaitingForMaterials = new MaterialsStatusItem("WaitingForMaterials", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, true, OverlayModes.None.ID);
			this.WaitingForMaterials.resolveStringCallback = delegate(string str, object data)
			{
				IFetchList fetchList = (IFetchList)data;
				string text = "";
				Dictionary<Tag, float> remaining = fetchList.GetRemaining();
				if (remaining.Count > 0)
				{
					bool flag = true;
					foreach (KeyValuePair<Tag, float> keyValuePair in remaining)
					{
						if (keyValuePair.Value != 0f)
						{
							if (!flag)
							{
								text += "\n";
							}
							if (Assets.IsTagCountable(keyValuePair.Key))
							{
								text += string.Format(BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_UNITS, GameUtil.GetUnitFormattedName(keyValuePair.Key.ProperName(), keyValuePair.Value, false));
							}
							else
							{
								text += string.Format(BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_MASS, keyValuePair.Key.ProperName(), GameUtil.GetFormattedMass(keyValuePair.Value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							flag = false;
						}
					}
				}
				str = str.Replace("{ItemsRemaining}", text);
				return str;
			};
			this.WaitingForHighEnergyParticles = new StatusItem("WaitingForRadiation", "BUILDING", "status_item_need_high_energy_particles", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.MeltingDown = this.CreateStatusItem("MeltingDown", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MissingFoundation = this.CreateStatusItem("MissingFoundation", "BUILDING", "status_item_missing_foundation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NeutroniumUnminable = this.CreateStatusItem("NeutroniumUnminable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NeedGasIn = this.CreateStatusItem("NeedGasIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
			this.NeedGasIn.resolveStringCallback = delegate(string str, object data)
			{
				global::Tuple<ConduitType, Tag> tuple = (global::Tuple<ConduitType, Tag>)data;
				string newValue = string.Format(BUILDING.STATUSITEMS.NEEDGASIN.LINE_ITEM, tuple.second.ProperName());
				str = str.Replace("{GasRequired}", newValue);
				return str;
			};
			this.NeedGasOut = this.CreateStatusItem("NeedGasOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.GasConduits.ID, true, 129022);
			this.NeedLiquidIn = this.CreateStatusItem("NeedLiquidIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.NeedLiquidIn.resolveStringCallback = delegate(string str, object data)
			{
				global::Tuple<ConduitType, Tag> tuple = (global::Tuple<ConduitType, Tag>)data;
				string newValue = string.Format(BUILDING.STATUSITEMS.NEEDLIQUIDIN.LINE_ITEM, tuple.second.ProperName());
				str = str.Replace("{LiquidRequired}", newValue);
				return str;
			};
			this.NeedLiquidOut = this.CreateStatusItem("NeedLiquidOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.LiquidConduits.ID, true, 129022);
			this.NeedSolidIn = this.CreateStatusItem("NeedSolidIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.SolidConveyor.ID, true, 129022);
			this.NeedSolidOut = this.CreateStatusItem("NeedSolidOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.SolidConveyor.ID, true, 129022);
			this.NeedResourceMass = this.CreateStatusItem("NeedResourceMass", "BUILDING", "status_item_need_resource", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NeedResourceMass.resolveStringCallback = delegate(string str, object data)
			{
				string text = "";
				EnergyGenerator.Formula formula = (EnergyGenerator.Formula)data;
				if (formula.inputs.Length != 0)
				{
					bool flag = true;
					foreach (EnergyGenerator.InputItem inputItem in formula.inputs)
					{
						if (!flag)
						{
							text += "\n";
							flag = false;
						}
						text += string.Format(BUILDING.STATUSITEMS.NEEDRESOURCEMASS.LINE_ITEM, inputItem.tag.ProperName());
					}
				}
				str = str.Replace("{ResourcesRequired}", text);
				return str;
			};
			this.LiquidPipeEmpty = this.CreateStatusItem("LiquidPipeEmpty", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.LiquidPipeObstructed = this.CreateStatusItem("LiquidPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.LiquidConduits.ID, true, 129022);
			this.GasPipeEmpty = this.CreateStatusItem("GasPipeEmpty", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
			this.GasPipeObstructed = this.CreateStatusItem("GasPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.GasConduits.ID, true, 129022);
			this.SolidPipeObstructed = this.CreateStatusItem("SolidPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.SolidConveyor.ID, true, 129022);
			this.NeedPlant = this.CreateStatusItem("NeedPlant", "BUILDING", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NeedPower = this.CreateStatusItem("NeedPower", "BUILDING", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.NotEnoughPower = this.CreateStatusItem("NotEnoughPower", "BUILDING", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.PowerLoopDetected = this.CreateStatusItem("PowerLoopDetected", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.DispenseRequested = this.CreateStatusItem("DispenseRequested", "BUILDING", "status_item_exclamation", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NewDuplicantsAvailable = this.CreateStatusItem("NewDuplicantsAvailable", "BUILDING", "status_item_new_duplicants_available", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NewDuplicantsAvailable.AddNotification(null, null, null);
			this.NewDuplicantsAvailable.notificationClickCallback = delegate(object data)
			{
				int idx = 0;
				for (int i = 0; i < Components.Telepads.Items.Count; i++)
				{
					if (Components.Telepads[i].GetComponent<KSelectable>().IsSelected)
					{
						idx = (i + 1) % Components.Telepads.Items.Count;
						break;
					}
				}
				Telepad targetTelepad = Components.Telepads[idx];
				int myWorldId = targetTelepad.GetMyWorldId();
				CameraController.Instance.ActiveWorldStarWipe(myWorldId, targetTelepad.transform.GetPosition(), 10f, delegate()
				{
					SelectTool.Instance.Select(targetTelepad.GetComponent<KSelectable>(), false);
				});
			};
			this.NoStorageFilterSet = this.CreateStatusItem("NoStorageFilterSet", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoSuitMarker = this.CreateStatusItem("NoSuitMarker", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.SuitMarkerWrongSide = this.CreateStatusItem("suitMarkerWrongSide", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.SuitMarkerTraversalAnytime = this.CreateStatusItem("suitMarkerTraversalAnytime", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SuitMarkerTraversalOnlyWhenRoomAvailable = this.CreateStatusItem("suitMarkerTraversalOnlyWhenRoomAvailable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NoFishableWaterBelow = this.CreateStatusItem("NoFishableWaterBelow", "BUILDING", "status_item_no_fishable_water_below", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoPowerConsumers = this.CreateStatusItem("NoPowerConsumers", "BUILDING", "status_item_no_power_consumers", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.NoWireConnected = this.CreateStatusItem("NoWireConnected", "BUILDING", "status_item_no_wire_connected", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.Power.ID, true, 129022);
			this.NoLogicWireConnected = this.CreateStatusItem("NoLogicWireConnected", "BUILDING", "status_item_no_logic_wire_connected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Logic.ID, true, 129022);
			this.NoTubeConnected = this.CreateStatusItem("NoTubeConnected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoTubeExits = this.CreateStatusItem("NoTubeExits", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.StoredCharge = this.CreateStatusItem("StoredCharge", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.StoredCharge.resolveStringCallback = delegate(string str, object data)
			{
				TravelTubeEntrance.SMInstance sminstance = (TravelTubeEntrance.SMInstance)data;
				if (sminstance != null)
				{
					str = string.Format(str, GameUtil.GetFormattedRoundedJoules(sminstance.master.AvailableJoules), GameUtil.GetFormattedRoundedJoules(sminstance.master.TotalCapacity), GameUtil.GetFormattedRoundedJoules(sminstance.master.UsageJoules));
				}
				return str;
			};
			this.PendingDeconstruction = this.CreateStatusItem("PendingDeconstruction", "BUILDING", "status_item_pending_deconstruction", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingDeconstruction.conditionalOverlayCallback = new Func<HashedString, object, bool>(BuildingStatusItems.ShowInUtilityOverlay);
			this.PendingDemolition = this.CreateStatusItem("PendingDemolition", "BUILDING", "status_item_pending_deconstruction", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingDemolition.conditionalOverlayCallback = new Func<HashedString, object, bool>(BuildingStatusItems.ShowInUtilityOverlay);
			this.PendingRepair = this.CreateStatusItem("PendingRepair", "BUILDING", "status_item_pending_repair", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.PendingRepair.resolveStringCallback = ((string str, object data) => str.Replace("{DamageInfo}", ((Repairable.SMInstance)data).master.GetComponent<BuildingHP>().GetDamageSourceInfo().ToString()));
			this.PendingRepair.conditionalOverlayCallback = ((HashedString mode, object data) => true);
			this.RequiresSkillPerk = this.CreateStatusItem("RequiresSkillPerk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RequiresSkillPerk.resolveStringCallback = delegate(string str, object data)
			{
				string id = (string)data;
				SkillPerk perk = Db.Get().SkillPerks.Get(id);
				List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(perk);
				List<string> list = new List<string>();
				foreach (Skill skill in skillsWithPerk)
				{
					if (!skill.deprecated)
					{
						list.Add(skill.Name);
					}
				}
				str = str.Replace("{Skills}", string.Join(", ", list.ToArray()));
				return str;
			};
			this.DigRequiresSkillPerk = this.CreateStatusItem("DigRequiresSkillPerk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.DigRequiresSkillPerk.resolveStringCallback = this.RequiresSkillPerk.resolveStringCallback;
			this.ColonyLacksRequiredSkillPerk = this.CreateStatusItem("ColonyLacksRequiredSkillPerk", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ColonyLacksRequiredSkillPerk.resolveStringCallback = delegate(string str, object data)
			{
				string id = (string)data;
				SkillPerk perk = Db.Get().SkillPerks.Get(id);
				List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(perk);
				List<string> list = new List<string>();
				foreach (Skill skill in skillsWithPerk)
				{
					if (!skill.deprecated)
					{
						list.Add(skill.Name);
					}
				}
				str = str.Replace("{Skills}", string.Join(", ", list.ToArray()));
				return str;
			};
			this.ColonyLacksRequiredSkillPerk.resolveTooltipCallback = delegate(string str, object data)
			{
				string id = (string)data;
				SkillPerk perk = Db.Get().SkillPerks.Get(id);
				List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(perk);
				List<string> list = new List<string>();
				foreach (Skill skill in skillsWithPerk)
				{
					if (!skill.deprecated)
					{
						list.Add(skill.Name);
					}
				}
				str = str.Replace("{Skills}", string.Join(", ", list.ToArray()));
				return str;
			};
			this.ClusterColonyLacksRequiredSkillPerk = this.CreateStatusItem("ClusterColonyLacksRequiredSkillPerk", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ClusterColonyLacksRequiredSkillPerk.resolveStringCallback = this.ColonyLacksRequiredSkillPerk.resolveStringCallback;
			this.ClusterColonyLacksRequiredSkillPerk.resolveTooltipCallback = this.ColonyLacksRequiredSkillPerk.resolveTooltipCallback;
			this.WorkRequiresMinion = this.CreateStatusItem("WorkRequiresMinion", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.SwitchStatusActive = this.CreateStatusItem("SwitchStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SwitchStatusInactive = this.CreateStatusItem("SwitchStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LogicSwitchStatusActive = this.CreateStatusItem("LogicSwitchStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LogicSwitchStatusInactive = this.CreateStatusItem("LogicSwitchStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LogicSensorStatusActive = this.CreateStatusItem("LogicSensorStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LogicSensorStatusInactive = this.CreateStatusItem("LogicSensorStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingFish = this.CreateStatusItem("PendingFish", "BUILDING", "status_item_pending_fish", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingSwitchToggle = this.CreateStatusItem("PendingSwitchToggle", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingUpgrade = this.CreateStatusItem("PendingUpgrade", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingWork = this.CreateStatusItem("PendingWork", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.PowerButtonOff = this.CreateStatusItem("PowerButtonOff", "BUILDING", "status_item_power_button_off", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.PressureOk = this.CreateStatusItem("PressureOk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Oxygen.ID, true, 129022);
			this.UnderPressure = this.CreateStatusItem("UnderPressure", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Oxygen.ID, true, 129022);
			this.UnderPressure.resolveTooltipCallback = delegate(string str, object data)
			{
				float mass = (float)data;
				return str.Replace("{TargetPressure}", GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.Unassigned = this.CreateStatusItem("Unassigned", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Rooms.ID, true, 129022);
			this.AssignedPublic = this.CreateStatusItem("AssignedPublic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Rooms.ID, true, 129022);
			this.UnderConstruction = this.CreateStatusItem("UnderConstruction", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.UnderConstructionNoWorker = this.CreateStatusItem("UnderConstructionNoWorker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Normal = this.CreateStatusItem("Normal", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ManualGeneratorChargingUp = this.CreateStatusItem("ManualGeneratorChargingUp", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.ManualGeneratorReleasingEnergy = this.CreateStatusItem("ManualGeneratorReleasingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.GeneratorOffline = this.CreateStatusItem("GeneratorOffline", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.Pipe = this.CreateStatusItem("Pipe", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.Pipe.resolveStringCallback = delegate(string str, object data)
			{
				Conduit conduit = (Conduit)data;
				int cell = Grid.PosToCell(conduit);
				ConduitFlow.ConduitContents contents = conduit.GetFlowManager().GetContents(cell);
				string text = BUILDING.STATUSITEMS.PIPECONTENTS.EMPTY;
				if (contents.mass > 0f)
				{
					Element element = ElementLoader.FindElementByHash(contents.element);
					text = string.Format(BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS, GameUtil.GetFormattedMass(contents.mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), element.name, GameUtil.GetFormattedTemperature(contents.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
					if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Disease.ID && contents.diseaseIdx != 255)
					{
						text += string.Format(BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS_WITH_DISEASE, GameUtil.GetFormattedDisease(contents.diseaseIdx, contents.diseaseCount, true));
					}
				}
				str = str.Replace("{Contents}", text);
				return str;
			};
			this.Conveyor = this.CreateStatusItem("Conveyor", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.SolidConveyor.ID, true, 129022);
			this.Conveyor.resolveStringCallback = delegate(string str, object data)
			{
				int cell = Grid.PosToCell((SolidConduit)data);
				SolidConduitFlow solidConduitFlow = Game.Instance.solidConduitFlow;
				SolidConduitFlow.ConduitContents contents = solidConduitFlow.GetContents(cell);
				string text = BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.EMPTY;
				if (contents.pickupableHandle.IsValid())
				{
					Pickupable pickupable = solidConduitFlow.GetPickupable(contents.pickupableHandle);
					if (pickupable)
					{
						PrimaryElement component = pickupable.GetComponent<PrimaryElement>();
						float mass = component.Mass;
						if (mass > 0f)
						{
							text = string.Format(BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.CONTENTS, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.GetProperName(), GameUtil.GetFormattedTemperature(component.Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
							if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Disease.ID && component.DiseaseIdx != 255)
							{
								text += string.Format(BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.CONTENTS_WITH_DISEASE, GameUtil.GetFormattedDisease(component.DiseaseIdx, component.DiseaseCount, true));
							}
						}
					}
				}
				str = str.Replace("{Contents}", text);
				return str;
			};
			this.FabricatorIdle = this.CreateStatusItem("FabricatorIdle", "BUILDING", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FabricatorEmpty = this.CreateStatusItem("FabricatorEmpty", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.FabricatorLacksHEP = this.CreateStatusItem("FabricatorLacksHEP", "BUILDING", "status_item_need_high_energy_particles", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.FabricatorLacksHEP.resolveStringCallback = delegate(string str, object data)
			{
				ComplexFabricator complexFabricator = (ComplexFabricator)data;
				if (complexFabricator != null)
				{
					int num = complexFabricator.HighestHEPQueued();
					HighEnergyParticleStorage component = complexFabricator.GetComponent<HighEnergyParticleStorage>();
					str = str.Replace("{HEPRequired}", num.ToString());
					str = str.Replace("{CurrentHEP}", component.Particles.ToString());
				}
				return str;
			};
			this.FossilMineIdle = this.CreateStatusItem("FossilIdle", "CODEX.STORY_TRAITS.FOSSILHUNT", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FossilMineEmpty = this.CreateStatusItem("FossilEmpty", "CODEX.STORY_TRAITS.FOSSILHUNT", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.FossilMinePendingWork = this.CreateStatusItem("FossilMinePendingWork", "CODEX.STORY_TRAITS.FOSSILHUNT", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.FossilEntombed = new StatusItem("FossilEntombed", "CODEX.STORY_TRAITS.FOSSILHUNT", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.Toilet = this.CreateStatusItem("Toilet", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Toilet.resolveStringCallback = delegate(string str, object data)
			{
				Toilet.StatesInstance statesInstance = (Toilet.StatesInstance)data;
				if (statesInstance != null)
				{
					str = str.Replace("{FlushesRemaining}", statesInstance.GetFlushesRemaining().ToString());
				}
				return str;
			};
			this.ToiletNeedsEmptying = this.CreateStatusItem("ToiletNeedsEmptying", "BUILDING", "status_item_toilet_needs_emptying", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DesalinatorNeedsEmptying = this.CreateStatusItem("DesalinatorNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MilkSeparatorNeedsEmptying = this.CreateStatusItem("MilkSeparatorNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Unusable = this.CreateStatusItem("Unusable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoResearchSelected = this.CreateStatusItem("NoResearchSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoResearchSelected.AddNotification(null, null, null);
			StatusItem noResearchSelected = this.NoResearchSelected;
			noResearchSelected.resolveTooltipCallback = (Func<string, object, string>)Delegate.Combine(noResearchSelected.resolveTooltipCallback, new Func<string, object, string>(delegate(string str, object data)
			{
				string newValue = GameInputMapping.FindEntry(global::Action.ManageResearch).mKeyCode.ToString();
				str = str.Replace("{RESEARCH_MENU_KEY}", newValue);
				return str;
			}));
			this.NoResearchSelected.notificationClickCallback = delegate(object d)
			{
				ManagementMenu.Instance.OpenResearch();
			};
			this.NoApplicableResearchSelected = this.CreateStatusItem("NoApplicableResearchSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoApplicableResearchSelected.AddNotification(null, null, null);
			this.NoApplicableAnalysisSelected = this.CreateStatusItem("NoApplicableAnalysisSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoApplicableAnalysisSelected.AddNotification(null, null, null);
			StatusItem noApplicableAnalysisSelected = this.NoApplicableAnalysisSelected;
			noApplicableAnalysisSelected.resolveTooltipCallback = (Func<string, object, string>)Delegate.Combine(noApplicableAnalysisSelected.resolveTooltipCallback, new Func<string, object, string>(delegate(string str, object data)
			{
				string newValue = GameInputMapping.FindEntry(global::Action.ManageStarmap).mKeyCode.ToString();
				str = str.Replace("{STARMAP_MENU_KEY}", newValue);
				return str;
			}));
			this.NoApplicableAnalysisSelected.notificationClickCallback = delegate(object d)
			{
				ManagementMenu.Instance.OpenStarmap();
			};
			this.NoResearchOrDestinationSelected = this.CreateStatusItem("NoResearchOrDestinationSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			StatusItem noResearchOrDestinationSelected = this.NoResearchOrDestinationSelected;
			noResearchOrDestinationSelected.resolveTooltipCallback = (Func<string, object, string>)Delegate.Combine(noResearchOrDestinationSelected.resolveTooltipCallback, new Func<string, object, string>(delegate(string str, object data)
			{
				string newValue = GameInputMapping.FindEntry(global::Action.ManageStarmap).mKeyCode.ToString();
				str = str.Replace("{STARMAP_MENU_KEY}", newValue);
				string newValue2 = GameInputMapping.FindEntry(global::Action.ManageResearch).mKeyCode.ToString();
				str = str.Replace("{RESEARCH_MENU_KEY}", newValue2);
				return str;
			}));
			this.NoResearchOrDestinationSelected.AddNotification(null, null, null);
			this.ValveRequest = this.CreateStatusItem("ValveRequest", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ValveRequest.resolveStringCallback = delegate(string str, object data)
			{
				Valve valve = (Valve)data;
				str = str.Replace("{QueuedMaxFlow}", GameUtil.GetFormattedMass(valve.QueuedMaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.EmittingLight = this.CreateStatusItem("EmittingLight", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingLight.resolveStringCallback = delegate(string str, object data)
			{
				string newValue = GameInputMapping.FindEntry(global::Action.Overlay5).mKeyCode.ToString();
				str = str.Replace("{LightGridOverlay}", newValue);
				return str;
			};
			this.RationBoxContents = this.CreateStatusItem("RationBoxContents", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RationBoxContents.resolveStringCallback = delegate(string str, object data)
			{
				RationBox rationBox = (RationBox)data;
				if (rationBox == null)
				{
					return str;
				}
				Storage component = rationBox.GetComponent<Storage>();
				if (component == null)
				{
					return str;
				}
				float num = 0f;
				foreach (GameObject gameObject in component.items)
				{
					Edible component2 = gameObject.GetComponent<Edible>();
					if (component2)
					{
						num += component2.Calories;
					}
				}
				str = str.Replace("{Stored}", GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true));
				return str;
			};
			this.EmittingElement = this.CreateStatusItem("EmittingElement", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingElement.resolveStringCallback = delegate(string str, object data)
			{
				IElementEmitter elementEmitter = (IElementEmitter)data;
				string newValue = ElementLoader.FindElementByHash(elementEmitter.Element).tag.ProperName();
				str = str.Replace("{ElementType}", newValue);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementEmitter.AverageEmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.EmittingOxygenAvg = this.CreateStatusItem("EmittingOxygenAvg", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingOxygenAvg.resolveStringCallback = delegate(string str, object data)
			{
				Sublimates sublimates = (Sublimates)data;
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(sublimates.AvgFlowRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.EmittingGasAvg = this.CreateStatusItem("EmittingGasAvg", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingGasAvg.resolveStringCallback = delegate(string str, object data)
			{
				Sublimates sublimates = (Sublimates)data;
				str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates.info.sublimatedElement).name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(sublimates.AvgFlowRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.EmittingBlockedHighPressure = this.CreateStatusItem("EmittingBlockedHighPressure", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingBlockedHighPressure.resolveStringCallback = delegate(string str, object data)
			{
				Sublimates sublimates = (Sublimates)data;
				str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates.info.sublimatedElement).name);
				return str;
			};
			this.EmittingBlockedLowTemperature = this.CreateStatusItem("EmittingBlockedLowTemperature", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingBlockedLowTemperature.resolveStringCallback = delegate(string str, object data)
			{
				Sublimates sublimates = (Sublimates)data;
				str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates.info.sublimatedElement).name);
				return str;
			};
			this.PumpingLiquidOrGas = this.CreateStatusItem("PumpingLiquidOrGas", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.PumpingLiquidOrGas.resolveStringCallback = delegate(string str, object data)
			{
				HandleVector<int>.Handle handle = (HandleVector<int>.Handle)data;
				float averageRate = Game.Instance.accumulators.GetAverageRate(handle);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(averageRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.PipeMayMelt = this.CreateStatusItem("PipeMayMelt", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoLiquidElementToPump = this.CreateStatusItem("NoLiquidElementToPump", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.NoGasElementToPump = this.CreateStatusItem("NoGasElementToPump", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
			this.NoFilterElementSelected = this.CreateStatusItem("NoFilterElementSelected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoLureElementSelected = this.CreateStatusItem("NoLureElementSelected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ElementConsumer = this.CreateStatusItem("ElementConsumer", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
			this.ElementConsumer.resolveStringCallback = delegate(string str, object data)
			{
				ElementConsumer elementConsumer = (ElementConsumer)data;
				string newValue = ElementLoader.FindElementByHash(elementConsumer.elementToConsume).tag.ProperName();
				str = str.Replace("{ElementTypes}", newValue);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementConsumer.AverageConsumeRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.ElementEmitterOutput = this.CreateStatusItem("ElementEmitterOutput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
			this.ElementEmitterOutput.resolveStringCallback = delegate(string str, object data)
			{
				ElementEmitter elementEmitter = (ElementEmitter)data;
				if (elementEmitter != null)
				{
					str = str.Replace("{ElementTypes}", elementEmitter.outputElement.Name);
					str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementEmitter.outputElement.massGenerationRate / elementEmitter.emissionFrequency, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				}
				return str;
			};
			this.AwaitingWaste = this.CreateStatusItem("AwaitingWaste", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
			this.AwaitingCompostFlip = this.CreateStatusItem("AwaitingCompostFlip", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
			this.JoulesAvailable = this.CreateStatusItem("JoulesAvailable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.JoulesAvailable.resolveStringCallback = delegate(string str, object data)
			{
				Battery battery = (Battery)data;
				str = str.Replace("{JoulesAvailable}", GameUtil.GetFormattedJoules(battery.JoulesAvailable, "F1", GameUtil.TimeSlice.None));
				str = str.Replace("{JoulesCapacity}", GameUtil.GetFormattedJoules(battery.Capacity, "F1", GameUtil.TimeSlice.None));
				return str;
			};
			this.Wattage = this.CreateStatusItem("Wattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.Wattage.resolveStringCallback = delegate(string str, object data)
			{
				Generator generator = (Generator)data;
				str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.SolarPanelWattage = this.CreateStatusItem("SolarPanelWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.SolarPanelWattage.resolveStringCallback = delegate(string str, object data)
			{
				SolarPanel solarPanel = (SolarPanel)data;
				str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(solarPanel.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.ModuleSolarPanelWattage = this.CreateStatusItem("ModuleSolarPanelWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.ModuleSolarPanelWattage.resolveStringCallback = delegate(string str, object data)
			{
				ModuleSolarPanel moduleSolarPanel = (ModuleSolarPanel)data;
				str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(moduleSolarPanel.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.SteamTurbineWattage = this.CreateStatusItem("SteamTurbineWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.SteamTurbineWattage.resolveStringCallback = delegate(string str, object data)
			{
				SteamTurbine steamTurbine = (SteamTurbine)data;
				str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(steamTurbine.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.Wattson = this.CreateStatusItem("Wattson", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Wattson.resolveStringCallback = delegate(string str, object data)
			{
				Telepad telepad = (Telepad)data;
				if (GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver())
				{
					str = BUILDING.STATUSITEMS.WATTSONGAMEOVER.NAME;
				}
				else if (telepad.GetComponent<Operational>().IsOperational)
				{
					str = str.Replace("{TimeRemaining}", GameUtil.GetFormattedCycles(telepad.GetTimeRemaining(), "F1", false));
				}
				else
				{
					str = str.Replace("{TimeRemaining}", BUILDING.STATUSITEMS.WATTSON.UNAVAILABLE);
				}
				return str;
			};
			this.FlushToilet = this.CreateStatusItem("FlushToilet", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FlushToilet.resolveStringCallback = delegate(string str, object data)
			{
				FlushToilet.SMInstance sminstance = (FlushToilet.SMInstance)data;
				return BUILDING.STATUSITEMS.FLUSHTOILET.NAME.Replace("{toilet}", sminstance.master.GetProperName());
			};
			this.FlushToilet.resolveTooltipCallback = ((string str, object Database) => BUILDING.STATUSITEMS.FLUSHTOILET.TOOLTIP);
			this.FlushToiletInUse = this.CreateStatusItem("FlushToiletInUse", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FlushToiletInUse.resolveStringCallback = delegate(string str, object data)
			{
				FlushToilet.SMInstance sminstance = (FlushToilet.SMInstance)data;
				return BUILDING.STATUSITEMS.FLUSHTOILETINUSE.NAME.Replace("{toilet}", sminstance.master.GetProperName());
			};
			this.FlushToiletInUse.resolveTooltipCallback = ((string str, object Database) => BUILDING.STATUSITEMS.FLUSHTOILETINUSE.TOOLTIP);
			this.WireNominal = this.CreateStatusItem("WireNominal", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.WireConnected = this.CreateStatusItem("WireConnected", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.WireDisconnected = this.CreateStatusItem("WireDisconnected", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.Overheated = this.CreateStatusItem("Overheated", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.Overloaded = this.CreateStatusItem("Overloaded", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.LogicOverloaded = this.CreateStatusItem("LogicOverloaded", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.Cooling = this.CreateStatusItem("Cooling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			Func<string, object, string> resolveStringCallback2 = delegate(string str, object data)
			{
				AirConditioner airConditioner = (AirConditioner)data;
				return string.Format(str, GameUtil.GetFormattedTemperature(airConditioner.lastGasTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.CoolingStalledColdGas = this.CreateStatusItem("CoolingStalledColdGas", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.CoolingStalledColdGas.resolveStringCallback = resolveStringCallback2;
			this.CoolingStalledColdLiquid = this.CreateStatusItem("CoolingStalledColdLiquid", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.CoolingStalledColdLiquid.resolveStringCallback = resolveStringCallback2;
			Func<string, object, string> resolveStringCallback3 = delegate(string str, object data)
			{
				AirConditioner airConditioner = (AirConditioner)data;
				return string.Format(str, GameUtil.GetFormattedTemperature(airConditioner.lastEnvTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(airConditioner.lastGasTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(airConditioner.maxEnvironmentDelta, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
			};
			this.CoolingStalledHotEnv = this.CreateStatusItem("CoolingStalledHotEnv", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.CoolingStalledHotEnv.resolveStringCallback = resolveStringCallback3;
			this.CoolingStalledHotLiquid = this.CreateStatusItem("CoolingStalledHotLiquid", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.CoolingStalledHotLiquid.resolveStringCallback = resolveStringCallback3;
			this.MissingRequirements = this.CreateStatusItem("MissingRequirements", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.GettingReady = this.CreateStatusItem("GettingReady", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Working = this.CreateStatusItem("Working", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NeedsValidRegion = this.CreateStatusItem("NeedsValidRegion", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NeedSeed = this.CreateStatusItem("NeedSeed", "BUILDING", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.AwaitingSeedDelivery = this.CreateStatusItem("AwaitingSeedDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.AwaitingBaitDelivery = this.CreateStatusItem("AwaitingBaitDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NoAvailableSeed = this.CreateStatusItem("NoAvailableSeed", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NeedEgg = this.CreateStatusItem("NeedEgg", "BUILDING", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.AwaitingEggDelivery = this.CreateStatusItem("AwaitingEggDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NoAvailableEgg = this.CreateStatusItem("NoAvailableEgg", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Grave = this.CreateStatusItem("Grave", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Grave.resolveStringCallback = delegate(string str, object data)
			{
				Grave.StatesInstance statesInstance = (Grave.StatesInstance)data;
				string text = str.Replace("{DeadDupe}", statesInstance.master.graveName);
				string[] strings = LocString.GetStrings(typeof(NAMEGEN.GRAVE.EPITAPHS));
				int num = statesInstance.master.epitaphIdx % strings.Length;
				return text.Replace("{Epitaph}", strings[num]);
			};
			this.GraveEmpty = this.CreateStatusItem("GraveEmpty", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CannotCoolFurther = this.CreateStatusItem("CannotCoolFurther", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CannotCoolFurther.resolveTooltipCallback = delegate(string str, object data)
			{
				float temp = (float)data;
				return str.Replace("{0}", GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.BuildingDisabled = this.CreateStatusItem("BuildingDisabled", "BUILDING", "status_item_building_disabled", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Expired = this.CreateStatusItem("Expired", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PumpingStation = this.CreateStatusItem("PumpingStation", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PumpingStation.resolveStringCallback = delegate(string str, object data)
			{
				LiquidPumpingStation liquidPumpingStation = (LiquidPumpingStation)data;
				if (liquidPumpingStation != null)
				{
					return liquidPumpingStation.ResolveString(str);
				}
				return str;
			};
			this.EmptyPumpingStation = this.CreateStatusItem("EmptyPumpingStation", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.WellPressurizing = this.CreateStatusItem("WellPressurizing", BUILDING.STATUSITEMS.WELL_PRESSURIZING.NAME, BUILDING.STATUSITEMS.WELL_PRESSURIZING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.WellPressurizing.resolveStringCallback = delegate(string str, object data)
			{
				OilWellCap.StatesInstance statesInstance = (OilWellCap.StatesInstance)data;
				if (statesInstance != null)
				{
					return string.Format(str, GameUtil.GetFormattedPercent(100f * statesInstance.GetPressurePercent(), GameUtil.TimeSlice.None));
				}
				return str;
			};
			this.WellOverpressure = this.CreateStatusItem("WellOverpressure", BUILDING.STATUSITEMS.WELL_OVERPRESSURE.NAME, BUILDING.STATUSITEMS.WELL_OVERPRESSURE.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.ReleasingPressure = this.CreateStatusItem("ReleasingPressure", BUILDING.STATUSITEMS.RELEASING_PRESSURE.NAME, BUILDING.STATUSITEMS.RELEASING_PRESSURE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.ReactorMeltdown = this.CreateStatusItem("ReactorMeltdown", BUILDING.STATUSITEMS.REACTORMELTDOWN.NAME, BUILDING.STATUSITEMS.REACTORMELTDOWN.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID, 129022);
			this.TooCold = this.CreateStatusItem("TooCold", BUILDING.STATUSITEMS.TOO_COLD.NAME, BUILDING.STATUSITEMS.TOO_COLD.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.IncubatorProgress = this.CreateStatusItem("IncubatorProgress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.IncubatorProgress.resolveStringCallback = delegate(string str, object data)
			{
				EggIncubator eggIncubator = (EggIncubator)data;
				str = str.Replace("{Percent}", GameUtil.GetFormattedPercent(eggIncubator.GetProgress() * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.HabitatNeedsEmptying = this.CreateStatusItem("HabitatNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DetectorScanning = this.CreateStatusItem("DetectorScanning", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.IncomingMeteors = this.CreateStatusItem("IncomingMeteors", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.HasGantry = this.CreateStatusItem("HasGantry", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MissingGantry = this.CreateStatusItem("MissingGantry", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DisembarkingDuplicant = this.CreateStatusItem("DisembarkingDuplicant", "BUILDING", "status_item_new_duplicants_available", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.RocketName = this.CreateStatusItem("RocketName", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RocketName.resolveStringCallback = delegate(string str, object data)
			{
				RocketModule rocketModule = (RocketModule)data;
				if (rocketModule != null)
				{
					return str.Replace("{0}", rocketModule.GetParentRocketName());
				}
				return str;
			};
			this.RocketName.resolveTooltipCallback = delegate(string str, object data)
			{
				RocketModule rocketModule = (RocketModule)data;
				if (rocketModule != null)
				{
					return str.Replace("{0}", rocketModule.GetParentRocketName());
				}
				return str;
			};
			this.LandedRocketLacksPassengerModule = this.CreateStatusItem("LandedRocketLacksPassengerModule", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.PathNotClear = new StatusItem("PATH_NOT_CLEAR", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.PathNotClear.resolveTooltipCallback = delegate(string str, object data)
			{
				ConditionFlightPathIsClear conditionFlightPathIsClear = (ConditionFlightPathIsClear)data;
				if (conditionFlightPathIsClear != null)
				{
					str = string.Format(str, conditionFlightPathIsClear.GetObstruction());
				}
				return str;
			};
			this.InvalidPortOverlap = this.CreateStatusItem("InvalidPortOverlap", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.InvalidPortOverlap.AddNotification(null, null, null);
			this.EmergencyPriority = this.CreateStatusItem("EmergencyPriority", BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME, BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.TOOLTIP, "status_item_doubleexclamation", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, 129022);
			this.EmergencyPriority.AddNotification(null, BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_NAME, BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_TOOLTIP);
			this.SkillPointsAvailable = this.CreateStatusItem("SkillPointsAvailable", BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.NAME, BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.TOOLTIP, "status_item_jobs", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.Baited = this.CreateStatusItem("Baited", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022);
			this.Baited.resolveStringCallback = delegate(string str, object data)
			{
				Element element = ElementLoader.FindElementByName(((CreatureBait.StatesInstance)data).master.baitElement.ToString());
				str = str.Replace("{0}", element.name);
				return str;
			};
			this.Baited.resolveTooltipCallback = delegate(string str, object data)
			{
				Element element = ElementLoader.FindElementByName(((CreatureBait.StatesInstance)data).master.baitElement.ToString());
				str = str.Replace("{0}", element.name);
				return str;
			};
			this.TanningLightSufficient = this.CreateStatusItem("TanningLightSufficient", BUILDING.STATUSITEMS.TANNINGLIGHTSUFFICIENT.NAME, BUILDING.STATUSITEMS.TANNINGLIGHTSUFFICIENT.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.TanningLightInsufficient = this.CreateStatusItem("TanningLightInsufficient", BUILDING.STATUSITEMS.TANNINGLIGHTINSUFFICIENT.NAME, BUILDING.STATUSITEMS.TANNINGLIGHTINSUFFICIENT.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.HotTubWaterTooCold = this.CreateStatusItem("HotTubWaterTooCold", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
			this.HotTubWaterTooCold.resolveStringCallback = delegate(string str, object data)
			{
				HotTub hotTub = (HotTub)data;
				str = str.Replace("{temperature}", GameUtil.GetFormattedTemperature(hotTub.minimumWaterTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.HotTubTooHot = this.CreateStatusItem("HotTubTooHot", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
			this.HotTubTooHot.resolveStringCallback = delegate(string str, object data)
			{
				HotTub hotTub = (HotTub)data;
				str = str.Replace("{temperature}", GameUtil.GetFormattedTemperature(hotTub.maxOperatingTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.HotTubFilling = this.CreateStatusItem("HotTubFilling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022);
			this.HotTubFilling.resolveStringCallback = delegate(string str, object data)
			{
				HotTub hotTub = (HotTub)data;
				str = str.Replace("{fullness}", GameUtil.GetFormattedPercent(hotTub.PercentFull, GameUtil.TimeSlice.None));
				return str;
			};
			this.WindTunnelIntake = this.CreateStatusItem("WindTunnelIntake", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.WarpPortalCharging = this.CreateStatusItem("WarpPortalCharging", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022);
			this.WarpPortalCharging.resolveStringCallback = delegate(string str, object data)
			{
				WarpPortal warpPortal = (WarpPortal)data;
				str = str.Replace("{charge}", GameUtil.GetFormattedPercent(100f * (((WarpPortal)data).rechargeProgress / 3000f), GameUtil.TimeSlice.None));
				return str;
			};
			this.WarpPortalCharging.resolveTooltipCallback = delegate(string str, object data)
			{
				WarpPortal warpPortal = (WarpPortal)data;
				str = str.Replace("{cycles}", string.Format("{0:0.0}", (3000f - ((WarpPortal)data).rechargeProgress) / 600f));
				return str;
			};
			this.WarpConduitPartnerDisabled = this.CreateStatusItem("WarpConduitPartnerDisabled", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.WarpConduitPartnerDisabled.resolveStringCallback = ((string str, object data) => str.Replace("{x}", data.ToString()));
			this.CollectingHEP = this.CreateStatusItem("CollectingHEP", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.Radiation.ID, false, 129022);
			this.CollectingHEP.resolveStringCallback = ((string str, object data) => str.Replace("{x}", ((HighEnergyParticleSpawner)data).PredictedPerCycleConsumptionRate.ToString()));
			this.InOrbit = this.CreateStatusItem("InOrbit", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.InOrbit.resolveStringCallback = delegate(string str, object data)
			{
				ClusterGridEntity clusterGridEntity = (ClusterGridEntity)data;
				return str.Replace("{Destination}", clusterGridEntity.Name);
			};
			this.InFlight = this.CreateStatusItem("InFlight", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.InFlight.resolveStringCallback = delegate(string str, object data)
			{
				ClusterTraveler clusterTraveler = (ClusterTraveler)data;
				ClusterDestinationSelector component = clusterTraveler.GetComponent<ClusterDestinationSelector>();
				RocketClusterDestinationSelector rocketClusterDestinationSelector = component as RocketClusterDestinationSelector;
				Sprite sprite;
				string newValue;
				string text;
				ClusterGrid.Instance.GetLocationDescription(component.GetDestination(), out sprite, out newValue, out text);
				if (rocketClusterDestinationSelector != null)
				{
					LaunchPad destinationPad = rocketClusterDestinationSelector.GetDestinationPad();
					string newValue2 = (destinationPad != null) ? destinationPad.GetProperName() : UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE.ToString();
					return str.Replace("{Destination_Asteroid}", newValue).Replace("{Destination_Pad}", newValue2).Replace("{ETA}", GameUtil.GetFormattedCycles(clusterTraveler.TravelETA(), "F1", false));
				}
				return str.Replace("{Destination_Asteroid}", newValue).Replace("{ETA}", GameUtil.GetFormattedCycles(clusterTraveler.TravelETA(), "F1", false));
			};
			this.DestinationOutOfRange = this.CreateStatusItem("DestinationOutOfRange", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DestinationOutOfRange.resolveStringCallback = delegate(string str, object data)
			{
				ClusterTraveler clusterTraveler = (ClusterTraveler)data;
				str = str.Replace("{Range}", GameUtil.GetFormattedRocketRange(clusterTraveler.GetComponent<CraftModuleInterface>().Range, GameUtil.TimeSlice.None, false));
				return str.Replace("{Distance}", clusterTraveler.RemainingTravelNodes().ToString() + " " + UI.CLUSTERMAP.TILES);
			};
			this.RocketStranded = this.CreateStatusItem("RocketStranded", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MissionControlAssistingRocket = this.CreateStatusItem("MissionControlAssistingRocket", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MissionControlAssistingRocket.resolveStringCallback = delegate(string str, object data)
			{
				Spacecraft spacecraft = data as Spacecraft;
				Clustercraft clustercraft = data as Clustercraft;
				return str.Replace("{0}", (spacecraft != null) ? spacecraft.rocketName : clustercraft.Name);
			};
			this.NoRocketsToMissionControlBoost = this.CreateStatusItem("NoRocketsToMissionControlBoost", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NoRocketsToMissionControlClusterBoost = this.CreateStatusItem("NoRocketsToMissionControlClusterBoost", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NoRocketsToMissionControlClusterBoost.resolveStringCallback = delegate(string str, object data)
			{
				if (str.Contains("{0}"))
				{
					str = str.Replace("{0}", 2.ToString());
				}
				return str;
			};
			this.MissionControlBoosted = this.CreateStatusItem("MissionControlBoosted", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MissionControlBoosted.resolveStringCallback = delegate(string str, object data)
			{
				Spacecraft spacecraft = data as Spacecraft;
				Clustercraft clustercraft = data as Clustercraft;
				str = str.Replace("{0}", GameUtil.GetFormattedPercent(20.000004f, GameUtil.TimeSlice.None));
				if (str.Contains("{1}"))
				{
					str = str.Replace("{1}", GameUtil.GetFormattedTime((spacecraft != null) ? spacecraft.controlStationBuffTimeRemaining : clustercraft.controlStationBuffTimeRemaining, "F0"));
				}
				return str;
			};
			this.TransitTubeEntranceWaxReady = this.CreateStatusItem("TransitTubeEntranceWaxReady", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TransitTubeEntranceWaxReady.resolveStringCallback = delegate(string str, object data)
			{
				TravelTubeEntrance travelTubeEntrance = data as TravelTubeEntrance;
				str = str.Replace("{0}", GameUtil.GetFormattedMass(travelTubeEntrance.waxPerLaunch, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				str = str.Replace("{1}", travelTubeEntrance.WaxLaunchesAvailable.ToString());
				return str;
			};
			this.SpecialCargoBayClusterCritterStored = this.CreateStatusItem("SpecialCargoBayClusterCritterStored", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpecialCargoBayClusterCritterStored.resolveStringCallback = delegate(string str, object data)
			{
				SpecialCargoBayClusterReceptacle specialCargoBayClusterReceptacle = data as SpecialCargoBayClusterReceptacle;
				if (specialCargoBayClusterReceptacle.Occupant != null)
				{
					str = str.Replace("{0}", specialCargoBayClusterReceptacle.Occupant.GetProperName());
				}
				return str;
			};
			this.RailgunpayloadNeedsEmptying = this.CreateStatusItem("RailgunpayloadNeedsEmptying", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.AwaitingEmptyBuilding = this.CreateStatusItem("AwaitingEmptyBuilding", "BUILDING", "action_empty_contents", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.DuplicantActivationRequired = this.CreateStatusItem("DuplicantActivationRequired", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RocketChecklistIncomplete = this.CreateStatusItem("RocketChecklistIncomplete", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.RocketCargoEmptying = this.CreateStatusItem("RocketCargoEmptying", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RocketCargoFilling = this.CreateStatusItem("RocketCargoFilling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RocketCargoFull = this.CreateStatusItem("RocketCargoFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FlightAllCargoFull = this.CreateStatusItem("FlightAllCargoFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FlightCargoRemaining = this.CreateStatusItem("FlightCargoRemaining", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FlightCargoRemaining.resolveStringCallback = delegate(string str, object data)
			{
				float mass = (float)data;
				return str.Replace("{0}", GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.PilotNeeded = this.CreateStatusItem("PilotNeeded", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PilotNeeded.resolveStringCallback = delegate(string str, object data)
			{
				RocketControlStation master = ((RocketControlStation.StatesInstance)data).master;
				return str.Replace("{timeRemaining}", GameUtil.GetFormattedTime(master.TimeRemaining, "F0"));
			};
			this.AutoPilotActive = this.CreateStatusItem("AutoPilotActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.InvalidMaskStationConsumptionState = this.CreateStatusItem("InvalidMaskStationConsumptionState", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ClusterTelescopeAllWorkComplete = this.CreateStatusItem("ClusterTelescopeAllWorkComplete", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RocketPlatformCloseToCeiling = this.CreateStatusItem("RocketPlatformCloseToCeiling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.RocketPlatformCloseToCeiling.resolveStringCallback = ((string str, object data) => str.Replace("{distance}", data.ToString()));
			this.ModuleGeneratorPowered = this.CreateStatusItem("ModuleGeneratorPowered", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.ModuleGeneratorPowered.resolveStringCallback = delegate(string str, object data)
			{
				Generator generator = (Generator)data;
				str = str.Replace("{ActiveWattage}", GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true));
				str = str.Replace("{MaxWattage}", GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.ModuleGeneratorNotPowered = this.CreateStatusItem("ModuleGeneratorNotPowered", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.ModuleGeneratorNotPowered.resolveStringCallback = delegate(string str, object data)
			{
				Generator generator = (Generator)data;
				str = str.Replace("{ActiveWattage}", GameUtil.GetFormattedWattage(0f, GameUtil.WattageFormatterUnit.Automatic, true));
				str = str.Replace("{MaxWattage}", GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.InOrbitRequired = this.CreateStatusItem("InOrbitRequired", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ReactorRefuelDisabled = this.CreateStatusItem("ReactorRefuelDisabled", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FridgeCooling = this.CreateStatusItem("FridgeCooling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FridgeCooling.resolveStringCallback = delegate(string str, object data)
			{
				RefrigeratorController.StatesInstance statesInstance = (RefrigeratorController.StatesInstance)data;
				str = str.Replace("{UsedPower}", GameUtil.GetFormattedWattage(statesInstance.GetNormalPower(), GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{MaxPower}", GameUtil.GetFormattedWattage(statesInstance.GetNormalPower(), GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.FridgeSteady = this.CreateStatusItem("FridgeSteady", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FridgeSteady.resolveStringCallback = delegate(string str, object data)
			{
				RefrigeratorController.StatesInstance statesInstance = (RefrigeratorController.StatesInstance)data;
				str = str.Replace("{UsedPower}", GameUtil.GetFormattedWattage(statesInstance.GetSaverPower(), GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{MaxPower}", GameUtil.GetFormattedWattage(statesInstance.GetNormalPower(), GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.TrapNeedsArming = this.CreateStatusItem("CREATURE_REUSABLE_TRAP.NEEDS_ARMING", "BUILDING", "status_item_bait", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.TrapArmed = this.CreateStatusItem("CREATURE_REUSABLE_TRAP.READY", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TrapHasCritter = this.CreateStatusItem("CREATURE_REUSABLE_TRAP.SPRUNG", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TrapHasCritter.resolveTooltipCallback = delegate(string str, object data)
			{
				string newValue = "";
				if (data != null)
				{
					newValue = ((GameObject)data).GetComponent<KPrefabID>().GetProperName();
				}
				str = str.Replace("{0}", newValue);
				return str;
			};
			this.RailGunCooldown = this.CreateStatusItem("RailGunCooldown", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RailGunCooldown.resolveStringCallback = delegate(string str, object data)
			{
				RailGun.StatesInstance statesInstance = (RailGun.StatesInstance)data;
				str = str.Replace("{timeleft}", GameUtil.GetFormattedTime(statesInstance.sm.cooldownTimer.Get(statesInstance), "F0"));
				return str;
			};
			this.RailGunCooldown.resolveTooltipCallback = delegate(string str, object data)
			{
				RailGun.StatesInstance statesInstance = (RailGun.StatesInstance)data;
				str = str.Replace("{x}", 6.ToString());
				return str;
			};
			this.NoSurfaceSight = new StatusItem("NOSURFACESIGHT", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.LimitValveLimitReached = this.CreateStatusItem("LimitValveLimitReached", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LimitValveLimitNotReached = this.CreateStatusItem("LimitValveLimitNotReached", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LimitValveLimitNotReached.resolveStringCallback = delegate(string str, object data)
			{
				LimitValve limitValve = (LimitValve)data;
				string arg;
				if (limitValve.displayUnitsInsteadOfMass)
				{
					arg = GameUtil.GetFormattedUnits(limitValve.RemainingCapacity, GameUtil.TimeSlice.None, true, LimitValveSideScreen.FLOAT_FORMAT);
				}
				else
				{
					arg = GameUtil.GetFormattedMass(limitValve.RemainingCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, LimitValveSideScreen.FLOAT_FORMAT);
				}
				return string.Format(BUILDING.STATUSITEMS.LIMITVALVELIMITNOTREACHED.NAME, arg);
			};
			this.LimitValveLimitNotReached.resolveTooltipCallback = ((string str, object data) => BUILDING.STATUSITEMS.LIMITVALVELIMITNOTREACHED.TOOLTIP);
			this.SpacePOIHarvesting = this.CreateStatusItem("SpacePOIHarvesting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpacePOIHarvesting.resolveStringCallback = delegate(string str, object data)
			{
				float mass = (float)data;
				return string.Format(BUILDING.STATUSITEMS.SPACEPOIHARVESTING.NAME, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.SpacePOIWasting = this.CreateStatusItem("SpacePOIWasting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.SpacePOIWasting.resolveStringCallback = delegate(string str, object data)
			{
				float mass = (float)data;
				return string.Format(BUILDING.STATUSITEMS.SPACEPOIWASTING.NAME, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.RocketRestrictionActive = new StatusItem("ROCKETRESTRICTIONACTIVE", "BUILDING", "status_item_rocket_restricted", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.RocketRestrictionInactive = new StatusItem("ROCKETRESTRICTIONINACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.NoRocketRestriction = new StatusItem("NOROCKETRESTRICTION", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.BroadcasterOutOfRange = new StatusItem("BROADCASTEROUTOFRANGE", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.LosingRadbolts = new StatusItem("LOSINGRADBOLTS", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.FabricatorAcceptsMutantSeeds = new StatusItem("FABRICATORACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.NoSpiceSelected = new StatusItem("SPICEGRINDERNOSPICE", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.GeoTunerNoGeyserSelected = new StatusItem("GEOTUNER_NEEDGEYSER", "BUILDING", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.GeoTunerResearchNeeded = new StatusItem("GEOTUNER_CHARGE_REQUIRED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.GeoTunerResearchInProgress = new StatusItem("GEOTUNER_CHARGING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.GeoTunerBroadcasting = new StatusItem("GEOTUNER_CHARGED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.GeoTunerBroadcasting.resolveStringCallback = delegate(string str, object data)
			{
				GeoTuner.Instance instance = (GeoTuner.Instance)data;
				str = str.Replace("{0}", ((float)Mathf.CeilToInt(instance.sm.expirationTimer.Get(instance) / instance.enhancementDuration * 100f)).ToString() + "%");
				return str;
			};
			this.GeoTunerBroadcasting.resolveTooltipCallback = delegate(string str, object data)
			{
				GeoTuner.Instance instance = (GeoTuner.Instance)data;
				float seconds = instance.sm.expirationTimer.Get(instance);
				float num = 100f / instance.enhancementDuration;
				str = str.Replace("{0}", GameUtil.GetFormattedTime(seconds, "F0"));
				str = str.Replace("{1}", "-" + num.ToString("0.00") + "%");
				return str;
			};
			this.GeoTunerGeyserStatus = new StatusItem("GEOTUNER_GEYSER_STATUS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.GeoTunerGeyserStatus.resolveStringCallback = delegate(string str, object data)
			{
				Geyser assignedGeyser = ((GeoTuner.Instance)data).GetAssignedGeyser();
				bool flag = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && assignedGeyser.smi.GetCurrentState().parent == assignedGeyser.smi.sm.erupt;
				bool flag2 = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() == assignedGeyser.smi.sm.dormant;
				if (!flag2)
				{
					bool flag3 = !flag;
				}
				return flag ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.NAME_ERUPTING : (flag2 ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.NAME_DORMANT : BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.NAME_IDLE);
			};
			this.GeoTunerGeyserStatus.resolveTooltipCallback = delegate(string str, object data)
			{
				Geyser assignedGeyser = ((GeoTuner.Instance)data).GetAssignedGeyser();
				if (assignedGeyser != null)
				{
					assignedGeyser.gameObject.GetProperName();
				}
				bool flag = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && assignedGeyser.smi.GetCurrentState().parent == assignedGeyser.smi.sm.erupt;
				bool flag2 = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() == assignedGeyser.smi.sm.dormant;
				if (!flag2)
				{
					bool flag3 = !flag;
				}
				return flag ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.TOOLTIP_ERUPTING : (flag2 ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.TOOLTIP_DORMANT : BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.TOOLTIP_IDLE);
			};
			this.GeyserGeotuned = new StatusItem("GEYSER_GEOTUNED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.GeyserGeotuned.resolveStringCallback = delegate(string str, object data)
			{
				Geyser geyser = (Geyser)data;
				int num = 0;
				int num2 = Components.GeoTuners.GetItems(geyser.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == geyser);
				for (int i = 0; i < geyser.modifications.Count; i++)
				{
					if (geyser.modifications[i].originID.Contains("GeoTuner"))
					{
						num++;
					}
				}
				str = str.Replace("{0}", num.ToString());
				str = str.Replace("{1}", num2.ToString());
				return str;
			};
			this.GeyserGeotuned.resolveTooltipCallback = delegate(string str, object data)
			{
				Geyser geyser = (Geyser)data;
				int num = 0;
				int num2 = Components.GeoTuners.GetItems(geyser.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == geyser);
				for (int i = 0; i < geyser.modifications.Count; i++)
				{
					if (geyser.modifications[i].originID.Contains("GeoTuner"))
					{
						num++;
					}
				}
				str = str.Replace("{0}", num.ToString());
				str = str.Replace("{1}", num2.ToString());
				return str;
			};
			this.SkyVisNone = new StatusItem("SkyVisNone", BUILDING.STATUSITEMS.SPACE_VISIBILITY_NONE.NAME, BUILDING.STATUSITEMS.SPACE_VISIBILITY_NONE.TOOLTIP, "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, 129022, true, new Func<string, object, string>(BuildingStatusItems.<CreateStatusItems>g__SkyVisResolveStringCallback|281_97));
			this.SkyVisLimited = new StatusItem("SkyVisLimited", BUILDING.STATUSITEMS.SPACE_VISIBILITY_REDUCED.NAME, BUILDING.STATUSITEMS.SPACE_VISIBILITY_REDUCED.TOOLTIP, "status_item_no_sky", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, false, new Func<string, object, string>(BuildingStatusItems.<CreateStatusItems>g__SkyVisResolveStringCallback|281_97));
			this.CreatureManipulatorWaiting = this.CreateStatusItem("CreatureManipulatorWaiting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CreatureManipulatorProgress = this.CreateStatusItem("CreatureManipulatorProgress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CreatureManipulatorProgress.resolveStringCallback = delegate(string str, object data)
			{
				GravitasCreatureManipulator.Instance instance = (GravitasCreatureManipulator.Instance)data;
				return string.Format(str, instance.ScannedSpecies.Count, instance.def.numSpeciesToUnlockMorphMode);
			};
			this.CreatureManipulatorProgress.resolveTooltipCallback = delegate(string str, object data)
			{
				GravitasCreatureManipulator.Instance instance = (GravitasCreatureManipulator.Instance)data;
				if (instance.ScannedSpecies.Count == 0)
				{
					str = str + "\n • " + BUILDING.STATUSITEMS.CREATUREMANIPULATORPROGRESS.NO_DATA;
				}
				else
				{
					foreach (Tag tag in instance.ScannedSpecies)
					{
						str = str + "\n • " + Strings.Get("STRINGS.CREATURES.FAMILY_PLURAL." + tag.ToString().ToUpper());
					}
				}
				return str;
			};
			this.CreatureManipulatorMorphModeLocked = this.CreateStatusItem("CreatureManipulatorMorphModeLocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CreatureManipulatorMorphMode = this.CreateStatusItem("CreatureManipulatorMorphMode", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CreatureManipulatorWorking = this.CreateStatusItem("CreatureManipulatorWorking", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MegaBrainTankActivationProgress = this.CreateStatusItem("MegaBrainTankActivationProgress", BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.PROGRESSIONRATE.NAME, BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.PROGRESSIONRATE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.MegaBrainNotEnoughOxygen = this.CreateStatusItem("MegaBrainNotEnoughOxygen", "BUILDING", "status_item_suit_locker_no_oxygen", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MegaBrainTankActivationProgress.resolveStringCallback = delegate(string str, object data)
			{
				MegaBrainTank.StatesInstance statesInstance = (MegaBrainTank.StatesInstance)data;
				return str.Replace("{ActivationProgress}", string.Format("{0}/{1}", statesInstance.ActivationProgress, 25));
			};
			this.MegaBrainTankDreamAnalysis = this.CreateStatusItem("MegaBrainTankDreamAnalysis", BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.DREAMANALYSIS.NAME, BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.DREAMANALYSIS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.MegaBrainTankDreamAnalysis.resolveStringCallback = delegate(string str, object data)
			{
				MegaBrainTank.StatesInstance statesInstance = (MegaBrainTank.StatesInstance)data;
				return str.Replace("{TimeToComplete}", statesInstance.TimeTilDigested.ToString());
			};
			this.MegaBrainTankComplete = this.CreateStatusItem("MegaBrainTankComplete", BUILDING.STATUSITEMS.MEGABRAINTANK.COMPLETE.NAME, BUILDING.STATUSITEMS.MEGABRAINTANK.COMPLETE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 129022);
			this.FossilHuntExcavationOrdered = this.CreateStatusItem("FossilHuntExcavationOrdered", BUILDING.STATUSITEMS.FOSSILHUNT.PENDING_EXCAVATION.NAME, BUILDING.STATUSITEMS.FOSSILHUNT.PENDING_EXCAVATION.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 129022);
			this.FossilHuntExcavationInProgress = this.CreateStatusItem("FossilHuntExcavationInProgress", BUILDING.STATUSITEMS.FOSSILHUNT.EXCAVATING.NAME, BUILDING.STATUSITEMS.FOSSILHUNT.EXCAVATING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 129022);
			this.ComplexFabricatorCooking = this.CreateStatusItem("COMPLEXFABRICATOR.COOKING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ComplexFabricatorCooking.resolveStringCallback = delegate(string str, object data)
			{
				ComplexFabricator complexFabricator = data as ComplexFabricator;
				if (complexFabricator != null && complexFabricator.CurrentWorkingOrder != null)
				{
					str = str.Replace("{Item}", complexFabricator.CurrentWorkingOrder.FirstResult.ProperName());
				}
				return str;
			};
			this.ComplexFabricatorProducing = this.CreateStatusItem("COMPLEXFABRICATOR.PRODUCING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ComplexFabricatorProducing.resolveStringCallback = delegate(string str, object data)
			{
				ComplexFabricator complexFabricator = data as ComplexFabricator;
				if (complexFabricator != null)
				{
					if (complexFabricator.CurrentWorkingOrder != null)
					{
						string newValue = complexFabricator.CurrentWorkingOrder.results[0].facadeID.IsNullOrWhiteSpace() ? complexFabricator.CurrentWorkingOrder.FirstResult.ProperName() : complexFabricator.CurrentWorkingOrder.results[0].facadeID.ProperName();
						str = str.Replace("{Item}", newValue);
					}
					return str;
				}
				TinkerStation tinkerStation = data as TinkerStation;
				if (tinkerStation != null)
				{
					str = str.Replace("{Item}", tinkerStation.outputPrefab.ProperName());
				}
				return str;
			};
			this.ComplexFabricatorResearching = this.CreateStatusItem("COMPLEXFABRICATOR.RESEARCHING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ComplexFabricatorResearching.resolveStringCallback = delegate(string str, object data)
			{
				if (data is IResearchCenter)
				{
					TechInstance activeResearch = Research.Instance.GetActiveResearch();
					if (activeResearch != null)
					{
						str = str.Replace("{Item}", activeResearch.tech.Name);
						return str;
					}
				}
				str = str.Replace("{Item}", (data as GameObject).GetProperName());
				return str;
			};
			this.ArtifactAnalysisAnalyzing = this.CreateStatusItem("COMPLEXFABRICATOR.ANALYZYING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ArtifactAnalysisAnalyzing.resolveStringCallback = delegate(string str, object data)
			{
				if (data as GameObject != null)
				{
					str = str.Replace("{Item}", (data as GameObject).GetProperName());
				}
				return str;
			};
			this.ComplexFabricatorTraining = this.CreateStatusItem("COMPLEXFABRICATOR.UNTRAINING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ComplexFabricatorTraining.resolveStringCallback = delegate(string str, object data)
			{
				ResetSkillsStation resetSkillsStation = data as ResetSkillsStation;
				if (resetSkillsStation != null && resetSkillsStation.assignable.assignee != null)
				{
					str = str.Replace("{Duplicant}", resetSkillsStation.assignable.assignee.GetProperName());
				}
				return str;
			};
			this.TelescopeWorking = this.CreateStatusItem("COMPLEXFABRICATOR.TELESCOPE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ClusterTelescopeMeteorWorking = this.CreateStatusItem("COMPLEXFABRICATOR.CLUSTERTELESCOPEMETEOR", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
		}

		// Token: 0x0600696A RID: 26986 RVA: 0x00283A10 File Offset: 0x00281C10
		private static bool ShowInUtilityOverlay(HashedString mode, object data)
		{
			Transform transform = (Transform)data;
			bool result = false;
			if (mode == OverlayModes.GasConduits.ID)
			{
				Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayScreen.GasVentIDs.Contains(prefabTag);
			}
			else if (mode == OverlayModes.LiquidConduits.ID)
			{
				Tag prefabTag2 = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayScreen.LiquidVentIDs.Contains(prefabTag2);
			}
			else if (mode == OverlayModes.Power.ID)
			{
				Tag prefabTag3 = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayScreen.WireIDs.Contains(prefabTag3);
			}
			else if (mode == OverlayModes.Logic.ID)
			{
				Tag prefabTag4 = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayModes.Logic.HighlightItemIDs.Contains(prefabTag4);
			}
			else if (mode == OverlayModes.SolidConveyor.ID)
			{
				Tag prefabTag5 = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayScreen.SolidConveyorIDs.Contains(prefabTag5);
			}
			else if (mode == OverlayModes.Radiation.ID)
			{
				Tag prefabTag6 = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayScreen.RadiationIDs.Contains(prefabTag6);
			}
			return result;
		}

		// Token: 0x0600696B RID: 26987 RVA: 0x00283B20 File Offset: 0x00281D20
		[CompilerGenerated]
		internal static string <CreateStatusItems>g__SkyVisResolveStringCallback|281_97(string str, object data)
		{
			BuildingStatusItems.ISkyVisInfo skyVisInfo = (BuildingStatusItems.ISkyVisInfo)data;
			return str.Replace("{VISIBILITY}", GameUtil.GetFormattedPercent(skyVisInfo.GetPercentVisible01() * 100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x0400490F RID: 18703
		public StatusItem MissingRequirements;

		// Token: 0x04004910 RID: 18704
		public StatusItem GettingReady;

		// Token: 0x04004911 RID: 18705
		public StatusItem Working;

		// Token: 0x04004912 RID: 18706
		public MaterialsStatusItem MaterialsUnavailable;

		// Token: 0x04004913 RID: 18707
		public MaterialsStatusItem MaterialsUnavailableForRefill;

		// Token: 0x04004914 RID: 18708
		public StatusItem AngerDamage;

		// Token: 0x04004915 RID: 18709
		public StatusItem ClinicOutsideHospital;

		// Token: 0x04004916 RID: 18710
		public StatusItem DigUnreachable;

		// Token: 0x04004917 RID: 18711
		public StatusItem MopUnreachable;

		// Token: 0x04004918 RID: 18712
		public StatusItem StorageUnreachable;

		// Token: 0x04004919 RID: 18713
		public StatusItem PassengerModuleUnreachable;

		// Token: 0x0400491A RID: 18714
		public StatusItem ConstructableDigUnreachable;

		// Token: 0x0400491B RID: 18715
		public StatusItem ConstructionUnreachable;

		// Token: 0x0400491C RID: 18716
		public StatusItem DispenseRequested;

		// Token: 0x0400491D RID: 18717
		public StatusItem NewDuplicantsAvailable;

		// Token: 0x0400491E RID: 18718
		public StatusItem NeedPlant;

		// Token: 0x0400491F RID: 18719
		public StatusItem NeedPower;

		// Token: 0x04004920 RID: 18720
		public StatusItem NotEnoughPower;

		// Token: 0x04004921 RID: 18721
		public StatusItem PowerLoopDetected;

		// Token: 0x04004922 RID: 18722
		public StatusItem NeedLiquidIn;

		// Token: 0x04004923 RID: 18723
		public StatusItem NeedGasIn;

		// Token: 0x04004924 RID: 18724
		public StatusItem NeedResourceMass;

		// Token: 0x04004925 RID: 18725
		public StatusItem NeedSolidIn;

		// Token: 0x04004926 RID: 18726
		public StatusItem NeedLiquidOut;

		// Token: 0x04004927 RID: 18727
		public StatusItem NeedGasOut;

		// Token: 0x04004928 RID: 18728
		public StatusItem NeedSolidOut;

		// Token: 0x04004929 RID: 18729
		public StatusItem InvalidBuildingLocation;

		// Token: 0x0400492A RID: 18730
		public StatusItem PendingDeconstruction;

		// Token: 0x0400492B RID: 18731
		public StatusItem PendingDemolition;

		// Token: 0x0400492C RID: 18732
		public StatusItem PendingSwitchToggle;

		// Token: 0x0400492D RID: 18733
		public StatusItem GasVentObstructed;

		// Token: 0x0400492E RID: 18734
		public StatusItem LiquidVentObstructed;

		// Token: 0x0400492F RID: 18735
		public StatusItem LiquidPipeEmpty;

		// Token: 0x04004930 RID: 18736
		public StatusItem LiquidPipeObstructed;

		// Token: 0x04004931 RID: 18737
		public StatusItem GasPipeEmpty;

		// Token: 0x04004932 RID: 18738
		public StatusItem GasPipeObstructed;

		// Token: 0x04004933 RID: 18739
		public StatusItem SolidPipeObstructed;

		// Token: 0x04004934 RID: 18740
		public StatusItem PartiallyDamaged;

		// Token: 0x04004935 RID: 18741
		public StatusItem Broken;

		// Token: 0x04004936 RID: 18742
		public StatusItem PendingRepair;

		// Token: 0x04004937 RID: 18743
		public StatusItem PendingUpgrade;

		// Token: 0x04004938 RID: 18744
		public StatusItem RequiresSkillPerk;

		// Token: 0x04004939 RID: 18745
		public StatusItem DigRequiresSkillPerk;

		// Token: 0x0400493A RID: 18746
		public StatusItem ColonyLacksRequiredSkillPerk;

		// Token: 0x0400493B RID: 18747
		public StatusItem ClusterColonyLacksRequiredSkillPerk;

		// Token: 0x0400493C RID: 18748
		public StatusItem WorkRequiresMinion;

		// Token: 0x0400493D RID: 18749
		public StatusItem PendingWork;

		// Token: 0x0400493E RID: 18750
		public StatusItem Flooded;

		// Token: 0x0400493F RID: 18751
		public StatusItem PowerButtonOff;

		// Token: 0x04004940 RID: 18752
		public StatusItem SwitchStatusActive;

		// Token: 0x04004941 RID: 18753
		public StatusItem SwitchStatusInactive;

		// Token: 0x04004942 RID: 18754
		public StatusItem LogicSwitchStatusActive;

		// Token: 0x04004943 RID: 18755
		public StatusItem LogicSwitchStatusInactive;

		// Token: 0x04004944 RID: 18756
		public StatusItem LogicSensorStatusActive;

		// Token: 0x04004945 RID: 18757
		public StatusItem LogicSensorStatusInactive;

		// Token: 0x04004946 RID: 18758
		public StatusItem ChangeDoorControlState;

		// Token: 0x04004947 RID: 18759
		public StatusItem CurrentDoorControlState;

		// Token: 0x04004948 RID: 18760
		public StatusItem Entombed;

		// Token: 0x04004949 RID: 18761
		public MaterialsStatusItem WaitingForMaterials;

		// Token: 0x0400494A RID: 18762
		public StatusItem WaitingForHighEnergyParticles;

		// Token: 0x0400494B RID: 18763
		public StatusItem WaitingForRepairMaterials;

		// Token: 0x0400494C RID: 18764
		public StatusItem MissingFoundation;

		// Token: 0x0400494D RID: 18765
		public StatusItem NeutroniumUnminable;

		// Token: 0x0400494E RID: 18766
		public StatusItem NoStorageFilterSet;

		// Token: 0x0400494F RID: 18767
		public StatusItem PendingFish;

		// Token: 0x04004950 RID: 18768
		public StatusItem NoFishableWaterBelow;

		// Token: 0x04004951 RID: 18769
		public StatusItem GasVentOverPressure;

		// Token: 0x04004952 RID: 18770
		public StatusItem LiquidVentOverPressure;

		// Token: 0x04004953 RID: 18771
		public StatusItem NoWireConnected;

		// Token: 0x04004954 RID: 18772
		public StatusItem NoLogicWireConnected;

		// Token: 0x04004955 RID: 18773
		public StatusItem NoTubeConnected;

		// Token: 0x04004956 RID: 18774
		public StatusItem NoTubeExits;

		// Token: 0x04004957 RID: 18775
		public StatusItem StoredCharge;

		// Token: 0x04004958 RID: 18776
		public StatusItem NoPowerConsumers;

		// Token: 0x04004959 RID: 18777
		public StatusItem PressureOk;

		// Token: 0x0400495A RID: 18778
		public StatusItem UnderPressure;

		// Token: 0x0400495B RID: 18779
		public StatusItem AssignedTo;

		// Token: 0x0400495C RID: 18780
		public StatusItem Unassigned;

		// Token: 0x0400495D RID: 18781
		public StatusItem AssignedPublic;

		// Token: 0x0400495E RID: 18782
		public StatusItem AssignedToRoom;

		// Token: 0x0400495F RID: 18783
		public StatusItem RationBoxContents;

		// Token: 0x04004960 RID: 18784
		public StatusItem ConduitBlocked;

		// Token: 0x04004961 RID: 18785
		public StatusItem OutputTileBlocked;

		// Token: 0x04004962 RID: 18786
		public StatusItem OutputPipeFull;

		// Token: 0x04004963 RID: 18787
		public StatusItem ConduitBlockedMultiples;

		// Token: 0x04004964 RID: 18788
		public StatusItem SolidConduitBlockedMultiples;

		// Token: 0x04004965 RID: 18789
		public StatusItem MeltingDown;

		// Token: 0x04004966 RID: 18790
		public StatusItem DeadReactorCoolingOff;

		// Token: 0x04004967 RID: 18791
		public StatusItem UnderConstruction;

		// Token: 0x04004968 RID: 18792
		public StatusItem UnderConstructionNoWorker;

		// Token: 0x04004969 RID: 18793
		public StatusItem Normal;

		// Token: 0x0400496A RID: 18794
		public StatusItem ManualGeneratorChargingUp;

		// Token: 0x0400496B RID: 18795
		public StatusItem ManualGeneratorReleasingEnergy;

		// Token: 0x0400496C RID: 18796
		public StatusItem GeneratorOffline;

		// Token: 0x0400496D RID: 18797
		public StatusItem Pipe;

		// Token: 0x0400496E RID: 18798
		public StatusItem Conveyor;

		// Token: 0x0400496F RID: 18799
		public StatusItem FabricatorIdle;

		// Token: 0x04004970 RID: 18800
		public StatusItem FabricatorEmpty;

		// Token: 0x04004971 RID: 18801
		public StatusItem FossilMineIdle;

		// Token: 0x04004972 RID: 18802
		public StatusItem FossilMineEmpty;

		// Token: 0x04004973 RID: 18803
		public StatusItem FossilEntombed;

		// Token: 0x04004974 RID: 18804
		public StatusItem FossilMinePendingWork;

		// Token: 0x04004975 RID: 18805
		public StatusItem FabricatorLacksHEP;

		// Token: 0x04004976 RID: 18806
		public StatusItem FlushToilet;

		// Token: 0x04004977 RID: 18807
		public StatusItem FlushToiletInUse;

		// Token: 0x04004978 RID: 18808
		public StatusItem Toilet;

		// Token: 0x04004979 RID: 18809
		public StatusItem ToiletNeedsEmptying;

		// Token: 0x0400497A RID: 18810
		public StatusItem DesalinatorNeedsEmptying;

		// Token: 0x0400497B RID: 18811
		public StatusItem MilkSeparatorNeedsEmptying;

		// Token: 0x0400497C RID: 18812
		public StatusItem Unusable;

		// Token: 0x0400497D RID: 18813
		public StatusItem NoResearchSelected;

		// Token: 0x0400497E RID: 18814
		public StatusItem NoApplicableResearchSelected;

		// Token: 0x0400497F RID: 18815
		public StatusItem NoApplicableAnalysisSelected;

		// Token: 0x04004980 RID: 18816
		public StatusItem NoResearchOrDestinationSelected;

		// Token: 0x04004981 RID: 18817
		public StatusItem Researching;

		// Token: 0x04004982 RID: 18818
		public StatusItem ValveRequest;

		// Token: 0x04004983 RID: 18819
		public StatusItem EmittingLight;

		// Token: 0x04004984 RID: 18820
		public StatusItem EmittingElement;

		// Token: 0x04004985 RID: 18821
		public StatusItem EmittingOxygenAvg;

		// Token: 0x04004986 RID: 18822
		public StatusItem EmittingGasAvg;

		// Token: 0x04004987 RID: 18823
		public StatusItem EmittingBlockedHighPressure;

		// Token: 0x04004988 RID: 18824
		public StatusItem EmittingBlockedLowTemperature;

		// Token: 0x04004989 RID: 18825
		public StatusItem PumpingLiquidOrGas;

		// Token: 0x0400498A RID: 18826
		public StatusItem NoLiquidElementToPump;

		// Token: 0x0400498B RID: 18827
		public StatusItem NoGasElementToPump;

		// Token: 0x0400498C RID: 18828
		public StatusItem PipeFull;

		// Token: 0x0400498D RID: 18829
		public StatusItem PipeMayMelt;

		// Token: 0x0400498E RID: 18830
		public StatusItem ElementConsumer;

		// Token: 0x0400498F RID: 18831
		public StatusItem ElementEmitterOutput;

		// Token: 0x04004990 RID: 18832
		public StatusItem AwaitingWaste;

		// Token: 0x04004991 RID: 18833
		public StatusItem AwaitingCompostFlip;

		// Token: 0x04004992 RID: 18834
		public StatusItem JoulesAvailable;

		// Token: 0x04004993 RID: 18835
		public StatusItem Wattage;

		// Token: 0x04004994 RID: 18836
		public StatusItem SolarPanelWattage;

		// Token: 0x04004995 RID: 18837
		public StatusItem ModuleSolarPanelWattage;

		// Token: 0x04004996 RID: 18838
		public StatusItem SteamTurbineWattage;

		// Token: 0x04004997 RID: 18839
		public StatusItem Wattson;

		// Token: 0x04004998 RID: 18840
		public StatusItem WireConnected;

		// Token: 0x04004999 RID: 18841
		public StatusItem WireNominal;

		// Token: 0x0400499A RID: 18842
		public StatusItem WireDisconnected;

		// Token: 0x0400499B RID: 18843
		public StatusItem Cooling;

		// Token: 0x0400499C RID: 18844
		public StatusItem CoolingStalledHotEnv;

		// Token: 0x0400499D RID: 18845
		public StatusItem CoolingStalledColdGas;

		// Token: 0x0400499E RID: 18846
		public StatusItem CoolingStalledHotLiquid;

		// Token: 0x0400499F RID: 18847
		public StatusItem CoolingStalledColdLiquid;

		// Token: 0x040049A0 RID: 18848
		public StatusItem CannotCoolFurther;

		// Token: 0x040049A1 RID: 18849
		public StatusItem NeedsValidRegion;

		// Token: 0x040049A2 RID: 18850
		public StatusItem NeedSeed;

		// Token: 0x040049A3 RID: 18851
		public StatusItem AwaitingSeedDelivery;

		// Token: 0x040049A4 RID: 18852
		public StatusItem AwaitingBaitDelivery;

		// Token: 0x040049A5 RID: 18853
		public StatusItem NoAvailableSeed;

		// Token: 0x040049A6 RID: 18854
		public StatusItem NeedEgg;

		// Token: 0x040049A7 RID: 18855
		public StatusItem AwaitingEggDelivery;

		// Token: 0x040049A8 RID: 18856
		public StatusItem NoAvailableEgg;

		// Token: 0x040049A9 RID: 18857
		public StatusItem Grave;

		// Token: 0x040049AA RID: 18858
		public StatusItem GraveEmpty;

		// Token: 0x040049AB RID: 18859
		public StatusItem NoFilterElementSelected;

		// Token: 0x040049AC RID: 18860
		public StatusItem NoLureElementSelected;

		// Token: 0x040049AD RID: 18861
		public StatusItem BuildingDisabled;

		// Token: 0x040049AE RID: 18862
		public StatusItem Overheated;

		// Token: 0x040049AF RID: 18863
		public StatusItem Overloaded;

		// Token: 0x040049B0 RID: 18864
		public StatusItem LogicOverloaded;

		// Token: 0x040049B1 RID: 18865
		public StatusItem Expired;

		// Token: 0x040049B2 RID: 18866
		public StatusItem PumpingStation;

		// Token: 0x040049B3 RID: 18867
		public StatusItem EmptyPumpingStation;

		// Token: 0x040049B4 RID: 18868
		public StatusItem GeneShuffleCompleted;

		// Token: 0x040049B5 RID: 18869
		public StatusItem GeneticAnalysisCompleted;

		// Token: 0x040049B6 RID: 18870
		public StatusItem DirectionControl;

		// Token: 0x040049B7 RID: 18871
		public StatusItem WellPressurizing;

		// Token: 0x040049B8 RID: 18872
		public StatusItem WellOverpressure;

		// Token: 0x040049B9 RID: 18873
		public StatusItem ReleasingPressure;

		// Token: 0x040049BA RID: 18874
		public StatusItem ReactorMeltdown;

		// Token: 0x040049BB RID: 18875
		public StatusItem NoSuitMarker;

		// Token: 0x040049BC RID: 18876
		public StatusItem SuitMarkerWrongSide;

		// Token: 0x040049BD RID: 18877
		public StatusItem SuitMarkerTraversalAnytime;

		// Token: 0x040049BE RID: 18878
		public StatusItem SuitMarkerTraversalOnlyWhenRoomAvailable;

		// Token: 0x040049BF RID: 18879
		public StatusItem TooCold;

		// Token: 0x040049C0 RID: 18880
		public StatusItem NotInAnyRoom;

		// Token: 0x040049C1 RID: 18881
		public StatusItem NotInRequiredRoom;

		// Token: 0x040049C2 RID: 18882
		public StatusItem NotInRecommendedRoom;

		// Token: 0x040049C3 RID: 18883
		public StatusItem IncubatorProgress;

		// Token: 0x040049C4 RID: 18884
		public StatusItem HabitatNeedsEmptying;

		// Token: 0x040049C5 RID: 18885
		public StatusItem DetectorScanning;

		// Token: 0x040049C6 RID: 18886
		public StatusItem IncomingMeteors;

		// Token: 0x040049C7 RID: 18887
		public StatusItem HasGantry;

		// Token: 0x040049C8 RID: 18888
		public StatusItem MissingGantry;

		// Token: 0x040049C9 RID: 18889
		public StatusItem DisembarkingDuplicant;

		// Token: 0x040049CA RID: 18890
		public StatusItem RocketName;

		// Token: 0x040049CB RID: 18891
		public StatusItem PathNotClear;

		// Token: 0x040049CC RID: 18892
		public StatusItem InvalidPortOverlap;

		// Token: 0x040049CD RID: 18893
		public StatusItem EmergencyPriority;

		// Token: 0x040049CE RID: 18894
		public StatusItem SkillPointsAvailable;

		// Token: 0x040049CF RID: 18895
		public StatusItem Baited;

		// Token: 0x040049D0 RID: 18896
		public StatusItem NoCoolant;

		// Token: 0x040049D1 RID: 18897
		public StatusItem TanningLightSufficient;

		// Token: 0x040049D2 RID: 18898
		public StatusItem TanningLightInsufficient;

		// Token: 0x040049D3 RID: 18899
		public StatusItem HotTubWaterTooCold;

		// Token: 0x040049D4 RID: 18900
		public StatusItem HotTubTooHot;

		// Token: 0x040049D5 RID: 18901
		public StatusItem HotTubFilling;

		// Token: 0x040049D6 RID: 18902
		public StatusItem WindTunnelIntake;

		// Token: 0x040049D7 RID: 18903
		public StatusItem CollectingHEP;

		// Token: 0x040049D8 RID: 18904
		public StatusItem ReactorRefuelDisabled;

		// Token: 0x040049D9 RID: 18905
		public StatusItem FridgeCooling;

		// Token: 0x040049DA RID: 18906
		public StatusItem FridgeSteady;

		// Token: 0x040049DB RID: 18907
		public StatusItem TrapNeedsArming;

		// Token: 0x040049DC RID: 18908
		public StatusItem TrapArmed;

		// Token: 0x040049DD RID: 18909
		public StatusItem TrapHasCritter;

		// Token: 0x040049DE RID: 18910
		public StatusItem WarpPortalCharging;

		// Token: 0x040049DF RID: 18911
		public StatusItem WarpConduitPartnerDisabled;

		// Token: 0x040049E0 RID: 18912
		public StatusItem InOrbit;

		// Token: 0x040049E1 RID: 18913
		public StatusItem InFlight;

		// Token: 0x040049E2 RID: 18914
		public StatusItem DestinationOutOfRange;

		// Token: 0x040049E3 RID: 18915
		public StatusItem RocketStranded;

		// Token: 0x040049E4 RID: 18916
		public StatusItem RailgunpayloadNeedsEmptying;

		// Token: 0x040049E5 RID: 18917
		public StatusItem AwaitingEmptyBuilding;

		// Token: 0x040049E6 RID: 18918
		public StatusItem DuplicantActivationRequired;

		// Token: 0x040049E7 RID: 18919
		public StatusItem RocketChecklistIncomplete;

		// Token: 0x040049E8 RID: 18920
		public StatusItem RocketCargoEmptying;

		// Token: 0x040049E9 RID: 18921
		public StatusItem RocketCargoFilling;

		// Token: 0x040049EA RID: 18922
		public StatusItem RocketCargoFull;

		// Token: 0x040049EB RID: 18923
		public StatusItem FlightAllCargoFull;

		// Token: 0x040049EC RID: 18924
		public StatusItem FlightCargoRemaining;

		// Token: 0x040049ED RID: 18925
		public StatusItem LandedRocketLacksPassengerModule;

		// Token: 0x040049EE RID: 18926
		public StatusItem PilotNeeded;

		// Token: 0x040049EF RID: 18927
		public StatusItem AutoPilotActive;

		// Token: 0x040049F0 RID: 18928
		public StatusItem InvalidMaskStationConsumptionState;

		// Token: 0x040049F1 RID: 18929
		public StatusItem ClusterTelescopeAllWorkComplete;

		// Token: 0x040049F2 RID: 18930
		public StatusItem RocketPlatformCloseToCeiling;

		// Token: 0x040049F3 RID: 18931
		public StatusItem ModuleGeneratorPowered;

		// Token: 0x040049F4 RID: 18932
		public StatusItem ModuleGeneratorNotPowered;

		// Token: 0x040049F5 RID: 18933
		public StatusItem InOrbitRequired;

		// Token: 0x040049F6 RID: 18934
		public StatusItem RailGunCooldown;

		// Token: 0x040049F7 RID: 18935
		public StatusItem NoSurfaceSight;

		// Token: 0x040049F8 RID: 18936
		public StatusItem LimitValveLimitReached;

		// Token: 0x040049F9 RID: 18937
		public StatusItem LimitValveLimitNotReached;

		// Token: 0x040049FA RID: 18938
		public StatusItem SpacePOIHarvesting;

		// Token: 0x040049FB RID: 18939
		public StatusItem SpacePOIWasting;

		// Token: 0x040049FC RID: 18940
		public StatusItem RocketRestrictionActive;

		// Token: 0x040049FD RID: 18941
		public StatusItem RocketRestrictionInactive;

		// Token: 0x040049FE RID: 18942
		public StatusItem NoRocketRestriction;

		// Token: 0x040049FF RID: 18943
		public StatusItem BroadcasterOutOfRange;

		// Token: 0x04004A00 RID: 18944
		public StatusItem LosingRadbolts;

		// Token: 0x04004A01 RID: 18945
		public StatusItem FabricatorAcceptsMutantSeeds;

		// Token: 0x04004A02 RID: 18946
		public StatusItem NoSpiceSelected;

		// Token: 0x04004A03 RID: 18947
		public StatusItem MissionControlAssistingRocket;

		// Token: 0x04004A04 RID: 18948
		public StatusItem NoRocketsToMissionControlBoost;

		// Token: 0x04004A05 RID: 18949
		public StatusItem NoRocketsToMissionControlClusterBoost;

		// Token: 0x04004A06 RID: 18950
		public StatusItem MissionControlBoosted;

		// Token: 0x04004A07 RID: 18951
		public StatusItem TransitTubeEntranceWaxReady;

		// Token: 0x04004A08 RID: 18952
		public StatusItem SpecialCargoBayClusterCritterStored;

		// Token: 0x04004A09 RID: 18953
		public StatusItem ComplexFabricatorCooking;

		// Token: 0x04004A0A RID: 18954
		public StatusItem ComplexFabricatorProducing;

		// Token: 0x04004A0B RID: 18955
		public StatusItem ComplexFabricatorTraining;

		// Token: 0x04004A0C RID: 18956
		public StatusItem ComplexFabricatorResearching;

		// Token: 0x04004A0D RID: 18957
		public StatusItem ArtifactAnalysisAnalyzing;

		// Token: 0x04004A0E RID: 18958
		public StatusItem TelescopeWorking;

		// Token: 0x04004A0F RID: 18959
		public StatusItem ClusterTelescopeMeteorWorking;

		// Token: 0x04004A10 RID: 18960
		public StatusItem GeoTunerNoGeyserSelected;

		// Token: 0x04004A11 RID: 18961
		public StatusItem GeoTunerResearchNeeded;

		// Token: 0x04004A12 RID: 18962
		public StatusItem GeoTunerResearchInProgress;

		// Token: 0x04004A13 RID: 18963
		public StatusItem GeoTunerBroadcasting;

		// Token: 0x04004A14 RID: 18964
		public StatusItem GeoTunerGeyserStatus;

		// Token: 0x04004A15 RID: 18965
		public StatusItem GeyserGeotuned;

		// Token: 0x04004A16 RID: 18966
		public StatusItem SkyVisNone;

		// Token: 0x04004A17 RID: 18967
		public StatusItem SkyVisLimited;

		// Token: 0x04004A18 RID: 18968
		public StatusItem CreatureManipulatorWaiting;

		// Token: 0x04004A19 RID: 18969
		public StatusItem CreatureManipulatorProgress;

		// Token: 0x04004A1A RID: 18970
		public StatusItem CreatureManipulatorMorphModeLocked;

		// Token: 0x04004A1B RID: 18971
		public StatusItem CreatureManipulatorMorphMode;

		// Token: 0x04004A1C RID: 18972
		public StatusItem CreatureManipulatorWorking;

		// Token: 0x04004A1D RID: 18973
		public StatusItem MegaBrainNotEnoughOxygen;

		// Token: 0x04004A1E RID: 18974
		public StatusItem MegaBrainTankActivationProgress;

		// Token: 0x04004A1F RID: 18975
		public StatusItem MegaBrainTankDreamAnalysis;

		// Token: 0x04004A20 RID: 18976
		public StatusItem MegaBrainTankAllDupesAreDead;

		// Token: 0x04004A21 RID: 18977
		public StatusItem MegaBrainTankComplete;

		// Token: 0x04004A22 RID: 18978
		public StatusItem FossilHuntExcavationOrdered;

		// Token: 0x04004A23 RID: 18979
		public StatusItem FossilHuntExcavationInProgress;

		// Token: 0x02001C1E RID: 7198
		public interface ISkyVisInfo
		{
			// Token: 0x06009BAD RID: 39853
			float GetPercentVisible01();
		}
	}
}
