using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000262 RID: 610
public class BalloonStandConfig : IEntityConfig
{
	// Token: 0x06000C4B RID: 3147 RVA: 0x00045AAC File Offset: 0x00043CAC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x00045AB4 File Offset: 0x00043CB4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(BalloonStandConfig.ID, BalloonStandConfig.ID, false);
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_balloon_receiver_kanim")
		};
		GetBalloonWorkable getBalloonWorkable = gameObject.AddOrGet<GetBalloonWorkable>();
		getBalloonWorkable.workTime = 2f;
		getBalloonWorkable.workLayer = Grid.SceneLayer.BuildingFront;
		getBalloonWorkable.overrideAnims = overrideAnims;
		getBalloonWorkable.synchronizeAnims = false;
		return gameObject;
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x00045B12 File Offset: 0x00043D12
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x00045B14 File Offset: 0x00043D14
	public void OnSpawn(GameObject inst)
	{
		GetBalloonWorkable component = inst.GetComponent<GetBalloonWorkable>();
		WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, component, null, true, new Action<Chore>(this.MakeNewBalloonChore), null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, true, true);
		workChore.AddPrecondition(this.HasNoBalloon, workChore);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, workChore);
		component.GetBalloonArtist().NextBalloonOverride();
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x00045B94 File Offset: 0x00043D94
	private void MakeNewBalloonChore(Chore chore)
	{
		GetBalloonWorkable component = chore.target.GetComponent<GetBalloonWorkable>();
		WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, component, null, true, new Action<Chore>(this.MakeNewBalloonChore), null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, true, true);
		workChore.AddPrecondition(this.HasNoBalloon, workChore);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, workChore);
		component.GetBalloonArtist().NextBalloonOverride();
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00045C18 File Offset: 0x00043E18
	public BalloonStandConfig()
	{
		Chore.Precondition hasNoBalloon = default(Chore.Precondition);
		hasNoBalloon.id = "HasNoBalloon";
		hasNoBalloon.description = "__ Duplicant doesn't have a balloon already";
		hasNoBalloon.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !context.consumerState.gameObject.GetComponent<Effects>().HasEffect("HasBalloon");
		};
		this.HasNoBalloon = hasNoBalloon;
		base..ctor();
	}

	// Token: 0x0400074E RID: 1870
	public static readonly string ID = "BalloonStand";

	// Token: 0x0400074F RID: 1871
	private Chore.Precondition HasNoBalloon;
}
