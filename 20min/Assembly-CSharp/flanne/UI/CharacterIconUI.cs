using System;
using flanne.UIExtensions;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x02000215 RID: 533
	public class CharacterIconUI : DataUI<CharacterData>
	{
		// Token: 0x06000BF8 RID: 3064 RVA: 0x0002C64A File Offset: 0x0002A84A
		protected override void SetProperties()
		{
			this.animator.runtimeAnimatorController = base.data.uiAnimController;
		}

		// Token: 0x04000855 RID: 2133
		[SerializeField]
		private Animator animator;
	}
}
