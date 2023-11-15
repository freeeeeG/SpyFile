using System;
using UnityEngine;

// Token: 0x02000A12 RID: 2578
public class PlayerControlsImpl_Default : MonoBehaviour, IPlayerControlsImpl
{
	// Token: 0x060032C3 RID: 12995 RVA: 0x000EDAD4 File Offset: 0x000EBED4
	public void RegisterForInteractTrigger(VoidGeneric<ClientInteractable> _callback)
	{
		this.m_clientImpl.RegisterForInteractTrigger(_callback);
	}

	// Token: 0x060032C4 RID: 12996 RVA: 0x000EDAE2 File Offset: 0x000EBEE2
	public void UnregisterForInteractTrigger(VoidGeneric<ClientInteractable> _callback)
	{
		this.m_clientImpl.UnregisterForInteractTrigger(_callback);
	}

	// Token: 0x060032C5 RID: 12997 RVA: 0x000EDAF0 File Offset: 0x000EBEF0
	public void RegisterForThrowTrigger(VoidGeneric<GameObject> _callback)
	{
		this.m_clientImpl.RegisterForThrowTrigger(_callback);
	}

	// Token: 0x060032C6 RID: 12998 RVA: 0x000EDAFE File Offset: 0x000EBEFE
	public void UnregisterForThrowTrigger(VoidGeneric<GameObject> _callback)
	{
		this.m_clientImpl.UnregisterForThrowTrigger(_callback);
	}

	// Token: 0x060032C7 RID: 12999 RVA: 0x000EDB0C File Offset: 0x000EBF0C
	public void RegisterForFallingTrigger(VoidGeneric<bool> _callback)
	{
		this.m_clientImpl.RegisterForFallingTrigger(_callback);
	}

	// Token: 0x060032C8 RID: 13000 RVA: 0x000EDB1A File Offset: 0x000EBF1A
	public void UnregisterForFallingTrigger(VoidGeneric<bool> _callback)
	{
		this.m_clientImpl.UnregisterForFallingTrigger(_callback);
	}

	// Token: 0x060032C9 RID: 13001 RVA: 0x000EDB28 File Offset: 0x000EBF28
	public void NotifySessionInteractionStarted(ClientSessionInteractable _interaction)
	{
		this.m_clientImpl.NotifySessionInteractionStarted(_interaction);
	}

	// Token: 0x060032CA RID: 13002 RVA: 0x000EDB36 File Offset: 0x000EBF36
	public void NotifySessionInteractionEnded(ClientSessionInteractable _interaction)
	{
		this.m_clientImpl.NotifySessionInteractionEnded(_interaction);
	}

	// Token: 0x060032CB RID: 13003 RVA: 0x000EDB44 File Offset: 0x000EBF44
	public void OnCollisionEnter(Collision _collision)
	{
		if (this.m_clientImpl != null)
		{
			this.m_clientImpl.OnCollisionEnter(_collision);
		}
	}

	// Token: 0x060032CC RID: 13004 RVA: 0x000EDB63 File Offset: 0x000EBF63
	public ClientInteractable GetCurrentlyInteracting()
	{
		if (this.m_clientImpl != null)
		{
			return this.m_clientImpl.GetCurrentlyInteracting();
		}
		return null;
	}

	// Token: 0x060032CD RID: 13005 RVA: 0x000EDB83 File Offset: 0x000EBF83
	public void Enable()
	{
		if (this.m_clientImpl != null)
		{
			this.m_clientImpl.Enable();
		}
		if (this.m_serverImpl != null)
		{
			this.m_serverImpl.Enable();
		}
	}

	// Token: 0x060032CE RID: 13006 RVA: 0x000EDBBD File Offset: 0x000EBFBD
	public void Disable()
	{
		if (this.m_clientImpl != null)
		{
			this.m_clientImpl.Disable();
		}
		if (this.m_serverImpl != null)
		{
			this.m_serverImpl.Disable();
		}
	}

	// Token: 0x060032CF RID: 13007 RVA: 0x000EDBF7 File Offset: 0x000EBFF7
	public void Update_Impl()
	{
		if (this.m_clientImpl != null)
		{
			this.m_clientImpl.Update_Impl();
		}
		if (this.m_serverImpl != null)
		{
			this.m_serverImpl.Update_Impl();
		}
	}

	// Token: 0x060032D0 RID: 13008 RVA: 0x000EDC31 File Offset: 0x000EC031
	public void Init(PlayerControls _controls)
	{
		if (this.m_clientImpl != null)
		{
			this.m_clientImpl.Init(_controls);
		}
		if (this.m_serverImpl != null)
		{
			this.m_serverImpl.Init(_controls);
		}
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x000EDC6D File Offset: 0x000EC06D
	public void SetPlayerControlSchemeData(PlayerControls.ControlSchemeData _controlScheme)
	{
		if (this.m_clientImpl != null)
		{
			this.m_clientImpl.SetPlayerControlSchemeData(_controlScheme);
		}
		if (this.m_serverImpl != null)
		{
			this.m_serverImpl.SetPlayerControlSchemeData(_controlScheme);
		}
	}

	// Token: 0x040028E5 RID: 10469
	public ServerPlayerControlsImpl_Default m_serverImpl;

	// Token: 0x040028E6 RID: 10470
	public ClientPlayerControlsImpl_Default m_clientImpl;
}
