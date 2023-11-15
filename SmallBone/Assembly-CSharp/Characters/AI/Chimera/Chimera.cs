using System;
using System.Collections;
using Characters.AI.Behaviours.Chimera;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Chimera
{
	// Token: 0x0200122A RID: 4650
	public class Chimera : AIController
	{
		// Token: 0x06005B2D RID: 23341 RVA: 0x0010DE78 File Offset: 0x0010C078
		private void Awake()
		{
			base.OnEnable();
			this.RegisterBehaviourtoEvent();
			base.StartCoroutine(this.CProcess());
			this.character.health.onDiedTryCatch += this.HandleOnDied;
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.Open(BossHealthbarController.Type.Chpater3_Phase1, this.character);
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x0010DEDA File Offset: 0x0010C0DA
		private void OnDestroy()
		{
			if (Service.quitting || Singleton<Service>.Instance.levelManager.player == null)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.player.cinematic.Detach(this);
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x0010DF16 File Offset: 0x0010C116
		private void HandleOnDied()
		{
			this._wreckDestroy.DestroyWreck(this.character);
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x0010DF29 File Offset: 0x0010C129
		protected override IEnumerator CProcess()
		{
			this.character.health.onDiedTryCatch += delegate()
			{
				Singleton<Service>.Instance.levelManager.player.cinematic.Attach(this);
				this._chimeraDie.KillAllEnemyInBounds(this);
				base.StopAllCoroutinesWithBehaviour();
				base.StartCoroutine(this.Die());
			};
			yield return this.Sleep();
			yield break;
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x0010DF38 File Offset: 0x0010C138
		public IEnumerator Combat()
		{
			yield return this.Intro();
			while (!this.character.health.dead)
			{
				yield return this._chimeraCombat.Combat();
			}
			yield break;
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x0010DF47 File Offset: 0x0010C147
		public bool CanUseWreckDrop()
		{
			return this._wreckDrop.CanUse(this.character);
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x0010DF5A File Offset: 0x0010C15A
		public bool CanUseSubjectDrop()
		{
			return this._subjectDrop.canUse;
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x0010DF67 File Offset: 0x0010C167
		public bool CanUseStomp()
		{
			return this._stomp.CanUse(this);
		}

		// Token: 0x06005B35 RID: 23349 RVA: 0x0010DF75 File Offset: 0x0010C175
		public bool CanUseVenomFall()
		{
			return this._venomFall.CanUse();
		}

		// Token: 0x06005B36 RID: 23350 RVA: 0x0010DF82 File Offset: 0x0010C182
		public bool CanUseBite()
		{
			return this._bite.CanUse(this);
		}

		// Token: 0x06005B37 RID: 23351 RVA: 0x0010DF90 File Offset: 0x0010C190
		public void SetAnimationSpeed(float speed)
		{
			this._chimeraAnimation.speed = speed;
		}

		// Token: 0x06005B38 RID: 23352 RVA: 0x0010DF9E File Offset: 0x0010C19E
		public IEnumerator RunPattern(Pattern pattern)
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			switch (pattern)
			{
			case Pattern.Idle:
				yield return this.Idle();
				break;
			case Pattern.SkippableIdle:
				yield return this.SkippableIdle();
				break;
			case Pattern.Bite:
				yield return this.CastBite();
				break;
			case Pattern.Stomp:
				yield return this.CastStomp();
				break;
			case Pattern.VenomFall:
				yield return this.CastVenomFall();
				break;
			case Pattern.VenomBall:
				yield return this.CastVenomBall();
				break;
			case Pattern.VenomCannon:
				yield return this.CastVenomCannon();
				break;
			case Pattern.SubjectDrop:
				yield return this.CastSubjectDrop();
				break;
			case Pattern.WreckDrop:
				yield return this.CastWreckDrop();
				break;
			case Pattern.WreckDestroy:
				yield return this.CastWreckDestroy();
				break;
			case Pattern.VenomBreath:
				yield return this.CastVenomBreath();
				break;
			}
			yield break;
		}

		// Token: 0x06005B39 RID: 23353 RVA: 0x0010DFB4 File Offset: 0x0010C1B4
		private IEnumerator Sleep()
		{
			yield return this._chimeraAnimation.PlaySleepAnimation();
			yield break;
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x0010DFC3 File Offset: 0x0010C1C3
		private IEnumerator Idle()
		{
			yield return this._chimeraAnimation.PlayIdleAnimation();
			yield break;
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x0010DFD2 File Offset: 0x0010C1D2
		private IEnumerator SkippableIdle()
		{
			if (!MMMaths.Chance(this._idleSkipChance))
			{
				yield return this._chimeraAnimation.PlayIdleAnimation();
			}
			yield break;
		}

		// Token: 0x06005B3C RID: 23356 RVA: 0x0010DFE1 File Offset: 0x0010C1E1
		private IEnumerator Intro()
		{
			this.character.cinematic.Attach(this);
			yield return this._chimeraAnimation.PlayIntroAnimation();
			this.character.cinematic.Detach(this);
			yield break;
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x0010DFF0 File Offset: 0x0010C1F0
		private IEnumerator CastBite()
		{
			yield return this._chimeraAnimation.PlayBiteAnimation();
			yield break;
		}

		// Token: 0x06005B3E RID: 23358 RVA: 0x0010DFFF File Offset: 0x0010C1FF
		private IEnumerator CastStomp()
		{
			yield return this._chimeraAnimation.PlayStompAnimation();
			yield break;
		}

		// Token: 0x06005B3F RID: 23359 RVA: 0x0010E00E File Offset: 0x0010C20E
		private IEnumerator CastVenomFall()
		{
			yield return this._chimeraAnimation.PlayVenomFallAnimation();
			yield break;
		}

		// Token: 0x06005B40 RID: 23360 RVA: 0x0010E01D File Offset: 0x0010C21D
		private IEnumerator CastVenomBall()
		{
			this._script.ShowCeil();
			yield return this._chimeraAnimation.PlayVenomBallAnimation();
			yield break;
		}

		// Token: 0x06005B41 RID: 23361 RVA: 0x0010E02C File Offset: 0x0010C22C
		private IEnumerator CastVenomCannon()
		{
			yield return this._chimeraAnimation.PlayVenomCannonAnimation();
			yield break;
		}

		// Token: 0x06005B42 RID: 23362 RVA: 0x0010E03B File Offset: 0x0010C23B
		private IEnumerator CastSubjectDrop()
		{
			yield return this._chimeraAnimation.PlaySubjectDropAnimation();
			yield break;
		}

		// Token: 0x06005B43 RID: 23363 RVA: 0x0010E04A File Offset: 0x0010C24A
		private IEnumerator CastVenomBreath()
		{
			this._script.HideCeil();
			yield return this._chimeraAnimation.PlayVenomBreathAnimation();
			yield break;
		}

		// Token: 0x06005B44 RID: 23364 RVA: 0x0010E059 File Offset: 0x0010C259
		private IEnumerator CastWreckDrop()
		{
			yield return this._chimeraAnimation.PlayWreckDropAnimation();
			yield break;
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x0010E068 File Offset: 0x0010C268
		private IEnumerator CastWreckDestroy()
		{
			yield return this._chimeraAnimation.PlayWreckDestroyAnimation();
			yield break;
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x0010E077 File Offset: 0x0010C277
		private IEnumerator Die()
		{
			this.SetAnimationSpeed(1f);
			yield return this._chimeraAnimation.PlayDieAnimation();
			Singleton<Service>.Instance.levelManager.player.cinematic.Detach(this);
			yield break;
		}

		// Token: 0x06005B47 RID: 23367 RVA: 0x0010E088 File Offset: 0x0010C288
		private void RegisterBehaviourtoEvent()
		{
			this._chimeraEventListener.onIntro_Ready += delegate()
			{
				this._intro.Ready(this.character);
			};
			this._chimeraEventListener.onIntro_Landing += delegate()
			{
				this._intro.Landing(this.character, this._script);
			};
			this._chimeraEventListener.onIntro_FallingRocks += delegate()
			{
				this._intro.FallingRocks(this.character);
			};
			this._chimeraEventListener.onIntro_Explosion += delegate()
			{
				this._intro.Explosion(this.character);
				if (this._darkAlchemist != null)
				{
					this._darkAlchemist.health.Kill();
				}
			};
			this._chimeraEventListener.onIntro_CameraZoomOut += delegate()
			{
				this._intro.CameraZoomOut();
			};
			this._chimeraEventListener.onIntro_Roar_Ready += delegate()
			{
				this._intro.RoarReady(this.character);
			};
			this._chimeraEventListener.onIntro_Roar += delegate()
			{
				this._intro.Roar(this.character);
			};
			this._chimeraEventListener.onIntro_Roar += delegate()
			{
				this._intro.LetterBoxOff();
			};
			this._chimeraEventListener.onIntro_Roar += delegate()
			{
				this._intro.HealthBarOn(this.character);
			};
			this._chimeraEventListener.onBite_Ready += delegate()
			{
				this._bite.Ready(this.character);
			};
			this._chimeraEventListener.onBite_Attack += delegate()
			{
				base.StartCoroutine(this._bite.CRun(this));
			};
			this._chimeraEventListener.onBite_End += delegate()
			{
				this._bite.End(this.character);
			};
			this._chimeraEventListener.onBite_Hit += delegate()
			{
				this._bite.Hit(this.character);
			};
			this._chimeraEventListener.onStomp_Ready += delegate()
			{
				this._stomp.Ready(this.character);
			};
			this._chimeraEventListener.onStomp_Attack += delegate()
			{
				base.StartCoroutine(this._stomp.CRun(this));
			};
			this._chimeraEventListener.onStomp_End += delegate()
			{
				this._stomp.End(this.character);
			};
			this._chimeraEventListener.onStomp_Hit += delegate()
			{
				this._stomp.Hit(this.character);
			};
			this._chimeraEventListener.onSubjectDrop_Roar_Ready += delegate()
			{
				this._subjectDrop.Ready(this.character);
			};
			this._chimeraEventListener.onSubjectDrop_Roar += delegate()
			{
				this._subjectDrop.Roar(this.character);
			};
			this._chimeraEventListener.onSubjectDrop_Fire += delegate()
			{
				this._script.HideCeil();
				base.StartCoroutine(this._subjectDrop.CRun(this));
			};
			this._chimeraEventListener.onSubjectDrop_Roar_End += delegate()
			{
				this._subjectDrop.End(this.character);
			};
			this._chimeraEventListener.onVenomBall_Ready += delegate()
			{
				this._venomBall.Ready(this.character);
			};
			this._chimeraEventListener.onVenomBall_Fire += delegate()
			{
				base.StartCoroutine(this._venomBall.CRun(this));
			};
			this._chimeraEventListener.onVenomCannon_Ready += delegate()
			{
				this._venomCannon.Ready(this.character);
			};
			this._chimeraEventListener.onVenomCannon_Fire += delegate()
			{
				base.StartCoroutine(this._venomCannon.CRun(this));
			};
			this._chimeraEventListener.onVenomFall_Roar_Ready += delegate()
			{
				this._venomFall.Ready(this.character);
			};
			this._chimeraEventListener.onVenomFall_Roar += delegate()
			{
				this._venomFall.Roar(this.character);
			};
			this._chimeraEventListener.onVenomFall_Fire += delegate()
			{
				base.StartCoroutine(this._venomFall.CRun(this));
			};
			this._chimeraEventListener.onVenomFall_Roar_End += delegate()
			{
				this._venomFall.EndRoar(this.character);
			};
			this._chimeraEventListener.onWreckDrop_Out_Ready += delegate()
			{
				this._wreckDrop.OutReady(this.character);
			};
			this._chimeraEventListener.onWreckDrop_Out += delegate()
			{
				this._wreckDrop.OutJump(this.character);
			};
			this._chimeraEventListener.onWreckDrop_In_Sign += delegate()
			{
				this._wreckDrop.InSign(this.character);
			};
			this._chimeraEventListener.onWreckDrop_In_Ready += delegate()
			{
				this._wreckDrop.InReady(this.character);
			};
			this._chimeraEventListener.onWreckDrop_Fire += delegate()
			{
				this._script.HideCeil();
				base.StartCoroutine(this._wreckDrop.CRun(this));
			};
			this._chimeraEventListener.oWreckDrop_In += delegate()
			{
				this._wreckDrop.InLanding(this.character);
			};
			this._chimeraEventListener.onVenomBreath_Ready += delegate()
			{
				this._venomBreath.Ready(this.character);
			};
			this._chimeraEventListener.onVenomBreath_Fire += delegate()
			{
				base.StartCoroutine(this._venomBreath.CRun(this));
			};
			this._chimeraEventListener.onVenomBreath_End += delegate()
			{
				this._venomBreath.End(this.character);
			};
			this._chimeraEventListener.onBigStomp_Ready += delegate()
			{
				this._wreckDestroy.Ready(this.character);
			};
			this._chimeraEventListener.onBigStomp_Attack += delegate()
			{
				this._wreckDestroy.Attack(this.character);
			};
			this._chimeraEventListener.onBigStomp_End += delegate()
			{
				this._wreckDestroy.End(this.character);
			};
			this._chimeraEventListener.onBigStomp_Hit += delegate()
			{
				base.StartCoroutine(this._wreckDestroy.CRun(this));
			};
			this._chimeraEventListener.onDead_Pause += delegate()
			{
				this._chimeraDie.Pause(this.character);
			};
			this._chimeraEventListener.onDead_Ready += delegate()
			{
				this._chimeraDie.Ready(this.character);
			};
			this._chimeraEventListener.onDead_Start += delegate()
			{
				this._chimeraDie.Down(this.character);
			};
			this._chimeraEventListener.onDead_BreakTerrain += delegate()
			{
				this._chimeraDie.BreakTerrain(this.character);
			};
			this._chimeraEventListener.onDead_Struggle1 += delegate()
			{
				this._chimeraDie.Struggle1(this.character);
			};
			this._chimeraEventListener.onDead_Struggle2 += delegate()
			{
				this._chimeraDie.Struggle2(this.character);
			};
			this._chimeraEventListener.onDead_Fall += delegate()
			{
				this._chimeraDie.Fall(this.character);
			};
			this._chimeraEventListener.onDead_Water += delegate()
			{
				this._chimeraDie.Water(this.character, this._script);
			};
		}

		// Token: 0x0400499B RID: 18843
		[Header("Intro")]
		[Subcomponent(typeof(Intro))]
		[SerializeField]
		private Intro _intro;

		// Token: 0x0400499C RID: 18844
		[SerializeField]
		[Subcomponent(typeof(Bite))]
		[Header("Bite")]
		private Bite _bite;

		// Token: 0x0400499D RID: 18845
		[SerializeField]
		[Subcomponent(typeof(Stomp))]
		[Header("Stomp")]
		private Stomp _stomp;

		// Token: 0x0400499E RID: 18846
		[Header("VenomFall")]
		[SerializeField]
		[Subcomponent(typeof(VenomFall))]
		private VenomFall _venomFall;

		// Token: 0x0400499F RID: 18847
		[Header("VenomBall")]
		[SerializeField]
		[Subcomponent(typeof(VenomBall))]
		private VenomBall _venomBall;

		// Token: 0x040049A0 RID: 18848
		[Header("VenomCannon")]
		[SerializeField]
		[Subcomponent(typeof(VenomCannon))]
		private VenomCannon _venomCannon;

		// Token: 0x040049A1 RID: 18849
		[Header("SubjectDrop")]
		[SerializeField]
		[Subcomponent(typeof(SubjectDrop))]
		private SubjectDrop _subjectDrop;

		// Token: 0x040049A2 RID: 18850
		[Header("WreckDrop")]
		[SerializeField]
		[Subcomponent(typeof(WreckDrop))]
		private WreckDrop _wreckDrop;

		// Token: 0x040049A3 RID: 18851
		[Header("WreckDestroy")]
		[SerializeField]
		[Subcomponent(typeof(WreckDestroy))]
		private WreckDestroy _wreckDestroy;

		// Token: 0x040049A4 RID: 18852
		[Header("VenomBreath")]
		[SerializeField]
		[Subcomponent(typeof(VenomBreath))]
		private VenomBreath _venomBreath;

		// Token: 0x040049A5 RID: 18853
		[Header("Dead")]
		[SerializeField]
		[Subcomponent(typeof(ChimeraDie))]
		private ChimeraDie _chimeraDie;

		// Token: 0x040049A6 RID: 18854
		[SerializeField]
		[Header("SkippableIdle")]
		[Range(0f, 1f)]
		private float _idleSkipChance = 0.3f;

		// Token: 0x040049A7 RID: 18855
		[Header("Tools")]
		[SerializeField]
		private GameObject _freezeHead1;

		// Token: 0x040049A8 RID: 18856
		[SerializeField]
		private GameObject _freezeHead2;

		// Token: 0x040049A9 RID: 18857
		[SerializeField]
		private GameObject _freezeHead3;

		// Token: 0x040049AA RID: 18858
		[SerializeField]
		private ChimeraEventReceiver _chimeraEventListener;

		// Token: 0x040049AB RID: 18859
		[SerializeField]
		private ChimeraAnimation _chimeraAnimation;

		// Token: 0x040049AC RID: 18860
		[SerializeField]
		private ChimeraCombat _chimeraCombat;

		// Token: 0x040049AD RID: 18861
		[SerializeField]
		private Chapter3Script _script;

		// Token: 0x040049AE RID: 18862
		[SerializeField]
		private Character _darkAlchemist;
	}
}
