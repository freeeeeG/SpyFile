using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator
{
	// Token: 0x020007B4 RID: 1972
	public class Repeater3 : Operation
	{
		// Token: 0x0600282D RID: 10285 RVA: 0x00079763 File Offset: 0x00077963
		private void Awake()
		{
			Array.Sort<float>(this._timesToTrigger.values);
			this._repeatCoroutineReferences = new CoroutineReference[this._timesToTrigger.values.Length];
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x0007978D File Offset: 0x0007798D
		internal IEnumerator CRun(IProjectile projectile)
		{
			int operationIndex = 0;
			float time = 0f;
			float[] timesToTrigger = this._timesToTrigger.values;
			while (operationIndex < timesToTrigger.Length)
			{
				while (operationIndex < timesToTrigger.Length && time >= timesToTrigger[operationIndex])
				{
					base.StartCoroutine(this._operations.CRun(projectile));
					int num = operationIndex;
					operationIndex = num + 1;
				}
				yield return null;
				time += projectile.owner.chronometer.projectile.deltaTime;
			}
			yield break;
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x0007715A File Offset: 0x0007535A
		public override void Run(IProjectile projectile)
		{
			this.Run(projectile);
		}

		// Token: 0x0400226D RID: 8813
		[SerializeField]
		private ReorderableFloatArray _timesToTrigger = new ReorderableFloatArray(new float[1]);

		// Token: 0x0400226E RID: 8814
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x0400226F RID: 8815
		private CoroutineReference[] _repeatCoroutineReferences;
	}
}
