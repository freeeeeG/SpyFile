using System;
using KSerialization;

// Token: 0x02000665 RID: 1637
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalControlledSwitch : CircuitSwitch
{
	// Token: 0x06002B56 RID: 11094 RVA: 0x000E6BC9 File Offset: 0x000E4DC9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.manuallyControlled = false;
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x000E6BD8 File Offset: 0x000E4DD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<OperationalControlledSwitch>(-592767678, OperationalControlledSwitch.OnOperationalChangedDelegate);
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x000E6BF4 File Offset: 0x000E4DF4
	private void OnOperationalChanged(object data)
	{
		bool state = (bool)data;
		this.SetState(state);
	}

	// Token: 0x04001967 RID: 6503
	private static readonly EventSystem.IntraObjectHandler<OperationalControlledSwitch> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalControlledSwitch>(delegate(OperationalControlledSwitch component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
