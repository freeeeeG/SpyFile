using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DF RID: 223
public class FloatDamageText : MonoBehaviour
{
	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060007BC RID: 1980 RVA: 0x0002AE46 File Offset: 0x00029046
	public float LifePercent
	{
		get
		{
			return 1f - this.lifeTimeLeft / this.lifeTimeMax;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060007BD RID: 1981 RVA: 0x0002AE5B File Offset: 0x0002905B
	public float xToY
	{
		get
		{
			return Mathf.Min(1f, Mathf.Sqrt(1f - this.LifePercent * this.LifePercent));
		}
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x0002AE7F File Offset: 0x0002907F
	private void Awake()
	{
		base.gameObject.name = "FloatDamageText";
		this.rectTransform = base.gameObject.GetComponent<RectTransform>();
		this.damage = 0.0;
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x0002AEB4 File Offset: 0x000290B4
	private void Update()
	{
		float num = 1f;
		float num2 = FPSDetector.inst.fps;
		if (num2 < 60f)
		{
			num2 = Mathf.Max(3f, num2);
			num = 60f / num2;
		}
		this.lifeTimeLeft -= Time.deltaTime * num;
		if (this.lifeTimeLeft <= 0f)
		{
			this.Die();
			return;
		}
		base.transform.Translate(this.direction * this.basicVelocity * this.xToY * Time.deltaTime);
		this.UpdateSize();
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x0002AF52 File Offset: 0x00029152
	private void Die()
	{
		ObjectPool.inst.DamageFloatText_GoPool(base.gameObject);
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0002AF64 File Offset: 0x00029164
	public void UpdateSize()
	{
		base.gameObject.transform.localScale = new Vector3(this.xToY, this.xToY, 1f);
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0002AF8C File Offset: 0x0002918C
	public void Init(double damage, bool ifCrit, bool ifPlayerHurt, bool ifHeal)
	{
		this.lifeTimeLeft = this.lifeTimeMax;
		this.direction = UnityEngine.Random.insideUnitCircle;
		this.damage = damage;
		string text = math.round(damage).ToSmartString_Int();
		if (ifHeal)
		{
			text = "+" + text;
		}
		this.textNum.text = text;
		if (ifCrit)
		{
			this.textNum.fontSize = this.critSize;
			if (ifHeal)
			{
				this.textNum.color = Color.green;
			}
			else
			{
				this.textNum.color = Player.inst.unit.mainColor.SetValue(1f).SetSaturation(0.54f);
			}
		}
		else
		{
			this.textNum.fontSize = this.normalSize;
			if (ifHeal)
			{
				this.textNum.color = Color.green;
			}
			else
			{
				this.textNum.color = Player.inst.unit.mainColor;
			}
		}
		if (ifPlayerHurt)
		{
			this.textNum.color = Color.red;
		}
	}

	// Token: 0x04000685 RID: 1669
	private RectTransform rectTransform;

	// Token: 0x04000686 RID: 1670
	[SerializeField]
	private BasicUnit ownerUnit;

	// Token: 0x04000687 RID: 1671
	[SerializeField]
	private double damage;

	// Token: 0x04000688 RID: 1672
	[SerializeField]
	private Text textNum;

	// Token: 0x04000689 RID: 1673
	[Header("Setting")]
	public float lifeTimeMax = 5f;

	// Token: 0x0400068A RID: 1674
	public float lifeTimeLeft = 2f;

	// Token: 0x0400068B RID: 1675
	public int normalSize = 30;

	// Token: 0x0400068C RID: 1676
	public Color normalColor = Color.white;

	// Token: 0x0400068D RID: 1677
	public int critSize = 40;

	// Token: 0x0400068E RID: 1678
	public Color critColor = Color.yellow;

	// Token: 0x0400068F RID: 1679
	[SerializeField]
	private float basicVelocity = 5f;

	// Token: 0x04000690 RID: 1680
	[SerializeField]
	private Vector2 direction = Vector2.one;
}
