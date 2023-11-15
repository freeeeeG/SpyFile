using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000600 RID: 1536
public class FishFeeder : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>
{
	// Token: 0x0600268C RID: 9868 RVA: 0x000D1640 File Offset: 0x000CF840
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notoperational;
		this.root.Enter(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.SetupFishFeederTopAndBot)).Exit(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.CleanupFishFeederTopAndBot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnStorageChange)).EventHandler(GameHashes.RefreshUserMenu, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnRefreshUserMenu));
		this.notoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.DefaultState(this.operational.on).TagTransition(GameTags.Operational, this.notoperational, true);
		this.operational.on.DoNothing();
		int num = 19;
		FishFeeder.ballSymbols = new HashedString[num];
		for (int i = 0; i < num; i++)
		{
			FishFeeder.ballSymbols[i] = "ball" + i.ToString();
		}
	}

	// Token: 0x0600268D RID: 9869 RVA: 0x000D1738 File Offset: 0x000CF938
	private static void SetupFishFeederTopAndBot(FishFeeder.Instance smi)
	{
		Storage storage = smi.Get<Storage>();
		smi.fishFeederTop = new FishFeeder.FishFeederTop(smi, FishFeeder.ballSymbols, storage.Capacity());
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot = new FishFeeder.FishFeederBot(smi, 10f, FishFeeder.ballSymbols);
		smi.fishFeederBot.RefreshStorage();
		smi.fishFeederTop.ToggleMutantSeedFetches(smi.ForbidMutantSeeds);
		smi.UpdateMutantSeedStatusItem();
	}

	// Token: 0x0600268E RID: 9870 RVA: 0x000D17A6 File Offset: 0x000CF9A6
	private static void CleanupFishFeederTopAndBot(FishFeeder.Instance smi)
	{
		smi.fishFeederTop.Cleanup();
		smi.fishFeederBot.Cleanup();
	}

	// Token: 0x0600268F RID: 9871 RVA: 0x000D17C0 File Offset: 0x000CF9C0
	private static void MoveStoredContentsToConsumeOffset(FishFeeder.Instance smi)
	{
		foreach (GameObject gameObject in smi.GetComponent<Storage>().items)
		{
			if (!(gameObject == null))
			{
				FishFeeder.OnStorageChange(smi, gameObject);
			}
		}
	}

	// Token: 0x06002690 RID: 9872 RVA: 0x000D1824 File Offset: 0x000CFA24
	private static void OnStorageChange(FishFeeder.Instance smi, object data)
	{
		if ((GameObject)data == null)
		{
			return;
		}
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot.RefreshStorage();
	}

	// Token: 0x06002691 RID: 9873 RVA: 0x000D184C File Offset: 0x000CFA4C
	private static void OnRefreshUserMenu(FishFeeder.Instance smi, object data)
	{
		if (DlcManager.FeatureRadiationEnabled())
		{
			Game.Instance.userMenu.AddButton(smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", smi.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT, delegate()
			{
				smi.ForbidMutantSeeds = !smi.ForbidMutantSeeds;
				FishFeeder.OnRefreshUserMenu(smi, null);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.FISH_FEEDER_TOOLTIP, true), 1f);
		}
	}

	// Token: 0x04001617 RID: 5655
	public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State notoperational;

	// Token: 0x04001618 RID: 5656
	public FishFeeder.OperationalState operational;

	// Token: 0x04001619 RID: 5657
	public static HashedString[] ballSymbols;

	// Token: 0x020012A5 RID: 4773
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020012A6 RID: 4774
	public class OperationalState : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State
	{
		// Token: 0x0400603C RID: 24636
		public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State on;
	}

	// Token: 0x020012A7 RID: 4775
	public new class Instance : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameInstance
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06007E0C RID: 32268 RVA: 0x002E6652 File Offset: 0x002E4852
		// (set) Token: 0x06007E0D RID: 32269 RVA: 0x002E665A File Offset: 0x002E485A
		public bool ForbidMutantSeeds
		{
			get
			{
				return this.forbidMutantSeeds;
			}
			set
			{
				this.forbidMutantSeeds = value;
				this.fishFeederTop.ToggleMutantSeedFetches(this.forbidMutantSeeds);
				this.UpdateMutantSeedStatusItem();
			}
		}

		// Token: 0x06007E0E RID: 32270 RVA: 0x002E667C File Offset: 0x002E487C
		public Instance(IStateMachineTarget master, FishFeeder.Def def) : base(master, def)
		{
			this.mutantSeedStatusItem = new StatusItem("FISHFEEDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettingsDelegate));
		}

		// Token: 0x06007E0F RID: 32271 RVA: 0x002E66D4 File Offset: 0x002E48D4
		private void OnCopySettingsDelegate(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return;
			}
			FishFeeder.Instance smi = gameObject.GetSMI<FishFeeder.Instance>();
			if (smi == null)
			{
				return;
			}
			this.ForbidMutantSeeds = smi.ForbidMutantSeeds;
		}

		// Token: 0x06007E10 RID: 32272 RVA: 0x002E6709 File Offset: 0x002E4909
		public void UpdateMutantSeedStatusItem()
		{
			base.gameObject.GetComponent<KSelectable>().ToggleStatusItem(this.mutantSeedStatusItem, DlcManager.IsContentActive("EXPANSION1_ID") && !this.forbidMutantSeeds, null);
		}

		// Token: 0x0400603D RID: 24637
		private StatusItem mutantSeedStatusItem;

		// Token: 0x0400603E RID: 24638
		public FishFeeder.FishFeederTop fishFeederTop;

		// Token: 0x0400603F RID: 24639
		public FishFeeder.FishFeederBot fishFeederBot;

		// Token: 0x04006040 RID: 24640
		[Serialize]
		private bool forbidMutantSeeds;
	}

	// Token: 0x020012A8 RID: 4776
	public class FishFeederTop : IRenderEveryTick
	{
		// Token: 0x06007E11 RID: 32273 RVA: 0x002E673B File Offset: 0x002E493B
		public FishFeederTop(FishFeeder.Instance smi, HashedString[] ball_symbols, float capacity)
		{
			this.smi = smi;
			this.ballSymbols = ball_symbols;
			this.massPerBall = capacity / (float)ball_symbols.Length;
			this.FillFeeder(this.mass);
			SimAndRenderScheduler.instance.Add(this, false);
		}

		// Token: 0x06007E12 RID: 32274 RVA: 0x002E6778 File Offset: 0x002E4978
		private void FillFeeder(float mass)
		{
			KBatchedAnimController component = this.smi.GetComponent<KBatchedAnimController>();
			SymbolOverrideController component2 = this.smi.GetComponent<SymbolOverrideController>();
			KAnim.Build.Symbol symbol = null;
			Storage component3 = this.smi.GetComponent<Storage>();
			if (component3.items.Count > 0 && component3.items[0] != null)
			{
				symbol = this.smi.GetComponent<Storage>().items[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol("algae");
			}
			for (int i = 0; i < this.ballSymbols.Length; i++)
			{
				bool is_visible = mass > (float)(i + 1) * this.massPerBall;
				component.SetSymbolVisiblity(this.ballSymbols[i], is_visible);
				if (symbol != null)
				{
					component2.AddSymbolOverride(this.ballSymbols[i], symbol, 0);
				}
			}
		}

		// Token: 0x06007E13 RID: 32275 RVA: 0x002E6864 File Offset: 0x002E4A64
		public void RefreshStorage()
		{
			float num = 0f;
			foreach (GameObject gameObject in this.smi.GetComponent<Storage>().items)
			{
				if (!(gameObject == null))
				{
					num += gameObject.GetComponent<PrimaryElement>().Mass;
				}
			}
			this.targetMass = num;
		}

		// Token: 0x06007E14 RID: 32276 RVA: 0x002E68E0 File Offset: 0x002E4AE0
		public void RenderEveryTick(float dt)
		{
			this.timeSinceLastBallAppeared += dt;
			if (this.targetMass > this.mass && this.timeSinceLastBallAppeared > 0.025f)
			{
				float num = Mathf.Min(this.massPerBall, this.targetMass - this.mass);
				this.mass += num;
				this.FillFeeder(this.mass);
				this.timeSinceLastBallAppeared = 0f;
			}
		}

		// Token: 0x06007E15 RID: 32277 RVA: 0x002E6954 File Offset: 0x002E4B54
		public void Cleanup()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x06007E16 RID: 32278 RVA: 0x002E6964 File Offset: 0x002E4B64
		public void ToggleMutantSeedFetches(bool allow)
		{
			StorageLocker component = this.smi.GetComponent<StorageLocker>();
			if (component != null)
			{
				component.UpdateForbiddenTag(GameTags.MutatedSeed, !allow);
			}
		}

		// Token: 0x04006041 RID: 24641
		private FishFeeder.Instance smi;

		// Token: 0x04006042 RID: 24642
		private float mass;

		// Token: 0x04006043 RID: 24643
		private float targetMass;

		// Token: 0x04006044 RID: 24644
		private HashedString[] ballSymbols;

		// Token: 0x04006045 RID: 24645
		private float massPerBall;

		// Token: 0x04006046 RID: 24646
		private float timeSinceLastBallAppeared;
	}

	// Token: 0x020012A9 RID: 4777
	public class FishFeederBot
	{
		// Token: 0x06007E17 RID: 32279 RVA: 0x002E6998 File Offset: 0x002E4B98
		public FishFeederBot(FishFeeder.Instance smi, float mass_per_ball, HashedString[] ball_symbols)
		{
			this.smi = smi;
			this.massPerBall = mass_per_ball;
			this.anim = GameUtil.KInstantiate(Assets.GetPrefab("FishFeederBot"), smi.transform.GetPosition(), Grid.SceneLayer.Front, null, 0).GetComponent<KBatchedAnimController>();
			this.anim.transform.SetParent(smi.transform);
			this.anim.gameObject.SetActive(true);
			this.anim.SetSceneLayer(Grid.SceneLayer.Building);
			this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
			this.anim.Stop();
			foreach (HashedString hash in ball_symbols)
			{
				this.anim.SetSymbolVisiblity(hash, false);
			}
			Storage[] components = smi.gameObject.GetComponents<Storage>();
			this.topStorage = components[0];
			this.botStorage = components[1];
		}

		// Token: 0x06007E18 RID: 32280 RVA: 0x002E6A90 File Offset: 0x002E4C90
		public void RefreshStorage()
		{
			if (this.refreshingStorage)
			{
				return;
			}
			this.refreshingStorage = true;
			foreach (GameObject gameObject in this.botStorage.items)
			{
				if (!(gameObject == null))
				{
					int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
					gameObject.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore));
				}
			}
			if (this.botStorage.IsEmpty())
			{
				float num = 0f;
				foreach (GameObject gameObject2 in this.topStorage.items)
				{
					if (!(gameObject2 == null))
					{
						num += gameObject2.GetComponent<PrimaryElement>().Mass;
					}
				}
				if (num > 0f)
				{
					this.anim.SetSymbolVisiblity(FishFeeder.FishFeederBot.HASH_FEEDBALL, true);
					this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
					Pickupable pickupable = this.topStorage.items[0].GetComponent<Pickupable>().Take(this.massPerBall);
					KAnim.Build.Symbol symbol = pickupable.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol("algae");
					if (symbol != null)
					{
						this.anim.GetComponent<SymbolOverrideController>().AddSymbolOverride(FishFeeder.FishFeederBot.HASH_FEEDBALL, symbol, 0);
					}
					this.botStorage.Store(pickupable.gameObject, false, false, true, false);
					int cell2 = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
					pickupable.transform.SetPosition(Grid.CellToPosCBC(cell2, Grid.SceneLayer.BuildingUse));
				}
				else
				{
					this.anim.SetSymbolVisiblity(FishFeeder.FishFeederBot.HASH_FEEDBALL, false);
				}
			}
			this.refreshingStorage = false;
		}

		// Token: 0x06007E19 RID: 32281 RVA: 0x002E6CB8 File Offset: 0x002E4EB8
		public void Cleanup()
		{
		}

		// Token: 0x04006047 RID: 24647
		private KBatchedAnimController anim;

		// Token: 0x04006048 RID: 24648
		private Storage topStorage;

		// Token: 0x04006049 RID: 24649
		private Storage botStorage;

		// Token: 0x0400604A RID: 24650
		private bool refreshingStorage;

		// Token: 0x0400604B RID: 24651
		private FishFeeder.Instance smi;

		// Token: 0x0400604C RID: 24652
		private float massPerBall;

		// Token: 0x0400604D RID: 24653
		private static readonly HashedString HASH_FEEDBALL = "feedball";
	}
}
