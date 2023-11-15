using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LayerLab.CasualGame
{
	// Token: 0x020000BA RID: 186
	public class PanelControlCasualGame : MonoBehaviour
	{
		// Token: 0x0600028C RID: 652 RVA: 0x0000A40C File Offset: 0x0000860C
		private void Start()
		{
			this.textTitle = base.transform.GetComponentInChildren<TextMeshProUGUI>();
			this.buttonPrev.onClick.AddListener(new UnityAction(this.Click_Prev));
			this.buttonNext.onClick.AddListener(new UnityAction(this.Click_Next));
			foreach (object obj in this.panelTransform)
			{
				Transform transform = (Transform)obj;
				this.panels.Add(transform.gameObject);
				transform.gameObject.SetActive(false);
			}
			this.panels[this.page].SetActive(true);
			this.isReady = true;
			this.CheckControl();
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000A4E8 File Offset: 0x000086E8
		private void Update()
		{
			if (this.panels.Count <= 0 || !this.isReady)
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

		// Token: 0x0600028E RID: 654 RVA: 0x0000A528 File Offset: 0x00008728
		public void Click_Prev()
		{
			if (this.page <= 0 || !this.isReady)
			{
				return;
			}
			this.panels[this.page].SetActive(false);
			this.panels[--this.page].SetActive(true);
			this.textTitle.text = this.panels[this.page].name;
			this.CheckControl();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000A5A8 File Offset: 0x000087A8
		public void Click_Next()
		{
			if (this.page >= this.panels.Count - 1)
			{
				return;
			}
			this.panels[this.page].SetActive(false);
			this.panels[++this.page].SetActive(true);
			this.CheckControl();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000A60A File Offset: 0x0000880A
		private void SetArrowActive()
		{
			this.buttonPrev.gameObject.SetActive(this.page > 0);
			this.buttonNext.gameObject.SetActive(this.page < this.panels.Count - 1);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000A64A File Offset: 0x0000884A
		private void CheckControl()
		{
			this.textTitle.text = this.panels[this.page].name.Replace("_", " ");
			this.SetArrowActive();
		}

		// Token: 0x0400021C RID: 540
		private int page;

		// Token: 0x0400021D RID: 541
		private bool isReady;

		// Token: 0x0400021E RID: 542
		[SerializeField]
		private List<GameObject> panels = new List<GameObject>();

		// Token: 0x0400021F RID: 543
		private TextMeshProUGUI textTitle;

		// Token: 0x04000220 RID: 544
		[SerializeField]
		private Transform panelTransform;

		// Token: 0x04000221 RID: 545
		[SerializeField]
		private Button buttonPrev;

		// Token: 0x04000222 RID: 546
		[SerializeField]
		private Button buttonNext;
	}
}
