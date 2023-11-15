using System;
using KSerialization;

// Token: 0x02000AE0 RID: 2784
[SerializationConfig(MemberSerialization.OptIn)]
public class CreatureBait : StateMachineComponent<CreatureBait.StatesInstance>
{
	// Token: 0x060055BB RID: 21947 RVA: 0x001F30EF File Offset: 0x001F12EF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060055BC RID: 21948 RVA: 0x001F30F8 File Offset: 0x001F12F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Tag[] constructionElements = base.GetComponent<Deconstructable>().constructionElements;
		this.baitElement = ((constructionElements.Length > 1) ? constructionElements[1] : constructionElements[0]);
		base.gameObject.GetSMI<Lure.Instance>().SetActiveLures(new Tag[]
		{
			this.baitElement
		});
		base.smi.StartSM();
	}

	// Token: 0x04003980 RID: 14720
	[Serialize]
	public Tag baitElement;

	// Token: 0x020019F5 RID: 6645
	public class StatesInstance : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.GameInstance
	{
		// Token: 0x060095B6 RID: 38326 RVA: 0x0033B266 File Offset: 0x00339466
		public StatesInstance(CreatureBait master) : base(master)
		{
		}
	}

	// Token: 0x020019F6 RID: 6646
	public class States : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait>
	{
		// Token: 0x060095B7 RID: 38327 RVA: 0x0033B270 File Offset: 0x00339470
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Baited, null).Enter(delegate(CreatureBait.StatesInstance smi)
			{
				KAnim.Build build = ElementLoader.FindElementByName(smi.master.baitElement.ToString()).substance.anim.GetData().build;
				KAnim.Build.Symbol symbol = build.GetSymbol(new KAnimHashedString(build.name));
				HashedString target_symbol = "snapTo_bait";
				smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(target_symbol, symbol, 0);
			}).TagTransition(GameTags.LureUsed, this.destroy, false);
			this.destroy.PlayAnim("use").EventHandler(GameHashes.AnimQueueComplete, delegate(CreatureBait.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x040077EA RID: 30698
		public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State idle;

		// Token: 0x040077EB RID: 30699
		public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State destroy;
	}
}
