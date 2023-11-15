using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
public static class MyInput
{
	// Token: 0x06000437 RID: 1079 RVA: 0x0001A671 File Offset: 0x00018871
	public static bool KeySkillDown()
	{
		return (!(BattleMapCanvas.inst != null) || !BattleMapCanvas.inst.IfAnyWindowActive()) && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space));
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x0001A6A2 File Offset: 0x000188A2
	public static bool KeySkillUp()
	{
		return Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.Space);
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0001A6B8 File Offset: 0x000188B8
	public static bool KeySkillHold()
	{
		return (!(BattleMapCanvas.inst != null) || !BattleMapCanvas.inst.IfAnyWindowActive()) && (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space));
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x0001A6E9 File Offset: 0x000188E9
	public static bool KeyShootDown()
	{
		return Input.GetMouseButtonDown(0);
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x0001A6F6 File Offset: 0x000188F6
	public static bool KeyShootUp()
	{
		return Input.GetMouseButtonUp(0);
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x0001A704 File Offset: 0x00018904
	public static bool KeyShootHold()
	{
		bool flag = false;
		if (Icon_AutoFire.inst != null)
		{
			flag = Icon_AutoFire.inst.open;
		}
		return Input.GetMouseButton(0) || flag;
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x0001A738 File Offset: 0x00018938
	public static bool KeyShiftHold()
	{
		return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x0001A752 File Offset: 0x00018952
	public static bool KeyCtrlHold()
	{
		return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x0001A76C File Offset: 0x0001896C
	public static bool GetKeyDownWithSound(KeyCode keyCode)
	{
		bool keyDown = Input.GetKeyDown(keyCode);
		if (keyDown)
		{
			SoundEffects.Inst.ui_ButtonClick.PlayRandom();
		}
		return keyDown;
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x0001A786 File Offset: 0x00018986
	public static bool GetKeyDownWithSound(bool flag)
	{
		if (flag)
		{
			SoundEffects.Inst.ui_ButtonClick.PlayRandom();
		}
		return flag;
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0001A79B File Offset: 0x0001899B
	public static bool IfGetCloseButtonDown()
	{
		bool flag = MyInput.GetKeyDownWithSound(KeyCode.Escape) || MyInput.GetKeyDownWithSound(Input.GetMouseButtonDown(1));
		if (flag)
		{
			UI_ToolTip.inst.TryClose();
		}
		return flag;
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0001A738 File Offset: 0x00018938
	public static bool GetKeyHold_Special_SwordAll()
	{
		return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
	}
}
