using System;
using System.Collections;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000650 RID: 1616
	public class ArcherTrap : MonoBehaviour
	{
		// Token: 0x0600207A RID: 8314 RVA: 0x0006254A File Offset: 0x0006074A
		private void Awake()
		{
			this._readyOperations.Initialize();
			this._activeOperations.Initialize();
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00062562 File Offset: 0x00060762
		private void OnEnable()
		{
			this.Ready();
			base.StartCoroutine(this.CActivate());
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00062577 File Offset: 0x00060777
		private void Ready()
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(this._character);
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x0006259B File Offset: 0x0006079B
		private void Hide()
		{
			this._character.gameObject.SetActive(false);
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000625AE File Offset: 0x000607AE
		private IEnumerator CActivate()
		{
			yield return Chronometer.global.WaitForSeconds(this._activeDelay);
			this._activeOperations.gameObject.SetActive(true);
			this._activeOperations.Run(this._character);
			base.StartCoroutine(this.CSleep());
			yield break;
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000625BD File Offset: 0x000607BD
		private IEnumerator CSleep()
		{
			yield return Chronometer.global.WaitForSeconds(this._lifeTime);
			this.Hide();
			yield break;
		}

		// Token: 0x04001B8C RID: 7052
		[SerializeField]
		private Character _character;

		// Token: 0x04001B8D RID: 7053
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _readyOperations;

		// Token: 0x04001B8E RID: 7054
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _activeOperations;

		// Token: 0x04001B8F RID: 7055
		[SerializeField]
		private float _activeDelay;

		// Token: 0x04001B90 RID: 7056
		[SerializeField]
		private float _lifeTime;
	}
}
