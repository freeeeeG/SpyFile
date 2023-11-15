using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Helios.GUI
{
	// Token: 0x020000DA RID: 218
	public class ControlPanel : MonoBehaviour
	{
		// Token: 0x06000324 RID: 804 RVA: 0x0000E2E4 File Offset: 0x0000C4E4
		private void Start()
		{
			this._textTitle = base.transform.GetComponentInChildren<TextMeshProUGUI>();
			this._btnPrev.onClick.AddListener(new UnityAction(this.Click_Prev));
			this._btnNext.onClick.AddListener(new UnityAction(this.Click_Next));
			foreach (object obj in this._tfPanel)
			{
				Transform transform = (Transform)obj;
				this._lsPanel.Add(transform.gameObject);
				transform.gameObject.SetActive(false);
			}
			this._lsPanel[this._nbPage].SetActive(true);
			this._isReady = true;
			this.CheckControl();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000E3C0 File Offset: 0x0000C5C0
		private void Update()
		{
			if (this._lsPanel.Count <= 0 || !this._isReady)
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.Click_Prev();
				return;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.Click_Next();
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000E400 File Offset: 0x0000C600
		public void Click_Prev()
		{
			if (this._nbPage <= 0 || !this._isReady)
			{
				return;
			}
			this._lsPanel[this._nbPage].SetActive(false);
			this._lsPanel[--this._nbPage].SetActive(true);
			this._textTitle.text = this._lsPanel[this._nbPage].name;
			this.CheckControl();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000E480 File Offset: 0x0000C680
		public void Click_Next()
		{
			if (this._nbPage >= this._lsPanel.Count - 1)
			{
				return;
			}
			this._lsPanel[this._nbPage].SetActive(false);
			this._lsPanel[++this._nbPage].SetActive(true);
			this.CheckControl();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E4E2 File Offset: 0x0000C6E2
		private void SetArrowActive()
		{
			this._btnPrev.gameObject.SetActive(this._nbPage > 0);
			this._btnNext.gameObject.SetActive(this._nbPage < this._lsPanel.Count - 1);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E522 File Offset: 0x0000C722
		private void CheckControl()
		{
			this._textTitle.text = this._lsPanel[this._nbPage].name.Replace("_", " ");
			this.SetArrowActive();
		}

		// Token: 0x04000304 RID: 772
		private int _nbPage;

		// Token: 0x04000305 RID: 773
		private bool _isReady;

		// Token: 0x04000306 RID: 774
		private TextMeshProUGUI _textTitle;

		// Token: 0x04000307 RID: 775
		[SerializeField]
		private List<GameObject> _lsPanel = new List<GameObject>();

		// Token: 0x04000308 RID: 776
		[SerializeField]
		private Transform _tfPanel;

		// Token: 0x04000309 RID: 777
		[SerializeField]
		private Button _btnPrev;

		// Token: 0x0400030A RID: 778
		[SerializeField]
		private Button _btnNext;
	}
}
