using System;
using GameResources;
using Scenes;
using UnityEngine;

namespace UI.SkulStories
{
	// Token: 0x0200045E RID: 1118
	public class Confirm : MonoBehaviour
	{
		// Token: 0x0600153E RID: 5438 RVA: 0x00042D58 File Offset: 0x00040F58
		public void Open()
		{
			UIManager uiManager = Scene<GameBase>.instance.uiManager;
			uiManager.confirm.Open(Localization.GetLocalizedString("label/skulstory/skipConfirm"), delegate()
			{
				uiManager.narration.sceneVisible = false;
			});
		}

		// Token: 0x04001293 RID: 4755
		private const string _labelName = "label/skulstory/skipConfirm";
	}
}
