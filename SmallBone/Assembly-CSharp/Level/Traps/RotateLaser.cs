using System;
using System.Collections;
using Characters;
using Characters.Operations;
using Characters.Operations.Attack;
using Characters.Utils;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000677 RID: 1655
	public sealed class RotateLaser : MonoBehaviour
	{
		// Token: 0x0600211F RID: 8479 RVA: 0x00063D80 File Offset: 0x00061F80
		private void Awake()
		{
			this._hitHistoryManager = new HitHistoryManager(99999);
			foreach (SweepAttack sweepAttack in this._attackOperations)
			{
				sweepAttack.Initialize();
				sweepAttack.collisionDetector.hits = this._hitHistoryManager;
			}
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x00063DCB File Offset: 0x00061FCB
		private void OnDestroy()
		{
			this._endAnimation = null;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00063DD4 File Offset: 0x00061FD4
		public void Activate(OperationInfos operationInfos)
		{
			this._owner = operationInfos.owner;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00063DEE File Offset: 0x00061FEE
		public void Activate(Character owner)
		{
			this._owner = owner;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x00063E03 File Offset: 0x00062003
		private void OnEnable()
		{
			this.ResetSetting();
			base.StartCoroutine(this.CStart(this._owner.chronometer.master));
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x00063E28 File Offset: 0x00062028
		private IEnumerator CStart(Chronometer chronometer)
		{
			yield return chronometer.WaitForSeconds(this._rotate.delay);
			base.StartCoroutine(this.CLoop(chronometer));
			yield break;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x00063E3E File Offset: 0x0006203E
		private IEnumerator CLoop(Chronometer chronometer)
		{
			SweepAttack[] attackOperations = this._attackOperations;
			for (int i = 0; i < attackOperations.Length; i++)
			{
				attackOperations[i].Run(this._owner);
			}
			float elapsed = 0f;
			while (elapsed < this._loopTime)
			{
				if (this._speed < this._rotate.amount)
				{
					this._speed += this._rotate.delta;
				}
				this._body.transform.Rotate(Vector3.forward, (float)this._direction * this._speed * chronometer.deltaTime);
				elapsed += chronometer.deltaTime;
				yield return null;
			}
			base.StartCoroutine(this.CEnd(chronometer));
			yield break;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x00063E54 File Offset: 0x00062054
		private IEnumerator CEnd(Chronometer chronometer)
		{
			yield return chronometer.WaitForSeconds(this._endAnimation.length);
			this.Hide();
			yield break;
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x00063E6A File Offset: 0x0006206A
		private void ResetSetting()
		{
			this._speed = 0f;
			this._body.rotation = Quaternion.identity;
			this._direction = (MMMaths.RandomBool() ? 1 : -1);
			this._hitHistoryManager.Clear();
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000075E7 File Offset: 0x000057E7
		private void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04001C37 RID: 7223
		[SerializeField]
		private SweepAttack[] _attackOperations;

		// Token: 0x04001C38 RID: 7224
		[SerializeField]
		private Transform _body;

		// Token: 0x04001C39 RID: 7225
		[SerializeField]
		private RotateLaser.Rotate _rotate;

		// Token: 0x04001C3A RID: 7226
		[SerializeField]
		private float _loopTime;

		// Token: 0x04001C3B RID: 7227
		[SerializeField]
		private AnimationClip _endAnimation;

		// Token: 0x04001C3C RID: 7228
		private Character _owner;

		// Token: 0x04001C3D RID: 7229
		private HitHistoryManager _hitHistoryManager;

		// Token: 0x04001C3E RID: 7230
		private int _direction;

		// Token: 0x04001C3F RID: 7231
		private float _speed;

		// Token: 0x02000678 RID: 1656
		[Serializable]
		private class Rotate
		{
			// Token: 0x04001C40 RID: 7232
			[SerializeField]
			internal float delay;

			// Token: 0x04001C41 RID: 7233
			[SerializeField]
			internal float amount;

			// Token: 0x04001C42 RID: 7234
			[SerializeField]
			internal float delta;
		}
	}
}
