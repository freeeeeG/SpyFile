using System;
using KSerialization;

// Token: 0x02000666 RID: 1638
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalValve : ValveBase
{
	// Token: 0x06002B5B RID: 11099 RVA: 0x000E6C33 File Offset: 0x000E4E33
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate);
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x000E6C4C File Offset: 0x000E4E4C
	protected override void OnSpawn()
	{
		this.OnOperationalChanged(this.operational.IsOperational);
		base.OnSpawn();
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x000E6C6A File Offset: 0x000E4E6A
	protected override void OnCleanUp()
	{
		base.Unsubscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x000E6C84 File Offset: 0x000E4E84
	private void OnOperationalChanged(object data)
	{
		bool flag = (bool)data;
		if (flag)
		{
			base.CurrentFlow = base.MaxFlow;
		}
		else
		{
			base.CurrentFlow = 0f;
		}
		this.operational.SetActive(flag, false);
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x000E6CC1 File Offset: 0x000E4EC1
	protected override void OnMassTransfer(float amount)
	{
		this.isDispensing = (amount > 0f);
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x000E6CD4 File Offset: 0x000E4ED4
	public override void UpdateAnim()
	{
		if (!this.operational.IsOperational)
		{
			this.controller.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (this.isDispensing)
		{
			this.controller.Queue("on_flow", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		this.controller.Queue("on", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001968 RID: 6504
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001969 RID: 6505
	private bool isDispensing;

	// Token: 0x0400196A RID: 6506
	private static readonly EventSystem.IntraObjectHandler<OperationalValve> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalValve>(delegate(OperationalValve component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
