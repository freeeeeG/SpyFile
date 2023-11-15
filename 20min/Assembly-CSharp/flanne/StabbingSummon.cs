using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200012D RID: 301
	public class StabbingSummon : AttackingSummon
	{
		// Token: 0x0600081C RID: 2076 RVA: 0x000226EC File Offset: 0x000208EC
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains("Enemy"))
			{
				Health component = other.gameObject.GetComponent<Health>();
				if (component == null)
				{
					return;
				}
				int damage = Mathf.FloorToInt(base.summonDamageMod.Modify((float)this.baseDamage));
				damage = base.ApplyDamageMods(damage);
				component.TakeDamage(DamageType.Summon, damage, 1f);
				this.PostNotification(Summon.SummonOnHitNotification, other.gameObject);
			}
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00022764 File Offset: 0x00020964
		protected override bool Attack()
		{
			if (this._stabbingCoroutine != null)
			{
				return false;
			}
			Vector2 vector = base.transform.position;
			if (EnemyFinder.GetRandomEnemy(vector, new Vector2(9f, 6f)) == null)
			{
				return false;
			}
			Vector2 vector2 = EnemyFinder.GetRandomEnemy(vector, new Vector2(9f, 6f)).transform.position - vector;
			if (vector2.magnitude > this.range)
			{
				return false;
			}
			Vector2 destination = vector2.normalized * this.range + vector;
			this._stabbingCoroutine = this.StabCR(destination);
			base.StartCoroutine(this._stabbingCoroutine);
			return true;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0002281B File Offset: 0x00020A1B
		private IEnumerator StabCR(Vector2 destination)
		{
			Vector2 b = base.transform.position;
			Vector2 vector = destination - b;
			Quaternion originalRotation = base.transform.localRotation;
			float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			base.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			Transform parent = base.transform.parent;
			base.transform.SetParent(null);
			int tweenID = LeanTween.move(base.gameObject, destination, base.finalAttackCooldown / 2f).id;
			while (LeanTween.isTweening(tweenID))
			{
				yield return null;
			}
			base.transform.SetParent(parent);
			tweenID = LeanTween.moveLocal(base.gameObject, Vector3.zero, base.finalAttackCooldown / 2f).id;
			while (LeanTween.isTweening(tweenID))
			{
				yield return null;
			}
			base.transform.localRotation = originalRotation;
			this._stabbingCoroutine = null;
			yield break;
		}

		// Token: 0x040005F2 RID: 1522
		[SerializeField]
		private int baseDamage = 50;

		// Token: 0x040005F3 RID: 1523
		[SerializeField]
		private float range;

		// Token: 0x040005F4 RID: 1524
		private IEnumerator _stabbingCoroutine;
	}
}
