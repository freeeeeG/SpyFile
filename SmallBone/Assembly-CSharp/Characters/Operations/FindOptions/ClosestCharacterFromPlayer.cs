using System;
using System.Collections.Generic;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000EA3 RID: 3747
	[Serializable]
	public class ClosestCharacterFromPlayer : IFilter
	{
		// Token: 0x060049E1 RID: 18913 RVA: 0x000D7B00 File Offset: 0x000D5D00
		public void Filtered(List<Character> characters)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			int index = 0;
			float num = 2.1474836E+09f;
			for (int i = 0; i < characters.Count; i++)
			{
				float num2 = Mathf.Abs(player.transform.position.x - characters[i].transform.position.x);
				if (num2 < num)
				{
					num = num2;
					index = i;
				}
			}
			Character item = characters[index];
			characters.Clear();
			characters.Add(item);
		}
	}
}
