using System;
using Database;
using UnityEngine;

// Token: 0x02000976 RID: 2422
public class SkyVisibilityMonitor : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>
{
	// Token: 0x06004756 RID: 18262 RVA: 0x00193121 File Offset: 0x00191321
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<SkyVisibilityMonitor.Instance, float>(SkyVisibilityMonitor.CheckSkyVisibility), UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06004757 RID: 18263 RVA: 0x00193148 File Offset: 0x00191348
	public static void CheckSkyVisibility(SkyVisibilityMonitor.Instance smi, float dt)
	{
		bool hasSkyVisibility = smi.HasSkyVisibility;
		ValueTuple<bool, float> visibilityOf = smi.def.skyVisibilityInfo.GetVisibilityOf(smi.gameObject);
		bool item = visibilityOf.Item1;
		float item2 = visibilityOf.Item2;
		smi.Internal_SetPercentClearSky(item2);
		KSelectable component = smi.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !item, smi);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, item && item2 < 1f, smi);
		if (hasSkyVisibility == item)
		{
			return;
		}
		smi.TriggerVisibilityChange();
	}

	// Token: 0x020017DC RID: 6108
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007037 RID: 28727
		public SkyVisibilityInfo skyVisibilityInfo;
	}

	// Token: 0x020017DD RID: 6109
	public new class Instance : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>.GameInstance, BuildingStatusItems.ISkyVisInfo
	{
		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06008F94 RID: 36756 RVA: 0x00322FFB File Offset: 0x003211FB
		public bool HasSkyVisibility
		{
			get
			{
				return this.PercentClearSky > 0f && !Mathf.Approximately(0f, this.PercentClearSky);
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06008F95 RID: 36757 RVA: 0x0032301F File Offset: 0x0032121F
		public float PercentClearSky
		{
			get
			{
				return this.percentClearSky01;
			}
		}

		// Token: 0x06008F96 RID: 36758 RVA: 0x00323027 File Offset: 0x00321227
		public void Internal_SetPercentClearSky(float percent01)
		{
			this.percentClearSky01 = percent01;
		}

		// Token: 0x06008F97 RID: 36759 RVA: 0x00323030 File Offset: 0x00321230
		float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
		{
			return this.percentClearSky01;
		}

		// Token: 0x06008F98 RID: 36760 RVA: 0x00323038 File Offset: 0x00321238
		public Instance(IStateMachineTarget master, SkyVisibilityMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008F99 RID: 36761 RVA: 0x00323042 File Offset: 0x00321242
		public override void StartSM()
		{
			base.StartSM();
			SkyVisibilityMonitor.CheckSkyVisibility(this, 0f);
			this.TriggerVisibilityChange();
		}

		// Token: 0x06008F9A RID: 36762 RVA: 0x0032305C File Offset: 0x0032125C
		public void TriggerVisibilityChange()
		{
			if (this.visibilityStatusItem != null)
			{
				base.smi.GetComponent<KSelectable>().ToggleStatusItem(this.visibilityStatusItem, !this.HasSkyVisibility, this);
			}
			base.smi.GetComponent<Operational>().SetFlag(SkyVisibilityMonitor.Instance.skyVisibilityFlag, this.HasSkyVisibility);
			if (this.SkyVisibilityChanged != null)
			{
				this.SkyVisibilityChanged();
			}
		}

		// Token: 0x04007038 RID: 28728
		private float percentClearSky01;

		// Token: 0x04007039 RID: 28729
		public System.Action SkyVisibilityChanged;

		// Token: 0x0400703A RID: 28730
		private StatusItem visibilityStatusItem;

		// Token: 0x0400703B RID: 28731
		private static readonly Operational.Flag skyVisibilityFlag = new Operational.Flag("sky visibility", Operational.Flag.Type.Requirement);
	}
}
