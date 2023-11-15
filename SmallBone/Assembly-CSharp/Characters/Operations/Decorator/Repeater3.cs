using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000ECC RID: 3788
	public class Repeater3 : CharacterOperation
	{
		// Token: 0x06004A64 RID: 19044 RVA: 0x000D91D3 File Offset: 0x000D73D3
		private void Awake()
		{
			Array.Sort<float>(this._timesToTrigger.values);
			this._repeatCoroutineReferences = new CoroutineReference[this._timesToTrigger.values.Length];
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x000D91FD File Offset: 0x000D73FD
		public override void Initialize()
		{
			this._operations.Initialize();
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x000D920A File Offset: 0x000D740A
		internal IEnumerator CRun(Character owner, Character target)
		{
			int operationIndex = 0;
			float time = 0f;
			float[] timesToTrigger = this._timesToTrigger.values;
			while (operationIndex < timesToTrigger.Length)
			{
				while (operationIndex < timesToTrigger.Length && time >= timesToTrigger[operationIndex])
				{
					base.StartCoroutine(this._operations.CRun(owner, target));
					int num = operationIndex;
					operationIndex = num + 1;
				}
				yield return null;
				time += owner.chronometer.animation.deltaTime * this.runSpeed;
			}
			yield break;
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x000D7314 File Offset: 0x000D5514
		public override void Run(Character owner)
		{
			this.Run(owner, owner);
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x000D9227 File Offset: 0x000D7427
		public override void Run(Character owner, Character target)
		{
			base.StartCoroutine(this.CRun(owner, target));
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x000D9238 File Offset: 0x000D7438
		public override void Stop()
		{
			this._operations.StopAll();
			base.StopAllCoroutines();
		}

		// Token: 0x0400398F RID: 14735
		[SerializeField]
		private ReorderableFloatArray _timesToTrigger = new ReorderableFloatArray(new float[1]);

		// Token: 0x04003990 RID: 14736
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04003991 RID: 14737
		private CoroutineReference[] _repeatCoroutineReferences;
	}
}
