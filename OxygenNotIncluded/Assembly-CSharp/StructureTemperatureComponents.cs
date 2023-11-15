using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009F4 RID: 2548
public class StructureTemperatureComponents : KGameObjectSplitComponentManager<StructureTemperatureHeader, StructureTemperaturePayload>
{
	// Token: 0x06004C0D RID: 19469 RVA: 0x001AA5F4 File Offset: 0x001A87F4
	public HandleVector<int>.Handle Add(GameObject go)
	{
		StructureTemperaturePayload structureTemperaturePayload = new StructureTemperaturePayload(go);
		return base.Add(go, new StructureTemperatureHeader
		{
			dirty = false,
			simHandle = -1,
			isActiveBuilding = false
		}, ref structureTemperaturePayload);
	}

	// Token: 0x06004C0E RID: 19470 RVA: 0x001AA633 File Offset: 0x001A8833
	public static void ClearInstanceMap()
	{
		StructureTemperatureComponents.handleInstanceMap.Clear();
	}

	// Token: 0x06004C10 RID: 19472 RVA: 0x001AA648 File Offset: 0x001A8848
	protected override void OnPrefabInit(HandleVector<int>.Handle handle)
	{
		this.InitializeStatusItem();
		base.OnPrefabInit(handle);
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(StructureTemperatureComponents.OnGetTemperature);
		structureTemperaturePayload.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(StructureTemperatureComponents.OnSetTemperature);
		new_data.isActiveBuilding = (structureTemperaturePayload.building.Def.SelfHeatKilowattsWhenActive != 0f || structureTemperaturePayload.ExhaustKilowatts != 0f);
		base.SetHeader(handle, new_data);
	}

	// Token: 0x06004C11 RID: 19473 RVA: 0x001AA6D8 File Offset: 0x001A88D8
	private void InitializeStatusItem()
	{
		if (this.operatingEnergyStatusItem != null)
		{
			return;
		}
		this.operatingEnergyStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.operatingEnergyStatusItem.resolveStringCallback = delegate(string str, object ev_data)
		{
			int key = (int)ev_data;
			HandleVector<int>.Handle handle = StructureTemperatureComponents.handleInstanceMap[key];
			StructureTemperaturePayload payload = base.GetPayload(handle);
			if (str != BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP)
			{
				try
				{
					return string.Format(str, GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				}
				catch (Exception obj)
				{
					global::Debug.LogWarning(obj);
					global::Debug.LogWarning(BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP);
					global::Debug.LogWarning(str);
					return str;
				}
			}
			string text = "";
			foreach (StructureTemperaturePayload.EnergySource energySource in payload.energySourcesKW)
			{
				text += string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, energySource.source, GameUtil.GetFormattedHeatEnergy(energySource.value * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
			}
			str = string.Format(str, GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S), text);
			return str;
		};
	}

	// Token: 0x06004C12 RID: 19474 RVA: 0x001AA730 File Offset: 0x001A8930
	protected override void OnSpawn(HandleVector<int>.Handle handle)
	{
		StructureTemperatureHeader structureTemperatureHeader;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out structureTemperatureHeader, out structureTemperaturePayload);
		if (structureTemperaturePayload.operational != null && structureTemperatureHeader.isActiveBuilding)
		{
			structureTemperaturePayload.primaryElement.Subscribe(824508782, delegate(object ev_data)
			{
				StructureTemperatureComponents.OnActiveChanged(handle);
			});
		}
		structureTemperaturePayload.maxTemperature = ((structureTemperaturePayload.overheatable != null) ? structureTemperaturePayload.overheatable.OverheatTemperature : 10000f);
		if (structureTemperaturePayload.maxTemperature <= 0f)
		{
			global::Debug.LogError("invalid max temperature");
		}
		base.SetPayload(handle, ref structureTemperaturePayload);
		this.SimRegister(handle, ref structureTemperatureHeader, ref structureTemperaturePayload);
	}

