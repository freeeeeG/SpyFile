using System;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000EFD RID: 3837
	public sealed class Chance : Target
	{
		// Token: 0x06004B22 RID: 19234 RVA: 0x000DD26C File Offset: 0x000DB46C
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			if (!MMMaths.Chance(this._lookRightChance))
			{
				return Character.LookingDirection.Left;
			}
			return Character.LookingDirection.Right;
		}

		// Token: 0x04003A55 RID: 14933
		[SerializeField]
		[Range(0f, 1f)]
		private float _lookRightChance = 0.5f;
	}
}
