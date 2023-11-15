using System;
using Services;
using UnityEngine;

namespace Characters.Abilities.Spirits
{
	// Token: 0x02000B83 RID: 2947
	public class SpiritContainer : MonoBehaviour
	{
		// Token: 0x06003C03 RID: 15363 RVA: 0x000B0E37 File Offset: 0x000AF037
		private void Awake()
		{
			this._spirit.transform.SetParent(null, false);
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x000B0E4B File Offset: 0x000AF04B
		private void OnEnable()
		{
			this._spirit.gameObject.SetActive(true);
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x000B0E5E File Offset: 0x000AF05E
		private void OnDisable()
		{
			if (Service.quitting)
			{
				return;
			}
			this._spirit.gameObject.SetActive(false);
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x000B0E79 File Offset: 0x000AF079
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			UnityEngine.Object.Destroy(this._spirit.gameObject);
		}

		// Token: 0x04002F26 RID: 12070
		[SerializeField]
		private Spirit _spirit;
	}
}
