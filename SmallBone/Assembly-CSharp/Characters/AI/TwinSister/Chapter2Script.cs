using System;
using System.Collections;
using System.Linq;
using Characters.Controllers;
using Characters.Operations.Fx;
using CutScenes;
using Data;
using FX;
using Level;
using Level.Chapter2;
using Runnables;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Characters.AI.TwinSister
{
	// Token: 0x02001153 RID: 4435
	public class Chapter2Script : MonoBehaviour
	{
		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x060056AB RID: 22187 RVA: 0x0010170C File Offset: 0x000FF90C
		// (set) Token: 0x060056AC RID: 22188 RVA: 0x00101714 File Offset: 0x000FF914
		public bool introCutScenePlayed
		{
			get
			{
				return this._introCutScenePlayed;
			}
			set
			{
				this._introCutScenePlayed = value;
			}
		}

		// Token: 0x060056AD RID: 22189 RVA: 0x00101720 File Offset: 0x000FF920
		private void Start()
		{
			this._intro = new Chapter2Script.Intro(this);
			this._inGame = new Chapter2Script.InGame(this);
			this._outro = new Chapter2Script.Outro(this);
			this.chest.OnOpen += delegate()
			{
				this._elevator.gameObject.SetActive(true);
			};
			if (GameData.HardmodeProgress.hardmode || GameData.Progress.cutscene.GetData(Key.leiana_Intro))
			{
				this._introCutScenePlayed = true;
			}
			base.StartCoroutine(this.CIntro());
		}

		// Token: 0x060056AE RID: 22190 RVA: 0x00101794 File Offset: 0x000FF994
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
			LetterBox.instance.Disappear(0.4f);
			PlayerInput.blocked.Detach(this);
			if (Singleton<Service>.Instance.levelManager.player != null)
			{
				Singleton<Service>.Instance.levelManager.player.invulnerable.Detach(this);
			}
			if (Scene<GameBase>.instance.cameraController == null || Singleton<Service>.Instance.levelManager.player == null)
			{
				return;
			}
			Scene<GameBase>.instance.cameraController.StartTrack(Singleton<Service>.Instance.levelManager.player.transform);
		}

		// Token: 0x060056AF RID: 22191 RVA: 0x000FBDD2 File Offset: 0x000F9FD2
		public void StartSequence()
		{
			Scene<GameBase>.instance.uiManager.letterBox.Appear(0.4f);
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x00101885 File Offset: 0x000FFA85
		public void EndSequence()
		{
			Scene<GameBase>.instance.uiManager.letterBox.Disappear(0.4f);
			Scene<GameBase>.instance.cameraController.StartTrack(Singleton<Service>.Instance.levelManager.player.transform);
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x001018C3 File Offset: 0x000FFAC3
		private IEnumerator CIntro()
		{
			this._shortHair.Attachinvincibility();
			this._longHair.Attachinvincibility();
			this._intro.IntroStart();
			yield return this._intro.CMovePlayerToCenter(this._playerIntroPoint.position);
			yield return Chronometer.global.WaitForSeconds(2f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._openDoorSound, base.transform.position);
			this.door.Open();
			yield return Chronometer.global.WaitForSeconds(1f);
			yield return this._intro.CAppearMaster(this.twinSisterMasterAI);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._closeDoorSound, base.transform.position);
			this.door.Close();
			yield return Chronometer.global.WaitForSeconds(1f);
			if (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(Key.leiana_Intro))
			{
				this._introTalk.Run();
			}
			while (!this._introCutScenePlayed)
			{
				yield return null;
			}
			base.StartCoroutine(this._intro.CIntroGoldenAide(this._leftGoldenAide));
			yield return this._intro.CIntroGoldenAide(this._rightGoldenAide);
			yield return Chronometer.global.WaitForSeconds(1f);
			this.bossNameDisplay.ShowAndHideAppearanceText();
			this._goldenAideWave.Spawn(false);
			base.StartCoroutine(this._shortHair.CIntro());
			yield return this._longHair.CIntro();
			if (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(Key.leiana_Intro))
			{
				this._talkToStartCombat.Run();
			}
			while (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(Key.leiana_Intro))
			{
				yield return null;
			}
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._bacgkroundMusicInfo);
			yield return Scene<GameBase>.instance.uiManager.letterBox.CDisappear(0.4f);
			this._shortHair.Dettachinvincibility();
			this._longHair.Dettachinvincibility();
			this._combatReference = this.StartCoroutineWithReference(this.Combat());
			this._intro.IntroEnd();
			yield break;
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x001018D2 File Offset: 0x000FFAD2
		private IEnumerator Combat()
		{
			base.StartCoroutine(this.WaitForAwakeningPrepare());
			yield return this.twinSisterMasterAI.RunIntroOut();
			yield return this.twinSisterMasterAI.ProcessDualCombat();
			this._inGame.SetFieldAideAndBehindAide(this._shortHair, this._longHair, this._leftGoldenAide, this._rightGoldenAide);
			yield return this._inGame.CGotoBehind();
			this._expireSingleCombatReference = this.StartCoroutineWithReference(this._inGame.CExpireSingleCombat(this.twinSisterMasterAI, this._siglePatternDuration));
			yield return this._inGame.CProcessSingleCombat(this.twinSisterMasterAI);
			while (!this._goldenAideEnd)
			{
				base.StartCoroutine(this._inGame.COutOfBehind());
				yield return this.twinSisterMasterAI.ProcessDualCombat();
				this._inGame.SetFieldAideAndBehindAide(this._shortHair, this._longHair, this._leftGoldenAide, this._rightGoldenAide);
				yield return this._inGame.CGotoBehind();
				this._expireSingleCombatReference = this.StartCoroutineWithReference(this._inGame.CExpireSingleCombat(this.twinSisterMasterAI, this._siglePatternDuration));
				yield return this._inGame.CProcessSingleCombat(this.twinSisterMasterAI);
			}
			yield break;
		}

		// Token: 0x060056B3 RID: 22195 RVA: 0x001018E1 File Offset: 0x000FFAE1
		private IEnumerator WaitForAwakeningPrepare()
		{
			while (this.twinSisterMasterAI.goldenAideDiedCount == 0)
			{
				yield return null;
			}
			this._goldenAideEnd = true;
			this._expireSingleCombatReference.Stop();
			GoldenAideAI toBeDarkAide = this._shortHair.dead ? this._longHair : this._shortHair;
			toBeDarkAide.Attachinvincibility();
			while (this.twinSisterMasterAI.lockForAwakening)
			{
				yield return null;
			}
			this._combatReference.Stop();
			PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
			if (this.twinSisterMasterAI.goldenAideDiedCount == 1)
			{
				toBeDarkAide.StopAllCoroutinesWithBehaviour();
				if (this.twinSisterMasterAI.singlePattern)
				{
					yield return this._inGame.COutOfBehind();
					base.StartCoroutine(this.twinSisterMasterAI.CPlaySurpriseReaction());
					yield return toBeDarkAide.CastAwakening();
				}
				else
				{
					this._leftGoldenAide.Hide();
					this._rightGoldenAide.Hide();
					base.StartCoroutine(this.twinSisterMasterAI.CPlaySurpriseReaction());
					yield return toBeDarkAide.CastAwakening();
				}
				this.SpawnDarkAide(toBeDarkAide.character, toBeDarkAide.character.transform.position, toBeDarkAide.character.lookingDirection);
				toBeDarkAide.Hide();
			}
			else if (this.twinSisterMasterAI.goldenAideDiedCount == 2)
			{
				this._leftGoldenAide.Hide();
				this._rightGoldenAide.Hide();
				toBeDarkAide = (MMMaths.RandomBool() ? this._longHair : this._shortHair);
				toBeDarkAide.StopAllCoroutinesWithBehaviour();
				toBeDarkAide.character.health.Revive();
				base.StartCoroutine(this.twinSisterMasterAI.CPlaySurpriseReaction());
				yield return toBeDarkAide.CastAwakening();
				this.SpawnDarkAide(toBeDarkAide.character, toBeDarkAide.character.transform.position, toBeDarkAide.character.lookingDirection);
				toBeDarkAide.Hide();
			}
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._awakenMusicInfo);
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			yield return Chronometer.global.WaitForSeconds(1f);
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.Open(BossHealthbarController.Type.Chapter2_Phase2, this.darkAide.character);
			yield break;
		}

		// Token: 0x060056B4 RID: 22196 RVA: 0x001018F0 File Offset: 0x000FFAF0
		private void SpawnDarkAide(Character healthOwner, Vector3 position, Character.LookingDirection lookingDirection)
		{
			float x = Map.Instance.bounds.center.x;
			this.twinSisterMasterAI.RemovePlayerHitReaction();
			this.darkAide.character.health.onDiedTryCatch += this._outro.StartOutro;
			this.darkAide.character.transform.position = position;
			this.darkAide.character.gameObject.SetActive(true);
			this.darkAide.character.ForceToLookAt(x);
			this.darkAide.ApplyHealth(healthOwner);
		}

		// Token: 0x040045AB RID: 17835
		[SerializeField]
		private MusicInfo _bacgkroundMusicInfo;

		// Token: 0x040045AC RID: 17836
		[SerializeField]
		private MusicInfo _awakenMusicInfo;

		// Token: 0x040045AD RID: 17837
		[SerializeField]
		private DarkAideAI darkAide;

		// Token: 0x040045AE RID: 17838
		[SerializeField]
		[Space]
		[Header("Intro")]
		private BossNameDisplay bossNameDisplay;

		// Token: 0x040045AF RID: 17839
		[SerializeField]
		private Transform _playerIntroPoint;

		// Token: 0x040045B0 RID: 17840
		[SerializeField]
		[Space]
		[Header("Door")]
		private Door door;

		// Token: 0x040045B1 RID: 17841
		[SerializeField]
		private SoundInfo _openDoorSound;

		// Token: 0x040045B2 RID: 17842
		[SerializeField]
		private SoundInfo _closeDoorSound;

		// Token: 0x040045B3 RID: 17843
		[Space]
		[Header("Master")]
		[SerializeField]
		private TwinSisterMasterAI twinSisterMasterAI;

		// Token: 0x040045B4 RID: 17844
		[Header("BehindGoldenAide")]
		[Space]
		[SerializeField]
		private BehindGoldenAide _leftGoldenAide;

		// Token: 0x040045B5 RID: 17845
		[SerializeField]
		private BehindGoldenAide _rightGoldenAide;

		// Token: 0x040045B6 RID: 17846
		[Header("FrontGoldenAide")]
		[Space]
		[SerializeField]
		private GoldenAideAI _shortHair;

		// Token: 0x040045B7 RID: 17847
		[SerializeField]
		private GoldenAideAI _longHair;

		// Token: 0x040045B8 RID: 17848
		[Header("Wave")]
		[Space]
		[SerializeField]
		private EnemyWave _goldenAideWave;

		// Token: 0x040045B9 RID: 17849
		[SerializeField]
		private float _siglePatternDuration;

		// Token: 0x040045BA RID: 17850
		[Header("Outro")]
		[SerializeField]
		private Characters.Operations.Fx.ScreenFlash flash;

		// Token: 0x040045BB RID: 17851
		[SerializeField]
		private Elevator _elevator;

		// Token: 0x040045BC RID: 17852
		[SerializeField]
		[Header("Reward")]
		private BossChest chest;

		// Token: 0x040045BD RID: 17853
		[SerializeField]
		private Chapter2Script.DarkQuartzPossibility.Reorderable darkQuartzes;

		// Token: 0x040045BE RID: 17854
		[SerializeField]
		private Runnable _introTalk;

		// Token: 0x040045BF RID: 17855
		[SerializeField]
		private Runnable _talkToStartCombat;

		// Token: 0x040045C0 RID: 17856
		[SerializeField]
		private Runnable _outroTalk;

		// Token: 0x040045C1 RID: 17857
		private bool _introCutScenePlayed;

		// Token: 0x040045C2 RID: 17858
		private Chapter2Script.Intro _intro;

		// Token: 0x040045C3 RID: 17859
		private Chapter2Script.InGame _inGame;

		// Token: 0x040045C4 RID: 17860
		private Chapter2Script.Outro _outro;

		// Token: 0x040045C5 RID: 17861
		private bool _goldenAideEnd;

		// Token: 0x040045C6 RID: 17862
		private CoroutineReference _combatReference;

		// Token: 0x040045C7 RID: 17863
		private CoroutineReference _expireSingleCombatReference;

		// Token: 0x02001154 RID: 4436
		[Serializable]
		private class DarkQuartzPossibility
		{
			// Token: 0x040045C8 RID: 17864
			[Range(0f, 100f)]
			public int weight;

			// Token: 0x040045C9 RID: 17865
			public CustomFloat amount;

			// Token: 0x02001155 RID: 4437
			[Serializable]
			internal class Reorderable : ReorderableArray<Chapter2Script.DarkQuartzPossibility>
			{
				// Token: 0x060056B8 RID: 22200 RVA: 0x001019A4 File Offset: 0x000FFBA4
				public int Take()
				{
					if (this.values.Length == 0)
					{
						return 0;
					}
					int maxExclusive = this.values.Sum((Chapter2Script.DarkQuartzPossibility v) => v.weight);
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

		// Token: 0x02001157 RID: 4439
		private class Intro
		{
			// Token: 0x060056BD RID: 22205 RVA: 0x00101A49 File Offset: 0x000FFC49
			public Intro(Chapter2Script script)
			{
				this._script = script;
			}

			// Token: 0x060056BE RID: 22206 RVA: 0x00101A58 File Offset: 0x000FFC58
			public void IntroStart()
			{
				Scene<GameBase>.instance.uiManager.headupDisplay.visible = false;
				PlayerInput.blocked.Attach(this._script);
				Scene<GameBase>.instance.uiManager.letterBox.Appear(0.4f);
			}

			// Token: 0x060056BF RID: 22207 RVA: 0x00101A98 File Offset: 0x000FFC98
			public void IntroEnd()
			{
				PlayerInput.blocked.Detach(this._script);
				Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.OpenChapter2Phase1(this._script._shortHair.character, this._script._longHair.character);
			}

			// Token: 0x060056C0 RID: 22208 RVA: 0x00101AEF File Offset: 0x000FFCEF
			public IEnumerator CMovePlayerToCenter(Vector2 dest)
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				player.CancelAction();
				yield return this.MoveTo(dest, player);
				yield break;
			}

			// Token: 0x060056C1 RID: 22209 RVA: 0x00101B05 File Offset: 0x000FFD05
			public IEnumerator CAppearMaster(TwinSisterMasterAI twinSisterMasterAI)
			{
				yield return twinSisterMasterAI.CIntro();
				yield break;
			}

			// Token: 0x060056C2 RID: 22210 RVA: 0x00101B14 File Offset: 0x000FFD14
			public IEnumerator CIntroGoldenAide(BehindGoldenAide behindGoldenAide)
			{
				yield return behindGoldenAide.CIntroOut();
				yield break;
			}

			// Token: 0x060056C3 RID: 22211 RVA: 0x00101B23 File Offset: 0x000FFD23
			public IEnumerator CAppear(GoldenAideAI goldenAide)
			{
				yield return goldenAide.CIntro();
				yield break;
			}

			// Token: 0x060056C4 RID: 22212 RVA: 0x00101B32 File Offset: 0x000FFD32
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

			// Token: 0x040045CC RID: 17868
			private Chapter2Script _script;
		}

		// Token: 0x0200115D RID: 4445
		private class InGame
		{
			// Token: 0x060056E3 RID: 22243 RVA: 0x00101D97 File Offset: 0x000FFF97
			public InGame(Chapter2Script script)
			{
				this._script = script;
			}

			// Token: 0x060056E4 RID: 22244 RVA: 0x00101DA6 File Offset: 0x000FFFA6
			public IEnumerator CGotoBehind()
			{
				yield return this._behind.CIn();
				yield break;
			}

			// Token: 0x060056E5 RID: 22245 RVA: 0x00101DB5 File Offset: 0x000FFFB5
			public IEnumerator COutOfBehind()
			{
				yield return this._behind.COut();
				yield break;
			}

			// Token: 0x060056E6 RID: 22246 RVA: 0x00101DC4 File Offset: 0x000FFFC4
			public IEnumerator CProcessSingleCombat(TwinSisterMasterAI master)
			{
				yield return master.ProcessSingleCombat(this._fieldAide, this._behindAide);
				yield break;
			}

			// Token: 0x060056E7 RID: 22247 RVA: 0x00101DDA File Offset: 0x000FFFDA
			public IEnumerator CExpireSingleCombat(TwinSisterMasterAI master, float duration)
			{
				float elapsed = 0f;
				master.singlePattern = true;
				while (master.singlePattern)
				{
					yield return null;
					elapsed += Chronometer.global.deltaTime;
					if (elapsed >= duration)
					{
						break;
					}
				}
				master.singlePattern = false;
				yield break;
			}

			// Token: 0x060056E8 RID: 22248 RVA: 0x00101DF0 File Offset: 0x000FFFF0
			public void SetFieldAideAndBehindAide(GoldenAideAI longhair, GoldenAideAI shorthair, BehindGoldenAide longhairBehind, BehindGoldenAide shorthairBehind)
			{
				if (MMMaths.RandomBool())
				{
					this._fieldAide = longhair;
					this._behindAide = shorthair;
					this._behind = shorthairBehind;
					return;
				}
				this._fieldAide = shorthair;
				this._behindAide = longhair;
				this._behind = longhairBehind;
			}

			// Token: 0x040045DE RID: 17886
			private GoldenAideAI _fieldAide;

			// Token: 0x040045DF RID: 17887
			private GoldenAideAI _behindAide;

			// Token: 0x040045E0 RID: 17888
			private BehindGoldenAide _behind;

			// Token: 0x040045E1 RID: 17889
			private Chapter2Script _script;
		}

		// Token: 0x02001162 RID: 4450
		private class Outro
		{
			// Token: 0x06005701 RID: 22273 RVA: 0x00102007 File Offset: 0x00100207
			public Outro(Chapter2Script script)
			{
				this._script = script;
			}

			// Token: 0x06005702 RID: 22274 RVA: 0x00102018 File Offset: 0x00100218
			public void StartOutro()
			{
				Singleton<Service>.Instance.levelManager.player.invulnerable.Attach(this._script);
				this._script.StartSequence();
				this._script.flash.Run(this._script.darkAide.character);
				PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
				this._script.StartCoroutine(this.CEndOutro());
			}

			// Token: 0x06005703 RID: 22275 RVA: 0x0010208B File Offset: 0x0010028B
			private IEnumerator CEndOutro()
			{
				this._script.twinSisterMasterAI.PlayAwakenDieReaction();
				yield return Chronometer.global.WaitForSeconds(8f);
				this.EndOutro();
				yield break;
			}

			// Token: 0x06005704 RID: 22276 RVA: 0x0010209A File Offset: 0x0010029A
			private void EndOutro()
			{
				this._script.StartCoroutine(this.CExit(this._script.twinSisterMasterAI, this._script.door));
			}

			// Token: 0x06005705 RID: 22277 RVA: 0x001020C4 File Offset: 0x001002C4
			public IEnumerator CExit(TwinSisterMasterAI masterAI, Door door)
			{
				if (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(Key.leiana_Outro))
				{
					this._script._outroTalk.Run();
				}
				while (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(Key.leiana_Outro))
				{
					yield return null;
				}
				yield return Chronometer.global.WaitForSeconds(1f);
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._script._openDoorSound, this._script.transform.position);
				door.Open();
				yield return masterAI.COutro();
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._script._closeDoorSound, this._script.transform.position);
				door.Close();
				yield return Chronometer.global.WaitForSeconds(1f);
				this._script.chest.gameObject.SetActive(true);
				Singleton<Service>.Instance.levelManager.DropDarkQuartz(this._script.darkQuartzes.Take(), 30, this._script.chest.transform.position, Vector2.up);
				this._script.EndSequence();
				Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
				yield break;
			}

			// Token: 0x040045F1 RID: 17905
			private Chapter2Script _script;
		}
	}
}
