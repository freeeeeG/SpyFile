using System;

// Token: 0x02000A2D RID: 2605
public class WaterTrapTrail : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>
{
	// Token: 0x06004E0B RID: 19979 RVA: 0x001B59D0 File Offset: 0x001B3BD0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.retracted;
		base.serializable = StateMachine.SerializeType.Never;
		this.retracted.EventHandler(GameHashes.TrapArmWorkPST, delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.loose, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.ShouldBeVisible)).Enter(delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		});
		this.loose.EventHandlerTransition(GameHashes.TagsChanged, this.retracted, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.OnTagsChangedWhenOnLooseState)).EventHandler(GameHashes.TrapCaptureCompleted, delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		}).Enter(delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		});
	}

	// Token: 0x06004E0C RID: 19980 RVA: 0x001B5ACC File Offset: 0x001B3CCC
	public static bool OnTagsChangedWhenOnLooseState(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance smi2 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		if (smi2 != null)
		{
			smi2.CAPTURING_SYMBOL_NAME = WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + smi.sm.depthAvailable.Get(smi).ToString();
		}
		return WaterTrapTrail.ShouldBeInvisible(smi, tagOBJ);
	}

	// Token: 0x06004E0D RID: 19981 RVA: 0x001B5B18 File Offset: 0x001B3D18
	public static bool ShouldBeInvisible(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		return !WaterTrapTrail.ShouldBeVisible(smi, tagOBJ);
	}

	// Token: 0x06004E0E RID: 19982 RVA: 0x001B5B24 File Offset: 0x001B3D24
	public static bool ShouldBeVisible(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance smi2 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		bool isOperational = smi.IsOperational;
		bool flag = smi.HasTag(GameTags.TrapArmed);
		bool flag2 = smi2 != null && smi2.IsInsideState(smi2.sm.operational.capture) && !smi2.IsInsideState(smi2.sm.operational.capture.idle) && !smi2.IsInsideState(smi2.sm.operational.capture.release);
		bool flag3 = smi2 != null && smi2.IsInsideState(smi2.sm.operational.unarmed) && smi2.GetWorkable().WorkInPstAnimation;
		return isOperational && (flag || flag2 || flag3);
	}

	// Token: 0x06004E0F RID: 19983 RVA: 0x001B5BDC File Offset: 0x001B3DDC
	public static void RefreshDepthAvailable(WaterTrapTrail.Instance smi, float dt)
	{
		bool flag = WaterTrapTrail.ShouldBeVisible(smi, null);
		int num = Grid.PosToCell(smi);
		int num2 = flag ? WaterTrapGuide.GetDepthAvailable(num, smi.gameObject) : 0;
		int num3 = 4;
		if (num2 != smi.sm.depthAvailable.Get(smi))
		{
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			for (int i = 1; i <= num3; i++)
			{
				component.SetSymbolVisiblity("pipe" + i.ToString(), i <= num2);
				component.SetSymbolVisiblity(WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + i.ToString(), i == num2);
			}
			int cell = Grid.OffsetCell(num, 0, -num2);
			smi.ChangeTrapCellPosition(cell);
			WaterTrapGuide.OccupyArea(smi.gameObject, num2);
			smi.sm.depthAvailable.Set(num2, smi, false);
		}
		smi.SetRangeVisualizerOffset(new Vector2I(0, -num2));
		smi.SetRangeVisualizerVisibility(flag);
	}

	// Token: 0x040032E7 RID: 13031
	private static string CAPTURING_SYMBOL_OVERRIDE_NAME = "creatureSymbol";

	// Token: 0x040032E8 RID: 13032
	public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State retracted;

	// Token: 0x040032E9 RID: 13033
	public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State loose;

	// Token: 0x040032EA RID: 13034
	private StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter depthAvailable = new StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter(-1);

	// Token: 0x020018B5 RID: 6325
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018B6 RID: 6326
	public new class Instance : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.GameInstance
	{
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06009291 RID: 37521 RVA: 0x0032C779 File Offset: 0x0032A979
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06009292 RID: 37522 RVA: 0x0032C786 File Offset: 0x0032A986
		public Lure.Instance lureSMI
		{
			get
			{
				if (this._lureSMI == null)
				{
					this._lureSMI = base.gameObject.GetSMI<Lure.Instance>();
				}
				return this._lureSMI;
			}
		}

		// Token: 0x06009293 RID: 37523 RVA: 0x0032C7A7 File Offset: 0x0032A9A7
		public Instance(IStateMachineTarget master, WaterTrapTrail.Def def) : base(master, def)
		{
		}

		// Token: 0x06009294 RID: 37524 RVA: 0x0032C7B1 File Offset: 0x0032A9B1
		public override void StartSM()
		{
			base.StartSM();
			this.RegisterListenersToCellChanges();
		}

		// Token: 0x06009295 RID: 37525 RVA: 0x0032C7C0 File Offset: 0x0032A9C0
		private void RegisterListenersToCellChanges()
		{
			int widthInCells = base.GetComponent<BuildingComplete>().Def.WidthInCells;
			CellOffset[] array = new CellOffset[widthInCells * 4];
			for (int i = 0; i < 4; i++)
			{
				int y = -(i + 1);
				for (int j = 0; j < widthInCells; j++)
				{
					array[i * widthInCells + j] = new CellOffset(j, y);
				}
			}
			Extents extents = new Extents(Grid.PosToCell(base.transform.GetPosition()), array);
			this.partitionerEntry_solids = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnLowerCellChanged));
			this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnLowerCellChanged));
		}

		// Token: 0x06009296 RID: 37526 RVA: 0x0032C89C File Offset: 0x0032AA9C
		private void UnregisterListenersToCellChanges()
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
		}

		// Token: 0x06009297 RID: 37527 RVA: 0x0032C8BE File Offset: 0x0032AABE
		private void OnLowerCellChanged(object o)
		{
			WaterTrapTrail.RefreshDepthAvailable(base.smi, 0f);
		}

		// Token: 0x06009298 RID: 37528 RVA: 0x0032C8D0 File Offset: 0x0032AAD0
		protected override void OnCleanUp()
		{
			this.UnregisterListenersToCellChanges();
			base.OnCleanUp();
		}

		// Token: 0x06009299 RID: 37529 RVA: 0x0032C8DE File Offset: 0x0032AADE
		public void SetRangeVisualizerVisibility(bool visible)
		{
			this.rangeVisualizer.RangeMax.x = (visible ? 0 : -1);
		}

		// Token: 0x0600929A RID: 37530 RVA: 0x0032C8F7 File Offset: 0x0032AAF7
		public void SetRangeVisualizerOffset(Vector2I offset)
		{
			this.rangeVisualizer.OriginOffset = offset;
		}

		// Token: 0x0600929B RID: 37531 RVA: 0x0032C905 File Offset: 0x0032AB05
		public void ChangeTrapCellPosition(int cell)
		{
			if (this.lureSMI != null)
			{
				this.lureSMI.ChangeLureCellPosition(cell);
			}
			base.gameObject.GetComponent<TrapTrigger>().SetTriggerCell(cell);
		}

		// Token: 0x040072BC RID: 29372
		[MyCmpGet]
		private Operational operational;

		// Token: 0x040072BD RID: 29373
		[MyCmpGet]
		private RangeVisualizer rangeVisualizer;

		// Token: 0x040072BE RID: 29374
		private HandleVector<int>.Handle partitionerEntry_buildings;

		// Token: 0x040072BF RID: 29375
		private HandleVector<int>.Handle partitionerEntry_solids;

		// Token: 0x040072C0 RID: 29376
		private Lure.Instance _lureSMI;
	}
}
