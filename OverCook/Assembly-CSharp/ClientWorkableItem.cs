using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200081D RID: 2077
public class ClientWorkableItem : ClientSynchroniserBase, IClientSurfacePlacementNotified
{
	// Token: 0x060027E2 RID: 10210 RVA: 0x000BAFD3 File Offset: 0x000B93D3
	public override EntityType GetEntityType()
	{
		return EntityType.Workable;
	}

	// Token: 0x060027E3 RID: 10211 RVA: 0x000BAFD8 File Offset: 0x000B93D8
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_workable = (WorkableItem)synchronisedObject;
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_workable.GetNextPrefab());
		this.m_ingredientMeshVisibility = base.gameObject.RequestComponent<ClientIngredientMeshVisibility>();
		GameObject gameObject = GameUtils.InstantiateUIController(this.m_workable.m_progressUIPrefab.gameObject, "HoverIconCanvas");
		this.m_guibar = gameObject.GetComponent<ProgressUIController>();
		this.m_guibar.AutoHide = false;
		this.m_guibar.SetFollowTransform(base.transform, Vector3.zero);
		this.m_guibar.gameObject.SetActive(false);
		this.m_chopsPerSlice = this.m_workable.GetChopTimeMultiplier(ClientUserSystem.m_Users.Count);
	}

	// Token: 0x060027E4 RID: 10212 RVA: 0x000BB090 File Offset: 0x000B9490
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.m_chopsPerSlice = this.m_workable.GetChopTimeMultiplier(ClientUserSystem.m_Users.Count);
		}
	}

	// Token: 0x060027E5 RID: 10213 RVA: 0x000BB0CC File Offset: 0x000B94CC
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		WorkableMessage workableMessage = (WorkableMessage)serialisable;
		this.m_onWorkstation = workableMessage.m_onWorkstation;
		this.m_progress = workableMessage.m_progress;
		this.m_subProgress = workableMessage.m_subProgress;
		this.UpdateProgressVisuals();
	}

	// Token: 0x060027E6 RID: 10214 RVA: 0x000BB10C File Offset: 0x000B950C
	private void Awake()
	{
		this.m_animator = base.gameObject.RequestComponentRecursive<Animator>();
		if (this.m_animator != null)
		{
			this.m_animator.enabled = false;
		}
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x060027E7 RID: 10215 RVA: 0x000BB160 File Offset: 0x000B9560
	protected override void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		if (this.m_guibar != null)
		{
			UnityEngine.Object.Destroy(this.m_guibar.gameObject);
		}
		base.OnDestroy();
	}

	// Token: 0x060027E8 RID: 10216 RVA: 0x000BB1AC File Offset: 0x000B95AC
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.m_subProgress > 0 || this.m_progress > 0)
		{
			this.m_guibar.gameObject.SetActive(true);
		}
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x000BB1DD File Offset: 0x000B95DD
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_guibar != null)
		{
			this.m_guibar.gameObject.SetActive(false);
		}
	}

	// Token: 0x060027EA RID: 10218 RVA: 0x000BB207 File Offset: 0x000B9607
	public void OnSurfacePlacement(ClientAttachStation _station)
	{
		if (_station.gameObject.RequestComponent<Workstation>() != null && this.m_animator != null)
		{
			this.m_animator.enabled = true;
		}
	}

	// Token: 0x060027EB RID: 10219 RVA: 0x000BB23C File Offset: 0x000B963C
	public void OnSurfaceDeplacement(ClientAttachStation _station)
	{
		if (this.GetProgress() <= 0f && this.m_animator != null)
		{
			this.m_animator.enabled = false;
		}
	}

	// Token: 0x060027EC RID: 10220 RVA: 0x000BB26C File Offset: 0x000B966C
	public override void UpdateSynchronising()
	{
		if (this.m_workable == null)
		{
			return;
		}
		if (this.m_onWorkstation)
		{
			this.SetVisState(IngredientMeshVisibility.VisState.Working);
		}
		else
		{
			if (this.m_guibar != null)
			{
				this.m_guibar.gameObject.SetActive(false);
			}
			if ((float)this.m_progress > 0.5f * (float)(this.m_workable.m_stages - 1))
			{
				this.SetVisState(IngredientMeshVisibility.VisState.Working);
			}
			else
			{
				this.SetVisState(IngredientMeshVisibility.VisState.Whole);
			}
		}
	}

	// Token: 0x060027ED RID: 10221 RVA: 0x000BB2F8 File Offset: 0x000B96F8
	protected void SetVisState(IngredientMeshVisibility.VisState _state)
	{
		if (this.m_prevVisState != null && this.m_prevVisState.Value == _state)
		{
			return;
		}
		if (this.m_ingredientMeshVisibility != null)
		{
			this.m_ingredientMeshVisibility.SetVisState(_state);
		}
		this.m_prevVisState = new IngredientMeshVisibility.VisState?(_state);
	}

	// Token: 0x060027EE RID: 10222 RVA: 0x000BB350 File Offset: 0x000B9750
	public float GetProgress()
	{
		return (float)this.m_progress / (float)Mathf.Max(this.m_workable.m_stages - 1, 1);
	}

	// Token: 0x060027EF RID: 10223 RVA: 0x000BB36E File Offset: 0x000B976E
	public bool HasFinished()
	{
		return this.m_progress == this.m_workable.m_stages - 1;
	}

	// Token: 0x060027F0 RID: 10224 RVA: 0x000BB388 File Offset: 0x000B9788
	public void DoWork(ClientAttachStation _station, GameObject _worker)
	{
		this.m_subProgress++;
		this.UpdateProgressVisuals();
		if (this.m_subProgress >= this.m_chopsPerSlice && !GameUtils.GetDebugConfig().m_infiniteChopping)
		{
			this.m_subProgress = 0;
			this.DoWork(_station, _worker, 1);
		}
	}

	// Token: 0x060027F1 RID: 10225 RVA: 0x000BB3DC File Offset: 0x000B97DC
	public void DoWork(ClientAttachStation _station, GameObject _worker, int _progress)
	{
		if (!this.HasFinished())
		{
			this.m_subProgress = 0;
			this.m_progress = Mathf.Min(this.m_progress + _progress, this.m_workable.m_stages - 1);
			if (!this.HasFinished())
			{
				this.UpdateProgressVisuals();
			}
		}
	}

	// Token: 0x060027F2 RID: 10226 RVA: 0x000BB42C File Offset: 0x000B982C
	private void UpdateProgressVisuals()
	{
		if (this.m_guibar != null)
		{
			this.m_guibar.SetProgress((float)(this.m_progress * this.m_chopsPerSlice + this.m_subProgress) / (float)(this.m_chopsPerSlice * this.m_workable.m_stages - 1));
			this.m_guibar.gameObject.SetActive(this.m_subProgress > 0 || this.m_progress > 0);
		}
		if (this.m_animator != null)
		{
			this.m_animator.SetInteger(this.m_workable.m_iAnimationVariable, this.m_progress);
		}
	}

	// Token: 0x04001F58 RID: 8024
	private WorkableItem m_workable;

	// Token: 0x04001F59 RID: 8025
	private ClientIngredientMeshVisibility m_ingredientMeshVisibility;

	// Token: 0x04001F5A RID: 8026
	private Animator m_animator;

	// Token: 0x04001F5B RID: 8027
	private ProgressUIController m_guibar;

	// Token: 0x04001F5C RID: 8028
	private bool m_onWorkstation;

	// Token: 0x04001F5D RID: 8029
	private int m_progress;

	// Token: 0x04001F5E RID: 8030
	private int m_subProgress;

	// Token: 0x04001F5F RID: 8031
	private int m_chopsPerSlice = 1;

	// Token: 0x04001F60 RID: 8032
	private IngredientMeshVisibility.VisState? m_prevVisState;
}
