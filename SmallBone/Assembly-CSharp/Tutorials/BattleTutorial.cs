using System;
using System.Collections;
using Characters.AI;
using Characters.AI.Mercenarys;
using Scenes;
using UI;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000D2 RID: 210
	public class BattleTutorial : Tutorial
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x0000DC21 File Offset: 0x0000BE21
		private void Awake()
		{
			this._ogre.character.cinematic.Attach(this);
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000DC52 File Offset: 0x0000BE52
		public override void Activate()
		{
			base.Activate();
			Scene<GameBase>.instance.cameraController.StartTrack(this._trackPoint);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000DC6F File Offset: 0x0000BE6F
		public override void Deactivate()
		{
			base.Deactivate();
			Scene<GameBase>.instance.cameraController.StartTrack(this._player.transform);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000DC91 File Offset: 0x0000BE91
		protected override IEnumerator Process()
		{
			UIManager uiManager = Scene<GameBase>.instance.uiManager;
			uiManager.npcConversation.Done();
			uiManager.headupDisplay.bossHealthBar.CloseAll();
			uiManager.headupDisplay.bossHealthBar.Open(BossHealthbarController.Type.Tutorial, this._ogre.character);
			this._bossNameDisplay.ShowAppearanceText();
			yield return Chronometer.global.WaitForSeconds(1.7f);
			this._bossNameDisplay.HideAppearanceText();
			this.Deactivate();
			this._witch.follow = false;
			this._ogre.character.cinematic.Detach(this);
			this._ogre.target = this._player;
			yield break;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000DCA0 File Offset: 0x0000BEA0
		protected override void OnDisable()
		{
			base.OnDisable();
			this._ogre.character.cinematic.Detach(this);
		}

		// Token: 0x04000324 RID: 804
		[SerializeField]
		private AIController _ogre;

		// Token: 0x04000325 RID: 805
		[SerializeField]
		private Transform _trackPoint;

		// Token: 0x04000326 RID: 806
		[SerializeField]
		private BossNameDisplay _bossNameDisplay;

		// Token: 0x04000327 RID: 807
		[SerializeField]
		private Mercenary _witch;
	}
}
