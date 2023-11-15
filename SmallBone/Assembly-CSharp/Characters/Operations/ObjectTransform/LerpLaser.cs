using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FB5 RID: 4021
	public sealed class LerpLaser : CharacterOperation
	{
		// Token: 0x06004DF9 RID: 19961 RVA: 0x000E93D0 File Offset: 0x000E75D0
		public override void Run(Character owner)
		{
			this._fromValue = this._fromDirectionTest;
			this._toValue = this._toDirectionTest;
			this._laser.Activate(owner, this._fromValue);
			if (this._curve.duration > 0f)
			{
				base.StartCoroutine(this.CRun(owner));
			}
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x000E9427 File Offset: 0x000E7627
		private IEnumerator CRun(Character owner)
		{
			float elapsed = 0f;
			while (elapsed < this._curve.duration)
			{
				float direction = Mathf.Lerp(this._fromValue, this._toValue, this._curve.Evaluate(elapsed));
				this._laser.Activate(owner, direction);
				elapsed += owner.chronometer.master.deltaTime;
				yield return null;
			}
			this._laser.Deactivate();
			yield break;
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x000E943D File Offset: 0x000E763D
		public override void Stop()
		{
			base.Stop();
			base.StopAllCoroutines();
			this._laser.Deactivate();
		}

		// Token: 0x04003DF1 RID: 15857
		[SerializeField]
		private Laser _laser;

		// Token: 0x04003DF2 RID: 15858
		[SerializeField]
		private CustomAngle _fromDirection;

		// Token: 0x04003DF3 RID: 15859
		[SerializeField]
		private CustomAngle _toDirection;

		// Token: 0x04003DF4 RID: 15860
		[SerializeField]
		private float _fromDirectionTest;

		// Token: 0x04003DF5 RID: 15861
		[SerializeField]
		private float _toDirectionTest;

		// Token: 0x04003DF6 RID: 15862
		[SerializeField]
		private Curve _curve;

		// Token: 0x04003DF7 RID: 15863
		private float _fromValue;

		// Token: 0x04003DF8 RID: 15864
		private float _toValue;
	}
}
