using System;
using UnityEngine;

namespace Level.ReactiveProps
{
	// Token: 0x0200056F RID: 1391
	public class RandomlyLoop : MonoBehaviour
	{
		// Token: 0x06001B4F RID: 6991 RVA: 0x00054D51 File Offset: 0x00052F51
		private void Start()
		{
			this._term = UnityEngine.Random.Range(this._termRange.x, this._termRange.y);
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x00054D74 File Offset: 0x00052F74
		private void Update()
		{
			this._elapsed += Chronometer.global.deltaTime;
			if (this._elapsed > this._term)
			{
				this._groups.Random<FlyGroup>().Activate();
				this._elapsed -= this._term;
				this._term = UnityEngine.Random.Range(this._termRange.x, this._termRange.y);
			}
		}

		// Token: 0x0400177B RID: 6011
		[SerializeField]
		private FlyGroup[] _groups;

		// Token: 0x0400177C RID: 6012
		[SerializeField]
		[MinMaxSlider(1f, 100f)]
		private Vector2 _termRange;

		// Token: 0x0400177D RID: 6013
		private float _term;

		// Token: 0x0400177E RID: 6014
		private float _elapsed;
	}
}
