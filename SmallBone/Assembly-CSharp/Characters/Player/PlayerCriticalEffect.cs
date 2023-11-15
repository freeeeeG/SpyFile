using System;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007F7 RID: 2039
	public class PlayerCriticalEffect : MonoBehaviour
	{
		// Token: 0x06002972 RID: 10610 RVA: 0x0007E97F File Offset: 0x0007CB7F
		private void Awake()
		{
			Character character = this._character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x0007E9A8 File Offset: 0x0007CBA8
		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (!(target.character == null))
			{
				Damage damage = tookDamage;
				if (damage.amount > 0.0 && tookDamage.critical)
				{
					Chronometer.global.AttachTimeScale(this, 0.2f, 0.1f);
					this._character.chronometer.master.AttachTimeScale(this, 5f, 0.1f);
					this._vignette.Run(this._character);
					return;
				}
			}
		}

		// Token: 0x040023A2 RID: 9122
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x040023A3 RID: 9123
		[Subcomponent(typeof(Vignette))]
		[SerializeField]
		private Vignette _vignette;
	}
}
