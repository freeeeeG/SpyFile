using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class Skill_Player0_Shileds : MonoBehaviour
{
	// Token: 0x06000314 RID: 788 RVA: 0x0001301A File Offset: 0x0001121A
	private void Awake()
	{
		if (Skill_Player0_Shileds.inst != null)
		{
			Debug.LogWarning("Shiled!=null ! clear");
			Object.Destroy(Skill_Player0_Shileds.inst.gameObject);
		}
		Skill_Player0_Shileds.inst = this;
	}

	// Token: 0x06000315 RID: 789 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Start()
	{
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00013048 File Offset: 0x00011248
	public void InitShields()
	{
		float num = Player.inst.unit.lastScale;
		num = Mathf.Max(num, 1f);
		base.gameObject.transform.localScale = new Vector3(num, num, 1f);
		float skillLevel = (float)GameData.inst.jobs[0].skillLevel;
		this.objs_Shiled[0].SetActive(true);
		this.objs_Shiled[1].SetActive(true);
		this.objs_Shiled[2].SetActive(true);
		this.objs_Shiled[3].SetActive(true);
		float num2 = (skillLevel + (float)1) * 0.9f;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(0))
		{
			SkillModule skillModule_CurrentJobWithEffectID = SkillModule.GetSkillModule_CurrentJobWithEffectID(0);
			num2 *= skillModule_CurrentJobWithEffectID.facs[0];
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			SkillModule skillModule_CurrentJobWithEffectID2 = SkillModule.GetSkillModule_CurrentJobWithEffectID(2);
			num2 *= skillModule_CurrentJobWithEffectID2.facs[0];
		}
		this.tranSizer.localScale = Vector2.one * num2;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(3))
		{
			this.objSingleShield.SetActive(true);
			this.objFourShields.SetActive(false);
			return;
		}
		this.objSingleShield.SetActive(false);
		this.objFourShields.SetActive(true);
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00013178 File Offset: 0x00011378
	public void DyeSprsWithColor(Color cl)
	{
		SpriteRenderer[] array = new SpriteRenderer[this.objs_Shiled.Length];
		for (int i = 0; i < array.Length; i++)
		{
			SpriteRenderer componentInChildren = this.objs_Shiled[i].GetComponentInChildren<SpriteRenderer>();
			array[i] = componentInChildren;
			componentInChildren.color = cl;
		}
		this.bloom.InitMat(array, ResourceLibrary.Inst.GlowIntensity_Unit, false);
	}

	// Token: 0x06000318 RID: 792 RVA: 0x000131D1 File Offset: 0x000113D1
	public void Update()
	{
		this.FollowPlayer();
	}

	// Token: 0x06000319 RID: 793 RVA: 0x000131DC File Offset: 0x000113DC
	public void FollowPlayer()
	{
		if (Player.inst == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.transform.position = Player.inst.transform.position;
		base.transform.rotation = Player.inst.transform.rotation;
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00013238 File Offset: 0x00011438
	public void Close()
	{
		GameObject[] array = this.objs_Shiled;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<Animator>().SetTrigger("Close");
		}
	}

	// Token: 0x0600031B RID: 795 RVA: 0x000051D0 File Offset: 0x000033D0
	private void OnDestroy()
	{
	}

	// Token: 0x040002C9 RID: 713
	[SerializeField]
	public static Skill_Player0_Shileds inst;

	// Token: 0x040002CA RID: 714
	[SerializeField]
	private GameObject[] objs_Shiled;

	// Token: 0x040002CB RID: 715
	[SerializeField]
	private Transform tranSizer;

	// Token: 0x040002CC RID: 716
	[SerializeField]
	private GameObject objFourShields;

	// Token: 0x040002CD RID: 717
	[SerializeField]
	private GameObject objSingleShield;

	// Token: 0x040002CE RID: 718
	[SerializeField]
	private BloomMaterialControl bloom;
}
