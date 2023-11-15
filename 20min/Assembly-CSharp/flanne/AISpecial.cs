using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200005E RID: 94
	public abstract class AISpecial : ScriptableObject
	{
		// Token: 0x0600045B RID: 1115
		public abstract void Use(AIComponent ai, Transform target);
	}
}
