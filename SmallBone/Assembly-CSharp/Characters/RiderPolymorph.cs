using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000718 RID: 1816
	public class RiderPolymorph : MonoBehaviour
	{
		// Token: 0x060024C9 RID: 9417 RVA: 0x0006E99B File Offset: 0x0006CB9B
		private void OnEnable()
		{
			if (!this._initialized)
			{
				this._operations.Initialize();
				this._initialized = true;
			}
			base.StartCoroutine(this.CStartOperation());
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x0006E9C4 File Offset: 0x0006CBC4
		private IEnumerator CStartOperation()
		{
			yield return null;
			yield return this._operations.CRun(this._polymorphBody.character);
			yield break;
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x0006E9D3 File Offset: 0x0006CBD3
		private void OnDisable()
		{
			this._operations.StopAll();
		}

		// Token: 0x04001F40 RID: 8000
		[SerializeField]
		private PolymorphBody _polymorphBody;

		// Token: 0x04001F41 RID: 8001
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04001F42 RID: 8002
		private bool _initialized;
	}
}
