using System;
using System.Collections;
using Characters.Abilities.Customs;
using UnityEngine;

namespace Characters.Operations.Customs.GraveDigger
{
	// Token: 0x0200100D RID: 4109
	public class SpawnCorpseForLandOfTheDead : CharacterOperation
	{
		// Token: 0x06004F57 RID: 20311 RVA: 0x000EEB74 File Offset: 0x000ECD74
		public void Set(GraveDiggerPassiveComponent passive, Vector2 left, Vector2 right)
		{
			this._passive = passive;
			this._left = left;
			this._right = right;
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x000EEB8B File Offset: 0x000ECD8B
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CSummon());
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x000EEB9A File Offset: 0x000ECD9A
		private IEnumerator CSummon()
		{
			for (;;)
			{
				Vector2 v = new Vector2(UnityEngine.Random.Range(this._left.x, this._right.x), this._left.y);
				this._passive.SpawnCorpse(v);
				yield return Chronometer.global.WaitForSeconds(this._summonInterval.value);
			}
			yield break;
		}

		// Token: 0x04003F8B RID: 16267
		[SerializeField]
		private CustomFloat _summonInterval;

		// Token: 0x04003F8C RID: 16268
		private GraveDiggerPassiveComponent _passive;

		// Token: 0x04003F8D RID: 16269
		private Vector2 _left;

		// Token: 0x04003F8E RID: 16270
		private Vector2 _right;
	}
}
