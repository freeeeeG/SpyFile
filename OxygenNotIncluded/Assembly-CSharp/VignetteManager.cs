using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008A2 RID: 2210
public class VignetteManager : GameStateMachine<VignetteManager, VignetteManager.Instance>
{
	// Token: 0x0600400E RID: 16398 RVA: 0x00166884 File Offset: 0x00164A84
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.IsTrue);
		this.on.Exit("VignetteOff", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).ParamTransition<bool>(this.isRedAlert, this.on.red, (VignetteManager.Instance smi, bool p) => this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isRedAlert, this.on.yellow, (VignetteManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isYellowAlert, this.on.yellow, (VignetteManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.IsFalse);
		this.on.red.Enter("EnterEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(1585324898, null);
		}).Exit("ExitEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-1393151672, null);
		}).Enter("EnableVignette", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 0f, 0f, 0.3f));
		}).Enter("SoundsOnRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("RedAlert_LP", false), null, true, false, true).ToggleNotification((VignetteManager.Instance smi) => smi.redAlertNotification);
		this.on.yellow.Enter("EnterEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("EnableVignette", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 1f, 0f, 0.3f));
		}).Enter("SoundsOnYellowAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_OFF", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("YellowAlert_LP", false), null, true, false, true);
	}

	// Token: 0x040029B8 RID: 10680
	public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040029B9 RID: 10681
	public VignetteManager.OnStates on;

	// Token: 0x040029BA RID: 10682
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isRedAlert = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x040029BB RID: 10683
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isYellowAlert = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x040029BC RID: 10684
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isOn = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x020016D5 RID: 5845
	public class OnStates : GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006D12 RID: 27922
		public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State yellow;

		// Token: 0x04006D13 RID: 27923
		public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State red;
	}

	// Token: 0x020016D6 RID: 5846
	public new class Instance : GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008CA2 RID: 36002 RVA: 0x00319726 File Offset: 0x00317926
		public static void DestroyInstance()
		{
			VignetteManager.Instance.instance = null;
		}

		// Token: 0x06008CA3 RID: 36003 RVA: 0x0031972E File Offset: 0x0031792E
		public static VignetteManager.Instance Get()
		{
			return VignetteManager.Instance.instance;
		}

		// Token: 0x06008CA4 RID: 36004 RVA: 0x00319738 File Offset: 0x00317938
		public Instance(IStateMachineTarget master) : base(master)
		{
			VignetteManager.Instance.instance = this;
		}

		// Token: 0x06008CA5 RID: 36005 RVA: 0x00319794 File Offset: 0x00317994
		public void UpdateState(float dt)
		{
			if (this.IsRedAlert())
			{
				base.smi.GoTo(base.sm.on.red);
				return;
			}
			if (this.IsYellowAlert())
			{
				base.smi.GoTo(base.sm.on.yellow);
				return;
			}
			if (!this.IsOn())
			{
				base.smi.GoTo(base.sm.off);
			}
		}

		// Token: 0x06008CA6 RID: 36006 RVA: 0x00319807 File Offset: 0x00317A07
		public bool IsOn()
		{
			return base.sm.isYellowAlert.Get(base.smi) || base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x06008CA7 RID: 36007 RVA: 0x00319839 File Offset: 0x00317A39
		public bool IsRedAlert()
		{
			return base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x06008CA8 RID: 36008 RVA: 0x00319851 File Offset: 0x00317A51
		public bool IsYellowAlert()
		{
			return base.sm.isYellowAlert.Get(base.smi);
		}

		// Token: 0x06008CA9 RID: 36009 RVA: 0x00319869 File Offset: 0x00317A69
		public bool IsRedAlertToggledOn()
		{
			return this.isToggled;
		}

		// Token: 0x06008CAA RID: 36010 RVA: 0x00319871 File Offset: 0x00317A71
		public void ToggleRedAlert(bool on)
		{
			this.isToggled = on;
			this.Refresh();
		}

		// Token: 0x06008CAB RID: 36011 RVA: 0x00319880 File Offset: 0x00317A80
		public void HasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x06008CAC RID: 36012 RVA: 0x00319890 File Offset: 0x00317A90
		private void Refresh()
		{
			base.sm.isYellowAlert.Set(this.hasTopPriorityChore, base.smi, false);
			base.sm.isRedAlert.Set(this.isToggled, base.smi, false);
			base.sm.isOn.Set(this.hasTopPriorityChore || this.isToggled, base.smi, false);
		}

		// Token: 0x04006D14 RID: 27924
		private static VignetteManager.Instance instance;

		// Token: 0x04006D15 RID: 27925
		private bool isToggled;

		// Token: 0x04006D16 RID: 27926
		private bool hasTopPriorityChore;

		// Token: 0x04006D17 RID: 27927
		public Notification redAlertNotification = new Notification(MISC.NOTIFICATIONS.REDALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REDALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
