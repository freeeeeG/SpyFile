using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200066A RID: 1642
	[RequireComponent(typeof(Character))]
	public class ModifyDamageAttacher : MonoBehaviour
	{
		// Token: 0x060020EB RID: 8427 RVA: 0x000635A2 File Offset: 0x000617A2
		private void Start()
		{
			this._abilityAttacher.Initialize(this._character);
			this._abilityAttacher.StartAttach();
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000635C0 File Offset: 0x000617C0
		private void OnDestroy()
		{
			this._abilityAttacher.StopAttach();
		}

		// Token: 0x04001C00 RID: 7168
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x04001C01 RID: 7169
		[AbilityAttacher.SubcomponentAttribute]
		[SerializeField]
		private AbilityAttacher _abilityAttacher;
	}
}
