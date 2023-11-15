using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000662 RID: 1634
public class ObjectDispenser : Switch, IUserControlledCapacity
{
	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06002B21 RID: 11041 RVA: 0x000E61DD File Offset: 0x000E43DD
	// (set) Token: 0x06002B22 RID: 11042 RVA: 0x000E61F5 File Offset: 0x000E43F5
	public virtual float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, base.GetComponent<Storage>().capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06002B23 RID: 11043 RVA: 0x000E6209 File Offset: 0x000E4409
	public float AmountStored
	{
		get
		{
			return base.GetComponent<Storage>().MassStored();
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06002B24 RID: 11044 RVA: 0x000E6216 File Offset: 0x000E4416
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06002B25 RID: 11045 RVA: 0x000E621D File Offset: 0x000E441D
	public float MaxCapacity
	{
		get
		{
			return base.GetComponent<Storage>().capacityKg;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06002B26 RID: 11046 RVA: 0x000E622A File Offset: 0x000E442A
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06002B27 RID: 11047 RVA: 0x000E622D File Offset: 0x000E442D
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x000E6235 File Offset: 0x000E4435
	protected override void OnPrefabInit()
	{
		this.Initialize();
	}

	// Token: 0x06002B29 RID: 11049 RVA: 0x000E6240 File Offset: 0x000E4440
	protected void Initialize()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("ObjectDispenser", 35);
		this.filteredStorage = new FilteredStorage(this, null, this, false, Db.Get().ChoreTypes.StorageFetch);
		base.Subscribe<ObjectDispenser>(-905833192, ObjectDispenser.OnCopySettingsDelegate);
	}

	// Token: 0x06002B2A RID: 11050 RVA: 0x000E6294 File Offset: 0x000E4494
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new ObjectDispenser.Instance(this, base.IsSwitchedOn);
		this.smi.StartSM();
		if (ObjectDispenser.infoStatusItem == null)
		{
			ObjectDispenser.infoStatusItem = new StatusItem("ObjectDispenserAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ObjectDispenser.infoStatusItem.resolveStringCallback = new Func<string, object, string>(ObjectDispenser.ResolveInfoStatusItemString);
		}
		this.filteredStorage.FilterChanged();
		base.GetComponent<KSelectable>().ToggleStatusItem(ObjectDispenser.infoStatusItem, true, this.smi);
	}

	// Token: 0x06002B2B RID: 11051 RVA: 0x000E632C File Offset: 0x000E452C
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
		base.OnCleanUp();
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x000E6340 File Offset: 0x000E4540
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		ObjectDispenser component = gameObject.GetComponent<ObjectDispenser>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x000E637C File Offset: 0x000E457C
	public void DropHeldItems()
	{
		while (this.storage.Count > 0)
		{
			GameObject gameObject = this.storage.Drop(this.storage.items[0], true);
			if (this.rotatable != null)
			{
				gameObject.transform.SetPosition(base.transform.GetPosition() + this.rotatable.GetRotatedCellOffset(this.dropOffset).ToVector3());
			}
			else
			{
				gameObject.transform.SetPosition(base.transform.GetPosition() + this.dropOffset.ToVector3());
			}
		}
		this.smi.GetMaster().GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x000E644B File Offset: 0x000E464B
	protected override void Toggle()
	{
		base.Toggle();
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x000E6453 File Offset: 0x000E4653
	protected override void OnRefreshUserMenu(object data)
	{
		if (!this.smi.IsAutomated())
		{
			base.OnRefreshUserMenu(data);
		}
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x000E646C File Offset: 0x000E466C
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		ObjectDispenser.Instance instance = (ObjectDispenser.Instance)data;
		string format = instance.IsAutomated() ? BUILDING.STATUSITEMS.OBJECTDISPENSER.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.OBJECTDISPENSER.MANUAL_CONTROL;
		string arg = instance.IsOpened ? BUILDING.STATUSITEMS.OBJECTDISPENSER.OPENED : BUILDING.STATUSITEMS.OBJECTDISPENSER.CLOSED;
		return string.Format(format, arg);
	}

	// Token: 0x04001940 RID: 6464
	public static readonly HashedString PORT_ID = "ObjectDispenser";

	// Token: 0x04001941 RID: 6465
	private LoggerFS log;

	// Token: 0x04001942 RID: 6466
	public CellOffset dropOffset;

	// Token: 0x04001943 RID: 6467
	[MyCmpReq]
	private Building building;

	// Token: 0x04001944 RID: 6468
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001945 RID: 6469
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001946 RID: 6470
	private ObjectDispenser.Instance smi;

	// Token: 0x04001947 RID: 6471
	private static StatusItem infoStatusItem;

	// Token: 0x04001948 RID: 6472
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04001949 RID: 6473
	protected FilteredStorage filteredStorage;

	// Token: 0x0400194A RID: 6474
	private static readonly EventSystem.IntraObjectHandler<ObjectDispenser> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ObjectDispenser>(delegate(ObjectDispenser component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200134D RID: 4941
	public class States : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser>
	{
		// Token: 0x0600809A RID: 32922 RVA: 0x002F1B40 File Offset: 0x002EFD40
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.idle.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, delegate(ObjectDispenser.Instance smi)
			{
				smi.UpdateState();
			}).ParamTransition<bool>(this.should_open, this.drop_item, (ObjectDispenser.Instance smi, bool p) => p && !smi.master.GetComponent<Storage>().IsEmpty());
			this.load_item.PlayAnim("working_load").OnAnimQueueComplete(this.load_item_pst);
			this.load_item_pst.ParamTransition<bool>(this.should_open, this.idle, (ObjectDispenser.Instance smi, bool p) => !p).ParamTransition<bool>(this.should_open, this.drop_item, (ObjectDispenser.Instance smi, bool p) => p);
			this.drop_item.PlayAnim("working_dispense").OnAnimQueueComplete(this.idle).Exit(delegate(ObjectDispenser.Instance smi)
			{
				smi.master.DropHeldItems();
			});
		}

		// Token: 0x0400622D RID: 25133
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item;

		// Token: 0x0400622E RID: 25134
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item_pst;

		// Token: 0x0400622F RID: 25135
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State drop_item;

		// Token: 0x04006230 RID: 25136
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State idle;

		// Token: 0x04006231 RID: 25137
		public StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.BoolParameter should_open;
	}

	// Token: 0x0200134E RID: 4942
	public class Instance : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.GameInstance
	{
		// Token: 0x0600809C RID: 32924 RVA: 0x002F1C94 File Offset: 0x002EFE94
		public Instance(ObjectDispenser master, bool manual_start_state) : base(master)
		{
			this.manual_on = manual_start_state;
			this.operational = base.GetComponent<Operational>();
			this.logic = base.GetComponent<LogicPorts>();
			base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.smi.sm.should_open.Set(true, base.smi, false);
		}

		// Token: 0x0600809D RID: 32925 RVA: 0x002F1D1A File Offset: 0x002EFF1A
		public void UpdateState()
		{
			base.smi.GoTo(base.sm.load_item);
		}

		// Token: 0x0600809E RID: 32926 RVA: 0x002F1D32 File Offset: 0x002EFF32
		public bool IsAutomated()
		{
			return this.logic.IsPortConnected(ObjectDispenser.PORT_ID);
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x0600809F RID: 32927 RVA: 0x002F1D44 File Offset: 0x002EFF44
		public bool IsOpened
		{
			get
			{
				if (!this.IsAutomated())
				{
					return this.manual_on;
				}
				return this.logic_on;
			}
		}

		// Token: 0x060080A0 RID: 32928 RVA: 0x002F1D5B File Offset: 0x002EFF5B
		public void SetSwitchState(bool on)
		{
			this.manual_on = on;
			this.UpdateShouldOpen();
		}

		// Token: 0x060080A1 RID: 32929 RVA: 0x002F1D6A File Offset: 0x002EFF6A
		public void SetActive(bool active)
		{
			this.operational.SetActive(active, false);
		}

		// Token: 0x060080A2 RID: 32930 RVA: 0x002F1D79 File Offset: 0x002EFF79
		private void OnOperationalChanged(object data)
		{
			this.UpdateShouldOpen();
		}

		// Token: 0x060080A3 RID: 32931 RVA: 0x002F1D84 File Offset: 0x002EFF84
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != ObjectDispenser.PORT_ID)
			{
				return;
			}
			this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.UpdateShouldOpen();
		}

		// Token: 0x060080A4 RID: 32932 RVA: 0x002F1DC4 File Offset: 0x002EFFC4
		private void UpdateShouldOpen()
		{
			this.SetActive(this.operational.IsOperational);
			if (!this.operational.IsOperational)
			{
				return;
			}
			if (this.IsAutomated())
			{
				base.smi.sm.should_open.Set(this.logic_on, base.smi, false);
				return;
			}
			base.smi.sm.should_open.Set(this.manual_on, base.smi, false);
		}

		// Token: 0x04006232 RID: 25138
		private Operational operational;

		// Token: 0x04006233 RID: 25139
		public LogicPorts logic;

		// Token: 0x04006234 RID: 25140
		public bool logic_on = true;

		// Token: 0x04006235 RID: 25141
		private bool manual_on;
	}
}
