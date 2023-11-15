using System;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000F07 RID: 3847
	public sealed class TargetObject : Target
	{
		// Token: 0x06004B34 RID: 19252 RVA: 0x000DD632 File Offset: 0x000DB832
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			if (this._target.position.x >= character.transform.position.x)
			{
				return Character.LookingDirection.Right;
			}
			return Character.LookingDirection.Left;
		}

		// Token: 0x04003A60 RID: 14944
		[SerializeField]
		private Transform _target;
	}
}
