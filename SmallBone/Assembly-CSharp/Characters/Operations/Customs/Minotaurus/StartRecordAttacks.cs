using System;
using System.Collections;
using Characters.Abilities.Weapons.Minotaurus;
using UnityEngine;

namespace Characters.Operations.Customs.Minotaurus
{
	// Token: 0x02001002 RID: 4098
	public sealed class StartRecordAttacks : CharacterOperation
	{
		// Token: 0x06004F33 RID: 20275 RVA: 0x000EE4AA File Offset: 0x000EC6AA
		public override void Run(Character owner)
		{
			base.StopCoroutine("CRun");
			this._passive.StartRecordingAttacks();
			base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x000EE4D0 File Offset: 0x000EC6D0
		private IEnumerator CRun(Character owner)
		{
			yield return owner.chronometer.master.WaitForSeconds(this._duration);
			this._passive.StopRecodingAttacks();
			yield break;
		}

		// Token: 0x04003F5B RID: 16219
		[SerializeField]
		private MinotaurusPassiveComponent _passive;

		// Token: 0x04003F5C RID: 16220
		[SerializeField]
		private float _duration;
	}
}
