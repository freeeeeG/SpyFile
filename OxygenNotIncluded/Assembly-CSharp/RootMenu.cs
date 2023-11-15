using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000A7B RID: 2683
public class RootMenu : KScreen
{
	// Token: 0x06005168 RID: 20840 RVA: 0x001CF1A7 File Offset: 0x001CD3A7
	public static void DestroyInstance()
	{
		RootMenu.Instance = null;
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x06005169 RID: 20841 RVA: 0x001CF1AF File Offset: 0x001CD3AF
	// (set) Token: 0x0600516A RID: 20842 RVA: 0x001CF1B6 File Offset: 0x001CD3B6
	public static RootMenu Instance { get; private set; }

	// Token: 0x0600516B RID: 20843 RVA: 0x001CF1BE File Offset: 0x001CD3BE
	public override float GetSortKey()
	{
		return -1f;
	}

	// Token: 0x0600516C RID: 20844 RVA: 0x001CF1C8 File Offset: 0x001CD3C8
	protected override void OnPrefabInit()
	{
		RootMenu.Instance = this;
		base.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
		base.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
		base.Subscribe(Game.Instance.gameObject, -809948329, new Action<object>(this.OnBuildingStatechanged));
		base.OnPrefabInit();
	}

	// Token: 0x0600516D RID: 20845 RVA: 0x001CF248 File Offset: 0x001CD448
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.detailsScreen = Util.KInstantiateUI(this.detailsScreenPrefab, base.gameObject, true).GetComponent<DetailsScreen>();
		this.detailsScreen.gameObject.SetActive(true);
		this.userMenuParent = this.detailsScreen.UserMenuPanel.gameObject;
		this.userMenu = Util.KInstantiateUI(this.userMenuPrefab.gameObject, this.userMenuParent, false).GetComponent<UserMenuScreen>();
		this.detailsScreen.gameObject.SetActive(false);
		this.userMenu.gameObject.SetActive(false);
	}

	// Token: 0x0600516E RID: 20846 RVA: 0x001CF2E3 File Offset: 0x001CD4E3
	private void OnClickCommon()
	{
		this.CloseSubMenus();
	}

	// Token: 0x0600516F RID: 20847 RVA: 0x001CF2EB File Offset: 0x001CD4EB
	public void AddSubMenu(KScreen sub_menu)
	{
		if (sub_menu.activateOnSpawn)
		{
			sub_menu.Show(true);
		}
		this.subMenus.Add(sub_menu);
	}

	// Token: 0x06005170 RID: 20848 RVA: 0x001CF308 File Offset: 0x001CD508
	public void RemoveSubMenu(KScreen sub_menu)
	{
		this.subMenus.Remove(sub_menu);
	}

	// Token: 0x06005171 RID: 20849 RVA: 0x001CF318 File Offset: 0x001CD518
	private void CloseSubMenus()
	{
		foreach (KScreen kscreen in this.subMenus)
		{
			if (kscreen != null)
			{
				if (kscreen.activateOnSpawn)
				{
					kscreen.gameObject.SetActive(false);
				}
				else
				{
					kscreen.Deactivate();
				}
			}
		}
		this.subMenus.Clear();
	}

