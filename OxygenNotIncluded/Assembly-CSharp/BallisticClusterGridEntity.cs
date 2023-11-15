using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000261 RID: 609
public class BallisticClusterGridEntity : ClusterGridEntity
{
	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00045957 File Offset: 0x00043B57
	public override string Name
	{
		get
		{
			return Strings.Get(this.nameKey);
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00045969 File Offset: 0x00043B69
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Payload;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0004596C File Offset: 0x00043B6C
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.clusterAnimName),
					initialAnim = "idle_loop",
					symbolSwapTarget = this.clusterAnimSymbolSwapTarget,
					symbolSwapSymbol = this.clusterAnimSymbolSwapSymbol
				}
			};
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000C3F RID: 3135 RVA: 0x000459CA File Offset: 0x00043BCA
	public override bool IsVisible
	{
		get
		{
			return !base.gameObject.HasTag(GameTags.ClusterEntityGrounded);
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000C40 RID: 3136 RVA: 0x000459DF File Offset: 0x00043BDF
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x000459E2 File Offset: 0x00043BE2
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x000459E8 File Offset: 0x00043BE8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.m_clusterTraveler.getSpeedCB = new Func<float>(this.GetSpeed);
		this.m_clusterTraveler.getCanTravelCB = new Func<bool, bool>(this.CanTravel);
		this.m_clusterTraveler.onTravelCB = null;
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x00045A35 File Offset: 0x00043C35
	private float GetSpeed()
	{
		return 10f;
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x00045A3C File Offset: 0x00043C3C
	private bool CanTravel(bool tryingToLand)
	{
		return this.HasTag(GameTags.EntityInSpace);
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x00045A49 File Offset: 0x00043C49
	public void Configure(AxialI source, AxialI destination)
	{
		this.m_location = source;
		this.m_destionationSelector.SetDestination(destination);
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x00045A5E File Offset: 0x00043C5E
	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x00045A6B File Offset: 0x00043C6B
	public override bool ShowProgressBar()
	{
		return this.m_selectable.IsSelected && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x00045A87 File Offset: 0x00043C87
	public override float GetProgress()
	{
		return this.m_clusterTraveler.GetMoveProgress();
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x00045A94 File Offset: 0x00043C94
	public void SwapSymbolFromSameAnim(string targetSymbolName, string swappedSymbolName)
	{
		this.clusterAnimSymbolSwapTarget = targetSymbolName;
		this.clusterAnimSymbolSwapSymbol = swappedSymbolName;
	}

	// Token: 0x04000748 RID: 1864
	[MyCmpReq]
	private ClusterDestinationSelector m_destionationSelector;

	// Token: 0x04000749 RID: 1865
	[MyCmpReq]
	private ClusterTraveler m_clusterTraveler;

	// Token: 0x0400074A RID: 1866
	[SerializeField]
	public string clusterAnimName;

	// Token: 0x0400074B RID: 1867
	[SerializeField]
	public StringKey nameKey;

	// Token: 0x0400074C RID: 1868
	private string clusterAnimSymbolSwapTarget;

	// Token: 0x0400074D RID: 1869
	private string clusterAnimSymbolSwapSymbol;
}
