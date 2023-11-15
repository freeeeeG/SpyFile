using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000BE RID: 190
	public class HitlessDetector : MonoBehaviour
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0001C6E2 File Offset: 0x0001A8E2
		// (set) Token: 0x06000627 RID: 1575 RVA: 0x0001C6EA File Offset: 0x0001A8EA
		public bool hitless { get; private set; }

		// Token: 0x06000628 RID: 1576 RVA: 0x0001C6F3 File Offset: 0x0001A8F3
		private void OnHit()
		{
			this.hitless = false;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001C6FC File Offset: 0x0001A8FC
		private void Start()
		{
			this.hitless = true;
			base.Invoke("AddListener", 0.01f);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001C715 File Offset: 0x0001A915
		private void OnDestroy()
		{
			this.playerHealth.onHurt.RemoveListener(new UnityAction(this.OnHit));
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001C733 File Offset: 0x0001A933
		private void AddListener()
		{
			this.playerHealth.onHurt.AddListener(new UnityAction(this.OnHit));
		}

		// Token: 0x040003F8 RID: 1016
		[SerializeField]
		private PlayerHealth playerHealth;
	}
}
