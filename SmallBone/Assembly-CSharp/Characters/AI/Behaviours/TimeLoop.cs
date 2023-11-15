using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012BA RID: 4794
	public class TimeLoop : Decorator
	{
		// Token: 0x06005EEC RID: 24300 RVA: 0x0011649F File Offset: 0x0011469F
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			base.StartCoroutine(this.CExpire());
			while (this._running)
			{
				yield return this._behaviour.CRun(controller);
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06005EED RID: 24301 RVA: 0x001164B5 File Offset: 0x001146B5
		private IEnumerator CExpire()
		{
			this._running = true;
			yield return Chronometer.global.WaitForSeconds((float)this._time);
			this._running = false;
			yield break;
		}

		// Token: 0x04004C40 RID: 19520
		[SerializeField]
		private int _time;

		// Token: 0x04004C41 RID: 19521
		[Behaviour.SubcomponentAttribute(true)]
		[SerializeField]
		private Behaviour _behaviour;

		// Token: 0x04004C42 RID: 19522
		private bool _running;
	}
}
