using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008A4 RID: 2212
public class YellowAlertManager : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance>
{
	// Token: 0x06004016 RID: 16406 RVA: 0x00166D98 File Offset: 0x00164F98
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.IsTrue);
		this.on.Enter("EnterEvent", delegate(YellowAlertManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(YellowAlertManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("EnableVignette", delegate(YellowAlertManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 1f, 0f, 0.1f));
		}).Exit("DisableVignette", delegate(YellowAlertManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).Enter("Sounds", delegate(YellowAlertManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("RedAlert_LP", false), null, true, true, true).ToggleNotification((YellowAlertManager.Instance smi) => smi.notification).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.IsFalse);
		this.on_pst.Enter("Sounds", delegate(YellowAlertManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		});
	}

	// Token: 0x040029BF RID: 10687
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040029C0 RID: 10688
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x040029C1 RID: 10689
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State on_pst;

	// Token: 0x040029C2 RID: 10690
	public StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.BoolParameter isOn = new StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x020016DB RID: 5851
	public new class Instance : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008CC9 RID: 36041 RVA: 0x00319E37 File Offset: 0x00318037
		public static void DestroyInstance()
		{
			YellowAlertManager.Instance.instance = null;
		}

		// Token: 0x06008CCA RID: 36042 RVA: 0x00319E3F File Offset: 0x0031803F
		public static YellowAlertManager.Instance Get()
		{
			return YellowAlertManager.Instance.instance;
		}

		// Token: 0x06008CCB RID: 36043 RVA: 0x00319E48 File Offset: 0x00318048
		public Instance(IStateMachineTarget master) : base(master)
		{
			YellowAlertManager.Instance.instance = this;
		}

		// Token: 0x06008CCC RID: 36044 RVA: 0x00319EA4 File Offset: 0x003180A4
		public bool IsOn()
		{
			return base.sm.isOn.Get(base.smi);
		}

		// Token: 0x06008CCD RID: 36045 RVA: 0x00319EBC File Offset: 0x003180BC
		public void HasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x06008CCE RID: 36046 RVA: 0x00319ECB File Offset: 0x003180CB
		private void Refresh()
		{
			base.sm.isOn.Set(this.hasTopPriorityChore, base.smi, false);
		}

		// Token: 0x04006D2F RID: 27951
		private static YellowAlertManager.Instance instance;

		// Token: 0x04006D30 RID: 27952
		private bool hasTopPriorityChore;

		// Token: 0x04006D31 RID: 27953
		public Notification notification = new Notification(MISC.NOTIFICATIONS.YELLOWALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.YELLOWALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
