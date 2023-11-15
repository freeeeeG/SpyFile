using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E14 RID: 3604
	public class DeactivateGameObject : CharacterOperation
	{
		// Token: 0x060047F4 RID: 18420 RVA: 0x000D1AB9 File Offset: 0x000CFCB9
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CRun(owner.chronometer.master));
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x000D1AD3 File Offset: 0x000CFCD3
		private IEnumerator CRun(Chronometer chronometer)
		{
			if (this._duration != 0f)
			{
				yield return chronometer.WaitForSeconds(this._duration);
			}
			this._gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0400371B RID: 14107
		[SerializeField]
		private GameObject _gameObject;

		// Token: 0x0400371C RID: 14108
		[SerializeField]
		private float _duration;
	}
}
