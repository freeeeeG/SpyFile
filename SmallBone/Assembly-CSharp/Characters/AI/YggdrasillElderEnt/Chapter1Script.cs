using System;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x02001130 RID: 4400
	public class Chapter1Script : MonoBehaviour
	{
		// Token: 0x06005593 RID: 21907 RVA: 0x000FF518 File Offset: 0x000FD718
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			LetterBox.instance.Disappear(0.4f);
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
			if (Scene<GameBase>.instance.cameraController == null || Singleton<Service>.Instance.levelManager.player == null)
			{
				return;
			}
			Scene<GameBase>.instance.cameraController.StartTrack(Singleton<Service>.Instance.levelManager.player.transform);
		}
	}
}
