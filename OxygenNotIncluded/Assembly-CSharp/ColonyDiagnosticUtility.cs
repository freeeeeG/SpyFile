using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200048F RID: 1167
public class ColonyDiagnosticUtility : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06001A24 RID: 6692 RVA: 0x0008A962 File Offset: 0x00088B62
	public static void DestroyInstance()
	{
		ColonyDiagnosticUtility.Instance = null;
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x0008A96C File Offset: 0x00088B6C
	public ColonyDiagnostic.DiagnosticResult.Opinion GetWorldDiagnosticResult(int worldID)
	{
		ColonyDiagnostic.DiagnosticResult.Opinion opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Good;
		foreach (ColonyDiagnostic colonyDiagnostic in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else
				{
					opinion = (ColonyDiagnostic.DiagnosticResult.Opinion)Math.Min((int)opinion, (int)colonyDiagnostic.LatestResult.opinion);
				}
			}
		}
		return opinion;
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x0008AA28 File Offset: 0x00088C28
	public string GetWorldDiagnosticResultStatus(int worldID)
	{
		ColonyDiagnostic colonyDiagnostic = null;
		foreach (ColonyDiagnostic colonyDiagnostic2 in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic2.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic2.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic2.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else if (colonyDiagnostic == null || colonyDiagnostic2.LatestResult.opinion < colonyDiagnostic.LatestResult.opinion)
				{
					colonyDiagnostic = colonyDiagnostic2;
				}
			}
		}
		if (colonyDiagnostic == null || colonyDiagnostic.LatestResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			return "";
		}
		return colonyDiagnostic.name;
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x0008AB08 File Offset: 0x00088D08
	public string GetWorldDiagnosticResultTooltip(int worldID)
	{
		string text = "";
		foreach (ColonyDiagnostic colonyDiagnostic in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else if (colonyDiagnostic.LatestResult.opinion < ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
				{
					text = text + "\n" + colonyDiagnostic.LatestResult.GetFormattedMessage();
				}
			}
		}
		return text;
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x0008ABE4 File Offset: 0x00088DE4
	public bool IsDiagnosticTutorialDisabled(string id)
	{
		return ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus.ContainsKey(id) && GameClock.Instance.GetTime() < ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus[id];
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x0008AC17 File Offset: 0x00088E17
	public void ClearDiagnosticTutorialSetting(string id)
	{
		if (ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus.ContainsKey(id))
		{
			ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus[id] = -1f;
		}
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x0008AC40 File Offset: 0x00088E40
	public bool IsCriteriaEnabled(int worldID, string diagnosticID, string criteriaID)
	{
		Dictionary<string, List<string>> dictionary = this.diagnosticCriteriaDisabled[worldID];
		return dictionary.ContainsKey(diagnosticID) && !dictionary[diagnosticID].Contains(criteriaID);
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x0008AC78 File Offset: 0x00088E78
	public void SetCriteriaEnabled(int worldID, string diagnosticID, string criteriaID, bool enabled)
	{
		Dictionary<string, List<string>> dictionary = this.diagnosticCriteriaDisabled[worldID];
		global::Debug.Assert(dictionary.ContainsKey(diagnosticID), string.Format("Trying to set criteria on World {0} lacks diagnostic {1} that criteria {2} relates to", worldID, diagnosticID, criteriaID));
		List<string> list = dictionary[diagnosticID];
		if (enabled && list.Contains(criteriaID))
		{
			list.Remove(criteriaID);
		}
		if (!enabled && !list.Contains(criteriaID))
		{
			list.Add(criteriaID);
		}
	}

	// Token: 0x06001A2C RID: 6700 RVA: 0x0008ACDF File Offset: 0x00088EDF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ColonyDiagnosticUtility.Instance = this;
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x0008ACF0 File Offset: 0x00088EF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (int worldID in ClusterManager.Instance.GetWorldIDsSorted())
		{
			this.AddWorld(worldID);
		}
		ClusterManager.Instance.Subscribe(-1280433810, new Action<object>(this.Refresh));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorld));
	}

	// Token: 0x06001A2E RID: 6702 RVA: 0x0008AD88 File Offset: 0x00088F88
	private void Refresh(object data)
	{
		int worldID = (int)data;
		this.AddWorld(worldID);
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x0008ADA4 File Offset: 0x00088FA4
	private void RemoveWorld(object data)
	{
		int key = (int)data;
		if (this.diagnosticDisplaySettings.Remove(key))
		{
			List<ColonyDiagnostic> list;
			if (this.worldDiagnostics.TryGetValue(key, out list))
			{
				foreach (ColonyDiagnostic colonyDiagnostic in list)
				{
					colonyDiagnostic.OnCleanUp();
				}
			}
			this.worldDiagnostics.Remove(key);
		}
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x0008AE24 File Offset: 0x00089024
	public ColonyDiagnostic GetDiagnostic(string id, int worldID)
	{
		return this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match.id == id);
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x0008AE5B File Offset: 0x0008905B
	public T GetDiagnostic<T>(int worldID) where T : ColonyDiagnostic
	{
		return (T)((object)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is T));
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x0008AE94 File Offset: 0x00089094
	public string GetDiagnosticName(string id)
	{
		foreach (KeyValuePair<int, List<ColonyDiagnostic>> keyValuePair in this.worldDiagnostics)
		{
			foreach (ColonyDiagnostic colonyDiagnostic in keyValuePair.Value)
			{
				if (colonyDiagnostic.id == id)
				{
					return colonyDiagnostic.name;
				}
			}
		}
		global::Debug.LogWarning("Cannot locate name of diagnostic " + id + " because no worlds have a diagnostic with that id ");
		return "";
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x0008AF54 File Offset: 0x00089154
	public ChoreGroupDiagnostic GetChoreGroupDiagnostic(int worldID, ChoreGroup choreGroup)
	{
		return (ChoreGroupDiagnostic)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is ChoreGroupDiagnostic && ((ChoreGroupDiagnostic)match).choreGroup == choreGroup);
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x0008AF90 File Offset: 0x00089190
	public WorkTimeDiagnostic GetWorkTimeDiagnostic(int worldID, ChoreGroup choreGroup)
	{
		return (WorkTimeDiagnostic)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is WorkTimeDiagnostic && ((WorkTimeDiagnostic)match).choreGroup == choreGroup);
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x0008AFCC File Offset: 0x000891CC
	private void TryAddDiagnosticToWorldCollection(ref List<ColonyDiagnostic> newWorldDiagnostics, ColonyDiagnostic newDiagnostic)
	{
		if (!DlcManager.IsDlcListValidForCurrentContent(newDiagnostic.GetDlcIds()))
		{
			return;
		}
		newWorldDiagnostics.Add(newDiagnostic);
	}

	// Token: 0x06001A36 RID: 6710 RVA: 0x0008AFE4 File Offset: 0x000891E4
	public void AddWorld(int worldID)
	{
		bool flag = false;
		if (!this.diagnosticDisplaySettings.ContainsKey(worldID))
		{
			this.diagnosticDisplaySettings.Add(worldID, new Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>());
			flag = true;
		}
		if (!this.diagnosticCriteriaDisabled.ContainsKey(worldID))
		{
			this.diagnosticCriteriaDisabled.Add(worldID, new Dictionary<string, List<string>>());
		}
		List<ColonyDiagnostic> list = new List<ColonyDiagnostic>();
		this.TryAddDiagnosticToWorldCollection(ref list, new BreathabilityDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new FoodDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new StressDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new RadiationDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new ReactorDiagnostic(worldID));
		if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
		{
			this.TryAddDiagnosticToWorldCollection(ref list, new FloatingRocketDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketFuelDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketOxidizerDiagnostic(worldID));
		}
		else
		{
			this.TryAddDiagnosticToWorldCollection(ref list, new BedDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new ToiletDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new PowerUseDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new BatteryDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new IdleDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new TrappedDuplicantDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new FarmDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new EntombedDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketsInOrbitDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new MeteorDiagnostic(worldID));
		}
		this.worldDiagnostics.Add(worldID, list);
		foreach (ColonyDiagnostic colonyDiagnostic in list)
		{
			if (!this.diagnosticDisplaySettings[worldID].ContainsKey(colonyDiagnostic.id))
			{
				this.diagnosticDisplaySettings[worldID].Add(colonyDiagnostic.id, ColonyDiagnosticUtility.DisplaySetting.AlertOnly);
			}
			if (!this.diagnosticCriteriaDisabled[worldID].ContainsKey(colonyDiagnostic.id))
			{
				this.diagnosticCriteriaDisabled[worldID].Add(colonyDiagnostic.id, new List<string>());
			}
		}
		if (flag)
		{
			this.diagnosticDisplaySettings[worldID][typeof(BreathabilityDiagnostic).Name] = ColonyDiagnosticUtility.DisplaySetting.Always;
			this.diagnosticDisplaySettings[worldID][typeof(FoodDiagnostic).Name] = ColonyDiagnosticUtility.DisplaySetting.Always;
			this.diagnosticDisplaySettings[worldID][typeof(StressDiagnostic).Name] = ColonyDiagnosticUtility.DisplaySetting.Always;
			this.diagnosticDisplaySettings[worldID][typeof(IdleDiagnostic).Name] = ColonyDiagnosticUtility.DisplaySetting.Never;
			if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
			{
				this.diagnosticDisplaySettings[worldID][typeof(FloatingRocketDiagnostic).Name] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID][typeof(RocketFuelDiagnostic).Name] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID][typeof(RocketOxidizerDiagnostic).Name] = ColonyDiagnosticUtility.DisplaySetting.Always;
			}
		}
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x0008B2F8 File Offset: 0x000894F8
	public void Sim1000ms(float dt)
	{
		if (ColonyDiagnosticUtility.IgnoreFirstUpdate)
		{
			ColonyDiagnosticUtility.IgnoreFirstUpdate = false;
		}
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x0008B308 File Offset: 0x00089508
	public static bool PastNewBuildingGracePeriod(Transform building)
	{
		BuildingComplete component = building.GetComponent<BuildingComplete>();
		return !(component != null) || GameClock.Instance.GetTime() - component.creationTime >= 600f;
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x0008B340 File Offset: 0x00089540
	public static bool IgnoreRocketsWithNoCrewRequested(int worldID, out ColonyDiagnostic.DiagnosticResult result)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(worldID);
		string message = world.IsModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID;
		result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, message, null);
		if (world.IsModuleInterior)
		{
			for (int i = 0; i < Components.Clustercrafts.Count; i++)
			{
				WorldContainer interiorWorld = Components.Clustercrafts[i].ModuleInterface.GetInteriorWorld();
				if (!(interiorWorld == null) && interiorWorld.id == worldID)
				{
					PassengerRocketModule passengerModule = Components.Clustercrafts[i].ModuleInterface.GetPassengerModule();
					if (passengerModule != null && !passengerModule.ShouldCrewGetIn())
					{
						result = default(ColonyDiagnostic.DiagnosticResult);
						result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
						result.Message = UI.COLONY_DIAGNOSTICS.NO_MINIONS_REQUESTED;
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x04000E84 RID: 3716
	public static ColonyDiagnosticUtility Instance;

	// Token: 0x04000E85 RID: 3717
	private Dictionary<int, List<ColonyDiagnostic>> worldDiagnostics = new Dictionary<int, List<ColonyDiagnostic>>();

	// Token: 0x04000E86 RID: 3718
	[Serialize]
	public Dictionary<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>> diagnosticDisplaySettings = new Dictionary<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>>();

	// Token: 0x04000E87 RID: 3719
	[Serialize]
	public Dictionary<int, Dictionary<string, List<string>>> diagnosticCriteriaDisabled = new Dictionary<int, Dictionary<string, List<string>>>();

	// Token: 0x04000E88 RID: 3720
	[Serialize]
	private Dictionary<string, float> diagnosticTutorialStatus = new Dictionary<string, float>
	{
		{
			typeof(ToiletDiagnostic).Name,
			450f
		},
		{
			typeof(BedDiagnostic).Name,
			900f
		},
		{
			typeof(BreathabilityDiagnostic).Name,
			1800f
		},
		{
			typeof(FoodDiagnostic).Name,
			3000f
		},
		{
			typeof(FarmDiagnostic).Name,
			6000f
		},
		{
			typeof(StressDiagnostic).Name,
			9000f
		},
		{
			typeof(PowerUseDiagnostic).Name,
			12000f
		},
		{
			typeof(BatteryDiagnostic).Name,
			12000f
		}
	};

	// Token: 0x04000E89 RID: 3721
	public static bool IgnoreFirstUpdate = true;

	// Token: 0x04000E8A RID: 3722
	public static ColonyDiagnostic.DiagnosticResult NoDataResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.NO_DATA, null);

	// Token: 0x0200110C RID: 4364
	public enum DisplaySetting
	{
		// Token: 0x04005B27 RID: 23335
		Always,
		// Token: 0x04005B28 RID: 23336
		AlertOnly,
		// Token: 0x04005B29 RID: 23337
		Never,
		// Token: 0x04005B2A RID: 23338
		LENGTH
	}
}
