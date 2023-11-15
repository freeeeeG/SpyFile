using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000132 RID: 306
	public class ThunderGenerator : MonoBehaviour
	{
		// Token: 0x0600082F RID: 2095 RVA: 0x000229F8 File Offset: 0x00020BF8
		private void Awake()
		{
			ThunderGenerator.SharedInstance = this;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00022A00 File Offset: 0x00020C00
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.thunderPrefab.name, this.thunderPrefab, 100, true);
			this.OP.AddObject(this.thunderImpactPrefab.name, this.thunderImpactPrefab, 100, true);
			this.sizeMultiplier = 1f;
			this.damageMod = new StatMod();
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00022A6C File Offset: 0x00020C6C
		public void GenerateAt(GameObject target, int damage)
		{
			this.GenerateAt(target.transform.position, damage);
			this.PostNotification(ThunderGenerator.ThunderHitEvent, target);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00022A8C File Offset: 0x00020C8C
		public void GenerateAt(Vector3 position, int damage)
		{
			Vector3 position2 = position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0f);
			GameObject pooledObject = this.OP.GetPooledObject(this.thunderPrefab.name);
			pooledObject.transform.position = position2;
			pooledObject.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-45f, 45f));
			pooledObject.SetActive(true);
			GameObject pooledObject2 = this.OP.GetPooledObject(this.thunderImpactPrefab.name);
			pooledObject2.transform.localScale = Vector3.one * this.sizeMultiplier;
			pooledObject2.transform.position = position2;
			pooledObject2.SetActive(true);
			Collider2D[] array = Physics2D.OverlapCircleAll(position, this.baseAoE * this.sizeMultiplier, 1 << TagLayerUtil.Enemy);
			for (int i = 0; i < array.Length; i++)
			{
				Health component = array[i].GetComponent<Health>();
				if (component != null)
				{
					component.TakeDamage(DamageType.Thunder, Mathf.FloorToInt(this.damageMod.Modify((float)damage)), 1f);
				}
			}
			pooledObject.GetComponent<SpriteRenderer>().flipX = (Random.Range(0, 1) == 0);
			this.soundFX.Play(null);
		}

		// Token: 0x04000605 RID: 1541
		public static ThunderGenerator SharedInstance;

		// Token: 0x04000606 RID: 1542
		public static string ThunderHitEvent = "ThunderGenerator.ThunderHitEvent";

		// Token: 0x04000607 RID: 1543
		[SerializeField]
		private GameObject thunderPrefab;

		// Token: 0x04000608 RID: 1544
		[SerializeField]
		private GameObject thunderImpactPrefab;

		// Token: 0x04000609 RID: 1545
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x0400060A RID: 1546
		[SerializeField]
		private float baseAoE = 0.35f;

		// Token: 0x0400060B RID: 1547
		public StatMod damageMod;

		// Token: 0x0400060C RID: 1548
		public float sizeMultiplier;

		// Token: 0x0400060D RID: 1549
		private ObjectPooler OP;
	}
}
