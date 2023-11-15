using System;
using System.Linq;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EC1 RID: 3777
	public class OperationWithWeight : MonoBehaviour
	{
		// Token: 0x06004A34 RID: 18996 RVA: 0x000D8B5C File Offset: 0x000D6D5C
		public override string ToString()
		{
			string arg = (this._operation == null) ? "Do Nothing" : this._operation.GetType().Name;
			return string.Format("{0}({1})", arg, this._weight);
		}

		// Token: 0x0400396B RID: 14699
		[SerializeField]
		private float _weight = 1f;

		// Token: 0x0400396C RID: 14700
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation _operation;

		// Token: 0x02000EC2 RID: 3778
		[Serializable]
		public class Subcomponents : SubcomponentArray<OperationWithWeight>
		{
			// Token: 0x06004A36 RID: 18998 RVA: 0x000D8BB8 File Offset: 0x000D6DB8
			internal void Initialize()
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					OperationWithWeight operationWithWeight = this._components[i];
					if (!(operationWithWeight._operation == null))
					{
						operationWithWeight._operation.Initialize();
					}
				}
			}

			// Token: 0x06004A37 RID: 18999 RVA: 0x000D8BFC File Offset: 0x000D6DFC
			public void RunWeightedRandom(Character owner)
			{
				CharacterOperation characterOperation = null;
				float num = UnityEngine.Random.Range(0f, this._components.Sum((OperationWithWeight c) => c._weight));
				for (int i = 0; i < this._components.Length; i++)
				{
					num -= this._components[i]._weight;
					if (num <= 0f)
					{
						characterOperation = this._components[i]._operation;
						break;
					}
				}
				if (characterOperation == null)
				{
					return;
				}
				characterOperation.Run(owner);
			}

			// Token: 0x06004A38 RID: 19000 RVA: 0x000D8C8C File Offset: 0x000D6E8C
			internal void StopAll()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					OperationWithWeight operationWithWeight = this._components[i];
					if (!(operationWithWeight._operation == null))
					{
						operationWithWeight._operation.Stop();
					}
				}
			}
		}
	}
}
