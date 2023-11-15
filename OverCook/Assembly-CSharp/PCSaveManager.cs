using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000731 RID: 1841
public class PCSaveManager : SaveManagerBase
{
	// Token: 0x0600231F RID: 8991 RVA: 0x000A9E65 File Offset: 0x000A8265
	protected void BootstrapAwake()
	{
		base.StartCoroutine(this.LoadProfile(null));
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x000A9E78 File Offset: 0x000A8278
	public override IEnumerator<SaveLoadResult?> LoadProfile(GamepadUser _user)
	{
		base.DestroyMetaSession();
		base.CreateMetaSession();
		MetaGameProgress metaGameProgess = base.GetMetaGameProgress();
		ReturnValue<bool> hasSave = new ReturnValue<bool>();
		string fileAddress = this.GetFileAddress(PCSaveManager.Type.Meta, -1, -1);
		if (!File.Exists(fileAddress))
		{
			yield return new SaveLoadResult?(SaveLoadResult.NotExist);
			yield break;
		}
		byte[] bytes = File.ReadAllBytes(fileAddress);
		if (!metaGameProgess.ByteLoad(bytes))
		{
			SaveLoadResult? dialogOutcome = null;
			SaveManagerBase.PlatformCallback callback = delegate(SaveLoadResult _result)
			{
				dialogOutcome = new SaveLoadResult?(_result);
			};
			base.ShowCorruptedMetaDialog(callback);
			while (dialogOutcome == null)
			{
				yield return null;
			}
			yield return dialogOutcome;
			yield break;
		}
		yield return new SaveLoadResult?(SaveLoadResult.Exists);
		yield break;
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x000A9E94 File Offset: 0x000A8294
	protected override IEnumerator<SaveLoadResult?> PlatformLoadSave(SaveMode _mode, int _slot, int _dlcNumber)
	{
		yield return new SaveLoadResult?(this.LoadData(_mode, _slot, _dlcNumber));
		yield break;
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x000A9EC4 File Offset: 0x000A82C4
	private SaveLoadResult LoadData(SaveMode _mode, int _slot, int _dlcNumber)
	{
		base.ShowLoadIcon(true);
		GameSession gameSession = GameUtils.GetGameSession();
		string fileAddress = this.GetFileAddress(this.ModeToType(_mode), _slot, _dlcNumber);
		if (!File.Exists(fileAddress))
		{
			base.HideLoadIcon();
			return SaveLoadResult.NotExist;
		}
		byte[] array = File.ReadAllBytes(fileAddress);
		if (array == null || !gameSession.Progress.Load(array))
		{
			base.HideLoadIcon();
			return SaveLoadResult.Corrupted;
		}
		base.HideLoadIcon();
		return SaveLoadResult.Exists;
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x000A9F34 File Offset: 0x000A8334
	protected override void PlatformSaveMetaProgress(SaveManagerBase.PlatformCallback _callback)
	{
		SaveLoadResult result = this.SaveFile(base.GetMetaGameProgress(), PCSaveManager.Type.Meta, -1, -1);
		_callback(result);
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x000A9F58 File Offset: 0x000A8358
	protected override void PlatformSaveData(SaveMode _mode, int _slot, int _dlcNumber, SaveManagerBase.PlatformCallback _callback)
	{
		SaveLoadResult result = SaveLoadResult.Exists;
		GameSession gameSession = GameUtils.GetGameSession();
		if (_mode == SaveMode.Main)
		{
			result = this.SaveFile(gameSession.Progress.SaveableData, this.ModeToType(_mode), _slot, _dlcNumber);
		}
		_callback(result);
	}

	// Token: 0x06002325 RID: 8997 RVA: 0x000A9F9C File Offset: 0x000A839C
	private SaveLoadResult SaveFile(IByteSerialization _data, PCSaveManager.Type _type, int _slot, int _dlcNumber)
	{
		base.ShowSaveIcon(true);
		if (_data != null)
		{
			FileStream fileStream = null;
			try
			{
				string fileAddress = this.GetFileAddress(_type, _slot, _dlcNumber);
				string path = fileAddress.Substring(0, fileAddress.LastIndexOf('/'));
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				byte[] array = _data.ByteSave();
				fileStream = new FileStream(fileAddress, FileMode.OpenOrCreate, FileAccess.Write);
				fileStream.SetLength((long)array.Length);
				fileStream.Write(array, 0, array.Length);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				int num = ex.HResultPublic();
				if (num == -2147024857 || num == -2147024784)
				{
					base.HideSaveIcon();
					return SaveLoadResult.NoSpace;
				}
				base.HideSaveIcon();
				throw ex;
			}
		}
		base.HideSaveIcon();
		return SaveLoadResult.Exists;
	}

	// Token: 0x06002326 RID: 8998 RVA: 0x000AA078 File Offset: 0x000A8478
	protected IEnumerator HasXFile(string _fileName, ReturnValue<SaveLoadResult> _result)
	{
		bool flag = File.Exists(_fileName);
		_result.Value = ((!flag) ? SaveLoadResult.NotExist : SaveLoadResult.Exists);
		yield break;
	}

	// Token: 0x06002327 RID: 8999 RVA: 0x000AA09C File Offset: 0x000A849C
	public override IEnumerator HasMetaSaveFile(ReturnValue<SaveLoadResult> _result)
	{
		string path = this.GetFileAddress(PCSaveManager.Type.Meta, -1, -1);
		IEnumerator hasFile = this.HasXFile(path, _result);
		while (hasFile.MoveNext())
		{
			yield return null;
		}
		if (_result.Value == SaveLoadResult.Exists)
		{
			byte[] array = File.ReadAllBytes(path);
			if (array == null || !MetaGameProgress.Validate(array))
			{
				_result.Value = SaveLoadResult.Corrupted;
			}
		}
		yield break;
	}

	// Token: 0x06002328 RID: 9000 RVA: 0x000AA0C0 File Offset: 0x000A84C0
	public override IEnumerator HasSaveFile(SaveMode _mode, int _slot, int _dlcNumber, ReturnValue<SaveLoadResult> _result)
	{
		string path = this.GetFileAddress(this.ModeToType(_mode), _slot, _dlcNumber);
		IEnumerator hasFile = this.HasXFile(path, _result);
		while (hasFile.MoveNext())
		{
			yield return null;
		}
		if (_result.Value == SaveLoadResult.Exists)
		{
			byte[] array = File.ReadAllBytes(path);
			if (array == null || !GameProgress.GameProgressData.Validate(array))
			{
				_result.Value = SaveLoadResult.Corrupted;
			}
		}
		yield break;
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000AA0F8 File Offset: 0x000A84F8
	public override void DeleteSave(SaveMode _mode, int _slot, int _dlcNumber, CallbackVoid _callback = null)
	{
		File.Delete(this.GetFileAddress(this.ModeToType(_mode), _slot, _dlcNumber));
		if (_callback != null)
		{
			_callback();
		}
	}

	// Token: 0x0600232A RID: 9002 RVA: 0x000AA11C File Offset: 0x000A851C
	public override void DeleteMetaSave(CallbackVoid _callback = null)
	{
		File.Delete(this.GetFileAddress(PCSaveManager.Type.Meta, -1, -1));
		if (_callback != null)
		{
			_callback();
		}
	}

	// Token: 0x0600232B RID: 9003 RVA: 0x000AA138 File Offset: 0x000A8538
	protected virtual string GetSaveDirectory()
	{
		return Application.persistentDataPath + "/";
	}

	// Token: 0x0600232C RID: 9004 RVA: 0x000AA14C File Offset: 0x000A854C
	private string GetFileAddress(PCSaveManager.Type _type, int _slot, int _dlcNumber)
	{
		if (_dlcNumber != -1)
		{
			if (_dlcNumber > base.MaxDLC)
			{
			}
			_dlcNumber = Mathf.Clamp(_dlcNumber, 0, base.MaxDLC);
		}
		string text = _type.ToString() + "_SaveFile";
		if (_type != PCSaveManager.Type.Meta)
		{
			if (_dlcNumber != -1)
			{
				text = "DLC" + _dlcNumber.ToString() + "_" + text;
			}
			text = text + "_" + _slot.ToString();
		}
		string saveDirectory = this.GetSaveDirectory();
		return saveDirectory + text + ".save";
	}

	// Token: 0x0600232D RID: 9005 RVA: 0x000AA1EA File Offset: 0x000A85EA
	private PCSaveManager.Type ModeToType(SaveMode _mode)
	{
		if (_mode != SaveMode.Main)
		{
			return PCSaveManager.Type.Meta;
		}
		return PCSaveManager.Type.CoopSlot;
	}

	// Token: 0x04001ACB RID: 6859
	protected const string c_filename = "SaveFile";

	// Token: 0x04001ACC RID: 6860
	protected const string c_extension = ".save";

	// Token: 0x04001ACD RID: 6861
	private const int c_metaSaveSlot = -1;

	// Token: 0x02000732 RID: 1842
	protected enum Type
	{
		// Token: 0x04001ACF RID: 6863
		Meta,
		// Token: 0x04001AD0 RID: 6864
		CoopSlot
	}
}
