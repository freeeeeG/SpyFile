using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000AC9 RID: 2761
public class InGameCustomisation : InGameMenuBehaviour
{
	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x0600379A RID: 14234 RVA: 0x0010602F File Offset: 0x0010442F
	public static InGameCustomisation Instance
	{
		get
		{
			return InGameCustomisation.s_instance;
		}
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x00106038 File Offset: 0x00104438
	protected override void Awake()
	{
		if (InGameCustomisation.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		InGameCustomisation.s_instance = this;
		base.Awake();
		if (this.m_mouseBlock != null)
		{
			this.m_mouseBlock.SetActive(false);
		}
	}

	// Token: 0x0600379C RID: 14236 RVA: 0x0010608A File Offset: 0x0010448A
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		this.m_customiser.SetChefs(this.m_chefCustomisation);
	}

	// Token: 0x0600379D RID: 14237 RVA: 0x001060A4 File Offset: 0x001044A4
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		this.m_selectButton = PlayerInputLookup.GetEngagedButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One, PadSide.Both);
		this.m_cancelButton = PlayerInputLookup.GetEngagedButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One, PadSide.Both);
		this.UpdateChefs();
		this.m_customiser.ActivateChefCustomisation(true);
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		if (this.m_selectButton != null)
		{
			this.m_selectButton.ClaimPressEvent();
			this.m_selectButton.ClaimReleaseEvent();
		}
		this.m_customiser.CacheCurrentAvatars();
		if (this.m_mouseBlock != null)
		{
			this.m_mouseBlock.SetActive(true);
		}
		this.OnActiveToggle(true);
		return true;
	}

	// Token: 0x0600379E RID: 14238 RVA: 0x00106164 File Offset: 0x00104564
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		this.m_customiser.ActivateChefCustomisation(false);
		this.m_customiser.PlaySelectedAnimations();
		if (this.m_mouseBlock != null)
		{
			this.m_mouseBlock.SetActive(false);
		}
		this.OnActiveToggle(false);
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x0600379F RID: 14239 RVA: 0x001061D4 File Offset: 0x001045D4
	private void UpdateChefs()
	{
		for (int i = 0; i < this.m_chefCustomisation.Length; i++)
		{
			FrontendChefCustomisation frontendChefCustomisation = this.m_chefCustomisation[i];
			UIPlayerMenuBehaviour uiplayerMenuBehaviour = (this.m_uiPlayerRoot.UIPlayers.Count <= i) ? null : this.m_uiPlayerRoot.UIPlayers[i];
			ChefMeshReplacer chefMeshReplacer = null;
			Animator chefAnimator = null;
			if (uiplayerMenuBehaviour != null)
			{
				chefMeshReplacer = uiplayerMenuBehaviour.gameObject.RequestComponent<ChefMeshReplacer>();
				if (chefMeshReplacer != null && chefMeshReplacer.ChefModel != null)
				{
					chefAnimator = chefMeshReplacer.ChefModel.RequireComponent<Animator>();
				}
			}
			frontendChefCustomisation.SetChefMeshReplacer(chefMeshReplacer);
			frontendChefCustomisation.SetChefAnimator(chefAnimator);
			frontendChefCustomisation.m_actualPlayer = (PlayerInputLookup.Player)i;
		}
	}

	// Token: 0x060037A0 RID: 14240 RVA: 0x0010628E File Offset: 0x0010468E
	private void OnUsersChanged()
	{
		this.UpdateChefs();
	}

	// Token: 0x060037A1 RID: 14241 RVA: 0x00106296 File Offset: 0x00104696
	private void OnActiveToggle(bool _active)
	{
		if (this.m_onActiveToggle != null)
		{
			this.m_onActiveToggle(_active);
		}
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x001062AF File Offset: 0x001046AF
	public void RegisterOnActiveToggle(GenericVoid<bool> _callback)
	{
		this.m_onActiveToggle = (GenericVoid<bool>)Delegate.Combine(this.m_onActiveToggle, _callback);
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x001062C8 File Offset: 0x001046C8
	public void UnregisterOnActiveToggle(GenericVoid<bool> _callback)
	{
		this.m_onActiveToggle = (GenericVoid<bool>)Delegate.Remove(this.m_onActiveToggle, _callback);
	}

	// Token: 0x060037A4 RID: 14244 RVA: 0x001062E4 File Offset: 0x001046E4
	protected override void Update()
	{
		base.Update();
		if (this.m_selectButton.JustPressed())
		{
			this.m_selectButton.ClaimPressEvent();
			this.m_selectButton.ClaimReleaseEvent();
			this.Hide(true, false);
		}
		if (this.m_cancelButton.JustPressed())
		{
			this.m_cancelButton.ClaimPressEvent();
			this.m_cancelButton.ClaimReleaseEvent();
		}
	}

	// Token: 0x060037A5 RID: 14245 RVA: 0x0010634C File Offset: 0x0010474C
	public override void Close()
	{
		this.m_customiser.RevertAvatars();
		base.Close();
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x0010635F File Offset: 0x0010475F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		InGameCustomisation.s_instance = null;
	}

	// Token: 0x04002C93 RID: 11411
	[SerializeField]
	private UIPlayerRootMenu m_uiPlayerRoot;

	// Token: 0x04002C94 RID: 11412
	[SerializeField]
	private FrontendChefCustomisation[] m_chefCustomisation;

	// Token: 0x04002C95 RID: 11413
	[SerializeField]
	private GameObject m_mouseBlock;

	// Token: 0x04002C96 RID: 11414
	private ChefCustomiser m_customiser = new ChefCustomiser();

	// Token: 0x04002C97 RID: 11415
	private GenericVoid<bool> m_onActiveToggle;

	// Token: 0x04002C98 RID: 11416
	private static InGameCustomisation s_instance;

	// Token: 0x04002C99 RID: 11417
	private ILogicalButton m_selectButton;

	// Token: 0x04002C9A RID: 11418
	private ILogicalButton m_cancelButton;
}
