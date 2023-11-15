using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000E9E RID: 3742
	[Serializable]
	public class EqualMovementType : ICondition
	{
		// Token: 0x060049D8 RID: 18904 RVA: 0x000D7A49 File Offset: 0x000D5C49
		public bool Satisfied(Character character)
		{
			return character.movement.config.type == this._movementType;
		}

		// Token: 0x0400391C RID: 14620
		[SerializeField]
		private Movement.Config.Type _movementType;
	}
}
