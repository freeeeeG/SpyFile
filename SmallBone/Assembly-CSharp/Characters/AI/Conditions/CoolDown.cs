using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011C9 RID: 4553
	public class CoolDown : Condition
	{
		// Token: 0x06005984 RID: 22916 RVA: 0x0010A6DA File Offset: 0x001088DA
		protected override bool Check(AIController controller)
		{
			if (!this._canUse)
			{
				return false;
			}
			base.StartCoroutine(this.CCoolDown(controller.character.chronometer.master));
			return true;
		}

		// Token: 0x06005985 RID: 22917 RVA: 0x0010A704 File Offset: 0x00108904
		private IEnumerator CCoolDown(Chronometer chronometer)
		{
			this._canUse = false;
			yield return chronometer.WaitForSeconds(this._coolTime);
			this._canUse = true;
			yield break;
		}

		// Token: 0x0400484A RID: 18506
		[SerializeField]
		private float _coolTime;

		// Token: 0x0400484B RID: 18507
		private bool _canUse = true;
	}
}
