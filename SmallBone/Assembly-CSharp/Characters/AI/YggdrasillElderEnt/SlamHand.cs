using System;
using System.Collections;
using Characters.Operations;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x02001134 RID: 4404
	public class SlamHand : MonoBehaviour
	{
		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x0600559C RID: 21916 RVA: 0x000FF65D File Offset: 0x000FD85D
		// (set) Token: 0x0600559D RID: 21917 RVA: 0x000FF665 File Offset: 0x000FD865
		public Vector3 destination { private get; set; }

		// Token: 0x0600559E RID: 21918 RVA: 0x000FF670 File Offset: 0x000FD870
		private void Start()
		{
			this._origin = base.transform.position;
			this._onSign.Initialize();
			this._onSlamStart.Initialize();
			this._onSlamEnd.Initialize();
			this._onRecoverySign.Initialize();
			this._onRecovery.Initialize();
			this._onEnd.Initialize();
		}

		// Token: 0x0600559F RID: 21919 RVA: 0x0001913A File Offset: 0x0001733A
		public void ActiavteHand()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x000075E7 File Offset: 0x000057E7
		public void DeactivateHand()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x000FF6D0 File Offset: 0x000FD8D0
		public void Sign()
		{
			this._onSign.gameObject.SetActive(true);
			this._onSign.Run(this._ai.character);
		}

		// Token: 0x060055A2 RID: 21922 RVA: 0x000FF6F9 File Offset: 0x000FD8F9
		public IEnumerator CSlam()
		{
			this._source = this._origin;
			yield return Chronometer.global.WaitForSeconds(this._attackDelayFromtargeting);
			this.StartSlam();
			yield return this.CMoveTarget(this._fistSlamAttackLength);
			this.EndSlam();
			yield break;
		}

		// Token: 0x060055A3 RID: 21923 RVA: 0x000FF708 File Offset: 0x000FD908
		private void StartSlam()
		{
			this._onSlamStart.gameObject.SetActive(true);
			this._onSlamStart.Run(this._ai.character);
			this.StartCollisionDetect();
		}

		// Token: 0x060055A4 RID: 21924 RVA: 0x000FF738 File Offset: 0x000FD938
		private void EndSlam()
		{
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._onSlamEndInHardmode.gameObject.SetActive(true);
				this._onSlamEndInHardmode.Run(this._ai.character);
			}
			else
			{
				this._onSlamEnd.gameObject.SetActive(true);
				this._onSlamEnd.Run(this._ai.character);
			}
			this.ActivateTerrain();
			this._collisionDetector.Stop();
		}

		// Token: 0x060055A5 RID: 21925 RVA: 0x000FF7B2 File Offset: 0x000FD9B2
		public IEnumerator CVibrate()
		{
			float elapsedTime = 0f;
			float length = 0.45f;
			float shakeAmount = 0.25f;
			CharacterChronometer chronometer = this._ai.character.chronometer;
			for (;;)
			{
				this._sprite.transform.localPosition = UnityEngine.Random.insideUnitSphere * shakeAmount;
				elapsedTime += chronometer.animation.deltaTime;
				if (elapsedTime > length)
				{
					break;
				}
				yield return null;
			}
			this._sprite.transform.localPosition = Vector3.zero;
			yield break;
		}

		// Token: 0x060055A6 RID: 21926 RVA: 0x000FF7C1 File Offset: 0x000FD9C1
		public IEnumerator CRecover()
		{
			this._source = base.transform.position;
			this.destination = this._origin;
			base.StopCoroutine(this._cChangeToTerrainReference);
			this.DeactivateTerrain();
			yield return this.CMoveTarget(this._fistSlamRecoveryLength);
			yield break;
		}

		// Token: 0x060055A7 RID: 21927 RVA: 0x000FF7D0 File Offset: 0x000FD9D0
		private IEnumerator CMoveTarget(float length)
		{
			float elapsedTime = 0f;
			CharacterChronometer chronometer = this._ai.character.chronometer;
			Vector3 dest = this.destination;
			for (;;)
			{
				Vector2 v = Vector2.Lerp(this._source, dest, elapsedTime / length);
				base.transform.position = v;
				elapsedTime += chronometer.animation.deltaTime;
				if (elapsedTime > length)
				{
					break;
				}
				yield return null;
			}
			base.transform.position = dest;
			yield break;
		}

		// Token: 0x060055A8 RID: 21928 RVA: 0x000FF7E6 File Offset: 0x000FD9E6
		private void ActivateTerrain()
		{
			this._cChangeToTerrainReference = base.StartCoroutine(this.CChangeTerrain());
		}

		// Token: 0x060055A9 RID: 21929 RVA: 0x000FF7FA File Offset: 0x000FD9FA
		private IEnumerator CChangeTerrain()
		{
			for (;;)
			{
				yield return null;
				UnityEngine.Object x = this._ai.FindClosestPlayerBody(this._collider);
				this._collider.enabled = true;
				if (!(x != null))
				{
					this._terrainCollider.gameObject.SetActive(true);
				}
			}
			yield break;
		}

		// Token: 0x060055AA RID: 21930 RVA: 0x000FF809 File Offset: 0x000FDA09
		private void DeactivateTerrain()
		{
			this._terrainCollider.gameObject.SetActive(false);
		}

		// Token: 0x060055AB RID: 21931 RVA: 0x000FF81C File Offset: 0x000FDA1C
		private void StartCollisionDetect()
		{
			this._collisionDetector.Initialize(this._monsterBody, this._collider);
			base.StartCoroutine(this._collisionDetector.CRun(base.transform));
		}

		// Token: 0x0400449B RID: 17563
		[SerializeField]
		[FrameTime]
		private float _fistSlamAttackLength = 0.16f;

		// Token: 0x0400449C RID: 17564
		[SerializeField]
		[FrameTime]
		private float _fistSlamRecoveryLength = 0.66f;

		// Token: 0x0400449D RID: 17565
		[SerializeField]
		[FrameTime]
		private float _attackDelayFromtargeting = 0.16f;

		// Token: 0x0400449E RID: 17566
		[SerializeField]
		[GetComponent]
		private Collider2D _collider;

		// Token: 0x0400449F RID: 17567
		[SerializeField]
		private AIController _ai;

		// Token: 0x040044A0 RID: 17568
		[SerializeField]
		private GameObject _monsterBody;

		// Token: 0x040044A1 RID: 17569
		[SerializeField]
		private YggdrasillElderEntCollisionDetector _collisionDetector;

		// Token: 0x040044A2 RID: 17570
		[SerializeField]
		[Header("For Terrain")]
		private SpriteRenderer _sprite;

		// Token: 0x040044A3 RID: 17571
		[SerializeField]
		private Collider2D _terrainCollider;

		// Token: 0x040044A4 RID: 17572
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onSign;

		// Token: 0x040044A5 RID: 17573
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onSlamStart;

		// Token: 0x040044A6 RID: 17574
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onSlamEnd;

		// Token: 0x040044A7 RID: 17575
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onSlamEndInHardmode;

		// Token: 0x040044A8 RID: 17576
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onRecoverySign;

		// Token: 0x040044A9 RID: 17577
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onRecovery;

		// Token: 0x040044AA RID: 17578
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onEnd;

		// Token: 0x040044AC RID: 17580
		private Coroutine _cChangeToTerrainReference;

		// Token: 0x040044AD RID: 17581
		private Vector3 _origin;

		// Token: 0x040044AE RID: 17582
		private Vector3 _source;
	}
}
