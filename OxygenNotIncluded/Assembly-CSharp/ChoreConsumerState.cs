using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020003D4 RID: 980
public class ChoreConsumerState
{
	// Token: 0x06001469 RID: 5225 RVA: 0x0006C018 File Offset: 0x0006A218
	public ChoreConsumerState(ChoreConsumer consumer)
	{
		this.consumer = consumer;
		this.navigator = consumer.GetComponent<Navigator>();
		this.prefabid = consumer.GetComponent<KPrefabID>();
		this.ownable = consumer.GetComponent<Ownable>();
		this.gameObject = consumer.gameObject;
		this.solidTransferArm = consumer.GetComponent<SolidTransferArm>();
		this.hasSolidTransferArm = (this.solidTransferArm != null);
		this.resume = consumer.GetComponent<MinionResume>();
		this.choreDriver = consumer.GetComponent<ChoreDriver>();
		this.schedulable = consumer.GetComponent<Schedulable>();
		this.traits = consumer.GetComponent<Traits>();
		this.choreProvider = consumer.GetComponent<ChoreProvider>();
		MinionIdentity component = consumer.GetComponent<MinionIdentity>();
		if (component != null)
		{
			if (component.assignableProxy == null)
			{
				component.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(component.assignableProxy, component);
			}
			this.assignables = component.GetSoleOwner();
			this.equipment = component.GetEquipment();
		}
		else
		{
			this.assignables = consumer.GetComponent<Assignables>();
			this.equipment = consumer.GetComponent<Equipment>();
		}
		this.storage = consumer.GetComponent<Storage>();
		this.consumableConsumer = consumer.GetComponent<ConsumableConsumer>();
		this.worker = consumer.GetComponent<Worker>();
		this.selectable = consumer.GetComponent<KSelectable>();
		if (this.schedulable != null)
		{
			int blockIdx = Schedule.GetBlockIdx();
			this.scheduleBlock = this.schedulable.GetSchedule().GetBlock(blockIdx);
		}
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x0006C174 File Offset: 0x0006A374
	public void Refresh()
	{
		if (this.schedulable != null)
		{
			int blockIdx = Schedule.GetBlockIdx();
			Schedule schedule = this.schedulable.GetSchedule();
			if (schedule != null)
			{
				this.scheduleBlock = schedule.GetBlock(blockIdx);
			}
		}
	}

	// Token: 0x04000AFA RID: 2810
	public KPrefabID prefabid;

	// Token: 0x04000AFB RID: 2811
	public GameObject gameObject;

	// Token: 0x04000AFC RID: 2812
	public ChoreConsumer consumer;

	// Token: 0x04000AFD RID: 2813
	public ChoreProvider choreProvider;

	// Token: 0x04000AFE RID: 2814
	public Navigator navigator;

	// Token: 0x04000AFF RID: 2815
	public Ownable ownable;

	// Token: 0x04000B00 RID: 2816
	public Assignables assignables;

	// Token: 0x04000B01 RID: 2817
	public MinionResume resume;

	// Token: 0x04000B02 RID: 2818
	public ChoreDriver choreDriver;

	// Token: 0x04000B03 RID: 2819
	public Schedulable schedulable;

	// Token: 0x04000B04 RID: 2820
	public Traits traits;

	// Token: 0x04000B05 RID: 2821
	public Equipment equipment;

	// Token: 0x04000B06 RID: 2822
	public Storage storage;

	// Token: 0x04000B07 RID: 2823
	public ConsumableConsumer consumableConsumer;

	// Token: 0x04000B08 RID: 2824
	public KSelectable selectable;

	// Token: 0x04000B09 RID: 2825
	public Worker worker;

	// Token: 0x04000B0A RID: 2826
	public SolidTransferArm solidTransferArm;

	// Token: 0x04000B0B RID: 2827
	public bool hasSolidTransferArm;

	// Token: 0x04000B0C RID: 2828
	public ScheduleBlock scheduleBlock;
}
