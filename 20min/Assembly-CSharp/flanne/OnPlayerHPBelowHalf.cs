using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000A4 RID: 164
	public class OnPlayerHPBelowHalf : MonoBehaviour
	{
		// Token: 0x06000590 RID: 1424 RVA: 0x0001AB4C File Offset: 0x00018D4C
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.health = componentInParent.playerHealth;
			this.health.onHealthChangedTo.AddListener(new UnityAction<int>(this.OnHPChanged));
			base.StartCoroutine(this.WaitToCheckHPCR());
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001AB95 File Offset: 0x00018D95
		private void OnDestroy()
		{
			this.health.onHealthChangedTo.RemoveListener(new UnityAction<int>(this.OnHPChanged));
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001ABB3 File Offset: 0x00018DB3
		private void OnHPChanged(int newHP)
		{
			this.CheckHP(newHP);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001ABBC File Offset: 0x00018DBC
		private void CheckHP(int hp)
		{
			if (this.orEqualTo)
			{
				if ((float)hp / (float)this.health.maxHP <= 0.5f)
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
			else if ((float)hp / (float)this.health.maxHP < 0.5f)
			{
				UnityEvent unityEvent2 = this.onHPBelowHalf;
				if (unityEvent2 == null)
				{
					return;
				}
				unityEvent2.Invoke();
			}
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001AC1E File Offset: 0x00018E1E
		private IEnumerator WaitToCheckHPCR()
		{
			yield return null;
			this.CheckHP(this.health.hp);
			yield break;
		}

		// Token: 0x0400037C RID: 892
		[SerializeField]
		private bool orEqualTo;

		// Token: 0x0400037D RID: 893
		public UnityEvent onHPBelowHalf;

		// Token: 0x0400037E RID: 894
		private PlayerHealth health;
	}
}
