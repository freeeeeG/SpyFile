using System;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02000B49 RID: 2889
public class KleiPermitDioramaVis_JoyResponseBalloon : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06005910 RID: 22800 RVA: 0x00209A51 File Offset: 0x00207C51
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06005911 RID: 22801 RVA: 0x00209A5C File Offset: 0x00207C5C
	public void ConfigureSetup()
	{
		this.minionUI.transform.localScale = Vector3.one * 0.7f;
		this.minionUI.transform.localPosition = new Vector3(this.minionUI.transform.localPosition.x - 73f, this.minionUI.transform.localPosition.y - 152f + 8f, this.minionUI.transform.localPosition.z);
	}

	// Token: 0x06005912 RID: 22802 RVA: 0x00209AEE File Offset: 0x00207CEE
	public void ConfigureWith(PermitResource permit)
	{
		this.ConfigureWith(Option.Some<BalloonArtistFacadeResource>((BalloonArtistFacadeResource)permit));
	}

	// Token: 0x06005913 RID: 22803 RVA: 0x00209B04 File Offset: 0x00207D04
	public void ConfigureWith(Option<BalloonArtistFacadeResource> permit)
	{
		KleiPermitDioramaVis_JoyResponseBalloon.<>c__DisplayClass10_0 CS$<>8__locals1 = new KleiPermitDioramaVis_JoyResponseBalloon.<>c__DisplayClass10_0();
		CS$<>8__locals1.permit = permit;
		KBatchedAnimController component = this.minionUI.SpawnedAvatar.GetComponent<KBatchedAnimController>();
		CS$<>8__locals1.minionSymbolOverrider = this.minionUI.SpawnedAvatar.GetComponent<SymbolOverrideController>();
		this.minionUI.SetMinion(this.specificPersonality.UnwrapOrElse(() => (from p in Db.Get().Personalities.GetAll(true, true)
		where p.joyTrait == "BalloonArtist"
		select p).GetRandom<Personality>(), null));
		if (!this.didAddAnims)
		{
			this.didAddAnims = true;
			component.AddAnimOverrides(Assets.GetAnim("anim_interacts_balloon_artist_kanim"), 0f);
		}
		component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		CS$<>8__locals1.<ConfigureWith>g__DisplayNextBalloon|3();
		Updater[] array = new Updater[2];
		array[0] = Updater.WaitForSeconds(1.3f);
		int num = 1;
		Func<Updater>[] array2 = new Func<Updater>[2];
		array2[0] = (() => Updater.WaitForSeconds(1.618f));
		array2[1] = (() => Updater.Do(new System.Action(base.<ConfigureWith>g__DisplayNextBalloon|3)));
		array[num] = Updater.Loop(array2);
		this.QueueUpdater(Updater.Series(array));
	}

	// Token: 0x06005914 RID: 22804 RVA: 0x00209C45 File Offset: 0x00207E45
	public void SetMinion(Personality personality)
	{
		this.specificPersonality = personality;
		if (base.gameObject.activeInHierarchy)
		{
			this.minionUI.SetMinion(personality);
		}
	}

	// Token: 0x06005915 RID: 22805 RVA: 0x00209C6C File Offset: 0x00207E6C
	private void QueueUpdater(Updater updater)
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.RunUpdater(updater);
			return;
		}
		this.updaterToRunOnStart = updater;
	}

	// Token: 0x06005916 RID: 22806 RVA: 0x00209C8F File Offset: 0x00207E8F
	private void RunUpdater(Updater updater)
	{
		if (this.updaterRoutine != null)
		{
			base.StopCoroutine(this.updaterRoutine);
			this.updaterRoutine = null;
		}
		this.updaterRoutine = base.StartCoroutine(updater);
	}

	// Token: 0x06005917 RID: 22807 RVA: 0x00209CBE File Offset: 0x00207EBE
	private void OnEnable()
	{
		if (this.updaterToRunOnStart.IsSome())
		{
			this.RunUpdater(this.updaterToRunOnStart.Unwrap());
			this.updaterToRunOnStart = Option.None;
		}
	}

	// Token: 0x04003C3D RID: 15421
	private const int FRAMES_TO_MAKE_BALLOON_IN_ANIM = 39;

	// Token: 0x04003C3E RID: 15422
	private const float SECONDS_TO_MAKE_BALLOON_IN_ANIM = 1.3f;

	// Token: 0x04003C3F RID: 15423
	private const float SECONDS_BETWEEN_BALLOONS = 1.618f;

	// Token: 0x04003C40 RID: 15424
	[SerializeField]
	private UIMinion minionUI;

	// Token: 0x04003C41 RID: 15425
	private bool didAddAnims;

	// Token: 0x04003C42 RID: 15426
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x04003C43 RID: 15427
	private const int TARGET_OVERRIDE_PRIORITY = 0;

	// Token: 0x04003C44 RID: 15428
	private Option<Personality> specificPersonality;

	// Token: 0x04003C45 RID: 15429
	private Option<PermitResource> lastConfiguredPermit;

	// Token: 0x04003C46 RID: 15430
	private Option<Updater> updaterToRunOnStart;

	// Token: 0x04003C47 RID: 15431
	private Coroutine updaterRoutine;
}
