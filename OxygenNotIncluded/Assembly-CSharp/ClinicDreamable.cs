using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200048E RID: 1166
[AddComponentMenu("KMonoBehaviour/Workable/Clinic Dreamable")]
public class ClinicDreamable : Workable
{
	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06001A15 RID: 6677 RVA: 0x0008A147 File Offset: 0x00088347
	// (set) Token: 0x06001A16 RID: 6678 RVA: 0x0008A14F File Offset: 0x0008834F
	public bool DreamIsDisturbed { get; private set; }

	// Token: 0x06001A17 RID: 6679 RVA: 0x0008A158 File Offset: 0x00088358
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.resetProgressOnStop = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Dreaming;
		this.workingStatusItem = null;
	}

	// Token: 0x06001A18 RID: 6680 RVA: 0x0008A184 File Offset: 0x00088384
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (ClinicDreamable.dreamJournalPrefab == null)
		{
			ClinicDreamable.dreamJournalPrefab = Assets.GetPrefab(DreamJournalConfig.ID);
			ClinicDreamable.sleepClinic = Db.Get().effects.Get("SleepClinic");
		}
		this.equippable = base.GetComponent<Equippable>();
		global::Debug.Assert(this.equippable != null);
		EquipmentDef def = this.equippable.def;
		def.OnEquipCallBack = (Action<Equippable>)Delegate.Combine(def.OnEquipCallBack, new Action<Equippable>(this.OnEquipPajamas));
		EquipmentDef def2 = this.equippable.def;
		def2.OnUnequipCallBack = (Action<Equippable>)Delegate.Combine(def2.OnUnequipCallBack, new Action<Equippable>(this.OnUnequipPajamas));
		this.OnEquipPajamas(this.equippable);
	}

	// Token: 0x06001A19 RID: 6681 RVA: 0x0008A250 File Offset: 0x00088450
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.equippable == null)
		{
			return;
		}
		EquipmentDef def = this.equippable.def;
		def.OnEquipCallBack = (Action<Equippable>)Delegate.Remove(def.OnEquipCallBack, new Action<Equippable>(this.OnEquipPajamas));
		EquipmentDef def2 = this.equippable.def;
		def2.OnUnequipCallBack = (Action<Equippable>)Delegate.Remove(def2.OnUnequipCallBack, new Action<Equippable>(this.OnUnequipPajamas));
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x0008A2CC File Offset: 0x000884CC
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (this.GetPercentComplete() >= 1f)
		{
			Vector3 position = this.dreamer.transform.position;
			position.y += 1f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Util.KInstantiate(ClinicDreamable.dreamJournalPrefab, position, Quaternion.identity, null, null, true, 0).SetActive(true);
			this.workTimeRemaining = this.GetWorkTime();
		}
		return false;
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x0008A344 File Offset: 0x00088544
	public void OnEquipPajamas(Equippable eq)
	{
		if (this.equippable == null || this.equippable != eq)
		{
			return;
		}
		MinionAssignablesProxy minionAssignablesProxy = this.equippable.assignee as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return;
		}
		if (minionAssignablesProxy.target is StoredMinionIdentity)
		{
			return;
		}
		GameObject targetGameObject = minionAssignablesProxy.GetTargetGameObject();
		this.effects = targetGameObject.GetComponent<Effects>();
		this.dreamer = targetGameObject.GetComponent<ChoreDriver>();
		this.selectable = targetGameObject.GetComponent<KSelectable>();
		this.dreamer.Subscribe(-1283701846, new Action<object>(this.WorkerStartedSleeping));
		this.dreamer.Subscribe(-2090444759, new Action<object>(this.WorkerStoppedSleeping));
		this.effects.Add(ClinicDreamable.sleepClinic, true);
		this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Wearing, null);
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x0008A42C File Offset: 0x0008862C
	public void OnUnequipPajamas(Equippable eq)
	{
		if (this.dreamer == null)
		{
			return;
		}
		if (this.equippable == null || this.equippable != eq)
		{
			return;
		}
		this.dreamer.Unsubscribe(-1283701846, new Action<object>(this.WorkerStartedSleeping));
		this.dreamer.Unsubscribe(-2090444759, new Action<object>(this.WorkerStoppedSleeping));
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Wearing, false);
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, false);
		this.effects.Remove(ClinicDreamable.sleepClinic.Id);
		this.StopDreamingThought();
		this.dreamer = null;
		this.selectable = null;
		this.effects = null;
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x0008A508 File Offset: 0x00088708
	public void WorkerStartedSleeping(object data)
	{
		SleepChore sleepChore = this.dreamer.GetCurrentChore() as SleepChore;
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context = sleepChore.smi.sm.isDisturbedByLight.GetContext(sleepChore.smi);
		context.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context2 = sleepChore.smi.sm.isDisturbedByMovement.GetContext(sleepChore.smi);
		context2.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context2.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context3 = sleepChore.smi.sm.isDisturbedByNoise.GetContext(sleepChore.smi);
		context3.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context3.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context4 = sleepChore.smi.sm.isScaredOfDark.GetContext(sleepChore.smi);
		context4.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context4.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		this.sleepable = (data as Sleepable);
		this.sleepable.Dreamable = this;
		base.StartWork(this.sleepable.worker);
		this.progressBar.Retarget(this.sleepable.gameObject);
		this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, this);
		this.StartDreamingThought();
	}

	// Token: 0x06001A1E RID: 6686 RVA: 0x0008A678 File Offset: 0x00088878
	public void WorkerStoppedSleeping(object data)
	{
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, false);
		SleepChore sleepChore = this.dreamer.GetCurrentChore() as SleepChore;
		if (!sleepChore.IsNullOrDestroyed() && !sleepChore.smi.IsNullOrDestroyed() && !sleepChore.smi.sm.IsNullOrDestroyed())
		{
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context = sleepChore.smi.sm.isDisturbedByLight.GetContext(sleepChore.smi);
			context.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context2 = sleepChore.smi.sm.isDisturbedByMovement.GetContext(sleepChore.smi);
			context2.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context2.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context3 = sleepChore.smi.sm.isDisturbedByNoise.GetContext(sleepChore.smi);
			context3.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context3.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context4 = sleepChore.smi.sm.isScaredOfDark.GetContext(sleepChore.smi);
			context4.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context4.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		}
		this.StopDreamingThought();
		this.DreamIsDisturbed = false;
		if (base.worker != null)
		{
			base.StopWork(base.worker, false);
		}
		if (this.sleepable != null)
		{
			this.sleepable.Dreamable = null;
			this.sleepable = null;
		}
	}

	// Token: 0x06001A1F RID: 6687 RVA: 0x0008A81C File Offset: 0x00088A1C
	private void OnSleepDisturbed(SleepChore.StatesInstance smi)
	{
		SleepChore sleepChore = this.dreamer.GetCurrentChore() as SleepChore;
		bool flag = sleepChore.smi.sm.isDisturbedByLight.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isDisturbedByMovement.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isDisturbedByNoise.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isScaredOfDark.Get(sleepChore.smi);
		this.DreamIsDisturbed = flag;
		if (flag)
		{
			this.StopDreamingThought();
		}
	}

	// Token: 0x06001A20 RID: 6688 RVA: 0x0008A8C0 File Offset: 0x00088AC0
	private void StartDreamingThought()
	{
		if (this.dreamer != null && !this.HasStartedThoughts_Dreaming)
		{
			this.dreamer.GetSMI<Dreamer.Instance>().SetDream(Db.Get().Dreams.CommonDream);
			this.dreamer.GetSMI<Dreamer.Instance>().StartDreaming();
			this.HasStartedThoughts_Dreaming = true;
		}
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x0008A919 File Offset: 0x00088B19
	private void StopDreamingThought()
	{
		if (this.dreamer != null && this.HasStartedThoughts_Dreaming)
		{
			this.dreamer.GetSMI<Dreamer.Instance>().StopDreaming();
			this.HasStartedThoughts_Dreaming = false;
		}
	}

	// Token: 0x04000E7A RID: 3706
	private static GameObject dreamJournalPrefab;

	// Token: 0x04000E7B RID: 3707
	private static Effect sleepClinic;

	// Token: 0x04000E7C RID: 3708
	public bool HasStartedThoughts_Dreaming;

	// Token: 0x04000E7E RID: 3710
	private ChoreDriver dreamer;

	// Token: 0x04000E7F RID: 3711
	private Equippable equippable;

	// Token: 0x04000E80 RID: 3712
	private Effects effects;

	// Token: 0x04000E81 RID: 3713
	private Sleepable sleepable;

	// Token: 0x04000E82 RID: 3714
	private KSelectable selectable;

	// Token: 0x04000E83 RID: 3715
	private HashedString dreamAnimName = "portal rocket comp";
}
