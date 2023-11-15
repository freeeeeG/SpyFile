using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200081C RID: 2076
public class ServerWorkableItem : ServerSynchroniserBase, ISurfacePlacementNotified
{
	// Token: 0x060027D4 RID: 10196 RVA: 0x000BACF7 File Offset: 0x000B90F7
	public override EntityType GetEntityType()
	{
		return EntityType.Workable;
	}

	// Token: 0x060027D5 RID: 10197 RVA: 0x000BACFA File Offset: 0x000B90FA
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_workable = (WorkableItem)synchronisedObject;
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_workable.GetNextPrefab());
		this.m_chopsPerSlice = this.m_workable.GetChopTimeMultiplier(ServerUserSystem.m_Users.Count);
	}

	// Token: 0x060027D6 RID: 10198 RVA: 0x000BAD3C File Offset: 0x000B913C
	private void SynchroniseClientState()
	{
		this.m_data.m_onWorkstation = this.m_onWorkstation;
		this.m_data.m_progress = this.m_progress;
		this.m_data.m_subProgress = this.m_subProgress;
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060027D7 RID: 10199 RVA: 0x000BAD88 File Offset: 0x000B9188
	public void OnSurfacePlacement(ServerAttachStation _station)
	{
		this.m_onWorkstation = true;
		this.SynchroniseClientState();
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x000BAD97 File Offset: 0x000B9197
	public void OnSurfaceDeplacement(ServerAttachStation _station)
	{
		this.m_onWorkstation = false;
		this.SynchroniseClientState();
	}

	// Token: 0x060027D9 RID: 10201 RVA: 0x000BADA6 File Offset: 0x000B91A6
	public float GetProgress()
	{
		return (float)this.m_progress / (float)Mathf.Max(this.m_workable.m_stages - 1, 1);
	}

	// Token: 0x060027DA RID: 10202 RVA: 0x000BADC4 File Offset: 0x000B91C4
	public bool HasFinished()
	{
		return this.m_progress == this.m_workable.m_stages - 1;
	}

	// Token: 0x060027DB RID: 10203 RVA: 0x000BADDC File Offset: 0x000B91DC
	public void DoWork(ServerAttachStation _station, GameObject _worker)
	{
		if (!this.HasFinished())
		{
			this.m_subProgress++;
			if (this.m_subProgress >= this.m_chopsPerSlice && !GameUtils.GetDebugConfig().m_infiniteChopping)
			{
				this.m_subProgress = 0;
				this.DoWork(_station, _worker, 1);
				if (this.HasFinished())
				{
					ServerMessenger.Achievement(_worker, 5, 1);
				}
			}
		}
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x000BAE48 File Offset: 0x000B9248
	public void DoWork(ServerAttachStation _station, GameObject _worker, int _progress)
	{
		if (!this.HasFinished())
		{
			this.m_subProgress = 0;
			this.m_progress = Mathf.Min(this.m_progress + _progress, this.m_workable.m_stages - 1);
			if (this.HasFinished())
			{
				ServerLimitedQuantityItem serverLimitedQuantityItem = this.m_workable.gameObject.RequestComponent<ServerLimitedQuantityItem>();
				if (serverLimitedQuantityItem != null)
				{
					serverLimitedQuantityItem.AddInvincibilityCondition(() => true);
				}
				Vector3 localPosition = base.transform.localPosition;
				Quaternion localRotation = base.transform.localRotation;
				IAttachment component = NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_workable.GetNextPrefab()).GetComponent<IAttachment>();
				GameObject gameObject = _station.TakeItem();
				_station.AddItem(component.AccessGameObject(), Vector2.up, new PlacementContext(PlacementContext.Source.Player));
				component.AccessGameObject().transform.localPosition = localPosition;
				component.AccessGameObject().transform.localRotation = localRotation;
				NetworkUtils.DestroyObject(base.gameObject);
			}
		}
	}

	// Token: 0x060027DD RID: 10205 RVA: 0x000BAF52 File Offset: 0x000B9352
	private void Awake()
	{
		UserSystemUtils.OnServerChangedGameState = (GenericVoid<GameState, GameStateMessage.GameStatePayload>)Delegate.Combine(UserSystemUtils.OnServerChangedGameState, new GenericVoid<GameState, GameStateMessage.GameStatePayload>(this.OnGameStateChanged));
	}

	// Token: 0x060027DE RID: 10206 RVA: 0x000BAF74 File Offset: 0x000B9374
	private void OnGameStateChanged(GameState _state, GameStateMessage.GameStatePayload payload)
	{
		if (_state == GameState.StartEntities)
		{
			this.m_chopsPerSlice = this.m_workable.GetChopTimeMultiplier(ServerUserSystem.m_Users.Count);
		}
	}

	// Token: 0x060027DF RID: 10207 RVA: 0x000BAF99 File Offset: 0x000B9399
	public override void OnDestroy()
	{
		UserSystemUtils.OnServerChangedGameState = (GenericVoid<GameState, GameStateMessage.GameStatePayload>)Delegate.Remove(UserSystemUtils.OnServerChangedGameState, new GenericVoid<GameState, GameStateMessage.GameStatePayload>(this.OnGameStateChanged));
		base.OnDestroy();
	}

	// Token: 0x04001F51 RID: 8017
	private WorkableItem m_workable;

	// Token: 0x04001F52 RID: 8018
	private WorkableMessage m_data = new WorkableMessage();

	// Token: 0x04001F53 RID: 8019
	private int m_chopsPerSlice = 1;

	// Token: 0x04001F54 RID: 8020
	private bool m_onWorkstation;

	// Token: 0x04001F55 RID: 8021
	private int m_progress;

	// Token: 0x04001F56 RID: 8022
	private int m_subProgress;
}
