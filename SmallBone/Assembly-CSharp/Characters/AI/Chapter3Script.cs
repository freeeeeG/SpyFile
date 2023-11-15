using System;
using System.Collections;
using System.Linq;
using Characters.AI.Chimera;
using Characters.Controllers;
using Characters.Operations;
using Characters.Operations.Fx;
using CutScenes;
using Data;
using FX;
using Level;
using Runnables;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010E6 RID: 4326
	public class Chapter3Script : MonoBehaviour
	{
		// Token: 0x0600540F RID: 21519 RVA: 0x000FBC3C File Offset: 0x000F9E3C
		private void Start()
		{
			this._intro = new Chapter3Script.Intro(this);
			this._outro = new Chapter3Script.Outro(this);
			this.chimera.character.health.onDiedTryCatch += delegate()
			{
				this._outro.StartOutro();
				this._outro.EndOutro();
				if (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(Key.chimera_Outro))
				{
					this._cutScene.Run();
				}
			};
			this.chest.OnOpen += delegate()
			{
				this._nextGate.SetActive(true);
			};
			base.StartCoroutine(this.CIntro());
		}

		// Token: 0x06005410 RID: 21520 RVA: 0x000FBCA8 File Offset: 0x000F9EA8
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			PlayerInput.blocked.Detach(this);
			Singleton<Service>.Instance.levelManager.player.movement.blocked.Detach(this);
			if (Scene<GameBase>.instance.uiManager != null && Scene<GameBase>.instance.uiManager.npcConversation != null)
			{
				Scene<GameBase>.instance.uiManager.npcConversation.Done();
			}
			if (Scene<GameBase>.instance.uiManager != null && Scene<GameBase>.instance.uiManager.headupDisplay != null)
			{
				Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			}
			LetterBox.instance.Disappear(0.4f);
			if (Scene<GameBase>.instance.cameraController == null || Singleton<Service>.Instance.levelManager.player)
			{
				return;
			}
			Scene<GameBase>.instance.cameraController.StartTrack(Singleton<Service>.Instance.levelManager.player.transform);
		}

		// Token: 0x06005411 RID: 21521 RVA: 0x000FBDC3 File Offset: 0x000F9FC3
		private IEnumerator CIntro()
		{
			this.StartSequence();
			yield return this._intro.CMovePlayerToCenter(this._playerIntroPoint.position);
			this._intro.IntroStart();
			if (GameData.HardmodeProgress.hardmode || GameData.Progress.cutscene.GetData(Key.chimera_Intro))
			{
				this.EndIntro();
			}
			yield break;
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x000FBDD2 File Offset: 0x000F9FD2
		public void StartSequence()
		{
			Scene<GameBase>.instance.uiManager.letterBox.Appear(0.4f);
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x000FBDED File Offset: 0x000F9FED
		public void EndSequence()
		{
			Scene<GameBase>.instance.uiManager.letterBox.Disappear(0.4f);
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x000FBE08 File Offset: 0x000FA008
		public void HideCeil()
		{
			this._ceil.gameObject.SetActive(false);
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x000FBE1B File Offset: 0x000FA01B
		public void ShowCeil()
		{
			this._ceil.gameObject.SetActive(true);
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x000FBE2E File Offset: 0x000FA02E
		public void EndIntro()
		{
			this._intro.IntroEnd();
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._bacgkroundMusicInfo);
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x000FBE4B File Offset: 0x000FA04B
		public void EndOutro()
		{
			if (GameData.HardmodeProgress.hardmode || GameData.Progress.cutscene.GetData(Key.chimera_Outro))
			{
				this.OpenChest();
			}
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x000FBE6C File Offset: 0x000FA06C
		public void OpenChest()
		{
			this.chest.gameObject.SetActive(true);
			Singleton<Service>.Instance.levelManager.DropDarkQuartz(this.darkQuartzes.Take(), 30, this.chest.transform.position, Vector2.up);
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x000FBEBB File Offset: 0x000FA0BB
		public void DisplayBossName()
		{
			this.bossNameDisplay.ShowAndHideAppearanceText();
		}

		// Token: 0x04004380 RID: 17280
		[SerializeField]
		private MusicInfo _bacgkroundMusicInfo;

		// Token: 0x04004381 RID: 17281
		[SerializeField]
		private Chimera chimera;

		// Token: 0x04004382 RID: 17282
		[SerializeField]
		[Space]
		[Header("Intro")]
		[Range(0.5f, 1.5f)]
		private float _cameraRatio = 1.2f;

		// Token: 0x04004383 RID: 17283
		[Range(0.1f, 10f)]
		[SerializeField]
		private float _cameraZoomOutSpeed = 1f;

		// Token: 0x04004384 RID: 17284
		[SerializeField]
		private BossNameDisplay fakeBossNameDisplay;

		// Token: 0x04004385 RID: 17285
		[SerializeField]
		private BossNameDisplay bossNameDisplay;

		// Token: 0x04004386 RID: 17286
		[SerializeField]
		private Transform _playerIntroPoint;

		// Token: 0x04004387 RID: 17287
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos alchemistDieOperation;

		// Token: 0x04004388 RID: 17288
		[SerializeField]
		[Space]
		[Header("InGame")]
		private GameObject _ceil;

		// Token: 0x04004389 RID: 17289
		[Subcomponent(typeof(Characters.Operations.Fx.ScreenFlash))]
		[Header("Outro")]
		[SerializeField]
		private Characters.Operations.Fx.ScreenFlash flash;

		// Token: 0x0400438A RID: 17290
		[SerializeField]
		[Header("Reward")]
		private BossChest chest;

		// Token: 0x0400438B RID: 17291
		[SerializeField]
		private GameObject _nextGate;

		// Token: 0x0400438C RID: 17292
		[SerializeField]
		private Chapter3Script.DarkQuartzPossibility.Reorderable darkQuartzes;

		// Token: 0x0400438D RID: 17293
		[SerializeField]
		private Runnable _cutScene;

		// Token: 0x0400438E RID: 17294
		private Chapter3Script.Intro _intro;

		// Token: 0x0400438F RID: 17295
		private Chapter3Script.Outro _outro;

		// Token: 0x020010E7 RID: 4327
		[Serializable]
		private class DarkQuartzPossibility
		{
			// Token: 0x04004390 RID: 17296
			[Range(0f, 100f)]
			public int weight;

			// Token: 0x04004391 RID: 17297
			public CustomFloat amount;

			// Token: 0x020010E8 RID: 4328
			[Serializable]
			internal class Reorderable : ReorderableArray<Chapter3Script.DarkQuartzPossibility>
			{
				// Token: 0x0600541E RID: 21534 RVA: 0x000FBF30 File Offset: 0x000FA130
				public int Take()
				{
					if (this.values.Length == 0)
					{
						return 0;
					}
					int maxExclusive = this.values.Sum((Chapter3Script.DarkQuartzPossibility v) => v.weight);
					int num = UnityEngine.Random.Range(0, maxExclusive) + 1;
					for (int i = 0; i < this.values.Length; i++)
					{
						num -= this.values[i].weight;
						if (num <= 0)
						{
							return (int)this.values[i].amount.value;
						}
					}
					return 0;
				}
			}
		}

		// Token: 0x020010EA RID: 4330
		private class Intro
		{
			// Token: 0x06005423 RID: 21539 RVA: 0x000FBFD5 File Offset: 0x000FA1D5
			internal Intro(Chapter3Script script)
			{
				this._script = script;
			}

			// Token: 0x06005424 RID: 21540 RVA: 0x000FBFE4 File Offset: 0x000FA1E4
			public void IntroStart()
			{
				if (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(Key.chimera_Intro))
				{
					this._script.fakeBossNameDisplay.ShowAndHideAppearanceText();
				}
			}

			// Token: 0x06005425 RID: 21541 RVA: 0x000FC00E File Offset: 0x000FA20E
			public void IntroEnd()
			{
				this._script.StartCoroutine(this._script.chimera.Combat());
			}

			// Token: 0x06005426 RID: 21542 RVA: 0x000FC02C File Offset: 0x000FA22C
			public IEnumerator CMovePlayerToCenter(Vector2 dest)
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				player.CancelAction();
				PlayerInput.blocked.Attach(this._script);
				yield return this.MoveTo(dest, player);
				PlayerInput.blocked.Detach(this._script);
				yield break;
			}

			// Token: 0x06005427 RID: 21543 RVA: 0x000FC042 File Offset: 0x000FA242
			private IEnumerator MoveTo(Vector3 destination, Character player)
			{
				for (;;)
				{
					float num = destination.x - player.transform.position.x;
					if (Mathf.Abs(num) < 0.1f)
					{
						break;
					}
					Vector2 move = (num > 0f) ? Vector2.right : Vector2.left;
					player.movement.move = move;
					yield return null;
				}
				yield break;
			}

			// Token: 0x04004394 RID: 17300
			private Chapter3Script _script;
		}

		// Token: 0x020010ED RID: 4333
		private class InGame
		{
			// Token: 0x06005434 RID: 21556 RVA: 0x000FC1A7 File Offset: 0x000FA3A7
			internal InGame(Chapter3Script script)
			{
				this._script = script;
			}

			// Token: 0x0400439D RID: 17309
			private Chapter3Script _script;
		}

		// Token: 0x020010EE RID: 4334
		private class Outro
		{
			// Token: 0x06005435 RID: 21557 RVA: 0x000FC1B6 File Offset: 0x000FA3B6
			internal Outro(Chapter3Script script)
			{
				this._script = script;
			}

			// Token: 0x06005436 RID: 21558 RVA: 0x000FC1C5 File Offset: 0x000FA3C5
			public void StartOutro()
			{
				this._script.StartSequence();
				this._script.flash.Run(this._script.chimera.character);
				PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
			}

			// Token: 0x06005437 RID: 21559 RVA: 0x000FC1FC File Offset: 0x000FA3FC
			public void EndOutro()
			{
				this._script.EndSequence();
				Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			}

			// Token: 0x0400439E RID: 17310
			private Chapter3Script _script;
		}
	}
}
