using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200028B RID: 651
	public class AssetBundleLoadAssetOperationSimulation : AssetBundleLoadAssetOperation
	{
		// Token: 0x06000BF5 RID: 3061 RVA: 0x0003E069 File Offset: 0x0003C469
		public AssetBundleLoadAssetOperationSimulation(UnityEngine.Object simulatedObject)
		{
			this.m_SimulatedObject = simulatedObject;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0003E078 File Offset: 0x0003C478
		public override T GetAsset<T>()
		{
			return this.m_SimulatedObject as T;
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0003E08A File Offset: 0x0003C48A
		public override bool Update()
		{
			return false;
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0003E08D File Offset: 0x0003C48D
		public override bool IsDone()
		{
			return true;
		}

		// Token: 0x04000913 RID: 2323
		private UnityEngine.Object m_SimulatedObject;
	}
}
