using System;
using System.Collections.Generic;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000EA7 RID: 3751
	[Serializable]
	public class Random : IFilter
	{
		// Token: 0x060049E6 RID: 18918 RVA: 0x000D7C1C File Offset: 0x000D5E1C
		public void Filtered(List<Character> characters)
		{
			Character item = characters.Random<Character>();
			characters.Clear();
			characters.Add(item);
		}
	}
}
