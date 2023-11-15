using System;
using System.Collections;
using Level;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Gear.Quintessences
{
	// Token: 0x020008DB RID: 2267
	public sealed class QuintessenceSubObject : MonoBehaviour
	{
		// Token: 0x06003063 RID: 12387 RVA: 0x00091154 File Offset: 0x0008F354
		private void Start()
		{
			this.RegisterEvents();
			this.Deactive();
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x00091162 File Offset: 0x0008F362
		private void RegisterEvents()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.Deactive;
			this._quintessence.onDiscard += this.OnDiscard;
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x00091196 File Offset: 0x0008F396
		public void Use()
		{
			if (base.transform.parent != null)
			{
				base.transform.SetParent(null);
			}
			UnityEvent onUse = this._onUse;
			if (onUse != null)
			{
				onUse.Invoke();
			}
			base.StartCoroutine(this.CLifeSpan());
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000911D5 File Offset: 0x0008F3D5
		private void OnUse()
		{
			UnityEvent onUse = this._onUse;
			if (onUse == null)
			{
				return;
			}
			onUse.Invoke();
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000911E8 File Offset: 0x0008F3E8
		private void OnDiscard(Gear gear)
		{
			base.transform.SetParent(Map.Instance.transform);
			UnityEvent onDiscard = this._onDiscard;
			if (onDiscard != null)
			{
				onDiscard.Invoke();
			}
			this.Deactive();
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.Deactive;
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000075E7 File Offset: 0x000057E7
		private void Deactive()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x0009123C File Offset: 0x0008F43C
		private IEnumerator CLifeSpan()
		{
			for (float elapsed = 0f; elapsed < this._lifeTime; elapsed += Chronometer.global.deltaTime)
			{
				yield return null;
			}
			this.Deactive();
			yield break;
		}

		// Token: 0x0400280C RID: 10252
		[SerializeField]
		private float _lifeTime;

		// Token: 0x0400280D RID: 10253
		[SerializeField]
		private Quintessence _quintessence;

		// Token: 0x0400280E RID: 10254
		[SerializeField]
		private UnityEvent _onUse;

		// Token: 0x0400280F RID: 10255
		[SerializeField]
		private UnityEvent _onDiscard;
	}
}
