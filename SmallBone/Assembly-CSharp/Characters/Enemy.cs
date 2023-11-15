using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000749 RID: 1865
	public sealed class Enemy : MonoBehaviour
	{
		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060025F5 RID: 9717 RVA: 0x000728A7 File Offset: 0x00070AA7
		public ICollection<Character> characters
		{
			get
			{
				return this._characters;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060025F6 RID: 9718 RVA: 0x000728AF File Offset: 0x00070AAF
		public ICollection<Behavior> behaviors
		{
			get
			{
				return this._behaviors;
			}
		}

		// Token: 0x040020B3 RID: 8371
		[SerializeField]
		private Character[] _characters;

		// Token: 0x040020B4 RID: 8372
		[SerializeField]
		private Behavior[] _behaviors;
	}
}
