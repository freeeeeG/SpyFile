using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndingCredit
{
	// Token: 0x02000194 RID: 404
	public class CreditList : MonoBehaviour
	{
		// Token: 0x060008BB RID: 2235 RVA: 0x00018F50 File Offset: 0x00017150
		public void Add(GameObject[] supporter)
		{
			this.AddList(this._maker);
			this.AddList(supporter);
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00018F74 File Offset: 0x00017174
		private void AddList(GameObject[] list)
		{
			foreach (GameObject item in list)
			{
				this._creditList.Add(item);
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00018FA1 File Offset: 0x000171A1
		private IEnumerator CRun()
		{
			this.Refresh();
			int currentCreditListIndex = 1;
			int listCount = this._creditList.Count - 1;
			while (currentCreditListIndex < listCount)
			{
				if ((this._destination.transform.position - this._creditList[currentCreditListIndex].transform.position).normalized.y < 0f)
				{
					this.Activate(currentCreditListIndex + 1);
					this.Deactivate(currentCreditListIndex - 1);
					int num = currentCreditListIndex;
					currentCreditListIndex = num + 1;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00018FB0 File Offset: 0x000171B0
		private void Activate(int index)
		{
			this._creditList[index].SetActive(true);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00018FC4 File Offset: 0x000171C4
		private void Deactivate(int index)
		{
			this._creditList[index].SetActive(false);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00018FD8 File Offset: 0x000171D8
		private void Refresh()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this._contentSizeFitter.transform);
		}

		// Token: 0x040006F8 RID: 1784
		[SerializeField]
		private GameObject[] _maker;

		// Token: 0x040006F9 RID: 1785
		[SerializeField]
		private Transform _destination;

		// Token: 0x040006FA RID: 1786
		[SerializeField]
		private ContentSizeFitter _contentSizeFitter;

		// Token: 0x040006FB RID: 1787
		private List<GameObject> _creditList = new List<GameObject>();
	}
}
