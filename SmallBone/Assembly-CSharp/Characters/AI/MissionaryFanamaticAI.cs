using System;
using System.Collections;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010DD RID: 4317
	public sealed class MissionaryFanamaticAI : AIController
	{
		// Token: 0x060053DF RID: 21471 RVA: 0x000FB63A File Offset: 0x000F983A
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x000FB662 File Offset: 0x000F9862
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			while (!base.dead)
			{
				if (base.target == null)
				{
					yield return null;
				}
				else if (base.stuned)
				{
					yield return null;
				}
				else
				{
					yield return this._fanaticAssemble.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x04004360 RID: 17248
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004361 RID: 17249
		[SerializeField]
		[Subcomponent(typeof(FanaticAssemble))]
		private FanaticAssemble _fanaticAssemble;
	}
}
