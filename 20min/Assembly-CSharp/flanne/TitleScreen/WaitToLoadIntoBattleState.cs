using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace flanne.TitleScreen
{
	// Token: 0x020001E9 RID: 489
	public class WaitToLoadIntoBattleState : TitleScreenState
	{
		// Token: 0x06000AB5 RID: 2741 RVA: 0x000297F3 File Offset: 0x000279F3
		public override void Enter()
		{
			base.StartCoroutine(this.WaitToLoadCR());
			AudioManager.Instance.FadeOutMusic(1f);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00029811 File Offset: 0x00027A11
		private IEnumerator WaitToLoadCR()
		{
			yield return new WaitForSeconds(1f);
			SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
			yield break;
		}
	}
}
