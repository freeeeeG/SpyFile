using System;
using System.Collections;
using UnityEngine;

namespace flanne.TitleScreen
{
	// Token: 0x020001E1 RID: 481
	public class InitState : TitleScreenState
	{
		// Token: 0x06000A8E RID: 2702 RVA: 0x000290D0 File Offset: 0x000272D0
		public override void Enter()
		{
			base.StartCoroutine(this.WaitToLoadCR());
			AudioManager.Instance.PlayMusic(base.titleScreenMusic);
			AudioManager.Instance.FadeInMusic(5f);
			SaveSystem.Load();
			base.characterUnlocker.LoadData(SaveSystem.data.characterUnlocks);
			base.gunUnlocker.LoadData(SaveSystem.data.gunUnlocks);
			base.runeUnlocker.LoadData(SaveSystem.data.runeUnlocks);
			base.characterVictories.SetProperties(SaveSystem.data.characterHighestClear);
			base.gunVictories.SetProperties(SaveSystem.data.gunHighestClear);
			PointsTracker.pts = SaveSystem.data.points;
			base.swordRuneTree.SetSelections(SaveSystem.data.swordRuneSelections);
			base.shieldRuneTree.SetSelections(SaveSystem.data.shieldRuneSelections);
			base.difficultyController.Init(Mathf.Clamp(SaveSystem.data.difficultyUnlocked, 0, 15));
			base.templeUnlocker.CheckUnlock(Mathf.Clamp(SaveSystem.data.difficultyUnlocked, 0, 15));
			base.pumpkinPatchUnlocker.CheckUnlock(Mathf.Clamp(SaveSystem.data.difficultyUnlocked, 0, 15));
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00029207 File Offset: 0x00027407
		private IEnumerator WaitToLoadCR()
		{
			yield return new WaitForSeconds(0.5f);
			base.screenCover.enabled = false;
			this.owner.ChangeState<TitleMainMenuState>();
			yield break;
		}
	}
}
