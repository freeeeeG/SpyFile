using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005E5 RID: 1509
[SerializationConfig(MemberSerialization.OptIn)]
public class Desalinator : StateMachineComponent<Desalinator.StatesInstance>
{
	// Token: 0x170001FD RID: 509
	// (get) Token: 0x0600259F RID: 9631 RVA: 0x000CC654 File Offset: 0x000CA854
	// (set) Token: 0x060025A0 RID: 9632 RVA: 0x000CC65C File Offset: 0x000CA85C
	public float SaltStorageLeft
	{
		get
		{
			return this._storageLeft;
		}
		set
		{
			this._storageLeft = value;
			base.smi.sm.saltStorageLeft.Set(value, base.smi, false);
		}
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x000CC684 File Offset: 0x000CA884
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.OnConduitConnectionChanged(base.GetComponent<ConduitConsumer>().IsConnected);
		base.Subscribe<Desalinator>(-2094018600, Desalinator.OnConduitConnectionChangedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x000CC6D8 File Offset: 0x000CA8D8
	private void OnConduitConnectionChanged(object data)
	{
		bool pause = (bool)data;
		foreach (ManualDeliveryKG manualDeliveryKG in this.deliveryComponents)
		{
			Element element = ElementLoader.GetElement(manualDeliveryKG.RequestedItemTag);
			if (element != null && element.IsLiquid)
			{
				manualDeliveryKG.Pause(pause, "pipe connected");
			}
		}
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x000CC72C File Offset: 0x000CA92C
	private void OnRefreshUserMenu(object data)
	{
		if (base.smi.GetCurrentState() == base.smi.sm.full || !base.smi.HasSalt || base.smi.emptyChore != null)
		{
			return;
		}
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("status_item_desalinator_needs_emptying", UI.USERMENUACTIONS.EMPTYDESALINATOR.NAME, delegate()
		{
			base.smi.GoTo(base.smi.sm.earlyEmpty);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
	}

	// Token: 0x060025A4 RID: 9636 RVA: 0x000CC7C0 File Offset: 0x000CA9C0
	private bool CheckCanConvert()
	{
		if (this.converters == null)
		{
			this.converters = base.GetComponents<ElementConverter>();
		}
		for (int i = 0; i < this.converters.Length; i++)
		{
			if (this.converters[i].CanConvertAtAll())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060025A5 RID: 9637 RVA: 0x000CC808 File Offset: 0x000CAA08
	private bool CheckEnoughMassToConvert()
	{
		if (this.converters == null)
		{
			this.converters = base.GetComponents<ElementConverter>();
		}
		for (int i = 0; i < this.converters.Length; i++)
		{
			if (this.converters[i].HasEnoughMassToStartConverting(false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0400157C RID: 5500
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400157D RID: 5501
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x0400157E RID: 5502
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400157F RID: 5503
	[Serialize]
	public float maxSalt = 1000f;

	// Token: 0x04001580 RID: 5504
	[Serialize]
	private float _storageLeft = 1000f;

	// Token: 0x04001581 RID: 5505
	private ElementConverter[] converters;

	// Token: 0x04001582 RID: 5506
	private static readonly EventSystem.IntraObjectHandler<Desalinator> OnConduitConnectionChangedDelegate = new EventSystem.IntraObjectHandler<Desalinator>(delegate(Desalinator component, object data)
	{
		component.OnConduitConnectionChanged(data);
	});

	// Token: 0x0200127E RID: 4734
	public class StatesInstance : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.GameInstance
	{
		// Token: 0x06007D99 RID: 32153 RVA: 0x002E4D52 File Offset: 0x002E2F52
		public StatesInstance(Desalinator smi) : base(smi)
		{
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06007D9A RID: 32154 RVA: 0x002E4D5B File Offset: 0x002E2F5B
		public bool HasSalt
		{
			get
			{
				return base.master.storage.Has(ElementLoader.FindElementByHash(SimHashes.Salt).tag);
			}
		}

		// Token: 0x06007D9B RID: 32155 RVA: 0x002E4D7C File Offset: 0x002E2F7C
		public bool IsFull()
		{
			return base.master.SaltStorageLeft <= 0f;
		}

		// Token: 0x06007D9C RID: 32156 RVA: 0x002E4D93 File Offset: 0x002E2F93
		public bool IsSaltRemoved()
		{
			return !this.HasSalt;
		}

		// Token: 0x06007D9D RID: 32157 RVA: 0x002E4DA0 File Offset: 0x002E2FA0
		public void CreateEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("dupe");
			}
			DesalinatorWorkableEmpty component = base.master.GetComponent<DesalinatorWorkableEmpty>();
			this.emptyChore = new WorkChore<DesalinatorWorkableEmpty>(Db.Get().ChoreTypes.EmptyDesalinator, component, null, true, new Action<Chore>(this.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

		// Token: 0x06007D9E RID: 32158 RVA: 0x002E4E08 File Offset: 0x002E3008
		public void CancelEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("Cancelled");
				this.emptyChore = null;
			}
		}

		// Token: 0x06007D9F RID: 32159 RVA: 0x002E4E2C File Offset: 0x002E302C
		private void OnEmptyComplete(Chore chore)
		{
			this.emptyChore = null;
			Tag tag = GameTagExtensions.Create(SimHashes.Salt);
			ListPool<GameObject, Desalinator>.PooledList pooledList = ListPool<GameObject, Desalinator>.Allocate();
			base.master.storage.Find(tag, pooledList);
			foreach (GameObject go in pooledList)
			{
				base.master.storage.Drop(go, true);
			}
			pooledList.Recycle();
		}

		// Token: 0x06007DA0 RID: 32160 RVA: 0x002E4EB8 File Offset: 0x002E30B8
		public void UpdateStorageLeft()
		{
			Tag tag = GameTagExtensions.Create(SimHashes.Salt);
			base.master.SaltStorageLeft = base.master.maxSalt - base.master.storage.GetMassAvailable(tag);
		}

		// Token: 0x04005FD3 RID: 24531
		public Chore emptyChore;
	}

	// Token: 0x0200127F RID: 4735
	public class States : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator>
	{
		// Token: 0x06007DA1 RID: 32161 RVA: 0x002E4EF8 File Offset: 0x002E30F8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (Desalinator.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (Desalinator.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (Desalinator.StatesInstance smi) => smi.master.CheckEnoughMassToConvert());
			this.on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.on.working_pst, (Desalinator.StatesInstance smi) => !smi.master.CheckCanConvert()).ParamTransition<float>(this.saltStorageLeft, this.full, (Desalinator.StatesInstance smi, float p) => smi.IsFull()).EventHandler(GameHashes.OnStorageChange, delegate(Desalinator.StatesInstance smi)
			{
				smi.UpdateStorageLeft();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.waiting);
			this.earlyEmpty.PlayAnims((Desalinator.StatesInstance smi) => Desalinator.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.earlyWaitingForEmpty);
			this.earlyWaitingForEmpty.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.CancelEmptyChore();
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Desalinator.StatesInstance smi) => smi.IsSaltRemoved());
			this.full.PlayAnims((Desalinator.StatesInstance smi) => Desalinator.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.fullWaitingForEmpty);
			this.fullWaitingForEmpty.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.CancelEmptyChore();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.DesalinatorNeedsEmptying, null).EventTransition(GameHashes.OnStorageChange, this.empty, (Desalinator.StatesInstance smi) => smi.IsSaltRemoved());
			this.empty.PlayAnim("off").Enter("ResetStorage", delegate(Desalinator.StatesInstance smi)
			{
				smi.master.SaltStorageLeft = smi.master.maxSalt;
			}).GoTo(this.on.waiting);
		}

		// Token: 0x04005FD4 RID: 24532
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State off;

		// Token: 0x04005FD5 RID: 24533
		public Desalinator.States.OnStates on;

		// Token: 0x04005FD6 RID: 24534
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State full;

		// Token: 0x04005FD7 RID: 24535
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State fullWaitingForEmpty;

		// Token: 0x04005FD8 RID: 24536
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State earlyEmpty;

		// Token: 0x04005FD9 RID: 24537
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State earlyWaitingForEmpty;

		// Token: 0x04005FDA RID: 24538
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State empty;

		// Token: 0x04005FDB RID: 24539
		private static readonly HashedString[] FULL_ANIMS = new HashedString[]
		{
			"working_pst",
			"off"
		};

		// Token: 0x04005FDC RID: 24540
		public StateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.FloatParameter saltStorageLeft = new StateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.FloatParameter(0f);

		// Token: 0x020020C4 RID: 8388
		public class OnStates : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State
		{
			// Token: 0x04009288 RID: 37512
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State waiting;

			// Token: 0x04009289 RID: 37513
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working_pre;

			// Token: 0x0400928A RID: 37514
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working;

			// Token: 0x0400928B RID: 37515
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working_pst;
		}
	}
}
