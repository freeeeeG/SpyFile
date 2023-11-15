using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E27 RID: 3623
	internal class OperationInfo : MonoBehaviour
	{
		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06004843 RID: 18499 RVA: 0x000D22D9 File Offset: 0x000D04D9
		public CharacterOperation operation
		{
			get
			{
				return this._operation;
			}
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06004844 RID: 18500 RVA: 0x000D22E1 File Offset: 0x000D04E1
		public float timeToTrigger
		{
			get
			{
				return this._timeToTrigger;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06004845 RID: 18501 RVA: 0x000D22E9 File Offset: 0x000D04E9
		public bool stay
		{
			get
			{
				return this._stay;
			}
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x000D22F1 File Offset: 0x000D04F1
		private void OnDestroy()
		{
			this._operation = null;
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x000D22FC File Offset: 0x000D04FC
		public override string ToString()
		{
			string arg = (this._operation == null) ? "null" : this._operation.GetType().Name;
			return string.Format("{0:0.##}s({1:0.##}f), {2}", this._timeToTrigger, this._timeToTrigger * 60f, arg);
		}

		// Token: 0x0400375A RID: 14170
		[FrameTime]
		[SerializeField]
		private float _timeToTrigger;

		// Token: 0x0400375B RID: 14171
		[SerializeField]
		private bool _stay;

		// Token: 0x0400375C RID: 14172
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation _operation;

		// Token: 0x02000E28 RID: 3624
		[Serializable]
		internal class Subcomponents : SubcomponentArray<OperationInfo>
		{
			// Token: 0x06004849 RID: 18505 RVA: 0x000D2358 File Offset: 0x000D0558
			internal void Initialize()
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].operation.Initialize();
				}
			}

			// Token: 0x0600484A RID: 18506 RVA: 0x000D238A File Offset: 0x000D058A
			internal void Sort()
			{
				this._components = (from operation in this._components
				orderby operation.timeToTrigger
				select operation).ToArray<OperationInfo>();
			}

			// Token: 0x0600484B RID: 18507 RVA: 0x000D23C1 File Offset: 0x000D05C1
			internal IEnumerator CRun(Character target)
			{
				return this.CRun(target, target);
			}

			// Token: 0x0600484C RID: 18508 RVA: 0x000D23CB File Offset: 0x000D05CB
			internal IEnumerator CRun(Character owner, Character target)
			{
				int operationIndex = 0;
				float time = 0f;
				while (operationIndex < this._components.Length)
				{
					while (operationIndex < this._components.Length && time >= this._components[operationIndex].timeToTrigger)
					{
						this._components[operationIndex].operation.Run(owner, target);
						int num = operationIndex;
						operationIndex = num + 1;
					}
					yield return null;
					time += owner.chronometer.animation.deltaTime;
				}
				yield break;
			}

			// Token: 0x0600484D RID: 18509 RVA: 0x000D23E8 File Offset: 0x000D05E8
			internal void StopAll()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					if (!base.components[i]._stay)
					{
						base.components[i]._operation.Stop();
					}
				}
			}

			// Token: 0x0600484E RID: 18510 RVA: 0x000D242C File Offset: 0x000D062C
			internal void ForceStopAll()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i]._operation.Stop();
				}
			}
		}
	}
}
