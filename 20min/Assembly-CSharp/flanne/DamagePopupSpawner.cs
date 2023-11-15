using System;
using TMPro;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200007B RID: 123
	public class DamagePopupSpawner : MonoBehaviour
	{
		// Token: 0x0600050F RID: 1295 RVA: 0x0001924C File Offset: 0x0001744C
		public void OnDamageTaken(int amount)
		{
			if (amount == 0)
			{
				return;
			}
			GameObject pooledObject = ObjectPooler.SharedInstance.GetPooledObject(this.popupOpTag);
			pooledObject.transform.position = base.transform.position;
			pooledObject.SetActive(true);
			pooledObject.GetComponent<TextMeshPro>().text = Mathf.Abs(amount).ToString();
		}

		// Token: 0x040002FA RID: 762
		[SerializeField]
		private string popupOpTag;
	}
}
