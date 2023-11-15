using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Adventurer.Magician
{
	// Token: 0x020013F1 RID: 5105
	public class FireJadeController : AIController
	{
		// Token: 0x06006482 RID: 25730 RVA: 0x000FCBBB File Offset: 0x000FADBB
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06006483 RID: 25731 RVA: 0x00123A2B File Offset: 0x00121C2B
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			this._attack.TryStart();
			yield break;
		}

		// Token: 0x04005115 RID: 20757
		[SerializeField]
		private Characters.Actions.Action _attack;
	}
}
