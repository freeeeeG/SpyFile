using System;
using System.Collections;
using Data;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D18 RID: 3352
	public sealed class OnUseReassembleAttacher : AbilityAttacher
	{
		// Token: 0x06004399 RID: 17305 RVA: 0x000C4E77 File Offset: 0x000C3077
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x000C4E84 File Offset: 0x000C3084
		public override void StartAttach()
		{
			this._cUpdateReference = this.StartCoroutineWithReference(this.CCheckLoop());
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x000C4E98 File Offset: 0x000C3098
		private IEnumerator CCheckLoop()
		{
			while (!GameData.Progress.reassembleUsed)
			{
				yield return Chronometer.global.WaitForSeconds(0.2f);
			}
			this.Attach();
			yield break;
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x000C4EA7 File Offset: 0x000C30A7
		private void Attach()
		{
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x000C4EC5 File Offset: 0x000C30C5
		public override void StopAttach()
		{
			this._cUpdateReference.Stop();
			if (base.owner != null)
			{
				base.owner.ability.Remove(this._abilityComponent.ability);
			}
		}

		// Token: 0x040033A3 RID: 13219
		private const float _checkInterval = 0.2f;

		// Token: 0x040033A4 RID: 13220
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x040033A5 RID: 13221
		private CoroutineReference _cUpdateReference;
	}
}
