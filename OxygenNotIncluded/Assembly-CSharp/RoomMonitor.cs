using System;

// Token: 0x02000890 RID: 2192
public class RoomMonitor : GameStateMachine<RoomMonitor, RoomMonitor.Instance>
{
	// Token: 0x06003FC2 RID: 16322 RVA: 0x001648A1 File Offset: 0x00162AA1
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.PathAdvanced, new StateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.State.Callback(RoomMonitor.UpdateRoomType));
	}

	// Token: 0x06003FC3 RID: 16323 RVA: 0x001648C8 File Offset: 0x00162AC8
	private static void UpdateRoomType(RoomMonitor.Instance smi)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(smi.master.gameObject);
		if (roomOfGameObject != smi.currentRoom)
		{
			smi.currentRoom = roomOfGameObject;
			if (roomOfGameObject != null)
			{
				roomOfGameObject.cavity.OnEnter(smi.master.gameObject);
			}
		}
	}

	// Token: 0x020016AB RID: 5803
	public new class Instance : GameStateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008BD2 RID: 35794 RVA: 0x00317244 File Offset: 0x00315444
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x04006C71 RID: 27761
		public Room currentRoom;
	}
}
