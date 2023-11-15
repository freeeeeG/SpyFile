using System;
using STRINGS;
using UnityEngine;

// Token: 0x020006C1 RID: 1729
[AddComponentMenu("KMonoBehaviour/Workable/Butcherable")]
public class Butcherable : Workable, ISaveLoadable
{
	// Token: 0x06002F0D RID: 12045 RVA: 0x000F82AE File Offset: 0x000F64AE
	public void SetDrops(string[] drops)
	{
		this.drops = drops;
	}

	// Token: 0x06002F0E RID: 12046 RVA: 0x000F82B8 File Offset: 0x000F64B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Butcherable>(1272413801, Butcherable.SetReadyToButcherDelegate);
		base.Subscribe<Butcherable>(493375141, Butcherable.OnRefreshUserMenuDelegate);
		this.workTime = 3f;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
	}

	// Token: 0x06002F0F RID: 12047 RVA: 0x000F8318 File Offset: 0x000F6518
	public void SetReadyToButcher(object param)
	{
		this.readyToButcher = true;
	}

	// Token: 0x06002F10 RID: 12048 RVA: 0x000F8321 File Offset: 0x000F6521
	public void SetReadyToButcher(bool ready)
	{
		this.readyToButcher = ready;
	}

	// Token: 0x06002F11 RID: 12049 RVA: 0x000F832C File Offset: 0x000F652C
	public void ActivateChore(object param)
	{
		if (this.chore != null)
		{
			return;
		}
		this.chore = new WorkChore<Butcherable>(Db.Get().ChoreTypes.Harvest, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06002F12 RID: 12050 RVA: 0x000F8375 File Offset: 0x000F6575
	public void CancelChore(object param)
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x06002F13 RID: 12051 RVA: 0x000F8397 File Offset: 0x000F6597
	private void OnClickCancel()
	{
		this.CancelChore(null);
	}

	// Token: 0x06002F14 RID: 12052 RVA: 0x000F83A0 File Offset: 0x000F65A0
	private void OnClickButcher()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnButcherComplete();
			return;
		}
		this.ActivateChore(null);
	}

	// Token: 0x06002F15 RID: 12053 RVA: 0x000F83B8 File Offset: 0x000F65B8
	private void OnRefreshUserMenu(object data)
	{
		if (!this.readyToButcher)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_harvest", "Cancel Meatify", new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, "", true) : new KIconButtonMenu.ButtonInfo("action_harvest", "Meatify", new System.Action(this.OnClickButcher), global::Action.NumActions, null, null, null, "", true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06002F16 RID: 12054 RVA: 0x000F8446 File Offset: 0x000F6646
	protected override void OnCompleteWork(Worker worker)
	{
		this.OnButcherComplete();
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x000F8450 File Offset: 0x000F6650
	public GameObject[] CreateDrops()
	{
		GameObject[] array = new GameObject[this.drops.Length];
		for (int i = 0; i < this.drops.Length; i++)
		{
			GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, this.drops[i], Grid.SceneLayer.Ore);
			gameObject.SetActive(true);
			Edible component = gameObject.GetComponent<Edible>();
			if (component)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
			}
			array[i] = gameObject;
		}
		return array;
	}

	// Token: 0x06002F18 RID: 12056 RVA: 0x000F84E8 File Offset: 0x000F66E8
	public void OnButcherComplete()
	{
		if (this.butchered)
		{
			return;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component && component.IsSelected)
		{
			SelectTool.Instance.Select(null, false);
		}
		Pickupable component2 = base.GetComponent<Pickupable>();
		Storage storage = (component2 != null) ? component2.storage : null;
		GameObject[] array = this.CreateDrops();
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (storage != null && storage.storeDropsFromButcherables)
				{
					storage.Store(array[i], false, false, true, false);
				}
			}
		}
		this.chore = null;
		this.butchered = true;
		this.readyToButcher = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
		base.Trigger(395373363, array);
	}

	// Token: 0x06002F19 RID: 12057 RVA: 0x000F85B0 File Offset: 0x000F67B0
	private int GetDropSpawnLocation()
	{
		int num = Grid.PosToCell(base.gameObject);
		int num2 = Grid.CellAbove(num);
		if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
		{
			return num2;
		}
		return num;
	}

	// Token: 0x04001BDE RID: 7134
	[MyCmpGet]
	private KAnimControllerBase controller;

	// Token: 0x04001BDF RID: 7135
	[MyCmpGet]
	private Harvestable harvestable;

	// Token: 0x04001BE0 RID: 7136
	private bool readyToButcher;

	// Token: 0x04001BE1 RID: 7137
	private bool butchered;

	// Token: 0x04001BE2 RID: 7138
	public string[] drops;

	// Token: 0x04001BE3 RID: 7139
	private Chore chore;

	// Token: 0x04001BE4 RID: 7140
	private static readonly EventSystem.IntraObjectHandler<Butcherable> SetReadyToButcherDelegate = new EventSystem.IntraObjectHandler<Butcherable>(delegate(Butcherable component, object data)
	{
		component.SetReadyToButcher(data);
	});

	// Token: 0x04001BE5 RID: 7141
	private static readonly EventSystem.IntraObjectHandler<Butcherable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Butcherable>(delegate(Butcherable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
