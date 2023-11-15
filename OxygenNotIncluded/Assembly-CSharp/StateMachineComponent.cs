using System;
using KSerialization;

// Token: 0x02000433 RID: 1075
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class StateMachineComponent : KMonoBehaviour, ISaveLoadable, IStateMachineTarget
{
	// Token: 0x060016A0 RID: 5792
	public abstract StateMachine.Instance GetSMI();

	// Token: 0x04000CA0 RID: 3232
	[MyCmpAdd]
	protected StateMachineController stateMachineController;
}
