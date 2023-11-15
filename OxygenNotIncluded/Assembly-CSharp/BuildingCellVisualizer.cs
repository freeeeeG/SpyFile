using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000486 RID: 1158
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/BuildingCellVisualizer")]
public class BuildingCellVisualizer : KMonoBehaviour
{
	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x0600197F RID: 6527 RVA: 0x0008561F File Offset: 0x0008381F
	public bool RequiresPowerInput
	{
		get
		{
			return (this.ports & BuildingCellVisualizer.Ports.PowerIn) > (BuildingCellVisualizer.Ports)0;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06001980 RID: 6528 RVA: 0x0008562C File Offset: 0x0008382C
	public bool RequiresPowerOutput
	{
		get
		{
			return (this.ports & BuildingCellVisualizer.Ports.PowerOut) > (BuildingCellVisualizer.Ports)0;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06001981 RID: 6529 RVA: 0x00085639 File Offset: 0x00083839
	public bool RequiresPower
	{
		get
		{
			return (this.ports & (BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut)) > (BuildingCellVisualizer.Ports)0;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06001982 RID: 6530 RVA: 0x00085646 File Offset: 0x00083846
	public bool RequiresGas
	{
		get
		{
			return (this.ports & (BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut)) > (BuildingCellVisualizer.Ports)0;
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x06001983 RID: 6531 RVA: 0x00085654 File Offset: 0x00083854
	public bool RequiresLiquid
	{
		get
		{
			return (this.ports & (BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut)) > (BuildingCellVisualizer.Ports)0;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06001984 RID: 6532 RVA: 0x00085662 File Offset: 0x00083862
	public bool RequiresSolid
	{
		get
		{
			return (this.ports & (BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut)) > (BuildingCellVisualizer.Ports)0;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06001985 RID: 6533 RVA: 0x00085673 File Offset: 0x00083873
	public bool RequiresUtilityConnection
	{
		get
		{
			return (this.ports & (BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut)) > (BuildingCellVisualizer.Ports)0;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06001986 RID: 6534 RVA: 0x00085684 File Offset: 0x00083884
	public bool RequiresHighEnergyParticles
	{
		get
		{
			return (this.ports & BuildingCellVisualizer.Ports.HighEnergyParticle) > (BuildingCellVisualizer.Ports)0;
		}
	}

	// Token: 0x06001987 RID: 6535 RVA: 0x00085695 File Offset: 0x00083895
	public void ConnectedEventWithDelay(float delay, int connectionCount, int cell, string soundName)
	{
		base.StartCoroutine(this.ConnectedDelay(delay, connectionCount, cell, soundName));
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x000856A9 File Offset: 0x000838A9
	private IEnumerator ConnectedDelay(float delay, int connectionCount, int cell, string soundName)
	{
		float startTime = Time.realtimeSinceStartup;
		float currentTime = startTime;
		while (currentTime < startTime + delay)
		{
			currentTime += Time.unscaledDeltaTime;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.ConnectedEvent(cell);
		string sound = GlobalAssets.GetSound(soundName, false);
		if (sound != null)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = 0f;
			EventInstance instance = SoundEvent.BeginOneShot(sound, position, 1f, false);
			instance.setParameterByName("connectedCount", (float)connectionCount, false);
			SoundEvent.EndOneShot(instance);
		}
		yield break;
	}

	// Token: 0x06001989 RID: 6537 RVA: 0x000856D8 File Offset: 0x000838D8
	public void ConnectedEvent(int cell)
	{
		GameObject gameObject = null;
		if (this.inputVisualizer != null && Grid.PosToCell(this.inputVisualizer) == cell)
		{
			gameObject = this.inputVisualizer;
		}
		else if (this.outputVisualizer != null && Grid.PosToCell(this.outputVisualizer) == cell)
		{
			gameObject = this.outputVisualizer;
		}
		else if (this.secondaryInputVisualizer != null && Grid.PosToCell(this.secondaryInputVisualizer) == cell)
		{
			gameObject = this.secondaryInputVisualizer;
		}
		else if (this.secondaryOutputVisualizer != null && Grid.PosToCell(this.secondaryOutputVisualizer) == cell)
		{
			gameObject = this.secondaryOutputVisualizer;
		}
		if (gameObject == null)
		{
			return;
		}
		SizePulse pulse = gameObject.gameObject.AddComponent<SizePulse>();
		pulse.speed = 20f;
		pulse.multiplier = 0.75f;
		pulse.updateWhenPaused = true;
		SizePulse pulse2 = pulse;
		pulse2.onComplete = (System.Action)Delegate.Combine(pulse2.onComplete, new System.Action(delegate()
		{
			UnityEngine.Object.Destroy(pulse);
		}));
	}

	// Token: 0x0600198A RID: 6538 RVA: 0x000857F0 File Offset: 0x000839F0
	private void MapBuilding()
	{
		BuildingDef def = this.building.Def;
		if (def.CheckRequiresPowerInput())
		{
			this.ports |= BuildingCellVisualizer.Ports.PowerIn;
		}
		if (def.CheckRequiresPowerOutput())
		{
			this.ports |= BuildingCellVisualizer.Ports.PowerOut;
		}
		if (def.CheckRequiresGasInput())
		{
			this.ports |= BuildingCellVisualizer.Ports.GasIn;
		}
		if (def.CheckRequiresGasOutput())
		{
			this.ports |= BuildingCellVisualizer.Ports.GasOut;
		}
		if (def.CheckRequiresLiquidInput())
		{
			this.ports |= BuildingCellVisualizer.Ports.LiquidIn;
		}
		if (def.CheckRequiresLiquidOutput())
		{
			this.ports |= BuildingCellVisualizer.Ports.LiquidOut;
		}
		if (def.CheckRequiresSolidInput())
		{
			this.ports |= BuildingCellVisualizer.Ports.SolidIn;
		}
		if (def.CheckRequiresSolidOutput())
		{
			this.ports |= BuildingCellVisualizer.Ports.SolidOut;
		}
		if (def.CheckRequiresHighEnergyParticleInput())
		{
			this.ports |= BuildingCellVisualizer.Ports.HighEnergyParticle;
		}
		if (def.CheckRequiresHighEnergyParticleOutput())
		{
			this.ports |= BuildingCellVisualizer.Ports.HighEnergyParticle;
		}
		DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo(def.DiseaseCellVisName);
		if (info.name != null)
		{
			this.diseaseSourceSprite = Assets.instance.DiseaseVisualization.overlaySprite;
			this.diseaseSourceColour = GlobalAssets.Instance.colorSet.GetColorByName(info.overlayColourName);
		}
		foreach (ISecondaryInput secondaryInput in def.BuildingComplete.GetComponents<ISecondaryInput>())
		{
			if (secondaryInput != null)
			{
				if (secondaryInput.HasSecondaryConduitType(ConduitType.Gas))
				{
					this.secondary_ports |= BuildingCellVisualizer.Ports.GasIn;
				}
				if (secondaryInput.HasSecondaryConduitType(ConduitType.Liquid))
				{
					this.secondary_ports |= BuildingCellVisualizer.Ports.LiquidIn;
				}
				if (secondaryInput.HasSecondaryConduitType(ConduitType.Solid))
				{
					this.secondary_ports |= BuildingCellVisualizer.Ports.SolidIn;
				}
			}
		}
		foreach (ISecondaryOutput secondaryOutput in def.BuildingComplete.GetComponents<ISecondaryOutput>())
		{
			if (secondaryOutput != null)
			{
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Gas))
				{
					this.secondary_ports |= BuildingCellVisualizer.Ports.GasOut;
				}
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Liquid))
				{
					this.secondary_ports |= BuildingCellVisualizer.Ports.LiquidOut;
				}
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Solid))
				{
					this.secondary_ports |= BuildingCellVisualizer.Ports.SolidOut;
				}
			}
		}
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x00085A28 File Offset: 0x00083C28
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.inputVisualizer != null)
		{
			UnityEngine.Object.Destroy(this.inputVisualizer);
		}
		if (this.outputVisualizer != null)
		{
			UnityEngine.Object.Destroy(this.outputVisualizer);
		}
		if (this.secondaryInputVisualizer != null)
		{
			UnityEngine.Object.Destroy(this.secondaryInputVisualizer);
		}
		if (this.secondaryOutputVisualizer != null)
		{
			UnityEngine.Object.Destroy(this.secondaryOutputVisualizer);
		}
	}

	// Token: 0x0600198C RID: 6540 RVA: 0x00085AA0 File Offset: 0x00083CA0
	private Color GetWireColor(int cell)
	{
		GameObject gameObject = Grid.Objects[cell, 26];
		if (gameObject == null)
		{
			return Color.white;
		}
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		if (!(component != null))
		{
			return Color.white;
		}
		return component.TintColour;
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x00085AEC File Offset: 0x00083CEC
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (this.resources == null)
		{
			this.resources = BuildingCellVisualizerResources.Instance();
		}
		if (this.icons == null)
		{
			this.icons = new Dictionary<GameObject, Image>();
		}
		this.enableRaycast = (this.building as BuildingComplete != null);
		this.MapBuilding();
		Components.BuildingCellVisualizers.Add(this);
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x00085B53 File Offset: 0x00083D53
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		Components.BuildingCellVisualizers.Remove(this);
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x00085B68 File Offset: 0x00083D68
	public void DrawIcons(HashedString mode)
	{
		if (base.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
		{
			this.DisableIcons();
			return;
		}
		if (mode == OverlayModes.Power.ID)
		{
			if (this.RequiresPower)
			{
				bool flag = this.building as BuildingPreview != null;
				BuildingEnabledButton component = this.building.GetComponent<BuildingEnabledButton>();
				int powerInputCell = this.building.GetPowerInputCell();
				if (this.RequiresPowerInput)
				{
					int circuitID = (int)Game.Instance.circuitManager.GetCircuitID(powerInputCell);
					Color tint = (component != null && !component.IsEnabled) ? Color.gray : Color.white;
					Sprite icon_img = (!flag && circuitID != 65535) ? this.resources.electricityConnectedIcon : this.resources.electricityInputIcon;
					this.DrawUtilityIcon(powerInputCell, icon_img, ref this.inputVisualizer, tint, this.GetWireColor(powerInputCell), 1f, false);
				}
				if (this.RequiresPowerOutput)
				{
					int powerOutputCell = this.building.GetPowerOutputCell();
					int circuitID2 = (int)Game.Instance.circuitManager.GetCircuitID(powerOutputCell);
					Color color = this.building.Def.UseWhitePowerOutputConnectorColour ? Color.white : this.resources.electricityOutputColor;
					Color32 c = (component != null && !component.IsEnabled) ? Color.gray : color;
					Sprite icon_img2 = (!flag && circuitID2 != 65535) ? this.resources.electricityConnectedIcon : this.resources.electricityInputIcon;
					this.DrawUtilityIcon(powerOutputCell, icon_img2, ref this.outputVisualizer, c, this.GetWireColor(powerOutputCell), 1f, false);
					return;
				}
			}
			else
			{
				bool flag2 = true;
				Switch component2 = base.GetComponent<Switch>();
				if (component2 != null)
				{
					int cell = Grid.PosToCell(base.transform.GetPosition());
					Color32 c2 = component2.IsHandlerOn() ? this.resources.switchColor : this.resources.switchOffColor;
					this.DrawUtilityIcon(cell, this.resources.switchIcon, ref this.outputVisualizer, c2, Color.white, 1f, false);
					flag2 = false;
				}
				else
				{
					WireUtilityNetworkLink component3 = base.GetComponent<WireUtilityNetworkLink>();
					if (component3 != null)
					{
						int cell2;
						int cell3;
						component3.GetCells(out cell2, out cell3);
						this.DrawUtilityIcon(cell2, (Game.Instance.circuitManager.GetCircuitID(cell2) == ushort.MaxValue) ? this.resources.electricityBridgeIcon : this.resources.electricityConnectedIcon, ref this.inputVisualizer, this.resources.electricityInputColor, Color.white, 1f, false);
						this.DrawUtilityIcon(cell3, (Game.Instance.circuitManager.GetCircuitID(cell3) == ushort.MaxValue) ? this.resources.electricityBridgeIcon : this.resources.electricityConnectedIcon, ref this.outputVisualizer, this.resources.electricityInputColor, Color.white, 1f, false);
						flag2 = false;
					}
				}
				if (flag2)
				{
					this.DisableIcons();
					return;
				}
			}
		}
		else if (mode == OverlayModes.GasConduits.ID)
		{
			if (!this.RequiresGas && (this.secondary_ports & (BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut)) == (BuildingCellVisualizer.Ports)0)
			{
				this.DisableIcons();
				return;
			}
			if ((this.ports & BuildingCellVisualizer.Ports.GasIn) != (BuildingCellVisualizer.Ports)0)
			{
				bool flag3 = null != Grid.Objects[this.building.GetUtilityInputCell(), 12];
				BuildingCellVisualizerResources.ConnectedDisconnectedColours input = this.resources.gasIOColours.input;
				Color tint2 = flag3 ? input.connected : input.disconnected;
				this.DrawUtilityIcon(this.building.GetUtilityInputCell(), this.resources.gasInputIcon, ref this.inputVisualizer, tint2);
			}
			if ((this.ports & BuildingCellVisualizer.Ports.GasOut) != (BuildingCellVisualizer.Ports)0)
			{
				bool flag4 = null != Grid.Objects[this.building.GetUtilityOutputCell(), 12];
				BuildingCellVisualizerResources.ConnectedDisconnectedColours output = this.resources.gasIOColours.output;
				Color tint3 = flag4 ? output.connected : output.disconnected;
				this.DrawUtilityIcon(this.building.GetUtilityOutputCell(), this.resources.gasOutputIcon, ref this.outputVisualizer, tint3);
			}
			if ((this.secondary_ports & BuildingCellVisualizer.Ports.GasIn) != (BuildingCellVisualizer.Ports)0)
			{
				ISecondaryInput[] components = this.building.GetComponents<ISecondaryInput>();
				CellOffset cellOffset = CellOffset.none;
				ISecondaryInput[] array = components;
				for (int i = 0; i < array.Length; i++)
				{
					cellOffset = array[i].GetSecondaryConduitOffset(ConduitType.Gas);
					if (cellOffset != CellOffset.none)
					{
						break;
					}
				}
				Color tint4 = BuildingCellVisualizer.secondInputColour;
				if ((this.ports & BuildingCellVisualizer.Ports.GasIn) == (BuildingCellVisualizer.Ports)0)
				{
					bool flag5 = null != Grid.Objects[Grid.OffsetCell(Grid.PosToCell(this.building.transform.GetPosition()), cellOffset), 12];
					BuildingCellVisualizerResources.ConnectedDisconnectedColours input2 = this.resources.gasIOColours.input;
					tint4 = (flag5 ? input2.connected : input2.disconnected);
				}
				int visualizerCell = this.GetVisualizerCell(this.building, cellOffset);
				this.DrawUtilityIcon(visualizerCell, this.resources.gasInputIcon, ref this.secondaryInputVisualizer, tint4, Color.white, 1.5f, false);
			}
			if ((this.secondary_ports & BuildingCellVisualizer.Ports.GasOut) != (BuildingCellVisualizer.Ports)0)
			{
				ISecondaryOutput[] components2 = this.building.GetComponents<ISecondaryOutput>();
				CellOffset cellOffset2 = CellOffset.none;
				ISecondaryOutput[] array2 = components2;
				for (int i = 0; i < array2.Length; i++)
				{
					cellOffset2 = array2[i].GetSecondaryConduitOffset(ConduitType.Gas);
					if (cellOffset2 != CellOffset.none)
					{
						break;
					}
				}
				Color tint5 = BuildingCellVisualizer.secondOutputColour;
				if ((this.ports & BuildingCellVisualizer.Ports.GasOut) == (BuildingCellVisualizer.Ports)0)
				{
					bool flag6 = null != Grid.Objects[Grid.OffsetCell(Grid.PosToCell(this.building.transform.GetPosition()), cellOffset2), 12];
					BuildingCellVisualizerResources.ConnectedDisconnectedColours output2 = this.resources.gasIOColours.output;
					tint5 = (flag6 ? output2.connected : output2.disconnected);
				}
				int visualizerCell2 = this.GetVisualizerCell(this.building, cellOffset2);
				this.DrawUtilityIcon(visualizerCell2, this.resources.gasOutputIcon, ref this.secondaryOutputVisualizer, tint5, Color.white, 1.5f, false);
				return;
			}
		}
		else if (mode == OverlayModes.LiquidConduits.ID)
		{
			if (!this.RequiresLiquid && (this.secondary_ports & (BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut)) == (BuildingCellVisualizer.Ports)0)
			{
				this.DisableIcons();
				return;
			}
			if ((this.ports & BuildingCellVisualizer.Ports.LiquidIn) != (BuildingCellVisualizer.Ports)0)
			{
				bool flag7 = null != Grid.Objects[this.building.GetUtilityInputCell(), 16];
				BuildingCellVisualizerResources.ConnectedDisconnectedColours input3 = this.resources.liquidIOColours.input;
				Color tint6 = flag7 ? input3.connected : input3.disconnected;
				this.DrawUtilityIcon(this.building.GetUtilityInputCell(), this.resources.liquidInputIcon, ref this.inputVisualizer, tint6);
			}
			if ((this.ports & BuildingCellVisualizer.Ports.LiquidOut) != (BuildingCellVisualizer.Ports)0)
			{
				bool flag8 = null != Grid.Objects[this.building.GetUtilityOutputCell(), 16];
				BuildingCellVisualizerResources.ConnectedDisconnectedColours output3 = this.resources.liquidIOColours.output;
				Color tint7 = flag8 ? output3.connected : output3.disconnected;
				this.DrawUtilityIcon(this.building.GetUtilityOutputCell(), this.resources.liquidOutputIcon, ref this.outputVisualizer, tint7);
			}
			if ((this.secondary_ports & BuildingCellVisualizer.Ports.LiquidIn) != (BuildingCellVisualizer.Ports)0)
			{
				ISecondaryInput[] components3 = this.building.GetComponents<ISecondaryInput>();
				CellOffset cellOffset3 = CellOffset.none;
				ISecondaryInput[] array = components3;
				for (int i = 0; i < array.Length; i++)
				{
					cellOffset3 = array[i].GetSecondaryConduitOffset(ConduitType.Liquid);
					if (cellOffset3 != CellOffset.none)
					{
						break;
					}
				}
				Color tint8 = BuildingCellVisualizer.secondInputColour;
				if ((this.ports & BuildingCellVisualizer.Ports.LiquidIn) == (BuildingCellVisualizer.Ports)0)
				{
					bool flag9 = null != Grid.Objects[Grid.OffsetCell(Grid.PosToCell(this.building.transform.GetPosition()), cellOffset3), 16];
					BuildingCellVisualizerResources.ConnectedDisconnectedColours input4 = this.resources.liquidIOColours.input;
					tint8 = (flag9 ? input4.connected : input4.disconnected);
				}
				int visualizerCell3 = this.GetVisualizerCell(this.building, cellOffset3);
				this.DrawUtilityIcon(visualizerCell3, this.resources.liquidInputIcon, ref this.secondaryInputVisualizer, tint8, Color.white, 1.5f, false);
			}
			if ((this.secondary_ports & BuildingCellVisualizer.Ports.LiquidOut) != (BuildingCellVisualizer.Ports)0)
			{
				ISecondaryOutput[] components4 = this.building.GetComponents<ISecondaryOutput>();
				CellOffset cellOffset4 = CellOffset.none;
				ISecondaryOutput[] array2 = components4;
				for (int i = 0; i < array2.Length; i++)
				{
					cellOffset4 = array2[i].GetSecondaryConduitOffset(ConduitType.Liquid);
					if (cellOffset4 != CellOffset.none)
					{
						break;
					}
				}
				Color tint9 = BuildingCellVisualizer.secondOutputColour;
				if ((this.ports & BuildingCellVisualizer.Ports.LiquidOut) == (BuildingCellVisualizer.Ports)0)
				{
					bool flag10 = null != Grid.Objects[Grid.OffsetCell(Grid.PosToCell(this.building.transform.GetPosition()), cellOffset4), 16];
					BuildingCellVisualizerResources.ConnectedDisconnectedColours output4 = this.resources.liquidIOColours.output;
					tint9 = (flag10 ? output4.connected : output4.disconnected);
				}
				int visualizerCell4 = this.GetVisualizerCell(this.building, cellOffset4);
				this.DrawUtilityIcon(visualizerCell4, this.resources.liquidOutputIcon, ref this.secondaryOutputVisualizer, tint9, Color.white, 1.5f, false);
				return;
			}
		}
		else if (mode == OverlayModes.SolidConveyor.ID)
		{
			if (!this.RequiresSolid && (this.secondary_ports & (BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut)) == (BuildingCellVisualizer.Ports)0)
			{
				this.DisableIcons();
				return;
			}
			if ((this.ports & BuildingCellVisualizer.Ports.SolidIn) != (BuildingCellVisualizer.Ports)0)
			{
				bool flag11 = null != Grid.Objects[this.building.GetUtilityInputCell(), 20];
				BuildingCellVisualizerResources.ConnectedDisconnectedColours input5 = this.resources.liquidIOColours.input;
				Color tint10 = flag11 ? input5.connected : input5.disconnected;
				this.DrawUtilityIcon(this.building.GetUtilityInputCell(), this.resources.liquidInputIcon, ref this.inputVisualizer, tint10);
			}
			if ((this.ports & BuildingCellVisualizer.Ports.SolidOut) != (BuildingCellVisualizer.Ports)0)
			{
				bool flag12 = null != Grid.Objects[this.building.GetUtilityOutputCell(), 20];
				BuildingCellVisualizerResources.ConnectedDisconnectedColours output5 = this.resources.liquidIOColours.output;
				Color tint11 = flag12 ? output5.connected : output5.disconnected;
				this.DrawUtilityIcon(this.building.GetUtilityOutputCell(), this.resources.liquidOutputIcon, ref this.outputVisualizer, tint11);
			}
			if ((this.secondary_ports & BuildingCellVisualizer.Ports.SolidIn) != (BuildingCellVisualizer.Ports)0)
			{
				ISecondaryInput[] components5 = this.building.GetComponents<ISecondaryInput>();
				CellOffset cellOffset5 = CellOffset.none;
				ISecondaryInput[] array = components5;
				for (int i = 0; i < array.Length; i++)
				{
					cellOffset5 = array[i].GetSecondaryConduitOffset(ConduitType.Solid);
					if (cellOffset5 != CellOffset.none)
					{
						break;
					}
				}
				Color tint12 = BuildingCellVisualizer.secondInputColour;
				if ((this.ports & BuildingCellVisualizer.Ports.SolidIn) == (BuildingCellVisualizer.Ports)0)
				{
					bool flag13 = null != Grid.Objects[Grid.OffsetCell(Grid.PosToCell(this.building.transform.GetPosition()), cellOffset5), 20];
					BuildingCellVisualizerResources.ConnectedDisconnectedColours input6 = this.resources.liquidIOColours.input;
					tint12 = (flag13 ? input6.connected : input6.disconnected);
				}
				int visualizerCell5 = this.GetVisualizerCell(this.building, cellOffset5);
				this.DrawUtilityIcon(visualizerCell5, this.resources.liquidInputIcon, ref this.secondaryInputVisualizer, tint12, Color.white, 1.5f, false);
			}
			if ((this.secondary_ports & BuildingCellVisualizer.Ports.SolidOut) != (BuildingCellVisualizer.Ports)0)
			{
				ISecondaryOutput[] components6 = this.building.GetComponents<ISecondaryOutput>();
				CellOffset cellOffset6 = CellOffset.none;
				ISecondaryOutput[] array2 = components6;
				for (int i = 0; i < array2.Length; i++)
				{
					cellOffset6 = array2[i].GetSecondaryConduitOffset(ConduitType.Solid);
					if (cellOffset6 != CellOffset.none)
					{
						break;
					}
				}
				Color tint13 = BuildingCellVisualizer.secondOutputColour;
				if ((this.ports & BuildingCellVisualizer.Ports.SolidOut) == (BuildingCellVisualizer.Ports)0)
				{
					bool flag14 = null != Grid.Objects[Grid.OffsetCell(Grid.PosToCell(this.building.transform.GetPosition()), cellOffset6), 20];
					BuildingCellVisualizerResources.ConnectedDisconnectedColours output6 = this.resources.liquidIOColours.output;
					tint13 = (flag14 ? output6.connected : output6.disconnected);
				}
				int visualizerCell6 = this.GetVisualizerCell(this.building, cellOffset6);
				this.DrawUtilityIcon(visualizerCell6, this.resources.liquidOutputIcon, ref this.secondaryOutputVisualizer, tint13, Color.white, 1.5f, false);
				return;
			}
		}
		else if (mode == OverlayModes.Disease.ID)
		{
			if (this.diseaseSourceSprite != null)
			{
				int utilityOutputCell = this.building.GetUtilityOutputCell();
				this.DrawUtilityIcon(utilityOutputCell, this.diseaseSourceSprite, ref this.inputVisualizer, this.diseaseSourceColour);
				return;
			}
		}
		else if (mode == OverlayModes.Radiation.ID && this.RequiresHighEnergyParticles)
		{
			int num = 3;
			if (this.building.Def.UseHighEnergyParticleInputPort)
			{
				int highEnergyParticleInputCell = this.building.GetHighEnergyParticleInputCell();
				this.DrawUtilityIcon(highEnergyParticleInputCell, this.resources.highEnergyParticleInputIcon, ref this.inputVisualizer, this.resources.highEnergyParticleInputColour, Color.white, (float)num, true);
			}
			if (this.building.Def.UseHighEnergyParticleOutputPort)
			{
				int highEnergyParticleOutputCell = this.building.GetHighEnergyParticleOutputCell();
				IHighEnergyParticleDirection component4 = this.building.GetComponent<IHighEnergyParticleDirection>();
				Sprite icon_img3 = this.resources.highEnergyParticleOutputIcons[0];
				if (component4 != null)
				{
					int directionIndex = EightDirectionUtil.GetDirectionIndex(component4.Direction);
					icon_img3 = this.resources.highEnergyParticleOutputIcons[directionIndex];
				}
				this.DrawUtilityIcon(highEnergyParticleOutputCell, icon_img3, ref this.outputVisualizer, this.resources.highEnergyParticleOutputColour, Color.white, (float)num, true);
			}
		}
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x000868BC File Offset: 0x00084ABC
	private int GetVisualizerCell(Building building, CellOffset offset)
	{
		CellOffset rotatedOffset = building.GetRotatedOffset(offset);
		return Grid.OffsetCell(building.GetCell(), rotatedOffset);
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x000868E0 File Offset: 0x00084AE0
	public void DisableIcons()
	{
		if (this.inputVisualizer != null && this.inputVisualizer.activeInHierarchy)
		{
			this.inputVisualizer.SetActive(false);
		}
		if (this.outputVisualizer != null && this.outputVisualizer.activeInHierarchy)
		{
			this.outputVisualizer.SetActive(false);
		}
		if (this.secondaryInputVisualizer != null && this.secondaryInputVisualizer.activeInHierarchy)
		{
			this.secondaryInputVisualizer.SetActive(false);
		}
		if (this.secondaryOutputVisualizer != null && this.secondaryOutputVisualizer.activeInHierarchy)
		{
			this.secondaryOutputVisualizer.SetActive(false);
		}
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x00086989 File Offset: 0x00084B89
	private void DrawUtilityIcon(int cell, Sprite icon_img, ref GameObject visualizerObj)
	{
		this.DrawUtilityIcon(cell, icon_img, ref visualizerObj, Color.white, Color.white, 1.5f, false);
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x000869A4 File Offset: 0x00084BA4
	private void DrawUtilityIcon(int cell, Sprite icon_img, ref GameObject visualizerObj, Color tint)
	{
		this.DrawUtilityIcon(cell, icon_img, ref visualizerObj, tint, Color.white, 1.5f, false);
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x000869BC File Offset: 0x00084BBC
	private void DrawUtilityIcon(int cell, Sprite icon_img, ref GameObject visualizerObj, Color tint, Color connectorColor, float scaleMultiplier = 1.5f, bool hideBG = false)
	{
		Vector3 position = Grid.CellToPosCCC(cell, Grid.SceneLayer.Building);
		if (visualizerObj == null)
		{
			visualizerObj = global::Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, null);
			visualizerObj.transform.SetAsFirstSibling();
			this.icons.Add(visualizerObj, visualizerObj.transform.GetChild(0).GetComponent<Image>());
		}
		if (!visualizerObj.gameObject.activeInHierarchy)
		{
			visualizerObj.gameObject.SetActive(true);
		}
		visualizerObj.GetComponent<Image>().enabled = !hideBG;
		this.icons[visualizerObj].raycastTarget = this.enableRaycast;
		this.icons[visualizerObj].sprite = icon_img;
		visualizerObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = tint;
		visualizerObj.transform.SetPosition(position);
		if (visualizerObj.GetComponent<SizePulse>() == null)
		{
			visualizerObj.transform.localScale = Vector3.one * scaleMultiplier;
		}
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x00086ACD File Offset: 0x00084CCD
	public Image GetOutputIcon()
	{
		if (!(this.outputVisualizer == null))
		{
			return this.outputVisualizer.transform.GetChild(0).GetComponent<Image>();
		}
		return null;
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x00086AF5 File Offset: 0x00084CF5
	public Image GetInputIcon()
	{
		if (!(this.inputVisualizer == null))
		{
			return this.inputVisualizer.transform.GetChild(0).GetComponent<Image>();
		}
		return null;
	}

	// Token: 0x04000E19 RID: 3609
	private BuildingCellVisualizerResources resources;

	// Token: 0x04000E1A RID: 3610
	[MyCmpReq]
	private Building building;

	// Token: 0x04000E1B RID: 3611
	[SerializeField]
	public static Color32 secondOutputColour = new Color(0.9843137f, 0.6901961f, 0.23137255f);

	// Token: 0x04000E1C RID: 3612
	[SerializeField]
	public static Color32 secondInputColour = new Color(0.9843137f, 0.6901961f, 0.23137255f);

	// Token: 0x04000E1D RID: 3613
	private const BuildingCellVisualizer.Ports POWER_PORTS = BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut;

	// Token: 0x04000E1E RID: 3614
	private const BuildingCellVisualizer.Ports GAS_PORTS = BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut;

	// Token: 0x04000E1F RID: 3615
	private const BuildingCellVisualizer.Ports LIQUID_PORTS = BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut;

	// Token: 0x04000E20 RID: 3616
	private const BuildingCellVisualizer.Ports SOLID_PORTS = BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut;

	// Token: 0x04000E21 RID: 3617
	private const BuildingCellVisualizer.Ports MATTER_PORTS = BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut;

	// Token: 0x04000E22 RID: 3618
	private BuildingCellVisualizer.Ports ports;

	// Token: 0x04000E23 RID: 3619
	private BuildingCellVisualizer.Ports secondary_ports;

	// Token: 0x04000E24 RID: 3620
	private Sprite diseaseSourceSprite;

	// Token: 0x04000E25 RID: 3621
	private Color32 diseaseSourceColour;

	// Token: 0x04000E26 RID: 3622
	private GameObject inputVisualizer;

	// Token: 0x04000E27 RID: 3623
	private GameObject outputVisualizer;

	// Token: 0x04000E28 RID: 3624
	private GameObject secondaryInputVisualizer;

	// Token: 0x04000E29 RID: 3625
	private GameObject secondaryOutputVisualizer;

	// Token: 0x04000E2A RID: 3626
	private bool enableRaycast;

	// Token: 0x04000E2B RID: 3627
	private Dictionary<GameObject, Image> icons;

	// Token: 0x02001101 RID: 4353
	[Flags]
	private enum Ports
	{
		// Token: 0x04005AF0 RID: 23280
		PowerIn = 1,
		// Token: 0x04005AF1 RID: 23281
		PowerOut = 2,
		// Token: 0x04005AF2 RID: 23282
		GasIn = 4,
		// Token: 0x04005AF3 RID: 23283
		GasOut = 8,
		// Token: 0x04005AF4 RID: 23284
		LiquidIn = 16,
		// Token: 0x04005AF5 RID: 23285
		LiquidOut = 32,
		// Token: 0x04005AF6 RID: 23286
		SolidIn = 64,
		// Token: 0x04005AF7 RID: 23287
		SolidOut = 128,
		// Token: 0x04005AF8 RID: 23288
		HighEnergyParticle = 256
	}
}
