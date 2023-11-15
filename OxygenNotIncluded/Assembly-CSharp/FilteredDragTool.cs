using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000819 RID: 2073
public class FilteredDragTool : DragTool
{
	// Token: 0x06003BC8 RID: 15304 RVA: 0x0014C001 File Offset: 0x0014A201
	public bool IsActiveLayer(string layer)
	{
		return this.currentFilterTargets[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On || (this.currentFilterTargets.ContainsKey(layer.ToUpper()) && this.currentFilterTargets[layer.ToUpper()] == ToolParameterMenu.ToggleState.On);
	}

	// Token: 0x06003BC9 RID: 15305 RVA: 0x0014C040 File Offset: 0x0014A240
	public bool IsActiveLayer(ObjectLayer layer)
	{
		if (this.currentFilterTargets.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) && this.currentFilterTargets[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On)
		{
			return true;
		}
		bool result = false;
		foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> keyValuePair in this.currentFilterTargets)
		{
			if (keyValuePair.Value == ToolParameterMenu.ToggleState.On && this.GetObjectLayerFromFilterLayer(keyValuePair.Key) == layer)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003BCA RID: 15306 RVA: 0x0014C0D4 File Offset: 0x0014A2D4
	protected virtual void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BACKWALL, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x06003BCB RID: 15307 RVA: 0x0014C141 File Offset: 0x0014A341
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ResetFilter(this.filterTargets);
	}

	// Token: 0x06003BCC RID: 15308 RVA: 0x0014C155 File Offset: 0x0014A355
	protected override void OnSpawn()
	{
		base.OnSpawn();
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
	}

	// Token: 0x06003BCD RID: 15309 RVA: 0x0014C183 File Offset: 0x0014A383
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		base.OnCleanUp();
	}

