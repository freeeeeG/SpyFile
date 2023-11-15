using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000627 RID: 1575
public class LogicBroadcastReceiver : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x060027E2 RID: 10210 RVA: 0x000D8C74 File Offset: 0x000D6E74
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		this.SetChannel(this.channel.Get());
		this.operational.SetFlag(LogicBroadcastReceiver.spaceVisible, this.IsSpaceVisible());
		this.operational.SetFlag(LogicBroadcastReceiver.validChannelInRange, this.CheckChannelValid() && LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject));
		this.wasOperational = !this.operational.IsOperational;
		this.OnOperationalChanged(null);
	}

	// Token: 0x060027E3 RID: 10211 RVA: 0x000D8D18 File Offset: 0x000D6F18
	public void SimEveryTick(float dt)
	{
		bool flag = this.IsSpaceVisible();
		this.operational.SetFlag(LogicBroadcastReceiver.spaceVisible, flag);
		if (!flag)
		{
			if (this.spaceNotVisibleStatusItem == Guid.Empty)
			{
				this.spaceNotVisibleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, null);
			}
		}
		else if (this.spaceNotVisibleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.spaceNotVisibleStatusItem, false);
			this.spaceNotVisibleStatusItem = Guid.Empty;
		}
		bool flag2 = this.CheckChannelValid() && LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject);
		this.operational.SetFlag(LogicBroadcastReceiver.validChannelInRange, flag2);
		if (flag2 && !this.syncToChannelComplete)
		{
			this.SyncWithBroadcast();
		}
	}

	// Token: 0x060027E4 RID: 10212 RVA: 0x000D8DEE File Offset: 0x000D6FEE
	public bool IsSpaceVisible()
	{
		return base.gameObject.GetMyWorld().IsModuleInterior || Grid.ExposedToSunlight[Grid.PosToCell(base.gameObject)] > 0;
	}

	// Token: 0x060027E5 RID: 10213 RVA: 0x000D8E1C File Offset: 0x000D701C
	private bool CheckChannelValid()
	{
		return this.channel.Get() != null && this.channel.Get().GetComponent<LogicPorts>().inputPorts != null;
	}

	// Token: 0x060027E6 RID: 10214 RVA: 0x000D8E4B File Offset: 0x000D704B
	public LogicBroadcaster GetChannel()
	{
		return this.channel.Get();
	}

	// Token: 0x060027E7 RID: 10215 RVA: 0x000D8E58 File Offset: 0x000D7058
	public void SetChannel(LogicBroadcaster broadcaster)
	{
		this.ClearChannel();
		if (broadcaster == null)
		{
			return;
		}
		this.channel.Set(broadcaster);
		this.syncToChannelComplete = false;
		this.channelEventListeners.Add(this.channel.Get().gameObject.Subscribe(-801688580, new Action<object>(this.OnChannelLogicEvent)));
		this.channelEventListeners.Add(this.channel.Get().gameObject.Subscribe(-592767678, new Action<object>(this.OnChannelLogicEvent)));
		this.SyncWithBroadcast();
	}

	// Token: 0x060027E8 RID: 10216 RVA: 0x000D8EF0 File Offset: 0x000D70F0
	private void ClearChannel()
	{
		if (this.CheckChannelValid())
		{
			for (int i = 0; i < this.channelEventListeners.Count; i++)
			{
				this.channel.Get().gameObject.Unsubscribe(this.channelEventListeners[i]);
			}
		}
		this.channelEventListeners.Clear();
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x000D8F47 File Offset: 0x000D7147
	private void OnChannelLogicEvent(object data)
	{
		if (!this.channel.Get().GetComponent<Operational>().IsOperational)
		{
			return;
		}
		this.SyncWithBroadcast();
	}

	// Token: 0x060027EA RID: 10218 RVA: 0x000D8F68 File Offset: 0x000D7168
	private void SyncWithBroadcast()
	{
		if (!this.CheckChannelValid())
		{
			return;
		}
		bool flag = LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject);
		this.UpdateRangeStatus(flag);
		if (!flag)
		{
			return;
		}
		base.GetComponent<LogicPorts>().SendSignal(this.PORT_ID, this.channel.Get().GetCurrentValue());
		this.syncToChannelComplete = true;
	}

	// Token: 0x060027EB RID: 10219 RVA: 0x000D8FD2 File Offset: 0x000D71D2
	public static bool CheckRange(GameObject broadcaster, GameObject receiver)
	{
		return AxialUtil.GetDistance(broadcaster.GetMyWorldLocation(), receiver.GetMyWorldLocation()) <= LogicBroadcaster.RANGE;
	}

	// Token: 0x060027EC RID: 10220 RVA: 0x000D8FF0 File Offset: 0x000D71F0
	private void UpdateRangeStatus(bool inRange)
	{
		if (!inRange && this.rangeStatusItem == Guid.Empty)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.rangeStatusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.BroadcasterOutOfRange, null);
			return;
		}
		if (this.rangeStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.rangeStatusItem, false);
			this.rangeStatusItem = Guid.Empty;
		}
	}

	// Token: 0x060027ED RID: 10221 RVA: 0x000D9068 File Offset: 0x000D7268
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

	// Token: 0x060027EE RID: 10222 RVA: 0x000D9124 File Offset: 0x000D7324
	protected override void OnCleanUp()
	{
		this.ClearChannel();
		base.OnCleanUp();
	}

	// Token: 0x0400172B RID: 5931
	[Serialize]
	private Ref<LogicBroadcaster> channel = new Ref<LogicBroadcaster>();

	// Token: 0x0400172C RID: 5932
	public string PORT_ID = "";

	// Token: 0x0400172D RID: 5933
	private List<int> channelEventListeners = new List<int>();

	// Token: 0x0400172E RID: 5934
	private bool syncToChannelComplete;

	// Token: 0x0400172F RID: 5935
	public static readonly Operational.Flag spaceVisible = new Operational.Flag("spaceVisible", Operational.Flag.Type.Requirement);

	// Token: 0x04001730 RID: 5936
	public static readonly Operational.Flag validChannelInRange = new Operational.Flag("validChannelInRange", Operational.Flag.Type.Requirement);

	// Token: 0x04001731 RID: 5937
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001732 RID: 5938
	private bool wasOperational;

	// Token: 0x04001733 RID: 5939
	[MyCmpGet]
	private KBatchedAnimController animController;

	// Token: 0x04001734 RID: 5940
	private Guid rangeStatusItem = Guid.Empty;

	// Token: 0x04001735 RID: 5941
	private Guid spaceNotVisibleStatusItem = Guid.Empty;
}
