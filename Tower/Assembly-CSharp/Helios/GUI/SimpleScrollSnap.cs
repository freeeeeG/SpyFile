using System;
using UnityEngine;
using UnityEngine.UI;

namespace Helios.GUI
{
	// Token: 0x020000E2 RID: 226
	public class SimpleScrollSnap : MonoBehaviour
	{
		// Token: 0x06000346 RID: 838 RVA: 0x0000E974 File Offset: 0x0000CB74
		private void Awake()
		{
			this._arrPosition = new float[base.transform.childCount];
			this._nbDistance = 1f / ((float)this._arrPosition.Length - 1f);
			this._scrollbar = this._goScrollbar.GetComponent<Scrollbar>();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000E9C4 File Offset: 0x0000CBC4
		private void Update()
		{
			if (this._isTimeToRun)
			{
				this.Snap(this._nbDistance, this._arrPosition, this._btnCliked);
				this._nbTimer += Time.deltaTime;
				if (this._nbTimer > 1f)
				{
					this._nbTimer = 0f;
					this._isTimeToRun = false;
				}
			}
			for (int i = 0; i < this._arrPosition.Length; i++)
			{
				this._arrPosition[i] = this._nbDistance * (float)i;
			}
			if (Input.GetMouseButton(0))
			{
				this._nbScrollPosition = this._scrollbar.value;
			}
			else
			{
				for (int j = 0; j < this._arrPosition.Length; j++)
				{
					if (this._nbScrollPosition < this._arrPosition[j] + this._nbDistance / 2f && this._nbScrollPosition > this._arrPosition[j] - this._nbDistance / 2f)
					{
						this._scrollbar.value = Mathf.Lerp(this._scrollbar.value, this._arrPosition[j], 0.1f);
					}
				}
			}
			for (int k = 0; k < this._arrPosition.Length; k++)
			{
				if (this._nbScrollPosition < this._arrPosition[k] + this._nbDistance / 2f && this._nbScrollPosition > this._arrPosition[k] - this._nbDistance / 2f)
				{
					base.transform.GetChild(k).localScale = Vector2.Lerp(base.transform.GetChild(k).localScale, new Vector2(1f, 1f), 0.1f);
					this._goPagination.transform.GetChild(k).localScale = Vector2.Lerp(this._goPagination.transform.GetChild(k).localScale, new Vector2(1.2f, 1.2f), 0.1f);
					this._goPagination.transform.GetChild(k).GetComponent<Image>().sprite = this._arrPaginationSprites[1];
					for (int l = 0; l < this._arrPosition.Length; l++)
					{
						if (l != k)
						{
							this._goPagination.transform.GetChild(l).GetComponent<Image>().sprite = this._arrPaginationSprites[0];
							this._goPagination.transform.GetChild(l).localScale = Vector2.Lerp(this._goPagination.transform.GetChild(l).localScale, new Vector2(0.8f, 0.8f), 0.1f);
							base.transform.GetChild(l).localScale = Vector2.Lerp(base.transform.GetChild(l).localScale, new Vector2(0.8f, 0.8f), 0.1f);
						}
					}
				}
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000ECC0 File Offset: 0x0000CEC0
		private void Snap(float distance, float[] pos, Button btn)
		{
			for (int i = 0; i < pos.Length; i++)
			{
				if (this._nbScrollPosition < pos[i] + distance / 2f && this._nbScrollPosition > pos[i] - distance / 2f)
				{
					this._scrollbar.value = Mathf.Lerp(this._scrollbar.value, pos[this._nbButtonIndex], 1f * Time.deltaTime);
				}
			}
			for (int j = 0; j < btn.transform.parent.transform.childCount; j++)
			{
				btn.transform.name = ".";
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000ED60 File Offset: 0x0000CF60
		public void WhichBtnClicked(Button btn)
		{
			btn.transform.name = "clicked";
			for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
			{
				if (btn.transform.parent.transform.GetChild(i).transform.name == "clicked")
				{
					this._nbButtonIndex = i;
					this._btnCliked = btn;
					this._nbTimer = 0f;
					this._nbScrollPosition = this._arrPosition[this._nbButtonIndex];
					this._isTimeToRun = true;
				}
			}
		}

		// Token: 0x0400031A RID: 794
		[SerializeField]
		private GameObject _goScrollbar;

		// Token: 0x0400031B RID: 795
		[SerializeField]
		private GameObject _goPagination;

		// Token: 0x0400031C RID: 796
		[SerializeField]
		private Sprite[] _arrPaginationSprites;

		// Token: 0x0400031D RID: 797
		private int _nbButtonIndex;

		// Token: 0x0400031E RID: 798
		private bool _isTimeToRun;

		// Token: 0x0400031F RID: 799
		private float _nbScrollPosition;

		// Token: 0x04000320 RID: 800
		private float _nbTimer;

		// Token: 0x04000321 RID: 801
		private float _nbDistance;

		// Token: 0x04000322 RID: 802
		private float[] _arrPosition;

		// Token: 0x04000323 RID: 803
		private Button _btnCliked;

		// Token: 0x04000324 RID: 804
		private Scrollbar _scrollbar;
	}
}
