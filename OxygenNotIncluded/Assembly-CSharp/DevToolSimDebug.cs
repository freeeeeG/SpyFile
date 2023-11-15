﻿using System;
using System.Collections.Generic;
using System.Reflection;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000569 RID: 1385
public class DevToolSimDebug : DevTool
{
	// Token: 0x060021AC RID: 8620 RVA: 0x000B71FC File Offset: 0x000B53FC
	public DevToolSimDebug()
	{
		this.elementNames = Enum.GetNames(typeof(SimHashes));
		Array.Sort<string>(this.elementNames);
		DevToolSimDebug.Instance = this;
		List<string> list = new List<string>();
		this.modeLookup = new Dictionary<string, HashedString>();
		this.revModeLookup = new Dictionary<HashedString, string>();
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			foreach (Type type in assemblies[i].GetTypes())
			{
				if (typeof(OverlayModes.Mode).IsAssignableFrom(type))
				{
					FieldInfo field = type.GetField("ID");
					if (field != null)
					{
						object value = field.GetValue(null);
						if (value != null)
						{
							HashedString hashedString = (HashedString)value;
							list.Add(type.Name);
							this.modeLookup[type.Name] = hashedString;
							this.revModeLookup[hashedString] = type.Name;
						}
					}
				}
			}
		}
		foreach (FieldInfo fieldInfo in typeof(SimDebugView.OverlayModes).GetFields())
		{
			if (fieldInfo.FieldType == typeof(HashedString))
			{
				object value2 = fieldInfo.GetValue(null);
				if (value2 != null)
				{
					HashedString hashedString2 = (HashedString)value2;
					list.Add(fieldInfo.Name);
					this.modeLookup[fieldInfo.Name] = hashedString2;
					this.revModeLookup[hashedString2] = fieldInfo.Name;
				}
			}
		}
		list.Sort();
		list.Insert(0, "None");
		this.modeLookup["None"] = "None";
		this.revModeLookup["None"] = "None";
		list.RemoveAll((string s) => s == null);
		this.overlayModes = list.ToArray();
		this.gameGridModes = Enum.GetNames(typeof(SimDebugView.GameGridMode));
	}

	// Token: 0x060021AD RID: 8621 RVA: 0x000B7450 File Offset: 0x000B5650
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			return;
		}
		HashedString hashedString = SimDebugView.Instance.GetMode();
		HashedString y = hashedString;
		if (this.overlayModes != null)
		{
			this.selectedOverlayMode = Array.IndexOf<string>(this.overlayModes, this.revModeLookup[hashedString]);
			this.selectedOverlayMode = ((this.selectedOverlayMode == -1) ? 0 : this.selectedOverlayMode);
			ImGui.Combo("Debug Mode", ref this.selectedOverlayMode, this.overlayModes, this.overlayModes.Length);
			hashedString = this.modeLookup[this.overlayModes[this.selectedOverlayMode]];
			if (hashedString == "None")
			{
				hashedString = OverlayModes.None.ID;
			}
		}
		if (hashedString != y)
		{
			SimDebugView.Instance.SetMode(hashedString);
		}
		if (hashedString == OverlayModes.Temperature.ID)
		{
			ImGui.InputFloat("Min Expected Temp:", ref SimDebugView.Instance.minTempExpected);
			ImGui.InputFloat("Max Expected Temp:", ref SimDebugView.Instance.maxTempExpected);
		}
		else if (hashedString == SimDebugView.OverlayModes.Mass)
		{
			ImGui.InputFloat("Min Expected Mass:", ref SimDebugView.Instance.minMassExpected);
			ImGui.InputFloat("Max Expected Mass:", ref SimDebugView.Instance.maxMassExpected);
		}
		else if (hashedString == SimDebugView.OverlayModes.Pressure)
		{
			ImGui.InputFloat("Min Expected Pressure:", ref SimDebugView.Instance.minPressureExpected);
			ImGui.InputFloat("Max Expected Pressure:", ref SimDebugView.Instance.maxPressureExpected);
		}
		else if (hashedString == SimDebugView.OverlayModes.GameGrid)
		{
			int gameGridMode = (int)SimDebugView.Instance.GetGameGridMode();
			ImGui.Combo("Grid Mode", ref gameGridMode, this.gameGridModes, this.gameGridModes.Length);
			SimDebugView.Instance.SetGameGridMode((SimDebugView.GameGridMode)gameGridMode);
		}
		int num;
		int num2;
		Grid.PosToXY(this.worldPos, out num, out num2);
		int num3 = num2 * Grid.WidthInCells + num;
		this.showMouseData = ImGui.CollapsingHeader("Mouse Data");
		if (this.showMouseData)
		{
			ImGui.Indent();
			string str = "WorldPos: ";
			Vector3 vector = this.worldPos;
			ImGui.Text(str + vector.ToString());
			ImGui.Unindent();
		}
		if (num3 < 0 || Grid.CellCount <= num3)
		{
			return;
		}
		if (this.showMouseData)
		{
			ImGui.Indent();
			ImGui.Text("CellPos: " + num.ToString() + ", " + num2.ToString());
			int num4 = (num2 + 1) * (Grid.WidthInCells + 2) + (num + 1);
			if (ImGui.InputInt("Sim Cell:", ref num4))
			{
				num = Mathf.Max(0, num4 % (Grid.WidthInCells + 2) - 1);
				num2 = Mathf.Max(0, num4 / (Grid.WidthInCells + 2) - 1);
				this.worldPos = Grid.CellToPosCCC(Grid.XYToCell(num, num2), Grid.SceneLayer.Front);
			}
			if (ImGui.InputInt("Game Cell:", ref num3))
			{
				num = num3 % Grid.WidthInCells;
				num2 = num3 / Grid.WidthInCells;
				this.worldPos = Grid.CellToPosCCC(Grid.XYToCell(num, num2), Grid.SceneLayer.Front);
			}
			int num5 = Grid.WidthInCells / 32;
			int num6 = num / 32;
			int num7 = num2 / 32;
			int num8 = num7 * num5 + num6;
			ImGui.Text(string.Format("Chunk Idx ({0}, {1}): {2}", num6, num7, num8));
			ImGui.Text("RenderedByWorld: " + Grid.RenderedByWorld[num3].ToString());
			ImGui.Text("Solid: " + Grid.Solid[num3].ToString());
			ImGui.Text("Damage: " + Grid.Damage[num3].ToString());
			ImGui.Text("Foundation: " + Grid.Foundation[num3].ToString());
			ImGui.Text("Revealed: " + Grid.Revealed[num3].ToString());
			ImGui.Text("Visible: " + Grid.Visible[num3].ToString());
			ImGui.Text("DupePassable: " + Grid.DupePassable[num3].ToString());
			ImGui.Text("DupeImpassable: " + Grid.DupeImpassable[num3].ToString());
			ImGui.Text("CritterImpassable: " + Grid.CritterImpassable[num3].ToString());
			ImGui.Text("FakeFloor: " + Grid.FakeFloor[num3].ToString());
			ImGui.Text("HasDoor: " + Grid.HasDoor[num3].ToString());
			ImGui.Text("HasLadder: " + Grid.HasLadder[num3].ToString());
			ImGui.Text("HasPole: " + Grid.HasPole[num3].ToString());
			ImGui.Text("GravitasFacility: " + Grid.GravitasFacility[num3].ToString());
			ImGui.Text("HasNavTeleporter: " + Grid.HasNavTeleporter[num3].ToString());
			ImGui.Text("IsTileUnderConstruction: " + Grid.IsTileUnderConstruction[num3].ToString());
			ImGui.Text("LiquidVisPlacers: " + Game.Instance.liquidConduitSystem.GetConnections(num3, false).ToString());
			ImGui.Text("LiquidPhysPlacers: " + Game.Instance.liquidConduitSystem.GetConnections(num3, true).ToString());
			ImGui.Text("GasVisPlacers: " + Game.Instance.gasConduitSystem.GetConnections(num3, false).ToString());
			ImGui.Text("GasPhysPlacers: " + Game.Instance.gasConduitSystem.GetConnections(num3, true).ToString());
			ImGui.Text("ElecVisPlacers: " + Game.Instance.electricalConduitSystem.GetConnections(num3, false).ToString());
			ImGui.Text("ElecPhysPlacers: " + Game.Instance.electricalConduitSystem.GetConnections(num3, true).ToString());
			ImGui.Text("World Idx: " + Grid.WorldIdx[num3].ToString());
			ImGui.Text("ZoneType: " + World.Instance.zoneRenderData.GetSubWorldZoneType(num3).ToString());
			ImGui.Text("Light Intensity: " + Grid.LightIntensity[num3].ToString());
			ImGui.Text("Sunlight: " + Grid.ExposedToSunlight[num3].ToString());
			ImGui.Text("Radiation: " + Grid.Radiation[num3].ToString());
			this.showAccessRestrictions = ImGui.CollapsingHeader("Access Restrictions");
			if (this.showAccessRestrictions)
			{
				ImGui.Indent();
				Grid.Restriction restriction;
				if (!Grid.DEBUG_GetRestrictions(num3, out restriction))
				{
					ImGui.Text("No access control.");
				}
				else
				{
					ImGui.Text("Orientation: " + restriction.orientation.ToString());
					ImGui.Text("Default Restriction: " + restriction.DirectionMasksForMinionInstanceID[-1].ToString());
					ImGui.Indent();
					foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
					{
						int instanceID = minionIdentity.GetComponent<MinionIdentity>().assignableProxy.Get().GetComponent<KPrefabID>().InstanceID;
						Grid.Restriction.Directions directions;
						if (restriction.DirectionMasksForMinionInstanceID.TryGetValue(instanceID, out directions))
						{
							ImGui.Text(minionIdentity.name + " Restriction: " + directions.ToString());
						}
						else
						{
							ImGui.Text(minionIdentity.name + ": Has No restriction");
						}
					}
					ImGui.Unindent();
				}
				ImGui.Unindent();
			}
			this.showGridContents = ImGui.CollapsingHeader("Grid Objects");
			if (this.showGridContents)
			{
				ImGui.Indent();
				for (int i = 0; i < 45; i++)
				{
					GameObject gameObject = Grid.Objects[num3, i];
					ImGui.Text(Enum.GetName(typeof(ObjectLayer), i) + ": " + ((gameObject != null) ? gameObject.name : "None"));
				}
				ImGui.Unindent();
			}
			this.showScenePartitionerContents = ImGui.CollapsingHeader("Scene Partitioner");
			if (this.showScenePartitionerContents)
			{
				ImGui.Indent();
				if (GameScenePartitioner.Instance != null)
				{
					this.showLayerToggles = ImGui.CollapsingHeader("Layers");
					if (this.showLayerToggles)
					{
						bool flag = false;
						foreach (ScenePartitionerLayer scenePartitionerLayer in GameScenePartitioner.Instance.GetLayers())
						{
							bool flag2 = this.toggledLayers.Contains(scenePartitionerLayer);
							bool flag3 = flag2;
							ImGui.Checkbox(HashCache.Get().Get(scenePartitionerLayer.name), ref flag3);
							if (flag3 != flag2)
							{
								flag = true;
								if (flag3)
								{
									this.toggledLayers.Add(scenePartitionerLayer);
								}
								else
								{
									this.toggledLayers.Remove(scenePartitionerLayer);
								}
							}
						}
						if (flag)
						{
							GameScenePartitioner.Instance.SetToggledLayers(this.toggledLayers);
							if (this.toggledLayers.Count > 0)
							{
								SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.ScenePartitioner);
							}
						}
					}
					ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
					foreach (ScenePartitionerLayer layer in GameScenePartitioner.Instance.GetLayers())
					{
						pooledList.Clear();
						GameScenePartitioner.Instance.GatherEntries(num, num2, 1, 1, layer, pooledList);
						foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
						{
							GameObject gameObject2 = scenePartitionerEntry.obj as GameObject;
							MonoBehaviour monoBehaviour = scenePartitionerEntry.obj as MonoBehaviour;
							if (gameObject2 != null)
							{
								ImGui.Text(gameObject2.name);
							}
							else if (monoBehaviour != null)
							{
								ImGui.Text(monoBehaviour.name);
							}
						}
					}
					pooledList.Recycle();
				}
				ImGui.Unindent();
			}
			this.showCavityInfo = ImGui.CollapsingHeader("Cavity Info");
			if (this.showCavityInfo)
			{
				ImGui.Indent();
				CavityInfo cavityInfo = null;
				if (Game.Instance != null && Game.Instance.roomProber != null)
				{
					cavityInfo = Game.Instance.roomProber.GetCavityForCell(num3);
				}
				if (cavityInfo != null)
				{
					ImGui.Text("Cell Count: " + cavityInfo.numCells.ToString());
					Room room = cavityInfo.room;
					if (room != null)
					{
						ImGui.Text("Is Room: True");
						this.showBuildings = ImGui.CollapsingHeader("Buildings (" + room.buildings.Count.ToString() + ")");
						if (this.showBuildings)
						{
							foreach (KPrefabID kprefabID in room.buildings)
							{
								ImGui.Text(kprefabID.ToString());
							}
						}
						this.showCreatures = ImGui.CollapsingHeader("Creatures (" + room.cavity.creatures.Count.ToString() + ")");
						if (!this.showCreatures)
						{
							goto IL_C53;
						}
						using (List<KPrefabID>.Enumerator enumerator4 = room.cavity.creatures.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								KPrefabID kprefabID2 = enumerator4.Current;
								ImGui.Text(kprefabID2.ToString());
							}
							goto IL_C53;
						}
					}
					ImGui.Text("Is Room: False");
				}
				else
				{
					ImGui.Text("No Cavity Detected");
				}
				IL_C53:
				ImGui.Unindent();
			}
			this.showPropertyInfo = ImGui.CollapsingHeader("Property Info");
			if (this.showPropertyInfo)
			{
				ImGui.Indent();
				bool flag4 = true;
				byte b = Grid.Properties[num3];
				foreach (object obj in Enum.GetValues(typeof(Sim.Cell.Properties)))
				{
					if (((int)b & (int)obj) != 0)
					{
						ImGui.Text(obj.ToString());
						flag4 = false;
					}
				}
				if (flag4)
				{
					ImGui.Text("No properties");
				}
				ImGui.Unindent();
			}
			ImGui.Unindent();
		}
		if (Grid.ObjectLayers != null)
		{
			Element element = Grid.Element[num3];
			this.showElementData = ImGui.CollapsingHeader("Element");
			ImGui.SameLine();
			ImGui.Text("[" + element.name + "]");
			ImGui.Text("Mass:" + Grid.Mass[num3].ToString());
			if (this.showElementData)
			{
				this.DrawElem(element);
			}
			ImGui.Text("Average Flow Rate (kg/s):" + (Grid.AccumulatedFlow[num3] / 3f).ToString());
		}
		this.showPhysicsData = ImGui.CollapsingHeader("Physics Data");
		if (this.showPhysicsData)
		{
			ImGui.Indent();
			ImGui.Text("Solid: " + Grid.Solid[num3].ToString());
			ImGui.Text("Pressure: " + Grid.Pressure[num3].ToString());
			ImGui.Text("Temperature (kelvin -272.15): " + Grid.Temperature[num3].ToString());
			ImGui.Text("Radiation: " + Grid.Radiation[num3].ToString());
			ImGui.Text("Mass: " + Grid.Mass[num3].ToString());
			ImGui.Text("Insulation: " + ((float)Grid.Insulation[num3] / 255f).ToString());
			ImGui.Text("Strength Multiplier: " + Grid.StrengthInfo[num3].ToString());
			ImGui.Text("Properties: 0x: " + Grid.Properties[num3].ToString("X"));
			ImGui.Text("Disease: " + ((Grid.DiseaseIdx[num3] == byte.MaxValue) ? "None" : Db.Get().Diseases[(int)Grid.DiseaseIdx[num3]].Name));
			ImGui.Text("Disease Count: " + Grid.DiseaseCount[num3].ToString());
			ImGui.Unindent();
		}
		this.showGasConduitData = ImGui.CollapsingHeader("Gas Conduit Data");
		if (this.showGasConduitData)
		{
			this.DrawConduitFlow(Game.Instance.gasConduitFlow, num3);
		}
		this.showLiquidConduitData = ImGui.CollapsingHeader("Liquid Conduit Data");
		if (this.showLiquidConduitData)
		{
			this.DrawConduitFlow(Game.Instance.liquidConduitFlow, num3);
		}
	}

	// Token: 0x060021AE RID: 8622 RVA: 0x000B8468 File Offset: 0x000B6668
	private void DrawElem(Element element)
	{
		ImGui.Indent();
		ImGui.Text("State: " + element.state.ToString());
		ImGui.Text("Thermal Conductivity: " + element.thermalConductivity.ToString());
		ImGui.Text("Specific Heat Capacity: " + element.specificHeatCapacity.ToString());
		if (element.lowTempTransition != null)
		{
			ImGui.Text("Low Temperature: " + element.lowTemp.ToString());
			ImGui.Text("Low Temperature Transition: " + element.lowTempTransitionTarget.ToString());
		}
		if (element.highTempTransition != null)
		{
			ImGui.Text("High Temperature: " + element.highTemp.ToString());
			ImGui.Text("HighTemp Temperature Transition: " + element.highTempTransitionTarget.ToString());
			if (element.highTempTransitionOreID != (SimHashes)0)
			{
				ImGui.Text("HighTemp Temperature Transition: " + element.highTempTransitionOreID.ToString());
			}
		}
		ImGui.Text("Light Absorption Factor: " + element.lightAbsorptionFactor.ToString());
		ImGui.Text("Radiation Absorption Factor: " + element.radiationAbsorptionFactor.ToString());
		ImGui.Text("Radiation Per 1000 Mass: " + element.radiationPer1000Mass.ToString());
		ImGui.Text("Sublimate ID: " + element.sublimateId.ToString());
		ImGui.Text("Sublimate FX: " + element.sublimateFX.ToString());
		ImGui.Text("Sublimate Rate: " + element.sublimateRate.ToString());
		ImGui.Text("Sublimate Efficiency: " + element.sublimateEfficiency.ToString());
		ImGui.Text("Sublimate Probability: " + element.sublimateProbability.ToString());
		ImGui.Text("Off Gas Percentage: " + element.offGasPercentage.ToString());
		if (element.IsGas)
		{
			ImGui.Text("Default Pressure: " + element.defaultValues.pressure.ToString());
		}
		else
		{
			ImGui.Text("Default Mass: " + element.defaultValues.mass.ToString());
		}
		ImGui.Text("Default Temperature: " + element.defaultValues.temperature.ToString());
		if (element.IsGas)
		{
			ImGui.Text("Flow: " + element.flow.ToString());
		}
		if (element.IsLiquid)
		{
			ImGui.Text("Max Comp: " + element.maxCompression.ToString());
			ImGui.Text("Max Mass: " + element.maxMass.ToString());
		}
		if (element.IsSolid)
		{
			ImGui.Text("Hardness: " + element.hardness.ToString());
			ImGui.Text("Unstable: " + element.IsUnstable.ToString());
		}
		ImGui.Unindent();
	}

	// Token: 0x060021AF RID: 8623 RVA: 0x000B877C File Offset: 0x000B697C
	private void DrawConduitFlow(ConduitFlow flow_mgr, int cell)
	{
		ImGui.Indent();
		ConduitFlow.ConduitContents contents = flow_mgr.GetContents(cell);
		ImGui.Text("Element: " + contents.element.ToString());
		ImGui.Text(string.Format("Mass: {0}", contents.mass));
		ImGui.Text(string.Format("Movable Mass: {0}", contents.movable_mass));
		ImGui.Text("Temperature: " + contents.temperature.ToString());
		ImGui.Text("Disease: " + ((contents.diseaseIdx == byte.MaxValue) ? "None" : Db.Get().Diseases[(int)contents.diseaseIdx].Name));
		ImGui.Text("Disease Count: " + contents.diseaseCount.ToString());
		ImGui.Text(string.Format("Update Order: {0}", flow_mgr.ComputeUpdateOrder(cell)));
		flow_mgr.SetContents(cell, contents);
		ConduitFlow.FlowDirections permittedFlow = flow_mgr.GetPermittedFlow(cell);
		if (permittedFlow == ConduitFlow.FlowDirections.None)
		{
			ImGui.Text("PermittedFlow: None");
		}
		else
		{
			string text = "";
			if ((permittedFlow & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
			{
				text += " Up ";
			}
			if ((permittedFlow & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
			{
				text += " Down ";
			}
			if ((permittedFlow & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
			{
				text += " Left ";
			}
			if ((permittedFlow & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
			{
				text += " Right ";
			}
			ImGui.Text("PermittedFlow: " + text);
		}
		ImGui.Unindent();
	}

	// Token: 0x060021B0 RID: 8624 RVA: 0x000B88F9 File Offset: 0x000B6AF9
	public void SetCell(int cell)
	{
		this.worldPos = Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
	}

	// Token: 0x04001317 RID: 4887
	private Vector3 worldPos = Vector3.zero;

	// Token: 0x04001318 RID: 4888
	private string[] elementNames;

	// Token: 0x04001319 RID: 4889
	private Dictionary<SimHashes, double> elementCounts = new Dictionary<SimHashes, double>();

	// Token: 0x0400131A RID: 4890
	public static DevToolSimDebug Instance;

	// Token: 0x0400131B RID: 4891
	private const string INVALID_OVERLAY_MODE_STR = "None";

	// Token: 0x0400131C RID: 4892
	private bool showElementData;

	// Token: 0x0400131D RID: 4893
	private bool showMouseData = true;

	// Token: 0x0400131E RID: 4894
	private bool showAccessRestrictions;

	// Token: 0x0400131F RID: 4895
	private bool showGridContents;

	// Token: 0x04001320 RID: 4896
	private bool showScenePartitionerContents;

	// Token: 0x04001321 RID: 4897
	private bool showLayerToggles;

	// Token: 0x04001322 RID: 4898
	private bool showCavityInfo;

	// Token: 0x04001323 RID: 4899
	private bool showPropertyInfo;

	// Token: 0x04001324 RID: 4900
	private bool showBuildings;

	// Token: 0x04001325 RID: 4901
	private bool showCreatures;

	// Token: 0x04001326 RID: 4902
	private bool showPhysicsData;

	// Token: 0x04001327 RID: 4903
	private bool showGasConduitData;

	// Token: 0x04001328 RID: 4904
	private bool showLiquidConduitData;

	// Token: 0x04001329 RID: 4905
	private string[] overlayModes;

	// Token: 0x0400132A RID: 4906
	private int selectedOverlayMode;

	// Token: 0x0400132B RID: 4907
	private string[] gameGridModes;

	// Token: 0x0400132C RID: 4908
	private Dictionary<string, HashedString> modeLookup;

	// Token: 0x0400132D RID: 4909
	private Dictionary<HashedString, string> revModeLookup;

	// Token: 0x0400132E RID: 4910
	private HashSet<ScenePartitionerLayer> toggledLayers = new HashSet<ScenePartitionerLayer>();
}
