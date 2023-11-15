using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x020009B1 RID: 2481
public class RocketClusterDestinationSelector : ClusterDestinationSelector
{
	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x060049DD RID: 18909 RVA: 0x0019FFB8 File Offset: 0x0019E1B8
	// (set) Token: 0x060049DE RID: 18910 RVA: 0x0019FFC0 File Offset: 0x0019E1C0
	public bool Repeat
	{
		get
		{
			return this.m_repeat;
		}
		set
		{
			this.m_repeat = value;
		}
	}

	// Token: 0x060049DF RID: 18911 RVA: 0x0019FFC9 File Offset: 0x0019E1C9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<RocketClusterDestinationSelector>(-1277991738, this.OnLaunchDelegate);
	}

	// Token: 0x060049E0 RID: 18912 RVA: 0x0019FFE3 File Offset: 0x0019E1E3
	protected override void OnSpawn()
	{
		if (this.isHarvesting)
		{
			this.WaitForPOIHarvest();
		}
	}

	// Token: 0x060049E1 RID: 18913 RVA: 0x0019FFF4 File Offset: 0x0019E1F4
	public LaunchPad GetDestinationPad(AxialI destination)
	{
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
		if (this.m_launchPad.ContainsKey(asteroidWorldIdAtLocation))
		{
			return this.m_launchPad[asteroidWorldIdAtLocation].Get();
		}
		return null;
	}

	// Token: 0x060049E2 RID: 18914 RVA: 0x001A0029 File Offset: 0x0019E229
	public LaunchPad GetDestinationPad()
	{
		return this.GetDestinationPad(this.m_destination);
	}

	// Token: 0x060049E3 RID: 18915 RVA: 0x001A0037 File Offset: 0x0019E237
	public override void SetDestination(AxialI location)
	{
		base.SetDestination(location);
	}

	// Token: 0x060049E4 RID: 18916 RVA: 0x001A0040 File Offset: 0x0019E240
	public void SetDestinationPad(LaunchPad pad)
	{
		Debug.Assert(pad == null || ClusterGrid.Instance.IsInRange(pad.GetMyWorldLocation(), this.m_destination, 1), "Tried sending a rocket to a launchpad that wasn't its destination world.");
		if (pad != null)
		{
			this.AddDestinationPad(pad.GetMyWorldLocation(), pad);
			base.SetDestination(pad.GetMyWorldLocation());
		}
		base.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationChanged, null);
	}

	// Token: 0x060049E5 RID: 18917 RVA: 0x001A00B0 File Offset: 0x0019E2B0
	private void AddDestinationPad(AxialI location, LaunchPad pad)
	{
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(location);
		if (asteroidWorldIdAtLocation < 0)
		{
			return;
		}
		if (!this.m_launchPad.ContainsKey(asteroidWorldIdAtLocation))
		{
			this.m_launchPad.Add(asteroidWorldIdAtLocation, new Ref<LaunchPad>());
		}
		this.m_launchPad[asteroidWorldIdAtLocation].Set(pad);
	}

	// Token: 0x060049E6 RID: 18918 RVA: 0x001A00FC File Offset: 0x0019E2FC
	protected override void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		if (clusterLocationChangedEvent.newLocation == this.m_destination)
		{
			base.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationReached, null);
			if (this.m_repeat)
			{
				if (ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(clusterLocationChangedEvent.newLocation, EntityLayer.POI) != null && this.CanRocketHarvest())
				{
					this.WaitForPOIHarvest();
					return;
				}
				this.SetUpReturnTrip();
			}
		}
	}

	// Token: 0x060049E7 RID: 18919 RVA: 0x001A0170 File Offset: 0x0019E370
	private void SetUpReturnTrip()
	{
		this.AddDestinationPad(this.m_prevDestination, this.m_prevLaunchPad.Get());
		this.m_destination = this.m_prevDestination;
		this.m_prevDestination = base.GetComponent<Clustercraft>().Location;
		this.m_prevLaunchPad.Set(base.GetComponent<CraftModuleInterface>().CurrentPad);
	}

	// Token: 0x060049E8 RID: 18920 RVA: 0x001A01C8 File Offset: 0x0019E3C8
	private bool CanRocketHarvest()
	{
		bool flag = false;
		List<ResourceHarvestModule.StatesInstance> allResourceHarvestModules = base.GetComponent<Clustercraft>().GetAllResourceHarvestModules();
		if (allResourceHarvestModules.Count > 0)
		{
			using (List<ResourceHarvestModule.StatesInstance>.Enumerator enumerator = allResourceHarvestModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CheckIfCanHarvest())
					{
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			List<ArtifactHarvestModule.StatesInstance> allArtifactHarvestModules = base.GetComponent<Clustercraft>().GetAllArtifactHarvestModules();
			if (allArtifactHarvestModules.Count > 0)
			{
				using (List<ArtifactHarvestModule.StatesInstance>.Enumerator enumerator2 = allArtifactHarvestModules.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.CheckIfCanHarvest())
						{
							flag = true;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x060049E9 RID: 18921 RVA: 0x001A0288 File Offset: 0x0019E488
	private void OnStorageChange(object data)
	{
		if (!this.CanRocketHarvest())
		{
			this.isHarvesting = false;
			foreach (Ref<RocketModuleCluster> @ref in base.GetComponent<Clustercraft>().ModuleInterface.ClusterModules)
			{
				if (@ref.Get().GetComponent<Storage>())
				{
					base.Unsubscribe(@ref.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
				}
			}
			this.SetUpReturnTrip();
		}
	}

	// Token: 0x060049EA RID: 18922 RVA: 0x001A0324 File Offset: 0x0019E524
	private void WaitForPOIHarvest()
	{
		this.isHarvesting = true;
		foreach (Ref<RocketModuleCluster> @ref in base.GetComponent<Clustercraft>().ModuleInterface.ClusterModules)
		{
			if (@ref.Get().GetComponent<Storage>())
			{
				base.Subscribe(@ref.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
			}
		}
	}

	// Token: 0x060049EB RID: 18923 RVA: 0x001A03B0 File Offset: 0x0019E5B0
	private void OnLaunch(object data)
	{
		CraftModuleInterface component = base.GetComponent<CraftModuleInterface>();
		this.m_prevLaunchPad.Set(component.CurrentPad);
		Clustercraft component2 = base.GetComponent<Clustercraft>();
		this.m_prevDestination = component2.Location;
	}

	// Token: 0x04003086 RID: 12422
	[Serialize]
	private Dictionary<int, Ref<LaunchPad>> m_launchPad = new Dictionary<int, Ref<LaunchPad>>();

	// Token: 0x04003087 RID: 12423
	[Serialize]
	private bool m_repeat;

	// Token: 0x04003088 RID: 12424
	[Serialize]
	private AxialI m_prevDestination;

	// Token: 0x04003089 RID: 12425
	[Serialize]
	private Ref<LaunchPad> m_prevLaunchPad = new Ref<LaunchPad>();

	// Token: 0x0400308A RID: 12426
	[Serialize]
	private bool isHarvesting;

	// Token: 0x0400308B RID: 12427
	private EventSystem.IntraObjectHandler<RocketClusterDestinationSelector> OnLaunchDelegate = new EventSystem.IntraObjectHandler<RocketClusterDestinationSelector>(delegate(RocketClusterDestinationSelector cmp, object data)
	{
		cmp.OnLaunch(data);
	});
}
