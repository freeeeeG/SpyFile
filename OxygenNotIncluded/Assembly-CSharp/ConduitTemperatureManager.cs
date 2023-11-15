using System;
using System.Runtime.InteropServices;

// Token: 0x020006FA RID: 1786
public class ConduitTemperatureManager
{
	// Token: 0x06003114 RID: 12564 RVA: 0x00104A8E File Offset: 0x00102C8E
	public ConduitTemperatureManager()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Initialize();
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x00104AB3 File Offset: 0x00102CB3
	public void Shutdown()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Shutdown();
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x00104ABC File Offset: 0x00102CBC
	public HandleVector<int>.Handle Allocate(ConduitType conduit_type, int conduit_idx, HandleVector<int>.Handle conduit_structure_temperature_handle, ref ConduitFlow.ConduitContents contents)
	{
		StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(conduit_structure_temperature_handle);
		Element element = payload.primaryElement.Element;
		BuildingDef def = payload.building.Def;
		float conduit_heat_capacity = def.MassForTemperatureModification * element.specificHeatCapacity;
		float conduit_thermal_conductivity = element.thermalConductivity * def.ThermalConductivity;
		int num = ConduitTemperatureManager.ConduitTemperatureManager_Add(contents.temperature, contents.mass, (int)contents.element, payload.simHandleCopy, conduit_heat_capacity, conduit_thermal_conductivity, def.ThermalConductivity < 1f);
		HandleVector<int>.Handle result = default(HandleVector<int>.Handle);
		result.index = num;
		int handleIndex = Sim.GetHandleIndex(num);
		if (handleIndex + 1 > this.temperatures.Length)
		{
			Array.Resize<float>(ref this.temperatures, (handleIndex + 1) * 2);
			Array.Resize<ConduitTemperatureManager.ConduitInfo>(ref this.conduitInfo, (handleIndex + 1) * 2);
		}
		this.temperatures[handleIndex] = contents.temperature;
		this.conduitInfo[handleIndex] = new ConduitTemperatureManager.ConduitInfo
		{
			type = conduit_type,
			idx = conduit_idx
		};
		return result;
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x00104BC0 File Offset: 0x00102DC0
	public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.temperatures[Sim.GetHandleIndex(handle.index)] = contents.temperature;
		ConduitTemperatureManager.ConduitTemperatureManager_Set(handle.index, contents.temperature, contents.mass, (int)contents.element);
	}

	// Token: 0x06003118 RID: 12568 RVA: 0x00104C10 File Offset: 0x00102E10
	public void Free(HandleVector<int>.Handle handle)
	{
		if (handle.IsValid())
		{
			int handleIndex = Sim.GetHandleIndex(handle.index);
			this.temperatures[handleIndex] = -1f;
			this.conduitInfo[handleIndex] = new ConduitTemperatureManager.ConduitInfo
			{
				type = ConduitType.None,
				idx = -1
			};
			ConduitTemperatureManager.ConduitTemperatureManager_Remove(handle.index);
		}
	}

	// Token: 0x06003119 RID: 12569 RVA: 0x00104C71 File Offset: 0x00102E71
	public void Clear()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Clear();
	}

	// Token: 0x0600311A RID: 12570 RVA: 0x00104C78 File Offset: 0x00102E78
	public unsafe void Sim200ms(float dt)
	{
		ConduitTemperatureManager.ConduitTemperatureUpdateData* ptr = (ConduitTemperatureManager.ConduitTemperatureUpdateData*)((void*)ConduitTemperatureManager.ConduitTemperatureManager_Update(dt, (IntPtr)((void*)Game.Instance.simData.buildingTemperatures)));
		int numEntries = ptr->numEntries;
		if (numEntries > 0)
		{
			Marshal.Copy((IntPtr)((void*)ptr->temperatures), this.temperatures, 0, numEntries);
		}
		for (int i = 0; i < ptr->numFrozenHandles; i++)
		{
			int handleIndex = Sim.GetHandleIndex(ptr->frozenHandles[i]);
			ConduitTemperatureManager.ConduitInfo conduitInfo = this.conduitInfo[handleIndex];
			Conduit.GetFlowManager(conduitInfo.type).FreezeConduitContents(conduitInfo.idx);
		}
		for (int j = 0; j < ptr->numMeltedHandles; j++)
		{
			int handleIndex2 = Sim.GetHandleIndex(ptr->meltedHandles[j]);
			ConduitTemperatureManager.ConduitInfo conduitInfo2 = this.conduitInfo[handleIndex2];
			Conduit.GetFlowManager(conduitInfo2.type).MeltConduitContents(conduitInfo2.idx);
		}
	}

	// Token: 0x0600311B RID: 12571 RVA: 0x00104D61 File Offset: 0x00102F61
	public float GetTemperature(HandleVector<int>.Handle handle)
	{
		return this.temperatures[Sim.GetHandleIndex(handle.index)];
	}

	// Token: 0x0600311C RID: 12572
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Initialize();

	// Token: 0x0600311D RID: 12573
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Shutdown();

	// Token: 0x0600311E RID: 12574
	[DllImport("SimDLL")]
	private static extern int ConduitTemperatureManager_Add(float contents_temperature, float contents_mass, int contents_element_hash, int conduit_structure_temperature_handle, float conduit_heat_capacity, float conduit_thermal_conductivity, bool conduit_insulated);

	// Token: 0x0600311F RID: 12575
	[DllImport("SimDLL")]
	private static extern int ConduitTemperatureManager_Set(int handle, float contents_temperature, float contents_mass, int contents_element_hash);

	// Token: 0x06003120 RID: 12576
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Remove(int handle);

	// Token: 0x06003121 RID: 12577
	[DllImport("SimDLL")]
	private static extern IntPtr ConduitTemperatureManager_Update(float dt, IntPtr building_conductivity_data);

	// Token: 0x06003122 RID: 12578
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Clear();

	// Token: 0x04001D86 RID: 7558
	private float[] temperatures = new float[0];

	// Token: 0x04001D87 RID: 7559
	private ConduitTemperatureManager.ConduitInfo[] conduitInfo = new ConduitTemperatureManager.ConduitInfo[0];

	// Token: 0x0200144B RID: 5195
	private struct ConduitInfo
	{
		// Token: 0x0400650E RID: 25870
		public ConduitType type;

		// Token: 0x0400650F RID: 25871
		public int idx;
	}

	// Token: 0x0200144C RID: 5196
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ConduitTemperatureUpdateData
	{
		// Token: 0x04006510 RID: 25872
		public int numEntries;

		// Token: 0x04006511 RID: 25873
		public unsafe float* temperatures;

		// Token: 0x04006512 RID: 25874
		public int numFrozenHandles;

		// Token: 0x04006513 RID: 25875
		public unsafe int* frozenHandles;

		// Token: 0x04006514 RID: 25876
		public int numMeltedHandles;

		// Token: 0x04006515 RID: 25877
		public unsafe int* meltedHandles;
	}
}
