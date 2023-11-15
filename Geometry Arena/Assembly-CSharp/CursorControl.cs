using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DD RID: 221
public class CursorControl : MonoBehaviour
{
	// Token: 0x060007AD RID: 1965 RVA: 0x0002AA7C File Offset: 0x00028C7C
	private void Start()
	{
		Cursor.visible = false;
		this.FollowMouse();
		if (TempData.inst.currentSceneType == EnumSceneType.BATTLE)
		{
			this.objAimCursor.SetActive(true);
			this.objNormalCursor.SetActive(false);
			this.sprAimCursor.color = DataBase.Inst.Data_VarColors[TempData.inst.varColorId].ColorRGB.SetValue(this.battle_ColorValue).SetSaturation(this.battle_ColorSat);
			return;
		}
		this.objAimCursor.SetActive(false);
		this.objNormalCursor.SetActive(true);
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x0002AB0E File Offset: 0x00028D0E
	private void Update()
	{
		this.FollowMouse();
		this.UpdateInBattle();
		this.UpdateResolution();
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x0002AB22 File Offset: 0x00028D22
	private void FixedUpdate()
	{
		this.FollowMouse();
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x0002AB2C File Offset: 0x00028D2C
	private void UpdateEnergy()
	{
		Player inst = Player.inst;
		if (inst == null)
		{
			return;
		}
		float num = inst.energy / inst.energyMax;
		if (num == 1f)
		{
			this.obj_Energy.SetActive(false);
			return;
		}
		this.obj_Energy.SetActive(true);
		this.rect_Energy.localScale = new Vector2(1f, num);
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0002AB93 File Offset: 0x00028D93
	private void FollowMouse()
	{
		base.gameObject.transform.position = Input.mousePosition;
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x0002ABAC File Offset: 0x00028DAC
	private void UpdateInBattle()
	{
		if (TempData.inst.currentSceneType != EnumSceneType.BATTLE)
		{
			return;
		}
		if (MyInput.KeyShootHold())
		{
			float zAngle = this.battle_RotateHold * Time.deltaTime * Player.inst.unit.playerFactorTotal.fireSpd / 3f;
			this.sprAimCursor.transform.Rotate(0f, 0f, zAngle);
			this.battle_Animator.SetBool("Aiming", true);
		}
		else if (MyInput.KeySkillHold())
		{
			float zAngle2 = this.battle_RotateHoldSkillMulti * this.battle_RotateHold * Time.deltaTime * Player.inst.unit.playerFactorTotal.fireSpd / 3f;
			this.sprAimCursor.transform.Rotate(0f, 0f, zAngle2);
		}
		else
		{
			this.sprAimCursor.transform.Rotate(0f, 0f, this.battle_RotateNormal * Time.deltaTime);
			this.battle_Animator.SetBool("Aiming", false);
		}
		if (MyInput.KeySkillDown())
		{
			this.battle_Animator.SetTrigger("ExpandOnce");
		}
		this.UpdateEnergy();
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x000051D0 File Offset: 0x000033D0
	private void UpdateResolution()
	{
	}

	// Token: 0x04000676 RID: 1654
	[SerializeField]
	private GameObject objNormalCursor;

	// Token: 0x04000677 RID: 1655
	[SerializeField]
	private GameObject objAimCursor;

	// Token: 0x04000678 RID: 1656
	[SerializeField]
	private Image sprAimCursor;

	// Token: 0x04000679 RID: 1657
	[SerializeField]
	private float battle_RotateNormal = 5f;

	// Token: 0x0400067A RID: 1658
	[SerializeField]
	private float battle_RotateHold = 15f;

	// Token: 0x0400067B RID: 1659
	[SerializeField]
	private float battle_RotateHoldSkillMulti = 3f;

	// Token: 0x0400067C RID: 1660
	[SerializeField]
	private Animator battle_Animator;

	// Token: 0x0400067D RID: 1661
	[SerializeField]
	[Range(0f, 1f)]
	private float battle_ColorValue = 1f;

	// Token: 0x0400067E RID: 1662
	[SerializeField]
	[Range(0f, 1f)]
	private float battle_ColorSat = 1f;

	// Token: 0x0400067F RID: 1663
	[SerializeField]
	private RectTransform rect_Energy;

	// Token: 0x04000680 RID: 1664
	[SerializeField]
	private GameObject obj_Energy;
}
