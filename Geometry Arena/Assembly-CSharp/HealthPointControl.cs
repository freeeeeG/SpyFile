using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A2 RID: 162
public class HealthPointControl : MonoBehaviour
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x0600059C RID: 1436 RVA: 0x00020344 File Offset: 0x0001E544
	private float Width
	{
		get
		{
			return this.prefabHpUnit.GetComponent<HealthPointUnit>().imageHp.gameObject.GetComponent<RectTransform>().sizeDelta.x;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x0600059D RID: 1437 RVA: 0x0002036A File Offset: 0x0001E56A
	private float Height
	{
		get
		{
			return this.prefabHpUnit.GetComponent<HealthPointUnit>().imageHp.gameObject.GetComponent<RectTransform>().sizeDelta.y;
		}
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00020390 File Offset: 0x0001E590
	private void Update()
	{
		this.UpdateEnergy();
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00020398 File Offset: 0x0001E598
	public void UpdateHpUnits()
	{
		if (this.listHpUnits == null)
		{
			this.listHpUnits = new List<GameObject>();
		}
		if (Player.inst == null || Player.inst.unit == null)
		{
			return;
		}
		int num = (int)Player.inst.unit.life;
		int lifeMax = Player.inst.LifeMax;
		float num2 = this.Width + this.distX;
		float num3 = this.Height + this.distY;
		float num4 = (float)lifeMax;
		float num5 = (float)num / (float)lifeMax;
		if (Setting.Inst.Option_SimpleHP)
		{
			num4 = 1f;
			this.textHp.gameObject.SetActive(true);
			this.textHp.text = num.ToString();
		}
		else
		{
			this.textHp.gameObject.SetActive(false);
		}
		int num6 = 0;
		while ((float)num6 < num4)
		{
			int count = this.listHpUnits.Count;
			GameObject gameObject;
			if (num6 >= count)
			{
				gameObject = Object.Instantiate<GameObject>(this.prefabHpUnit, this.rectHpUnits);
				this.listHpUnits.Add(gameObject);
			}
			else
			{
				gameObject = this.listHpUnits[num6];
			}
			Vector2 v = this.startPos + new Vector2(num2 * (float)(num6 % this.columNum), -num3 * (float)(num6 / this.columNum));
			gameObject.transform.localPosition = v;
			HealthPointUnit component = gameObject.GetComponent<HealthPointUnit>();
			if (Setting.Inst.Option_SimpleHP)
			{
				float scale = num5;
				component.Active(false, scale);
				component.SetWidth(300f);
			}
			else
			{
				if (num6 < num)
				{
					component.Active(false, 1f);
				}
				else
				{
					component.Deactice(false, 1f);
				}
				component.SetWidth(30f);
			}
			num6++;
		}
		int num7 = this.listHpUnits.Count - 1;
		while ((float)num7 >= num4)
		{
			Object obj = this.listHpUnits[num7];
			this.listHpUnits.RemoveAt(num7);
			Object.Destroy(obj);
			num7--;
		}
		this.UpdateShieldsUnits();
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x000205A0 File Offset: 0x0001E7A0
	public void UpdateShieldsUnits()
	{
		int shield = Player.inst.shield;
		int maxShields = Player.inst.Get_MaxShields();
		float num = this.Width + this.distX;
		float num2 = this.Height + this.distY;
		for (int i = 0; i < maxShields; i++)
		{
			int count = this.listShieldUnits.Count;
			GameObject gameObject;
			if (i >= count)
			{
				gameObject = Object.Instantiate<GameObject>(this.prefabHpUnit, this.rectShieldUnits);
				this.listShieldUnits.Add(gameObject);
			}
			else
			{
				gameObject = this.listShieldUnits[i];
			}
			Vector2 v = this.startPos + new Vector2(num * (float)(i % this.columNum), -num2 * (float)(i / this.columNum));
			gameObject.transform.localPosition = v;
			HealthPointUnit component = gameObject.GetComponent<HealthPointUnit>();
			if (i < shield)
			{
				component.Active(true, 1f);
			}
			else
			{
				component.Deactice(true, 1f);
			}
		}
		for (int j = this.listShieldUnits.Count - 1; j >= maxShields; j--)
		{
			Object obj = this.listShieldUnits[j];
			this.listShieldUnits.RemoveAt(j);
			Object.Destroy(obj);
		}
		int num3 = (Player.inst.LifeMax - 1) / this.columNum + 1;
		if (Setting.Inst.Option_SimpleHP)
		{
			num3 = 1;
		}
		this.rectShieldUnits.localPosition = new Vector2(0f, -num2 * (float)num3);
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x00020724 File Offset: 0x0001E924
	private void UpdateEnergy()
	{
		Player player = Player.inst;
		float y = player.energy / player.energyMax;
		this.text_Energy.text = player.energy.RoundToInt().ToString();
		if (TempData.inst.jobId != 0)
		{
			this.obj_Energy.SetActive(false);
			return;
		}
		this.obj_Energy.SetActive(true);
		this.rect_Energy.localScale = new Vector2(1f, y);
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x000207A3 File Offset: 0x0001E9A3
	private void Awake()
	{
		HealthPointControl.inst = this;
	}

	// Token: 0x04000489 RID: 1161
	public static HealthPointControl inst;

	// Token: 0x0400048A RID: 1162
	[SerializeField]
	private GameObject prefabHpUnit;

	// Token: 0x0400048B RID: 1163
	[SerializeField]
	private float distX = 10f;

	// Token: 0x0400048C RID: 1164
	[SerializeField]
	private float distY = 10f;

	// Token: 0x0400048D RID: 1165
	[SerializeField]
	private int columNum = 5;

	// Token: 0x0400048E RID: 1166
	[SerializeField]
	private Vector2 startPos = Vector2.zero;

	// Token: 0x0400048F RID: 1167
	[Header("Life")]
	[SerializeField]
	private List<GameObject> listHpUnits = new List<GameObject>();

	// Token: 0x04000490 RID: 1168
	[SerializeField]
	private RectTransform rectHpUnits;

	// Token: 0x04000491 RID: 1169
	[SerializeField]
	private Text textHp;

	// Token: 0x04000492 RID: 1170
	[Header("Shiled")]
	[SerializeField]
	private List<GameObject> listShieldUnits = new List<GameObject>();

	// Token: 0x04000493 RID: 1171
	[SerializeField]
	private RectTransform rectShieldUnits;

	// Token: 0x04000494 RID: 1172
	[Header("Energy")]
	[SerializeField]
	private RectTransform rect_Energy;

	// Token: 0x04000495 RID: 1173
	[SerializeField]
	private GameObject obj_Energy;

	// Token: 0x04000496 RID: 1174
	[SerializeField]
	private Text text_Energy;
}
