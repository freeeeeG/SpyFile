using System;
using Level;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FF5 RID: 4085
	public sealed class SpawnPropAtTarget : TargetedCharacterOperation
	{
		// Token: 0x06004EE5 RID: 20197 RVA: 0x000ECE25 File Offset: 0x000EB025
		public override void Run(Character owner, Character target)
		{
			UnityEngine.Object.Instantiate<Prop>(this._prop, target.transform.position, Quaternion.identity, Map.Instance.transform);
		}

		// Token: 0x04003F06 RID: 16134
		[SerializeField]
		private Prop _prop;
	}
}
