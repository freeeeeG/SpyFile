using System;
using STRINGS;
using UnityEngine;

// Token: 0x020005BA RID: 1466
[AddComponentMenu("KMonoBehaviour/Workable/AstronautTrainingCenter")]
public class AstronautTrainingCenter : Workable
{
	// Token: 0x060023E2 RID: 9186 RVA: 0x000C463D File Offset: 0x000C283D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.chore = this.CreateChore();
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x000C4654 File Offset: 0x000C2854
	private Chore CreateChore()
	{
		return new WorkChore<AstronautTrainingCenter>(Db.Get().ChoreTypes.Train, this, null, true, null, null, null, false, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x060023E4 RID: 9188 RVA: 0x000C4687 File Offset: 0x000C2887
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<Operational>().SetActive(true, false);
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x000C469D File Offset: 0x000C289D
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		worker == null;
		return true;
	}

	// Token: 0x060023E6 RID: 9190 RVA: 0x000C46A8 File Offset: 0x000C28A8
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		if (this.chore != null && !this.chore.isComplete)
		{
			this.chore.Cancel("completed but not complete??");
		}
		this.chore = this.CreateChore();
	}

	// Token: 0x060023E7 RID: 9191 RVA: 0x000C46E2 File Offset: 0x000C28E2
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<Operational>().SetActive(false, false);
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x000C46F8 File Offset: 0x000C28F8
	public override float GetPercentComplete()
	{
		base.worker == null;
		return 0f;
	}

	// Token: 0x060023E9 RID: 9193 RVA: 0x000C470C File Offset: 0x000C290C
	public AstronautTrainingCenter()
	{
		Chore.Precondition isNotMarkedForDeconstruction = default(Chore.Precondition);
		isNotMarkedForDeconstruction.id = "IsNotMarkedForDeconstruction";
		isNotMarkedForDeconstruction.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION;
		isNotMarkedForDeconstruction.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Deconstructable deconstructable = data as Deconstructable;
			return deconstructable == null || !deconstructable.IsMarkedForDeconstruction();
		};
		this.IsNotMarkedForDeconstruction = isNotMarkedForDeconstruction;
		base..ctor();
	}

	// Token: 0x0400148C RID: 5260
	public float daysToMasterRole;

	// Token: 0x0400148D RID: 5261
	private Chore chore;

	// Token: 0x0400148E RID: 5262
	public Chore.Precondition IsNotMarkedForDeconstruction;
}
