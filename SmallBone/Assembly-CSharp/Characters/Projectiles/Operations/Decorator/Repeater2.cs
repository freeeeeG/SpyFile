using System;
using System.Collections;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator
{
	// Token: 0x020007B2 RID: 1970
	public class Repeater2 : Operation
	{
		// Token: 0x06002823 RID: 10275 RVA: 0x00079610 File Offset: 0x00077810
		private void Awake()
		{
			Array.Sort<float>(this._timesToTrigger.values);
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x00079622 File Offset: 0x00077822
		internal IEnumerator CRun(IProjectile projectile)
		{
			int operationIndex = 0;
			float time = 0f;
			float[] timesToTrigger = this._timesToTrigger.values;
			while (operationIndex < timesToTrigger.Length)
			{
				while (operationIndex < timesToTrigger.Length && time >= timesToTrigger[operationIndex])
				{
					this._toRepeat.Run(projectile);
					int num = operationIndex;
					operationIndex = num + 1;
				}
				yield return null;
				time += projectile.owner.chronometer.projectile.deltaTime;
			}
			yield break;
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x00079638 File Offset: 0x00077838
		public override void Run(IProjectile projectile)
		{
			base.StartCoroutine(this.CRun(projectile));
		}

		// Token: 0x04002264 RID: 8804
		[SerializeField]
		private ReorderableFloatArray _timesToTrigger = new ReorderableFloatArray(new float[1]);

		// Token: 0x04002265 RID: 8805
		[SerializeField]
		[Operation.SubcomponentAttribute]
		private Operation.Subcomponents _toRepeat;
	}
}
