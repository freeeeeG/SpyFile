using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003BB RID: 955
public class MovePickupableChore : Chore<MovePickupableChore.StatesInstance>
{
	// Token: 0x060013E2 RID: 5090 RVA: 0x00069DAC File Offset: 0x00067FAC
	public MovePickupableChore(IStateMachineTarget target, GameObject pickupable, Action<Chore> onEnd) : base(Db.Get().ChoreTypes.Fetch, target, target.GetComponent<ChoreProvider>(), false, null, null, onEnd, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MovePickupableChore.StatesInstance(this);
		Pickupable component = pickupable.GetComponent<Pickupable>();
		base.AddPrecondition(ChorePreconditions.instance.CanMoveTo, target.GetComponent<Storage>());
		base.AddPrecondition(ChorePreconditions.instance.IsNotARobot, this);
		base.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, this);
		if (pickupable.GetComponent<CreatureBrain>())
		{
			base.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Reachable);
			base.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanWrangleCreatures);
			base.AddPrecondition(ChorePreconditions.instance.CanMoveTo, pickupable.GetComponent<Capturable>());
		}
		else
		{
			base.AddPrecondition(ChorePreconditions.instance.CanPickup, component);
		}
		PrimaryElement component2 = pickupable.GetComponent<PrimaryElement>();
		base.smi.sm.requestedamount.Set(component2.Mass, base.smi, false);
		base.smi.sm.pickupablesource.Set(pickupable.gameObject, base.smi, false);
		base.smi.sm.deliverypoint.Set(target.gameObject, base.smi, false);
		this.movePlacer = target.gameObject;
		bool flag = MinionGroupProber.Get().IsReachable(Grid.PosToCell(pickupable), OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(Grid.PosToCell(target.gameObject), OffsetGroups.Standard);
		this.OnReachableChanged(flag);
		pickupable.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		target.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		Prioritizable component3 = target.GetComponent<Prioritizable>();
		if (!component3.IsPrioritizable())
		{
			component3.AddRef();
		}
		base.SetPrioritizable(target.GetComponent<Prioritizable>());
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x00069FAC File Offset: 0x000681AC
	private void OnReachableChanged(object data)
	{
		Color color = ((bool)data) ? Color.white : new Color(0.91f, 0.21f, 0.2f);
		this.SetColor(this.movePlacer, color);
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x00069FEA File Offset: 0x000681EA
	private void SetColor(GameObject visualizer, Color color)
	{
		if (visualizer != null)
		{
			visualizer.GetComponentInChildren<MeshRenderer>().material.color = color;
		}
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x0006A008 File Offset: 0x00068208
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("MovePickupable null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("MovePickupable null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("MovePickupable null smi.sm");
			return;
		}
		if (base.smi.sm.pickupablesource == null)
		{
			global::Debug.LogError("MovePickupable null smi.sm.pickupablesource");
			return;
		}
		base.smi.sm.deliverer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04000ABF RID: 2751
	public GameObject movePlacer;

	// Token: 0x0200100D RID: 4109
	public class StatesInstance : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.GameInstance
	{
		// Token: 0x06007487 RID: 29831 RVA: 0x002C8953 File Offset: 0x002C6B53
		public StatesInstance(MovePickupableChore master) : base(master)
		{
		}
	}

	// Token: 0x0200100E RID: 4110
	public class States : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore>
	{
		// Token: 0x06007488 RID: 29832 RVA: 0x002C895C File Offset: 0x002C6B5C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.deliverypoint);
			this.fetch.Target(this.deliverer).DefaultState(this.fetch.approach).Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				this.pickupablesource.Get<Pickupable>(smi).ClearReservations();
			}).ToggleReserve(this.deliverer, this.pickupablesource, this.requestedamount, this.actualamount).EnterTransition(this.fetch.approachCritter, (MovePickupableChore.StatesInstance smi) => this.IsCritter(smi)).OnTargetLost(this.pickupablesource, null);
			this.fetch.approachCritter.Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				GameObject gameObject = this.pickupablesource.Get(smi);
				if (!gameObject.HasTag(GameTags.Creatures.Bagged))
				{
					IdleStates.Instance smi2 = gameObject.GetSMI<IdleStates.Instance>();
					if (!smi2.IsNullOrStopped())
					{
						smi2.GoTo(smi2.sm.root);
					}
					FlopStates.Instance smi3 = gameObject.GetSMI<FlopStates.Instance>();
					if (!smi3.IsNullOrStopped())
					{
						smi3.GoTo(smi3.sm.root);
					}
					gameObject.GetComponent<Navigator>().Stop(false, true);
				}
			}).MoveTo<Capturable>(this.pickupablesource, this.fetch.wrangle, null, null, null);
			this.fetch.wrangle.EnterTransition(this.fetch.approach, (MovePickupableChore.StatesInstance smi) => this.pickupablesource.Get(smi).HasTag(GameTags.Creatures.Bagged)).ToggleWork<Capturable>(this.pickupablesource, this.fetch.approach, null, null);
			this.fetch.approach.MoveTo<IApproachable>(this.pickupablesource, this.fetch.pickup, null, null, null);
			this.fetch.pickup.DoPickup(this.pickupablesource, this.pickup, this.actualamount, this.approachstorage, this.delivering.deliverfail);
			this.approachstorage.DefaultState(this.approachstorage.deliveryStorage);
			this.approachstorage.deliveryStorage.InitializeStates(this.deliverer, this.deliverypoint, this.delivering.storing, this.delivering.deliverfail, null, NavigationTactics.ReduceTravelDistance);
			this.delivering.storing.Target(this.deliverer).DoDelivery(this.deliverer, this.deliverypoint, this.success, this.delivering.deliverfail);
			this.delivering.deliverfail.ReturnFailure();
			this.success.Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				Storage component = this.deliverypoint.Get(smi).GetComponent<Storage>();
				Storage component2 = this.deliverer.Get(smi).GetComponent<Storage>();
				float num = this.actualamount.Get(smi);
				GameObject gameObject = this.pickup.Get(smi);
				num += gameObject.GetComponent<PrimaryElement>().Mass;
				this.actualamount.Set(num, smi, false);
				component2.Transfer(this.pickup.Get(smi), component, false, false);
				this.DropPickupable(component, gameObject);
				CancellableMove component3 = component.GetComponent<CancellableMove>();
				Movable component4 = gameObject.GetComponent<Movable>();
				component3.RemoveMovable(component4);
				component4.ClearMove();
				if (!this.IsDeliveryComplete(smi))
				{
					GameObject go = this.pickupablesource.Get(smi);
					int num2 = Grid.PosToCell(this.deliverypoint.Get(smi));
					if (this.pickupablesource.Get(smi) == null || Grid.PosToCell(go) == num2)
					{
						GameObject nextTarget = component3.GetNextTarget();
						this.pickupablesource.Set(nextTarget, smi, false);
						PrimaryElement component5 = nextTarget.GetComponent<PrimaryElement>();
						smi.sm.requestedamount.Set(component5.Mass, smi, false);
					}
					smi.GoTo(this.fetch);
				}
			}).ReturnSuccess();
		}

		// Token: 0x06007489 RID: 29833 RVA: 0x002C8B7C File Offset: 0x002C6D7C
		private void DropPickupable(Storage storage, GameObject delivered)
		{
			if (delivered.GetComponent<Capturable>() != null)
			{
				List<GameObject> items = storage.items;
				int count = items.Count;
				Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(storage), Grid.SceneLayer.Creatures);
				for (int i = count - 1; i >= 0; i--)
				{
					GameObject gameObject = items[i];
					storage.Drop(gameObject, true);
					gameObject.transform.SetPosition(position);
					gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
				}
				return;
			}
			storage.DropAll(false, false, default(Vector3), true, null);
		}

		// Token: 0x0600748A RID: 29834 RVA: 0x002C8C00 File Offset: 0x002C6E00
		private bool IsDeliveryComplete(MovePickupableChore.StatesInstance smi)
		{
			GameObject gameObject = smi.sm.deliverypoint.Get(smi);
			return !(gameObject != null) || gameObject.GetComponent<CancellableMove>().IsDeliveryComplete();
		}

		// Token: 0x0600748B RID: 29835 RVA: 0x002C8C38 File Offset: 0x002C6E38
		private bool IsCritter(MovePickupableChore.StatesInstance smi)
		{
			GameObject gameObject = this.pickupablesource.Get(smi);
			return gameObject != null && gameObject.GetComponent<Capturable>() != null;
		}

		// Token: 0x04005801 RID: 22529
		public static CellOffset[] critterCellOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};

		// Token: 0x04005802 RID: 22530
		public static HashedString[] critterReleaseWorkAnims = new HashedString[]
		{
			"place",
			"release"
		};

		// Token: 0x04005803 RID: 22531
		public static KAnimFile[] critterReleaseAnim = new KAnimFile[]
		{
			Assets.GetAnim("anim_restrain_creature_kanim")
		};

		// Token: 0x04005804 RID: 22532
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter deliverer;

		// Token: 0x04005805 RID: 22533
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter pickupablesource;

		// Token: 0x04005806 RID: 22534
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter pickup;

		// Token: 0x04005807 RID: 22535
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter deliverypoint;

		// Token: 0x04005808 RID: 22536
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.FloatParameter requestedamount;

		// Token: 0x04005809 RID: 22537
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.FloatParameter actualamount;

		// Token: 0x0400580A RID: 22538
		public MovePickupableChore.States.FetchState fetch;

		// Token: 0x0400580B RID: 22539
		public MovePickupableChore.States.ApproachStorage approachstorage;

		// Token: 0x0400580C RID: 22540
		public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State success;

		// Token: 0x0400580D RID: 22541
		public MovePickupableChore.States.DeliveryState delivering;

		// Token: 0x02001FE2 RID: 8162
		public class ApproachStorage : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x04008F8D RID: 36749
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Storage> deliveryStorage;

			// Token: 0x04008F8E RID: 36750
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Storage> unbagCritter;
		}

		// Token: 0x02001FE3 RID: 8163
		public class DeliveryState : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x04008F8F RID: 36751
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State storing;

			// Token: 0x04008F90 RID: 36752
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State deliverfail;
		}

		// Token: 0x02001FE4 RID: 8164
		public class FetchState : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x04008F91 RID: 36753
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Pickupable> approach;

			// Token: 0x04008F92 RID: 36754
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State pickup;

			// Token: 0x04008F93 RID: 36755
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State approachCritter;

			// Token: 0x04008F94 RID: 36756
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State wrangle;
		}
	}
}
