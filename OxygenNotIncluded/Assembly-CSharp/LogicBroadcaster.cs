using System;
using KSerialization;

// Token: 0x02000628 RID: 1576
public class LogicBroadcaster : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x1700021C RID: 540
	// (get) Token: 0x060027F1 RID: 10225 RVA: 0x000D9193 File Offset: 0x000D7393
	// (set) Token: 0x060027F2 RID: 10226 RVA: 0x000D919B File Offset: 0x000D739B
	public int BroadCastChannelID
	{
		get
		{
			return this.broadcastChannelID;
		}
		private set
		{
			this.broadcastChannelID = value;
		}
	}

	// Token: 0x060027F3 RID: 10227 RVA: 0x000D91A4 File Offset: 0x000D73A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.LogicBroadcasters.Add(this);
	}

	// Token: 0x060027F4 RID: 10228 RVA: 0x000D91B7 File Offset: 0x000D73B7
	protected override void OnCleanUp()
	{
		Components.LogicBroadcasters.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060027F5 RID: 10229 RVA: 0x000D91CC File Offset: 0x000D73CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicBroadcaster>(-801688580, LogicBroadcaster.OnLogicValueChangedDelegate);
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		this.operational.SetFlag(LogicBroadcaster.spaceVisible, this.IsSpaceVisible());
		this.wasOperational = !this.operational.IsOperational;
		this.OnOperationalChanged(null);
	}

	// Token: 0x060027F6 RID: 10230 RVA: 0x000D9239 File Offset: 0x000D7439
	public bool IsSpaceVisible()
	{
		return base.gameObject.GetMyWorld().IsModuleInterior || Grid.ExposedToSunlight[Grid.PosToCell(base.gameObject)] > 0;
	}

	// Token: 0x060027F7 RID: 10231 RVA: 0x000D9267 File Offset: 0x000D7467
	public int GetCurrentValue()
	{
		return base.GetComponent<LogicPorts>().GetInputValue(this.PORT_ID);
	}

	// Token: 0x060027F8 RID: 10232 RVA: 0x000D927F File Offset: 0x000D747F
	private void OnLogicValueChanged(object data)
	{
	}

	// Token: 0x060027F9 RID: 10233 RVA: 0x000D9284 File Offset: 0x000D7484
	public void SimEveryTick(float dt)
	{
		bool flag = this.IsSpaceVisible();
		this.operational.SetFlag(LogicBroadcaster.spaceVisible, flag);
		if (!flag)
		{
			if (this.spaceNotVisibleStatusItem == Guid.Empty)
			{
				this.spaceNotVisibleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, null);
				return;
			}
		}
		else if (this.spaceNotVisibleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.spaceNotVisibleStatusItem, false);
			this.spaceNotVisibleStatusItem = Guid.Empty;
		}
	}

	// Token: 0x060027FA RID: 10234 RVA: 0x000D9310 File Offset: 0x000D7510
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			if (!this.wasOperational)
			{
				this.wasOperational = true;
				this.animController.Queue("on_pre", KAnim.PlayMode.Once, 1f, 0f);
				this.animController.Queue("on", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		else if (this.wasOperational)
		{
			this.wasOperational = false;
			this.animController.Queue("on_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.animController.Queue("off", KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x04001736 RID: 5942
	public static int RANGE = 5;

	// Token: 0x04001737 RID: 5943
	private static int INVALID_CHANNEL_ID = -1;

	// Token: 0x04001738 RID: 5944
	public string PORT_ID = "";

	// Token: 0x04001739 RID: 5945
	private bool wasOperational;

	// Token: 0x0400173A RID: 5946
	[Serialize]
	private int broadcastChannelID = LogicBroadcaster.INVALID_CHANNEL_ID;

	// Token: 0x0400173B RID: 5947
	private static readonly EventSystem.IntraObjectHandler<LogicBroadcaster> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicBroadcaster>(delegate(LogicBroadcaster component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x0400173C RID: 5948
	public static readonly Operational.Flag spaceVisible = new Operational.Flag("spaceVisible", Operational.Flag.Type.Requirement);

	// Token: 0x0400173D RID: 5949
	private Guid spaceNotVisibleStatusItem = Guid.Empty;

	// Token: 0x0400173E RID: 5950
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400173F RID: 5951
	[MyCmpGet]
	private KBatchedAnimController animController;
}
