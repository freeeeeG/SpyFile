using System;
using System.Collections;
using Characters.AI.Hero;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DC0 RID: 3520
	public class PillarOfLightAttack : CharacterOperation
	{
		// Token: 0x060046C1 RID: 18113 RVA: 0x000CD670 File Offset: 0x000CB870
		public override void Run(Character owner)
		{
			PillarOfLightContainer component = this._container.GetChild(UnityEngine.Random.Range(0, this._container.childCount)).GetComponent<PillarOfLightContainer>();
			base.StartCoroutine(this.CRun(owner, component));
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x000CD6AE File Offset: 0x000CB8AE
		private IEnumerator CRun(Character owner, PillarOfLightContainer container)
		{
			container.Sign(owner);
			yield return owner.chronometer.master.WaitForSeconds(this._attackDelay);
			container.Attack(owner);
			yield break;
		}

		// Token: 0x0400359B RID: 13723
		[SerializeField]
		private Transform _container;

		// Token: 0x0400359C RID: 13724
		[SerializeField]
		private float _attackDelay;
	}
}
