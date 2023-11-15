using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200099B RID: 2459
[SerializationConfig(MemberSerialization.OptIn)]
public class CraftModuleInterface : KMonoBehaviour, ISim4000ms
{
	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x06004903 RID: 18691 RVA: 0x0019B470 File Offset: 0x00199670
	public IList<Ref<RocketModuleCluster>> ClusterModules
	{
		get
		{
			return this.clusterModules.AsReadOnly();
		}
	}

	// Token: 0x06004904 RID: 18692 RVA: 0x0019B47D File Offset: 0x0019967D
	public LaunchPad GetPreferredLaunchPadForWorld(int world_id)
	{
		if (this.preferredLaunchPad.ContainsKey(world_id))
		{
			return this.preferredLaunchPad[world_id].Get();
		}
		return null;
	}

	// Token: 0x06004905 RID: 18693 RVA: 0x0019B4A0 File Offset: 0x001996A0
	private void SetPreferredLaunchPadForWorld(LaunchPad pad)
	{
		if (!this.preferredLaunchPad.ContainsKey(pad.GetMyWorldId()))
		{
			this.preferredLaunchPad.Add(this.CurrentPad.GetMyWorldId(), new Ref<LaunchPad>());
		}
		this.preferredLaunchPad[this.CurrentPad.GetMyWorldId()].Set(this.CurrentPad);
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06004906 RID: 18694 RVA: 0x0019B4FC File Offset: 0x001996FC
	public LaunchPad CurrentPad
	{
		get
		{
			if (this.m_clustercraft != null && this.m_clustercraft.Status != Clustercraft.CraftStatus.InFlight && this.clusterModules.Count > 0)
			{
				if (this.bottomModule == null)
				{
					this.SetBottomModule();
				}
				global::Debug.Assert(this.bottomModule != null && this.bottomModule.Get() != null, "More than one cluster module but no bottom module found.");
				int num = Grid.CellBelow(Grid.PosToCell(this.bottomModule.Get().transform.position));
				if (Grid.IsValidCell(num))
				{
					GameObject gameObject = null;
					Grid.ObjectLayers[1].TryGetValue(num, out gameObject);
					if (gameObject != null)
					{
						return gameObject.GetComponent<LaunchPad>();
					}
				}
			}
			return null;
		}
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x06004907 RID: 18695 RVA: 0x0019B5B8 File Offset: 0x001997B8
	public float Speed
	{
		get
		{
			return this.m_clustercraft.Speed;
		}
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x06004908 RID: 18696 RVA: 0x0019B5C8 File Offset: 0x001997C8
	public float Range
	{
		get
		{
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				RocketEngineCluster component = @ref.Get().GetComponent<RocketEngineCluster>();
				if (component != null)
				{
					return this.BurnableMassRemaining / component.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
				}
			}
			return 0f;
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x06004909 RID: 18697 RVA: 0x0019B648 File Offset: 0x00199848
	public float FuelPerHex
	{
		get
		{
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				RocketEngineCluster component = @ref.Get().GetComponent<RocketEngineCluster>();
				if (component != null)
				{
					return component.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance * 600f;
				}
			}
			return float.PositiveInfinity;
		}
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x0600490A RID: 18698 RVA: 0x0019B6C8 File Offset: 0x001998C8
	public float BurnableMassRemaining
	{
		get
		{
			RocketEngineCluster rocketEngineCluster = null;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				rocketEngineCluster = @ref.Get().GetComponent<RocketEngineCluster>();
				if (rocketEngineCluster != null)
				{
					break;
				}
			}
			if (rocketEngineCluster == null)
			{
				return 0f;
			}
			if (!rocketEngineCluster.requireOxidizer)
			{
				return this.FuelRemaining;
			}
			return Mathf.Min(this.FuelRemaining, this.OxidizerPowerRemaining);
		}
	}

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x0600490B RID: 18699 RVA: 0x0019B75C File Offset: 0x0019995C
	public float FuelRemaining
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine == null)
			{
				return 0f;
			}
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				IFuelTank component = @ref.Get().GetComponent<IFuelTank>();
				if (!component.IsNullOrDestroyed())
				{
					num += component.Storage.GetAmountAvailable(engine.fuelTag);
				}
			}
			return (float)Mathf.CeilToInt(num);
		}
	}

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x0600490C RID: 18700 RVA: 0x0019B7F4 File Offset: 0x001999F4
	public float OxidizerPowerRemaining
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				OxidizerTank component = @ref.Get().GetComponent<OxidizerTank>();
				if (component != null)
				{
					num += component.TotalOxidizerPower;
				}
			}
			return (float)Mathf.CeilToInt(num);
		}
	}

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x0600490D RID: 18701 RVA: 0x0019B86C File Offset: 0x00199A6C
	public int MaxHeight
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				return engine.maxHeight;
			}
			return -1;
		}
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x0600490E RID: 18702 RVA: 0x0019B891 File Offset: 0x00199A91
	public float TotalBurden
	{
		get
		{
			return this.m_clustercraft.TotalBurden;
		}
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x0600490F RID: 18703 RVA: 0x0019B89E File Offset: 0x00199A9E
	public float EnginePower
	{
		get
		{
			return this.m_clustercraft.EnginePower;
		}
	}

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x06004910 RID: 18704 RVA: 0x0019B8AC File Offset: 0x00199AAC
	public int RocketHeight
	{
		get
		{
			int num = 0;
			foreach (Ref<RocketModuleCluster> @ref in this.ClusterModules)
			{
				num += @ref.Get().GetComponent<Building>().Def.HeightInCells;
			}
			return num;
		}
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x06004911 RID: 18705 RVA: 0x0019B910 File Offset: 0x00199B10
	public bool HasCargoModule
	{
		get
		{
			using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.ClusterModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Get().GetComponent<CargoBayCluster>() != null)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x06004912 RID: 18706 RVA: 0x0019B968 File Offset: 0x00199B68
	protected override void OnPrefabInit()
	{
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(instance.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
	}

	// Token: 0x06004913 RID: 18707 RVA: 0x0019B990 File Offset: 0x00199B90
	protected override void OnSpawn()
	{
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(instance.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
		if (this.m_clustercraft.Status != Clustercraft.CraftStatus.Grounded)
		{
			this.ForceAttachmentNetwork();
		}
		this.SetBottomModule();
		base.Subscribe(-1311384361, new Action<object>(this.CompleteSelfDestruct));
	}

	// Token: 0x06004914 RID: 18708 RVA: 0x0019B9F4 File Offset: 0x00199BF4
	private void OnLoad(Game.GameSaveData data)
	{
		foreach (Ref<RocketModule> @ref in this.modules)
		{
			this.clusterModules.Add(new Ref<RocketModuleCluster>(@ref.Get().GetComponent<RocketModuleCluster>()));
		}
		this.modules.Clear();
		foreach (Ref<RocketModuleCluster> ref2 in this.clusterModules)
		{
			if (!(ref2.Get() == null))
			{
				ref2.Get().CraftInterface = this;
			}
		}
		bool flag = false;
		for (int i = this.clusterModules.Count - 1; i >= 0; i--)
		{
			if (this.clusterModules[i] == null || this.clusterModules[i].Get() == null)
			{
				global::Debug.LogWarning(string.Format("Rocket {0} had a null module at index {1} on load! Why????", base.name, i), this);
				this.clusterModules.RemoveAt(i);
				flag = true;
			}
		}
		this.SetBottomModule();
		if (flag && this.m_clustercraft.Status == Clustercraft.CraftStatus.Grounded)
		{
			global::Debug.LogWarning("The module stack was broken. Collapsing " + base.name + "...", this);
			this.SortModuleListByPosition();
			LaunchPad currentPad = this.CurrentPad;
			if (currentPad != null)
			{
				int num = currentPad.RocketBottomPosition;
				for (int j = 0; j < this.clusterModules.Count; j++)
				{
					RocketModuleCluster rocketModuleCluster = this.clusterModules[j].Get();
					if (num != Grid.PosToCell(rocketModuleCluster.transform.GetPosition()))
					{
						global::Debug.LogWarning(string.Format("Collapsing space under module {0}:{1}", j, rocketModuleCluster.name));
						rocketModuleCluster.transform.SetPosition(Grid.CellToPos(num, CellAlignment.Bottom, Grid.SceneLayer.Building));
					}
					num = Grid.OffsetCell(num, 0, this.clusterModules[j].Get().GetComponent<Building>().Def.HeightInCells);
				}
			}
			for (int k = 0; k < this.clusterModules.Count - 1; k++)
			{
				BuildingAttachPoint component = this.clusterModules[k].Get().GetComponent<BuildingAttachPoint>();
				if (component != null)
				{
					AttachableBuilding component2 = this.clusterModules[k + 1].Get().GetComponent<AttachableBuilding>();
					if (component.points[0].attachedBuilding != component2)
					{
						global::Debug.LogWarning("Reattaching " + component.name + " & " + component2.name);
						component.points[0].attachedBuilding = component2;
					}
				}
			}
		}
	}

	// Token: 0x06004915 RID: 18709 RVA: 0x0019BCE8 File Offset: 0x00199EE8
	public void AddModule(RocketModuleCluster newModule)
	{
		for (int i = 0; i < this.clusterModules.Count; i++)
		{
			if (this.clusterModules[i].Get() == newModule)
			{
				global::Debug.LogError(string.Concat(new string[]
				{
					"Adding module ",
					(newModule != null) ? newModule.ToString() : null,
					" to the same rocket (",
					this.m_clustercraft.Name,
					") twice"
				}));
			}
		}
		this.clusterModules.Add(new Ref<RocketModuleCluster>(newModule));
		newModule.CraftInterface = this;
		base.Trigger(1512695988, newModule);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			if (rocketModuleCluster != null && rocketModuleCluster != newModule)
			{
				rocketModuleCluster.Trigger(1512695988, newModule);
			}
		}
		newModule.Trigger(1512695988, newModule);
		this.SetBottomModule();
	}

	// Token: 0x06004916 RID: 18710 RVA: 0x0019BE04 File Offset: 0x0019A004
	public void RemoveModule(RocketModuleCluster module)
	{
		for (int i = this.clusterModules.Count - 1; i >= 0; i--)
		{
			if (this.clusterModules[i].Get() == module)
			{
				this.clusterModules.RemoveAt(i);
				break;
			}
		}
		base.Trigger(1512695988, null);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(1512695988, null);
		}
		this.SetBottomModule();
		if (this.clusterModules.Count == 0)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06004917 RID: 18711 RVA: 0x0019BEC8 File Offset: 0x0019A0C8
	private void SortModuleListByPosition()
	{
		this.clusterModules.Sort(delegate(Ref<RocketModuleCluster> a, Ref<RocketModuleCluster> b)
		{
			if (Grid.CellToPos(Grid.PosToCell(a.Get())).y >= Grid.CellToPos(Grid.PosToCell(b.Get())).y)
			{
				return 1;
			}
			return -1;
		});
	}

	// Token: 0x06004918 RID: 18712 RVA: 0x0019BEF4 File Offset: 0x0019A0F4
	private void SetBottomModule()
	{
		if (this.clusterModules.Count > 0)
		{
			this.bottomModule = this.clusterModules[0];
			Vector3 vector = this.bottomModule.Get().transform.position;
			using (List<Ref<RocketModuleCluster>>.Enumerator enumerator = this.clusterModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ref<RocketModuleCluster> @ref = enumerator.Current;
					Vector3 position = @ref.Get().transform.position;
					if (position.y < vector.y)
					{
						this.bottomModule = @ref;
						vector = position;
					}
				}
				return;
			}
		}
		this.bottomModule = null;
	}

	// Token: 0x06004919 RID: 18713 RVA: 0x0019BFA8 File Offset: 0x0019A1A8
	public int GetHeightOfModuleTop(GameObject module)
	{
		int num = 0;
		for (int i = 0; i < this.ClusterModules.Count; i++)
		{
			num += this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells;
			if (this.clusterModules[i].Get().gameObject == module)
			{
				return num;
			}
		}
		global::Debug.LogError("Could not find module " + module.GetProperName() + " in CraftModuleInterface craft " + this.m_clustercraft.Name);
		return 0;
	}

	// Token: 0x0600491A RID: 18714 RVA: 0x0019C038 File Offset: 0x0019A238
	public int GetModuleRelativeVerticalPosition(GameObject module)
	{
		int num = 0;
		for (int i = 0; i < this.ClusterModules.Count; i++)
		{
			if (this.clusterModules[i].Get().gameObject == module)
			{
				return num;
			}
			num += this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells;
		}
		global::Debug.LogError("Could not find module " + module.GetProperName() + " in CraftModuleInterface craft " + this.m_clustercraft.Name);
		return 0;
	}

	// Token: 0x0600491B RID: 18715 RVA: 0x0019C0C8 File Offset: 0x0019A2C8
	public void Sim4000ms(float dt)
	{
		int num = 0;
		foreach (ProcessCondition.ProcessConditionType conditionType in this.conditionsToCheck)
		{
			if (this.EvaluateConditionSet(conditionType) != ProcessCondition.Status.Failure)
			{
				num++;
			}
		}
		if (num != this.lastConditionTypeSucceeded)
		{
			this.lastConditionTypeSucceeded = num;
			this.TriggerEventOnCraftAndRocket(GameHashes.LaunchConditionChanged, null);
		}
	}

	// Token: 0x0600491C RID: 18716 RVA: 0x0019C140 File Offset: 0x0019A340
	public bool IsLaunchRequested()
	{
		return this.m_clustercraft.LaunchRequested;
	}

	// Token: 0x0600491D RID: 18717 RVA: 0x0019C14D File Offset: 0x0019A34D
	public bool CheckPreppedForLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight) > ProcessCondition.Status.Failure;
	}

	// Token: 0x0600491E RID: 18718 RVA: 0x0019C16D File Offset: 0x0019A36D
	public bool CheckReadyToLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) > ProcessCondition.Status.Failure;
	}

	// Token: 0x0600491F RID: 18719 RVA: 0x0019C196 File Offset: 0x0019A396
	public bool HasLaunchWarnings()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Warning || this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Warning || this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) == ProcessCondition.Status.Warning;
	}

	// Token: 0x06004920 RID: 18720 RVA: 0x0019C1B8 File Offset: 0x0019A3B8
	public bool CheckReadyForAutomatedLaunchCommand()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Ready;
	}

	// Token: 0x06004921 RID: 18721 RVA: 0x0019C1D0 File Offset: 0x0019A3D0
	public bool CheckReadyForAutomatedLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) == ProcessCondition.Status.Ready;
	}

	// Token: 0x06004922 RID: 18722 RVA: 0x0019C1F4 File Offset: 0x0019A3F4
	public void TriggerEventOnCraftAndRocket(GameHashes evt, object data)
	{
		base.Trigger((int)evt, data);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger((int)evt, data);
		}
	}

	// Token: 0x06004923 RID: 18723 RVA: 0x0019C254 File Offset: 0x0019A454
	public void CancelLaunch()
	{
		this.m_clustercraft.CancelLaunch();
	}

	// Token: 0x06004924 RID: 18724 RVA: 0x0019C261 File Offset: 0x0019A461
	public void TriggerLaunch(bool automated = false)
	{
		this.m_clustercraft.RequestLaunch(automated);
	}

	// Token: 0x06004925 RID: 18725 RVA: 0x0019C270 File Offset: 0x0019A470
	public void DoLaunch()
	{
		this.SortModuleListByPosition();
		this.CurrentPad.Trigger(705820818, this);
		this.SetPreferredLaunchPadForWorld(this.CurrentPad);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(705820818, this);
		}
	}

	// Token: 0x06004926 RID: 18726 RVA: 0x0019C2F0 File Offset: 0x0019A4F0
	public void DoLand(LaunchPad pad)
	{
		int num = pad.RocketBottomPosition;
		for (int i = 0; i < this.clusterModules.Count; i++)
		{
			this.clusterModules[i].Get().MoveToPad(num);
			num = Grid.OffsetCell(num, 0, this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells);
		}
		this.SetBottomModule();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(-1165815793, pad);
		}
		pad.Trigger(-1165815793, this);
	}

	// Token: 0x06004927 RID: 18727 RVA: 0x0019C3BC File Offset: 0x0019A5BC
	public LaunchConditionManager FindLaunchConditionManager()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			LaunchConditionManager component = @ref.Get().GetComponent<LaunchConditionManager>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06004928 RID: 18728 RVA: 0x0019C424 File Offset: 0x0019A624
	public LaunchableRocketCluster FindLaunchableRocket()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			LaunchableRocketCluster component = rocketModuleCluster.GetComponent<LaunchableRocketCluster>();
			if (component != null && rocketModuleCluster.CraftInterface != null && rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06004929 RID: 18729 RVA: 0x0019C4AC File Offset: 0x0019A6AC
	public List<GameObject> GetParts()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			list.Add(@ref.Get().gameObject);
		}
		return list;
	}

	// Token: 0x0600492A RID: 18730 RVA: 0x0019C510 File Offset: 0x0019A710
	public RocketEngineCluster GetEngine()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketEngineCluster component = @ref.Get().GetComponent<RocketEngineCluster>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x0600492B RID: 18731 RVA: 0x0019C578 File Offset: 0x0019A778
	public PassengerRocketModule GetPassengerModule()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			PassengerRocketModule component = @ref.Get().GetComponent<PassengerRocketModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x0600492C RID: 18732 RVA: 0x0019C5E0 File Offset: 0x0019A7E0
	public WorldContainer GetInteriorWorld()
	{
		PassengerRocketModule passengerModule = this.GetPassengerModule();
		if (passengerModule == null)
		{
			return null;
		}
		ClustercraftInteriorDoor interiorDoor = passengerModule.GetComponent<ClustercraftExteriorDoor>().GetInteriorDoor();
		if (interiorDoor == null)
		{
			return null;
		}
		return interiorDoor.GetMyWorld();
	}

	// Token: 0x0600492D RID: 18733 RVA: 0x0019C61C File Offset: 0x0019A81C
	public RocketClusterDestinationSelector GetClusterDestinationSelector()
	{
		return base.GetComponent<RocketClusterDestinationSelector>();
	}

	// Token: 0x0600492E RID: 18734 RVA: 0x0019C624 File Offset: 0x0019A824
	public bool HasClusterDestinationSelector()
	{
		return base.GetComponent<RocketClusterDestinationSelector>() != null;
	}

	// Token: 0x0600492F RID: 18735 RVA: 0x0019C634 File Offset: 0x0019A834
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		this.returnConditions.Clear();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			List<ProcessCondition> conditionSet = @ref.Get().GetConditionSet(conditionType);
			if (conditionSet != null)
			{
				this.returnConditions.AddRange(conditionSet);
			}
		}
		if (this.CurrentPad != null)
		{
			List<ProcessCondition> conditionSet2 = this.CurrentPad.GetComponent<LaunchPadConditions>().GetConditionSet(conditionType);
			if (conditionSet2 != null)
			{
				this.returnConditions.AddRange(conditionSet2);
			}
		}
		return this.returnConditions;
	}

	// Token: 0x06004930 RID: 18736 RVA: 0x0019C6DC File Offset: 0x0019A8DC
	private ProcessCondition.Status EvaluateConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		ProcessCondition.Status status = ProcessCondition.Status.Ready;
		foreach (ProcessCondition processCondition in this.GetConditionSet(conditionType))
		{
			ProcessCondition.Status status2 = processCondition.EvaluateCondition();
			if (status2 < status)
			{
				status = status2;
			}
			if (status == ProcessCondition.Status.Failure)
			{
				break;
			}
		}
		return status;
	}

	// Token: 0x06004931 RID: 18737 RVA: 0x0019C73C File Offset: 0x0019A93C
	private void ForceAttachmentNetwork()
	{
		RocketModuleCluster rocketModuleCluster = null;
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster2 = @ref.Get();
			if (rocketModuleCluster != null)
			{
				BuildingAttachPoint component = rocketModuleCluster.GetComponent<BuildingAttachPoint>();
				AttachableBuilding component2 = rocketModuleCluster2.GetComponent<AttachableBuilding>();
				component.points[0].attachedBuilding = component2;
			}
			rocketModuleCluster = rocketModuleCluster2;
		}
	}

	// Token: 0x06004932 RID: 18738 RVA: 0x0019C7B8 File Offset: 0x0019A9B8
	public static Storage SpawnRocketDebris(string nameSuffix, SimHashes element)
	{
		Vector3 position = new Vector3(-1f, -1f, 0f);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DebrisPayload"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(element, true);
		gameObject.name += nameSuffix;
		gameObject.SetActive(true);
		return gameObject.GetComponent<Storage>();
	}

	// Token: 0x06004933 RID: 18739 RVA: 0x0019C81C File Offset: 0x0019AA1C
	public void CompleteSelfDestruct(object data = null)
	{
		global::Debug.Assert(this.HasTag(GameTags.RocketInSpace), "Self Destruct is only valid for in-space rockets!");
		SimHashes elementID = this.GetPassengerModule().GetComponent<PrimaryElement>().ElementID;
		List<RocketModule> list = new List<RocketModule>();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			list.Add(@ref.Get());
		}
		List<GameObject> list2 = new List<GameObject>();
		List<GameObject> list3 = new List<GameObject>();
		foreach (RocketModule rocketModule in list)
		{
			Storage[] components = rocketModule.GetComponents<Storage>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].DropAll(false, false, default(Vector3), true, list3);
				foreach (GameObject gameObject in list3)
				{
					if (gameObject.HasTag(GameTags.Creature))
					{
						Butcherable component = gameObject.GetComponent<Butcherable>();
						if (component != null && component.drops != null && component.drops.Length != 0)
						{
							GameObject[] collection = component.CreateDrops();
							list2.AddRange(collection);
						}
						gameObject.DeleteObject();
					}
					else
					{
						list2.Add(gameObject);
					}
				}
				list3.Clear();
			}
			Deconstructable component2 = rocketModule.GetComponent<Deconstructable>();
			list2.AddRange(component2.ForceDestroyAndGetMaterials());
		}
		List<Storage> list4 = new List<Storage>();
		foreach (GameObject gameObject2 in list2)
		{
			Pickupable component3 = gameObject2.GetComponent<Pickupable>();
			if (component3 != null)
			{
				component3.PrimaryElement.Units = (float)Mathf.Max(1, Mathf.RoundToInt(component3.PrimaryElement.Units * 0.5f));
				if ((list4.Count == 0 || list4[list4.Count - 1].RemainingCapacity() == 0f) && component3.PrimaryElement.Mass > 0f)
				{
					list4.Add(CraftModuleInterface.SpawnRocketDebris(" from CMI", elementID));
				}
				Storage storage = list4[list4.Count - 1];
				while (component3.PrimaryElement.Mass > storage.RemainingCapacity())
				{
					Pickupable pickupable = component3.Take(storage.RemainingCapacity());
					storage.Store(pickupable.gameObject, false, false, true, false);
					storage = CraftModuleInterface.SpawnRocketDebris(" from CMI", elementID);
					list4.Add(storage);
				}
				if (component3.PrimaryElement.Mass > 0f)
				{
					storage.Store(component3.gameObject, false, false, true, false);
				}
			}
		}
		foreach (Storage cmp in list4)
		{
			RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
			smi.StartSM();
			smi.Travel(this.m_clustercraft.Location, ClusterUtil.ClosestVisibleAsteroidToLocation(this.m_clustercraft.Location).Location);
		}
		this.m_clustercraft.SetExploding();
	}

	// Token: 0x04003023 RID: 12323
	[Serialize]
	private List<Ref<RocketModule>> modules = new List<Ref<RocketModule>>();

	// Token: 0x04003024 RID: 12324
	[Serialize]
	private List<Ref<RocketModuleCluster>> clusterModules = new List<Ref<RocketModuleCluster>>();

	// Token: 0x04003025 RID: 12325
	private Ref<RocketModuleCluster> bottomModule;

	// Token: 0x04003026 RID: 12326
	[Serialize]
	private Dictionary<int, Ref<LaunchPad>> preferredLaunchPad = new Dictionary<int, Ref<LaunchPad>>();

	// Token: 0x04003027 RID: 12327
	[MyCmpReq]
	private Clustercraft m_clustercraft;

	// Token: 0x04003028 RID: 12328
	private List<ProcessCondition.ProcessConditionType> conditionsToCheck = new List<ProcessCondition.ProcessConditionType>
	{
		ProcessCondition.ProcessConditionType.RocketPrep,
		ProcessCondition.ProcessConditionType.RocketStorage,
		ProcessCondition.ProcessConditionType.RocketBoard,
		ProcessCondition.ProcessConditionType.RocketFlight
	};

	// Token: 0x04003029 RID: 12329
	private int lastConditionTypeSucceeded = -1;

	// Token: 0x0400302A RID: 12330
	private List<ProcessCondition> returnConditions = new List<ProcessCondition>();
}
