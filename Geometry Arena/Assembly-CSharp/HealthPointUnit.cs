using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A3 RID: 163
public class HealthPointUnit : MonoBehaviour
{
	// Token: 0x060005A4 RID: 1444 RVA: 0x000207FD File Offset: 0x0001E9FD
	private void Awake()
	{
		if (this.anm == null)
		{
			this.anm = base.GetComponentInChildren<Animator>();
		}
		if (this.imageHp == null)
		{
			Debug.LogError("HpObjEmpty!");
			return;
		}
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x00020834 File Offset: 0x0001EA34
	private void UpdateColor(bool ifShield, float scale)
	{
		Color color = DataBase.Inst.Data_VarColors[TempData.inst.varColorId].ColorRGB;
		ColorSet colorSet_UI = ResourceLibrary.Inst.colorSet_UI;
		color = color.ApplyColorSet(colorSet_UI);
		if (!ifShield)
		{
			this.imageHp.color = color;
			this.imageBack.color = color.SetSaturation(color.Saturation() * this.satFac).SetAlpha(0.3f);
		}
		else
		{
			this.imageHp.color = Color.white;
			this.imageBack.color = this.color_ShieldOff;
		}
		this.imageHp.gameObject.SetActive(this.ifOn);
		this.imageBack.gameObject.SetActive(true);
		this.imageHp.transform.localScale = new Vector2(scale, 1f);
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00020910 File Offset: 0x0001EB10
	private void HpOn(bool ifShield, float scale)
	{
		if (!this.ifOn)
		{
			this.HpAnim();
		}
		else if (this.lastScale != scale)
		{
			this.HpAnim();
		}
		this.ifOn = true;
		this.lastScale = scale;
		this.UpdateColor(ifShield, scale);
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00020947 File Offset: 0x0001EB47
	private void HpOff(bool ifShield, float scale)
	{
		if (this.ifOn)
		{
			this.HpAnim();
		}
		this.ifOn = false;
		this.UpdateColor(ifShield, scale);
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x00020966 File Offset: 0x0001EB66
	private void HpAnim()
	{
		if (Setting.Inst.Option_SimpleHP)
		{
			return;
		}
		this.anm.SetTrigger("ExpandOnce");
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00020985 File Offset: 0x0001EB85
	public void Active(bool ifShield = false, float scale = 1f)
	{
		this.HpOn(ifShield, scale);
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x0002098F File Offset: 0x0001EB8F
	public void Deactice(bool ifShield = false, float scale = 1f)
	{
		this.HpOff(ifShield, scale);
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00020999 File Offset: 0x0001EB99
	public void SetWidth(float width)
	{
		this.imageBack.rectTransform.sizeDelta = new Vector2(width, 40f);
		this.imageHp.rectTransform.sizeDelta = new Vector2(width, 40f);
	}

	// Token: 0x04000497 RID: 1175
	[SerializeField]
	private bool ifOn;

	// Token: 0x04000498 RID: 1176
	[SerializeField]
	private Color color_ShieldOff = Color.white;

	// Token: 0x04000499 RID: 1177
	[SerializeField]
	public Image imageHp;

	// Token: 0x0400049A RID: 1178
	[SerializeField]
	public Image imageBack;

	// Token: 0x0400049B RID: 1179
	[SerializeField]
	public Animator anm;

	// Token: 0x0400049C RID: 1180
	[SerializeField]
	public float satFac = 0.2f;

	// Token: 0x0400049D RID: 1181
	[SerializeField]
	private float lastScale;
}
