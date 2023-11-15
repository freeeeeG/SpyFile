using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using Characters.Actions;
using Characters.Controllers;
using Level;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000D6 RID: 214
	public class DieTutorial : Tutorial
	{
		// Token: 0x06000416 RID: 1046 RVA: 0x0000DE4D File Offset: 0x0000C04D
		protected override void Start()
		{
			base.Start();
			this._player = Singleton<Service>.Instance.levelManager.player;
			this._enemyWave.onClear += delegate()
			{
				PlayerInput.blocked.Attach(this);
				Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
				base.StartCoroutine(this.<Start>g__ProcessOgreDie|12_1());
			};
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000DE81 File Offset: 0x0000C081
		protected override IEnumerator Process()
		{
			yield return Chronometer.global.WaitForSeconds(2f);
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			this._player.transform.position = this._plyerConversationPoint.position;
			this._ogre.transform.position = this._ogreConversationPoint.position;
			this._witch.transform.position = this._witchConversationPoint.position;
			UnityEngine.Object.Destroy(this._darkOgre);
			this._ogre.gameObject.SetActive(true);
			this._ogre.transform.localScale = new Vector3(-1f, 1f, 1f);
			this._witch.lookingDirection = Character.LookingDirection.Right;
			this._player.lookingDirection = Character.LookingDirection.Right;
			yield return Chronometer.global.WaitForSeconds(2f);
			this._wakeUp.TryStart();
			yield return Singleton<Service>.Instance.fadeInOut.CFadeIn();
			while (this._wakeUp.running)
			{
				yield return null;
			}
			this._npcConversation.Done();
			this._laugh.TryStart();
			while (this._laugh.running)
			{
				yield return null;
			}
			yield return this.MoveTo(this._diePoint.position);
			Scene<GameBase>.instance.cameraController.Shake(1.3f, 0.5f);
			this._hero.transform.parent.gameObject.SetActive(true);
			yield return this._hero.CAppear();
			this._npcConversation.Done();
			yield return this._hero.CAttack();
			yield return base.CDeactivate();
			PlayerInput.blocked.Detach(this);
			Singleton<Service>.Instance.levelManager.ResetGame(Chapter.Type.Castle);
			yield break;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000DE90 File Offset: 0x0000C090
		private new IEnumerator MoveTo(Vector3 destination)
		{
			for (;;)
			{
				float num = destination.x - this._ogre.transform.position.x;
				if (Mathf.Abs(num) < 1f)
				{
					break;
				}
				Vector2 move = (num > 0f) ? Vector2.right : Vector2.left;
				this._ogre.movement.move = move;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000DEA6 File Offset: 0x0000C0A6
		protected override void OnDisable()
		{
			base.OnDisable();
			PlayerInput.blocked.Detach(this);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000DEED File Offset: 0x0000C0ED
		[CompilerGenerated]
		private IEnumerator <Start>g__ProcessOgreDie|12_1()
		{
			Singleton<Service>.Instance.fadeInOut.SetFadeColor(Color.white);
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			yield return Singleton<Service>.Instance.fadeInOut.CFadeIn();
			Singleton<Service>.Instance.fadeInOut.SetFadeColor(Color.black);
			yield return Chronometer.global.WaitForSeconds(1.7f);
			this.Activate();
			yield break;
		}

		// Token: 0x0400032E RID: 814
		[SerializeField]
		private EnemyWave _enemyWave;

		// Token: 0x0400032F RID: 815
		[SerializeField]
		private Hero _hero;

		// Token: 0x04000330 RID: 816
		[SerializeField]
		private Character _ogre;

		// Token: 0x04000331 RID: 817
		[SerializeField]
		private GameObject _darkOgre;

		// Token: 0x04000332 RID: 818
		[SerializeField]
		private Character _witch;

		// Token: 0x04000333 RID: 819
		[SerializeField]
		private Transform _plyerConversationPoint;

		// Token: 0x04000334 RID: 820
		[SerializeField]
		private Transform _ogreConversationPoint;

		// Token: 0x04000335 RID: 821
		[SerializeField]
		private Transform _witchConversationPoint;

		// Token: 0x04000336 RID: 822
		[SerializeField]
		private Transform _diePoint;

		// Token: 0x04000337 RID: 823
		[SerializeField]
		private BossNameDisplay _bossNameDisplay;

		// Token: 0x04000338 RID: 824
		[SerializeField]
		private Characters.Actions.Action _wakeUp;

		// Token: 0x04000339 RID: 825
		[SerializeField]
		private Characters.Actions.Action _laugh;
	}
}
