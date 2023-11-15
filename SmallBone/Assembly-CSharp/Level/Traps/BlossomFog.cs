using System;
using Characters;
using Characters.Operations;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000653 RID: 1619
	public class BlossomFog : Trap
	{
		// Token: 0x0600208D RID: 8333 RVA: 0x000626D4 File Offset: 0x000608D4
		private void Awake()
		{
			this._operationInfos.Initialize();
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000626E1 File Offset: 0x000608E1
		private void OnEnable()
		{
			this._operationInfos.gameObject.SetActive(true);
			this._operationInfos.Run(this._character);
		}

		// Token: 0x04001B97 RID: 7063
		[SerializeField]
		private Character _character;

		// Token: 0x04001B98 RID: 7064
		[SerializeField]
		private OperationInfos _operationInfos;
	}
}
