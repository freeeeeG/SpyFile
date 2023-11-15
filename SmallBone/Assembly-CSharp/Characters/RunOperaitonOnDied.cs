using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200071A RID: 1818
	public sealed class RunOperaitonOnDied : MonoBehaviour
	{
		// Token: 0x060024D3 RID: 9427 RVA: 0x0006EA72 File Offset: 0x0006CC72
		private void Awake()
		{
			this._character.health.onDied += this.OnDied;
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x0006EA90 File Offset: 0x0006CC90
		private void OnDied()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.health.onDied -= this.OnDied;
			if (this._infos == null)
			{
				return;
			}
			this._infos.gameObject.SetActive(true);
			this._infos.Run(this._character);
		}

		// Token: 0x04001F46 RID: 8006
		[SerializeField]
		private Character _character;

		// Token: 0x04001F47 RID: 8007
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _infos;
	}
}
