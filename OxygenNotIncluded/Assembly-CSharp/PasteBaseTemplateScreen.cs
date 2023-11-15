using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02000BBB RID: 3003
public class PasteBaseTemplateScreen : KScreen
{
	// Token: 0x06005DE8 RID: 24040 RVA: 0x002260F6 File Offset: 0x002242F6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PasteBaseTemplateScreen.Instance = this;
		TemplateCache.Init();
		this.button_directory_up.onClick += this.UpDirectory;
		base.ConsumeMouseScroll = true;
		this.RefreshStampButtons();
	}

	// Token: 0x06005DE9 RID: 24041 RVA: 0x0022612D File Offset: 0x0022432D
	protected override void OnForcedCleanUp()
	{
		PasteBaseTemplateScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06005DEA RID: 24042 RVA: 0x0022613C File Offset: 0x0022433C
	[ContextMenu("Refresh")]
	public void RefreshStampButtons()
	{
		this.directory_path_text.text = this.m_CurrentDirectory;
		this.button_directory_up.isInteractable = (this.m_CurrentDirectory != PasteBaseTemplateScreen.NO_DIRECTORY);
		foreach (GameObject obj in this.m_template_buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.m_template_buttons.Clear();
		global::Debug.Log("Changing directory to " + this.m_CurrentDirectory);
		if (this.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
		{
			this.directory_path_text.text = "";
			using (List<string>.Enumerator enumerator2 = DlcManager.RELEASE_ORDER.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string dlcId = enumerator2.Current;
					if (DlcManager.IsContentActive(dlcId))
					{
						GameObject gameObject = global::Util.KInstantiateUI(this.prefab_directory_button, this.button_list_container, true);
						gameObject.GetComponent<KButton>().onClick += delegate()
						{
							this.UpdateDirectory(SettingsCache.GetScope(dlcId));
						};
						gameObject.GetComponentInChildren<LocText>().text = ((dlcId == "") ? UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.BASE_GAME_FOLDER_NAME.text : SettingsCache.GetScope(dlcId));
						this.m_template_buttons.Add(gameObject);
					}
				}
			}
			return;
		}
		string[] directories = Directory.GetDirectories(TemplateCache.RewriteTemplatePath(this.m_CurrentDirectory));
		for (int i = 0; i < directories.Length; i++)
		{
			string path = directories[i];
			string directory_name = System.IO.Path.GetFileNameWithoutExtension(path);
			GameObject gameObject2 = global::Util.KInstantiateUI(this.prefab_directory_button, this.button_list_container, true);
			gameObject2.GetComponent<KButton>().onClick += delegate()
			{
				this.UpdateDirectory(directory_name);
			};
			gameObject2.GetComponentInChildren<LocText>().text = directory_name;
			this.m_template_buttons.Add(gameObject2);
		}
		ListPool<FileHandle, PasteBaseTemplateScreen>.PooledList pooledList = ListPool<FileHandle, PasteBaseTemplateScreen>.Allocate();
		FileSystem.GetFiles(TemplateCache.RewriteTemplatePath(this.m_CurrentDirectory), "*.yaml", pooledList);
		foreach (FileHandle fileHandle in pooledList)
		{
			string file_path_no_extension = System.IO.Path.GetFileNameWithoutExtension(fileHandle.full_path);
			GameObject gameObject3 = global::Util.KInstantiateUI(this.prefab_paste_button, this.button_list_container, true);
			gameObject3.GetComponent<KButton>().onClick += delegate()
			{
				this.OnClickPasteButton(file_path_no_extension);
			};
			gameObject3.GetComponentInChildren<LocText>().text = file_path_no_extension;
			this.m_template_buttons.Add(gameObject3);
		}
	}

	// Token: 0x06005DEB RID: 24043 RVA: 0x00226428 File Offset: 0x00224628
	private void UpdateDirectory(string relativePath)
	{
		if (this.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
		{
			this.m_CurrentDirectory = "";
		}
		this.m_CurrentDirectory = FileSystem.CombineAndNormalize(new string[]
		{
			this.m_CurrentDirectory,
			relativePath
		});
		this.RefreshStampButtons();
	}

	// Token: 0x06005DEC RID: 24044 RVA: 0x00226478 File Offset: 0x00224678
	private void UpDirectory()
	{
		int num = this.m_CurrentDirectory.LastIndexOf("/");
		if (num > 0)
		{
			this.m_CurrentDirectory = this.m_CurrentDirectory.Substring(0, num);
		}
		else
		{
			string dlcId;
			string str;
			SettingsCache.GetDlcIdAndPath(this.m_CurrentDirectory, out dlcId, out str);
			if (str.IsNullOrWhiteSpace())
			{
				this.m_CurrentDirectory = PasteBaseTemplateScreen.NO_DIRECTORY;
			}
			else
			{
				this.m_CurrentDirectory = SettingsCache.GetScope(dlcId);
			}
		}
		this.RefreshStampButtons();
	}

	// Token: 0x06005DED RID: 24045 RVA: 0x002264E8 File Offset: 0x002246E8
	private void OnClickPasteButton(string template_name)
	{
		if (template_name == null)
		{
			return;
		}
		string text = FileSystem.CombineAndNormalize(new string[]
		{
			this.m_CurrentDirectory,
			template_name
		});
		DebugTool.Instance.DeactivateTool(null);
		DebugBaseTemplateButton.Instance.ClearSelection();
		DebugBaseTemplateButton.Instance.nameField.text = text;
		TemplateContainer template = TemplateCache.GetTemplate(text);
		StampTool.Instance.Activate(template, true, false);
	}

	// Token: 0x04003F45 RID: 16197
	public static PasteBaseTemplateScreen Instance;

	// Token: 0x04003F46 RID: 16198
	public GameObject button_list_container;

	// Token: 0x04003F47 RID: 16199
	public GameObject prefab_paste_button;

	// Token: 0x04003F48 RID: 16200
	public GameObject prefab_directory_button;

	// Token: 0x04003F49 RID: 16201
	public KButton button_directory_up;

	// Token: 0x04003F4A RID: 16202
	public LocText directory_path_text;

	// Token: 0x04003F4B RID: 16203
	private List<GameObject> m_template_buttons = new List<GameObject>();

	// Token: 0x04003F4C RID: 16204
	private static readonly string NO_DIRECTORY = "NONE";

	// Token: 0x04003F4D RID: 16205
	private string m_CurrentDirectory = PasteBaseTemplateScreen.NO_DIRECTORY;
}
