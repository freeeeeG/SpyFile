using System;
using System.Collections;
using Characters.Gear.Synergy.Inscriptions.FairyTaleSummon;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DF4 RID: 3572
	public class OberonBomb : CharacterOperation
	{
		// Token: 0x0600477D RID: 18301 RVA: 0x000CFA60 File Offset: 0x000CDC60
		private void Awake()
		{
			this._orbs = new OberonBombOrb[this._container.childCount];
			for (int i = 0; i < this._container.childCount; i++)
			{
				this._orbs[i] = this._container.GetChild(i).GetComponent<OberonBombOrb>();
			}
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x000CFAB2 File Offset: 0x000CDCB2
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x000CFAC2 File Offset: 0x000CDCC2
		private IEnumerator CRun(Character owner)
		{
			this._orbs.Shuffle<OberonBombOrb>();
			yield return this.CActivate(owner);
			yield return owner.chronometer.master.WaitForSeconds(this._bombDelay);
			this.Deactivate(owner);
			yield break;
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x000CFAD8 File Offset: 0x000CDCD8
		private IEnumerator CActivate(Character owner)
		{
			OberonBombOrb[] array = this._orbs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Activate(owner);
				yield return owner.chronometer.master.WaitForSeconds(this._activateInterval);
			}
			array = null;
			yield break;
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x000CFAF0 File Offset: 0x000CDCF0
		private void Deactivate(Character owner)
		{
			OberonBombOrb[] orbs = this._orbs;
			for (int i = 0; i < orbs.Length; i++)
			{
				orbs[i].Deactivate(owner);
			}
		}

		// Token: 0x04003680 RID: 13952
		[SerializeField]
		private Transform _container;

		// Token: 0x04003681 RID: 13953
		[SerializeField]
		private float _activateInterval;

		// Token: 0x04003682 RID: 13954
		[SerializeField]
		private float _bombDelay = 1f;

		// Token: 0x04003683 RID: 13955
		private OberonBombOrb[] _orbs;
	}
}
