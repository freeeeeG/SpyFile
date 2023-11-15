using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x020007AD RID: 1965
public class CellEventLogger : EventLogger<CellEventInstance, CellEvent>
{
	// Token: 0x0600368A RID: 13962 RVA: 0x001264AD File Offset: 0x001246AD
	public static void DestroyInstance()
	{
		CellEventLogger.Instance = null;
	}

	// Token: 0x0600368B RID: 13963 RVA: 0x001264B5 File Offset: 0x001246B5
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void LogCallbackSend(int cell, int callback_id)
	{
		if (callback_id != -1)
		{
			this.CallbackToCellMap[callback_id] = cell;
		}
	}

	// Token: 0x0600368C RID: 13964 RVA: 0x001264C8 File Offset: 0x001246C8
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void LogCallbackReceive(int callback_id)
	{
		int invalidCell = Grid.InvalidCell;
		this.CallbackToCellMap.TryGetValue(callback_id, out invalidCell);
	}

	// Token: 0x0600368D RID: 13965 RVA: 0x001264EC File Offset: 0x001246EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CellEventLogger.Instance = this;
		this.SimMessagesSolid = (base.AddEvent(new CellSolidEvent("SimMessageSolid", "Sim Message", false, true)) as CellSolidEvent);
		this.SimCellOccupierDestroy = (base.AddEvent(new CellSolidEvent("SimCellOccupierClearSolid", "Sim Cell Occupier Destroy", false, true)) as CellSolidEvent);
		this.SimCellOccupierForceSolid = (base.AddEvent(new CellSolidEvent("SimCellOccupierForceSolid", "Sim Cell Occupier Force Solid", false, true)) as CellSolidEvent);
		this.SimCellOccupierSolidChanged = (base.AddEvent(new CellSolidEvent("SimCellOccupierSolidChanged", "Sim Cell Occupier Solid Changed", false, true)) as CellSolidEvent);
		this.DoorOpen = (base.AddEvent(new CellElementEvent("DoorOpen", "Door Open", true, true)) as CellElementEvent);
		this.DoorClose = (base.AddEvent(new CellElementEvent("DoorClose", "Door Close", true, true)) as CellElementEvent);
		this.Excavator = (base.AddEvent(new CellElementEvent("Excavator", "Excavator", true, true)) as CellElementEvent);
		this.DebugTool = (base.AddEvent(new CellElementEvent("DebugTool", "Debug Tool", true, true)) as CellElementEvent);
		this.SandBoxTool = (base.AddEvent(new CellElementEvent("SandBoxTool", "Sandbox Tool", true, true)) as CellElementEvent);
		this.TemplateLoader = (base.AddEvent(new CellElementEvent("TemplateLoader", "Template Loader", true, true)) as CellElementEvent);
		this.Scenario = (base.AddEvent(new CellElementEvent("Scenario", "Scenario", true, true)) as CellElementEvent);
		this.SimCellOccupierOnSpawn = (base.AddEvent(new CellElementEvent("SimCellOccupierOnSpawn", "Sim Cell Occupier OnSpawn", true, true)) as CellElementEvent);
		this.SimCellOccupierDestroySelf = (base.AddEvent(new CellElementEvent("SimCellOccupierDestroySelf", "Sim Cell Occupier Destroy Self", true, true)) as CellElementEvent);
		this.WorldGapManager = (base.AddEvent(new CellElementEvent("WorldGapManager", "World Gap Manager", true, true)) as CellElementEvent);
		this.ReceiveElementChanged = (base.AddEvent(new CellElementEvent("ReceiveElementChanged", "Sim Message", false, false)) as CellElementEvent);
		this.ObjectSetSimOnSpawn = (base.AddEvent(new CellElementEvent("ObjectSetSimOnSpawn", "Object set sim on spawn", true, true)) as CellElementEvent);
		this.DecompositionDirtyWater = (base.AddEvent(new CellElementEvent("DecompositionDirtyWater", "Decomposition dirty water", true, true)) as CellElementEvent);
		this.SendCallback = (base.AddEvent(new CellCallbackEvent("SendCallback", true, true)) as CellCallbackEvent);
		this.ReceiveCallback = (base.AddEvent(new CellCallbackEvent("ReceiveCallback", false, true)) as CellCallbackEvent);
		this.Dig = (base.AddEvent(new CellDigEvent(true)) as CellDigEvent);
		this.WorldDamageDelayedSpawnFX = (base.AddEvent(new CellAddRemoveSubstanceEvent("WorldDamageDelayedSpawnFX", "World Damage Delayed Spawn FX", false)) as CellAddRemoveSubstanceEvent);
		this.OxygenModifierSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("OxygenModifierSimUpdate", "Oxygen Modifier SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.LiquidChunkOnStore = (base.AddEvent(new CellAddRemoveSubstanceEvent("LiquidChunkOnStore", "Liquid Chunk On Store", false)) as CellAddRemoveSubstanceEvent);
		this.FallingWaterAddToSim = (base.AddEvent(new CellAddRemoveSubstanceEvent("FallingWaterAddToSim", "Falling Water Add To Sim", false)) as CellAddRemoveSubstanceEvent);
		this.ExploderOnSpawn = (base.AddEvent(new CellAddRemoveSubstanceEvent("ExploderOnSpawn", "Exploder OnSpawn", false)) as CellAddRemoveSubstanceEvent);
		this.ExhaustSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("ExhaustSimUpdate", "Exhaust SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.ElementConsumerSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementConsumerSimUpdate", "Element Consumer SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.SublimatesEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("SublimatesEmit", "Sublimates Emit", false)) as CellAddRemoveSubstanceEvent);
		this.Mop = (base.AddEvent(new CellAddRemoveSubstanceEvent("Mop", "Mop", false)) as CellAddRemoveSubstanceEvent);
		this.OreMelted = (base.AddEvent(new CellAddRemoveSubstanceEvent("OreMelted", "Ore Melted", false)) as CellAddRemoveSubstanceEvent);
		this.ConstructTile = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConstructTile", "ConstructTile", false)) as CellAddRemoveSubstanceEvent);
		this.Dumpable = (base.AddEvent(new CellAddRemoveSubstanceEvent("Dympable", "Dumpable", false)) as CellAddRemoveSubstanceEvent);
		this.Cough = (base.AddEvent(new CellAddRemoveSubstanceEvent("Cough", "Cough", false)) as CellAddRemoveSubstanceEvent);
		this.Meteor = (base.AddEvent(new CellAddRemoveSubstanceEvent("Meteor", "Meteor", false)) as CellAddRemoveSubstanceEvent);
		this.ElementChunkTransition = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementChunkTransition", "Element Chunk Transition", false)) as CellAddRemoveSubstanceEvent);
		this.OxyrockEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("OxyrockEmit", "Oxyrock Emit", false)) as CellAddRemoveSubstanceEvent);
		this.BleachstoneEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("BleachstoneEmit", "Bleachstone Emit", false)) as CellAddRemoveSubstanceEvent);
		this.UnstableGround = (base.AddEvent(new CellAddRemoveSubstanceEvent("UnstableGround", "Unstable Ground", false)) as CellAddRemoveSubstanceEvent);
		this.ConduitFlowEmptyConduit = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConduitFlowEmptyConduit", "Conduit Flow Empty Conduit", false)) as CellAddRemoveSubstanceEvent);
		this.ConduitConsumerWrongElement = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConduitConsumerWrongElement", "Conduit Consumer Wrong Element", false)) as CellAddRemoveSubstanceEvent);
		this.OverheatableMeltingDown = (base.AddEvent(new CellAddRemoveSubstanceEvent("OverheatableMeltingDown", "Overheatable MeltingDown", false)) as CellAddRemoveSubstanceEvent);
		this.FabricatorProduceMelted = (base.AddEvent(new CellAddRemoveSubstanceEvent("FabricatorProduceMelted", "Fabricator Produce Melted", false)) as CellAddRemoveSubstanceEvent);
		this.PumpSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("PumpSimUpdate", "Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.WallPumpSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("WallPumpSimUpdate", "Wall Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.Vomit = (base.AddEvent(new CellAddRemoveSubstanceEvent("Vomit", "Vomit", false)) as CellAddRemoveSubstanceEvent);
		this.Tears = (base.AddEvent(new CellAddRemoveSubstanceEvent("Tears", "Tears", false)) as CellAddRemoveSubstanceEvent);
		this.Pee = (base.AddEvent(new CellAddRemoveSubstanceEvent("Pee", "Pee", false)) as CellAddRemoveSubstanceEvent);
		this.AlgaeHabitat = (base.AddEvent(new CellAddRemoveSubstanceEvent("AlgaeHabitat", "AlgaeHabitat", false)) as CellAddRemoveSubstanceEvent);
		this.CO2FilterOxygen = (base.AddEvent(new CellAddRemoveSubstanceEvent("CO2FilterOxygen", "CO2FilterOxygen", false)) as CellAddRemoveSubstanceEvent);
		this.ToiletEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("ToiletEmit", "ToiletEmit", false)) as CellAddRemoveSubstanceEvent);
		this.ElementEmitted = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementEmitted", "Element Emitted", false)) as CellAddRemoveSubstanceEvent);
		this.CO2ManagerFixedUpdate = (base.AddEvent(new CellModifyMassEvent("CO2ManagerFixedUpdate", "CO2Manager FixedUpdate", false)) as CellModifyMassEvent);
		this.EnvironmentConsumerFixedUpdate = (base.AddEvent(new CellModifyMassEvent("EnvironmentConsumerFixedUpdate", "EnvironmentConsumer FixedUpdate", false)) as CellModifyMassEvent);
		this.ExcavatorShockwave = (base.AddEvent(new CellModifyMassEvent("ExcavatorShockwave", "Excavator Shockwave", false)) as CellModifyMassEvent);
		this.OxygenBreatherSimUpdate = (base.AddEvent(new CellModifyMassEvent("OxygenBreatherSimUpdate", "Oxygen Breather SimUpdate", false)) as CellModifyMassEvent);
		this.CO2ScrubberSimUpdate = (base.AddEvent(new CellModifyMassEvent("CO2ScrubberSimUpdate", "CO2Scrubber SimUpdate", false)) as CellModifyMassEvent);
		this.RiverSourceSimUpdate = (base.AddEvent(new CellModifyMassEvent("RiverSourceSimUpdate", "RiverSource SimUpdate", false)) as CellModifyMassEvent);
		this.RiverTerminusSimUpdate = (base.AddEvent(new CellModifyMassEvent("RiverTerminusSimUpdate", "RiverTerminus SimUpdate", false)) as CellModifyMassEvent);
		this.DebugToolModifyMass = (base.AddEvent(new CellModifyMassEvent("DebugToolModifyMass", "DebugTool ModifyMass", false)) as CellModifyMassEvent);
		this.EnergyGeneratorModifyMass = (base.AddEvent(new CellModifyMassEvent("EnergyGeneratorModifyMass", "EnergyGenerator ModifyMass", false)) as CellModifyMassEvent);
		this.SolidFilterEvent = (base.AddEvent(new CellSolidFilterEvent("SolidFilterEvent", true)) as CellSolidFilterEvent);
	}

	// Token: 0x04002146 RID: 8518
	public static CellEventLogger Instance;

	// Token: 0x04002147 RID: 8519
	public CellSolidEvent SimMessagesSolid;

	// Token: 0x04002148 RID: 8520
	public CellSolidEvent SimCellOccupierDestroy;

	// Token: 0x04002149 RID: 8521
	public CellSolidEvent SimCellOccupierForceSolid;

	// Token: 0x0400214A RID: 8522
	public CellSolidEvent SimCellOccupierSolidChanged;

	// Token: 0x0400214B RID: 8523
	public CellElementEvent DoorOpen;

	// Token: 0x0400214C RID: 8524
	public CellElementEvent DoorClose;

	// Token: 0x0400214D RID: 8525
	public CellElementEvent Excavator;

	// Token: 0x0400214E RID: 8526
	public CellElementEvent DebugTool;

	// Token: 0x0400214F RID: 8527
	public CellElementEvent SandBoxTool;

	// Token: 0x04002150 RID: 8528
	public CellElementEvent TemplateLoader;

	// Token: 0x04002151 RID: 8529
	public CellElementEvent Scenario;

	// Token: 0x04002152 RID: 8530
	public CellElementEvent SimCellOccupierOnSpawn;

	// Token: 0x04002153 RID: 8531
	public CellElementEvent SimCellOccupierDestroySelf;

	// Token: 0x04002154 RID: 8532
	public CellElementEvent WorldGapManager;

	// Token: 0x04002155 RID: 8533
	public CellElementEvent ReceiveElementChanged;

	// Token: 0x04002156 RID: 8534
	public CellElementEvent ObjectSetSimOnSpawn;

	// Token: 0x04002157 RID: 8535
	public CellElementEvent DecompositionDirtyWater;

	// Token: 0x04002158 RID: 8536
	public CellElementEvent LaunchpadDesolidify;

	// Token: 0x04002159 RID: 8537
	public CellCallbackEvent SendCallback;

	// Token: 0x0400215A RID: 8538
	public CellCallbackEvent ReceiveCallback;

	// Token: 0x0400215B RID: 8539
	public CellDigEvent Dig;

	// Token: 0x0400215C RID: 8540
	public CellAddRemoveSubstanceEvent WorldDamageDelayedSpawnFX;

	// Token: 0x0400215D RID: 8541
	public CellAddRemoveSubstanceEvent SublimatesEmit;

	// Token: 0x0400215E RID: 8542
	public CellAddRemoveSubstanceEvent OxygenModifierSimUpdate;

	// Token: 0x0400215F RID: 8543
	public CellAddRemoveSubstanceEvent LiquidChunkOnStore;

	// Token: 0x04002160 RID: 8544
	public CellAddRemoveSubstanceEvent FallingWaterAddToSim;

	// Token: 0x04002161 RID: 8545
	public CellAddRemoveSubstanceEvent ExploderOnSpawn;

	// Token: 0x04002162 RID: 8546
	public CellAddRemoveSubstanceEvent ExhaustSimUpdate;

	// Token: 0x04002163 RID: 8547
	public CellAddRemoveSubstanceEvent ElementConsumerSimUpdate;

	// Token: 0x04002164 RID: 8548
	public CellAddRemoveSubstanceEvent ElementChunkTransition;

	// Token: 0x04002165 RID: 8549
	public CellAddRemoveSubstanceEvent OxyrockEmit;

	// Token: 0x04002166 RID: 8550
	public CellAddRemoveSubstanceEvent BleachstoneEmit;

	// Token: 0x04002167 RID: 8551
	public CellAddRemoveSubstanceEvent UnstableGround;

	// Token: 0x04002168 RID: 8552
	public CellAddRemoveSubstanceEvent ConduitFlowEmptyConduit;

	// Token: 0x04002169 RID: 8553
	public CellAddRemoveSubstanceEvent ConduitConsumerWrongElement;

	// Token: 0x0400216A RID: 8554
	public CellAddRemoveSubstanceEvent OverheatableMeltingDown;

	// Token: 0x0400216B RID: 8555
	public CellAddRemoveSubstanceEvent FabricatorProduceMelted;

	// Token: 0x0400216C RID: 8556
	public CellAddRemoveSubstanceEvent PumpSimUpdate;

	// Token: 0x0400216D RID: 8557
	public CellAddRemoveSubstanceEvent WallPumpSimUpdate;

	// Token: 0x0400216E RID: 8558
	public CellAddRemoveSubstanceEvent Vomit;

	// Token: 0x0400216F RID: 8559
	public CellAddRemoveSubstanceEvent Tears;

	// Token: 0x04002170 RID: 8560
	public CellAddRemoveSubstanceEvent Pee;

	// Token: 0x04002171 RID: 8561
	public CellAddRemoveSubstanceEvent AlgaeHabitat;

	// Token: 0x04002172 RID: 8562
	public CellAddRemoveSubstanceEvent CO2FilterOxygen;

	// Token: 0x04002173 RID: 8563
	public CellAddRemoveSubstanceEvent ToiletEmit;

	// Token: 0x04002174 RID: 8564
	public CellAddRemoveSubstanceEvent ElementEmitted;

	// Token: 0x04002175 RID: 8565
	public CellAddRemoveSubstanceEvent Mop;

	// Token: 0x04002176 RID: 8566
	public CellAddRemoveSubstanceEvent OreMelted;

	// Token: 0x04002177 RID: 8567
	public CellAddRemoveSubstanceEvent ConstructTile;

	// Token: 0x04002178 RID: 8568
	public CellAddRemoveSubstanceEvent Dumpable;

	// Token: 0x04002179 RID: 8569
	public CellAddRemoveSubstanceEvent Cough;

	// Token: 0x0400217A RID: 8570
	public CellAddRemoveSubstanceEvent Meteor;

	// Token: 0x0400217B RID: 8571
	public CellModifyMassEvent CO2ManagerFixedUpdate;

	// Token: 0x0400217C RID: 8572
	public CellModifyMassEvent EnvironmentConsumerFixedUpdate;

	// Token: 0x0400217D RID: 8573
	public CellModifyMassEvent ExcavatorShockwave;

	// Token: 0x0400217E RID: 8574
	public CellModifyMassEvent OxygenBreatherSimUpdate;

	// Token: 0x0400217F RID: 8575
	public CellModifyMassEvent CO2ScrubberSimUpdate;

	// Token: 0x04002180 RID: 8576
	public CellModifyMassEvent RiverSourceSimUpdate;

	// Token: 0x04002181 RID: 8577
	public CellModifyMassEvent RiverTerminusSimUpdate;

	// Token: 0x04002182 RID: 8578
	public CellModifyMassEvent DebugToolModifyMass;

	// Token: 0x04002183 RID: 8579
	public CellModifyMassEvent EnergyGeneratorModifyMass;

	// Token: 0x04002184 RID: 8580
	public CellSolidFilterEvent SolidFilterEvent;

	// Token: 0x04002185 RID: 8581
	public Dictionary<int, int> CallbackToCellMap = new Dictionary<int, int>();
}
