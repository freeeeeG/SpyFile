using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EE RID: 238
public class HealthBar : ReusableObject
{
	// Token: 0x1700027E RID: 638
	// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001036D File Offset: 0x0000E56D
	// (set) Token: 0x060005F0 RID: 1520 RVA: 0x00010375 File Offset: 0x0000E575
	public float FillAmount
	{
		get
		{
			return this.fillAmount;
		}
		set
		{
			this.fillAmount = value;
			this.healthProgress.fillAmount = value;
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0001038A File Offset: 0x0000E58A
	// (set) Token: 0x060005F2 RID: 1522 RVA: 0x00010392 File Offset: 0x0000E592
	public float FrostAmount
	{
		get
		{
			return this.frostAmount;
		}
		set
		{
			this.frostAmount = value;
			this.frostProgess.fillAmount = value;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x060005F3 RID: 1523 RVA: 0x000103A7 File Offset: 0x0000E5A7
	// (set) Token: 0x060005F4 RID: 1524 RVA: 0x000103B0 File Offset: 0x0000E5B0
	public float DamageIntensify
	{
		get
		{
			return this.damageIntensify;
		}
		set
		{
			this.damageIntensify = value;
			this.DmgIntensifyTxt.text = Mathf.RoundToInt(this.damageIntensify * 100f).ToString() + "%";
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x060005F5 RID: 1525 RVA: 0x000103F2 File Offset: 0x0000E5F2
	// (set) Token: 0x060005F6 RID: 1526 RVA: 0x000103FC File Offset: 0x0000E5FC
	public float FrostIntensify
	{
		get
		{
			return this.frostIntensify;
		}
		set
		{
			this.frostIntensify = value;
			this.FrostIntensifyTxt.text = Mathf.RoundToInt(this.frostIntensify * 100f).ToString() + "%";
		}
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0001043E File Offset: 0x0000E63E
	private void Start()
	{
		Singleton<GameEvents>.Instance.onShowDamageIntensify += this.ShowDamageIntensify;
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x00010456 File Offset: 0x0000E656
	private void OnDestroy()
	{
		Singleton<GameEvents>.Instance.onShowDamageIntensify -= this.ShowDamageIntensify;
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0001046E File Offset: 0x0000E66E
	private void ShowDamageIntensify(bool value)
	{
		this.DmgIntensifyTxt.gameObject.SetActive(value);
		this.FrostIntensifyTxt.gameObject.SetActive(value);
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x00010492 File Offset: 0x0000E692
	private void OnEnable()
	{
		this.ShowDamageIntensify(StaticData.ShowIntensify);
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0001049F File Offset: 0x0000E69F
	public override void OnUnSpawn()
	{
		this.FillAmount = 1f;
		this.FrostAmount = 0f;
		this.BossTextAnim.Play("BossDialogue_Default");
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x000104C7 File Offset: 0x0000E6C7
	public void ShowIcon(int id, bool value)
	{
		this.Icons[id].SetActive(value);
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x000104D8 File Offset: 0x0000E6D8
	public void ShowBossTxt(EnemyAttribute att, float chance)
	{
		if (Random.value > chance)
		{
			return;
		}
		if (att.BossDialogues.Length != 0)
		{
			string traduction = GameMultiLang.GetTraduction(att.BossDialogues[Random.Range(0, att.BossDialogues.Length)]);
			base.StartCoroutine(this.BossTextCor(traduction));
		}
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x00010526 File Offset: 0x0000E726
	private IEnumerator BossTextCor(string text)
	{
		yield return new WaitForSeconds(0.5f);
		this.BossDialogueTxt.text = text;
		this.BossTextAnim.SetBool("Show", true);
		yield return new WaitForSeconds(3f);
		this.BossTextAnim.SetBool("Show", false);
		yield break;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0001053C File Offset: 0x0000E73C
	private void LateUpdate()
	{
		base.transform.position = base.transform.parent.position + this.enemyOffset;
		base.transform.rotation = Quaternion.identity;
	}

	// Token: 0x04000297 RID: 663
	[SerializeField]
	private Image healthProgress;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	private Image frostProgess;

	// Token: 0x04000299 RID: 665
	[SerializeField]
	private Vector2 enemyOffset;

	// Token: 0x0400029A RID: 666
	[SerializeField]
	private GameObject[] Icons;

	// Token: 0x0400029B RID: 667
	[SerializeField]
	private Text DmgIntensifyTxt;

	// Token: 0x0400029C RID: 668
	[SerializeField]
	private Text FrostIntensifyTxt;

	// Token: 0x0400029D RID: 669
	[SerializeField]
	private TextMeshProUGUI BossDialogueTxt;

	// Token: 0x0400029E RID: 670
	[SerializeField]
	private Animator BossTextAnim;

	// Token: 0x0400029F RID: 671
	public Transform followTrans;

	// Token: 0x040002A0 RID: 672
	private float fillAmount;

	// Token: 0x040002A1 RID: 673
	private float frostAmount;

	// Token: 0x040002A2 RID: 674
	private float damageIntensify;

	// Token: 0x040002A3 RID: 675
	private float frostIntensify;
}
