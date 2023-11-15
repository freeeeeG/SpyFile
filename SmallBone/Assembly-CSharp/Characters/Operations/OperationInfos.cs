using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E2B RID: 3627
	public class OperationInfos : MonoBehaviour
	{
		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06004859 RID: 18521 RVA: 0x000D2578 File Offset: 0x000D0778
		// (remove) Token: 0x0600485A RID: 18522 RVA: 0x000D25B0 File Offset: 0x000D07B0
		public event Action onEnd;

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x0600485B RID: 18523 RVA: 0x000D25E5 File Offset: 0x000D07E5
		public float duration
		{
			get
			{
				return this._duration;
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x0600485C RID: 18524 RVA: 0x000D25ED File Offset: 0x000D07ED
		public Character owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x0600485D RID: 18525 RVA: 0x000D25F5 File Offset: 0x000D07F5
		public bool running
		{
			get
			{
				return this._running;
			}
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x000D25FD File Offset: 0x000D07FD
		private void OnDisable()
		{
			if (this._running)
			{
				this._operations.StopAll();
				this._running = false;
				Action action = this.onEnd;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x000D2629 File Offset: 0x000D0829
		public void Initialize()
		{
			this._operations.Initialize();
			this._operationsOnEnd.Initialize();
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x000D2641 File Offset: 0x000D0841
		public void Run(Character owner)
		{
			this.Run(owner, 1f);
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x000D264F File Offset: 0x000D084F
		public void Run(Character owner, float speed = 1f)
		{
			this._owner = owner;
			base.StartCoroutine(this.CRun(speed));
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x000D2666 File Offset: 0x000D0866
		private IEnumerator CRun(float speed)
		{
			this._running = true;
			int operationIndex = 0;
			float time = 0f;
			OperationInfo[] components = this._operations.components;
			while ((this._duration == 0f && operationIndex < components.Length) || (this._duration > 0f && time < this._duration))
			{
				time += Chronometer.global.deltaTime * speed;
				while (operationIndex < components.Length && time >= components[operationIndex].timeToTrigger)
				{
					if (components[operationIndex].operation.gameObject.activeSelf && this._owner.gameObject.activeSelf)
					{
						components[operationIndex].operation.Run(this._owner);
					}
					int num = operationIndex;
					operationIndex = num + 1;
				}
				yield return null;
				if (this._owner == null || !this._owner.gameObject.activeSelf)
				{
					break;
				}
			}
			this.Stop();
			yield break;
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x000D267C File Offset: 0x000D087C
		public void Stop()
		{
			this._operations.StopAll();
			if (this._owner != null)
			{
				this._operationsOnEnd.Run(this._owner);
			}
			this._running = false;
			Action action = this.onEnd;
			if (action != null)
			{
				action();
			}
			this._owner = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x04003767 RID: 14183
		[SerializeField]
		private float _duration;

		// Token: 0x04003768 RID: 14184
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04003769 RID: 14185
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operationsOnEnd;

		// Token: 0x0400376A RID: 14186
		private bool _running;

		// Token: 0x0400376B RID: 14187
		private Character _owner;
	}
}
