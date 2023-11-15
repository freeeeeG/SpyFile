using System;
using KSerialization;

// Token: 0x02000434 RID: 1076
[SerializationConfig(MemberSerialization.OptIn)]
public class StateMachineComponent<StateMachineInstanceType> : StateMachineComponent, ISaveLoadable where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060016A2 RID: 5794 RVA: 0x00075B38 File Offset: 0x00073D38
	public StateMachineInstanceType smi
	{
		get
		{
			if (this._smi == null)
			{
				this._smi = (StateMachineInstanceType)((object)Activator.CreateInstance(typeof(StateMachineInstanceType), new object[]
				{
					this
				}));
			}
			return this._smi;
		}
	}

	// Token: 0x060016A3 RID: 5795 RVA: 0x00075B71 File Offset: 0x00073D71
	public override StateMachine.Instance GetSMI()
	{
		return this._smi;
	}

	// Token: 0x060016A4 RID: 5796 RVA: 0x00075B7E File Offset: 0x00073D7E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this._smi != null)
		{
			this._smi.StopSM("StateMachineComponent.OnCleanUp");
			this._smi = default(StateMachineInstanceType);
		}
	}

	// Token: 0x060016A5 RID: 5797 RVA: 0x00075BB4 File Offset: 0x00073DB4
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (base.isSpawned)
		{
			this.smi.StartSM();
		}
	}

	// Token: 0x060016A6 RID: 5798 RVA: 0x00075BD4 File Offset: 0x00073DD4
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this._smi != null)
		{
			this._smi.StopSM("StateMachineComponent.OnDisable");
		}
	}

	// Token: 0x04000CA1 RID: 3233
	private StateMachineInstanceType _smi;
}
