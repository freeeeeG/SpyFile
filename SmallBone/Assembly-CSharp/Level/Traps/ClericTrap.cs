using System;
using System.Collections;
using Characters;
using Characters.Abilities;
using Characters.Actions;
using Characters.Operations.Attack;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000654 RID: 1620
	public class ClericTrap : MonoBehaviour
	{
		// Token: 0x06002090 RID: 8336 RVA: 0x00062705 File Offset: 0x00060905
		private void Awake()
		{
			this._sweepAttack.Initialize();
			this._abilityAttacher.Initialize(this._character);
			this._abilityAttacher.StartAttach();
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x0006272E File Offset: 0x0006092E
		private void OnEnable()
		{
			this._sweepAttack.Run(this._character);
			base.StartCoroutine(this.CAttack());
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x0006274E File Offset: 0x0006094E
		private IEnumerator CAttack()
		{
			yield return Chronometer.global.WaitForSeconds(this._explosionTime);
			this._sweepAttack.Stop();
			this._attackAction.TryStart();
			while (this._attackAction.running)
			{
				yield return null;
			}
			this._character.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x0006275D File Offset: 0x0006095D
		private void OnDisable()
		{
			this._sweepAttack.Stop();
			this._character.CancelAction();
			this._abilityAttacher.StopAttach();
		}

		// Token: 0x04001B99 RID: 7065
		[SerializeField]
		private Character _character;

		// Token: 0x04001B9A RID: 7066
		[SerializeField]
		private SweepAttack2 _sweepAttack;

		// Token: 0x04001B9B RID: 7067
		[SerializeField]
		private float _explosionTime;

		// Token: 0x04001B9C RID: 7068
		[SerializeField]
		private Characters.Actions.Action _attackAction;

		// Token: 0x04001B9D RID: 7069
		[SerializeField]
		[AbilityAttacher.SubcomponentAttribute]
		private AbilityAttacher _abilityAttacher;
	}
}
