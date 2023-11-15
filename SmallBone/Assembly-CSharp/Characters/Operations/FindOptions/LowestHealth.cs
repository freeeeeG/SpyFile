using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000EA5 RID: 3749
	[Serializable]
	public class LowestHealth : IFilter
	{
		// Token: 0x060049E4 RID: 18916 RVA: 0x000D7B8C File Offset: 0x000D5D8C
		public void Filtered(List<Character> characters)
		{
			double num = 2147483647.0;
			Character item = null;
			foreach (Character character in characters)
			{
				double num2;
				if (this._healthType == LowestHealth.HealthType.Constant)
				{
					num2 = character.health.currentHealth;
				}
				else
				{
					num2 = character.health.percent;
				}
				if (num2 < num)
				{
					num = num2;
					item = character;
				}
			}
			characters.Clear();
			characters.Add(item);
		}

		// Token: 0x04003921 RID: 14625
		[SerializeField]
		private LowestHealth.HealthType _healthType;

		// Token: 0x02000EA6 RID: 3750
		private enum HealthType
		{
			// Token: 0x04003923 RID: 14627
			Percent,
			// Token: 0x04003924 RID: 14628
			Constant
		}
	}
}
