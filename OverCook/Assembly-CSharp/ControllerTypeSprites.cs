using System;
using UnityEngine;

// Token: 0x02000A75 RID: 2677
[CreateAssetMenu(fileName = "ControllerTypeSprites", menuName = "Team17/ControllerTypeSprites")]
public class ControllerTypeSprites : ScriptableObject
{
	// Token: 0x06003501 RID: 13569 RVA: 0x000F7A70 File Offset: 0x000F5E70
	public Sprite GetImage(PadSide _side, GamepadUser.ControlTypeEnum _type)
	{
		ControllerTypeSprites.ControllerSprites controllerSpritesForPlatform = this.GetControllerSpritesForPlatform(_type);
		if (_side == PadSide.Both)
		{
			return controllerSpritesForPlatform.m_full;
		}
		if (_side == PadSide.Left)
		{
			return controllerSpritesForPlatform.m_left;
		}
		if (_side != PadSide.Right)
		{
			return null;
		}
		return controllerSpritesForPlatform.m_right;
	}

	// Token: 0x06003502 RID: 13570 RVA: 0x000F7AB4 File Offset: 0x000F5EB4
	public Sprite GetEngagementImage(PadSide _side, GamepadUser.ControlTypeEnum _type)
	{
		ControllerTypeSprites.ControllerSprites controllerSpritesForPlatform = this.GetControllerSpritesForPlatform(_type);
		if (_side == PadSide.Both)
		{
			return controllerSpritesForPlatform.m_fullEngagement;
		}
		if (_side == PadSide.Left)
		{
			return controllerSpritesForPlatform.m_leftEngagement;
		}
		if (_side != PadSide.Right)
		{
			return null;
		}
		return controllerSpritesForPlatform.m_rightEngagement;
	}

	// Token: 0x06003503 RID: 13571 RVA: 0x000F7AF8 File Offset: 0x000F5EF8
	protected ControllerTypeSprites.ControllerSprites GetControllerSpritesForPlatform(GamepadUser.ControlTypeEnum _type)
	{
		ControllerTypeSprites.ControllerSprites result = null;
		if (_type == GamepadUser.ControlTypeEnum.Keyboard)
		{
			result = this.m_keyboardSprites;
		}
		else
		{
			ControllerTypeSprites.PCPadVisuals padToUseOnPC = this.m_padToUseOnPC;
			if (padToUseOnPC != ControllerTypeSprites.PCPadVisuals.NX)
			{
				if (padToUseOnPC != ControllerTypeSprites.PCPadVisuals.PS4)
				{
					if (padToUseOnPC == ControllerTypeSprites.PCPadVisuals.X1)
					{
						result = this.m_padSpritesX1;
					}
				}
				else
				{
					result = this.m_padSpritesPS4;
				}
			}
			else
			{
				result = this.m_padSpritesNX;
			}
		}
		return result;
	}

	// Token: 0x04002A73 RID: 10867
	[Header("Pad Sprites")]
	[SerializeField]
	public ControllerTypeSprites.ControllerSprites m_padSpritesNX;

	// Token: 0x04002A74 RID: 10868
	[SerializeField]
	public ControllerTypeSprites.ControllerSprites m_padSpritesPS4;

	// Token: 0x04002A75 RID: 10869
	[SerializeField]
	public ControllerTypeSprites.ControllerSprites m_padSpritesX1;

	// Token: 0x04002A76 RID: 10870
	[Space]
	[Header("PC Pad")]
	[SerializeField]
	private ControllerTypeSprites.PCPadVisuals m_padToUseOnPC = ControllerTypeSprites.PCPadVisuals.X1;

	// Token: 0x04002A77 RID: 10871
	[Space]
	[Header("Keyboard Sprites")]
	[SerializeField]
	public ControllerTypeSprites.ControllerSprites m_keyboardSprites;

	// Token: 0x02000A76 RID: 2678
	[Serializable]
	public class ControllerSprites
	{
		// Token: 0x04002A78 RID: 10872
		[Header("Split-Left Controller Sprites")]
		[SerializeField]
		public Sprite m_left;

		// Token: 0x04002A79 RID: 10873
		[SerializeField]
		public Sprite m_leftEngagement;

		// Token: 0x04002A7A RID: 10874
		[Header("Full Controller Sprites")]
		[SerializeField]
		public Sprite m_full;

		// Token: 0x04002A7B RID: 10875
		[SerializeField]
		public Sprite m_fullEngagement;

		// Token: 0x04002A7C RID: 10876
		[Header("Split-Right Controller Sprites")]
		[SerializeField]
		public Sprite m_right;

		// Token: 0x04002A7D RID: 10877
		[SerializeField]
		public Sprite m_rightEngagement;
	}

	// Token: 0x02000A77 RID: 2679
	public enum PCPadVisuals
	{
		// Token: 0x04002A7F RID: 10879
		NX,
		// Token: 0x04002A80 RID: 10880
		PS4,
		// Token: 0x04002A81 RID: 10881
		X1
	}
}
