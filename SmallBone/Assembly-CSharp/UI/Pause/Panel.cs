using System;
using Characters.Controllers;
using Data;
using FX;
using Scenes;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.Pause
{
	// Token: 0x0200041E RID: 1054
	public class Panel : Dialogue
	{
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x0003D128 File Offset: 0x0003B328
		// (set) Token: 0x060013FB RID: 5115 RVA: 0x0003D130 File Offset: 0x0003B330
		public Panel.State state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._statePanels[this._state].Close();
				this._statePanels[value].Open();
				this._state = value;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x00018EC5 File Offset: 0x000170C5
		public override bool closeWithPauseKey
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0003D160 File Offset: 0x0003B360
		private void Awake()
		{
			this._statePanels = new EnumArray<Panel.State, Dialogue>(new Dialogue[]
			{
				this._menu,
				this._controls,
				this._settings
			});
			Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.AddComponent<PlaySoundOnSelected>().soundInfo = this._selectSound;
			}
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0003D1C7 File Offset: 0x0003B3C7
		protected override void OnEnable()
		{
			base.OnEnable();
			this.state = Panel.State.Menu;
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._openSound, Vector3.zero);
			PlayerInput.blocked.Attach(this);
			Chronometer.global.AttachTimeScale(this, 0f);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0003D208 File Offset: 0x0003B408
		protected override void OnDisable()
		{
			if (Service.quitting)
			{
				return;
			}
			base.OnDisable();
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._closeSound, Vector3.zero);
			this._statePanels[this._state].Close();
			PlayerInput.blocked.Detach(this);
			Chronometer.global.DetachTimeScale(this);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0003D267 File Offset: 0x0003B467
		protected override void Update()
		{
			base.Update();
			if (KeyMapper.Map.Pause.WasPressed || KeyMapper.Map.Cancel.WasPressed)
			{
				this.Return();
			}
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0003D298 File Offset: 0x0003B498
		public void Return()
		{
			if (!base.gameObject.activeSelf)
			{
				if (!Scene<GameBase>.instance.uiManager.npcConversation.visible && !Scene<GameBase>.instance.uiManager.testingTool.gameObject.activeSelf)
				{
					base.Open();
				}
				return;
			}
			if (!this._menu.focused && !this._settings.focused && !this._controls.focused)
			{
				return;
			}
			if (this.state == Panel.State.Menu)
			{
				base.Close();
				return;
			}
			this.state = Panel.State.Menu;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0003D328 File Offset: 0x0003B528
		public void ReturnToTitleScreen()
		{
			GameData.Generic.tutorial.Stop();
			PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
			Singleton<Service>.Instance.ResetGameScene();
			base.gameObject.SetActive(false);
		}

		// Token: 0x040010F6 RID: 4342
		[SerializeField]
		private SoundInfo _openSound;

		// Token: 0x040010F7 RID: 4343
		[SerializeField]
		private SoundInfo _closeSound;

		// Token: 0x040010F8 RID: 4344
		[SerializeField]
		private SoundInfo _selectSound;

		// Token: 0x040010F9 RID: 4345
		[SerializeField]
		private Menu _menu;

		// Token: 0x040010FA RID: 4346
		[SerializeField]
		private Controls _controls;

		// Token: 0x040010FB RID: 4347
		[SerializeField]
		private Settings _settings;

		// Token: 0x040010FC RID: 4348
		private Panel.State _state;

		// Token: 0x040010FD RID: 4349
		private EnumArray<Panel.State, Dialogue> _statePanels;

		// Token: 0x0200041F RID: 1055
		public enum State
		{
			// Token: 0x040010FF RID: 4351
			Menu,
			// Token: 0x04001100 RID: 4352
			Controls,
			// Token: 0x04001101 RID: 4353
			Settings
		}
	}
}
