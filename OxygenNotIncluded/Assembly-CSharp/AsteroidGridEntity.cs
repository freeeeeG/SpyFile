using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

// Token: 0x020009C0 RID: 2496
public class AsteroidGridEntity : ClusterGridEntity
{
	// Token: 0x06004A90 RID: 19088 RVA: 0x001A4006 File Offset: 0x001A2206
	public override bool ShowName()
	{
		return true;
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06004A91 RID: 19089 RVA: 0x001A4009 File Offset: 0x001A2209
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x06004A92 RID: 19090 RVA: 0x001A4011 File Offset: 0x001A2211
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Asteroid;
		}
	}

	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x06004A93 RID: 19091 RVA: 0x001A4014 File Offset: 0x001A2214
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			List<ClusterGridEntity.AnimConfig> list = new List<ClusterGridEntity.AnimConfig>();
			ClusterGridEntity.AnimConfig item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim(this.m_asteroidAnim.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : this.m_asteroidAnim),
				initialAnim = "idle_loop"
			};
			list.Add(item);
			item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim("orbit_kanim"),
				initialAnim = "orbit"
			};
			list.Add(item);
			item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim("shower_asteroid_current_kanim"),
				initialAnim = "off",
				playMode = KAnim.PlayMode.Once
			};
			list.Add(item);
			return list;
		}
	}

	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x06004A94 RID: 19092 RVA: 0x001A40D6 File Offset: 0x001A22D6
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x06004A95 RID: 19093 RVA: 0x001A40D9 File Offset: 0x001A22D9
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x06004A96 RID: 19094 RVA: 0x001A40DC File Offset: 0x001A22DC
	public void Init(string name, AxialI location, string asteroidTypeId)
	{
		this.m_name = name;
		this.m_location = location;
		this.m_asteroidAnim = asteroidTypeId;
	}

	// Token: 0x06004A97 RID: 19095 RVA: 0x001A40F4 File Offset: 0x001A22F4
	protected override void OnSpawn()
	{
		Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Subscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
		Game.Instance.Subscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
		if (ClusterGrid.Instance.IsCellVisible(this.m_location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.m_location, 1);
		}
		base.OnSpawn();
	}

	// Token: 0x06004A98 RID: 19096 RVA: 0x001A41A0 File Offset: 0x001A23A0
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Unsubscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
		Game.Instance.Unsubscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
		base.OnCleanUp();
	}

	// Token: 0x06004A99 RID: 19097 RVA: 0x001A4220 File Offset: 0x001A2420
	public void OnClusterLocationChanged(object data)
	{
		if (this.m_worldContainer.IsDiscovered)
		{
			return;
		}
		if (!ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			return;
		}
		Clustercraft component = ((ClusterLocationChangedEvent)data).entity.GetComponent<Clustercraft>();
		if (component == null)
		{
			return;
		}
		if (component.GetOrbitAsteroid() == this)
		{
			this.m_worldContainer.SetDiscovered(true);
		}
	}

	// Token: 0x06004A9A RID: 19098 RVA: 0x001A4283 File Offset: 0x001A2483
	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		base.OnClusterMapIconShown(levelUsed);
		if (levelUsed == ClusterRevealLevel.Visible)
		{
			this.RefreshMeteorShowerEffect();
		}
	}

	// Token: 0x06004A9B RID: 19099 RVA: 0x001A4296 File Offset: 0x001A2496
	private void OnMeteorShowerEventChanged(object _worldID)
	{
		if ((int)_worldID == this.m_worldContainer.id)
		{
			this.RefreshMeteorShowerEffect();
		}
	}

	// Token: 0x06004A9C RID: 19100 RVA: 0x001A42B4 File Offset: 0x001A24B4
	public void RefreshMeteorShowerEffect()
	{
		if (ClusterMapScreen.Instance == null)
		{
			return;
		}
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		if (entityVisAnim == null)
		{
			return;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null)
		{
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(this.m_worldContainer.id, ref list);
			bool flag = false;
			string s = "off";
			foreach (GameplayEventInstance gameplayEventInstance in list)
			{
				if (gameplayEventInstance != null && gameplayEventInstance.smi is MeteorShowerEvent.StatesInstance)
				{
					MeteorShowerEvent.StatesInstance statesInstance = gameplayEventInstance.smi as MeteorShowerEvent.StatesInstance;
					if (statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
					{
						flag = true;
						s = "idle_loop";
						break;
					}
				}
			}
			animController.Play(s, flag ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06004A9D RID: 19101 RVA: 0x001A43BC File Offset: 0x001A25BC
	public void OnFogOfWarRevealed(object data = null)
	{
		if (data == null)
		{
			return;
		}
		if ((AxialI)data != this.m_location)
		{
			return;
		}
		if (!ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			return;
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			WorldDetectedMessage message = new WorldDetectedMessage(this.m_worldContainer);
			MusicManager.instance.PlaySong("Stinger_WorldDetected", false);
			Messenger.Instance.QueueMessage(message);
			if (!this.m_worldContainer.IsDiscovered)
			{
				using (IEnumerator enumerator = Components.Clustercrafts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((Clustercraft)enumerator.Current).GetOrbitAsteroid() == this)
						{
							this.m_worldContainer.SetDiscovered(true);
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x040030F7 RID: 12535
	public static string DEFAULT_ASTEROID_ICON_ANIM = "asteroid_sandstone_start_kanim";

	// Token: 0x040030F8 RID: 12536
	[MyCmpReq]
	private WorldContainer m_worldContainer;

	// Token: 0x040030F9 RID: 12537
	[Serialize]
	private string m_name;

	// Token: 0x040030FA RID: 12538
	[Serialize]
	private string m_asteroidAnim;
}
