using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000E7 RID: 231
	public class HealOnSmiteKill : MonoBehaviour
	{
		// Token: 0x060006CB RID: 1739 RVA: 0x0001E5B9 File Offset: 0x0001C7B9
		private void OnSmiteKill(object sender, object args)
		{
			this._killCounter++;
			if (this._killCounter >= this.killsToHeal)
			{
				this._killCounter = 0;
				this.playerHealth.Heal(1);
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001E5EC File Offset: 0x0001C7EC
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.playerHealth = componentInParent.playerHealth;
			this.AddObserver(new Action<object, object>(this.OnSmiteKill), SmitePassive.SmiteKillNotification);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001E623 File Offset: 0x0001C823
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnSmiteKill), SmitePassive.SmiteKillNotification);
		}

		// Token: 0x04000496 RID: 1174
		[SerializeField]
		private int killsToHeal;

		// Token: 0x04000497 RID: 1175
		private PlayerHealth playerHealth;

		// Token: 0x04000498 RID: 1176
		private int _killCounter;
	}
}
