using System;
using System.Collections;
using Level;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DC9 RID: 3529
	public class SummonGolemOnEnable : MonoBehaviour
	{
		// Token: 0x060046E4 RID: 18148 RVA: 0x000CDD30 File Offset: 0x000CBF30
		private void OnEnable()
		{
			if (this._spawned != null)
			{
				return;
			}
			this._leftAlchemist.health.Kill();
			this._rightAlchemist.health.Kill();
			this._spawned = UnityEngine.Object.Instantiate<Character>(this._golemCharacter, this._position);
			Map.Instance.waveContainer.Attach(this._spawned);
			base.StartCoroutine(this.CDisableSign());
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x000CDDA5 File Offset: 0x000CBFA5
		private IEnumerator CDisableSign()
		{
			yield return Chronometer.global.WaitForSeconds(this._despawnTimeOfSign);
			this._sign.SetActive(false);
			yield break;
		}

		// Token: 0x040035C7 RID: 13767
		[SerializeField]
		private Character _leftAlchemist;

		// Token: 0x040035C8 RID: 13768
		[SerializeField]
		private Character _rightAlchemist;

		// Token: 0x040035C9 RID: 13769
		[SerializeField]
		private Character _golemCharacter;

		// Token: 0x040035CA RID: 13770
		[SerializeField]
		private GameObject _sign;

		// Token: 0x040035CB RID: 13771
		[SerializeField]
		private float _despawnTimeOfSign;

		// Token: 0x040035CC RID: 13772
		[SerializeField]
		private Transform _position;

		// Token: 0x040035CD RID: 13773
		private Character _spawned;
	}
}
