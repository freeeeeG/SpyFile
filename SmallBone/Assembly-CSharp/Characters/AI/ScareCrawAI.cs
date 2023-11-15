using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001127 RID: 4391
	public class ScareCrawAI : AIController
	{
		// Token: 0x0600555D RID: 21853 RVA: 0x000FEC52 File Offset: 0x000FCE52
		protected override IEnumerator CProcess()
		{
			yield break;
		}

		// Token: 0x0600555E RID: 21854 RVA: 0x000FEC5A File Offset: 0x000FCE5A
		public void Appear()
		{
			this._appear.TryStart();
		}

		// Token: 0x04004465 RID: 17509
		[SerializeField]
		private Characters.Actions.Action _appear;
	}
}
