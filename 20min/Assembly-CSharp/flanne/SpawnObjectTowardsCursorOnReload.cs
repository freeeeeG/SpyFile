using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x02000066 RID: 102
	public class SpawnObjectTowardsCursorOnReload : MonoBehaviour
	{
		// Token: 0x06000480 RID: 1152 RVA: 0x00016FF0 File Offset: 0x000151F0
		private void OnReload()
		{
			Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
			Vector2 vector = this.player.transform.position;
			Vector2 b = (a - vector).normalized * this.distanceFromPlayerToSpawn;
			Vector2 v = vector + b;
			GameObject pooledObject = this.OP.GetPooledObject(this.prefab.name);
			pooledObject.transform.position = v;
			pooledObject.SetActive(true);
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00017094 File Offset: 0x00015294
		private void Start()
		{
			this.SC = ShootingCursor.Instance;
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.prefab.name, this.prefab, 100, true);
			this.player = base.GetComponentInParent<PlayerController>();
			this.ammo = this.player.ammo;
			this.ammo.OnReload.AddListener(new UnityAction(this.OnReload));
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001710F File Offset: 0x0001530F
		private void OnDestroy()
		{
			this.ammo.OnReload.RemoveListener(new UnityAction(this.OnReload));
		}

		// Token: 0x04000278 RID: 632
		[SerializeField]
		private GameObject prefab;

		// Token: 0x04000279 RID: 633
		[SerializeField]
		private float distanceFromPlayerToSpawn;

		// Token: 0x0400027A RID: 634
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x0400027B RID: 635
		private ShootingCursor SC;

		// Token: 0x0400027C RID: 636
		private ObjectPooler OP;

		// Token: 0x0400027D RID: 637
		private PlayerController player;

		// Token: 0x0400027E RID: 638
		private Ammo ammo;
	}
}
