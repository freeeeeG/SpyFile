using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000524 RID: 1316
public class ClientMixingStation : ClientSynchroniserBase
{
	// Token: 0x0600189A RID: 6298 RVA: 0x0007CEEB File Offset: 0x0007B2EB
	public override EntityType GetEntityType()
	{
		return EntityType.MixingStation;
	}

	// Token: 0x0600189B RID: 6299 RVA: 0x0007CEEF File Offset: 0x0007B2EF
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		this.ApplyStateSyncMessage(serialisable);
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x0007CEF8 File Offset: 0x0007B2F8
	protected virtual void ApplyStateSyncMessage(Serialisable serialisable)
	{
		MixingStationMessage mixingStationMessage = (MixingStationMessage)serialisable;
		this.m_isTurnedOn = mixingStationMessage.m_isTurnedOn;
		this.UpdateAnimations();
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x0007CF20 File Offset: 0x0007B320
	private void Awake()
	{
		this.m_MixingStation = base.GetComponent<MixingStation>();
		this.m_AttachStation = base.GetComponent<ClientAttachStation>();
		this.m_AttachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
		this.m_AttachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_AttachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_Animator = base.GetComponentInChildren<Animator>();
		if (ClientMixingStation.s_AnimatorMixingHash == 0)
		{
			ClientMixingStation.s_AnimatorMixingHash = Animator.StringToHash("On");
		}
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x0007CFB0 File Offset: 0x0007B3B0
	protected override void OnDestroy()
	{
		this.m_AttachStation.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
		this.m_AttachStation.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_AttachStation.UnregisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		base.OnDestroy();
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x0007D008 File Offset: 0x0007B408
	private bool CanAddItem(GameObject _object, PlacementContext _context)
	{
		PlacementContext.Source source = _context.m_source;
		if (source == PlacementContext.Source.Game)
		{
			return true;
		}
		if (source != PlacementContext.Source.Player)
		{
			return false;
		}
		if (!this.m_MixingStation.m_bAttachRestrictions)
		{
			return true;
		}
		if (_object.RequestInterface<IClientMixable>() == null)
		{
			return false;
		}
		IClientCookable clientCookable = _object.RequestInterface<IClientCookable>();
		if (clientCookable != null)
		{
			IIngredientContents ingredientContents = _object.RequestInterface<IIngredientContents>();
			if (ingredientContents != null && ingredientContents.HasContents() && (clientCookable.GetCookedOrderState() != CookedCompositeOrderNode.CookingProgress.Raw || clientCookable.AccessCookingTime > 0f))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x0007D09C File Offset: 0x0007B49C
	private void OnItemAdded(IClientAttachment _iHoldable)
	{
		this.m_mixable = _iHoldable.AccessGameObject().RequestInterface<IClientMixable>();
		this.UpdateAnimations();
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x0007D0B5 File Offset: 0x0007B4B5
	private void OnItemRemoved(IClientAttachment _iHoldable)
	{
		this.m_isTurnedOn = false;
		this.m_mixable = null;
		this.UpdateAnimations();
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x0007D0CC File Offset: 0x0007B4CC
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_isTurnedOn)
		{
			if (this.m_activeLoopingAudio == null && this.m_mixable != null)
			{
				this.m_activeLoopingAudio = new GameLoopingAudioTag?(this.m_mixable.GetMixingSoundTag());
				GameUtils.StartAudio(this.m_activeLoopingAudio.Value, this, base.gameObject.layer);
			}
		}
		else if (this.m_activeLoopingAudio != null)
		{
			GameUtils.StopAudio(this.m_activeLoopingAudio.Value, this);
			this.m_activeLoopingAudio = null;
		}
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x0007D170 File Offset: 0x0007B570
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_activeLoopingAudio != null)
		{
			GameUtils.StopAudio(this.m_activeLoopingAudio.Value, this);
			this.m_activeLoopingAudio = null;
		}
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x0007D1B3 File Offset: 0x0007B5B3
	private void UpdateAnimations()
	{
		if (this.m_Animator != null)
		{
			this.m_Animator.SetBool(ClientMixingStation.s_AnimatorMixingHash, this.m_isTurnedOn);
		}
	}

	// Token: 0x040013C2 RID: 5058
	private MixingStation m_MixingStation;

	// Token: 0x040013C3 RID: 5059
	private ClientAttachStation m_AttachStation;

	// Token: 0x040013C4 RID: 5060
	private IClientMixable m_mixable;

	// Token: 0x040013C5 RID: 5061
	private bool m_isTurnedOn;

	// Token: 0x040013C6 RID: 5062
	private Animator m_Animator;

	// Token: 0x040013C7 RID: 5063
	private const string c_AnimatorMixingVar = "On";

	// Token: 0x040013C8 RID: 5064
	private static int s_AnimatorMixingHash;

	// Token: 0x040013C9 RID: 5065
	private GameLoopingAudioTag? m_activeLoopingAudio;
}
