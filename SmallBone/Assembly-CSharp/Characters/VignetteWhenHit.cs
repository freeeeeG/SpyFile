using System;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000731 RID: 1841
	public class VignetteWhenHit : MonoBehaviour
	{
		// Token: 0x06002573 RID: 9587 RVA: 0x00070EAB File Offset: 0x0006F0AB
		private void Awake()
		{
			this._owner.health.onTookDamage += new TookDamageDelegate(this.onTookDamage);
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x00070EC9 File Offset: 0x0006F0C9
		private void onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (damageDealt > 0.0 && tookDamage.attackType != Damage.AttackType.Additional)
			{
				this._vignette.Run(this._owner);
			}
		}

		// Token: 0x04001FCB RID: 8139
		[SerializeField]
		[GetComponent]
		private Character _owner;

		// Token: 0x04001FCC RID: 8140
		[Subcomponent(typeof(Vignette))]
		[SerializeField]
		private Vignette _vignette;
	}
}
