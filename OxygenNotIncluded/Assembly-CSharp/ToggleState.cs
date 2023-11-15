using System;
using UnityEngine;

// Token: 0x02000C9B RID: 3227
[Serializable]
public struct ToggleState
{
	// Token: 0x0400470A RID: 18186
	public string Name;

	// Token: 0x0400470B RID: 18187
	public string on_click_override_sound_path;

	// Token: 0x0400470C RID: 18188
	public string on_release_override_sound_path;

	// Token: 0x0400470D RID: 18189
	public string sound_parameter_name;

	// Token: 0x0400470E RID: 18190
	public float sound_parameter_value;

	// Token: 0x0400470F RID: 18191
	public bool has_sound_parameter;

	// Token: 0x04004710 RID: 18192
	public Sprite sprite;

	// Token: 0x04004711 RID: 18193
	public Color color;

	// Token: 0x04004712 RID: 18194
	public Color color_on_hover;

	// Token: 0x04004713 RID: 18195
	public bool use_color_on_hover;

	// Token: 0x04004714 RID: 18196
	public bool use_rect_margins;

	// Token: 0x04004715 RID: 18197
	public Vector2 rect_margins;

	// Token: 0x04004716 RID: 18198
	public StatePresentationSetting[] additional_display_settings;
}
