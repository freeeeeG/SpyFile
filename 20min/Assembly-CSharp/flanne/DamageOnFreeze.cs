using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000DD RID: 221
	public class DamageOnFreeze : MonoBehaviour
	{
		// Token: 0x060006A2 RID: 1698 RVA: 0x0001DDA0 File Offset: 0x0001BFA0
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnFreeze), FreezeSystem.InflictFreezeEvent);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001DDB9 File Offset: 0x0001BFB9
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnFreeze), FreezeSystem.InflictFreezeEvent);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001DDD4 File Offset: 0x0001BFD4
		private void OnFreeze(object sender, object args)
		{
			GameObject gameObject = args as GameObject;
			Health component = gameObject.GetComponent<Health>();
			int change;
			if (gameObject.tag.Contains("Champion"))
			{
				change = -1 * Mathf.FloorToInt((float)component.maxHP * this.championPercentDamage);
			}
			else
			{
				change = -1 * Mathf.FloorToInt((float)component.maxHP * this.percentDamage);
			}
			component.HPChange(change);
		}

		// Token: 0x04000470 RID: 1136
		[Range(0f, 1f)]
		[SerializeField]
		private float percentDamage;

		// Token: 0x04000471 RID: 1137
		[Range(0f, 1f)]
		[SerializeField]
		private float championPercentDamage;
	}
}
