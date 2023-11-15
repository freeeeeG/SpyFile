using System;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public class ExplosionTrap : TrapContent
{
	// Token: 0x06000C6E RID: 3182 RVA: 0x00020644 File Offset: 0x0001E844
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		base.OnContentPass(enemy, content, index);
		Vector2 vector = (content == null) ? base.transform.position : content.transform.position;
		float amount = (1f + base.TrapIntensify + (float)enemy.DamageStrategy.TrapIntensify) * 0.05f * enemy.DamageStrategy.CurrentHealth;
		Collider2D[] array = Physics2D.OverlapCircleAll(vector, this.sputteringRange, StaticData.EnemyLayerMask);
		for (int i = 0; i < array.Length; i++)
		{
			TargetPoint component = array[i].GetComponent<TargetPoint>();
			if (component)
			{
				float num;
				component.Enemy.DamageStrategy.ApplyDamage(amount, out num, null, true);
				this.DamageAnalysis += (long)((int)num);
			}
		}
		ParticalControl particalControl = Singleton<ObjectPool>.Instance.Spawn(this.explisionPrefab) as ParticalControl;
		particalControl.transform.position = vector;
		particalControl.transform.localScale = Mathf.Max(0.3f, this.sputteringRange * 2f) * Vector3.one;
		particalControl.PlayEffect();
		Singleton<Sound>.Instance.PlayEffect("Sound_ExplosionTrap");
		enemy.DamageStrategy.TrapIntensify = 0;
	}

	// Token: 0x04000631 RID: 1585
	[SerializeField]
	private ParticalControl explisionPrefab;

	// Token: 0x04000632 RID: 1586
	private float sputteringRange = 0.5f;
}
