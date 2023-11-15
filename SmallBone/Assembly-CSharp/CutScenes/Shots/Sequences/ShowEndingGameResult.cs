using System;
using System.Collections;
using Scenes;
using UserInput;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001CF RID: 463
	public class ShowEndingGameResult : Sequence
	{
		// Token: 0x060009A2 RID: 2466 RVA: 0x0001B4A0 File Offset: 0x000196A0
		public override IEnumerator CRun()
		{
			GameBase gameBase = Scene<GameBase>.instance;
			gameBase.uiManager.gameResult.ShowEndingResult();
			while (!gameBase.uiManager.gameResult.animationFinished || (!KeyMapper.Map.Attack.WasPressed && !KeyMapper.Map.Submit.WasPressed))
			{
				yield return null;
			}
			this.Hide();
			yield break;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001B4AF File Offset: 0x000196AF
		private void Hide()
		{
			Scene<GameBase>.instance.uiManager.gameResult.Hide();
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001B4C5 File Offset: 0x000196C5
		private void OnDisable()
		{
			this.Hide();
		}
	}
}
