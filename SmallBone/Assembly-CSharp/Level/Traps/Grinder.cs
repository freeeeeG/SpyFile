using System;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000660 RID: 1632
	public class Grinder : Trap
	{
		// Token: 0x060020C2 RID: 8386 RVA: 0x00062F9E File Offset: 0x0006119E
		private void Awake()
		{
			this._operationInfos.Initialize();
			this._operationInfos.Run(this._character);
		}

		// Token: 0x04001BD1 RID: 7121
		[SerializeField]
		private Character _character;

		// Token: 0x04001BD2 RID: 7122
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _operationInfos;
	}
}
