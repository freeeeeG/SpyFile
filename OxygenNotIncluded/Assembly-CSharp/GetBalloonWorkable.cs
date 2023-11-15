using System;
using Database;
using UnityEngine;

// Token: 0x0200060E RID: 1550
[AddComponentMenu("KMonoBehaviour/Workable/GetBalloonWorkable")]
public class GetBalloonWorkable : Workable
{
	// Token: 0x06002700 RID: 9984 RVA: 0x000D3D80 File Offset: 0x000D1F80
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = null;
		this.workingStatusItem = null;
		this.workAnims = GetBalloonWorkable.GET_BALLOON_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			GetBalloonWorkable.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			GetBalloonWorkable.PST_ANIM
		};
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x000D3DE4 File Offset: 0x000D1FE4
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
		if (balloonOverride.animFile.IsNone())
		{
			worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"), 0);
			return;
		}
		worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", balloonOverride.symbol.Unwrap(), 0);
	}

	// Token: 0x06002702 RID: 9986 RVA: 0x000D3E80 File Offset: 0x000D2080
	protected override void OnCompleteWork(Worker worker)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EquippableBalloon"), worker.transform.GetPosition());
		gameObject.GetComponent<Equippable>().Assign(worker.GetComponent<MinionIdentity>());
		gameObject.GetComponent<Equippable>().isEquipped = true;
		gameObject.SetActive(true);
		base.OnCompleteWork(worker);
		BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
		this.balloonArtist.GiveBalloon(balloonOverride);
		gameObject.GetComponent<EquippableBalloon>().SetBalloonOverride(balloonOverride);
	}

	// Token: 0x06002703 RID: 9987 RVA: 0x000D3EFA File Offset: 0x000D20FA
	public override Vector3 GetFacingTarget()
	{
		return this.balloonArtist.master.transform.GetPosition();
	}

	// Token: 0x06002704 RID: 9988 RVA: 0x000D3F11 File Offset: 0x000D2111
	public void SetBalloonArtist(BalloonArtistChore.StatesInstance chore)
	{
		this.balloonArtist = chore;
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x000D3F1A File Offset: 0x000D211A
	public BalloonArtistChore.StatesInstance GetBalloonArtist()
	{
		return this.balloonArtist;
	}

	// Token: 0x0400165C RID: 5724
	private static readonly HashedString[] GET_BALLOON_ANIMS = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x0400165D RID: 5725
	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	// Token: 0x0400165E RID: 5726
	private BalloonArtistChore.StatesInstance balloonArtist;

	// Token: 0x0400165F RID: 5727
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x04001660 RID: 5728
	private const int TARGET_OVERRIDE_PRIORITY = 0;
}
