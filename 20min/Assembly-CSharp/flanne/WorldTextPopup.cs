using System;
using TMPro;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200007C RID: 124
	public class WorldTextPopup : MonoBehaviour
	{
		// Token: 0x06000511 RID: 1297 RVA: 0x000192A4 File Offset: 0x000174A4
		private void OnEnable()
		{
			this.timer = 0f;
			base.transform.position = new Vector3(base.transform.position.x + Random.Range(-0.5f, 0.5f), base.transform.position.y + Random.Range(-0.5f, 0.5f));
			this.newPos = new Vector3(base.transform.position.x + Random.Range(-0.5f, 0.5f), base.transform.position.y + 0.5f, 0f);
			base.transform.localScale = Vector3.one;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00019364 File Offset: 0x00017564
		private void Update()
		{
			if (this.timer < this.lifetime)
			{
				this.timer += Time.deltaTime;
				base.transform.position = Vector3.MoveTowards(base.transform.position, this.newPos, (1f - this.timer / this.lifetime) / 2f * Time.deltaTime);
				if (this.timer < this.lifetime * 0.3f)
				{
					base.transform.localScale = Vector3.one + Vector3.one * (this.timer / 0.3f);
					return;
				}
			}
			else
			{
				if (this.destroyOnStop)
				{
					Object.Destroy(base.gameObject);
					return;
				}
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x040002FB RID: 763
		[SerializeField]
		private TextMeshPro tmp;

		// Token: 0x040002FC RID: 764
		[SerializeField]
		private float lifetime;

		// Token: 0x040002FD RID: 765
		[SerializeField]
		private bool destroyOnStop;

		// Token: 0x040002FE RID: 766
		private float timer;

		// Token: 0x040002FF RID: 767
		private Vector3 newPos;
	}
}
