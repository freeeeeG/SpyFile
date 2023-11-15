using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000EB RID: 235
	public class SmiteOnHit : MonoBehaviour
	{
		// Token: 0x060006DD RID: 1757 RVA: 0x0001E7E8 File Offset: 0x0001C9E8
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.smiteFXPrefab.name, this.smiteFXPrefab, 50, true);
			this.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001E840 File Offset: 0x0001CA40
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001E864 File Offset: 0x0001CA64
		private void OnImpact(object sender, object args)
		{
			if (Random.Range(0f, 1f) < this.chanceToHit)
			{
				GameObject gameObject = args as GameObject;
				if (gameObject.tag.Contains("Enemy"))
				{
					base.StartCoroutine(this.SmiteCR(gameObject));
				}
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001E8AF File Offset: 0x0001CAAF
		private IEnumerator SmiteCR(GameObject enemy)
		{
			yield return new WaitForSeconds(0.1f);
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			GameObject pooledObject = this.OP.GetPooledObject(this.smiteFXPrefab.name);
			pooledObject.transform.position = enemy.transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0f);
			pooledObject.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-45f, 45f));
			pooledObject.transform.position = enemy.transform.position;
			pooledObject.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			enemy.GetComponent<Health>().TakeDamage(DamageType.Smite, this.baseDamage, 1f);
			yield break;
		}

		// Token: 0x040004A8 RID: 1192
		[SerializeField]
		private GameObject smiteFXPrefab;

		// Token: 0x040004A9 RID: 1193
		[Range(0f, 1f)]
		[SerializeField]
		private float chanceToHit;

		// Token: 0x040004AA RID: 1194
		[SerializeField]
		private int baseDamage;

		// Token: 0x040004AB RID: 1195
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x040004AC RID: 1196
		private ObjectPooler OP;
	}
}
