using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009F5 RID: 2549
public class StructureToStructureTemperature : KMonoBehaviour
{
	// Token: 0x06004C2C RID: 19500 RVA: 0x001AB4F0 File Offset: 0x001A96F0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<StructureToStructureTemperature>(-1555603773, StructureToStructureTemperature.OnStructureTemperatureRegisteredDelegate);
	}

	// Token: 0x06004C2D RID: 19501 RVA: 0x001AB509 File Offset: 0x001A9709
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.DefineConductiveCells();
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
	}

	// Token: 0x06004C2E RID: 19502 RVA: 0x001AB537 File Offset: 0x001A9737
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
		this.UnregisterToSIM();
		base.OnCleanUp();
	}

	// Token: 0x06004C2F RID: 19503 RVA: 0x001AB568 File Offset: 0x001A9768
	private void OnStructureTemperatureRegistered(object _sim_handle)
	{
		int sim_handle = (int)_sim_handle;
		this.RegisterToSIM(sim_handle);
	}

	// Token: 0x06004C30 RID: 19504 RVA: 0x001AB584 File Offset: 0x001A9784
	private void RegisterToSIM(int sim_handle)
	{
		string name = this.building.Def.Name;
		SimMessages.RegisterBuildingToBuildingHeatExchange(sim_handle2, Game.Instance.simComponentCallbackManager.Add(delegate(int sim_handle, object callback_data)
		{
			this.OnSimRegistered(sim_handle);
		}, null, "StructureToStructureTemperature.SimRegister").index);
	}

	// Token: 0x06004C31 RID: 19505 RVA: 0x001AB5D1 File Offset: 0x001A97D1
	private void OnSimRegistered(int sim_handle)
	{
		if (sim_handle != -1)
		{
			this.selfHandle = sim_handle;
			this.hasBeenRegister = true;
			if (this.buildingDestroyed)
			{
				this.UnregisterToSIM();
				return;
			}
			this.Refresh_InContactBuildings();
		}
	}

	// Token: 0x06004C32 RID: 19506 RVA: 0x001AB5FA File Offset: 0x001A97FA
	private void UnregisterToSIM()
	{
		if (this.hasBeenRegister)
		{
			SimMessages.RemoveBuildingToBuildingHeatExchange(this.selfHandle, -1);
		}
		this.buildingDestroyed = true;
	}

	// Token: 0x06004C33 RID: 19507 RVA: 0x001AB618 File Offset: 0x001A9818
	private void DefineConductiveCells()
	{
		this.conductiveCells = new List<int>(this.building.PlacementCells);
		this.conductiveCells.Remove(this.building.GetUtilityInputCell());
		this.conductiveCells.Remove(this.building.GetUtilityOutputCell());
	}

	// Token: 0x06004C34 RID: 19508 RVA: 0x001AB669 File Offset: 0x001A9869
	private void Add(StructureToStructureTemperature.InContactBuildingData buildingData)
	{
		if (this.inContactBuildings.Add(buildingData.buildingInContact))
		{
			SimMessages.AddBuildingToBuildingHeatExchange(this.selfHandle, buildingData.buildingInContact, buildingData.cellsInContact);
		}
	}

	// Token: 0x06004C35 RID: 19509 RVA: 0x001AB695 File Offset: 0x001A9895
	private void Remove(int building)
	{
		if (this.inContactBuildings.Contains(building))
		{
			this.inContactBuildings.Remove(building);
			SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchange(this.selfHandle, building);
		}
	}

	// Token: 0x06004C36 RID: 19510 RVA: 0x001AB6C0 File Offset: 0x001A98C0
	private void OnAnyBuildingChanged(int _cell, object _data)
	{
		if (this.hasBeenRegister)
		{
			StructureToStructureTemperature.BuildingChangedObj buildingChangedObj = (StructureToStructureTemperature.BuildingChangedObj)_data;
			bool flag = false;
			int num = 0;
			for (int i = 0; i < buildingChangedObj.building.PlacementCells.Length; i++)
			{
				int item = buildingChangedObj.building.PlacementCells[i];
				if (this.conductiveCells.Contains(item))
				{
					flag = true;
					num++;
				}
			}
			if (flag)
			{
				int simHandler = buildingChangedObj.simHandler;
				StructureToStructureTemperature.BuildingChangeType changeType = buildingChangedObj.changeType;
				if (changeType == StructureToStructureTemperature.BuildingChangeType.Created)
				{
					StructureToStructureTemperature.InContactBuildingData buildingData = new StructureToStructureTemperature.InContactBuildingData
					{
						buildingInContact = simHandler,
						cellsInContact = num
					};
					this.Add(buildingData);
					return;
				}
				if (changeType != StructureToStructureTemperature.BuildingChangeType.Destroyed)
				{
					return;
				}
				this.Remove(simHandler);
			}
		}
	}

	// Token: 0x06004C37 RID: 19511 RVA: 0x001AB76C File Offset: 0x001A996C
	private void Refresh_InContactBuildings()
	{
		foreach (StructureToStructureTemperature.InContactBuildingData buildingData in this.GetAll_InContact_Buildings())
		{
			this.Add(buildingData);
		}
	}

	// Token: 0x06004C38 RID: 19512 RVA: 0x001AB7C0 File Offset: 0x001A99C0
	private List<StructureToStructureTemperature.InContactBuildingData> GetAll_InContact_Buildings()
	{
		Dictionary<Building, int> dictionary = new Dictionary<Building, int>();
		List<StructureToStructureTemperature.InContactBuildingData> list = new List<StructureToStructureTemperature.InContactBuildingData>();
		List<GameObject> buildingsInCell = new List<GameObject>();
		using (List<int>.Enumerator enumerator = this.conductiveCells.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int cell = enumerator.Current;
				buildingsInCell.Clear();
				Action<int> action = delegate(int layer)
				{
					GameObject gameObject = Grid.Objects[cell, layer];
					if (gameObject != null && !buildingsInCell.Contains(gameObject))
					{
						buildingsInCell.Add(gameObject);
					}
				};
				action(1);
				action(26);
				action(27);
				action(31);
				action(32);
				action(30);
				action(12);
				action(13);
				action(16);
				action(17);
				action(24);
				action(2);
				for (int i = 0; i < buildingsInCell.Count; i++)
				{
					Building building = (buildingsInCell[i] == null) ? null : buildingsInCell[i].GetComponent<Building>();
					if (building != null && building.Def.UseStructureTemperature && building.PlacementCellsContainCell(cell))
					{
						if (!dictionary.ContainsKey(building))
						{
							dictionary.Add(building, 0);
						}
						Dictionary<Building, int> dictionary2 = dictionary;
						Building key = building;
						int num = dictionary2[key];
						dictionary2[key] = num + 1;
					}
				}
			}
		}
		foreach (Building building2 in dictionary.Keys)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(building2);
			if (handle != HandleVector<int>.InvalidHandle)
			{
				int simHandleCopy = GameComps.StructureTemperatures.GetPayload(handle).simHandleCopy;
				StructureToStructureTemperature.InContactBuildingData item = new StructureToStructureTemperature.InContactBuildingData
				{
					buildingInContact = simHandleCopy,
					cellsInContact = dictionary[building2]
				};
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x040031B1 RID: 12721
	[MyCmpGet]
	private Building building;

	// Token: 0x040031B2 RID: 12722
	private List<int> conductiveCells;

	// Token: 0x040031B3 RID: 12723
	private HashSet<int> inContactBuildings = new HashSet<int>();

	// Token: 0x040031B4 RID: 12724
	private bool hasBeenRegister;

	// Token: 0x040031B5 RID: 12725
	private bool buildingDestroyed;

	// Token: 0x040031B6 RID: 12726
	private int selfHandle;

	// Token: 0x040031B7 RID: 12727
	protected static readonly EventSystem.IntraObjectHandler<StructureToStructureTemperature> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<StructureToStructureTemperature>(delegate(StructureToStructureTemperature component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	// Token: 0x02001879 RID: 6265
	public enum BuildingChangeType
	{
		// Token: 0x0400721E RID: 29214
		Created,
		// Token: 0x0400721F RID: 29215
		Destroyed,
		// Token: 0x04007220 RID: 29216
		Moved
	}

	// Token: 0x0200187A RID: 6266
	public struct InContactBuildingData
	{
		// Token: 0x04007221 RID: 29217
		public int buildingInContact;

		// Token: 0x04007222 RID: 29218
		public int cellsInContact;
	}

	// Token: 0x0200187B RID: 6267
	public struct BuildingChangedObj
	{
		// Token: 0x060091D7 RID: 37335 RVA: 0x0032A910 File Offset: 0x00328B10
		public BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType _changeType, Building _building, int sim_handler)
		{
			this.changeType = _changeType;
			this.building = _building;
			this.simHandler = sim_handler;
		}

		// Token: 0x04007223 RID: 29219
		public StructureToStructureTemperature.BuildingChangeType changeType;

		// Token: 0x04007224 RID: 29220
		public int simHandler;

		// Token: 0x04007225 RID: 29221
		public Building building;
	}
}
