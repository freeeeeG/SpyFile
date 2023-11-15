using System;
using System.Collections.Generic;
using UnityEngine;

namespace EndingCredit
{
	// Token: 0x02000193 RID: 403
	public class Creation : MonoBehaviour
	{
		// Token: 0x060008B9 RID: 2233 RVA: 0x00018ED8 File Offset: 0x000170D8
		private void Awake()
		{
			this._suppoterGroup = new List<GameObject>();
			for (int i = 0; i < this._number; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._supporter, this._transform).transform.GetChild(0).gameObject;
				this._suppoterGroup.Add(gameObject);
			}
			this._text.Initialize();
			this._creditList.Add(this._suppoterGroup.ToArray());
		}

		// Token: 0x040006F2 RID: 1778
		[SerializeField]
		private CreditList _creditList;

		// Token: 0x040006F3 RID: 1779
		[SerializeField]
		private CreditText _text;

		// Token: 0x040006F4 RID: 1780
		[SerializeField]
		private Transform _transform;

		// Token: 0x040006F5 RID: 1781
		[SerializeField]
		private GameObject _supporter;

		// Token: 0x040006F6 RID: 1782
		[SerializeField]
		private int _number;

		// Token: 0x040006F7 RID: 1783
		private List<GameObject> _suppoterGroup;
	}
}
