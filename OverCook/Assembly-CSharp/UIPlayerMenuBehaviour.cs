using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ADF RID: 2783
[RequireComponent(typeof(T17Button))]
public class UIPlayerMenuBehaviour : BaseMenuBehaviour
{
	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06003850 RID: 14416 RVA: 0x0010908F File Offset: 0x0010748F
	public Selectable MenuButton
	{
		get
		{
			return this.m_nameButton;
		}
	}

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06003851 RID: 14417 RVA: 0x00109097 File Offset: 0x00107497
	public Transform DialogAnchor
	{
		get
		{
			return this.m_dialogAnchor;
		}
	}

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06003852 RID: 14418 RVA: 0x0010909F File Offset: 0x0010749F
	public Transform EmoteWheelAnchor
	{
		get
		{
			return this.m_emoteWheelAnchor;
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06003853 RID: 14419 RVA: 0x001090A7 File Offset: 0x001074A7
	public string LastSetDisplayName
	{
		get
		{
			return this.m_lastSetDisplayName;
		}
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06003854 RID: 14420 RVA: 0x001090AF File Offset: 0x001074AF
	// (set) Token: 0x06003855 RID: 14421 RVA: 0x001090B8 File Offset: 0x001074B8
	public User UserInfo
	{
		get
		{
			return this.m_User;
		}
		set
		{
			this.m_User = value;
			if (this.m_User != null)
			{
				this.m_nameOptions.m_displayName.text = this.m_User.DisplayName;
				this.m_nameOptions.m_displayName.gameObject.SetActive(true);
				this.m_nameOptions.m_searching.gameObject.SetActive(false);
				this.m_loadingIcon.SetActive(false);
				this.m_nameOptions.m_empty.gameObject.SetActive(false);
				this.m_playerNumber.enabled = true;
				int num = ClientUserSystem.m_Users.FindIndex((User user) => user == this.m_User);
				this.m_voiceChatUI.PlayerSlot = (EngagementSlot)((num == -1) ? 4 : num);
				this.m_lastSetDisplayName = this.m_User.DisplayName;
			}
			else
			{
				this.m_nameOptions.m_displayName.gameObject.SetActive(false);
				bool flag = this.m_rootMenu.IsMatchmaking();
				this.m_nameOptions.m_searching.gameObject.SetActive(flag);
				this.m_loadingIcon.SetActive(flag);
				this.m_nameOptions.m_empty.gameObject.SetActive(!flag);
				this.HideMenu(true);
				this.m_playerNumber.enabled = false;
				this.m_lastSetDisplayName = null;
			}
			this.m_nameButton.interactable = this.CanBeInteractedWith();
			this.UpdateColour();
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06003856 RID: 14422 RVA: 0x00109220 File Offset: 0x00107620
	// (set) Token: 0x06003857 RID: 14423 RVA: 0x0010922D File Offset: 0x0010762D
	public string PlayerNumber
	{
		get
		{
			return this.m_playerNumber.text;
		}
		set
		{
			this.m_playerNumber.text = value;
		}
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x0010923B File Offset: 0x0010763B
	protected override void Awake()
	{
		base.Awake();
		this.HideMenu(true);
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x0010924C File Offset: 0x0010764C
	protected override void Start()
	{
		base.Start();
		if (this.m_nameButton == null)
		{
			this.m_nameButton = base.GetComponent<T17Button>();
		}
		this.m_IPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		T17Button nameButton = this.m_nameButton;
		nameButton.OnButtonSelect = (T17Button.T17ButtonDelegate)Delegate.Combine(nameButton.OnButtonSelect, new T17Button.T17ButtonDelegate(this.OnSelected));
		T17Button nameButton2 = this.m_nameButton;
		nameButton2.OnButtonDeselect = (T17Button.T17ButtonDelegate)Delegate.Combine(nameButton2.OnButtonDeselect, new T17Button.T17ButtonDelegate(this.OnDeselected));
		this.m_nameButton.onClick.AddListener(delegate()
		{
			this.OnClick(UIPlayerMenuBehaviour.UIPlayerMenuOptions.Name);
		});
		for (int i = 0; i < this.m_menuOptions.Count; i++)
		{
			UIPlayerMenuBehaviour.MenuOption option = this.m_menuOptions[i];
			T17Button button = option.m_button;
			button.OnButtonSelect = (T17Button.T17ButtonDelegate)Delegate.Combine(button.OnButtonSelect, new T17Button.T17ButtonDelegate(this.OnSelected));
			T17Button button2 = option.m_button;
			button2.OnButtonDeselect = (T17Button.T17ButtonDelegate)Delegate.Combine(button2.OnButtonDeselect, new T17Button.T17ButtonDelegate(this.OnDeselected));
			option.m_button.onClick.AddListener(delegate()
			{
				this.OnClick(option.m_type);
			});
		}
		this.SetupChefModel(true);
		base.gameObject.layer = LayerMask.NameToLayer(this.m_rootMenu.m_uiChefLayer);
		this.m_background.transform.SetParent(this.m_rootMenu.m_backgroundsContainer);
		InGameCustomisation instance = InGameCustomisation.Instance;
		if (instance != null)
		{
			this.m_nameButton.interactable = this.CanBeInteractedWith();
			instance.RegisterOnActiveToggle(new GenericVoid<bool>(this.OnCustomisationToggle));
		}
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x0010941C File Offset: 0x0010781C
	protected override void Update()
	{
		base.Update();
		if ((this.m_nameButton.navigation.selectOnUp != null && !this.m_nameButton.navigation.selectOnUp.IsInteractable()) || (this.m_nameButton.navigation.selectOnDown != null && !this.m_nameButton.navigation.selectOnDown.IsInteractable()))
		{
			this.UpdateNavigation();
		}
		if (this.m_hideMenu == null && this.IsSelected())
		{
			this.m_hideMenu = base.StartCoroutine(this.HideMenuRoutine(false));
		}
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x001094D4 File Offset: 0x001078D4
	protected void SetupChefModel(bool _force = false)
	{
		if (this.m_User == null)
		{
			if (this.m_chef != null && this.m_chef.ChefModel != null)
			{
				this.m_chef.ChefModel.SetActive(false);
			}
			return;
		}
		if (this.m_chef != null && this.m_chef.ChefModel != null)
		{
			this.m_chef.ChefModel.SetActive(true);
		}
		base.gameObject.layer = LayerMask.NameToLayer(this.m_rootMenu.m_uiChefLayer);
		this.m_chef = base.gameObject.RequireComponent<FrontendChef>();
		GameObject chefModel = this.m_chef.ChefModel;
		this.m_chef.SetChefData(this.m_User.SelectedChefData, _force);
		this.m_chef.SetChefHat(HatMeshVisibility.VisState.Fancy);
		this.m_chef.SetShaderMode(FrontendChef.ShaderMode.eUI);
		this.m_chef.SetUIChefAmbientLighting(this.m_AmbientColor);
		if (this.m_chef.ChefModel != chefModel)
		{
			int num = ClientUserSystem.m_Users.FindIndex((User x) => x == this.m_User);
			if (num != -1)
			{
				this.m_chef.SetAnimationSet((FrontendChef.AnimationSet)num);
			}
		}
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x00109618 File Offset: 0x00107A18
	private ChefColourData GetTeamColour()
	{
		ChefColourData result = this.m_noChef;
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		if (metaGameProgress == null)
		{
			return result;
		}
		AvatarDirectoryData avatarDirectory = metaGameProgress.AvatarDirectory;
		if (avatarDirectory == null)
		{
			return result;
		}
		if (this.m_User == null)
		{
			return result;
		}
		bool flag = true;
		if (ClientGameSetup.Mode == GameMode.Versus)
		{
			flag = false;
		}
		else
		{
			GameSession gameSession = GameUtils.GetGameSession();
			if (gameSession != null && gameSession.TypeSettings.Type == GameSession.GameType.Competitive)
			{
				flag = false;
			}
		}
		TeamID team = this.m_User.Team;
		if (team != TeamID.None)
		{
			if (team != TeamID.One)
			{
				if (team == TeamID.Two)
				{
					result = this.m_teamTwo;
				}
			}
			else
			{
				result = this.m_teamOne;
			}
		}
		else if (flag)
		{
			int num = ClientUserSystem.m_Users.FindIndex((User x) => x == this.m_User);
			if (num != -1)
			{
				result = metaGameProgress.AvatarDirectory.Colours[num];
			}
		}
		else
		{
			result = this.m_teamNone;
		}
		return result;
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x00109728 File Offset: 0x00107B28
	public void UpdateBackground()
	{
		ChefColourData teamColour = this.GetTeamColour();
		this.m_background.sprite = teamColour.Background;
		this.m_speakerImage.color = teamColour.DarkUIColour;
		this.m_muteImage.color = teamColour.DarkUIColour;
		this.m_playerNumber.color = teamColour.DarkUIColour;
	}

	// Token: 0x0600385E RID: 14430 RVA: 0x00109780 File Offset: 0x00107B80
	public void UpdateColour()
	{
		if (this.m_User != null)
		{
			this.UpdateBackground();
			this.SetupChefModel(false);
		}
		else
		{
			if (this.m_chef != null && this.m_chef.ChefModel != null)
			{
				this.m_chef.ChefModel.SetActive(false);
			}
			this.UpdateBackground();
		}
	}

	// Token: 0x0600385F RID: 14431 RVA: 0x001097E8 File Offset: 0x00107BE8
	public bool HasMenuOptions()
	{
		if (this.m_User != null)
		{
			for (int i = 0; i < this.m_menuOptions.Count; i++)
			{
				UIPlayerMenuBehaviour.MenuOption option = this.m_menuOptions[i];
				if (this.IsOptionActiveOnCurrentPlatform(option) && (!this.m_User.IsLocal || !option.m_disableIfLocal))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003860 RID: 14432 RVA: 0x00109854 File Offset: 0x00107C54
	private bool IsOptionActiveOnCurrentPlatform(UIPlayerMenuBehaviour.MenuOption _option)
	{
		return PlatformUtils.HasPlatformFlag(_option.m_platformsActiveOn);
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x00109864 File Offset: 0x00107C64
	protected void OnClick(UIPlayerMenuBehaviour.UIPlayerMenuOptions _option)
	{
		if (this.HasMenuOptions())
		{
			switch (_option)
			{
			case UIPlayerMenuBehaviour.UIPlayerMenuOptions.Name:
				this.ToggleMenu();
				break;
			case UIPlayerMenuBehaviour.UIPlayerMenuOptions.Profile:
				if (this.m_User.IsLocal)
				{
					this.m_IPlayerManager.ShowGamerCard(this.m_User.GamepadUser);
				}
				else
				{
					this.m_IPlayerManager.ShowGamerCard(this.m_User.PlatformID);
				}
				break;
			case UIPlayerMenuBehaviour.UIPlayerMenuOptions.Mute:
				this.SetMuted(true);
				break;
			case UIPlayerMenuBehaviour.UIPlayerMenuOptions.Unmute:
				this.SetMuted(false);
				break;
			case UIPlayerMenuBehaviour.UIPlayerMenuOptions.Kick:
				this.KickUser();
				break;
			}
		}
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x00109910 File Offset: 0x00107D10
	protected void SetMuted(bool toMuted)
	{
		this.m_User.SessionId.IsLocallyMuted = toMuted;
		this.UpdateMenuStructure();
		for (int i = 0; i < this.m_menuOptions.Count; i++)
		{
			if (this.m_menuOptions[i].m_type == UIPlayerMenuBehaviour.UIPlayerMenuOptions.Mute && !toMuted)
			{
				this.m_rootMenu.CachedEventSystem.SetSelectedGameObject(this.m_menuOptions[i].m_button.gameObject);
			}
			else if (this.m_menuOptions[i].m_type == UIPlayerMenuBehaviour.UIPlayerMenuOptions.Unmute && toMuted)
			{
				this.m_rootMenu.CachedEventSystem.SetSelectedGameObject(this.m_menuOptions[i].m_button.gameObject);
			}
		}
	}

	// Token: 0x06003863 RID: 14435 RVA: 0x001099E8 File Offset: 0x00107DE8
	protected virtual void OnSelected(T17Button _button)
	{
		if ((_button == this.m_nameButton || _button.gameObject.IsInHierarchyOf(this.m_menu)) && this.m_hideMenu != null)
		{
			base.StopCoroutine(this.m_hideMenu);
		}
		this.UpdateNavigation();
	}

	// Token: 0x06003864 RID: 14436 RVA: 0x00109A39 File Offset: 0x00107E39
	protected virtual void OnDeselected(T17Button _button)
	{
		this.HideMenu(false);
		this.UpdateNavigation();
	}

	// Token: 0x06003865 RID: 14437 RVA: 0x00109A48 File Offset: 0x00107E48
	private IEnumerator HideMenuRoutine(bool _force = false)
	{
		if (!_force)
		{
			yield return new WaitForEndOfFrame();
			while (this.IsSelected())
			{
				if (Input.mousePresent && Input.GetMouseButtonDown(0))
				{
					break;
				}
				yield return null;
			}
			if (T17DialogBoxManager.HasAnyOpenDialogs())
			{
				this.m_hideMenu = null;
				yield break;
			}
		}
		this.m_menu.SetActive(false);
		this.UpdateNavigation();
		yield break;
	}

	// Token: 0x06003866 RID: 14438 RVA: 0x00109A6C File Offset: 0x00107E6C
	protected void UpdateNavigation()
	{
		this.UpdateChildNavigation();
		Navigation navigation = this.m_nameButton.navigation;
		Selectable firstSelectableChild = this.GetFirstSelectableChild();
		navigation.selectOnUp = firstSelectableChild;
		if (navigation.selectOnUp == null)
		{
			navigation.selectOnUp = this.m_nameButton.FindSelectable(this.m_nameButton.transform.up);
		}
		if (navigation.selectOnDown == null)
		{
			navigation.selectOnDown = this.m_nameButton.FindSelectable(-this.m_nameButton.transform.up);
		}
		this.m_nameButton.navigation = navigation;
	}

	// Token: 0x06003867 RID: 14439 RVA: 0x00109B14 File Offset: 0x00107F14
	protected Selectable GetFirstSelectableChild()
	{
		Selectable[] array = this.m_menu.RequestComponentsInImmediateChildren<Selectable>();
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (array[i].isActiveAndEnabled)
			{
				return array[i];
			}
		}
		return null;
	}

	// Token: 0x06003868 RID: 14440 RVA: 0x00109B58 File Offset: 0x00107F58
	protected Selectable GetAdjacentSelectable(Selectable selectable, UIPlayerMenuBehaviour.SelectableDirection direction)
	{
		Selectable[] array = this.m_menu.RequestComponentsInImmediateChildren<Selectable>();
		int num = array.FindIndex_Predicate((Selectable x) => x == selectable);
		if (num == -1)
		{
			return null;
		}
		int num2 = (direction != UIPlayerMenuBehaviour.SelectableDirection.Up) ? 1 : -1;
		while (num + num2 >= 0 && num + num2 < array.Length)
		{
			if (array[num + num2].isActiveAndEnabled)
			{
				return array[num + num2];
			}
			num2 += ((direction != UIPlayerMenuBehaviour.SelectableDirection.Up) ? 1 : -1);
		}
		if (direction == UIPlayerMenuBehaviour.SelectableDirection.Up)
		{
			return selectable.FindSelectable(this.m_nameButton.transform.up);
		}
		return selectable.FindSelectable(-this.m_nameButton.transform.up);
	}

	// Token: 0x06003869 RID: 14441 RVA: 0x00109C28 File Offset: 0x00108028
	protected void UpdateChildNavigation()
	{
		for (int i = 0; i < this.m_menuOptions.Count; i++)
		{
			if (this.m_menuOptions[i].m_button.isActiveAndEnabled)
			{
				Navigation navigation = this.m_menuOptions[i].m_button.navigation;
				navigation.mode = Navigation.Mode.Explicit;
				navigation.selectOnUp = this.GetAdjacentSelectable(this.m_menuOptions[i].m_button, UIPlayerMenuBehaviour.SelectableDirection.Up);
				Selectable selectable = this.GetAdjacentSelectable(this.m_menuOptions[i].m_button, UIPlayerMenuBehaviour.SelectableDirection.Down);
				if (selectable == null)
				{
					selectable = this.m_nameButton;
				}
				navigation.selectOnDown = selectable;
				navigation.selectOnLeft = this.m_nameButton.navigation.selectOnLeft;
				navigation.selectOnRight = this.m_nameButton.navigation.selectOnRight;
				this.m_menuOptions[i].m_button.navigation = navigation;
			}
		}
	}

	// Token: 0x0600386A RID: 14442 RVA: 0x00109D48 File Offset: 0x00108148
	protected bool IsSelected()
	{
		if (this.m_rootMenu.CachedEventSystem == null)
		{
			return false;
		}
		GameObject currentSelectedGameObject = this.m_rootMenu.CachedEventSystem.currentSelectedGameObject;
		return currentSelectedGameObject == this || currentSelectedGameObject == null;
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x00109D94 File Offset: 0x00108194
	public void ShowMenu(UIPlayerMenuBehaviour.UIPlayerMenuOptions? _optionToSelect = null)
	{
		this.m_menu.SetActive(true);
		this.UpdateMenuStructure();
		if (_optionToSelect != null && this.m_rootMenu.CachedEventSystem != null)
		{
			for (int i = 0; i < this.m_menuOptions.Count; i++)
			{
				UIPlayerMenuBehaviour.MenuOption menuOption = this.m_menuOptions[i];
				if (menuOption.m_type == _optionToSelect)
				{
					this.m_rootMenu.CachedEventSystem.SetSelectedGameObject(menuOption.m_button.gameObject);
					break;
				}
			}
		}
	}

	// Token: 0x0600386C RID: 14444 RVA: 0x00109E40 File Offset: 0x00108240
	public UIPlayerMenuBehaviour.UIPlayerMenuOptions? GetSelectedMenuOption()
	{
		if (this.m_rootMenu.CachedEventSystem == null)
		{
			return null;
		}
		GameObject gameObject = this.m_rootMenu.CachedEventSystem.GetPendingSelectedGameObject();
		if (gameObject == null)
		{
			gameObject = this.m_rootMenu.CachedEventSystem.currentSelectedGameObject;
		}
		if (gameObject == null)
		{
			return null;
		}
		for (int i = 0; i < this.m_menuOptions.Count; i++)
		{
			UIPlayerMenuBehaviour.MenuOption menuOption = this.m_menuOptions[i];
			if (menuOption.m_button.gameObject == gameObject)
			{
				return new UIPlayerMenuBehaviour.UIPlayerMenuOptions?(menuOption.m_type);
			}
		}
		return null;
	}

	// Token: 0x0600386D RID: 14445 RVA: 0x00109F07 File Offset: 0x00108307
	protected void KickUser()
	{
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x00109F0C File Offset: 0x0010830C
	protected void UpdateMenuStructure()
	{
		for (int i = 0; i < this.m_menuOptions.Count; i++)
		{
			UIPlayerMenuBehaviour.UIPlayerMenuOptions type = this.m_menuOptions[i].m_type;
			bool flag = this.IsOptionActiveOnCurrentPlatform(this.m_menuOptions[i]);
			if (flag)
			{
				if (this.m_User == null)
				{
					flag = false;
				}
				else if (this.m_User.IsLocal && this.m_menuOptions[i].m_disableIfLocal)
				{
					flag = false;
				}
				else if (type == UIPlayerMenuBehaviour.UIPlayerMenuOptions.Mute || type == UIPlayerMenuBehaviour.UIPlayerMenuOptions.Unmute)
				{
					if (this.m_User.SessionId == null || (type == UIPlayerMenuBehaviour.UIPlayerMenuOptions.Mute && this.m_User.SessionId.IsLocallyMuted) || (type == UIPlayerMenuBehaviour.UIPlayerMenuOptions.Unmute && !this.m_User.SessionId.IsLocallyMuted))
					{
						flag = false;
					}
				}
				else if (type == UIPlayerMenuBehaviour.UIPlayerMenuOptions.Kick && !this.m_canKick && !this.m_User.IsLocal)
				{
					flag = false;
				}
			}
			this.m_menuOptions[i].m_button.gameObject.SetActive(flag);
		}
		this.UpdateNavigation();
	}

	// Token: 0x0600386F RID: 14447 RVA: 0x0010A04D File Offset: 0x0010844D
	public void HideMenu(bool _force = false)
	{
		this.m_hideMenu = base.StartCoroutine(this.HideMenuRoutine(_force));
	}

	// Token: 0x06003870 RID: 14448 RVA: 0x0010A064 File Offset: 0x00108464
	public void ToggleMenu()
	{
		if (this.m_menu.activeSelf)
		{
			this.HideMenu(false);
		}
		else
		{
			this.ShowMenu(null);
		}
	}

	// Token: 0x06003871 RID: 14449 RVA: 0x0010A09C File Offset: 0x0010849C
	protected void OnCustomisationToggle(bool _active)
	{
		if (this.m_nameButton != null)
		{
			this.m_nameButton.interactable = this.CanBeInteractedWith();
		}
	}

	// Token: 0x06003872 RID: 14450 RVA: 0x0010A0C0 File Offset: 0x001084C0
	public bool CanBeInteractedWith()
	{
		if (this.m_User == null)
		{
			return false;
		}
		InGameCustomisation instance = InGameCustomisation.Instance;
		return (!(instance != null) || !instance.isActiveAndEnabled) && this.HasMenuOptions();
	}

	// Token: 0x06003873 RID: 14451 RVA: 0x0010A108 File Offset: 0x00108508
	protected override void OnDestroy()
	{
		base.OnDestroy();
		UnityEngine.Object.Destroy(this.m_background.gameObject);
		InGameCustomisation instance = InGameCustomisation.Instance;
		if (instance != null)
		{
			instance.UnregisterOnActiveToggle(new GenericVoid<bool>(this.OnCustomisationToggle));
		}
	}

	// Token: 0x04002D04 RID: 11524
	[SerializeField]
	private List<UIPlayerMenuBehaviour.MenuOption> m_menuOptions;

	// Token: 0x04002D05 RID: 11525
	[SerializeField]
	private UIPlayerMenuBehaviour.NameStrings m_nameOptions;

	// Token: 0x04002D06 RID: 11526
	[SerializeField]
	[AssignChildRecursive("TalkingIcon", Editorbility.Editable)]
	private T17Image m_speakerImage;

	// Token: 0x04002D07 RID: 11527
	[SerializeField]
	[AssignChildRecursive("MuteIcon", Editorbility.Editable)]
	private T17Image m_muteImage;

	// Token: 0x04002D08 RID: 11528
	[SerializeField]
	[AssignChildRecursive("LoadingIconParent", Editorbility.Editable)]
	private GameObject m_loadingIcon;

	// Token: 0x04002D09 RID: 11529
	[SerializeField]
	[AssignChildRecursive("Background", Editorbility.Editable)]
	private T17Image m_background;

	// Token: 0x04002D0A RID: 11530
	[SerializeField]
	[AssignChildRecursive("Menu", Editorbility.Editable)]
	private GameObject m_menu;

	// Token: 0x04002D0B RID: 11531
	public bool m_canKick;

	// Token: 0x04002D0C RID: 11532
	[SerializeField]
	[AssignChild("NameBacker", Editorbility.Editable)]
	private T17Button m_nameButton;

	// Token: 0x04002D0D RID: 11533
	[HideInInspector]
	public UIPlayerRootMenu m_rootMenu;

	// Token: 0x04002D0E RID: 11534
	[SerializeField]
	private T17Text m_playerNumber;

	// Token: 0x04002D0F RID: 11535
	[SerializeField]
	private Transform m_dialogAnchor;

	// Token: 0x04002D10 RID: 11536
	[SerializeField]
	private Transform m_emoteWheelAnchor;

	// Token: 0x04002D11 RID: 11537
	[SerializeField]
	private ChefColourData m_teamOne;

	// Token: 0x04002D12 RID: 11538
	[SerializeField]
	private ChefColourData m_teamTwo;

	// Token: 0x04002D13 RID: 11539
	[SerializeField]
	private ChefColourData m_teamNone;

	// Token: 0x04002D14 RID: 11540
	[SerializeField]
	private ChefColourData m_noChef;

	// Token: 0x04002D15 RID: 11541
	[SerializeField]
	private Color m_AmbientColor;

	// Token: 0x04002D16 RID: 11542
	private FrontendChef m_chef;

	// Token: 0x04002D17 RID: 11543
	private User m_User;

	// Token: 0x04002D18 RID: 11544
	private string m_lastSetDisplayName;

	// Token: 0x04002D19 RID: 11545
	[SerializeField]
	private VoiceChatUserUI m_voiceChatUI;

	// Token: 0x04002D1A RID: 11546
	private IPlayerManager m_IPlayerManager;

	// Token: 0x04002D1B RID: 11547
	private Coroutine m_hideMenu;

	// Token: 0x02000AE0 RID: 2784
	[Serializable]
	public struct MenuOption
	{
		// Token: 0x04002D1C RID: 11548
		[SerializeField]
		public T17Button m_button;

		// Token: 0x04002D1D RID: 11549
		[SerializeField]
		public UIPlayerMenuBehaviour.UIPlayerMenuOptions m_type;

		// Token: 0x04002D1E RID: 11550
		[SerializeField]
		public bool m_disableIfLocal;

		// Token: 0x04002D1F RID: 11551
		[Mask(typeof(PlatformUtils.Platforms))]
		public int m_platformsActiveOn;
	}

	// Token: 0x02000AE1 RID: 2785
	[Serializable]
	public struct NameStrings
	{
		// Token: 0x04002D20 RID: 11552
		[SerializeField]
		public T17Text m_displayName;

		// Token: 0x04002D21 RID: 11553
		[SerializeField]
		public T17Text m_searching;

		// Token: 0x04002D22 RID: 11554
		[SerializeField]
		public T17Text m_empty;
	}

	// Token: 0x02000AE2 RID: 2786
	[Serializable]
	public enum UIPlayerMenuOptions
	{
		// Token: 0x04002D24 RID: 11556
		Name,
		// Token: 0x04002D25 RID: 11557
		Profile,
		// Token: 0x04002D26 RID: 11558
		Mute,
		// Token: 0x04002D27 RID: 11559
		Unmute,
		// Token: 0x04002D28 RID: 11560
		Kick
	}

	// Token: 0x02000AE3 RID: 2787
	protected enum SelectableDirection
	{
		// Token: 0x04002D2A RID: 11562
		Up,
		// Token: 0x04002D2B RID: 11563
		Down
	}
}
