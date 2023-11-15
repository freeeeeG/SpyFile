using System;
using System.IO;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000A51 RID: 2641
[AddComponentMenu("KMonoBehaviour/scripts/BaseNaming")]
public class BaseNaming : KMonoBehaviour
{
	// Token: 0x06004F89 RID: 20361 RVA: 0x001C15B0 File Offset: 0x001BF7B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.GenerateBaseName();
		this.shuffleBaseNameButton.onClick += this.GenerateBaseName;
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnEditing));
		this.minionSelectScreen = base.GetComponent<MinionSelectScreen>();
	}

	// Token: 0x06004F8A RID: 20362 RVA: 0x001C1624 File Offset: 0x001BF824
	private bool CheckBaseName(string newName)
	{
		if (string.IsNullOrEmpty(newName))
		{
			return true;
		}
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (this.minionSelectScreen != null)
		{
			bool flag = false;
			try
			{
				bool flag2 = Directory.Exists(Path.Combine(savePrefixAndCreateFolder, newName));
				bool flag3 = cloudSavePrefix != null && Directory.Exists(Path.Combine(cloudSavePrefix, newName));
				flag = (flag2 || flag3);
			}
			catch (Exception arg)
			{
				flag = true;
				global::Debug.Log(string.Format("Base Naming / Warning / {0}", arg));
			}
			if (flag)
			{
				this.minionSelectScreen.SetProceedButtonActive(false, string.Format(UI.IMMIGRANTSCREEN.DUPLICATE_COLONY_NAME, newName));
				return false;
			}
			this.minionSelectScreen.SetProceedButtonActive(true, null);
		}
		return true;
	}

	// Token: 0x06004F8B RID: 20363 RVA: 0x001C16D4 File Offset: 0x001BF8D4
	private void OnEditing(string newName)
	{
		Util.ScrubInputField(this.inputField, false, false);
		this.CheckBaseName(this.inputField.text);
	}

	// Token: 0x06004F8C RID: 20364 RVA: 0x001C16F8 File Offset: 0x001BF8F8
	private void OnEndEdit(string newName)
	{
		if (Localization.HasDirtyWords(newName))
		{
			this.inputField.text = this.GenerateBaseNameString();
			newName = this.inputField.text;
		}
		if (string.IsNullOrEmpty(newName))
		{
			return;
		}
		if (newName.EndsWith(" "))
		{
			newName = newName.TrimEnd(new char[]
			{
				' '
			});
		}
		if (!this.CheckBaseName(newName))
		{
			return;
		}
		this.inputField.text = newName;
		SaveGame.Instance.SetBaseName(newName);
		string path = Path.ChangeExtension(newName, ".sav");
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		string path2 = savePrefixAndCreateFolder;
		if (SaveLoader.GetCloudSavesAvailable() && Game.Instance.SaveToCloudActive && cloudSavePrefix != null)
		{
			path2 = cloudSavePrefix;
		}
		SaveLoader.SetActiveSaveFilePath(Path.Combine(path2, newName, path));
	}

	// Token: 0x06004F8D RID: 20365 RVA: 0x001C17B4 File Offset: 0x001BF9B4
	private void GenerateBaseName()
	{
		string text = this.GenerateBaseNameString();
		((LocText)this.inputField.placeholder).text = text;
		this.inputField.text = text;
		this.OnEndEdit(text);
	}

	// Token: 0x06004F8E RID: 20366 RVA: 0x001C17F4 File Offset: 0x001BF9F4
	private string GenerateBaseNameString()
	{
		string fullString = LocString.GetStrings(typeof(NAMEGEN.COLONY.FORMATS)).GetRandom<string>();
		fullString = this.ReplaceStringWithRandom(fullString, "{noun}", LocString.GetStrings(typeof(NAMEGEN.COLONY.NOUN)));
		string[] strings = LocString.GetStrings(typeof(NAMEGEN.COLONY.ADJECTIVE));
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective}", strings);
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective2}", strings);
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective3}", strings);
		return this.ReplaceStringWithRandom(fullString, "{adjective4}", strings);
	}

	// Token: 0x06004F8F RID: 20367 RVA: 0x001C187B File Offset: 0x001BFA7B
	private string ReplaceStringWithRandom(string fullString, string replacementKey, string[] replacementValues)
	{
		if (!fullString.Contains(replacementKey))
		{
			return fullString;
		}
		return fullString.Replace(replacementKey, replacementValues.GetRandom<string>());
	}

	// Token: 0x04003408 RID: 13320
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04003409 RID: 13321
	[SerializeField]
	private KButton shuffleBaseNameButton;

	// Token: 0x0400340A RID: 13322
	private MinionSelectScreen minionSelectScreen;
}
