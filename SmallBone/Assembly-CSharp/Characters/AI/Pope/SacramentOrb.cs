using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011F9 RID: 4601
	public class SacramentOrb : MonoBehaviour
	{
		// Token: 0x06005A52 RID: 23122 RVA: 0x0010C2B6 File Offset: 0x0010A4B6
		public void Initialize(Character character)
		{
			this._onReady.Initialize();
			this._onAttack.Initialize();
			this._character = character;
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x0010C2D5 File Offset: 0x0010A4D5
		private void OnEnable()
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x06005A54 RID: 23124 RVA: 0x0010C2E4 File Offset: 0x0010A4E4
		public void ShowSign()
		{
			this._onReady.gameObject.SetActive(true);
			this._onReady.Run(this._character);
		}

		// Token: 0x06005A55 RID: 23125 RVA: 0x0010C308 File Offset: 0x0010A508
		public void Run()
		{
			this._onAttack.gameObject.SetActive(true);
			this._onAttack.Run(this._character);
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x0010C32C File Offset: 0x0010A52C
		private IEnumerator CRun()
		{
			this.ShowSign();
			yield return this._character.chronometer.master.WaitForSeconds(this._delay);
			this.Run();
			yield break;
		}

		// Token: 0x040048EF RID: 18671
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onReady;

		// Token: 0x040048F0 RID: 18672
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onAttack;

		// Token: 0x040048F1 RID: 18673
		[SerializeField]
		private float _delay;

		// Token: 0x040048F2 RID: 18674
		private Character _character;
	}
}
