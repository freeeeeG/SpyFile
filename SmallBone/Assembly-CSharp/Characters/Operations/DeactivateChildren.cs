using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DD8 RID: 3544
	public class DeactivateChildren : CharacterOperation
	{
		// Token: 0x06004717 RID: 18199 RVA: 0x000CE66C File Offset: 0x000CC86C
		public override void Run(Character owner)
		{
			foreach (object obj in this._parentPool.currentEffectParent)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
		}

		// Token: 0x040035FA RID: 13818
		[SerializeField]
		private ParentPool _parentPool;
	}
}
