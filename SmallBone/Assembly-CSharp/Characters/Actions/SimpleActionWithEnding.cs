using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x0200095E RID: 2398
	public class SimpleActionWithEnding : SimpleAction
	{
		// Token: 0x060033A5 RID: 13221 RVA: 0x0009926C File Offset: 0x0009746C
		protected override void Awake()
		{
			base.Awake();
			if (this._endingOpreationsOnEnd)
			{
				this._onEnd = (Action)Delegate.Combine(this._onEnd, new Action(this.Run));
			}
			if (this._endingOpreationsOnCancel)
			{
				this._onCancel = (Action)Delegate.Combine(this._onCancel, new Action(this.Run));
			}
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x000992D3 File Offset: 0x000974D3
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			this._endingOperations.Initialize();
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x000992E7 File Offset: 0x000974E7
		private void Run()
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x000992F6 File Offset: 0x000974F6
		private IEnumerator CRun()
		{
			int operationIndex = 0;
			float time = 0f;
			OperationInfo[] components = this._endingOperations.components;
			while ((this._endingOperationDuration == 0f && operationIndex < components.Length) || (this._endingOperationDuration > 0f && time < this._endingOperationDuration))
			{
				time += Chronometer.global.deltaTime;
				while (operationIndex < components.Length && time >= components[operationIndex].timeToTrigger)
				{
					components[operationIndex].operation.Run(base.owner);
					int num = operationIndex;
					operationIndex = num + 1;
				}
				yield return null;
				if (base.owner == null || !base.owner.gameObject.activeSelf)
				{
					break;
				}
			}
			this._endingOperations.StopAll();
			yield break;
		}

		// Token: 0x040029E3 RID: 10723
		[Header("Ending")]
		[SerializeField]
		private bool _endingOpreationsOnEnd = true;

		// Token: 0x040029E4 RID: 10724
		[SerializeField]
		private bool _endingOpreationsOnCancel = true;

		// Token: 0x040029E5 RID: 10725
		[SerializeField]
		private float _endingOperationDuration;

		// Token: 0x040029E6 RID: 10726
		[SerializeField]
		[Space]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _endingOperations;
	}
}
