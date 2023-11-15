using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x02001270 RID: 4720
	public class SaintSwordEmerge : MonoBehaviour
	{
		// Token: 0x06005D97 RID: 23959 RVA: 0x00113607 File Offset: 0x00111807
		public void Emerge(Character owner)
		{
			base.StartCoroutine(this.CMove(owner));
		}

		// Token: 0x06005D98 RID: 23960 RVA: 0x00113617 File Offset: 0x00111817
		private IEnumerator CMove(Character owner)
		{
			float elapsed = 0f;
			Vector3 source = this._sourceTransfom.position;
			Vector3 dest = this._destTransfom.position;
			base.transform.position = source;
			this.Show();
			while (elapsed < this._duration)
			{
				yield return null;
				elapsed += owner.chronometer.master.deltaTime;
				base.transform.position = Vector2.Lerp(source, dest, elapsed / this._duration);
			}
			base.transform.position = dest;
			yield break;
		}

		// Token: 0x06005D99 RID: 23961 RVA: 0x0011362D File Offset: 0x0011182D
		private void Show()
		{
			this._body.gameObject.SetActive(true);
		}

		// Token: 0x06005D9A RID: 23962 RVA: 0x00113640 File Offset: 0x00111840
		public void Hide()
		{
			this._body.gameObject.SetActive(false);
		}

		// Token: 0x04004B1F RID: 19231
		[SerializeField]
		private Transform _body;

		// Token: 0x04004B20 RID: 19232
		[SerializeField]
		private float _duration;

		// Token: 0x04004B21 RID: 19233
		[SerializeField]
		private Transform _sourceTransfom;

		// Token: 0x04004B22 RID: 19234
		[SerializeField]
		private Transform _destTransfom;
	}
}
