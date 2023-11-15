using System;
using System.Collections;
using Characters;
using FX;
using Singletons;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000F2 RID: 242
	public class Witch : MonoBehaviour
	{
		// Token: 0x060004AA RID: 1194 RVA: 0x0000F1D7 File Offset: 0x0000D3D7
		public IEnumerator EscapeCage()
		{
			this._animator.Play("Idle_Human");
			yield return Chronometer.global.WaitForSeconds(1f);
			yield break;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000F1E6 File Offset: 0x0000D3E6
		public IEnumerator TurnIntoCat()
		{
			this._animator.Play("Polymorph_Cat");
			yield return Chronometer.global.WaitForSeconds(2f);
			this._summon.gameObject.SetActive(true);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._meowSoundInfo, base.transform.position);
			yield break;
		}

		// Token: 0x04000396 RID: 918
		[SerializeField]
		private Animator _animator;

		// Token: 0x04000397 RID: 919
		[SerializeField]
		private SoundInfo _meowSoundInfo;

		// Token: 0x04000398 RID: 920
		[SerializeField]
		private Character _summon;
	}
}
