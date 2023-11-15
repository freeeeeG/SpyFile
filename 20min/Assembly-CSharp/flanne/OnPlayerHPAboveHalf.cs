using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000A3 RID: 163
	public class OnPlayerHPAboveHalf : MonoBehaviour
	{
		// Token: 0x0600058A RID: 1418 RVA: 0x0001AA68 File Offset: 0x00018C68
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.health = componentInParent.playerHealth;
			this.health.onHealthChangedTo.AddListener(new UnityAction<int>(this.OnHPChanged));
			base.StartCoroutine(this.WaitToCheckHPCR());
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001AAB1 File Offset: 0x00018CB1
		private void OnDestroy()
		{
			this.health.onHealthChangedTo.RemoveListener(new UnityAction<int>(this.OnHPChanged));
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001AACF File Offset: 0x00018CCF
		private void OnHPChanged(int newHP)
		{
			this.CheckHP(newHP);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001AAD8 File Offset: 0x00018CD8
		private void CheckHP(int hp)
		{
			if (this.orEqualTo)
			{
				if ((float)hp / (float)this.health.maxHP >= 0.5f)
				{
					UnityEvent unityEvent = this.onHPBelowHalf;
					if (unityEvent == null)
					{
						return;
					}
					unityEvent.Invoke();
					return;
				}
			}
			else if ((float)hp / (float)this.health.maxHP > 0.5f)
			{
				UnityEvent unityEvent2 = this.onHPBelowHalf;
				if (unityEvent2 == null)
				{
					return;
				}
				unityEvent2.Invoke();
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001AB3A File Offset: 0x00018D3A
		private IEnumerator WaitToCheckHPCR()
		{
			yield return null;
			this.CheckHP(this.health.hp);
			yield break;
		}

		// Token: 0x04000379 RID: 889
		[SerializeField]
		private bool orEqualTo;

		// Token: 0x0400037A RID: 890
		public UnityEvent onHPBelowHalf;

		// Token: 0x0400037B RID: 891
		private PlayerHealth health;
	}
}
