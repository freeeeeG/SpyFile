using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.CharacterPassives
{
	// Token: 0x0200025A RID: 602
	public class DamageOnShadowCloneDash : MonoBehaviour
	{
		// Token: 0x06000D0D RID: 3341 RVA: 0x0002F92C File Offset: 0x0002DB2C
		private void OnShadowCloneStart()
		{
			this.harm.damageAmount = Mathf.FloorToInt(this.damageMultiplier * this.gun.damage);
			this.harm.gameObject.SetActive(true);
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0002F964 File Offset: 0x0002DB64
		private void Start()
		{
			PlayerController component = base.transform.root.GetComponent<PlayerController>();
			this.gun = component.gun;
			this.shadowClone = component.GetComponentInChildren<ShadowClonePassive>();
			this.shadowClone.onUse.AddListener(new UnityAction(this.OnShadowCloneStart));
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0002F9B6 File Offset: 0x0002DBB6
		private void OnDestroy()
		{
			this.shadowClone.onUse.RemoveListener(new UnityAction(this.OnShadowCloneStart));
		}

		// Token: 0x0400095A RID: 2394
		[SerializeField]
		private Harmful harm;

		// Token: 0x0400095B RID: 2395
		[SerializeField]
		private float damageMultiplier = 1f;

		// Token: 0x0400095C RID: 2396
		private Gun gun;

		// Token: 0x0400095D RID: 2397
		private ShadowClonePassive shadowClone;
	}
}
