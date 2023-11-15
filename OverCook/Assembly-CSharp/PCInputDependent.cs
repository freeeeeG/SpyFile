using System;
using UnityEngine;

// Token: 0x02000155 RID: 341
internal class PCInputDependent : MonoBehaviour
{
	// Token: 0x060005FD RID: 1533 RVA: 0x0002BB4C File Offset: 0x00029F4C
	private static bool IsKeyboard(EngagementSlot slot)
	{
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		GamepadUser user = playerManager.GetUser(slot);
		return user != null && user.ControlType == GamepadUser.ControlTypeEnum.Keyboard;
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0002BB80 File Offset: 0x00029F80
	private void Start()
	{
		GameObject[] array = (!PCInputDependent.IsKeyboard(EngagementSlot.One)) ? this.m_keyboard : this.m_pad;
		if (array != null && array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}
	}

	// Token: 0x04000505 RID: 1285
	public GameObject[] m_keyboard;

	// Token: 0x04000506 RID: 1286
	public GameObject[] m_pad;
}
