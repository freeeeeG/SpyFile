using System;
using System.Collections;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level
{
	// Token: 0x02000541 RID: 1345
	public class WaveAction : MonoBehaviour
	{
		// Token: 0x06001A71 RID: 6769 RVA: 0x00052E2B File Offset: 0x0005102B
		private void Awake()
		{
			this._operations.Initialize();
			this._target.onClear += this.Run;
			if (this._alsoClear)
			{
				base.StartCoroutine(this.CCheckAlsoClear());
			}
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00052E64 File Offset: 0x00051064
		private void Run()
		{
			if (this._run)
			{
				return;
			}
			this._operations.Run(this._character);
			this._run = true;
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00052E87 File Offset: 0x00051087
		private IEnumerator CCheckAlsoClear()
		{
			while (this._target != null && this._target.state != Wave.State.Stopped)
			{
				yield return null;
			}
			if (this._target != null)
			{
				this.Run();
			}
			yield break;
		}

		// Token: 0x04001706 RID: 5894
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04001707 RID: 5895
		[SerializeField]
		private Wave _target;

		// Token: 0x04001708 RID: 5896
		[SerializeField]
		private bool _alsoClear;

		// Token: 0x04001709 RID: 5897
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _operations;

		// Token: 0x0400170A RID: 5898
		private bool _run;
	}
}
