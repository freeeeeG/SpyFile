using System;
using System.Collections;
using UnityEngine;

namespace flanne.Core
{
	// Token: 0x020001F3 RID: 499
	public class InitState : GameState
	{
		// Token: 0x06000B40 RID: 2880 RVA: 0x0002A52C File Offset: 0x0002872C
		public override void Enter()
		{
			PauseController.SharedInstance.Pause();
			base.StartCoroutine(this.WaitToShowStartBattle());
			AudioManager.Instance.SetLowPassFilter(true);
			AudioManager.Instance.PlayMusic(base.battleMusic);
			AudioManager.Instance.FadeInMusic(0.5f);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0002A57A File Offset: 0x0002877A
		public override void Exit()
		{
			PauseController.SharedInstance.UnPause();
			AudioManager.Instance.SetLowPassFilter(false);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0002A591 File Offset: 0x00028791
		private IEnumerator WaitToShowStartBattle()
		{
			yield return new WaitForSecondsRealtime(0.5f);
			this.fogRevealTweenID = LeanTween.scale(base.playerFogRevealer, new Vector3(0.5f, 0.5f, 1f), 0.5f).setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true).id;
			while (LeanTween.isTweening(this.fogRevealTweenID))
			{
				yield return null;
			}
			this.fogRevealTweenID = LeanTween.scale(base.playerFogRevealer, Vector3.one, 0.5f).setEase(LeanTweenType.easeInOutCubic).setIgnoreTimeScale(true).id;
			while (LeanTween.isTweening(this.fogRevealTweenID))
			{
				yield return null;
			}
			base.hud.Show();
			this.owner.ChangeState<CombatState>();
			yield break;
		}

		// Token: 0x040007E5 RID: 2021
		private int fogRevealTweenID;
	}
}
