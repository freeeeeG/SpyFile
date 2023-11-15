using System;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pause
{
	// Token: 0x0200041C RID: 1052
	public class Menu : Dialogue
	{
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x00018EC5 File Offset: 0x000170C5
		public override bool closeWithPauseKey
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0003CFB4 File Offset: 0x0003B1B4
		private void Awake()
		{
			this._continue.onClick.AddListener(delegate
			{
				this._panel.Close();
			});
			this._newGame.onClick.AddListener(delegate
			{
				Scene<GameBase>.instance.uiManager.confirm.Open(Localization.GetLocalizedString("label/pause/menu/newGame/confirm"), delegate()
				{
					Singleton<Service>.Instance.fadeInOut.FadeOutImmediately();
					this._panel.gameObject.SetActive(false);
					GameData.Generic.tutorial.Stop();
					Singleton<Service>.Instance.levelManager.ResetGame();
				});
			});
			this._controls.onClick.AddListener(delegate
			{
				this._panel.state = Panel.State.Controls;
			});
			this._settings.onClick.AddListener(delegate
			{
				this._panel.state = Panel.State.Settings;
			});
			this._quit.onClick.AddListener(delegate
			{
				Scene<GameBase>.instance.uiManager.confirm.Open(Localization.GetLocalizedString("label/pause/menu/quit/confirm"), new Action(Application.Quit));
			});
		}

		// Token: 0x040010EE RID: 4334
		[SerializeField]
		private Panel _panel;

		// Token: 0x040010EF RID: 4335
		[SerializeField]
		private Button _continue;

		// Token: 0x040010F0 RID: 4336
		[SerializeField]
		private Button _newGame;

		// Token: 0x040010F1 RID: 4337
		[SerializeField]
		private Button _controls;

		// Token: 0x040010F2 RID: 4338
		[SerializeField]
		private Button _settings;

		// Token: 0x040010F3 RID: 4339
		[SerializeField]
		private Button _quit;
	}
}
