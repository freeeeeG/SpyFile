using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DF2 RID: 3570
	public class MakeNoiseToContainer : CharacterOperation
	{
		// Token: 0x06004772 RID: 18290 RVA: 0x000CF8E5 File Offset: 0x000CDAE5
		private void Awake()
		{
			this._origin = new Vector2[this._container.childCount];
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x000CF900 File Offset: 0x000CDB00
		public override void Run(Character owner)
		{
			for (int i = 0; i < this._container.childCount; i++)
			{
				Transform child = this._container.GetChild(i);
				this._origin[i] = child.transform.position;
				child.Translate(UnityEngine.Random.insideUnitSphere * this._noise);
			}
			if (this._restoreTime > 0f)
			{
				base.StartCoroutine(this.CRestore(owner.chronometer.master));
			}
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x000CF987 File Offset: 0x000CDB87
		private IEnumerator CRestore(Chronometer chronometer)
		{
			yield return chronometer.WaitForSeconds(this._restoreTime);
			this.Restore();
			yield break;
		}

		// Token: 0x06004775 RID: 18293 RVA: 0x000CF9A0 File Offset: 0x000CDBA0
		private void Restore()
		{
			for (int i = 0; i < this._container.childCount; i++)
			{
				this._container.GetChild(i).transform.position = this._origin[i];
			}
		}

		// Token: 0x04003678 RID: 13944
		[SerializeField]
		private Transform _container;

		// Token: 0x04003679 RID: 13945
		[SerializeField]
		private float _noise;

		// Token: 0x0400367A RID: 13946
		[SerializeField]
		private float _restoreTime;

		// Token: 0x0400367B RID: 13947
		private Vector2[] _origin;
	}
}
