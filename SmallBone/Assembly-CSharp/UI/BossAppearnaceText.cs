using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x02000389 RID: 905
	public class BossAppearnaceText : MonoBehaviour
	{
		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001088 RID: 4232 RVA: 0x00030EAE File Offset: 0x0002F0AE
		// (set) Token: 0x06001089 RID: 4233 RVA: 0x00030EBB File Offset: 0x0002F0BB
		public new string name
		{
			get
			{
				return this._name.text;
			}
			set
			{
				this._name.text = value;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x00030EC9 File Offset: 0x0002F0C9
		// (set) Token: 0x0600108B RID: 4235 RVA: 0x00030ED6 File Offset: 0x0002F0D6
		public string subname
		{
			get
			{
				return this._subname.text;
			}
			set
			{
				this._subname.text = value;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00030EE4 File Offset: 0x0002F0E4
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x00030EF1 File Offset: 0x0002F0F1
		public bool visible
		{
			get
			{
				return this._container.activeSelf;
			}
			set
			{
				this._container.SetActive(value);
			}
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00030EFF File Offset: 0x0002F0FF
		public void Appear(string name, string subName, float duration = 1.7f)
		{
			this._name.text = name;
			this._subname.text = subName;
			this.visible = true;
			base.StartCoroutine(this.CAppear(duration));
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00030F2E File Offset: 0x0002F12E
		private IEnumerator CAppear(float duration)
		{
			this._name.color = new Color(1f, 1f, 1f, 0f);
			this._subname.color = new Color(1f, 1f, 1f, 0f);
			Color baseColor = new Color(1f, 1f, 1f, 0f);
			Color destinationColor = new Color(1f, 1f, 1f, 1f);
			float elapsed = 0f;
			for (;;)
			{
				yield return null;
				Color color = Color.Lerp(baseColor, destinationColor, elapsed / duration);
				this._name.color = color;
				this._subname.color = color;
				if (elapsed > duration)
				{
					break;
				}
				elapsed += Chronometer.global.deltaTime;
			}
			yield break;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00030F44 File Offset: 0x0002F144
		public void Disappear(float duration = 1.7f)
		{
			base.StartCoroutine(this.CDisappear(duration));
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00030F54 File Offset: 0x0002F154
		private IEnumerator CDisappear(float duration)
		{
			Color baseColor = new Color(1f, 1f, 1f, 1f);
			Color destinationColor = new Color(1f, 1f, 1f, 0f);
			float elapsed = 0f;
			for (;;)
			{
				yield return null;
				Color color = Color.Lerp(baseColor, destinationColor, elapsed / duration);
				this._name.color = color;
				this._subname.color = color;
				if (elapsed > duration)
				{
					break;
				}
				elapsed += Chronometer.global.deltaTime;
			}
			this.visible = false;
			yield break;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00030F6A File Offset: 0x0002F16A
		public IEnumerator ShowAndHideText(string name, string subName)
		{
			this.Appear(name, subName, 0.5f);
			yield return Chronometer.global.WaitForSeconds(2.5f);
			yield return this.CDisappear(0.5f);
			yield break;
		}

		// Token: 0x04000D8F RID: 3471
		[SerializeField]
		private GameObject _container;

		// Token: 0x04000D90 RID: 3472
		[SerializeField]
		private TextMeshProUGUI _name;

		// Token: 0x04000D91 RID: 3473
		[SerializeField]
		private TextMeshProUGUI _subname;
	}
}
