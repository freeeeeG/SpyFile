using System;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000681 RID: 1665
	public class ThiefTrap : MonoBehaviour
	{
		// Token: 0x0600214F RID: 8527 RVA: 0x000643B1 File Offset: 0x000625B1
		private void Awake()
		{
			this._operationsOnDie.Initialize();
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x000643BE File Offset: 0x000625BE
		private void OnEnable()
		{
			base.StartCoroutine(this._operationsOnDie.CRun(this._character));
		}

		// Token: 0x04001C60 RID: 7264
		[SerializeField]
		private Character _character;

		// Token: 0x04001C61 RID: 7265
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operationsOnDie;
	}
}
