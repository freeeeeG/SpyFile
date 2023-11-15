using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class FloatDamageTextControl : MonoBehaviour
{
	// Token: 0x060007C4 RID: 1988 RVA: 0x0002B0F5 File Offset: 0x000292F5
	private void Awake()
	{
		FloatDamageTextControl.inst = this;
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x0002B100 File Offset: 0x00029300
	public void NewFloatBattleText(Vector2 position, double damagePlus, bool ifCrit, bool ifPlayerHurt, bool ifHeal)
	{
		if (!Setting.Inst.Option_ShowDamageText)
		{
			return;
		}
		GameObject pool_FloatDamageText_GetOrNew = ObjectPool.inst.GetPool_FloatDamageText_GetOrNew(this.prefabFloatDamageText);
		pool_FloatDamageText_GetOrNew.transform.SetParent(base.transform);
		pool_FloatDamageText_GetOrNew.transform.localScale = Vector2.one;
		pool_FloatDamageText_GetOrNew.transform.position = Camera.main.WorldToScreenPoint(position);
		pool_FloatDamageText_GetOrNew.GetComponent<FloatDamageText>().Init(damagePlus, ifCrit, ifPlayerHurt, ifHeal);
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x0002B17C File Offset: 0x0002937C
	public EnemyInfoDisplayText NewEnemyInfo(Enemy enemy)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.prefabEnemyInfoText);
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localScale = Vector2.one;
		gameObject.transform.position = enemy.transform.position;
		EnemyInfoDisplayText component = gameObject.GetComponent<EnemyInfoDisplayText>();
		component.InitFromEnemy(enemy);
		return component;
	}

	// Token: 0x04000691 RID: 1681
	public static FloatDamageTextControl inst;

	// Token: 0x04000692 RID: 1682
	[SerializeField]
	private GameObject prefabFloatDamageText;

	// Token: 0x04000693 RID: 1683
	[SerializeField]
	private GameObject prefabEnemyInfoText;
}
