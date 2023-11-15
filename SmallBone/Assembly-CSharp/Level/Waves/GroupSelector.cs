using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000560 RID: 1376
	public abstract class GroupSelector : MonoBehaviour
	{
		// Token: 0x06001B25 RID: 6949
		public abstract ICollection<Character> Load();
	}
}
