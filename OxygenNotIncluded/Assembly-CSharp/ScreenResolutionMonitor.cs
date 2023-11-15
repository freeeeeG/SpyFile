using System;
using UnityEngine;

// Token: 0x02000C9E RID: 3230
public class ScreenResolutionMonitor : MonoBehaviour
{
	// Token: 0x060066D4 RID: 26324 RVA: 0x00265C2C File Offset: 0x00263E2C
	private void Awake()
	{
		this.previousSize = new Vector2((float)Screen.width, (float)Screen.height);
	}

	// Token: 0x060066D5 RID: 26325 RVA: 0x00265C48 File Offset: 0x00263E48
	private void Update()
	{
		if ((this.previousSize.x != (float)Screen.width || this.previousSize.y != (float)Screen.height) && Game.Instance != null)
		{
			Game.Instance.Trigger(445618876, null);
			this.previousSize.x = (float)Screen.width;
			this.previousSize.y = (float)Screen.height;
		}
		this.UpdateShouldUseGamepadUIMode();
	}

	// Token: 0x060066D6 RID: 26326 RVA: 0x00265CC0 File Offset: 0x00263EC0
	public static bool UsingGamepadUIMode()
	{
		return ScreenResolutionMonitor.previousGamepadUIMode;
	}

	// Token: 0x060066D7 RID: 26327 RVA: 0x00265CC8 File Offset: 0x00263EC8
	private void UpdateShouldUseGamepadUIMode()
	{
		bool flag = (Screen.dpi > 130f && Screen.height < 900) || KInputManager.currentControllerIsGamepad;
		if (flag != ScreenResolutionMonitor.previousGamepadUIMode)
		{
			ScreenResolutionMonitor.previousGamepadUIMode = flag;
			if (Game.Instance == null)
			{
				return;
			}
			Game.Instance.Trigger(-442024484, null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound(flag ? "ControllerType_ToggleOn" : "ControllerType_ToggleOff", false));
		}
	}

	// Token: 0x04004721 RID: 18209
	[SerializeField]
	private Vector2 previousSize;

	// Token: 0x04004722 RID: 18210
	private static bool previousGamepadUIMode;

	// Token: 0x04004723 RID: 18211
	private const float HIGH_DPI = 130f;
}
