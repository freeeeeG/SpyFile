using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000FD RID: 253
	[CreateAssetMenu(fileName = "PowerupProfile", menuName = "PowerupProfile", order = 2)]
	public class PowerupPoolProfile : ScriptableObject
	{
		// Token: 0x04000522 RID: 1314
		public List<Powerup> powerupPool;
	}
}
