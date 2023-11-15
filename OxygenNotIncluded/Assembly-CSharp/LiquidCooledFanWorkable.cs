using System;
using UnityEngine;

// Token: 0x02000622 RID: 1570
[AddComponentMenu("KMonoBehaviour/Workable/LiquidCooledFanWorkable")]
public class LiquidCooledFanWorkable : Workable
{
	// Token: 0x060027A8 RID: 10152 RVA: 0x000D7253 File Offset: 0x000D5453
	private LiquidCooledFanWorkable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x000D7262 File Offset: 0x000D5462
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = null;
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x000D7271 File Offset: 0x000D5471
	protected override void OnSpawn()
	{
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		base.OnSpawn();
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x000D72AF File Offset: 0x000D54AF
	protected override void OnStartWork(Worker worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x060027AC RID: 10156 RVA: 0x000D72BE File Offset: 0x000D54BE
	protected override void OnStopWork(Worker worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x060027AD RID: 10157 RVA: 0x000D72CD File Offset: 0x000D54CD
	protected override void OnCompleteWork(Worker worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x040016FA RID: 5882
	[MyCmpGet]
	private Operational operational;
}
