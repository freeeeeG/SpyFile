using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DFD RID: 3581
	public class RunOperationOnEnable : MonoBehaviour
	{
		// Token: 0x060047A2 RID: 18338 RVA: 0x000D040F File Offset: 0x000CE60F
		private void Start()
		{
			this._operationInfos.Initialize();
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x000D041C File Offset: 0x000CE61C
		private void OnEnable()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			this._operationInfos.Run(player);
		}

		// Token: 0x040036A6 RID: 13990
		[SerializeField]
		private OperationInfos _operationInfos;
	}
}
