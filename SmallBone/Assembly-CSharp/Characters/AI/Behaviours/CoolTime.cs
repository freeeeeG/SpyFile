using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012AC RID: 4780
	public class CoolTime : Decorator
	{
		// Token: 0x06005EB3 RID: 24243 RVA: 0x00115F6B File Offset: 0x0011416B
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (!this._canRun)
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			base.StartCoroutine(this.CCooldown(controller.character.chronometer.master));
			yield return this._behaviour.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06005EB4 RID: 24244 RVA: 0x00115F81 File Offset: 0x00114181
		private IEnumerator CCooldown(Chronometer chronometer)
		{
			this._canRun = false;
			yield return chronometer.WaitForSeconds(this._value);
			this._canRun = true;
			yield break;
		}

		// Token: 0x04004C14 RID: 19476
		[SerializeField]
		private float _value;

		// Token: 0x04004C15 RID: 19477
		private bool _canRun = true;

		// Token: 0x04004C16 RID: 19478
		[SerializeField]
		[Behaviour.SubcomponentAttribute(true)]
		private Behaviour _behaviour;
	}
}
