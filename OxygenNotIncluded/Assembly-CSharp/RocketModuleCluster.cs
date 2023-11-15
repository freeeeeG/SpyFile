using System;
using UnityEngine;

// Token: 0x020009B6 RID: 2486
public class RocketModuleCluster : RocketModule
{
	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06004A0D RID: 18957 RVA: 0x001A13D1 File Offset: 0x0019F5D1
	// (set) Token: 0x06004A0E RID: 18958 RVA: 0x001A13D9 File Offset: 0x0019F5D9
	public CraftModuleInterface CraftInterface
	{
		get
		{
			return this._craftInterface;
		}
		set
		{
			this._craftInterface = value;
			if (this._craftInterface != null)
			{
				base.name = base.name + ": " + this.GetParentRocketName();
			}
		}
	}

	// Token: 0x06004A0F RID: 18959 RVA: 0x001A140C File Offset: 0x0019F60C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<RocketModuleCluster>(2121280625, RocketModuleCluster.OnNewConstructionDelegate);
	}

	// Token: 0x06004A10 RID: 18960 RVA: 0x001A1428 File Offset: 0x0019F628
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.CraftInterface == null && DlcManager.FeatureClusterSpaceEnabled())
		{
			this.RegisterWithCraftModuleInterface();
		}
		if (base.GetComponent<RocketEngine>() == null && base.GetComponent<RocketEngineCluster>() == null && base.GetComponent<BuildingUnderConstruction>() == null)
		{
			base.Subscribe<RocketModuleCluster>(1655598572, RocketModuleCluster.OnLaunchConditionChangedDelegate);
			base.Subscribe<RocketModuleCluster>(-887025858, RocketModuleCluster.OnLandDelegate);
		}
	}

	// Token: 0x06004A11 RID: 18961 RVA: 0x001A14A4 File Offset: 0x0019F6A4
	protected void OnNewConstruction(object data)
	{
		Constructable constructable = (Constructable)data;
		if (constructable == null)
		{
			return;
		}
		RocketModuleCluster component = constructable.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return;
		}
		if (component.CraftInterface != null)
		{
			component.CraftInterface.AddModule(this);
		}
	}

	// Token: 0x06004A12 RID: 18962 RVA: 0x001A14F0 File Offset: 0x0019F6F0
	private void RegisterWithCraftModuleInterface()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			if (!(gameObject == base.gameObject))
			{
				RocketModuleCluster component = gameObject.GetComponent<RocketModuleCluster>();
				if (component != null)
				{
					component.CraftInterface.AddModule(this);
					break;
				}
			}
		}
	}

	// Token: 0x06004A13 RID: 18963 RVA: 0x001A1570 File Offset: 0x0019F770
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.CraftInterface.RemoveModule(this);
	}

	// Token: 0x06004A14 RID: 18964 RVA: 0x001A1584 File Offset: 0x0019F784
	public override LaunchConditionManager FindLaunchConditionManager()
	{
		return this.CraftInterface.FindLaunchConditionManager();
	}

	// Token: 0x06004A15 RID: 18965 RVA: 0x001A1591 File Offset: 0x0019F791
	public override string GetParentRocketName()
	{
		if (this.CraftInterface != null)
		{
			return this.CraftInterface.GetComponent<Clustercraft>().Name;
		}
		return this.parentRocketName;
	}

	// Token: 0x06004A16 RID: 18966 RVA: 0x001A15B8 File Offset: 0x0019F7B8
	private void OnLaunchConditionChanged(object data)
	{
		this.UpdateAnimations();
	}

	// Token: 0x06004A17 RID: 18967 RVA: 0x001A15C0 File Offset: 0x0019F7C0
	private void OnLand(object data)
	{
		this.UpdateAnimations();
	}

	// Token: 0x06004A18 RID: 18968 RVA: 0x001A15C8 File Offset: 0x0019F7C8
	protected void UpdateAnimations()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Clustercraft clustercraft = (this.CraftInterface == null) ? null : this.CraftInterface.GetComponent<Clustercraft>();
		if (clustercraft != null && clustercraft.Status == Clustercraft.CraftStatus.Launching && component.HasAnimation("launch"))
		{
			component.ClearQueue();
			if (component.HasAnimation("launch_pre"))
			{
				component.Play("launch_pre", KAnim.PlayMode.Once, 1f, 0f);
			}
			component.Queue("launch", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (this.CraftInterface != null && this.CraftInterface.CheckPreppedForLaunch())
		{
			component.initialAnim = "ready_to_launch";
			component.Play("pre_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("ready_to_launch", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		component.initialAnim = "grounded";
		component.Play("pst_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("grounded", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x040030BF RID: 12479
	public RocketModulePerformance performanceStats;

	// Token: 0x040030C0 RID: 12480
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnNewConstructionDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnNewConstruction(data);
	});

	// Token: 0x040030C1 RID: 12481
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnLaunchConditionChangedDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnLaunchConditionChanged(data);
	});

	// Token: 0x040030C2 RID: 12482
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnLandDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnLand(data);
	});

	// Token: 0x040030C3 RID: 12483
	private CraftModuleInterface _craftInterface;
}
