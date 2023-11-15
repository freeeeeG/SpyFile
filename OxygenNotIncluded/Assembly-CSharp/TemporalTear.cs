using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x020009BE RID: 2494
[SerializationConfig(MemberSerialization.OptIn)]
public class TemporalTear : ClusterGridEntity
{
	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x06004A77 RID: 19063 RVA: 0x001A3A28 File Offset: 0x001A1C28
	public override string Name
	{
		get
		{
			return Db.Get().SpaceDestinationTypes.Wormhole.typeName;
		}
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06004A78 RID: 19064 RVA: 0x001A3A3E File Offset: 0x001A1C3E
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x06004A79 RID: 19065 RVA: 0x001A3A44 File Offset: 0x001A1C44
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("temporal_tear_kanim"),
					initialAnim = "closed_loop"
				}
			};
		}
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06004A7A RID: 19066 RVA: 0x001A3A87 File Offset: 0x001A1C87
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x06004A7B RID: 19067 RVA: 0x001A3A8A File Offset: 0x001A1C8A
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x06004A7C RID: 19068 RVA: 0x001A3A8D File Offset: 0x001A1C8D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ClusterManager.Instance.GetComponent<ClusterPOIManager>().RegisterTemporalTear(this);
		this.UpdateStatus();
	}

	// Token: 0x06004A7D RID: 19069 RVA: 0x001A3AAC File Offset: 0x001A1CAC
	public void UpdateStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		ClusterMapVisualizer clusterMapVisualizer = null;
		if (ClusterMapScreen.Instance != null)
		{
			clusterMapVisualizer = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		}
		if (this.IsOpen())
		{
			if (clusterMapVisualizer != null)
			{
				clusterMapVisualizer.PlayAnim("open_loop", KAnim.PlayMode.Loop);
			}
			component.RemoveStatusItem(Db.Get().MiscStatusItems.TearClosed, false);
			component.AddStatusItem(Db.Get().MiscStatusItems.TearOpen, null);
			return;
		}
		if (clusterMapVisualizer != null)
		{
			clusterMapVisualizer.PlayAnim("closed_loop", KAnim.PlayMode.Loop);
		}
		component.RemoveStatusItem(Db.Get().MiscStatusItems.TearOpen, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.TearClosed, null);
	}

	// Token: 0x06004A7E RID: 19070 RVA: 0x001A3B6F File Offset: 0x001A1D6F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06004A7F RID: 19071 RVA: 0x001A3B78 File Offset: 0x001A1D78
	public void ConsumeCraft(Clustercraft craft)
	{
		if (this.m_open && craft.Location == base.Location && !craft.IsFlightInProgress())
		{
			for (int i = 0; i < Components.MinionIdentities.Count; i++)
			{
				MinionIdentity minionIdentity = Components.MinionIdentities[i];
				if (minionIdentity.GetMyWorldId() == craft.ModuleInterface.GetInteriorWorld().id)
				{
					Util.KDestroyGameObject(minionIdentity.gameObject);
				}
			}
			craft.DestroyCraftAndModules();
			this.m_hasConsumedCraft = true;
		}
	}

	// Token: 0x06004A80 RID: 19072 RVA: 0x001A3BF9 File Offset: 0x001A1DF9
	public void Open()
	{
		this.m_open = true;
		this.UpdateStatus();
	}

	// Token: 0x06004A81 RID: 19073 RVA: 0x001A3C08 File Offset: 0x001A1E08
	public bool IsOpen()
	{
		return this.m_open;
	}

	// Token: 0x06004A82 RID: 19074 RVA: 0x001A3C10 File Offset: 0x001A1E10
	public bool HasConsumedCraft()
	{
		return this.m_hasConsumedCraft;
	}

	// Token: 0x040030ED RID: 12525
	[Serialize]
	private bool m_open;

	// Token: 0x040030EE RID: 12526
	[Serialize]
	private bool m_hasConsumedCraft;
}
