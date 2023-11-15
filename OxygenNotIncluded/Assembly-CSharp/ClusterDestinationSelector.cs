using System;
using KSerialization;

// Token: 0x02000990 RID: 2448
public class ClusterDestinationSelector : KMonoBehaviour
{
	// Token: 0x06004844 RID: 18500 RVA: 0x00197BBD File Offset: 0x00195DBD
	protected override void OnPrefabInit()
	{
		base.Subscribe<ClusterDestinationSelector>(-1298331547, this.OnClusterLocationChangedDelegate);
	}

	// Token: 0x06004845 RID: 18501 RVA: 0x00197BD1 File Offset: 0x00195DD1
	protected virtual void OnClusterLocationChanged(object data)
	{
		if (((ClusterLocationChangedEvent)data).newLocation == this.m_destination)
		{
			base.Trigger(1796608350, data);
		}
	}

	// Token: 0x06004846 RID: 18502 RVA: 0x00197BF7 File Offset: 0x00195DF7
	public int GetDestinationWorld()
	{
		return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination);
	}

	// Token: 0x06004847 RID: 18503 RVA: 0x00197C04 File Offset: 0x00195E04
	public AxialI GetDestination()
	{
		return this.m_destination;
	}

	// Token: 0x06004848 RID: 18504 RVA: 0x00197C0C File Offset: 0x00195E0C
	public virtual void SetDestination(AxialI location)
	{
		if (this.requireAsteroidDestination)
		{
			Debug.Assert(ClusterUtil.GetAsteroidWorldIdAtLocation(location) != -1, string.Format("Cannot SetDestination to {0} as there is no world there", location));
		}
		this.m_destination = location;
		base.Trigger(543433792, location);
	}

	// Token: 0x06004849 RID: 18505 RVA: 0x00197C5A File Offset: 0x00195E5A
	public bool HasAsteroidDestination()
	{
		return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination) != -1;
	}

	// Token: 0x0600484A RID: 18506 RVA: 0x00197C6D File Offset: 0x00195E6D
	public virtual bool IsAtDestination()
	{
		return this.GetMyWorldLocation() == this.m_destination;
	}

	// Token: 0x04002FE0 RID: 12256
	[Serialize]
	protected AxialI m_destination;

	// Token: 0x04002FE1 RID: 12257
	public bool assignable;

	// Token: 0x04002FE2 RID: 12258
	public bool requireAsteroidDestination;

	// Token: 0x04002FE3 RID: 12259
	[Serialize]
	public bool canNavigateFogOfWar;

	// Token: 0x04002FE4 RID: 12260
	public bool dodgesHiddenAsteroids;

	// Token: 0x04002FE5 RID: 12261
	public bool requireLaunchPadOnAsteroidDestination;

	// Token: 0x04002FE6 RID: 12262
	public bool shouldPointTowardsPath;

	// Token: 0x04002FE7 RID: 12263
	private EventSystem.IntraObjectHandler<ClusterDestinationSelector> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<ClusterDestinationSelector>(delegate(ClusterDestinationSelector cmp, object data)
	{
		cmp.OnClusterLocationChanged(data);
	});
}
