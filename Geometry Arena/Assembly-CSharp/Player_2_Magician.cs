using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class Player_2_Magician : Player
{
	// Token: 0x0600037C RID: 892 RVA: 0x00015E74 File Offset: 0x00014074
	protected override void Awake()
	{
		base.Awake();
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			this.unit.gun_Objs = this.magicGuns;
			this.magicGuns[1].gameObject.SetActive(true);
			this.magicGuns[2].gameObject.SetActive(true);
			return;
		}
		this.unit.gun_Objs = new GunObj[]
		{
			this.magicGuns[0]
		};
		this.magicGuns[1].gameObject.SetActive(false);
		this.magicGuns[2].gameObject.SetActive(false);
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00015F10 File Offset: 0x00014110
	protected override void DetectInput_Skill()
	{
		if (!base.IfMouseNotOnButton())
		{
			return;
		}
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		if (this.skillLevel <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = this.theSkill.facs;
		this.gunDistMax = facs[0];
		this.gunDistMoveSpd = facs[1];
		if (TempData.inst.GetBool_SkillModuleOpenFlag(1))
		{
			this.gunDist = this.gunDistMin + this.unit.lastScale;
		}
		else
		{
			if (MyInput.KeyShootHold())
			{
				this.gunDist -= this.gunDistMoveSpd * Time.deltaTime;
			}
			if (MyInput.KeySkillHold())
			{
				this.gunDist += this.gunDistMoveSpd * Time.deltaTime;
			}
		}
		this.gunDist = Mathf.Clamp(this.gunDist, this.gunDistMin + this.unit.lastScale, this.gunDistMax + this.unit.lastScale);
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00016004 File Offset: 0x00014204
	protected override void SkillInFixedUpdate()
	{
		this.canAimAtMouse = false;
		this.skill_CanShoot = false;
		this.unit.Shoot_WantoShootOnce(MyTool.MouseToPlayerVec2());
		Player_2_Magician.gunAngle += this.gunRotateSpd * Time.fixedDeltaTime;
		if (Player_2_Magician.gunAngle > 360f)
		{
			Player_2_Magician.gunAngle -= 360f;
		}
		if (Player_2_Magician.gunAngle < -360f)
		{
			Player_2_Magician.gunAngle += 360f;
		}
		this.UpdateGun();
	}

	// Token: 0x0600037F RID: 895 RVA: 0x00016088 File Offset: 0x00014288
	private void UpdateGun()
	{
		for (int i = 0; i < 3; i++)
		{
			Vector2 vector = MyTool.AngleToVec2(Player_2_Magician.gunAngle + (float)(i * 120));
			this.magicGuns[i].transform.position = base.transform.position + vector * this.gunDist;
			Vector2 direction = vector.Rotate(90f);
			this.magicGuns[i].transform.rotation = Quaternion.Euler(0f, 0f, MyTool.Vec2toAngle180(direction));
		}
	}

	// Token: 0x0400030F RID: 783
	public static float gunAngle;

	// Token: 0x04000310 RID: 784
	public float gunDist = 1f;

	// Token: 0x04000311 RID: 785
	public float gunDistMin = 1f;

	// Token: 0x04000312 RID: 786
	public float gunDistMax = 5f;

	// Token: 0x04000313 RID: 787
	public float gunDistMoveSpd = 9f;

	// Token: 0x04000314 RID: 788
	[SerializeField]
	private float gunRotateSpd = 90f;

	// Token: 0x04000315 RID: 789
	[SerializeField]
	private GunObj[] magicGuns = new GunObj[0];
}
