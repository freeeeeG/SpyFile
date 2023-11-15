using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000972 RID: 2418
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SimTemperatureTransfer")]
public class SimTemperatureTransfer : KMonoBehaviour
{
	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06004726 RID: 18214 RVA: 0x00192107 File Offset: 0x00190307
	// (set) Token: 0x06004727 RID: 18215 RVA: 0x0019210F File Offset: 0x0019030F
	public float SurfaceArea
	{
		get
		{
			return this.surfaceArea;
		}
		set
		{
			this.surfaceArea = value;
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06004728 RID: 18216 RVA: 0x00192118 File Offset: 0x00190318
	// (set) Token: 0x06004729 RID: 18217 RVA: 0x00192120 File Offset: 0x00190320
	public float Thickness
	{
		get
		{
			return this.thickness;
		}
		set
		{
			this.thickness = value;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x0600472A RID: 18218 RVA: 0x00192129 File Offset: 0x00190329
	// (set) Token: 0x0600472B RID: 18219 RVA: 0x00192131 File Offset: 0x00190331
	public float GroundTransferScale
	{
		get
		{
			return this.GroundTransferScale;
		}
		set
		{
			this.groundTransferScale = value;
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x0600472C RID: 18220 RVA: 0x0019213A File Offset: 0x0019033A
	public int SimHandle
	{
		get
		{
			return this.simHandle;
		}
	}

	// Token: 0x0600472D RID: 18221 RVA: 0x00192142 File Offset: 0x00190342
	public static void ClearInstanceMap()
	{
		SimTemperatureTransfer.handleInstanceMap.Clear();
	}

	// Token: 0x0600472E RID: 18222 RVA: 0x00192150 File Offset: 0x00190350
	public static void DoOreMeltTransition(int sim_handle)
	{
		SimTemperatureTransfer simTemperatureTransfer = null;
		if (!SimTemperatureTransfer.handleInstanceMap.TryGetValue(sim_handle, out simTemperatureTransfer))
		{
			return;
		}
		if (simTemperatureTransfer == null)
		{
			return;
		}
		if (simTemperatureTransfer.HasTag(GameTags.Sealed))
		{
			return;
		}
		PrimaryElement component = simTemperatureTransfer.GetComponent<PrimaryElement>();
		Element element = component.Element;
		bool flag = component.Temperature >= element.highTemp;
		bool flag2 = component.Temperature <= element.lowTemp;
		DebugUtil.DevAssert(flag || flag2, "An ore got a melt message from the sim but it's still the correct temperature for its state!", component);
		if (flag && element.highTempTransitionTarget == SimHashes.Unobtanium)
		{
			return;
		}
		if (flag2 && element.lowTempTransitionTarget == SimHashes.Unobtanium)
		{
			return;
		}
		if (component.Mass > 0f)
		{
			int gameCell = Grid.PosToCell(simTemperatureTransfer.transform.GetPosition());
			float num = component.Mass;
			int num2 = component.DiseaseCount;
			SimHashes new_element = flag ? element.highTempTransitionTarget : element.lowTempTransitionTarget;
			SimHashes simHashes = flag ? element.highTempTransitionOreID : element.lowTempTransitionOreID;
			float num3 = flag ? element.highTempTransitionOreMassConversion : element.lowTempTransitionOreMassConversion;
			if (simHashes != (SimHashes)0)
			{
				float num4 = num * num3;
				int num5 = (int)((float)num2 * num3);
				if (num4 > 0.001f)
				{
					num -= num4;
					num2 -= num5;
					Element element2 = ElementLoader.FindElementByHash(simHashes);
					if (element2.IsSolid)
					{
						GameObject obj = element2.substance.SpawnResource(simTemperatureTransfer.transform.GetPosition(), num4, component.Temperature, component.DiseaseIdx, num5, true, false, true);
						element2.substance.ActivateSubstanceGameObject(obj, component.DiseaseIdx, num5);
					}
					else
					{
						SimMessages.AddRemoveSubstance(gameCell, element2.id, CellEventLogger.Instance.OreMelted, num4, component.Temperature, component.DiseaseIdx, num5, true, -1);
					}
				}
			}
			SimMessages.AddRemoveSubstance(gameCell, new_element, CellEventLogger.Instance.OreMelted, num, component.Temperature, component.DiseaseIdx, num2, true, -1);
		}
		simTemperatureTransfer.OnCleanUp();
		Util.KDestroyGameObject(simTemperatureTransfer.gameObject);
	}

	// Token: 0x0600472F RID: 18223 RVA: 0x00192340 File Offset: 0x00190540
	protected override void OnPrefabInit()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(SimTemperatureTransfer.OnGetTemperature);
		component.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(SimTemperatureTransfer.OnSetTemperature);
		component.onDataChanged = (Action<PrimaryElement>)Delegate.Combine(component.onDataChanged, new Action<PrimaryElement>(this.OnDataChanged));
	}

	// Token: 0x06004730 RID: 18224 RVA: 0x00192398 File Offset: 0x00190598
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Element element = component.Element;
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "SimTemperatureTransfer.OnSpawn");
		if (!Grid.IsValidCell(Grid.PosToCell(this)) || component.Element.HasTag(GameTags.Special) || element.specificHeatCapacity == 0f)
		{
			base.enabled = false;
		}
		this.SimRegister();
	}

	// Token: 0x06004731 RID: 18225 RVA: 0x00192414 File Offset: 0x00190614
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.SimRegister();
		if (Sim.IsValidHandle(this.simHandle))
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			SimTemperatureTransfer.OnSetTemperature(component, component.Temperature);
		}
	}

	// Token: 0x06004732 RID: 18226 RVA: 0x00192440 File Offset: 0x00190640
	protected override void OnCmpDisable()
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			float temperature = component.Temperature;
			component.InternalTemperature = component.Temperature;
			SimMessages.SetElementChunkData(this.simHandle, temperature, 0f);
		}
		base.OnCmpDisable();
	}

	// Token: 0x06004733 RID: 18227 RVA: 0x0019248C File Offset: 0x0019068C
	private void OnCellChanged()
	{
		int cell = Grid.PosToCell(this);
		if (!Grid.IsValidCell(cell))
		{
			base.enabled = false;
			return;
		}
		this.SimRegister();
		if (Sim.IsValidHandle(this.simHandle))
		{
			SimMessages.MoveElementChunk(this.simHandle, cell);
		}
	}

	// Token: 0x06004734 RID: 18228 RVA: 0x001924CF File Offset: 0x001906CF
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
		this.SimUnregister();
		base.OnForcedCleanUp();
	}

	// Token: 0x06004735 RID: 18229 RVA: 0x001924F9 File Offset: 0x001906F9
	public void ModifyEnergy(float delta_kilojoules)
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			SimMessages.ModifyElementChunkEnergy(this.simHandle, delta_kilojoules);
			return;
		}
		this.pendingEnergyModifications += delta_kilojoules;
	}

	// Token: 0x06004736 RID: 18230 RVA: 0x00192524 File Offset: 0x00190724
	private unsafe static float OnGetTemperature(PrimaryElement primary_element)
	{
		SimTemperatureTransfer component = primary_element.GetComponent<SimTemperatureTransfer>();
		float result;
		if (Sim.IsValidHandle(component.simHandle))
		{
			int handleIndex = Sim.GetHandleIndex(component.simHandle);
			result = Game.Instance.simData.elementChunks[handleIndex].temperature;
			component.deltaKJ = Game.Instance.simData.elementChunks[handleIndex].deltaKJ;
		}
		else
		{
			result = primary_element.InternalTemperature;
		}
		return result;
	}

	// Token: 0x06004737 RID: 18231 RVA: 0x001925A0 File Offset: 0x001907A0
	private unsafe static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		if (temperature <= 0f)
		{
			KCrashReporter.Assert(false, "STT.OnSetTemperature - Tried to set <= 0 degree temperature");
			temperature = 293f;
		}
		SimTemperatureTransfer component = primary_element.GetComponent<SimTemperatureTransfer>();
		if (Sim.IsValidHandle(component.simHandle))
		{
			float mass = primary_element.Mass;
			float heat_capacity = (mass >= 0.01f) ? (mass * primary_element.Element.specificHeatCapacity) : 0f;
			SimMessages.SetElementChunkData(component.simHandle, temperature, heat_capacity);
			int handleIndex = Sim.GetHandleIndex(component.simHandle);
			Game.Instance.simData.elementChunks[handleIndex].temperature = temperature;
			return;
		}
		primary_element.InternalTemperature = temperature;
	}

	// Token: 0x06004738 RID: 18232 RVA: 0x00192640 File Offset: 0x00190840
	private void OnDataChanged(PrimaryElement primary_element)
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			float heat_capacity = (primary_element.Mass >= 0.01f) ? (primary_element.Mass * primary_element.Element.specificHeatCapacity) : 0f;
			SimMessages.SetElementChunkData(this.simHandle, primary_element.Temperature, heat_capacity);
		}
	}

	// Token: 0x06004739 RID: 18233 RVA: 0x00192694 File Offset: 0x00190894
	protected void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1 && base.enabled)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (component.Mass > 0f && !component.Element.IsTemperatureInsulated)
			{
				int gameCell = Grid.PosToCell(base.transform.GetPosition());
				this.simHandle = -2;
				HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle = Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(SimTemperatureTransfer.OnSimRegisteredCallback), this, "SimTemperatureTransfer.SimRegister");
				float num = component.InternalTemperature;
				if (num <= 0f)
				{
					component.InternalTemperature = 293f;
					num = 293f;
				}
				SimMessages.AddElementChunk(gameCell, component.ElementID, component.Mass, num, this.surfaceArea, this.thickness, this.groundTransferScale, handle.index);
			}
		}
	}

	// Token: 0x0600473A RID: 18234 RVA: 0x00192770 File Offset: 0x00190970
	protected unsafe void SimUnregister()
	{
		if (this.simHandle != -1 && !KMonoBehaviour.isLoadingScene)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (Sim.IsValidHandle(this.simHandle))
			{
				int handleIndex = Sim.GetHandleIndex(this.simHandle);
				component.InternalTemperature = Game.Instance.simData.elementChunks[handleIndex].temperature;
				SimMessages.RemoveElementChunk(this.simHandle, -1);
				SimTemperatureTransfer.handleInstanceMap.Remove(this.simHandle);
			}
			this.simHandle = -1;
		}
	}

	// Token: 0x0600473B RID: 18235 RVA: 0x001927F5 File Offset: 0x001909F5
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((SimTemperatureTransfer)data).OnSimRegistered(handle);
	}

	// Token: 0x0600473C RID: 18236 RVA: 0x00192804 File Offset: 0x00190A04
	private unsafe void OnSimRegistered(int handle)
	{
		if (this != null && this.simHandle == -2)
		{
			this.simHandle = handle;
			int handleIndex = Sim.GetHandleIndex(handle);
			if (Game.Instance.simData.elementChunks[handleIndex].temperature <= 0f)
			{
				KCrashReporter.Assert(false, "Bad temperature");
			}
			SimTemperatureTransfer.handleInstanceMap[this.simHandle] = this;
			if (this.pendingEnergyModifications > 0f)
			{
				this.ModifyEnergy(this.pendingEnergyModifications);
				this.pendingEnergyModifications = 0f;
			}
			if (this.onSimRegistered != null)
			{
				this.onSimRegistered(this);
			}
			if (!base.enabled)
			{
				this.OnCmpDisable();
				return;
			}
		}
		else
		{
			SimMessages.RemoveElementChunk(handle, -1);
		}
	}

	// Token: 0x04002F28 RID: 12072
	private const float SIM_FREEZE_SPAWN_ORE_PERCENT = 0.8f;

	// Token: 0x04002F29 RID: 12073
	public const float MIN_MASS_FOR_TEMPERATURE_TRANSFER = 0.01f;

	// Token: 0x04002F2A RID: 12074
	public float deltaKJ;

	// Token: 0x04002F2B RID: 12075
	public Action<SimTemperatureTransfer> onSimRegistered;

	// Token: 0x04002F2C RID: 12076
	protected int simHandle = -1;

	// Token: 0x04002F2D RID: 12077
	private float pendingEnergyModifications;

	// Token: 0x04002F2E RID: 12078
	[SerializeField]
	protected float surfaceArea = 10f;

	// Token: 0x04002F2F RID: 12079
	[SerializeField]
	protected float thickness = 0.01f;

	// Token: 0x04002F30 RID: 12080
	[SerializeField]
	protected float groundTransferScale = 0.0625f;

	// Token: 0x04002F31 RID: 12081
	private static Dictionary<int, SimTemperatureTransfer> handleInstanceMap = new Dictionary<int, SimTemperatureTransfer>();
}
