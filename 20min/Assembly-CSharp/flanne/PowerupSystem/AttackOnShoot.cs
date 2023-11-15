using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PowerupSystem
{
	// Token: 0x02000245 RID: 581
	public abstract class AttackOnShoot : MonoBehaviour
	{
		// Token: 0x06000CB1 RID: 3249 RVA: 0x0002E480 File Offset: 0x0002C680
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.myGun = componentInParent.gun;
			this.myGun.OnShoot.AddListener(new UnityAction(this.IncrementCounter));
			this.Init();
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0002E4C2 File Offset: 0x0002C6C2
		private void OnDestroy()
		{
			this.myGun.OnShoot.RemoveListener(new UnityAction(this.IncrementCounter));
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0002E4E0 File Offset: 0x0002C6E0
		public void IncrementCounter()
		{
			this._counter++;
			if (this._counter >= this.shotsPerAttack)
			{
				this._counter = 0;
				this.Attack();
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO == null)
				{
					return;
				}
				soundEffectSO.Play(null);
			}
		}

		// Token: 0x06000CB4 RID: 3252
		public abstract void Attack();

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00002F51 File Offset: 0x00001151
		protected virtual void Init()
		{
		}

		// Token: 0x040008E6 RID: 2278
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x040008E7 RID: 2279
		[SerializeField]
		private int shotsPerAttack;

		// Token: 0x040008E8 RID: 2280
		private int _counter;

		// Token: 0x040008E9 RID: 2281
		private Gun myGun;
	}
}
