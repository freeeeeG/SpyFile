using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000741 RID: 1857
public class CryoTank : StateMachineComponent<CryoTank.StatesInstance>, ISidescreenButtonControl
{
	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x06003312 RID: 13074 RVA: 0x0010F29D File Offset: 0x0010D49D
	public string SidescreenButtonText
	{
		get
		{
			return BUILDINGS.PREFABS.CRYOTANK.DEFROSTBUTTON;
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x06003313 RID: 13075 RVA: 0x0010F2A9 File Offset: 0x0010D4A9
	public string SidescreenButtonTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.CRYOTANK.DEFROSTBUTTONTOOLTIP;
		}
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x0010F2B5 File Offset: 0x0010D4B5
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x0010F2B8 File Offset: 0x0010D4B8
	public void OnSidescreenButtonPressed()
	{
		this.OnClickOpen();
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x0010F2C0 File Offset: 0x0010D4C0
	public bool SidescreenButtonInteractable()
	{
		return this.HasDefrostedFriend();
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x0010F2C8 File Offset: 0x0010D4C8
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x0010F2CC File Offset: 0x0010D4CC
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x0010F2D3 File Offset: 0x0010D4D3
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x0010F2D8 File Offset: 0x0010D4D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Demolishable component = base.GetComponent<Demolishable>();
		if (component != null)
		{
			component.allowDemolition = !this.HasDefrostedFriend();
		}
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x0010F315 File Offset: 0x0010D515
	public bool HasDefrostedFriend()
	{
		return base.smi.IsInsideState(base.smi.sm.closed) && this.chore == null;
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x0010F340 File Offset: 0x0010D540
	public void DropContents()
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID), null, null);
		gameObject.name = Assets.GetPrefab(MinionConfig.ID).name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(base.transform.position), this.dropOffset), Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		new MinionStartingStats(false, null, "AncientKnowledge", false).Apply(gameObject);
		gameObject.GetComponent<MinionIdentity>().arrivalTime = (float)UnityEngine.Random.Range(-2000, -1000);
		MinionResume component = gameObject.GetComponent<MinionResume>();
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			component.ForceAddSkillPoint();
		}
		base.smi.sm.defrostedDuplicant.Set(gameObject, base.smi, false);
		gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
		ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
		if (component2 != null)
		{
			base.smi.defrostAnimChore = new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_cryo_chamber_kanim", new HashedString[]
			{
				"defrost",
				"defrost_exit"
			}, KAnim.PlayMode.Once, false);
			Vector3 position2 = gameObject.transform.GetPosition();
			position2.z = Grid.GetLayerZ(Grid.SceneLayer.Gas);
			gameObject.transform.SetPosition(position2);
			gameObject.GetMyWorld().SetDupeVisited();
		}
		SaveGame.Instance.GetComponent<ColonyAchievementTracker>().defrostedDuplicant = true;
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x0010F4E4 File Offset: 0x0010D6E4
	public void ShowEventPopup()
	{
		GameObject gameObject = base.smi.sm.defrostedDuplicant.Get(base.smi);
		if (this.opener != null && gameObject != null)
		{
			SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.CryoFriend, -1, null).smi as SimpleEvent.StatesInstance;
			statesInstance.minions = new GameObject[]
			{
				gameObject,
				this.opener
			};
			statesInstance.SetTextParameter("dupe", this.opener.GetProperName());
			statesInstance.SetTextParameter("friend", gameObject.GetProperName());
			statesInstance.ShowEventPopup();
		}
	}

	// Token: 0x0600331E RID: 13086 RVA: 0x0010F590 File Offset: 0x0010D790
	public void Cheer()
	{
		GameObject gameObject = base.smi.sm.defrostedDuplicant.Get(base.smi);
		if (this.opener != null && gameObject != null)
		{
			Db db = Db.Get();
			this.opener.GetComponent<Effects>().Add(Db.Get().effects.Get("CryoFriend"), true);
			new EmoteChore(this.opener.GetComponent<Effects>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 1, null);
			gameObject.GetComponent<Effects>().Add(Db.Get().effects.Get("CryoFriend"), true);
			new EmoteChore(gameObject.GetComponent<Effects>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 1, null);
		}
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x0010F67A File Offset: 0x0010D87A
	private void OnClickOpen()
	{
		this.ActivateChore(null);
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x0010F683 File Offset: 0x0010D883
	private void OnClickCancel()
	{
		this.CancelActivateChore(null);
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x0010F68C File Offset: 0x0010D88C
	public void ActivateChore(object param = null)
	{
		if (this.chore != null)
		{
			return;
		}
		base.GetComponent<Workable>().SetWorkTime(1.5f);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, delegate(Chore o)
		{
			this.CompleteActivateChore();
		}, null, null, true, null, false, true, Assets.GetAnim(this.overrideAnim), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x0010F6F8 File Offset: 0x0010D8F8
	public void CancelActivateChore(object param = null)
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x0010F71C File Offset: 0x0010D91C
	private void CompleteActivateChore()
	{
		this.opener = this.chore.driver.gameObject;
		base.smi.GoTo(base.smi.sm.open);
		this.chore = null;
		Demolishable component = base.smi.GetComponent<Demolishable>();
		if (component != null)
		{
			component.allowDemolition = true;
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x04001EB9 RID: 7865
	public string[][] possible_contents_ids;

	// Token: 0x04001EBA RID: 7866
	public string machineSound;

	// Token: 0x04001EBB RID: 7867
	public string overrideAnim;

	// Token: 0x04001EBC RID: 7868
	public CellOffset dropOffset = CellOffset.none;

	// Token: 0x04001EBD RID: 7869
	private GameObject opener;

	// Token: 0x04001EBE RID: 7870
	private Chore chore;

	// Token: 0x020014DE RID: 5342
	public class StatesInstance : GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.GameInstance
	{
		// Token: 0x06008622 RID: 34338 RVA: 0x00307FB0 File Offset: 0x003061B0
		public StatesInstance(CryoTank master) : base(master)
		{
		}

		// Token: 0x040066AB RID: 26283
		public Chore defrostAnimChore;
	}

	// Token: 0x020014DF RID: 5343
	public class States : GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank>
	{
		// Token: 0x06008623 RID: 34339 RVA: 0x00307FBC File Offset: 0x003061BC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.closed;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.closed.PlayAnim("on").Enter(delegate(CryoTank.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StartSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
			this.open.GoTo(this.defrost).Exit(delegate(CryoTank.StatesInstance smi)
			{
				smi.master.DropContents();
			});
			this.defrost.PlayAnim("defrost").OnAnimQueueComplete(this.defrostExit).Update(delegate(CryoTank.StatesInstance smi, float dt)
			{
				smi.sm.defrostedDuplicant.Get(smi).GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(CryoTank.StatesInstance smi)
			{
				smi.master.ShowEventPopup();
			});
			this.defrostExit.PlayAnim("defrost_exit").Update(delegate(CryoTank.StatesInstance smi, float dt)
			{
				if (smi.defrostAnimChore == null || smi.defrostAnimChore.isComplete)
				{
					smi.GoTo(this.off);
				}
			}, UpdateRate.SIM_200ms, false).Exit(delegate(CryoTank.StatesInstance smi)
			{
				smi.sm.defrostedDuplicant.Get(smi).GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Move);
				smi.master.Cheer();
			});
			this.off.PlayAnim("off").Enter(delegate(CryoTank.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StopSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
		}

		// Token: 0x040066AC RID: 26284
		public StateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.TargetParameter defrostedDuplicant;

		// Token: 0x040066AD RID: 26285
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State closed;

		// Token: 0x040066AE RID: 26286
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State open;

		// Token: 0x040066AF RID: 26287
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State defrost;

		// Token: 0x040066B0 RID: 26288
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State defrostExit;

		// Token: 0x040066B1 RID: 26289
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State off;
	}
}
