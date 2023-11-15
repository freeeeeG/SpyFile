using System;
using UnityEngine;

// Token: 0x020003D2 RID: 978
public class Chore<StateMachineInstanceType> : Chore, IStateMachineTarget where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600144E RID: 5198 RVA: 0x0006BD4D File Offset: 0x00069F4D
	// (set) Token: 0x0600144F RID: 5199 RVA: 0x0006BD55 File Offset: 0x00069F55
	public StateMachineInstanceType smi { get; protected set; }

	// Token: 0x06001450 RID: 5200 RVA: 0x0006BD5E File Offset: 0x00069F5E
	protected override StateMachine.Instance GetSMI()
	{
		return this.smi;
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x0006BD6B File Offset: 0x00069F6B
	public int Subscribe(int hash, Action<object> handler)
	{
		return this.GetComponent<KPrefabID>().Subscribe(hash, handler);
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x0006BD7A File Offset: 0x00069F7A
	public void Unsubscribe(int hash, Action<object> handler)
	{
		this.GetComponent<KPrefabID>().Unsubscribe(hash, handler);
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x0006BD89 File Offset: 0x00069F89
	public void Unsubscribe(int id)
	{
		this.GetComponent<KPrefabID>().Unsubscribe(id);
	}

	// Token: 0x06001454 RID: 5204 RVA: 0x0006BD97 File Offset: 0x00069F97
	public void Trigger(int hash, object data = null)
	{
		this.GetComponent<KPrefabID>().Trigger(hash, data);
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x0006BDA6 File Offset: 0x00069FA6
	public ComponentType GetComponent<ComponentType>()
	{
		return base.target.GetComponent<ComponentType>();
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x06001456 RID: 5206 RVA: 0x0006BDB3 File Offset: 0x00069FB3
	public override GameObject gameObject
	{
		get
		{
			return base.target.gameObject;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06001457 RID: 5207 RVA: 0x0006BDC0 File Offset: 0x00069FC0
	public Transform transform
	{
		get
		{
			return base.target.gameObject.transform;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06001458 RID: 5208 RVA: 0x0006BDD2 File Offset: 0x00069FD2
	public string name
	{
		get
		{
			return this.gameObject.name;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06001459 RID: 5209 RVA: 0x0006BDDF File Offset: 0x00069FDF
	public override bool isNull
	{
		get
		{
			return base.target.isNull;
		}
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x0006BDEC File Offset: 0x00069FEC
	public Chore(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, PriorityScreen.PriorityClass master_priority_class = PriorityScreen.PriorityClass.basic, int master_priority_value = 5, bool is_preemptable = false, bool allow_in_context_menu = true, int priority_mod = 0, bool add_to_daily_report = false, ReportManager.ReportType report_type = ReportManager.ReportType.WorkTime) : base(chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, master_priority_class, master_priority_value, is_preemptable, allow_in_context_menu, priority_mod, add_to_daily_report, report_type)
	{
		target.Subscribe(1969584890, new Action<object>(this.OnTargetDestroyed));
		this.reportType = report_type;
		this.addToDailyReport = add_to_daily_report;
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, chore_type.Name, GameUtil.GetChoreName(this, null));
		}
	}

	// Token: 0x0600145B RID: 5211 RVA: 0x0006BE65 File Offset: 0x0006A065
	public override string ResolveString(string str)
	{
		if (!base.target.isNull)
		{
			str = str.Replace("{Target}", base.target.gameObject.GetProperName());
		}
		return base.ResolveString(str);
	}

	// Token: 0x0600145C RID: 5212 RVA: 0x0006BE98 File Offset: 0x0006A098
	public override void Cleanup()
	{
		base.Cleanup();
		if (base.target != null)
		{
			base.target.Unsubscribe(1969584890, new Action<object>(this.OnTargetDestroyed));
		}
		if (this.onCleanup != null)
		{
			this.onCleanup(this);
		}
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x0006BED8 File Offset: 0x0006A0D8
	private void OnTargetDestroyed(object data)
	{
		base.Cancel("Target Destroyed");
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x0006BEE5 File Offset: 0x0006A0E5
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		return base.CanPreempt(context);
	}
}
