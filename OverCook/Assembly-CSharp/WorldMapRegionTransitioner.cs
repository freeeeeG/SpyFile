using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BC2 RID: 3010
public class WorldMapRegionTransitioner : MonoBehaviour
{
	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06003D9B RID: 15771 RVA: 0x00126290 File Offset: 0x00124690
	public WorldMapRegion CurrentRegion
	{
		get
		{
			return this.m_currentRegion;
		}
	}

	// Token: 0x06003D9C RID: 15772 RVA: 0x00126298 File Offset: 0x00124698
	public void Start()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003D9D RID: 15773 RVA: 0x001262B2 File Offset: 0x001246B2
	public void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003D9E RID: 15774 RVA: 0x001262CC File Offset: 0x001246CC
	public void RegisterRegionChangeCallback(WorldMapRegionTransitioner.RegionChangeCallback _callback)
	{
		this.m_onRegionChanged = (WorldMapRegionTransitioner.RegionChangeCallback)Delegate.Combine(this.m_onRegionChanged, _callback);
	}

	// Token: 0x06003D9F RID: 15775 RVA: 0x001262E5 File Offset: 0x001246E5
	public void UnregisterRegionChangeCallback(WorldMapRegionTransitioner.RegionChangeCallback _callback)
	{
		this.m_onRegionChanged = (WorldMapRegionTransitioner.RegionChangeCallback)Delegate.Remove(this.m_onRegionChanged, _callback);
	}

	// Token: 0x06003DA0 RID: 15776 RVA: 0x001262FE File Offset: 0x001246FE
	private void OnRegionChanged(WorldMapRegion _newRegion)
	{
		if (_newRegion == null)
		{
			return;
		}
		if (this.m_onRegionChanged != null)
		{
			this.m_onRegionChanged(this.m_currentRegion, _newRegion);
		}
		this.m_currentRegion = _newRegion;
	}

	// Token: 0x06003DA1 RID: 15777 RVA: 0x00126334 File Offset: 0x00124734
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.MapStartEntities)
		{
			this.m_CanPollRegion = true;
		}
	}

	// Token: 0x06003DA2 RID: 15778 RVA: 0x0012635C File Offset: 0x0012475C
	private void Update()
	{
		if (!this.m_CanPollRegion)
		{
			return;
		}
		WorldMapRegion worldMapRegion = WorldMapRegion.FindRegionForPoint(base.transform.position);
		if (worldMapRegion != this.m_currentRegion)
		{
			this.OnRegionChanged(worldMapRegion);
		}
	}

	// Token: 0x0400317D RID: 12669
	private WorldMapRegion m_currentRegion;

	// Token: 0x0400317E RID: 12670
	private WorldMapRegionTransitioner.RegionChangeCallback m_onRegionChanged;

	// Token: 0x0400317F RID: 12671
	private bool m_CanPollRegion;

	// Token: 0x02000BC3 RID: 3011
	// (Invoke) Token: 0x06003DA4 RID: 15780
	public delegate void RegionChangeCallback(WorldMapRegion _oldRegion, WorldMapRegion _newRegion);
}
