using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x0200126A RID: 4714
	public class PillarOfLightContainerPool : MonoBehaviour
	{
		// Token: 0x06005D76 RID: 23926 RVA: 0x001130C8 File Offset: 0x001112C8
		private void Start()
		{
			this._pool = new List<PillarOfLightContainer>(this._container.childCount);
			foreach (object obj in this._container)
			{
				Transform transform = (Transform)obj;
				this._pool.Add(transform.GetComponent<PillarOfLightContainer>());
			}
		}

		// Token: 0x06005D77 RID: 23927 RVA: 0x00113144 File Offset: 0x00111344
		public void Run(Character owner)
		{
			base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x06005D78 RID: 23928 RVA: 0x00113154 File Offset: 0x00111354
		private IEnumerator CRun(Character owner)
		{
			PillarOfLightContainer selected = this._pool.Random<PillarOfLightContainer>();
			selected.gameObject.SetActive(true);
			selected.Sign(owner);
			yield return owner.chronometer.master.WaitForSeconds(this._delay);
			selected.Attack(owner);
			yield break;
		}

		// Token: 0x04004B04 RID: 19204
		[SerializeField]
		private Transform _container;

		// Token: 0x04004B05 RID: 19205
		[SerializeField]
		private float _delay;

		// Token: 0x04004B06 RID: 19206
		private List<PillarOfLightContainer> _pool;
	}
}