	// Token: 0x06005172 RID: 20850 RVA: 0x001CF394 File Offset: 0x001CD594
	private void OnSelectObject(object data)
	{
		GameObject gameObject = (GameObject)data;
		bool flag = false;
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && !component.IsInitialized())
			{
				return;
			}
			flag = (component != null || CellSelectionObject.IsSelectionObject(gameObject));
		}
		if (gameObject != this.selectedGO)
		{
			if (this.selectedGO != null)
			{
				this.selectedGO.Unsubscribe(1980521255, new Action<object>(this.TriggerRefresh));
			}
			this.selectedGO = null;
			this.CloseSubMenus();
			if (flag)
			{
				this.selectedGO = gameObject;
				this.selectedGO.Subscribe(1980521255, new Action<object>(this.TriggerRefresh));
				this.AddSubMenu(this.detailsScreen);
				this.AddSubMenu(this.userMenu);
			}
			this.userMenu.SetSelected(this.selectedGO);
		}
		this.Refresh();
	}

	// Token: 0x06005173 RID: 20851 RVA: 0x001CF47D File Offset: 0x001CD67D
	public void TriggerRefresh(object obj)
	{
		this.Refresh();
	}

	// Token: 0x06005174 RID: 20852 RVA: 0x001CF485 File Offset: 0x001CD685
	public void Refresh()
	{
		if (this.selectedGO == null)
		{
			return;
		}
		this.detailsScreen.Refresh(this.selectedGO);
		this.userMenu.Refresh(this.selectedGO);
	}

	// Token: 0x06005175 RID: 20853 RVA: 0x001CF4B8 File Offset: 0x001CD6B8
	private void OnBuildingStatechanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == this.selectedGO)
		{
			this.OnSelectObject(gameObject);
		}
	}

	// Token: 0x06005176 RID: 20854 RVA: 0x001CF4E4 File Offset: 0x001CD6E4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && e.TryConsume(global::Action.Escape) && SelectTool.Instance.enabled)
		{
			if (!this.canTogglePauseScreen)
			{
				return;
			}
			if (this.AreSubMenusOpen())
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
				this.CloseSubMenus();
				SelectTool.Instance.Select(null, false);
			}
			else if (e.IsAction(global::Action.Escape))
			{
				if (!SelectTool.Instance.enabled)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
				}
				if (PlayerController.Instance.IsUsingDefaultTool())
				{
					if (SelectTool.Instance.selected != null)
					{
						SelectTool.Instance.Select(null, false);
					}
					else
					{
						CameraController.Instance.ForcePanningState(false);
						this.TogglePauseScreen();
					}
				}
				else
				{
					Game.Instance.Trigger(288942073, null);
				}
				ToolMenu.Instance.ClearSelection();
				SelectTool.Instance.Activate();
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005177 RID: 20855 RVA: 0x001CF5E6 File Offset: 0x001CD7E6
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		if (!e.Consumed && e.TryConsume(global::Action.AlternateView) && this.tileScreenInst != null)
		{
			this.tileScreenInst.Deactivate();
			this.tileScreenInst = null;
		}
	}

	// Token: 0x06005178 RID: 20856 RVA: 0x001CF621 File Offset: 0x001CD821
	public void TogglePauseScreen()
	{
		PauseScreen.Instance.Show(true);
	}

	// Token: 0x06005179 RID: 20857 RVA: 0x001CF62E File Offset: 0x001CD82E
	public void ExternalClose()
	{
		this.OnClickCommon();
	}

	// Token: 0x0600517A RID: 20858 RVA: 0x001CF636 File Offset: 0x001CD836
	private void OnUIClear(object data)
	{
		this.CloseSubMenus();
		SelectTool.Instance.Select(null, true);
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
			return;
		}
		global::Debug.LogWarning("OnUIClear() Event system is null");
	}

	// Token: 0x0600517B RID: 20859 RVA: 0x001CF66D File Offset: 0x001CD86D
	protected override void OnActivate()
	{
		base.OnActivate();
	}

	// Token: 0x0600517C RID: 20860 RVA: 0x001CF675 File Offset: 0x001CD875
	private bool AreSubMenusOpen()
	{
		return this.subMenus.Count > 0;
	}

	// Token: 0x0600517D RID: 20861 RVA: 0x001CF688 File Offset: 0x001CD888
	private KToggleMenu.ToggleInfo[] GetFillers()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		List<KToggleMenu.ToggleInfo> list = new List<KToggleMenu.ToggleInfo>();
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			KPrefabID kprefabID = pickupable.KPrefabID;
			if (kprefabID.HasTag(GameTags.Filler) && hashSet.Add(kprefabID.PrefabTag))
			{
				string text = kprefabID.GetComponent<PrimaryElement>().Element.id.ToString();
				list.Add(new KToggleMenu.ToggleInfo(text, null, global::Action.NumActions));
			}
		}
		return list.ToArray();
	}

	// Token: 0x0600517E RID: 20862 RVA: 0x001CF73C File Offset: 0x001CD93C
	public bool IsBuildingChorePanelActive()
	{
		return this.detailsScreen != null && this.detailsScreen.GetActiveTab() is BuildingChoresPanel;
	}

	// Token: 0x04003572 RID: 13682
	private DetailsScreen detailsScreen;

	// Token: 0x04003573 RID: 13683
	private UserMenuScreen userMenu;

	// Token: 0x04003574 RID: 13684
	[SerializeField]
	private GameObject detailsScreenPrefab;

	// Token: 0x04003575 RID: 13685
	[SerializeField]
	private UserMenuScreen userMenuPrefab;

	// Token: 0x04003576 RID: 13686
	private GameObject userMenuParent;

	// Token: 0x04003577 RID: 13687
	[SerializeField]
	private TileScreen tileScreen;

	// Token: 0x04003579 RID: 13689
	public KScreen buildMenu;

	// Token: 0x0400357A RID: 13690
	private List<KScreen> subMenus = new List<KScreen>();

	// Token: 0x0400357B RID: 13691
	private TileScreen tileScreenInst;

	// Token: 0x0400357C RID: 13692
	public bool canTogglePauseScreen = true;

	// Token: 0x0400357D RID: 13693
	public GameObject selectedGO;
}
