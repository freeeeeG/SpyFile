using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.AI.Pope;
using Characters.Operations;
using CutScenes.Objects.Chapter4;
using Data;
using Level.Pope;
using Runnables;
using Scenes;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Chapter4
{
	// Token: 0x02000641 RID: 1601
	public class Scenario : MonoBehaviour
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x0600201C RID: 8220 RVA: 0x000612C0 File Offset: 0x0005F4C0
		// (remove) Token: 0x0600201D RID: 8221 RVA: 0x000612F8 File Offset: 0x0005F4F8
		public event Action OnPhase1Start;

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x0600201E RID: 8222 RVA: 0x00061330 File Offset: 0x0005F530
		// (remove) Token: 0x0600201F RID: 8223 RVA: 0x00061368 File Offset: 0x0005F568
		public event Action OnPhase1End;

		// Token: 0x06002020 RID: 8224 RVA: 0x0006139D File Offset: 0x0005F59D
		private bool IsCenterPlatform(Transform target)
		{
			return target == this._centerPlatform;
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x000613AC File Offset: 0x0005F5AC
		private void Start()
		{
			this._darkCrystalLeft.health.onDied += this.TryStart2Phase_Left;
			this._darkCrystalRight.health.onDied += this.TryStart2Phase_Right;
			this._chest.OnOpen += delegate()
			{
				this._gate.SetActive(true);
				UnityEvent onChestOpend = this._onChestOpend;
				if (onChestOpend == null)
				{
					return;
				}
				onChestOpend.Invoke();
			};
			if (GameData.HardmodeProgress.hardmode)
			{
				this._targets = new HashSet<Transform>();
			}
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0006141A File Offset: 0x0005F61A
		private IEnumerator CProcessInHardmodePhase2()
		{
			while (Scene<GameBase>.instance.uiManager.letterBox.visible)
			{
				yield return null;
			}
			while (!this.popeAI.character.health.dead)
			{
				yield return Chronometer.global.WaitForSeconds(this._phase2EnvironmentsInterval.value);
				if (this.popeAI.character.health.dead)
				{
					yield break;
				}
				this._floorPlatforms.Shuffle<Scenario.Floor>();
				this._targets.Clear();
				Scenario.Floor[] floorPlatforms = this._floorPlatforms;
				for (int i = 0; i < floorPlatforms.Length; i++)
				{
					Transform transform = floorPlatforms[i].FindLastStandingPlatform();
					if (!(transform == null))
					{
						this._attackPoint.position = transform.position;
						if (this.IsCenterPlatform(transform))
						{
							base.StartCoroutine(this._phase2EnvironmentsCenterInHardmode.CRun(this.popeAI.character));
						}
						else
						{
							base.StartCoroutine(this._phase2EnvironmentsInHardmode.CRun(this.popeAI.character));
						}
						this._targets.Add(transform);
						break;
					}
				}
				int count = this._targets.Count;
				foreach (Scenario.Floor floor in this._floorPlatforms)
				{
					if (floor.floorValue == 2 || floor.floorValue == 4)
					{
						if (this._targets.Count >= 1)
						{
							Transform obj = this._targets.Random<Transform>();
							bool flag = false;
							Platform[] platforms = floor.platforms;
							for (int j = 0; j < platforms.Length; j++)
							{
								if (platforms[j].transform.Equals(obj))
								{
									flag = true;
								}
							}
							if (flag)
							{
								goto IL_249;
							}
						}
						Transform randomPlatform = floor.GetRandomPlatform();
						this._attackPoint.position = randomPlatform.position;
						if (this.IsCenterPlatform(randomPlatform))
						{
							base.StartCoroutine(this._phase2EnvironmentsCenterInHardmode.CRun(this.popeAI.character));
						}
						else
						{
							base.StartCoroutine(this._phase2EnvironmentsInHardmode.CRun(this.popeAI.character));
						}
						this._targets.Add(randomPlatform);
						break;
					}
					IL_249:;
				}
				foreach (Scenario.Floor floor2 in this._floorPlatforms)
				{
					bool flag2 = false;
					foreach (Platform platform in floor2.platforms)
					{
						if (this._targets.Contains(platform.transform))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						Transform randomPlatform2 = floor2.GetRandomPlatform();
						this._attackPoint.position = randomPlatform2.position;
						if (this.IsCenterPlatform(randomPlatform2))
						{
							base.StartCoroutine(this._phase2EnvironmentsCenterInHardmode.CRun(this.popeAI.character));
						}
						else
						{
							base.StartCoroutine(this._phase2EnvironmentsInHardmode.CRun(this.popeAI.character));
						}
						this._targets.Add(randomPlatform2);
						if (this._targets.Count >= 3)
						{
							break;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x0006142C File Offset: 0x0005F62C
		private void TryStart2Phase_Left()
		{
			this._cleansing.Run();
			this._darkCrystalLeft.health.onDied -= this.TryStart2Phase_Left;
			if (!this._darkCrystalRight.health.dead)
			{
				this._barrier.Crack();
				return;
			}
			Action onPhase1End = this.OnPhase1End;
			if (onPhase1End != null)
			{
				onPhase1End();
			}
			this.StopDoing();
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x00061498 File Offset: 0x0005F698
		private void TryStart2Phase_Right()
		{
			this._cleansing.Run();
			this._darkCrystalRight.health.onDied -= this.TryStart2Phase_Right;
			if (!this._darkCrystalLeft.health.dead)
			{
				this._barrier.Crack();
				return;
			}
			Action onPhase1End = this.OnPhase1End;
			if (onPhase1End != null)
			{
				onPhase1End();
			}
			this.StopDoing();
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x00061504 File Offset: 0x0005F704
		private void StopDoing()
		{
			this.popeAI.StopAllCoroutinesWithBehaviour();
			foreach (Character character in Map.Instance.waveContainer.GetAllEnemies())
			{
				if (!(this.popeAI.character == character))
				{
					character.health.Kill();
				}
			}
			this.fanaticFactory.StopToSummon();
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x00061590 File Offset: 0x0005F790
		public void Start1Phase()
		{
			Action onPhase1Start = this.OnPhase1Start;
			if (onPhase1Start != null)
			{
				onPhase1Start();
			}
			this.popeAI.StartCombat();
			this.fanaticFactory.StartToSummon();
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x000615BC File Offset: 0x0005F7BC
		public void Start2Phase()
		{
			this._platformContainer.Appear();
			this._chair.Hide();
			this._fire.Appear();
			this.ZoomOut();
			this.popeAI.NextPhase();
			if (GameData.HardmodeProgress.hardmode)
			{
				base.StartCoroutine(this.CProcessInHardmodePhase2());
			}
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0006160F File Offset: 0x0005F80F
		private void ZoomOut()
		{
			Scene<GameBase>.instance.cameraController.Zoom(1.3f, 1f);
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0006162A File Offset: 0x0005F82A
		private void ZoomIn()
		{
			Scene<GameBase>.instance.cameraController.Zoom(1f, 10f);
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x00061645 File Offset: 0x0005F845
		private void OnDestroy()
		{
			this.ZoomIn();
		}

		// Token: 0x04001B37 RID: 6967
		[SerializeField]
		private UnityEvent _onChestOpend;

		// Token: 0x04001B38 RID: 6968
		[SerializeField]
		private GameObject _gate;

		// Token: 0x04001B39 RID: 6969
		[SerializeField]
		private BossChest _chest;

		// Token: 0x04001B3A RID: 6970
		[SerializeField]
		private PopeAI popeAI;

		// Token: 0x04001B3B RID: 6971
		[SerializeField]
		[Header("Phase 1")]
		private Barrier _barrier;

		// Token: 0x04001B3C RID: 6972
		[SerializeField]
		private FanaticFactory fanaticFactory;

		// Token: 0x04001B3D RID: 6973
		[SerializeField]
		private Character _darkCrystalLeft;

		// Token: 0x04001B3E RID: 6974
		[SerializeField]
		[Space]
		private Character _darkCrystalRight;

		// Token: 0x04001B3F RID: 6975
		[Header("Phase 2")]
		[SerializeField]
		private PlatformContainer _platformContainer;

		// Token: 0x04001B40 RID: 6976
		[SerializeField]
		private Chair _chair;

		// Token: 0x04001B41 RID: 6977
		[SerializeField]
		private Fire _fire;

		// Token: 0x04001B42 RID: 6978
		[SerializeField]
		private Cleansing _cleansing;

		// Token: 0x04001B43 RID: 6979
		[Header("하드모드")]
		[SerializeField]
		private Scenario.Floor[] _floorPlatforms;

		// Token: 0x04001B44 RID: 6980
		[SerializeField]
		private Transform _centerPlatform;

		// Token: 0x04001B45 RID: 6981
		[SerializeField]
		private Transform _attackPoint;

		// Token: 0x04001B46 RID: 6982
		[SerializeField]
		private CustomFloat _phase2EnvironmentsInterval;

		// Token: 0x04001B47 RID: 6983
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _phase2EnvironmentsCenterInHardmode;

		// Token: 0x04001B48 RID: 6984
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _phase2EnvironmentsInHardmode;

		// Token: 0x04001B49 RID: 6985
		private HashSet<Transform> _targets;

		// Token: 0x04001B4A RID: 6986
		private const int maxAttackCount = 3;

		// Token: 0x02000642 RID: 1602
		[Serializable]
		private class Floor
		{
			// Token: 0x170006C0 RID: 1728
			// (get) Token: 0x0600202D RID: 8237 RVA: 0x0006166B File Offset: 0x0005F86B
			public int floorValue
			{
				get
				{
					return this._floorValue;
				}
			}

			// Token: 0x170006C1 RID: 1729
			// (get) Token: 0x0600202E RID: 8238 RVA: 0x00061673 File Offset: 0x0005F873
			public Platform[] platforms
			{
				get
				{
					return this._platforms;
				}
			}

			// Token: 0x0600202F RID: 8239 RVA: 0x0006167B File Offset: 0x0005F87B
			public Transform GetRandomPlatform()
			{
				return this._platforms.Random<Platform>().transform;
			}

			// Token: 0x06002030 RID: 8240 RVA: 0x00061690 File Offset: 0x0005F890
			public Transform FindLastStandingPlatform()
			{
				Collider2D lastStandingCollider = Singleton<Service>.Instance.levelManager.player.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					return null;
				}
				foreach (Platform platform in this._platforms)
				{
					if (platform.collider == lastStandingCollider)
					{
						return platform.transform;
					}
				}
				return null;
			}

			// Token: 0x04001B4B RID: 6987
			[SerializeField]
			private int _floorValue;

			// Token: 0x04001B4C RID: 6988
			[SerializeField]
			private Platform[] _platforms;
		}
	}
}
