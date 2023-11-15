using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000EC RID: 236
	public class SmitePassive : MonoBehaviour
	{
		// Token: 0x060006E2 RID: 1762 RVA: 0x0001E8C5 File Offset: 0x0001CAC5
		private void OnAmmoChanged(int ammoAmount)
		{
			if (ammoAmount == 0)
			{
				base.StartCoroutine(this.SmiteCR());
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001E8D8 File Offset: 0x0001CAD8
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.smiteFXPrefab.name, this.smiteFXPrefab, 50, true);
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.ammo = componentInParent.ammo;
			this.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001E93E File Offset: 0x0001CB3E
		private void OnDestroy()
		{
			this.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001E95C File Offset: 0x0001CB5C
		private IEnumerator SmiteCR()
		{
			yield return new WaitForSeconds(0.1f);
			Vector2 point = base.transform.position;
			Collider2D[] enemies = Physics2D.OverlapCircleAll(point, this.range, 1 << TagLayerUtil.Enemy);
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			foreach (Collider2D collider2D in enemies)
			{
				GameObject pooledObject = this.OP.GetPooledObject(this.smiteFXPrefab.name);
				pooledObject.transform.position = collider2D.transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0f);
				pooledObject.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-45f, 45f));
				pooledObject.transform.position = collider2D.transform.position;
				pooledObject.SetActive(true);
			}
			yield return new WaitForSeconds(0.1f);
			Collider2D[] array = enemies;
			for (int i = 0; i < array.Length; i++)
			{
				Health component = array[i].GetComponent<Health>();
				int num = this.baseDamage.NotifyModifiers(SmitePassive.SmiteTweakDamageNotification, this);
				component.HPChange(-1 * num);
				this.PostNotification(SmitePassive.SmiteKillNotification);
			}
			yield break;
		}

		// Token: 0x040004AD RID: 1197
		public static string SmiteTweakDamageNotification = "SmitePassive.SmiteTweakDamageNotification";

		// Token: 0x040004AE RID: 1198
		public static string SmiteKillNotification = "SmitePassive.SmiteKillNotification";

		// Token: 0x040004AF RID: 1199
		[SerializeField]
		private GameObject smiteFXPrefab;

		// Token: 0x040004B0 RID: 1200
		[SerializeField]
		private float range;

		// Token: 0x040004B1 RID: 1201
		[SerializeField]
		private int baseDamage;

		// Token: 0x040004B2 RID: 1202
		[SerializeField]
		private int damageBonusPerHP;

		// Token: 0x040004B3 RID: 1203
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x040004B4 RID: 1204
		private ObjectPooler OP;

		// Token: 0x040004B5 RID: 1205
		private Ammo ammo;
	}
}
