using System;
using UnityEngine;

// Token: 0x020005CD RID: 1485
[AddComponentMenu("KMonoBehaviour/Workable/CommandModuleWorkable")]
public class CommandModuleWorkable : Workable
{
	// Token: 0x060024A2 RID: 9378 RVA: 0x000C8284 File Offset: 0x000C6484
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsets(CommandModuleWorkable.entryOffsets);
		this.synchronizeAnims = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_incubator_kanim")
		};
		base.SetWorkTime(float.PositiveInfinity);
		this.showProgressBar = false;
	}

	// Token: 0x060024A3 RID: 9379 RVA: 0x000C82D9 File Offset: 0x000C64D9
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x060024A4 RID: 9380 RVA: 0x000C82E4 File Offset: 0x000C64E4
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (!(worker != null))
		{
			return base.OnWorkTick(worker, dt);
		}
		if (DlcManager.IsExpansion1Active())
		{
			GameObject gameObject = worker.gameObject;
			base.CompleteWork(worker);
			base.GetComponent<ClustercraftExteriorDoor>().FerryMinion(gameObject);
			return true;
		}
		GameObject gameObject2 = worker.gameObject;
		base.CompleteWork(worker);
		base.GetComponent<MinionStorage>().SerializeMinion(gameObject2);
		return true;
	}

	// Token: 0x060024A5 RID: 9381 RVA: 0x000C8341 File Offset: 0x000C6541
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
	}

	// Token: 0x060024A6 RID: 9382 RVA: 0x000C834A File Offset: 0x000C654A
	protected override void OnCompleteWork(Worker worker)
	{
	}

	// Token: 0x04001501 RID: 5377
	private static CellOffset[] entryOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(0, 2),
		new CellOffset(0, 3),
		new CellOffset(0, 4)
	};
}
