using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200007A RID: 122
	public class DamageBoostInRange : MonoBehaviour
	{
		// Token: 0x06000509 RID: 1289 RVA: 0x000190FC File Offset: 0x000172FC
		private void OnTweakDamage(object sender, object args)
		{
			(args as List<ValueModifier>).Add(new MultValueModifier(0, 1f + this.damageBoost));
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001911B File Offset: 0x0001731B
		private void Start()
		{
			this._targetsInRange = new List<Health>();
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00019128 File Offset: 0x00017328
		private void OnDestroy()
		{
			foreach (Health sender in this._targetsInRange)
			{
				this.RemoveObserver(new Action<object, object>(this.OnTweakDamage), Health.TweakDamageEvent, sender);
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001918C File Offset: 0x0001738C
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains(this.hitTag))
			{
				Health component = other.gameObject.GetComponent<Health>();
				if (component != null)
				{
					this.AddObserver(new Action<object, object>(this.OnTweakDamage), Health.TweakDamageEvent, component);
					this._targetsInRange.Add(component);
				}
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000191EC File Offset: 0x000173EC
		private void OnCollisionExit2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains(this.hitTag))
			{
				Health component = other.gameObject.GetComponent<Health>();
				if (component != null)
				{
					this.RemoveObserver(new Action<object, object>(this.OnTweakDamage), Health.TweakDamageEvent, component);
					this._targetsInRange.Remove(component);
				}
			}
		}

		// Token: 0x040002F7 RID: 759
		[NonSerialized]
		public float damageBoost;

		// Token: 0x040002F8 RID: 760
		[SerializeField]
		private string hitTag;

		// Token: 0x040002F9 RID: 761
		private List<Health> _targetsInRange;
	}
}
