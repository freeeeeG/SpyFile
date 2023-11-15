using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DE RID: 222
public class EnemyInfoDisplayText : MonoBehaviour
{
	// Token: 0x060007B5 RID: 1973 RVA: 0x0002AD0D File Offset: 0x00028F0D
	public void InitFromEnemy(Enemy enemy)
	{
		this.enemy = enemy;
		this.UpdateText();
		this.UpdateTransform();
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x0002AD22 File Offset: 0x00028F22
	private void LateUpdate()
	{
		if (this.enemy == null || this.enemy.gameObject == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.UpdateTransform();
		this.UpdateText();
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x0002AD60 File Offset: 0x00028F60
	private void UpdateText()
	{
		double num = this.enemy.unit.GetDouble_LifePct();
		if (double.IsInfinity(this.enemy.unit.life))
		{
			num = 1.0;
		}
		string text = num.ToString("0%");
		this.textInfo.text = text;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x0002ADB8 File Offset: 0x00028FB8
	private void UpdateTransform()
	{
		base.transform.position = Camera.main.WorldToScreenPoint(this.enemy.transform.position);
		int fontSize = Mathf.Max(this.minFontSize, this.basicFontSize * Mathf.Sqrt(this.enemy.transform.localScale.x)).RoundToInt();
		this.textInfo.fontSize = fontSize;
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Start()
	{
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Update()
	{
	}

	// Token: 0x04000681 RID: 1665
	[SerializeField]
	private Text textInfo;

	// Token: 0x04000682 RID: 1666
	[SerializeField]
	private Enemy enemy;

	// Token: 0x04000683 RID: 1667
	[SerializeField]
	private float basicFontSize = 9f;

	// Token: 0x04000684 RID: 1668
	[SerializeField]
	private float minFontSize = 6f;
}
