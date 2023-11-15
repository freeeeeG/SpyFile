using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class FixedCapturePoint : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>
{
	// Token: 0x06001B56 RID: 6998 RVA: 0x0009303C File Offset: 0x0009123C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.operational;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.unoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.DefaultState(this.operational.manual).TagTransition(GameTags.Operational, this.unoperational, true);
		this.operational.manual.ParamTransition<bool>(this.automated, this.operational.automated, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsTrue);
		this.operational.automated.ParamTransition<bool>(this.automated, this.operational.manual, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsFalse).ToggleChore((FixedCapturePoint.Instance smi) => smi.CreateChore(), this.unoperational, this.unoperational).Update("FindFixedCapturable", delegate(FixedCapturePoint.Instance smi, float dt)
		{
			smi.FindFixedCapturable();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x04000F3C RID: 3900
	private StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.BoolParameter automated;

	// Token: 0x04000F3D RID: 3901
	public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State unoperational;

	// Token: 0x04000F3E RID: 3902
	public FixedCapturePoint.OperationalState operational;

	// Token: 0x02001168 RID: 4456
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005C35 RID: 23605
		public Func<GameObject, FixedCapturePoint.Instance, bool> isCreatureEligibleToBeCapturedCb;

		// Token: 0x04005C36 RID: 23606
		public Func<FixedCapturePoint.Instance, int> getTargetCapturePoint = delegate(FixedCapturePoint.Instance smi)
		{
			int num = Grid.PosToCell(smi);
			Navigator component = smi.targetCapturable.GetComponent<Navigator>();
			if (Grid.IsValidCell(num - 1) && component.CanReach(num - 1))
			{
				return num - 1;
			}
			if (Grid.IsValidCell(num + 1) && component.CanReach(num + 1))
			{
				return num + 1;
			}
			return num;
		};
	}

	// Token: 0x02001169 RID: 4457
	public class OperationalState : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State
	{
		// Token: 0x04005C37 RID: 23607
		public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State manual;

		// Token: 0x04005C38 RID: 23608
		public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State automated;
	}

	// Token: 0x0200116A RID: 4458
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.GameInstance, ICheckboxControl
	{
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600797A RID: 31098 RVA: 0x002D94AC File Offset: 0x002D76AC
		// (set) Token: 0x0600797B RID: 31099 RVA: 0x002D94B4 File Offset: 0x002D76B4
		public FixedCapturableMonitor.Instance targetCapturable { get; private set; }

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600797C RID: 31100 RVA: 0x002D94BD File Offset: 0x002D76BD
		// (set) Token: 0x0600797D RID: 31101 RVA: 0x002D94C5 File Offset: 0x002D76C5
		public bool shouldCreatureGoGetCaptured { get; private set; }

		// Token: 0x0600797E RID: 31102 RVA: 0x002D94CE File Offset: 0x002D76CE
		public Instance(IStateMachineTarget master, FixedCapturePoint.Def def) : base(master, def)
		{
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
		}

		// Token: 0x0600797F RID: 31103 RVA: 0x002D94F0 File Offset: 0x002D76F0
		private void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return;
			}
			FixedCapturePoint.Instance smi = gameObject.GetSMI<FixedCapturePoint.Instance>();
			if (smi == null)
			{
				return;
			}
			base.sm.automated.Set(base.sm.automated.Get(smi), this, false);
		}

		// Token: 0x06007980 RID: 31104 RVA: 0x002D953D File Offset: 0x002D773D
		public Chore CreateChore()
		{
			this.FindFixedCapturable();
			return new FixedCaptureChore(base.GetComponent<KPrefabID>());
		}

		// Token: 0x06007981 RID: 31105 RVA: 0x002D9550 File Offset: 0x002D7750
		public bool IsCreatureAvailableForFixedCapture()
		{
			if (!this.targetCapturable.IsNullOrStopped())
			{
				int num = Grid.PosToCell(base.transform.GetPosition());
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(num);
				return FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, num);
			}
			return false;
		}

		// Token: 0x06007982 RID: 31106 RVA: 0x002D959C File Offset: 0x002D779C
		public void SetRancherIsAvailableForCapturing()
		{
			this.shouldCreatureGoGetCaptured = true;
		}

		// Token: 0x06007983 RID: 31107 RVA: 0x002D95A5 File Offset: 0x002D77A5
		public void ClearRancherIsAvailableForCapturing()
		{
			this.shouldCreatureGoGetCaptured = false;
		}

		// Token: 0x06007984 RID: 31108 RVA: 0x002D95B0 File Offset: 0x002D77B0
		private static bool CanCapturableBeCapturedAtCapturePoint(FixedCapturableMonitor.Instance capturable, FixedCapturePoint.Instance capture_point, CavityInfo capture_cavity_info, int capture_cell)
		{
			if (!capturable.IsRunning())
			{
				return false;
			}
			if (capturable.targetCapturePoint != capture_point && !capturable.targetCapturePoint.IsNullOrStopped())
			{
				return false;
			}
			if (capture_point.def.isCreatureEligibleToBeCapturedCb != null && !capture_point.def.isCreatureEligibleToBeCapturedCb(capturable.gameObject, capture_point))
			{
				return false;
			}
			int cell = Grid.PosToCell(capturable.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (cavityForCell == null || cavityForCell != capture_cavity_info)
			{
				return false;
			}
			if (capturable.HasTag(GameTags.Creatures.Bagged))
			{
				return false;
			}
			if (!capturable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>())
			{
				return false;
			}
			if (capturable.GetComponent<Navigator>().GetNavigationCost(capture_cell) == -1)
			{
				return false;
			}
			TreeFilterable component = capture_point.GetComponent<TreeFilterable>();
			IUserControlledCapacity component2 = capture_point.GetComponent<IUserControlledCapacity>();
			return !component.ContainsTag(capturable.GetComponent<KPrefabID>().PrefabTag) || component2.AmountStored > component2.UserMaxCapacity;
		}

		// Token: 0x06007985 RID: 31109 RVA: 0x002D9694 File Offset: 0x002D7894
		public void FindFixedCapturable()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(num);
			if (cavityForCell == null)
			{
				this.ResetCapturePoint();
				return;
			}
			if (!this.targetCapturable.IsNullOrStopped() && !FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, num))
			{
				this.ResetCapturePoint();
			}
			if (this.targetCapturable.IsNullOrStopped())
			{
				foreach (object obj in Components.FixedCapturableMonitors)
				{
					FixedCapturableMonitor.Instance instance = (FixedCapturableMonitor.Instance)obj;
					if (FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(instance, this, cavityForCell, num))
					{
						this.targetCapturable = instance;
						if (!this.targetCapturable.IsNullOrStopped())
						{
							this.targetCapturable.targetCapturePoint = this;
							break;
						}
						break;
					}
				}
			}
		}

		// Token: 0x06007986 RID: 31110 RVA: 0x002D9774 File Offset: 0x002D7974
		public void ResetCapturePoint()
		{
			base.Trigger(643180843, null);
			if (!this.targetCapturable.IsNullOrStopped())
			{
				this.targetCapturable.targetCapturePoint = null;
				this.targetCapturable.Trigger(1034952693, null);
				this.targetCapturable = null;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06007987 RID: 31111 RVA: 0x002D97B3 File Offset: 0x002D79B3
		string ICheckboxControl.CheckboxTitleKey
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.TITLE.key.String;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06007988 RID: 31112 RVA: 0x002D97C4 File Offset: 0x002D79C4
		string ICheckboxControl.CheckboxLabel
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06007989 RID: 31113 RVA: 0x002D97D0 File Offset: 0x002D79D0
		string ICheckboxControl.CheckboxTooltip
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE_TOOLTIP;
			}
		}

		// Token: 0x0600798A RID: 31114 RVA: 0x002D97DC File Offset: 0x002D79DC
		bool ICheckboxControl.GetCheckboxValue()
		{
			return base.sm.automated.Get(this);
		}

		// Token: 0x0600798B RID: 31115 RVA: 0x002D97EF File Offset: 0x002D79EF
		void ICheckboxControl.SetCheckboxValue(bool value)
		{
			base.sm.automated.Set(value, this, false);
		}
	}
}