	// Token: 0x06003BCE RID: 15310 RVA: 0x0014C1B1 File Offset: 0x0014A3B1
	public void ResetFilter()
	{
		this.ResetFilter(this.filterTargets);
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x0014C1BF File Offset: 0x0014A3BF
	protected void ResetFilter(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Clear();
		this.GetDefaultFilters(filters);
		this.currentFilterTargets = filters;
	}

	// Token: 0x06003BD0 RID: 15312 RVA: 0x0014C1D5 File Offset: 0x0014A3D5
	protected override void OnActivateTool()
	{
		this.active = true;
		base.OnActivateTool();
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
	}

	// Token: 0x06003BD1 RID: 15313 RVA: 0x0014C1F4 File Offset: 0x0014A3F4
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.active = false;
		ToolMenu.Instance.toolParameterMenu.ClearMenu();
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06003BD2 RID: 15314 RVA: 0x0014C214 File Offset: 0x0014A414
	public virtual string GetFilterLayerFromGameObject(GameObject input)
	{
		BuildingComplete component = input.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component2 = input.GetComponent<BuildingUnderConstruction>();
		if (component)
		{
			return this.GetFilterLayerFromObjectLayer(component.Def.ObjectLayer);
		}
		if (component2)
		{
			return this.GetFilterLayerFromObjectLayer(component2.Def.ObjectLayer);
		}
		if (input.GetComponent<Clearable>() != null || input.GetComponent<Moppable>() != null)
		{
			return "CleanAndClear";
		}
		if (input.GetComponent<Diggable>() != null)
		{
			return "DigPlacer";
		}
		return "Default";
	}

	// Token: 0x06003BD3 RID: 15315 RVA: 0x0014C2A0 File Offset: 0x0014A4A0
	public string GetFilterLayerFromObjectLayer(ObjectLayer gamer_layer)
	{
		if (gamer_layer > ObjectLayer.FoundationTile)
		{
			switch (gamer_layer)
			{
			case ObjectLayer.GasConduit:
			case ObjectLayer.GasConduitConnection:
				return "GasPipes";
			case ObjectLayer.GasConduitTile:
			case ObjectLayer.ReplacementGasConduit:
			case ObjectLayer.LiquidConduitTile:
			case ObjectLayer.ReplacementLiquidConduit:
				goto IL_AC;
			case ObjectLayer.LiquidConduit:
			case ObjectLayer.LiquidConduitConnection:
				return "LiquidPipes";
			case ObjectLayer.SolidConduit:
				break;
			default:
				switch (gamer_layer)
				{
				case ObjectLayer.SolidConduitConnection:
					break;
				case ObjectLayer.LadderTile:
				case ObjectLayer.ReplacementLadder:
				case ObjectLayer.WireTile:
				case ObjectLayer.ReplacementWire:
					goto IL_AC;
				case ObjectLayer.Wire:
				case ObjectLayer.WireConnectors:
					return "Wires";
				case ObjectLayer.LogicGate:
				case ObjectLayer.LogicWire:
					return "Logic";
				default:
					if (gamer_layer == ObjectLayer.Gantry)
					{
						goto IL_7C;
					}
					goto IL_AC;
				}
				break;
			}
			return "SolidConduits";
		}
		if (gamer_layer != ObjectLayer.Building)
		{
			if (gamer_layer == ObjectLayer.Backwall)
			{
				return "BackWall";
			}
			if (gamer_layer != ObjectLayer.FoundationTile)
			{
				goto IL_AC;
			}
			return "Tiles";
		}
		IL_7C:
		return "Buildings";
		IL_AC:
		return "Default";
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x0014C360 File Offset: 0x0014A560
	private ObjectLayer GetObjectLayerFromFilterLayer(string filter_layer)
	{
		string text = filter_layer.ToLower();
		if (text != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			ObjectLayer result;
			if (num <= 2200975418U)
			{
				if (num <= 388608975U)
				{
					if (num != 25076977U)
					{
						if (num != 388608975U)
						{
							goto IL_12D;
						}
						if (!(text == "solidconduits"))
						{
							goto IL_12D;
						}
						result = ObjectLayer.SolidConduit;
					}
					else
					{
						if (!(text == "wires"))
						{
							goto IL_12D;
						}
						result = ObjectLayer.Wire;
					}
				}
				else if (num != 614364310U)
				{
					if (num != 2200975418U)
					{
						goto IL_12D;
					}
					if (!(text == "backwall"))
					{
						goto IL_12D;
					}
					result = ObjectLayer.Backwall;
				}
				else
				{
					if (!(text == "liquidpipes"))
					{
						goto IL_12D;
					}
					result = ObjectLayer.LiquidConduit;
				}
			}
			else if (num <= 2875565775U)
			{
				if (num != 2366751346U)
				{
					if (num != 2875565775U)
					{
						goto IL_12D;
					}
					if (!(text == "gaspipes"))
					{
						goto IL_12D;
					}
					result = ObjectLayer.GasConduit;
				}
				else
				{
					if (!(text == "buildings"))
					{
						goto IL_12D;
					}
					result = ObjectLayer.Building;
				}
			}
			else if (num != 3464443665U)
			{
				if (num != 4178729166U)
				{
					goto IL_12D;
				}
				if (!(text == "tiles"))
				{
					goto IL_12D;
				}
				result = ObjectLayer.FoundationTile;
			}
			else
			{
				if (!(text == "logic"))
				{
					goto IL_12D;
				}
				result = ObjectLayer.LogicWire;
			}
			return result;
		}
		IL_12D:
		throw new ArgumentException("Invalid filter layer: " + filter_layer);
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x0014C4AC File Offset: 0x0014A6AC
	private void OnOverlayChanged(HashedString overlay)
	{
		if (!this.active)
		{
			return;
		}
		string text = null;
		if (overlay == OverlayModes.Power.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.WIRES;
		}
		else if (overlay == OverlayModes.LiquidConduits.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT;
		}
		else if (overlay == OverlayModes.GasConduits.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.GASCONDUIT;
		}
		else if (overlay == OverlayModes.SolidConveyor.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT;
		}
		else if (overlay == OverlayModes.Logic.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.LOGIC;
		}
		this.currentFilterTargets = this.filterTargets;
		if (text != null)
		{
			using (List<string>.Enumerator enumerator = new List<string>(this.filterTargets.Keys).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text2 = enumerator.Current;
					this.filterTargets[text2] = ToolParameterMenu.ToggleState.Disabled;
					if (text2 == text)
					{
						this.filterTargets[text2] = ToolParameterMenu.ToggleState.On;
					}
				}
				goto IL_102;
			}
		}
		if (this.overlayFilterTargets.Count == 0)
		{
			this.ResetFilter(this.overlayFilterTargets);
		}
		this.currentFilterTargets = this.overlayFilterTargets;
		IL_102:
		ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.currentFilterTargets);
	}

	// Token: 0x04002755 RID: 10069
	private Dictionary<string, ToolParameterMenu.ToggleState> filterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>();

	// Token: 0x04002756 RID: 10070
	private Dictionary<string, ToolParameterMenu.ToggleState> overlayFilterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>();

	// Token: 0x04002757 RID: 10071
	private Dictionary<string, ToolParameterMenu.ToggleState> currentFilterTargets;

	// Token: 0x04002758 RID: 10072
	private bool active;
}
