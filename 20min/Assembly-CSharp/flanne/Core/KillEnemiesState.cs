using System;
using System.Collections;
using UnityEngine;

namespace flanne.Core
{
	// Token: 0x020001F4 RID: 500
	public class KillEnemiesState : GameState
	{
		// Token: 0x06000B44 RID: 2884 RVA: 0x0002A5A0 File Offset: 0x000287A0
		public override void Enter()
		{
			base.StartCoroutine(this.KillEnemiesCR());
			GameTimer.SharedInstance.Stop();
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00002F51 File Offset: 0x00001151
		public override void Exit()
		{
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0002A5B9 File Offset: 0x000287B9
		private IEnumerator KillEnemiesCR()
		{
			base.screenFlash.Flash(1);
			LeanTween.scale(base.playerFogRevealer, new Vector3(5f, 5f, 1f), 0.4f).setEase(LeanTweenType.easeInCubic);
			yield return new WaitForSeconds(0.3f);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
			for (int i = 0; i < array.Length; i++)
			{
				Health component = array[i].GetComponent<Health>();
				if (component != null)
				{
					component.AutoKill(true);
				}
			}
			base.screenFlash.Flash(4);
			yield return new WaitForSeconds(1.5f);
			PauseController.SharedInstance.Pause();
			AudioManager.Instance.FadeOutMusic(0.5f);
			yield return new WaitForSecondsRealtime(0.5f);
			MapData mapData = SelectedMap.MapData;
			if (SaveSystem.data != null && !SaveSystem.data.characterUnlocks.unlocks[8] && mapData.name == "20M_Temple")
			{
				this.owner.ChangeState<HasturUnlockedState>();
			}
			else if (SaveSystem.data != null && !SaveSystem.data.characterUnlocks.unlocks[9] && mapData.name == "20M_PumpkinPatch")
			{
				this.owner.ChangeState<RavenUnlockedState>();
			}
			else
			{
				this.owner.ChangeState<PlayerSurvivedState>();
			}
			yield break;
		}
	}
}
