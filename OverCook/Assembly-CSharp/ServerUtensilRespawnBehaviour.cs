using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000620 RID: 1568
public class ServerUtensilRespawnBehaviour : ServerSynchroniserBase, IRespawnBehaviour, ISurfacePlacementNotified
{
	// Token: 0x06001DAB RID: 7595 RVA: 0x0008D671 File Offset: 0x0008BA71
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_utensilRespawnBehaviour = (UtensilRespawnBehaviour)synchronisedObject;
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x0008D67F File Offset: 0x0008BA7F
	public override EntityType GetEntityType()
	{
		return EntityType.RespawnBehaviour;
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x0008D684 File Offset: 0x0008BA84
	public IEnumerator RespawnCoroutine(ServerRespawnCollider _collider)
	{
		if (this.m_isRespawning)
		{
			yield break;
		}
		this.m_isRespawning = true;
		ServerPhysicalAttachment attachment = base.gameObject.RequestComponent<ServerPhysicalAttachment>();
		ServerUtensilRespawnBehaviour.ReleaseFromAttached(attachment);
		if (attachment)
		{
			attachment.ManualDisable(true);
		}
		this.m_ServerData.m_RespawnType = RespawnCollider.RespawnType.FallDeath;
		this.m_ServerData.m_Phase = RespawnMessage.Phase.Begin;
		this.SendServerEvent(this.m_ServerData);
		ServerContentsDisposalBehaviour disposal = base.gameObject.RequestComponent<ServerContentsDisposalBehaviour>();
		if (disposal != null)
		{
			disposal.AddToDisposer(null);
		}
		base.gameObject.SetActive(false);
		IEnumerator wait = CoroutineUtils.TimerRoutine(this.m_utensilRespawnBehaviour.m_respawnTime, base.gameObject.layer);
		while (wait.MoveNext())
		{
			yield return null;
		}
		ServerAttachStation freeStation = null;
		while (freeStation == null)
		{
			yield return null;
			freeStation = this.GetFreeAttachStation();
		}
		if (attachment)
		{
			attachment.ManualEnable();
		}
		this.AddItemToStation(freeStation);
		if (this == null || base.gameObject == null)
		{
			yield break;
		}
		Collider collider = base.gameObject.GetComponent<Collider>();
		if (collider != null)
		{
			collider.enabled = true;
		}
		this.m_ServerData.m_Phase = RespawnMessage.Phase.End;
		this.m_ServerData.m_RespawnPosition = freeStation.GetAttachPoint(base.gameObject).position;
		this.SendServerEvent(this.m_ServerData);
		this.m_isRespawning = false;
		yield break;
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x0008D69F File Offset: 0x0008BA9F
	public void OnSurfacePlacement(ServerAttachStation _station)
	{
		if (this.m_IdealSpawnLocation == null)
		{
			this.m_IdealSpawnLocation = _station;
		}
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x0008D6B9 File Offset: 0x0008BAB9
	public void OnSurfaceDeplacement(ServerAttachStation _station)
	{
	}

	// Token: 0x06001DB0 RID: 7600 RVA: 0x0008D6BB File Offset: 0x0008BABB
	public void SetIdealRespawnLocation(ServerAttachStation _station)
	{
		this.m_IdealSpawnLocation = _station;
	}

	// Token: 0x06001DB1 RID: 7601 RVA: 0x0008D6C4 File Offset: 0x0008BAC4
	public ServerAttachStation GetIdealRespawnLocation()
	{
		return this.m_IdealSpawnLocation;
	}

	// Token: 0x06001DB2 RID: 7602 RVA: 0x0008D6CC File Offset: 0x0008BACC
	public virtual float GetStationRespawnPriority(ServerAttachStation _station)
	{
		return this.GetRespawnDistance(_station.transform.position);
	}

	// Token: 0x06001DB3 RID: 7603 RVA: 0x0008D6E0 File Offset: 0x0008BAE0
	protected float GetRespawnDistance(Vector3 _position)
	{
		Vector3 b = (!(this.m_IdealSpawnLocation != null)) ? base.transform.position : this.m_IdealSpawnLocation.transform.position;
		return (_position - b).magnitude;
	}

	// Token: 0x06001DB4 RID: 7604 RVA: 0x0008D72E File Offset: 0x0008BB2E
	protected bool IsInLevelBounds(ServerAttachStation _station, LevelConfigBase _levelConfig = null)
	{
		if (_levelConfig == null)
		{
			_levelConfig = GameUtils.GetLevelConfig();
		}
		return !_levelConfig.m_enableRespawnBounds || LevelBounds.ActiveBoundsContain(_station.transform.position);
	}

	// Token: 0x06001DB5 RID: 7605 RVA: 0x0008D764 File Offset: 0x0008BB64
	protected virtual bool CanRespawnOnStation(ServerAttachStation _attachStation)
	{
		return !_attachStation.CompareTag("CookingStation") && !_attachStation.CompareTag("PlateReturn") && !_attachStation.CompareTag("PlateStation") && !(_attachStation.gameObject.RequestComponent<RubbishBin>() != null) && !(_attachStation.gameObject.RequestComponent<ConveyorStation>() != null) && !(_attachStation.gameObject.RequestComponent<WashingStation>() != null) && _attachStation.CanAttachToSelf(base.gameObject, default(PlacementContext));
	}

	// Token: 0x06001DB6 RID: 7606 RVA: 0x0008D7FC File Offset: 0x0008BBFC
	protected virtual ServerAttachStation GetFreeAttachStation()
	{
		ServerAttachStation result = null;
		float num = float.PositiveInfinity;
		LevelConfigBase levelConfig = GameUtils.GetLevelConfig();
		if (this.m_allRespawnStations == null)
		{
			this.m_allRespawnStations = this.GetRespawnStations();
		}
		for (int i = 0; i < this.m_allRespawnStations.Length; i++)
		{
			ServerAttachStation serverAttachStation = this.m_allRespawnStations[i];
			if (serverAttachStation.enabled && serverAttachStation.gameObject.activeInHierarchy && this.IsInLevelBounds(serverAttachStation, levelConfig) && this.CanRespawnOnStation(serverAttachStation))
			{
				float stationRespawnPriority = this.GetStationRespawnPriority(serverAttachStation);
				if (stationRespawnPriority < num)
				{
					result = serverAttachStation;
					num = stationRespawnPriority;
				}
			}
		}
		return result;
	}

	// Token: 0x06001DB7 RID: 7607 RVA: 0x0008D8A7 File Offset: 0x0008BCA7
	protected virtual ServerAttachStation[] GetRespawnStations()
	{
		return UnityEngine.Object.FindObjectsOfType<ServerAttachStation>();
	}

	// Token: 0x06001DB8 RID: 7608 RVA: 0x0008D8B0 File Offset: 0x0008BCB0
	private static void ReleaseFromAttached(ServerPhysicalAttachment _attachment)
	{
		if (_attachment.IsAttached())
		{
			ServerHandlePickupReferral serverHandlePickupReferral = _attachment.gameObject.RequestComponent<ServerHandlePickupReferral>();
			if (serverHandlePickupReferral != null)
			{
				IHandlePickup handlePickupReferree = serverHandlePickupReferral.GetHandlePickupReferree();
				if (handlePickupReferree != null)
				{
					if (handlePickupReferree as ServerAttachStation != null)
					{
						ServerAttachStation serverAttachStation = handlePickupReferree as ServerAttachStation;
						if (serverAttachStation.HasItem())
						{
							serverAttachStation.TakeItem();
						}
					}
					else if (handlePickupReferree is ServerPlayerAttachmentCarrier.BlockPickup)
					{
						ServerPlayerAttachmentCarrier.BlockPickup blockPickup = handlePickupReferree as ServerPlayerAttachmentCarrier.BlockPickup;
						blockPickup.ForceDetach();
					}
				}
			}
		}
	}

	// Token: 0x06001DB9 RID: 7609 RVA: 0x0008D934 File Offset: 0x0008BD34
	protected virtual void AddItemToStation(ServerAttachStation _station)
	{
		_station.AddItem(base.gameObject, -_station.transform.forward.XZ(), default(PlacementContext));
	}

	// Token: 0x040016ED RID: 5869
	protected UtensilRespawnBehaviour m_utensilRespawnBehaviour;

	// Token: 0x040016EE RID: 5870
	private RespawnMessage m_ServerData = new RespawnMessage();

	// Token: 0x040016EF RID: 5871
	protected ServerAttachStation m_IdealSpawnLocation;

	// Token: 0x040016F0 RID: 5872
	private bool m_isRespawning;

	// Token: 0x040016F1 RID: 5873
	private ServerAttachStation[] m_allRespawnStations;
}
