using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200078D RID: 1933
	internal class OperationInfo : MonoBehaviour
	{
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600279F RID: 10143 RVA: 0x000771AE File Offset: 0x000753AE
		public Operation operation
		{
			get
			{
				return this._operation;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000771B6 File Offset: 0x000753B6
		public float timeToTrigger
		{
			get
			{
				return this._timeToTrigger;
			}
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000771C0 File Offset: 0x000753C0
		public override string ToString()
		{
			string arg = (this._operation == null) ? "null" : this._operation.GetType().Name;
			return string.Format("{0:0.##}s({1:0.##}f), {2}", this._timeToTrigger, this._timeToTrigger * 60f, arg);
		}

		// Token: 0x040021BC RID: 8636
		[SerializeField]
		[FrameTime]
		private float _timeToTrigger;

		// Token: 0x040021BD RID: 8637
		[Operation.SubcomponentAttribute]
		[SerializeField]
		private Operation _operation;

		// Token: 0x0200078E RID: 1934
		[Serializable]
		internal class Subcomponents : SubcomponentArray<OperationInfo>
		{
			// Token: 0x060027A3 RID: 10147 RVA: 0x0007721A File Offset: 0x0007541A
			internal void Sort()
			{
				this._components = (from operation in this._components
				orderby operation.timeToTrigger
				select operation).ToArray<OperationInfo>();
			}

			// Token: 0x060027A4 RID: 10148 RVA: 0x00077251 File Offset: 0x00075451
			internal IEnumerator CRun(IProjectile projectile)
			{
				int operationIndex = 0;
				float time = 0f;
				while (operationIndex < this._components.Length)
				{
					while (operationIndex < this._components.Length && time >= this._components[operationIndex].timeToTrigger)
					{
						this._components[operationIndex].operation.Run(projectile);
						int num = operationIndex;
						operationIndex = num + 1;
					}
					yield return null;
					time += projectile.owner.chronometer.projectile.deltaTime;
				}
				yield break;
			}
		}
	}
}
