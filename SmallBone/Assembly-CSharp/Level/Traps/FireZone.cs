using System;
using System.Collections;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200064B RID: 1611
	public class FireZone : ControlableTrap
	{
		// Token: 0x06002062 RID: 8290 RVA: 0x0006219B File Offset: 0x0006039B
		private void Start()
		{
			this._operations.Initialize();
			if (this._activeManually)
			{
				return;
			}
			this.Activate();
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x000621B7 File Offset: 0x000603B7
		public override void Activate()
		{
			if (this._repeat)
			{
				this._coroutineReference = this.StartCoroutineWithReference(this.CRun());
				return;
			}
			base.StartCoroutine(this._operations.CRun(this._character));
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000621EC File Offset: 0x000603EC
		private IEnumerator CRun()
		{
			for (;;)
			{
				yield return this._operations.CRun(this._character);
				yield return this._character.chronometer.master.WaitForSeconds(this._interval);
			}
			yield break;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x000621FB File Offset: 0x000603FB
		public override void Deactivate()
		{
			this._coroutineReference.Stop();
		}

		// Token: 0x04001B74 RID: 7028
		[SerializeField]
		private Character _character;

		// Token: 0x04001B75 RID: 7029
		[SerializeField]
		private float _interval;

		// Token: 0x04001B76 RID: 7030
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04001B77 RID: 7031
		[SerializeField]
		private bool _repeat;

		// Token: 0x04001B78 RID: 7032
		[SerializeField]
		private bool _activeManually;

		// Token: 0x04001B79 RID: 7033
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04001B7A RID: 7034
		private CoroutineReference _coroutineReference;
	}
}
