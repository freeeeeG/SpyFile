using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000061 RID: 97
	public class BurnSystem : MonoBehaviour
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00016BBC File Offset: 0x00014DBC
		private float burnDuration
		{
			get
			{
				return this.burnDurationMultiplier.Modify(this.baseBurnDuration) + 0.1f;
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00016BD5 File Offset: 0x00014DD5
		private void Awake()
		{
			BurnSystem.SharedInstance = this;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00016BE0 File Offset: 0x00014DE0
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.burnFXOPTag, this.burnFXPrefab, 100, true);
			this.burnDamageMultiplier = new StatMod();
			this.burnDurationMultiplier = new StatMod();
			this._currentTargets = new List<BurnSystem.BurnTarget>();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00016C34 File Offset: 0x00014E34
		public bool IsBurning(GameObject target)
		{
			return this._currentTargets.Find((BurnSystem.BurnTarget bt) => bt.target == target) != null;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00016C68 File Offset: 0x00014E68
		public void Burn(GameObject target, int burnDamage)
		{
			if (target.tag == "Player")
			{
				return;
			}
			BurnSystem.BurnTarget burnTarget = this._currentTargets.Find((BurnSystem.BurnTarget bt) => bt.target == target);
			if (burnTarget == null)
			{
				Health component = target.GetComponent<Health>();
				if (component != null)
				{
					base.StartCoroutine(this.StartBurnCR(component, burnDamage));
				}
				else
				{
					Debug.LogWarning("No health attached to burn target: " + target);
				}
			}
			else
			{
				base.StartCoroutine(this.AddBurnCR(burnTarget, burnDamage, this.burnDuration));
			}
			this.PostNotification(BurnSystem.InflictBurnEvent, target);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00016D18 File Offset: 0x00014F18
		private IEnumerator StartBurnCR(Health targetHealth, int burnDamage)
		{
			BurnSystem.BurnTarget burnTarget = new BurnSystem.BurnTarget(targetHealth.gameObject, 0);
			base.StartCoroutine(this.AddBurnCR(burnTarget, burnDamage, this.burnDuration));
			this._currentTargets.Add(burnTarget);
			GameObject burnObj = this.OP.GetPooledObject(this.burnFXOPTag);
			burnObj.transform.SetParent(targetHealth.transform);
			burnObj.transform.localPosition = Vector3.zero;
			burnObj.SetActive(true);
			yield return null;
			while (targetHealth.gameObject.activeInHierarchy && burnTarget.damage > 0)
			{
				yield return new WaitForSeconds(1f);
				targetHealth.TakeDamage(DamageType.Burn, Mathf.FloorToInt(this.burnDamageMultiplier.Modify((float)burnTarget.damage)), 1f);
			}
			burnObj.transform.SetParent(this.OP.transform);
			burnObj.SetActive(false);
			this._currentTargets.Remove(burnTarget);
			yield break;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00016D35 File Offset: 0x00014F35
		private IEnumerator AddBurnCR(BurnSystem.BurnTarget burnTarget, int burnDamage, float duration)
		{
			burnTarget.damage += burnDamage;
			yield return new WaitForSeconds(duration);
			burnTarget.damage -= burnDamage;
			yield break;
		}

		// Token: 0x04000257 RID: 599
		public static BurnSystem SharedInstance;

		// Token: 0x04000258 RID: 600
		public static string InflictBurnEvent = "BurnSystem.InflictBurnEvent";

		// Token: 0x04000259 RID: 601
		[SerializeField]
		private GameObject burnFXPrefab;

		// Token: 0x0400025A RID: 602
		[SerializeField]
		private string burnFXOPTag;

		// Token: 0x0400025B RID: 603
		public StatMod burnDamageMultiplier;

		// Token: 0x0400025C RID: 604
		public StatMod burnDurationMultiplier;

		// Token: 0x0400025D RID: 605
		private float baseBurnDuration = 4f;

		// Token: 0x0400025E RID: 606
		private ObjectPooler OP;

		// Token: 0x0400025F RID: 607
		private List<BurnSystem.BurnTarget> _currentTargets;

		// Token: 0x02000296 RID: 662
		private class BurnTarget
		{
			// Token: 0x06000DD7 RID: 3543 RVA: 0x00032BAD File Offset: 0x00030DAD
			public BurnTarget(GameObject t, int d)
			{
				this.target = t;
				this.damage = d;
			}

			// Token: 0x04000A3E RID: 2622
			public GameObject target;

			// Token: 0x04000A3F RID: 2623
			public int damage;
		}
	}
}
