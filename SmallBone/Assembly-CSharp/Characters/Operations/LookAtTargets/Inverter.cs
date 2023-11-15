using System;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000F01 RID: 3841
	public class Inverter : Target
	{
		// Token: 0x06004B2A RID: 19242 RVA: 0x000DD443 File Offset: 0x000DB643
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			if (this._target.GetDirectionFrom(character) != Character.LookingDirection.Right)
			{
				return Character.LookingDirection.Right;
			}
			return Character.LookingDirection.Left;
		}

		// Token: 0x04003A59 RID: 14937
		[Target.SubcomponentAttribute]
		[SerializeField]
		private Target _target;
	}
}
