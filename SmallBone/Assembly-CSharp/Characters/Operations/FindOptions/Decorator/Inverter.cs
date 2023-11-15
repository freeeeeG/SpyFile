using System;
using UnityEngine;

namespace Characters.Operations.FindOptions.Decorator
{
	// Token: 0x02000EAD RID: 3757
	[Serializable]
	public class Inverter : ICondition
	{
		// Token: 0x060049F2 RID: 18930 RVA: 0x000D7D5B File Offset: 0x000D5F5B
		public bool Satisfied(Character character)
		{
			return !this._condition.Satisfied(character);
		}

		// Token: 0x0400392C RID: 14636
		[SerializeReference]
		[SubclassSelector]
		private ICondition _condition;
	}
}