	// Token: 0x06004C13 RID: 19475 RVA: 0x001AA7EC File Offset: 0x001A89EC
	private static void OnActiveChanged(HandleVector<int>.Handle handle)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.InternalTemperature = structureTemperaturePayload.Temperature;
		new_data.dirty = true;
		GameComps.StructureTemperatures.SetHeader(handle, new_data);
	}

	// Token: 0x06004C14 RID: 19476 RVA: 0x001AA82F File Offset: 0x001A8A2F
	protected override void OnCleanUp(HandleVector<int>.Handle handle)
	{
		this.SimUnregister(handle);
		base.OnCleanUp(handle);
	}

	// Token: 0x06004C15 RID: 19477 RVA: 0x001AA840 File Offset: 0x001A8A40
	public override void Sim200ms(float dt)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		List<StructureTemperatureHeader> list;
		List<StructureTemperaturePayload> list2;
		base.GetDataLists(out list, out list2);
		ListPool<int, StructureTemperatureComponents>.PooledList pooledList = ListPool<int, StructureTemperatureComponents>.Allocate();
		pooledList.Capacity = Math.Max(pooledList.Capacity, list.Count);
		ListPool<int, StructureTemperatureComponents>.PooledList pooledList2 = ListPool<int, StructureTemperatureComponents>.Allocate();
		pooledList2.Capacity = Math.Max(pooledList2.Capacity, list.Count);
		ListPool<int, StructureTemperatureComponents>.PooledList pooledList3 = ListPool<int, StructureTemperatureComponents>.Allocate();
		pooledList3.Capacity = Math.Max(pooledList3.Capacity, list.Count);
		for (int num4 = 0; num4 != list.Count; num4++)
		{
			StructureTemperatureHeader structureTemperatureHeader = list[num4];
			if (Sim.IsValidHandle(structureTemperatureHeader.simHandle))
			{
				pooledList.Add(num4);
				if (structureTemperatureHeader.dirty)
				{
					pooledList2.Add(num4);
					structureTemperatureHeader.dirty = false;
					list[num4] = structureTemperatureHeader;
				}
				if (structureTemperatureHeader.isActiveBuilding)
				{
					pooledList3.Add(num4);
				}
			}
		}
		foreach (int index in pooledList2)
		{
			StructureTemperaturePayload structureTemperaturePayload = list2[index];
			StructureTemperatureComponents.UpdateSimState(ref structureTemperaturePayload);
		}
		foreach (int index2 in pooledList2)
		{
			if (list2[index2].pendingEnergyModifications != 0f)
			{
				StructureTemperaturePayload structureTemperaturePayload2 = list2[index2];
				SimMessages.ModifyBuildingEnergy(structureTemperaturePayload2.simHandleCopy, structureTemperaturePayload2.pendingEnergyModifications, 0f, 10000f);
				structureTemperaturePayload2.pendingEnergyModifications = 0f;
				list2[index2] = structureTemperaturePayload2;
			}
		}
		foreach (int index3 in pooledList3)
		{
			StructureTemperaturePayload structureTemperaturePayload3 = list2[index3];
			if (structureTemperaturePayload3.operational == null || structureTemperaturePayload3.operational.IsActive)
			{
				num++;
				if (!structureTemperaturePayload3.isActiveStatusItemSet)
				{
					num3++;
					structureTemperaturePayload3.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, this.operatingEnergyStatusItem, structureTemperaturePayload3.simHandleCopy);
					structureTemperaturePayload3.isActiveStatusItemSet = true;
				}
				structureTemperaturePayload3.energySourcesKW = this.AccumulateProducedEnergyKW(structureTemperaturePayload3.energySourcesKW, structureTemperaturePayload3.OperatingKilowatts, BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING);
				if (structureTemperaturePayload3.ExhaustKilowatts != 0f)
				{
					num2++;
					Extents extents = structureTemperaturePayload3.GetExtents();
					int num5 = extents.width * extents.height;
					float num6 = structureTemperaturePayload3.ExhaustKilowatts * dt / (float)num5;
					for (int i = 0; i < extents.height; i++)
					{
						int num7 = extents.y + i;
						for (int j = 0; j < extents.width; j++)
						{
							int num8 = extents.x + j;
							int num9 = num7 * Grid.WidthInCells + num8;
							float num10 = Mathf.Min(Grid.Mass[num9], 1.5f) / 1.5f;
							float kilojoules = num6 * num10;
							SimMessages.ModifyEnergy(num9, kilojoules, structureTemperaturePayload3.maxTemperature, SimMessages.EnergySourceID.StructureTemperature);
						}
					}
					structureTemperaturePayload3.energySourcesKW = this.AccumulateProducedEnergyKW(structureTemperaturePayload3.energySourcesKW, structureTemperaturePayload3.ExhaustKilowatts, BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING);
				}
			}
			else if (structureTemperaturePayload3.isActiveStatusItemSet)
			{
				num3++;
				structureTemperaturePayload3.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, null, null);
				structureTemperaturePayload3.isActiveStatusItemSet = false;
			}
			list2[index3] = structureTemperaturePayload3;
		}
		pooledList3.Recycle();
		pooledList2.Recycle();
		pooledList.Recycle();
	}

	// Token: 0x06004C16 RID: 19478 RVA: 0x001AAC50 File Offset: 0x001A8E50
	private static void UpdateSimState(ref StructureTemperaturePayload payload)
	{
		DebugUtil.Assert(Sim.IsValidHandle(payload.simHandleCopy));
		float internalTemperature = payload.primaryElement.InternalTemperature;
		BuildingDef def = payload.building.Def;
		float mass = def.MassForTemperatureModification;
		float operatingKilowatts = payload.OperatingKilowatts;
		float overheat_temperature = (payload.overheatable != null) ? payload.overheatable.OverheatTemperature : 10000f;
		if (!payload.enabled || payload.bypass)
		{
			mass = 0f;
		}
		Extents extents = payload.GetExtents();
		ushort idx = payload.primaryElement.Element.idx;
		SimMessages.ModifyBuildingHeatExchange(payload.simHandleCopy, extents, mass, internalTemperature, def.ThermalConductivity, overheat_temperature, operatingKilowatts, idx);
	}

	// Token: 0x06004C17 RID: 19479 RVA: 0x001AAD00 File Offset: 0x001A8F00
	private unsafe static float OnGetTemperature(PrimaryElement primary_element)
	{
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
		StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
		float result;
		if (Sim.IsValidHandle(payload.simHandleCopy) && payload.enabled)
		{
			if (!payload.bypass)
			{
				int handleIndex = Sim.GetHandleIndex(payload.simHandleCopy);
				result = Game.Instance.simData.buildingTemperatures[handleIndex].temperature;
			}
			else
			{
				int i = Grid.PosToCell(payload.primaryElement.transform.GetPosition());
				result = Grid.Temperature[i];
			}
		}
		else
		{
			result = payload.primaryElement.InternalTemperature;
		}
		return result;
	}

	// Token: 0x06004C18 RID: 19480 RVA: 0x001AADAC File Offset: 0x001A8FAC
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
		StructureTemperatureHeader structureTemperatureHeader;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out structureTemperatureHeader, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.InternalTemperature = temperature;
		structureTemperatureHeader.dirty = true;
		GameComps.StructureTemperatures.SetHeader(handle, structureTemperatureHeader);
		if (!structureTemperatureHeader.isActiveBuilding && Sim.IsValidHandle(structureTemperaturePayload.simHandleCopy))
		{
			StructureTemperatureComponents.UpdateSimState(ref structureTemperaturePayload);
			if (structureTemperaturePayload.pendingEnergyModifications != 0f)
			{
				SimMessages.ModifyBuildingEnergy(structureTemperaturePayload.simHandleCopy, structureTemperaturePayload.pendingEnergyModifications, 0f, 10000f);
				structureTemperaturePayload.pendingEnergyModifications = 0f;
				GameComps.StructureTemperatures.SetPayload(handle, ref structureTemperaturePayload);
			}
		}
	}

	// Token: 0x06004C19 RID: 19481 RVA: 0x001AAE58 File Offset: 0x001A9058
	public void ProduceEnergy(HandleVector<int>.Handle handle, float delta_kilojoules, string source, float display_dt)
	{
		StructureTemperaturePayload payload = base.GetPayload(handle);
		if (Sim.IsValidHandle(payload.simHandleCopy))
		{
			SimMessages.ModifyBuildingEnergy(payload.simHandleCopy, delta_kilojoules, 0f, 10000f);
		}
		else
		{
			payload.pendingEnergyModifications += delta_kilojoules;
			StructureTemperatureHeader header = base.GetHeader(handle);
			header.dirty = true;
			base.SetHeader(handle, header);
		}
		payload.energySourcesKW = this.AccumulateProducedEnergyKW(payload.energySourcesKW, delta_kilojoules / display_dt, source);
		base.SetPayload(handle, ref payload);
	}

	// Token: 0x06004C1A RID: 19482 RVA: 0x001AAED8 File Offset: 0x001A90D8
	private List<StructureTemperaturePayload.EnergySource> AccumulateProducedEnergyKW(List<StructureTemperaturePayload.EnergySource> sources, float kw, string source)
	{
		if (sources == null)
		{
			sources = new List<StructureTemperaturePayload.EnergySource>();
		}
		bool flag = false;
		for (int i = 0; i < sources.Count; i++)
		{
			if (sources[i].source == source)
			{
				sources[i].Accumulate(kw);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			sources.Add(new StructureTemperaturePayload.EnergySource(kw, source));
		}
		return sources;
	}

	// Token: 0x06004C1B RID: 19483 RVA: 0x001AAF38 File Offset: 0x001A9138
	public static void DoStateTransition(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			StructureTemperatureComponents.DoMelt(GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement);
		}
	}

	// Token: 0x06004C1C RID: 19484 RVA: 0x001AAF74 File Offset: 0x001A9174
	public static void DoMelt(PrimaryElement primary_element)
	{
		Element element = primary_element.Element;
		if (element.highTempTransitionTarget != SimHashes.Unobtanium)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(primary_element.transform.GetPosition()), element.highTempTransitionTarget, CellEventLogger.Instance.OreMelted, primary_element.Mass, primary_element.Element.highTemp, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
			Util.KDestroyGameObject(primary_element.gameObject);
		}
	}

	// Token: 0x06004C1D RID: 19485 RVA: 0x001AAFE4 File Offset: 0x001A91E4
	public static void DoOverheat(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(1832602615, null);
		}
	}

	// Token: 0x06004C1E RID: 19486 RVA: 0x001AB02C File Offset: 0x001A922C
	public static void DoNoLongerOverheated(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(171119937, null);
		}
	}

	// Token: 0x06004C1F RID: 19487 RVA: 0x001AB071 File Offset: 0x001A9271
	public bool IsEnabled(HandleVector<int>.Handle handle)
	{
		return base.GetPayload(handle).enabled;
	}

	// Token: 0x06004C20 RID: 19488 RVA: 0x001AB080 File Offset: 0x001A9280
	private void Enable(HandleVector<int>.Handle handle, bool isEnabled)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		new_data.dirty = true;
		structureTemperaturePayload.enabled = isEnabled;
		base.SetData(handle, new_data, ref structureTemperaturePayload);
	}

	// Token: 0x06004C21 RID: 19489 RVA: 0x001AB0B2 File Offset: 0x001A92B2
	public void Enable(HandleVector<int>.Handle handle)
	{
		this.Enable(handle, true);
	}

	// Token: 0x06004C22 RID: 19490 RVA: 0x001AB0BC File Offset: 0x001A92BC
	public void Disable(HandleVector<int>.Handle handle)
	{
		this.Enable(handle, false);
	}

	// Token: 0x06004C23 RID: 19491 RVA: 0x001AB0C6 File Offset: 0x001A92C6
	public bool IsBypassed(HandleVector<int>.Handle handle)
	{
		return base.GetPayload(handle).bypass;
	}

	// Token: 0x06004C24 RID: 19492 RVA: 0x001AB0D4 File Offset: 0x001A92D4
	private void Bypass(HandleVector<int>.Handle handle, bool bypass)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		new_data.dirty = true;
		structureTemperaturePayload.bypass = bypass;
		base.SetData(handle, new_data, ref structureTemperaturePayload);
	}

	// Token: 0x06004C25 RID: 19493 RVA: 0x001AB106 File Offset: 0x001A9306
	public void Bypass(HandleVector<int>.Handle handle)
	{
		this.Bypass(handle, true);
	}

	// Token: 0x06004C26 RID: 19494 RVA: 0x001AB110 File Offset: 0x001A9310
	public void UnBypass(HandleVector<int>.Handle handle)
	{
		this.Bypass(handle, false);
	}

	// Token: 0x06004C27 RID: 19495 RVA: 0x001AB11C File Offset: 0x001A931C
	protected void SimRegister(HandleVector<int>.Handle handle, ref StructureTemperatureHeader header, ref StructureTemperaturePayload payload)
	{
		if (payload.simHandleCopy != -1)
		{
			return;
		}
		PrimaryElement primaryElement = payload.primaryElement;
		if (primaryElement.Mass <= 0f)
		{
			return;
		}
		if (primaryElement.Element.IsTemperatureInsulated)
		{
			return;
		}
		payload.simHandleCopy = -2;
		string dbg_name = primaryElement.name;
		HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle2 = Game.Instance.simComponentCallbackManager.Add(delegate(int sim_handle, object callback_data)
		{
			StructureTemperatureComponents.OnSimRegistered(handle, sim_handle, dbg_name);
		}, null, "StructureTemperature.SimRegister");
		BuildingDef def = primaryElement.GetComponent<Building>().Def;
		float internalTemperature = primaryElement.InternalTemperature;
		float massForTemperatureModification = def.MassForTemperatureModification;
		float operatingKilowatts = payload.OperatingKilowatts;
		Extents extents = payload.GetExtents();
		ushort idx = primaryElement.Element.idx;
		SimMessages.AddBuildingHeatExchange(extents, massForTemperatureModification, internalTemperature, def.ThermalConductivity, operatingKilowatts, idx, handle2.index);
		header.simHandle = payload.simHandleCopy;
		base.SetData(handle, header, ref payload);
	}

	// Token: 0x06004C28 RID: 19496 RVA: 0x001AB20C File Offset: 0x001A940C
	private static void OnSimRegistered(HandleVector<int>.Handle handle, int sim_handle, string dbg_name)
	{
		if (!GameComps.StructureTemperatures.IsValid(handle))
		{
			return;
		}
		if (!GameComps.StructureTemperatures.IsVersionValid(handle))
		{
			return;
		}
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		if (structureTemperaturePayload.simHandleCopy == -2)
		{
			StructureTemperatureComponents.handleInstanceMap[sim_handle] = handle;
			new_data.simHandle = sim_handle;
			structureTemperaturePayload.simHandleCopy = sim_handle;
			GameComps.StructureTemperatures.SetData(handle, new_data, ref structureTemperaturePayload);
			structureTemperaturePayload.primaryElement.Trigger(-1555603773, sim_handle);
			int cell = Grid.PosToCell(structureTemperaturePayload.building.transform.GetPosition());
			GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.contactConductiveLayer, new StructureToStructureTemperature.BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType.Created, structureTemperaturePayload.building, sim_handle));
			return;
		}
		SimMessages.RemoveBuildingHeatExchange(sim_handle, -1);
	}

	// Token: 0x06004C29 RID: 19497 RVA: 0x001AB2D4 File Offset: 0x001A94D4
	protected unsafe void SimUnregister(HandleVector<int>.Handle handle)
	{
		if (!GameComps.StructureTemperatures.IsVersionValid(handle))
		{
			KCrashReporter.Assert(false, "Handle version mismatch in StructureTemperature.SimUnregister");
			return;
		}
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		if (structureTemperaturePayload.simHandleCopy != -1)
		{
			int cell = Grid.PosToCell(structureTemperaturePayload.building);
			GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.contactConductiveLayer, new StructureToStructureTemperature.BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType.Destroyed, structureTemperaturePayload.building, structureTemperaturePayload.simHandleCopy));
			if (Sim.IsValidHandle(structureTemperaturePayload.simHandleCopy))
			{
				int handleIndex = Sim.GetHandleIndex(structureTemperaturePayload.simHandleCopy);
				structureTemperaturePayload.primaryElement.InternalTemperature = Game.Instance.simData.buildingTemperatures[handleIndex].temperature;
				SimMessages.RemoveBuildingHeatExchange(structureTemperaturePayload.simHandleCopy, -1);
				StructureTemperatureComponents.handleInstanceMap.Remove(structureTemperaturePayload.simHandleCopy);
			}
			structureTemperaturePayload.simHandleCopy = -1;
			new_data.simHandle = -1;
			base.SetData(handle, new_data, ref structureTemperaturePayload);
		}
	}

	// Token: 0x040031AE RID: 12718
	private const float MAX_PRESSURE = 1.5f;

	// Token: 0x040031AF RID: 12719
	private static Dictionary<int, HandleVector<int>.Handle> handleInstanceMap = new Dictionary<int, HandleVector<int>.Handle>();

	// Token: 0x040031B0 RID: 12720
	private StatusItem operatingEnergyStatusItem;
}
