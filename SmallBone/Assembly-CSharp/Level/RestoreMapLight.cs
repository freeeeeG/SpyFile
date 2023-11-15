using System;
using UnityEngine;

namespace Level
{
	// Token: 0x02000517 RID: 1303
	public class RestoreMapLight : MonoBehaviour
	{
		// Token: 0x060019B3 RID: 6579 RVA: 0x00050A29 File Offset: 0x0004EC29
		private void OnTriggerEnter2D(Collider2D collision)
		{
			Map.Instance.RestoreLight(this._changingTime);
		}

		// Token: 0x0400167E RID: 5758
		[Information("레이어를 Interaction으로 설정하고 트리거 콜라이더를 넣어주세요.", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private float _changingTime = 1f;
	}
}
