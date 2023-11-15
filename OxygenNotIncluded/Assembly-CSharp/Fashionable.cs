using System;

// Token: 0x020008AF RID: 2223
[SkipSaveFileSerialization]
public class Fashionable : StateMachineComponent<Fashionable.StatesInstance>
{
	// Token: 0x06004070 RID: 16496 RVA: 0x00168C10 File Offset: 0x00166E10
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004071 RID: 16497 RVA: 0x00168C20 File Offset: 0x00166E20
	protected bool IsUncomfortable()
	{
		ClothingWearer component = base.GetComponent<ClothingWearer>();
		return component != null && component.currentClothing.decorMod <= 0;
	}

	// Token: 0x020016F3 RID: 5875
	public class StatesInstance : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.GameInstance
	{
		// Token: 0x06008D0F RID: 36111 RVA: 0x0031A86A File Offset: 0x00318A6A
		public StatesInstance(Fashionable master) : base(master)
		{
		}
	}

	// Token: 0x020016F4 RID: 5876
	public class States : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable>
	{
		// Token: 0x06008D10 RID: 36112 RVA: 0x0031A874 File Offset: 0x00318A74
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.EventHandler(GameHashes.EquippedItemEquipper, delegate(Fashionable.StatesInstance smi)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}).EventHandler(GameHashes.UnequippedItemEquipper, delegate(Fashionable.StatesInstance smi)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			});
			this.suffering.AddEffect("UnfashionableClothing").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04006D7C RID: 28028
		public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State satisfied;

		// Token: 0x04006D7D RID: 28029
		public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State suffering;
	}
}
