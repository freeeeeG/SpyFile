using System;

// Token: 0x020008E8 RID: 2280
public class SaltPlant : StateMachineComponent<SaltPlant.StatesInstance>
{
	// Token: 0x060041EB RID: 16875 RVA: 0x001708A9 File Offset: 0x0016EAA9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SaltPlant>(-724860998, SaltPlant.OnWiltDelegate);
		base.Subscribe<SaltPlant>(712767498, SaltPlant.OnWiltRecoverDelegate);
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x001708D3 File Offset: 0x0016EAD3
	private void OnWilt(object data = null)
	{
		base.gameObject.GetComponent<ElementConsumer>().EnableConsumption(false);
	}

	// Token: 0x060041ED RID: 16877 RVA: 0x001708E6 File Offset: 0x0016EAE6
	private void OnWiltRecover(object data = null)
	{
		base.gameObject.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x04002B07 RID: 11015
	private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltDelegate = new EventSystem.IntraObjectHandler<SaltPlant>(delegate(SaltPlant component, object data)
	{
		component.OnWilt(data);
	});

	// Token: 0x04002B08 RID: 11016
	private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltRecoverDelegate = new EventSystem.IntraObjectHandler<SaltPlant>(delegate(SaltPlant component, object data)
	{
		component.OnWiltRecover(data);
	});

	// Token: 0x02001735 RID: 5941
	public class StatesInstance : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.GameInstance
	{
		// Token: 0x06008DD3 RID: 36307 RVA: 0x0031DAA8 File Offset: 0x0031BCA8
		public StatesInstance(SaltPlant master) : base(master)
		{
		}
	}

	// Token: 0x02001736 RID: 5942
	public class States : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant>
	{
		// Token: 0x06008DD4 RID: 36308 RVA: 0x0031DAB1 File Offset: 0x0031BCB1
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.alive;
			this.alive.DoNothing();
		}

		// Token: 0x04006DE7 RID: 28135
		public GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.State alive;
	}
}
