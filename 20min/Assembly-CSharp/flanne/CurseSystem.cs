using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000079 RID: 121
	public class CurseSystem : MonoBehaviour
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00018FA5 File Offset: 0x000171A5
		public int curseDamage
		{
			get
			{
				return Mathf.FloorToInt(this.myGun.damage * this.curseDamageMultiplier);
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00018FBE File Offset: 0x000171BE
		private void Awake()
		{
			if (CurseSystem.Instance != null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			CurseSystem.Instance = this;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00018FE0 File Offset: 0x000171E0
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.curseFXPrefab.name, this.curseFXPrefab, 100, true);
			this._cursedTargets = new List<Health>();
			this.myGun = PlayerController.Instance.gun;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00019034 File Offset: 0x00017234
		public bool IsCursed(GameObject target)
		{
			return this._cursedTargets.Find((Health bt) => bt.gameObject == target) != null;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001906C File Offset: 0x0001726C
		public void Curse(GameObject target)
		{
			Health component = target.GetComponent<Health>();
			if (component == null || this._cursedTargets.Contains(component))
			{
				return;
			}
			base.StartCoroutine(this.CurseCR(component));
			this.PostNotification(CurseSystem.InflictCurseEvent, target);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000190B2 File Offset: 0x000172B2
		private IEnumerator CurseCR(Health targetHealth)
		{
			this._cursedTargets.Add(targetHealth);
			GameObject curseObj = this.OP.GetPooledObject(this.curseFXPrefab.name);
			curseObj.transform.SetParent(targetHealth.transform);
			curseObj.transform.localPosition = Vector3.zero;
			curseObj.SetActive(true);
			yield return new WaitForSeconds(this.duration);
			this._cursedTargets.Remove(targetHealth);
			if (targetHealth.gameObject.activeSelf)
			{
				targetHealth.TakeDamage(DamageType.Curse, this.curseDamage, 1f);
			}
			curseObj.SetActive(false);
			curseObj.transform.SetParent(this.OP.transform);
			curseObj.SetActive(false);
			yield break;
		}

		// Token: 0x040002EE RID: 750
		public static CurseSystem Instance;

		// Token: 0x040002EF RID: 751
		public static string InflictCurseEvent = "CurseSystem.InflictCurseEvent";

		// Token: 0x040002F0 RID: 752
		public static string CurseKillNotification = "CurseSystem.CurseKillNotification";

		// Token: 0x040002F1 RID: 753
		public float duration = 0.7f;

		// Token: 0x040002F2 RID: 754
		public float curseDamageMultiplier = 2f;

		// Token: 0x040002F3 RID: 755
		[SerializeField]
		private GameObject curseFXPrefab;

		// Token: 0x040002F4 RID: 756
		private ObjectPooler OP;

		// Token: 0x040002F5 RID: 757
		private Gun myGun;

		// Token: 0x040002F6 RID: 758
		private List<Health> _cursedTargets;
	}
}
