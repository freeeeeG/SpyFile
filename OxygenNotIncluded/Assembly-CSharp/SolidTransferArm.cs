using System;
using System.Collections.Generic;
using System.Diagnostics;
using Database;
using FMODUnity;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000692 RID: 1682
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidTransferArm : StateMachineComponent<SolidTransferArm.SMInstance>, ISim1000ms, IRenderEveryTick
{
	// Token: 0x06002D05 RID: 11525 RVA: 0x000EEDB4 File Offset: 0x000ECFB4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreConsumer.AddProvider(GlobalChoreProvider.Instance);
		this.choreConsumer.SetReach(this.pickupRange);
		Klei.AI.Attributes attributes = this.GetAttributes();
		if (attributes.Get(Db.Get().Attributes.CarryAmount) == null)
		{
			attributes.Add(Db.Get().Attributes.CarryAmount);
		}
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, this.max_carry_weight, base.gameObject.GetProperName(), false, false, true);
		this.GetAttributes().Add(modifier);
		this.worker.usesMultiTool = false;
		this.storage.fxPrefix = Storage.FXPrefix.PickedUp;
		this.simRenderLoadBalance = false;
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x000EEE78 File Offset: 0x000ED078
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		string name = component.name + ".arm";
		this.arm_go = new GameObject(name);
		this.arm_go.SetActive(false);
		this.arm_go.transform.parent = component.transform;
		this.looping_sounds = this.arm_go.AddComponent<LoopingSounds>();
		string sound = GlobalAssets.GetSound(this.rotateSoundName, false);
		this.rotateSound = RuntimeManager.PathToEventReference(sound);
		this.arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.arm_anim_ctrl = this.arm_go.AddComponent<KBatchedAnimController>();
		this.arm_anim_ctrl.AnimFiles = new KAnimFile[]
		{
			component.AnimFiles[0]
		};
		this.arm_anim_ctrl.initialAnim = "arm";
		this.arm_anim_ctrl.isMovable = true;
		this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
		component.SetSymbolVisiblity("arm_target", false);
		bool flag;
		Vector3 position = component.GetSymbolTransform(new HashedString("arm_target"), out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
		this.arm_go.transform.SetPosition(position);
		this.arm_go.SetActive(true);
		this.link = new KAnimLink(component, this.arm_anim_ctrl);
		ChoreGroups choreGroups = Db.Get().ChoreGroups;
		for (int i = 0; i < choreGroups.Count; i++)
		{
			this.choreConsumer.SetPermittedByUser(choreGroups[i], true);
		}
		base.Subscribe<SolidTransferArm>(-592767678, SolidTransferArm.OnOperationalChangedDelegate);
		base.Subscribe<SolidTransferArm>(1745615042, SolidTransferArm.OnEndChoreDelegate);
		this.RotateArm(this.rotatable.GetRotatedOffset(Vector3.up), true, 0f);
		this.DropLeftovers();
		component.enabled = false;
		component.enabled = true;
		MinionGroupProber.Get().SetValidSerialNos(this, this.serial_no, this.serial_no);
		base.smi.StartSM();
	}

	// Token: 0x06002D07 RID: 11527 RVA: 0x000EF085 File Offset: 0x000ED285
	protected override void OnCleanUp()
	{
		MinionGroupProber.Get().ReleaseProber(this);
		base.OnCleanUp();
	}

	// Token: 0x06002D08 RID: 11528 RVA: 0x000EF09C File Offset: 0x000ED29C
	public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms, float time_delta)
	{
		SolidTransferArm.BatchUpdateContext batchUpdateContext = new SolidTransferArm.BatchUpdateContext(solid_transfer_arms);
		if (batchUpdateContext.solid_transfer_arms.Count == 0)
		{
			batchUpdateContext.Finish();
			return;
		}
		SolidTransferArm.cached_pickupables.Clear();
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in Game.Instance.fetchManager.prefabIdToFetchables)
		{
			List<FetchManager.Fetchable> dataList = keyValuePair.Value.fetchables.GetDataList();
			SolidTransferArm.cached_pickupables.Capacity = Math.Max(SolidTransferArm.cached_pickupables.Capacity, SolidTransferArm.cached_pickupables.Count + dataList.Count);
			foreach (FetchManager.Fetchable fetchable in dataList)
			{
				SolidTransferArm.cached_pickupables.Add(new SolidTransferArm.CachedPickupable
				{
					pickupable = fetchable.pickupable,
					storage_cell = fetchable.pickupable.cachedCell
				});
			}
		}
		SolidTransferArm.batch_update_job.Reset(batchUpdateContext);
		int num = Math.Max(1, batchUpdateContext.solid_transfer_arms.Count / CPUBudget.coreCount);
		int num2 = Math.Min(batchUpdateContext.solid_transfer_arms.Count, CPUBudget.coreCount);
		for (int num3 = 0; num3 != num2; num3++)
		{
			int num4 = num3 * num;
			int end = (num3 == num2 - 1) ? batchUpdateContext.solid_transfer_arms.Count : (num4 + num);
			SolidTransferArm.batch_update_job.Add(new SolidTransferArm.BatchUpdateTask(num4, end));
		}
		GlobalJobManager.Run(SolidTransferArm.batch_update_job);
		for (int num5 = 0; num5 != SolidTransferArm.batch_update_job.Count; num5++)
		{
			SolidTransferArm.batch_update_job.GetWorkItem(num5).Finish();
		}
		batchUpdateContext.Finish();
		SolidTransferArm.batch_update_job.Reset(null);
		SolidTransferArm.cached_pickupables.Clear();
	}

	// Token: 0x06002D09 RID: 11529 RVA: 0x000EF298 File Offset: 0x000ED498
	private void Sim()
	{
		Chore.Precondition.Context context = default(Chore.Precondition.Context);
		if (this.choreConsumer.FindNextChore(ref context))
		{
			if (context.chore is FetchChore)
			{
				this.choreDriver.SetChore(context);
				FetchChore chore = context.chore as FetchChore;
				this.storage.DropUnlessMatching(chore);
				this.arm_anim_ctrl.enabled = false;
				this.arm_anim_ctrl.enabled = true;
			}
			else
			{
				bool condition = false;
				string str = "I am but a lowly transfer arm. I should only acquire FetchChores: ";
				Chore chore2 = context.chore;
				global::Debug.Assert(condition, str + ((chore2 != null) ? chore2.ToString() : null));
			}
		}
		this.operational.SetActive(this.choreDriver.HasChore(), false);
	}

	// Token: 0x06002D0A RID: 11530 RVA: 0x000EF340 File Offset: 0x000ED540
	public void Sim1000ms(float dt)
	{
	}

	// Token: 0x06002D0B RID: 11531 RVA: 0x000EF344 File Offset: 0x000ED544
	private void UpdateArmAnim()
	{
		FetchAreaChore fetchAreaChore = this.choreDriver.GetCurrentChore() as FetchAreaChore;
		if (this.worker.workable && fetchAreaChore != null && this.rotation_complete)
		{
			this.StopRotateSound();
			this.SetArmAnim(fetchAreaChore.IsDelivering ? SolidTransferArm.ArmAnim.Drop : SolidTransferArm.ArmAnim.Pickup);
			return;
		}
		this.SetArmAnim(SolidTransferArm.ArmAnim.Idle);
	}

	// Token: 0x06002D0C RID: 11532 RVA: 0x000EF3A0 File Offset: 0x000ED5A0
	private bool AsyncUpdate(int cell, HashSet<int> workspace, GameObject game_object)
	{
		workspace.Clear();
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		for (int i = num2 - this.pickupRange; i < num2 + this.pickupRange + 1; i++)
		{
			for (int j = num - this.pickupRange; j < num + this.pickupRange + 1; j++)
			{
				int num3 = Grid.XYToCell(j, i);
				if (Grid.IsValidCell(num3) && Grid.IsPhysicallyAccessible(num, num2, j, i, true))
				{
					workspace.Add(num3);
				}
			}
		}
		bool flag = !this.reachableCells.SetEquals(workspace);
		if (flag)
		{
			this.reachableCells.Clear();
			this.reachableCells.UnionWith(workspace);
		}
		this.pickupables.Clear();
		foreach (SolidTransferArm.CachedPickupable cachedPickupable in SolidTransferArm.cached_pickupables)
		{
			if (Grid.GetCellRange(cell, cachedPickupable.storage_cell) <= this.pickupRange && this.IsPickupableRelevantToMyInterests(cachedPickupable.pickupable.KPrefabID, cachedPickupable.storage_cell) && cachedPickupable.pickupable.CouldBePickedUpByTransferArm(game_object))
			{
				this.pickupables.Add(cachedPickupable.pickupable);
			}
		}
		return flag;
	}

	// Token: 0x06002D0D RID: 11533 RVA: 0x000EF4EC File Offset: 0x000ED6EC
	private void IncrementSerialNo()
	{
		this.serial_no += 1;
		MinionGroupProber.Get().SetValidSerialNos(this, this.serial_no, this.serial_no);
		MinionGroupProber.Get().Occupy(this, this.serial_no, this.reachableCells);
	}

	// Token: 0x06002D0E RID: 11534 RVA: 0x000EF52B File Offset: 0x000ED72B
	public bool IsCellReachable(int cell)
	{
		return this.reachableCells.Contains(cell);
	}

	// Token: 0x06002D0F RID: 11535 RVA: 0x000EF539 File Offset: 0x000ED739
	private bool IsPickupableRelevantToMyInterests(KPrefabID prefabID, int storage_cell)
	{
		return Assets.IsTagSolidTransferArmConveyable(prefabID.PrefabTag) && this.IsCellReachable(storage_cell);
	}

	// Token: 0x06002D10 RID: 11536 RVA: 0x000EF551 File Offset: 0x000ED751
	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		return FetchManager.FindFetchTarget(this.pickupables, destination, chore);
	}

	// Token: 0x06002D11 RID: 11537 RVA: 0x000EF560 File Offset: 0x000ED760
	public void RenderEveryTick(float dt)
	{
		if (this.worker.workable)
		{
			Vector3 targetPoint = this.worker.workable.GetTargetPoint();
			targetPoint.z = 0f;
			Vector3 position = base.transform.GetPosition();
			position.z = 0f;
			Vector3 target_dir = Vector3.Normalize(targetPoint - position);
			this.RotateArm(target_dir, false, dt);
		}
		this.UpdateArmAnim();
	}

	// Token: 0x06002D12 RID: 11538 RVA: 0x000EF5D0 File Offset: 0x000ED7D0
	private void OnEndChore(object data)
	{
		this.DropLeftovers();
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x000EF5D8 File Offset: 0x000ED7D8
	private void DropLeftovers()
	{
		if (!this.storage.IsEmpty() && !this.choreDriver.HasChore())
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x06002D14 RID: 11540 RVA: 0x000EF618 File Offset: 0x000ED818
	private void SetArmAnim(SolidTransferArm.ArmAnim new_anim)
	{
		if (new_anim == this.arm_anim)
		{
			return;
		}
		this.arm_anim = new_anim;
		switch (this.arm_anim)
		{
		case SolidTransferArm.ArmAnim.Idle:
			this.arm_anim_ctrl.Play("arm", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		case SolidTransferArm.ArmAnim.Pickup:
			this.arm_anim_ctrl.Play("arm_pickup", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		case SolidTransferArm.ArmAnim.Drop:
			this.arm_anim_ctrl.Play("arm_drop", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x000EF6B2 File Offset: 0x000ED8B2
	private void OnOperationalChanged(object data)
	{
		if (!(bool)data)
		{
			if (this.choreDriver.HasChore())
			{
				this.choreDriver.StopChore();
			}
			this.UpdateArmAnim();
		}
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x000EF6DA File Offset: 0x000ED8DA
	private void SetArmRotation(float rot)
	{
		this.arm_rot = rot;
		this.arm_go.transform.rotation = Quaternion.Euler(0f, 0f, this.arm_rot);
	}

	// Token: 0x06002D17 RID: 11543 RVA: 0x000EF708 File Offset: 0x000ED908
	private void RotateArm(Vector3 target_dir, bool warp, float dt)
	{
		float num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
		if (num < -180f)
		{
			num += 360f;
		}
		if (num > 180f)
		{
			num -= 360f;
		}
		if (!warp)
		{
			num = Mathf.Clamp(num, -this.turn_rate * dt, this.turn_rate * dt);
		}
		this.arm_rot += num;
		this.SetArmRotation(this.arm_rot);
		this.rotation_complete = Mathf.Approximately(num, 0f);
		if (!warp && !this.rotation_complete)
		{
			if (!this.rotateSoundPlaying)
			{
				this.StartRotateSound();
			}
			this.SetRotateSoundParameter(this.arm_rot);
			return;
		}
		this.StopRotateSound();
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x000EF7BF File Offset: 0x000ED9BF
	private void StartRotateSound()
	{
		if (!this.rotateSoundPlaying)
		{
			this.looping_sounds.StartSound(this.rotateSound);
			this.rotateSoundPlaying = true;
		}
	}

	// Token: 0x06002D19 RID: 11545 RVA: 0x000EF7E2 File Offset: 0x000ED9E2
	private void SetRotateSoundParameter(float arm_rot)
	{
		if (this.rotateSoundPlaying)
		{
			this.looping_sounds.SetParameter(this.rotateSound, SolidTransferArm.HASH_ROTATION, arm_rot);
		}
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x000EF803 File Offset: 0x000EDA03
	private void StopRotateSound()
	{
		if (this.rotateSoundPlaying)
		{
			this.looping_sounds.StopSound(this.rotateSound);
			this.rotateSoundPlaying = false;
		}
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x000EF825 File Offset: 0x000EDA25
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x06002D1C RID: 11548 RVA: 0x000EF827 File Offset: 0x000EDA27
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x06002D1D RID: 11549 RVA: 0x000EF829 File Offset: 0x000EDA29
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x06002D1E RID: 11550 RVA: 0x000EF82B File Offset: 0x000EDA2B
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x04001A84 RID: 6788
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001A85 RID: 6789
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001A86 RID: 6790
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001A87 RID: 6791
	[MyCmpAdd]
	private Worker worker;

	// Token: 0x04001A88 RID: 6792
	[MyCmpAdd]
	private ChoreConsumer choreConsumer;

	// Token: 0x04001A89 RID: 6793
	[MyCmpAdd]
	private ChoreDriver choreDriver;

	// Token: 0x04001A8A RID: 6794
	public int pickupRange = 4;

	// Token: 0x04001A8B RID: 6795
	private float max_carry_weight = 1000f;

	// Token: 0x04001A8C RID: 6796
	private List<Pickupable> pickupables = new List<Pickupable>();

	// Token: 0x04001A8D RID: 6797
	private KBatchedAnimController arm_anim_ctrl;

	// Token: 0x04001A8E RID: 6798
	private GameObject arm_go;

	// Token: 0x04001A8F RID: 6799
	private LoopingSounds looping_sounds;

	// Token: 0x04001A90 RID: 6800
	private bool rotateSoundPlaying;

	// Token: 0x04001A91 RID: 6801
	private string rotateSoundName = "TransferArm_rotate";

	// Token: 0x04001A92 RID: 6802
	private EventReference rotateSound;

	// Token: 0x04001A93 RID: 6803
	private KAnimLink link;

	// Token: 0x04001A94 RID: 6804
	private float arm_rot = 45f;

	// Token: 0x04001A95 RID: 6805
	private float turn_rate = 360f;

	// Token: 0x04001A96 RID: 6806
	private bool rotation_complete;

	// Token: 0x04001A97 RID: 6807
	private SolidTransferArm.ArmAnim arm_anim;

	// Token: 0x04001A98 RID: 6808
	private HashSet<int> reachableCells = new HashSet<int>();

	// Token: 0x04001A99 RID: 6809
	private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001A9A RID: 6810
	private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnEndChoreDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data)
	{
		component.OnEndChore(data);
	});

	// Token: 0x04001A9B RID: 6811
	private static WorkItemCollection<SolidTransferArm.BatchUpdateTask, SolidTransferArm.BatchUpdateContext> batch_update_job = new WorkItemCollection<SolidTransferArm.BatchUpdateTask, SolidTransferArm.BatchUpdateContext>();

	// Token: 0x04001A9C RID: 6812
	private static List<SolidTransferArm.CachedPickupable> cached_pickupables = new List<SolidTransferArm.CachedPickupable>();

	// Token: 0x04001A9D RID: 6813
	private short serial_no;

	// Token: 0x04001A9E RID: 6814
	private static HashedString HASH_ROTATION = "rotation";

	// Token: 0x020013AC RID: 5036
	private enum ArmAnim
	{
		// Token: 0x04006312 RID: 25362
		Idle,
		// Token: 0x04006313 RID: 25363
		Pickup,
		// Token: 0x04006314 RID: 25364
		Drop
	}

	// Token: 0x020013AD RID: 5037
	public class SMInstance : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.GameInstance
	{
		// Token: 0x060081E7 RID: 33255 RVA: 0x002F6DCF File Offset: 0x002F4FCF
		public SMInstance(SolidTransferArm master) : base(master)
		{
		}
	}

	// Token: 0x020013AE RID: 5038
	public class States : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm>
	{
		// Token: 0x060081E8 RID: 33256 RVA: 0x002F6DD8 File Offset: 0x002F4FD8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidTransferArm.SMInstance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidTransferArm.SMInstance smi)
			{
				smi.master.StopRotateSound();
			});
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidTransferArm.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (SolidTransferArm.SMInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.working.PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (SolidTransferArm.SMInstance smi) => !smi.GetComponent<Operational>().IsActive);
		}

		// Token: 0x04006315 RID: 25365
		public StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.BoolParameter transferring;

		// Token: 0x04006316 RID: 25366
		public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State off;

		// Token: 0x04006317 RID: 25367
		public SolidTransferArm.States.ReadyStates on;

		// Token: 0x02002127 RID: 8487
		public class ReadyStates : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State
		{
			// Token: 0x04009493 RID: 38035
			public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State idle;

			// Token: 0x04009494 RID: 38036
			public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State working;
		}
	}

	// Token: 0x020013AF RID: 5039
	private class BatchUpdateContext
	{
		// Token: 0x060081EA RID: 33258 RVA: 0x002F6F40 File Offset: 0x002F5140
		public BatchUpdateContext(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms)
		{
			this.solid_transfer_arms = ListPool<SolidTransferArm, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.solid_transfer_arms.Capacity = solid_transfer_arms.Count;
			this.refreshed_reachable_cells = ListPool<bool, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.refreshed_reachable_cells.Capacity = solid_transfer_arms.Count;
			this.cells = ListPool<int, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.cells.Capacity = solid_transfer_arms.Count;
			this.game_objects = ListPool<GameObject, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.game_objects.Capacity = solid_transfer_arms.Count;
			for (int num = 0; num != solid_transfer_arms.Count; num++)
			{
				UpdateBucketWithUpdater<ISim1000ms>.Entry entry = solid_transfer_arms[num];
				entry.lastUpdateTime = 0f;
				solid_transfer_arms[num] = entry;
				SolidTransferArm solidTransferArm = (SolidTransferArm)entry.data;
				if (solidTransferArm.operational.IsOperational)
				{
					this.solid_transfer_arms.Add(solidTransferArm);
					this.refreshed_reachable_cells.Add(false);
					this.cells.Add(Grid.PosToCell(solidTransferArm));
					this.game_objects.Add(solidTransferArm.gameObject);
				}
			}
		}

		// Token: 0x060081EB RID: 33259 RVA: 0x002F7044 File Offset: 0x002F5244
		public void Finish()
		{
			for (int num = 0; num != this.solid_transfer_arms.Count; num++)
			{
				if (this.refreshed_reachable_cells[num])
				{
					this.solid_transfer_arms[num].IncrementSerialNo();
				}
				this.solid_transfer_arms[num].Sim();
			}
			this.refreshed_reachable_cells.Recycle();
			this.cells.Recycle();
			this.game_objects.Recycle();
			this.solid_transfer_arms.Recycle();
		}

		// Token: 0x04006318 RID: 25368
		public ListPool<SolidTransferArm, SolidTransferArm.BatchUpdateContext>.PooledList solid_transfer_arms;

		// Token: 0x04006319 RID: 25369
		public ListPool<bool, SolidTransferArm.BatchUpdateContext>.PooledList refreshed_reachable_cells;

		// Token: 0x0400631A RID: 25370
		public ListPool<int, SolidTransferArm.BatchUpdateContext>.PooledList cells;

		// Token: 0x0400631B RID: 25371
		public ListPool<GameObject, SolidTransferArm.BatchUpdateContext>.PooledList game_objects;
	}

	// Token: 0x020013B0 RID: 5040
	private struct BatchUpdateTask : IWorkItem<SolidTransferArm.BatchUpdateContext>
	{
		// Token: 0x060081EC RID: 33260 RVA: 0x002F70C3 File Offset: 0x002F52C3
		public BatchUpdateTask(int start, int end)
		{
			this.start = start;
			this.end = end;
			this.reachable_cells_workspace = HashSetPool<int, SolidTransferArm>.Allocate();
		}

		// Token: 0x060081ED RID: 33261 RVA: 0x002F70E0 File Offset: 0x002F52E0
		public void Run(SolidTransferArm.BatchUpdateContext context)
		{
			for (int num = this.start; num != this.end; num++)
			{
				context.refreshed_reachable_cells[num] = context.solid_transfer_arms[num].AsyncUpdate(context.cells[num], this.reachable_cells_workspace, context.game_objects[num]);
			}
		}

		// Token: 0x060081EE RID: 33262 RVA: 0x002F713E File Offset: 0x002F533E
		public void Finish()
		{
			this.reachable_cells_workspace.Recycle();
		}

		// Token: 0x0400631C RID: 25372
		private int start;

		// Token: 0x0400631D RID: 25373
		private int end;

		// Token: 0x0400631E RID: 25374
		private HashSetPool<int, SolidTransferArm>.PooledHashSet reachable_cells_workspace;
	}

	// Token: 0x020013B1 RID: 5041
	public struct CachedPickupable
	{
		// Token: 0x0400631F RID: 25375
		public Pickupable pickupable;

		// Token: 0x04006320 RID: 25376
		public int storage_cell;
	}
}
