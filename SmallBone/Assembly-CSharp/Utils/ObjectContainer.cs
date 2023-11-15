using System;
using Services;
using UnityEngine;

namespace Utils
{
	// Token: 0x0200046B RID: 1131
	public class ObjectContainer : MonoBehaviour
	{
		// Token: 0x06001593 RID: 5523 RVA: 0x00043E6B File Offset: 0x0004206B
		private void Awake()
		{
			this._element.transform.SetParent(null, false);
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x00043E7F File Offset: 0x0004207F
		private void OnEnable()
		{
			this._element.SetActive(true);
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x00043E8D File Offset: 0x0004208D
		private void OnDisable()
		{
			this._element.SetActive(false);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x00043E9B File Offset: 0x0004209B
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			UnityEngine.Object.Destroy(this._element);
		}

		// Token: 0x040012DE RID: 4830
		[SerializeField]
		private GameObject _element;
	}
}
