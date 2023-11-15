using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords
{
	// Token: 0x02001285 RID: 4741
	public class LightSwordProjectile : MonoBehaviour
	{
		// Token: 0x06005E06 RID: 24070 RVA: 0x0011495A File Offset: 0x00112B5A
		public IEnumerator CFire(Vector2 firePosition, Vector2 destination, float angle)
		{
			this.Initialize(firePosition, angle);
			this.Show();
			yield return this.CMove(firePosition, destination);
			this.Hide();
			yield break;
		}

		// Token: 0x06005E07 RID: 24071 RVA: 0x0011497E File Offset: 0x00112B7E
		private IEnumerator CMove(Vector2 src, Vector2 dest)
		{
			float elapsed = 0f;
			while (elapsed < this._duration)
			{
				yield return null;
				elapsed += Chronometer.global.deltaTime;
				base.transform.position = Vector2.Lerp(src, dest, elapsed / this._duration);
			}
			base.transform.position = dest;
			yield break;
		}

		// Token: 0x06005E08 RID: 24072 RVA: 0x0011499B File Offset: 0x00112B9B
		private void Initialize(Vector2 position, float angle)
		{
			base.transform.position = position;
			this._body.transform.rotation = Quaternion.Euler(0f, 0f, angle);
		}

		// Token: 0x06005E09 RID: 24073 RVA: 0x001149CE File Offset: 0x00112BCE
		private void Show()
		{
			this._body.SetActive(true);
		}

		// Token: 0x06005E0A RID: 24074 RVA: 0x001149DC File Offset: 0x00112BDC
		public void Hide()
		{
			this._body.SetActive(false);
		}

		// Token: 0x04004B88 RID: 19336
		[SerializeField]
		private GameObject _body;

		// Token: 0x04004B89 RID: 19337
		[SerializeField]
		private float _duration = 0.5f;
	}
}
