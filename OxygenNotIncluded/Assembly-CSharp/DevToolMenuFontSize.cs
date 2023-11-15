using System;
using ImGuiNET;

// Token: 0x0200055B RID: 1371
public class DevToolMenuFontSize
{
	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06002164 RID: 8548 RVA: 0x000B37A9 File Offset: 0x000B19A9
	// (set) Token: 0x06002163 RID: 8547 RVA: 0x000B37A0 File Offset: 0x000B19A0
	public bool initialized { get; private set; }

	// Token: 0x06002165 RID: 8549 RVA: 0x000B37B4 File Offset: 0x000B19B4
	public void RefreshFontSize()
	{
		DevToolMenuFontSize.FontSizeCategory @int = (DevToolMenuFontSize.FontSizeCategory)KPlayerPrefs.GetInt("Imgui_font_size_category", 2);
		this.SetFontSizeCategory(@int);
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x000B37D4 File Offset: 0x000B19D4
	public void InitializeIfNeeded()
	{
		if (!this.initialized)
		{
			this.initialized = true;
			this.RefreshFontSize();
		}
	}

	// Token: 0x06002167 RID: 8551 RVA: 0x000B37EC File Offset: 0x000B19EC
	public void DrawMenu()
	{
		if (ImGui.BeginMenu("Settings"))
		{
			bool flag = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Fabric;
			bool flag2 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Small;
			bool flag3 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Regular;
			bool flag4 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Large;
			if (ImGui.BeginMenu("Size"))
			{
				if (ImGui.Checkbox("Original Font", ref flag) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Fabric)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Fabric);
				}
				if (ImGui.Checkbox("Small Text", ref flag2) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Small)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Small);
				}
				if (ImGui.Checkbox("Regular Text", ref flag3) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Regular)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Regular);
				}
				if (ImGui.Checkbox("Large Text", ref flag4) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Large)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Large);
				}
				ImGui.EndMenu();
			}
			ImGui.EndMenu();
		}
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x000B38C0 File Offset: 0x000B1AC0
	public unsafe void SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory size)
	{
		this.fontSizeCategory = size;
		KPlayerPrefs.SetInt("Imgui_font_size_category", (int)size);
		ImGuiIOPtr io = ImGui.GetIO();
		if (size < (DevToolMenuFontSize.FontSizeCategory)io.Fonts.Fonts.Size)
		{
			ImFontPtr wrappedPtr = *io.Fonts.Fonts[(int)size];
			io.NativePtr->FontDefault = wrappedPtr;
		}
	}

	// Token: 0x040012CE RID: 4814
	public const string SETTINGS_KEY_FONT_SIZE_CATEGORY = "Imgui_font_size_category";

	// Token: 0x040012CF RID: 4815
	private DevToolMenuFontSize.FontSizeCategory fontSizeCategory;

	// Token: 0x020011F3 RID: 4595
	public enum FontSizeCategory
	{
		// Token: 0x04005E09 RID: 24073
		Fabric,
		// Token: 0x04005E0A RID: 24074
		Small,
		// Token: 0x04005E0B RID: 24075
		Regular,
		// Token: 0x04005E0C RID: 24076
		Large
	}
}
