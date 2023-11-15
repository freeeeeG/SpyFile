using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011F5 RID: 4597
	public abstract class LadderSelectPolicy : MonoBehaviour
	{
		// Token: 0x06005A3F RID: 23103 RVA: 0x0010C0BC File Offset: 0x0010A2BC
		private void Awake()
		{
			this._fanaticLadders = new FanaticLadder[this._spawnPointContainer.childCount];
			int num = 0;
			foreach (object obj in this._spawnPointContainer)
			{
				Transform transform = (Transform)obj;
				this._fanaticLadders[num++] = transform.GetComponent<FanaticLadder>();
			}
		}

		// Token: 0x06005A40 RID: 23104 RVA: 0x0010C138 File Offset: 0x0010A338
		public IEnumerator<FanaticLadder> GetLadders()
		{
			return this.SelectLadders().GetEnumerator() as IEnumerator<FanaticLadder>;
		}

		// Token: 0x06005A41 RID: 23105
		public abstract FanaticLadder[] SelectLadders();

		// Token: 0x040048E5 RID: 18661
		[SerializeField]
		private Transform _spawnPointContainer;

		// Token: 0x040048E6 RID: 18662
		private FanaticLadder[] _fanaticLadders;
	}
}
