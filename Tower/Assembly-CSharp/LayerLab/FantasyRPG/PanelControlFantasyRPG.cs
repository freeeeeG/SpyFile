using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LayerLab.FantasyRPG
{
	// Token: 0x020000B7 RID: 183
	public class PanelControlFantasyRPG : MonoBehaviour
	{
		// Token: 0x0600027F RID: 639 RVA: 0x0000A0B0 File Offset: 0x000082B0
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

		// Token: 0x06000280 RID: 640 RVA: 0x0000A18C File Offset: 0x0000838C
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

		// Token: 0x06000281 RID: 641 RVA: 0x0000A1CC File Offset: 0x000083CC
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

		// Token: 0x06000282 RID: 642 RVA: 0x0000A24C File Offset: 0x0000844C
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

		// Token: 0x06000283 RID: 643 RVA: 0x0000A2AE File Offset: 0x000084AE
		private void SetArrowActive()
		{
			this.buttonPrev.gameObject.SetActive(this.page > 0);
			this.buttonNext.gameObject.SetActive(this.page < this.panels.Count - 1);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000A2EE File Offset: 0x000084EE
		private void CheckControl()
		{
			this.textTitle.text = this.panels[this.page].name.Replace("_", " ");
			this.SetArrowActive();
		}

		// Token: 0x04000213 RID: 531
		private int page;

		// Token: 0x04000214 RID: 532
		private bool isReady;

		// Token: 0x04000215 RID: 533
		[SerializeField]
		private List<GameObject> panels = new List<GameObject>();

		// Token: 0x04000216 RID: 534
		private TextMeshProUGUI textTitle;

		// Token: 0x04000217 RID: 535
		[SerializeField]
		private Transform panelTransform;

		// Token: 0x04000218 RID: 536
		[SerializeField]
		private Button buttonPrev;

		// Token: 0x04000219 RID: 537
		[SerializeField]
		private Button buttonNext;
	}
}
