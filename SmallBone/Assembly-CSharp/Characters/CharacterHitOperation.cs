using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006C3 RID: 1731
	public class CharacterHitOperation : MonoBehaviour
	{
		// Token: 0x060022C7 RID: 8903 RVA: 0x00068D92 File Offset: 0x00066F92
		private void Awake()
		{
			this._health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x00068DAB File Offset: 0x00066FAB
		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (!this._health.dead)
			{
				base.StartCoroutine(this._hitOperations.CRun(this._character));
			}
		}

		// Token: 0x04001DA6 RID: 7590
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x04001DA7 RID: 7591
		[GetComponent]
		[SerializeField]
		private CharacterHealth _health;

		// Token: 0x04001DA8 RID: 7592
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _hitOperations;
	}
}
