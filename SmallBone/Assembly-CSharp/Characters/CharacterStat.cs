using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200069A RID: 1690
	public class CharacterStat : MonoBehaviour, ICharacterStat
	{
		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060021C0 RID: 8640 RVA: 0x00065920 File Offset: 0x00063B20
		// (set) Token: 0x060021C1 RID: 8641 RVA: 0x00065928 File Offset: 0x00063B28
		public Stat stat { get; private set; }
	}
}
