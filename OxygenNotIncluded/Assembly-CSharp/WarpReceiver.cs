using System;
using System.Linq;
using KSerialization;

// Token: 0x020006B9 RID: 1721
public class WarpReceiver : Workable
{
	// Token: 0x06002EE0 RID: 12000 RVA: 0x000F7928 File Offset: 0x000F5B28
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x000F7930 File Offset: 0x000F5B30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.warpReceiverSMI = new WarpReceiver.WarpReceiverSM.Instance(this);
		this.warpReceiverSMI.StartSM();
		Components.WarpReceivers.Add(this);
	}

	// Token: 0x06002EE2 RID: 12002 RVA: 0x000F795C File Offset: 0x000F5B5C
	public void ReceiveWarpedDuplicant(Worker dupe)
	{
		dupe.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(this), CellAlignment.Bottom, Grid.SceneLayer.Move));
		Debug.Assert(this.chore == null);
		KAnimFile anim = Assets.GetAnim("anim_interacts_warp_portal_receiver_kanim");
		ChoreType migrate = Db.Get().ChoreTypes.Migrate;
		KAnimFile override_anims = anim;
		this.chore = new WorkChore<Workable>(migrate, this, dupe.GetComponent<ChoreProvider>(), true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, true, true, override_anims, false, true, false, PriorityScreen.PriorityClass.compulsory, 5, false, true);
		Workable component = base.GetComponent<Workable>();
		component.workLayer = Grid.SceneLayer.Building;
		component.workAnims = new HashedString[]
		{
			"printing_pre",
			"printing_loop"
		};
		component.workingPstComplete = new HashedString[]
		{
			"printing_pst"
		};
		component.workingPstFailed = new HashedString[]
		{
			"printing_pst"
		};
		component.synchronizeAnims = true;
		float num = 0f;
		KAnimFileData data = anim.GetData();
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim2 = data.GetAnim(i);
			if (component.workAnims.Contains(anim2.hash))
			{
				num += anim2.totalTime;
			}
		}
		component.SetWorkTime(num);
		this.Used = true;
	}

	// Token: 0x06002EE3 RID: 12003 RVA: 0x000F7AB7 File Offset: 0x000F5CB7
	private void CompleteChore()
	{
		this.chore.Cleanup();
		this.chore = null;
		this.warpReceiverSMI.GoTo(this.warpReceiverSMI.sm.idle);
	}

	// Token: 0x06002EE4 RID: 12004 RVA: 0x000F7AE6 File Offset: 0x000F5CE6
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.WarpReceivers.Remove(this);
	}

	// Token: 0x04001BC8 RID: 7112
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001BC9 RID: 7113
	private WarpReceiver.WarpReceiverSM.Instance warpReceiverSMI;

	// Token: 0x04001BCA RID: 7114
	private Notification notification;

	// Token: 0x04001BCB RID: 7115
	[Serialize]
	public bool IsConsumed;

	// Token: 0x04001BCC RID: 7116
	private Chore chore;

	// Token: 0x04001BCD RID: 7117
	[Serialize]
	public bool Used;

	// Token: 0x020013FA RID: 5114
	public class WarpReceiverSM : GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver>
	{
		// Token: 0x06008303 RID: 33539 RVA: 0x002FC259 File Offset: 0x002FA459
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("idle");
		}

		// Token: 0x040063FD RID: 25597
		public GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver, object>.State idle;

		// Token: 0x02002149 RID: 8521
		public new class Instance : GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver, object>.GameInstance
		{
			// Token: 0x0600A9BA RID: 43450 RVA: 0x003724D6 File Offset: 0x003706D6
			public Instance(WarpReceiver master) : base(master)
			{
			}
		}
	}
}
