using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A08 RID: 2568
public class Trap : StateMachineComponent<Trap.StatesInstance>
{
	// Token: 0x06004CD8 RID: 19672 RVA: 0x001AEF54 File Offset: 0x001AD154
	private static void CreateStatusItems()
	{
		if (Trap.statusSprung == null)
		{
			Trap.statusReady = new StatusItem("Ready", BUILDING.STATUSITEMS.CREATURE_TRAP.READY.NAME, BUILDING.STATUSITEMS.CREATURE_TRAP.READY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			Trap.statusSprung = new StatusItem("Sprung", BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.NAME, BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			Trap.statusSprung.resolveTooltipCallback = delegate(string str, object obj)
			{
				Trap.StatesInstance statesInstance = (Trap.StatesInstance)obj;
				return string.Format(str, statesInstance.master.contents.Get().GetProperName());
			};
		}
	}

	// Token: 0x06004CD9 RID: 19673 RVA: 0x001AF002 File Offset: 0x001AD202
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.contents = new Ref<KPrefabID>();
		Trap.CreateStatusItems();
	}

	// Token: 0x06004CDA RID: 19674 RVA: 0x001AF01C File Offset: 0x001AD21C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage component = base.GetComponent<Storage>();
		base.smi.StartSM();
		if (!component.IsEmpty())
		{
			KPrefabID component2 = component.items[0].GetComponent<KPrefabID>();
			if (component2 != null)
			{
				this.contents.Set(component2);
				base.smi.GoTo(base.smi.sm.occupied);
				return;
			}
			component.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x04003225 RID: 12837
	[Serialize]
	private Ref<KPrefabID> contents;

	// Token: 0x04003226 RID: 12838
	public TagSet captureTags = new TagSet();

	// Token: 0x04003227 RID: 12839
	private static StatusItem statusReady;

	// Token: 0x04003228 RID: 12840
	private static StatusItem statusSprung;

	// Token: 0x02001892 RID: 6290
	public class StatesInstance : GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.GameInstance
	{
		// Token: 0x06009225 RID: 37413 RVA: 0x0032B60D File Offset: 0x0032980D
		public StatesInstance(Trap master) : base(master)
		{
		}

		// Token: 0x06009226 RID: 37414 RVA: 0x0032B618 File Offset: 0x00329818
		public void OnTrapTriggered(object data)
		{
			KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
			base.master.contents.Set(component);
			base.smi.sm.trapTriggered.Trigger(base.smi);
		}
	}

	// Token: 0x02001893 RID: 6291
	public class States : GameStateMachine<Trap.States, Trap.StatesInstance, Trap>
	{
		// Token: 0x06009227 RID: 37415 RVA: 0x0032B660 File Offset: 0x00329860
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready;
			base.serializable = StateMachine.SerializeType.Never;
			Trap.CreateStatusItems();
			this.ready.EventHandler(GameHashes.TrapTriggered, delegate(Trap.StatesInstance smi, object data)
			{
				smi.OnTrapTriggered(data);
			}).OnSignal(this.trapTriggered, this.trapping).ToggleStatusItem(Trap.statusReady, null);
			this.trapping.PlayAnim("working_pre").OnAnimQueueComplete(this.occupied);
			this.occupied.ToggleTag(GameTags.Trapped).ToggleStatusItem(Trap.statusSprung, (Trap.StatesInstance smi) => smi).DefaultState(this.occupied.idle).EventTransition(GameHashes.OnStorageChange, this.finishedUsing, (Trap.StatesInstance smi) => smi.master.GetComponent<Storage>().IsEmpty());
			this.occupied.idle.PlayAnim("working_loop", KAnim.PlayMode.Loop);
			this.finishedUsing.PlayAnim("working_pst").OnAnimQueueComplete(this.destroySelf);
			this.destroySelf.Enter(delegate(Trap.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x04007258 RID: 29272
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State ready;

		// Token: 0x04007259 RID: 29273
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State trapping;

		// Token: 0x0400725A RID: 29274
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State finishedUsing;

		// Token: 0x0400725B RID: 29275
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State destroySelf;

		// Token: 0x0400725C RID: 29276
		public StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.Signal trapTriggered;

		// Token: 0x0400725D RID: 29277
		public Trap.States.OccupiedStates occupied;

		// Token: 0x02002201 RID: 8705
		public class OccupiedStates : GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State
		{
			// Token: 0x0400983A RID: 38970
			public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State idle;
		}
	}
}
