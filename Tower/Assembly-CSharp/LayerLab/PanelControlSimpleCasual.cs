using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LayerLab
{
	// Token: 0x020000B5 RID: 181
	public class PanelControlSimpleCasual : MonoBehaviour
	{
		// Token: 0x06000273 RID: 627 RVA: 0x00009BFC File Offset: 0x00007DFC
		private void Start()
		{
			this.textTitle = base.transform.GetComponentInChildren<TextMeshProUGUI>();
			this.buttonPrev.onClick.AddListener(new UnityAction(this.Click_Prev));
			this.buttonNext.onClick.AddListener(new UnityAction(this.Click_Next));
			foreach (object obj in this.panelTransformLight)
			{
				Transform transform = (Transform)obj;
				this.panelLight.Add(transform.gameObject);
				transform.gameObject.SetActive(false);
			}
			foreach (object obj2 in this.panelTransformDark)
			{
				Transform transform2 = (Transform)obj2;
				this.panelDark.Add(transform2.gameObject);
				transform2.gameObject.SetActive(false);
			}
			this.panelLight[this.page].SetActive(true);
			this.panelDark[this.page].SetActive(true);
			this.isReady = true;
			this.CheckControl();
			this.SetMode();
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00009D54 File Offset: 0x00007F54
		private void Update()
		{
			if (!this.isReady)
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

		// Token: 0x06000275 RID: 629 RVA: 0x00009D84 File Offset: 0x00007F84
		public void Click_Prev()
		{
			if (this.page <= 0 || !this.isReady)
			{
				return;
			}
			this.panelLight[this.page].SetActive(false);
			this.panelDark[this.page].SetActive(false);
			this.page--;
			this.panelLight[this.page].SetActive(true);
			this.panelDark[this.page].SetActive(true);
			if (!this.isDarakMode)
			{
				this.textTitle.text = this.panelLight[this.page].name;
			}
			else
			{
				this.textTitle.text = this.panelDark[this.page].name;
			}
			this.CheckControl();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00009E60 File Offset: 0x00008060
		public void Click_Next()
		{
			if (this.page >= this.panelLight.Count - 1)
			{
				return;
			}
			this.panelLight[this.page].SetActive(false);
			this.panelDark[this.page].SetActive(false);
			this.page++;
			this.panelLight[this.page].SetActive(true);
			this.panelDark[this.page].SetActive(true);
			this.CheckControl();
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00009EF3 File Offset: 0x000080F3
		private void SetArrowActive()
		{
			this.buttonPrev.gameObject.SetActive(this.page > 0);
			this.buttonNext.gameObject.SetActive(this.page < this.panelLight.Count - 1);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00009F34 File Offset: 0x00008134
		private void CheckControl()
		{
			if (!this.isDarakMode)
			{
				this.textTitle.text = this.panelLight[this.page].name.Replace("_", " ");
			}
			else
			{
				this.textTitle.text = this.panelDark[this.page].name.Replace("_", " ");
			}
			this.SetArrowActive();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00009FB1 File Offset: 0x000081B1
		public void Click_Mode()
		{
			this.isDarakMode = !this.isDarakMode;
			this.SetMode();
			this.CheckControl();
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00009FD0 File Offset: 0x000081D0
		private void SetMode()
		{
			if (!this.isDarakMode)
			{
				this.panelTransformLight.gameObject.SetActive(true);
				this.panelTransformDark.gameObject.SetActive(false);
				return;
			}
			this.panelTransformLight.gameObject.SetActive(false);
			this.panelTransformDark.gameObject.SetActive(true);
		}

		// Token: 0x04000208 RID: 520
		private int page;

		// Token: 0x04000209 RID: 521
		private bool isReady;

		// Token: 0x0400020A RID: 522
		[SerializeField]
		private List<GameObject> panelLight = new List<GameObject>();

		// Token: 0x0400020B RID: 523
		[SerializeField]
		private List<GameObject> panelDark = new List<GameObject>();

		// Token: 0x0400020C RID: 524
		private TextMeshProUGUI textTitle;

		// Token: 0x0400020D RID: 525
		[SerializeField]
		private Transform panelTransformLight;

		// Token: 0x0400020E RID: 526
		[SerializeField]
		private Transform panelTransformDark;

		// Token: 0x0400020F RID: 527
		[SerializeField]
		private Button buttonPrev;

		// Token: 0x04000210 RID: 528
		[SerializeField]
		private Button buttonNext;

		// Token: 0x04000211 RID: 529
		private bool isDarakMode;
	}
}
