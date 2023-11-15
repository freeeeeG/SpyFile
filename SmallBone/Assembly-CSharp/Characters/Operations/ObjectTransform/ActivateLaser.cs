using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FA8 RID: 4008
	public sealed class ActivateLaser : CharacterOperation
	{
		// Token: 0x06004DD0 RID: 19920 RVA: 0x000E8D32 File Offset: 0x000E6F32
		public override void Run(Character owner)
		{
			this._laser.Activate(owner, this._direction.value);
			if (this._duration > 0f)
			{
				base.StartCoroutine(this.CRun(owner));
			}
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x000E8D66 File Offset: 0x000E6F66
		private IEnumerator CRun(Character owner)
		{
			yield return owner.chronometer.master.WaitForSeconds(this._duration);
			this._laser.Deactivate();
			yield break;
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x000E8D7C File Offset: 0x000E6F7C
		public override void Stop()
		{
			base.Stop();
			base.StopAllCoroutines();
			this._laser.Deactivate();
		}

		// Token: 0x04003DBE RID: 15806
		[SerializeField]
		private Laser _laser;

		// Token: 0x04003DBF RID: 15807
		[SerializeField]
		private CustomAngle _direction;

		// Token: 0x04003DC0 RID: 15808
		[SerializeField]
		private float _duration;
	}
}
