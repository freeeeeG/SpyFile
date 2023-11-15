using System;
using System.Collections;
using UnityEngine;

namespace Level
{
	// Token: 0x02000514 RID: 1300
	public class ReactiveProp : MonoBehaviour
	{
		// Token: 0x060019A1 RID: 6561 RVA: 0x000507C9 File Offset: 0x0004E9C9
		private void Awake()
		{
			this._originPosition = base.transform.localPosition;
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x000507DC File Offset: 0x0004E9DC
		public void ResetPosition()
		{
			base.transform.localPosition = this._originPosition;
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x000507F0 File Offset: 0x0004E9F0
		protected void Activate()
		{
			this._flying = true;
			this._direction = Quaternion.AngleAxis(this._angle, Vector3.forward) * Vector3.right;
			if (this._direction.x < 0f)
			{
				base.transform.localScale.Set(-1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale.Set(1f, 1f, 1f);
			}
			this._destination = this._originPosition + this._direction * this._distance;
			base.StartCoroutine(this.CFlyAway());
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x000508B5 File Offset: 0x0004EAB5
		private IEnumerator CReadyToFly()
		{
			this._animator.Play("Ready");
			yield return Chronometer.global.WaitForSeconds(0.4f);
			yield break;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x000508C4 File Offset: 0x0004EAC4
		private IEnumerator CFlyAway()
		{
			float t = 0f;
			this._animator.Play("Fly");
			while (t < this._curve.duration)
			{
				float t2 = this._curve.Evaluate(t);
				base.transform.localPosition = Vector3.Lerp(this._originPosition, this._destination, t2);
				yield return null;
				t += Chronometer.global.deltaTime;
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0400166F RID: 5743
		[SerializeField]
		[GetComponent]
		protected Animator _animator;

		// Token: 0x04001670 RID: 5744
		[SerializeField]
		private float _angle;

		// Token: 0x04001671 RID: 5745
		[SerializeField]
		private float _distance;

		// Token: 0x04001672 RID: 5746
		[SerializeField]
		private Curve _curve;

		// Token: 0x04001673 RID: 5747
		private Vector2 _direction;

		// Token: 0x04001674 RID: 5748
		protected bool _flying;

		// Token: 0x04001675 RID: 5749
		private Vector3 _originPosition;

		// Token: 0x04001676 RID: 5750
		private Vector3 _destination;
	}
}
