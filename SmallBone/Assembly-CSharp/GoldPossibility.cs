using System;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x02000051 RID: 81
[Serializable]
public class GoldPossibility
{
	// Token: 0x0600017C RID: 380 RVA: 0x00007680 File Offset: 0x00005880
	public bool Drop(Vector3 position)
	{
		if (!MMMaths.Chance(this._possibility / 100f))
		{
			return false;
		}
		int num = UnityEngine.Random.Range(this._min, this._max);
		Singleton<Service>.Instance.levelManager.DropGold(num, Mathf.Max(num / this._goldAmountPerCoin, 1), position);
		return true;
	}

	// Token: 0x04000141 RID: 321
	[SerializeField]
	[Range(0f, 100f)]
	private float _possibility;

	// Token: 0x04000142 RID: 322
	[SerializeField]
	private int _min;

	// Token: 0x04000143 RID: 323
	[SerializeField]
	private int _max;

	// Token: 0x04000144 RID: 324
	[SerializeField]
	private int _goldAmountPerCoin;
}
