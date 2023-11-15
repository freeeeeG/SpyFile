using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000127 RID: 295
	public class SetHitboxActiveSummon : AttackingSummon
	{
		// Token: 0x06000804 RID: 2052 RVA: 0x00021F68 File Offset: 0x00020168
		private void OnHit(object sender, object args)
		{
			GameObject e = args as GameObject;
			this.PostNotification(Summon.SummonOnHitNotification, e);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00021F88 File Offset: 0x00020188
		protected override void Init()
		{
			this.AddObserver(new Action<object, object>(this.OnHit), HarmfulOnContact.HitNotification, this.hitbox);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00021FA7 File Offset: 0x000201A7
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnHit), HarmfulOnContact.HitNotification, this.hitbox);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00021FC8 File Offset: 0x000201C8
		protected override bool Attack()
		{
			Vector2 vector = base.transform.position;
			GameObject randomEnemy = EnemyFinder.GetRandomEnemy(vector, new Vector2(this.range, this.range));
			if (randomEnemy != null)
			{
				this.hitbox.GetComponent<HarmfulOnContact>().damageAmount = Mathf.FloorToInt(base.summonDamageMod.Modify((float)this.baseDamage));
				Vector2 vector2 = randomEnemy.transform.position - vector;
				float angle = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
				this.hitbox.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				this.hitbox.gameObject.SetActive(true);
				return true;
			}
			return false;
		}

		// Token: 0x040005D4 RID: 1492
		public int baseDamage;

		// Token: 0x040005D5 RID: 1493
		[SerializeField]
		private HarmfulOnContact hitbox;

		// Token: 0x040005D6 RID: 1494
		[SerializeField]
		private float range;
	}
}
