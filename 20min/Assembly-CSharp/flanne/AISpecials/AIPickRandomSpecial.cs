using System;
using UnityEngine;

namespace flanne.AISpecials
{
	// Token: 0x02000263 RID: 611
	[CreateAssetMenu(fileName = "AIPickRandomSpecial", menuName = "AISpecials/AIPickRandomSpecial")]
	public class AIPickRandomSpecial : AISpecial
	{
		// Token: 0x06000D35 RID: 3381 RVA: 0x00030210 File Offset: 0x0002E410
		public override void Use(AIComponent ai, Transform target)
		{
			this.specialPool[Random.Range(0, this.specialPool.Length)].Use(ai, target);
		}

		// Token: 0x04000995 RID: 2453
		[SerializeField]
		private AISpecial[] specialPool;
	}
}
