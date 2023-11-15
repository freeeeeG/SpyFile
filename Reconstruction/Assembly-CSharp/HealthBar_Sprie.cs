using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class HealthBar_Sprie : ReusableObject
{
	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000601 RID: 1537 RVA: 0x00010591 File Offset: 0x0000E791
	// (set) Token: 0x06000602 RID: 1538 RVA: 0x00010599 File Offset: 0x0000E799
	public float FillAmount
	{
		get
		{
			return this.fillAmount;
		}
		set
		{
			this.fillAmount = value;
			this.healthImg.localScale = new Vector3(this.fillAmount, 1f, 1f);
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000603 RID: 1539 RVA: 0x000105C2 File Offset: 0x0000E7C2
	// (set) Token: 0x06000604 RID: 1540 RVA: 0x000105CA File Offset: 0x0000E7CA
	public float FrostAmount
	{
		get
		{
			return this.frostAmount;
		}
		set
		{
			this.frostAmount = value;
			this.frostImg.localScale = new Vector3(this.frostAmount, 1f, 1f);
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000605 RID: 1541 RVA: 0x000105F3 File Offset: 0x0000E7F3
	// (set) Token: 0x06000606 RID: 1542 RVA: 0x000105FC File Offset: 0x0000E7FC
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

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000607 RID: 1543 RVA: 0x0001063E File Offset: 0x0000E83E
	// (set) Token: 0x06000608 RID: 1544 RVA: 0x00010648 File Offset: 0x0000E848
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

	// Token: 0x06000609 RID: 1545 RVA: 0x0001068A File Offset: 0x0000E88A
	private void Start()
	{
		Singleton<GameEvents>.Instance.onShowDamageIntensify += this.ShowDamageIntensify;
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x000106A2 File Offset: 0x0000E8A2
	private void OnDestroy()
	{
		Singleton<GameEvents>.Instance.onShowDamageIntensify -= this.ShowDamageIntensify;
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x000106BA File Offset: 0x0000E8BA
	private void ShowDamageIntensify(bool value)
	{
		this.DmgIntensifyTxt.gameObject.SetActive(value);
		this.FrostIntensifyTxt.gameObject.SetActive(value);
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x000106DE File Offset: 0x0000E8DE
	private void OnEnable()
	{
		this.ShowDamageIntensify(StaticData.ShowIntensify);
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x000106EB File Offset: 0x0000E8EB
	public override void OnUnSpawn()
	{
		this.FillAmount = 1f;
		this.FrostAmount = 0f;
		this.BossTextAnim.Play("BossDialogue_Default");
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00010713 File Offset: 0x0000E913
	public void ShowIcon(int id, bool value)
	{
		this.Icons[id].SetActive(value);
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x00010724 File Offset: 0x0000E924
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

	// Token: 0x06000610 RID: 1552 RVA: 0x00010772 File Offset: 0x0000E972
	private IEnumerator BossTextCor(string text)
	{
		yield return new WaitForSeconds(0.5f);
		this.BossDialogueTxt.text = text;
		this.BossTextAnim.SetBool("Show", true);
		yield return new WaitForSeconds(3f);
		this.BossTextAnim.SetBool("Show", false);
		yield break;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x00010788 File Offset: 0x0000E988
	private void LateUpdate()
	{
		base.transform.position = base.transform.parent.position + this.enemyOffset;
		base.transform.rotation = Quaternion.identity;
	}

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	private TextMeshPro DmgIntensifyTxt;

	// Token: 0x040002A5 RID: 677
	[SerializeField]
	private TextMeshPro FrostIntensifyTxt;

	// Token: 0x040002A6 RID: 678
	[SerializeField]
	private Vector2 enemyOffset;

	// Token: 0x040002A7 RID: 679
	[SerializeField]
	private GameObject[] Icons;

	// Token: 0x040002A8 RID: 680
	[SerializeField]
	private TextMeshPro BossDialogueTxt;

	// Token: 0x040002A9 RID: 681
	[SerializeField]
	private Animator BossTextAnim;

	// Token: 0x040002AA RID: 682
	[SerializeField]
	private Transform healthImg;

	// Token: 0x040002AB RID: 683
	[SerializeField]
	private Transform frostImg;

	// Token: 0x040002AC RID: 684
	public Transform followTrans;

	// Token: 0x040002AD RID: 685
	private float fillAmount;

	// Token: 0x040002AE RID: 686
	private float frostAmount;

	// Token: 0x040002AF RID: 687
	private float damageIntensify;

	// Token: 0x040002B0 RID: 688
	private float frostIntensify;
}
