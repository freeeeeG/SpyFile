using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000ECA RID: 3786
	public class Repeater2 : CharacterOperation
	{
		// Token: 0x06004A57 RID: 19031 RVA: 0x000D9043 File Offset: 0x000D7243
		private void Awake()
		{
			Array.Sort<float>(this._timesToTrigger.values);
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x000D9055 File Offset: 0x000D7255
		public override void Initialize()
		{
			this._toRepeat.Initialize();
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x000D9062 File Offset: 0x000D7262
		internal IEnumerator CRun(Character owner, Character target)
		{
			int operationIndex = 0;
			float time = 0f;
			float[] timesToTrigger = this._timesToTrigger.values;
			while (operationIndex < timesToTrigger.Length)
			{
				while (operationIndex < timesToTrigger.Length && time >= timesToTrigger[operationIndex])
				{
					this._toRepeat.Run(owner, target);
					int num = operationIndex;
					operationIndex = num + 1;
				}
				yield return null;
				time += owner.chronometer.animation.deltaTime * this.runSpeed;
			}
			yield break;
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x000D7314 File Offset: 0x000D5514
		public override void Run(Character owner)
		{
			this.Run(owner, owner);
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x000D907F File Offset: 0x000D727F
		public override void Run(Character owner, Character target)
		{
			this._repeatCoroutineReference = this.StartCoroutineWithReference(this.CRun(owner, target));
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x000D9095 File Offset: 0x000D7295
		public override void Stop()
		{
			this._toRepeat.Stop();
			this._repeatCoroutineReference.Stop();
		}

		// Token: 0x04003984 RID: 14724
		[SerializeField]
		private ReorderableFloatArray _timesToTrigger = new ReorderableFloatArray(new float[1]);

		// Token: 0x04003985 RID: 14725
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _toRepeat;

		// Token: 0x04003986 RID: 14726
		private CoroutineReference _repeatCoroutineReference;
	}
}
