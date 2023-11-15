using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x02001260 RID: 4704
	public class GiganticSaintSword : MonoBehaviour
	{
		// Token: 0x140000FE RID: 254
		// (add) Token: 0x06005D3E RID: 23870 RVA: 0x001127AC File Offset: 0x001109AC
		// (remove) Token: 0x06005D3F RID: 23871 RVA: 0x001127E4 File Offset: 0x001109E4
		public event GiganticSaintSword.OnStuckDelegate OnStuck;

		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06005D40 RID: 23872 RVA: 0x00112819 File Offset: 0x00110A19
		public bool isStuck
		{
			get
			{
				return this._stuck.gameObject.activeSelf;
			}
		}

		// Token: 0x06005D41 RID: 23873 RVA: 0x0011282B File Offset: 0x00110A2B
		public void Fire(Vector2 firePosition, float destY)
		{
			this._projectile.transform.position = firePosition;
			this._projectile.SetActive(true);
			base.StartCoroutine(this.CMove(destY));
		}

		// Token: 0x06005D42 RID: 23874 RVA: 0x0011285D File Offset: 0x00110A5D
		private IEnumerator CMove(float destY)
		{
			float elapsed = 0f;
			Vector3 source = this._projectile.transform.position;
			Vector2 dest = new Vector2(source.x, destY);
			while (elapsed < this._dropDuration)
			{
				elapsed += Chronometer.global.deltaTime;
				this._projectile.transform.position = Vector2.Lerp(source, dest, elapsed / this._dropDuration);
				yield return null;
			}
			this._projectile.transform.position = dest;
			this.Stuck(dest);
			yield break;
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x00112874 File Offset: 0x00110A74
		private void Stuck(Vector2 point)
		{
			GiganticSaintSword.OnStuckDelegate onStuck = this.OnStuck;
			if (onStuck != null)
			{
				onStuck();
			}
			this._projectile.SetActive(false);
			this._stuck.transform.position = point;
			this._stuck.SetActive(true);
		}

		// Token: 0x06005D44 RID: 23876 RVA: 0x001128C0 File Offset: 0x00110AC0
		public void Despawn()
		{
			this._projectile.SetActive(false);
			this._stuck.SetActive(false);
		}

		// Token: 0x04004ADC RID: 19164
		[SerializeField]
		[Header("Projectiles")]
		private GameObject _projectile;

		// Token: 0x04004ADD RID: 19165
		[SerializeField]
		private float _dropDuration;

		// Token: 0x04004ADE RID: 19166
		[SerializeField]
		[Header("Stuck")]
		private GameObject _stuck;

		// Token: 0x02001261 RID: 4705
		// (Invoke) Token: 0x06005D47 RID: 23879
		public delegate void OnStuckDelegate();
	}
}
