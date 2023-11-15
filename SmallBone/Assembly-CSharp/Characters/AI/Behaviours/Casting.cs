using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001295 RID: 4757
	public class Casting : Decorator
	{
		// Token: 0x06005E56 RID: 24150 RVA: 0x001153A4 File Offset: 0x001135A4
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			Character owner = controller.character;
			this._controller = controller;
			this._cumulativeDamage = 0.0;
			owner.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
			owner.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
			yield return this._behaviour.CRun(controller);
			owner.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06005E57 RID: 24151 RVA: 0x001153BC File Offset: 0x001135BC
		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._cumulativeDamage += damageDealt;
			if (this._cumulativeDamage >= this._breakTotalDamage)
			{
				base.result = Behaviour.Result.Fail;
				this._controller.character.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
				this._controller.StopAllCoroutinesWithBehaviour();
			}
		}

		// Token: 0x04004BCC RID: 19404
		[SerializeField]
		private double _breakTotalDamage;

		// Token: 0x04004BCD RID: 19405
		[SerializeField]
		private Behaviour _behaviour;

		// Token: 0x04004BCE RID: 19406
		private double _cumulativeDamage;

		// Token: 0x04004BCF RID: 19407
		private AIController _controller;
	}
}
