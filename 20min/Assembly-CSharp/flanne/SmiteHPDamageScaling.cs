using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000112 RID: 274
	public class SmiteHPDamageScaling : MonoBehaviour
	{
		// Token: 0x060007AB RID: 1963 RVA: 0x00021128 File Offset: 0x0001F328
		private void OnTweakDamage(object sender, object args)
		{
			List<ValueModifier> list = args as List<ValueModifier>;
			int num = this.playerHealth.hp * 10;
			list.Add(new AddValueModifier(0, (float)num));
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00021158 File Offset: 0x0001F358
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.playerHealth = componentInParent.playerHealth;
			this.AddObserver(new Action<object, object>(this.OnTweakDamage), SmitePassive.SmiteTweakDamageNotification);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0002118F File Offset: 0x0001F38F
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnTweakDamage), SmitePassive.SmiteTweakDamageNotification);
		}

		// Token: 0x04000583 RID: 1411
		private PlayerHealth playerHealth;
	}
}
