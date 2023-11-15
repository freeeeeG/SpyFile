using System;
using System.Collections;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x0200113A RID: 4410
	public class SlamHandController : MonoBehaviour
	{
		// Token: 0x060055CB RID: 21963 RVA: 0x000FFC49 File Offset: 0x000FDE49
		private void Awake()
		{
			this._owenrHealth.onDie += this.DisableHands;
		}

		// Token: 0x060055CC RID: 21964 RVA: 0x000FFC62 File Offset: 0x000FDE62
		public void Ready()
		{
			this._left.ActiavteHand();
			this._right.ActiavteHand();
			base.StartCoroutine(this.CsetDestination());
		}

		// Token: 0x060055CD RID: 21965 RVA: 0x000FFC87 File Offset: 0x000FDE87
		public void Slam()
		{
			base.StartCoroutine(this._current.CSlam());
		}

		// Token: 0x060055CE RID: 21966 RVA: 0x000FFC9B File Offset: 0x000FDE9B
		public void Recover()
		{
			base.StartCoroutine(this._current.CRecover());
		}

		// Token: 0x060055CF RID: 21967 RVA: 0x000FFCAF File Offset: 0x000FDEAF
		public void Vibrate()
		{
			base.StartCoroutine(this._current.CVibrate());
		}

		// Token: 0x060055D0 RID: 21968 RVA: 0x000FFCC3 File Offset: 0x000FDEC3
		public void DisableHands()
		{
			this._left.DeactivateHand();
			this._right.DeactivateHand();
		}

		// Token: 0x060055D1 RID: 21969 RVA: 0x000FFCDC File Offset: 0x000FDEDC
		private void SetHand()
		{
			float x = Singleton<Service>.Instance.levelManager.player.transform.position.x;
			this._current = ((Mathf.Abs(x - this._left.transform.position.x) < Mathf.Abs(x - this._right.transform.position.x)) ? this._left : this._right);
		}

		// Token: 0x060055D2 RID: 21970 RVA: 0x000FFD5A File Offset: 0x000FDF5A
		private IEnumerator CsetDestination()
		{
			yield return Chronometer.global.WaitForSeconds(this._targetingDelay);
			this.SetHand();
			this.SetDestination();
			yield break;
		}

		// Token: 0x060055D3 RID: 21971 RVA: 0x000FFD6C File Offset: 0x000FDF6C
		private void SetDestination()
		{
			Transform transform = Singleton<Service>.Instance.levelManager.player.transform;
			this._current.destination = new Vector3(transform.position.x, Map.Instance.bounds.max.y);
		}

		// Token: 0x040044C6 RID: 17606
		[SerializeField]
		private float _targetingDelay = 0.64f;

		// Token: 0x040044C7 RID: 17607
		[SerializeField]
		private Health _owenrHealth;

		// Token: 0x040044C8 RID: 17608
		[SerializeField]
		private SlamHand _left;

		// Token: 0x040044C9 RID: 17609
		[SerializeField]
		private SlamHand _right;

		// Token: 0x040044CA RID: 17610
		private SlamHand _current;
	}
}
