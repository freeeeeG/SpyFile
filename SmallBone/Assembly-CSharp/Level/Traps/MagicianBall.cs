using System;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000669 RID: 1641
	public class MagicianBall : Trap
	{
		// Token: 0x060020E8 RID: 8424 RVA: 0x0006356B File Offset: 0x0006176B
		private void Awake()
		{
			this._character = base.GetComponentInParent<Character>();
			this._runOperations.Initialize();
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00063584 File Offset: 0x00061784
		private void OnEnable()
		{
			this._runOperations.Initialize();
			this._runOperations.Run(this._character);
		}

		// Token: 0x04001BFE RID: 7166
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _runOperations;

		// Token: 0x04001BFF RID: 7167
		private Character _character;
	}
}
