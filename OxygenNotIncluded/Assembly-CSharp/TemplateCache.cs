using System;
using System.Collections.Generic;
using Klei;
using ProcGen;

// Token: 0x02000749 RID: 1865
public static class TemplateCache
{
	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06003384 RID: 13188 RVA: 0x00112D84 File Offset: 0x00110F84
	// (set) Token: 0x06003385 RID: 13189 RVA: 0x00112D8B File Offset: 0x00110F8B
	public static bool Initted { get; private set; }

	// Token: 0x06003386 RID: 13190 RVA: 0x00112D93 File Offset: 0x00110F93
	public static void Init()
	{
		if (TemplateCache.Initted)
		{
			return;
		}
		TemplateCache.templates = new Dictionary<string, TemplateContainer>();
		TemplateCache.Initted = true;
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x00112DAD File Offset: 0x00110FAD
	public static void Clear()
	{
		TemplateCache.templates = null;
		TemplateCache.Initted = false;
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x00112DBC File Offset: 0x00110FBC
	public static string RewriteTemplatePath(string scopePath)
	{
		string dlcId;
		string str;
		SettingsCache.GetDlcIdAndPath(scopePath, out dlcId, out str);
		return SettingsCache.GetAbsoluteContentPath(dlcId, "templates/" + str);
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x00112DE4 File Offset: 0x00110FE4
	public static string RewriteTemplateYaml(string scopePath)
	{
		return TemplateCache.RewriteTemplatePath(scopePath) + ".yaml";
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x00112DF8 File Offset: 0x00110FF8
	public static TemplateContainer GetTemplate(string templatePath)
	{
		if (!TemplateCache.templates.ContainsKey(templatePath))
		{
			TemplateCache.templates.Add(templatePath, null);
		}
		if (TemplateCache.templates[templatePath] == null)
		{
			string text = TemplateCache.RewriteTemplateYaml(templatePath);
			TemplateContainer templateContainer = YamlIO.LoadFile<TemplateContainer>(text, null, null);
			if (templateContainer == null)
			{
				Debug.LogWarning("Missing template [" + text + "]");
			}
			templateContainer.name = templatePath;
			TemplateCache.templates[templatePath] = templateContainer;
		}
		return TemplateCache.templates[templatePath];
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x00112E71 File Offset: 0x00111071
	public static bool TemplateExists(string templatePath)
	{
		return FileSystem.FileExists(TemplateCache.RewriteTemplateYaml(templatePath));
	}

	// Token: 0x04001EF9 RID: 7929
	private const string defaultAssetFolder = "bases";

	// Token: 0x04001EFA RID: 7930
	private static Dictionary<string, TemplateContainer> templates;
}
