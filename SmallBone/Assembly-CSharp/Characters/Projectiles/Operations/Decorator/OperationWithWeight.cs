using System;
using System.Linq;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator
{
	// Token: 0x020007AB RID: 1963
	public class OperationWithWeight : MonoBehaviour
	{
		// Token: 0x0600280F RID: 10255 RVA: 0x000793CC File Offset: 0x000775CC
		public override string ToString()
		{
			string arg = (this._operation == null) ? "Do Nothing" : this._operation.GetType().Name;
			return string.Format("{0}({1})", arg, this._weight);
		}

		// Token: 0x04002254 RID: 8788
		[SerializeField]
		private float _weight = 1f;

		// Token: 0x04002255 RID: 8789
		[SerializeField]
		[Operation.SubcomponentAttribute]
		private Operation _operation;

		// Token: 0x020007AC RID: 1964
		[Serializable]
		public class Subcomponents : SubcomponentArray<OperationWithWeight>
		{
			// Token: 0x06002811 RID: 10257 RVA: 0x00079428 File Offset: 0x00077628
			public void RunWeightedRandom(IProjectile projectile)
			{
				Operation operation = null;
				float num = UnityEngine.Random.Range(0f, this._components.Sum((OperationWithWeight c) => c._weight));
				for (int i = 0; i < this._components.Length; i++)
				{
					num -= this._components[i]._weight;
					if (num <= 0f)
					{
						operation = this._components[i]._operation;
						break;
					}
				}
				if (operation == null)
				{
					return;
				}
				operation.Run(projectile);
			}
		}
	}
}
