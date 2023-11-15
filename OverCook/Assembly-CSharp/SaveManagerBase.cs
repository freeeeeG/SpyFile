using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200073B RID: 1851
public abstract class SaveManagerBase : Manager, ISaveManager
{
	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06002379 RID: 9081 RVA: 0x000A8B54 File Offset: 0x000A6F54
	protected int MaxSlots
	{
		get
		{
			return this.m_maxSlots;
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x0600237A RID: 9082 RVA: 0x000A8B5C File Offset: 0x000A6F5C
	protected int MaxDLC
	{
		get
		{
			return DLCManagerBase.SupportedDLCLimit;
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x0600237B RID: 9083 RVA: 0x000A8B63 File Offset: 0x000A6F63
	public bool ProfileLoaded
	{
		get
		{
			return this.m_metaGameProgress != null;
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x0600237C RID: 9084 RVA: 0x000A8B71 File Offset: 0x000A6F71
	public bool IsSavingMeta
	{
		get
		{
			return this.m_savingMeta;
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x0600237D RID: 9085 RVA: 0x000A8B79 File Offset: 0x000A6F79
	public bool IsSavingProgress
	{
		get
		{
			return this.m_savingProgress;
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x0600237E RID: 9086 RVA: 0x000A8B81 File Offset: 0x000A6F81
	public bool IsSaving
	{
		get
		{
			return this.IsSavingMeta || this.IsSavingProgress;
		}
	}

	// Token: 0x0600237F RID: 9087 RVA: 0x000A8B97 File Offset: 0x000A6F97
	protected virtual void Awake()
	{
		this.m_activeInputHideCallback = delegate()
		{
			this.m_activeInputDialog = null;
		};
	}

	// Token: 0x06002380 RID: 9088 RVA: 0x000A8BAB File Offset: 0x000A6FAB
	public MetaGameProgress GetMetaGameProgress()
	{
		return this.m_metaGameProgress;
	}

	// Token: 0x06002381 RID: 9089 RVA: 0x000A8BB3 File Offset: 0x000A6FB3
	public void CreateMetaSession()
	{
		this.m_metaSession = this.m_metaSessionPrefab.InstantiateOnParent(null, true);
		this.m_metaGameProgress = this.m_metaSession.RequireComponentRecursive<MetaGameProgress>();
		this.m_metaGameProgress.ConsoleReset();
	}

	// Token: 0x06002382 RID: 9090 RVA: 0x000A8BE4 File Offset: 0x000A6FE4
	public virtual void UnloadProfile()
	{
		this.DestroyMetaSession();
	}

	// Token: 0x06002383 RID: 9091 RVA: 0x000A8BEC File Offset: 0x000A6FEC
	public void DestroyMetaSession()
	{
		if (this.m_metaSession != null)
		{
			if (this.IsSaving)
			{
			}
			this.m_metaSession.DestroyImmediate();
			this.m_metaSession = null;
			this.m_metaGameProgress = null;
		}
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x000A8C24 File Offset: 0x000A7024
	protected void DestroySession()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession != null)
		{
			gameSession.gameObject.DestroyImmediate();
		}
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x000A8C50 File Offset: 0x000A7050
	public virtual void DeleteAll()
	{
		this.DeleteMetaSave(null);
		this.DestroyMetaSession();
		this.CreateMetaSession();
		for (int i = 0; i < this.MaxSlots; i++)
		{
			this.DeleteSave(SaveMode.Main, i, -1, null);
			for (int j = 0; j < this.MaxDLC; j++)
			{
				this.DeleteSave(SaveMode.Main, i, j, null);
			}
		}
		this.DestroySession();
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x000A8CB8 File Offset: 0x000A70B8
	protected virtual void Update()
	{
		if (this.m_deleteAllCo != null && !this.m_deleteAllCo.MoveNext())
		{
			this.m_deleteAllCo = null;
		}
	}

	// Token: 0x06002387 RID: 9095 RVA: 0x000A8CDC File Offset: 0x000A70DC
	protected void ShowSaveIcon(bool _resetIconStartTime = true)
	{
		if (this.m_showSave == null)
		{
			this.m_showSave = SpinnerIconManager.Instance.Show(SpinnerIconManager.SpinnerIconType.Save, this, _resetIconStartTime);
		}
	}

	// Token: 0x06002388 RID: 9096 RVA: 0x000A8CFC File Offset: 0x000A70FC
	protected void HideSaveIcon()
	{
		if (this.m_showSave != null)
		{
			this.m_showSave.Release();
			this.m_showSave = null;
		}
	}

	// Token: 0x06002389 RID: 9097 RVA: 0x000A8D1B File Offset: 0x000A711B
	protected void ShowLoadIcon(bool _resetIconStartTime = true)
	{
		if (this.m_showLoad == null)
		{
			this.m_showLoad = SpinnerIconManager.Instance.Show(SpinnerIconManager.SpinnerIconType.Load, this, _resetIconStartTime);
		}
	}

	// Token: 0x0600238A RID: 9098 RVA: 0x000A8D3B File Offset: 0x000A713B
	protected void HideLoadIcon()
	{
		if (this.m_showLoad != null)
		{
			this.m_showLoad.Release();
			this.m_showLoad = null;
		}
	}

	// Token: 0x0600238B RID: 9099
	public abstract IEnumerator<SaveLoadResult?> LoadProfile(GamepadUser _user);

	// Token: 0x0600238C RID: 9100
	protected abstract IEnumerator<SaveLoadResult?> PlatformLoadSave(SaveMode _mode, int _slot, int _dlcNumber);

	// Token: 0x0600238D RID: 9101 RVA: 0x000A8D5C File Offset: 0x000A715C
	public IEnumerator<SaveLoadResult?> LoadSave(SaveMode _mode, int _slot, int _dlcNumber)
	{
		IEnumerator<SaveLoadResult?> load = this.PlatformLoadSave(_mode, _slot, _dlcNumber);
		while (load.MoveNext())
		{
			yield return null;
		}
		SaveLoadResult? result = load.Current;
		if (result.GetValueOrDefault() == SaveLoadResult.Corrupted && result != null)
		{
			result = null;
			this.ShowCorruptedSaveDialog(_mode, _slot, _dlcNumber, delegate
			{
				result = new SaveLoadResult?(SaveLoadResult.NotExist);
			}, delegate
			{
				result = new SaveLoadResult?(SaveLoadResult.Cancel);
			});
			while (result == null)
			{
				yield return null;
			}
		}
		yield return result;
		yield break;
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x000A8D8C File Offset: 0x000A718C
	public void SaveMetaProgress(SaveSystemCallback _callback = null)
	{
		base.StartCoroutine(this.SaveMetaProgressRoutine(_callback));
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000A8D9C File Offset: 0x000A719C
	protected IEnumerator SaveMetaProgressRoutine(SaveSystemCallback _callback = null)
	{
		this.m_savingMeta = true;
		bool abortSaving = false;
		SaveSystemStatus? status = null;
		SaveSystemCallback callback = delegate(SaveSystemStatus _status)
		{
			status = new SaveSystemStatus?(_status);
		};
		while ((status == null || status.Value.Status == SaveSystemStatus.SaveStatus.Retry) && !abortSaving)
		{
			status = null;
			this.TrySaveMetaProgress(callback);
			SaveSystemStatus.SaveStatus lastStatus = SaveSystemStatus.SaveStatus.COUNT;
			while ((status == null || status.Value.Status == SaveSystemStatus.SaveStatus.InProgress) && !abortSaving)
			{
				if (status != null && status.Value.Status != lastStatus)
				{
					if (_callback != null)
					{
						_callback(status.Value);
					}
					if (!abortSaving)
					{
						abortSaving = this.CheckForClientSaveFailure(status.Value);
					}
					lastStatus = status.Value.Status;
				}
				yield return null;
			}
			if (status != null && !abortSaving)
			{
				abortSaving = this.CheckForClientSaveFailure(status.Value);
			}
			if (!abortSaving && _callback != null && status.Value.Status != SaveSystemStatus.SaveStatus.Complete)
			{
				_callback(status.Value);
			}
		}
		this.m_savingMeta = false;
		if (!abortSaving)
		{
			if (_callback != null)
			{
				_callback(status.Value);
			}
		}
		else if (_callback != null)
		{
			_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Complete, SaveLoadResult.NotSaveable));
		}
		if (!this.IsSaving)
		{
			this.OnIdle();
		}
		yield break;
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000A8DC0 File Offset: 0x000A71C0
	private void TrySaveMetaProgress(SaveSystemCallback _callback = null)
	{
		SaveManagerBase.PlatformCallback callback = delegate(SaveLoadResult _result)
		{
			if (_result == SaveLoadResult.NoSpace)
			{
				this.CloseSpinner();
				if (_callback != null)
				{
					_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.InProgress, SaveLoadResult.NoSpace));
				}
				this.ShowNoSpaceForMetaDialog(delegate
				{
					if (this.m_spinner == null)
					{
						this.m_spinner = T17DialogBoxManager.GetDialog(false);
						if (this.m_spinner != null)
						{
							this.m_spinner.Initialize("Save.Spinner.RetryingMetaSave.Title", "Save.Spinner.RetryingMetaSave.Body", null, null, null, T17DialogBox.Symbols.Spinner, true, true, false);
							this.m_spinner.Show();
						}
					}
					if (_callback != null)
					{
						_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Retry, SaveLoadResult.NoSpace));
					}
				}, delegate
				{
					if (_callback != null)
					{
						_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Complete, SaveLoadResult.Cancel));
					}
				});
				return;
			}
			this.CloseSpinner();
			if (_callback != null)
			{
				_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Complete, _result));
			}
		};
		if (this.m_metaGameProgress != null)
		{
			this.PlatformSaveMetaProgress(callback);
		}
		else if (_callback != null)
		{
			_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Complete, SaveLoadResult.Cancel));
		}
	}

	// Token: 0x06002391 RID: 9105
	protected abstract void PlatformSaveMetaProgress(SaveManagerBase.PlatformCallback _callback);

	// Token: 0x06002392 RID: 9106
	protected abstract void PlatformSaveData(SaveMode _mode, int _slot, int _dlcNumber, SaveManagerBase.PlatformCallback _callback);

	// Token: 0x06002393 RID: 9107 RVA: 0x000A8E28 File Offset: 0x000A7228
	private void TrySaveData(SaveMode _mode, int _slot, int _dlcNumber, SaveSystemCallback _callback = null)
	{
		SaveManagerBase.PlatformCallback callback = delegate(SaveLoadResult _result)
		{
			if (_result == SaveLoadResult.NoSpace)
			{
				this.CloseSpinner();
				if (_callback != null)
				{
					_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.InProgress, SaveLoadResult.NoSpace));
				}
				this.ShowNoSpaceForSaveDialog(delegate
				{
					if (this.m_spinner == null)
					{
						this.m_spinner = T17DialogBoxManager.GetDialog(false);
						if (this.m_spinner != null)
						{
							this.m_spinner.Initialize("Save.Spinner.RetryingSaveSlot.Title", "Save.Spinner.RetryingSaveSlot.Body", null, null, null, T17DialogBox.Symbols.Spinner, true, true, false);
							this.m_spinner.Show();
						}
					}
					if (_callback != null)
					{
						_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Retry, SaveLoadResult.NoSpace));
					}
				}, delegate
				{
					if (_callback != null)
					{
						_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Complete, SaveLoadResult.Cancel));
					}
				});
				return;
			}
			this.CloseSpinner();
			if (_result == SaveLoadResult.Exists)
			{
				this.SaveMetaProgress(_callback);
			}
			else if (_callback != null)
			{
				_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Complete, _result));
			}
		};
		if (this.ProfileLoaded)
		{
			this.PlatformSaveData(_mode, _slot, _dlcNumber, callback);
		}
		else if (_callback != null)
		{
			_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Complete, SaveLoadResult.Cancel));
		}
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x000A8E90 File Offset: 0x000A7290
	public IEnumerator SaveData(SaveMode _mode, int _slot, int _dlcNumber, SaveSystemCallback _callback = null)
	{
		this.m_savingProgress = true;
		bool abortSaving = false;
		SaveSystemStatus? status = null;
		SaveSystemCallback callback = delegate(SaveSystemStatus _status)
		{
			status = new SaveSystemStatus?(_status);
		};
		while ((status == null || status.Value.Status == SaveSystemStatus.SaveStatus.Retry) && !abortSaving)
		{
			status = null;
			this.TrySaveData(_mode, _slot, _dlcNumber, callback);
			SaveSystemStatus.SaveStatus lastStatus = SaveSystemStatus.SaveStatus.COUNT;
			while (status == null || (status.Value.Status == SaveSystemStatus.SaveStatus.InProgress && !abortSaving))
			{
				if (status != null && status.Value.Status != lastStatus)
				{
					if (_callback != null)
					{
						_callback(status.Value);
					}
					if (!abortSaving)
					{
						abortSaving = this.CheckForClientSaveFailure(status.Value);
					}
					lastStatus = status.Value.Status;
				}
				yield return null;
			}
			if (status != null && !abortSaving)
			{
				abortSaving = this.CheckForClientSaveFailure(status.Value);
			}
			if (!abortSaving && _callback != null && status.Value.Status != SaveSystemStatus.SaveStatus.Complete)
			{
				_callback(status.Value);
			}
		}
		this.m_savingProgress = false;
		if (!abortSaving)
		{
			if (_callback != null)
			{
				_callback(status.Value);
			}
		}
		else if (_callback != null)
		{
			_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Complete, SaveLoadResult.NotSaveable));
		}
		if (!this.IsSaving)
		{
			this.OnIdle();
		}
		yield break;
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x000A8EC8 File Offset: 0x000A72C8
	private bool CheckForClientSaveFailure(SaveSystemStatus _status)
	{
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost() && _status.Result == SaveLoadResult.NoSpace)
		{
			T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
			if (dialog != null)
			{
				dialog.Initialize("ClientSave.SaveFailed.Title", "ClientSave.SaveFailed.Message", "ClientSave.SaveFailed.Confirm", string.Empty, string.Empty, T17DialogBox.Symbols.Warning, true, true, false);
				dialog.Show();
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002396 RID: 9110
	public abstract IEnumerator HasMetaSaveFile(ReturnValue<SaveLoadResult> _result);

	// Token: 0x06002397 RID: 9111
	public abstract IEnumerator HasSaveFile(SaveMode _type, int _slot, int _dlcNumber, ReturnValue<SaveLoadResult> _hasSave);

	// Token: 0x06002398 RID: 9112
	public abstract void DeleteSave(SaveMode _type, int _slot, int _dlcNumber, CallbackVoid _callback = null);

	// Token: 0x06002399 RID: 9113
	public abstract void DeleteMetaSave(CallbackVoid _callback = null);

	// Token: 0x0600239A RID: 9114 RVA: 0x000A8F38 File Offset: 0x000A7338
	private void OnIdle()
	{
		int num = 0;
		for (int i = 0; i < this.m_idleCallbacks.Count; i++)
		{
			this.m_idleCallbacks[i]();
			num++;
			if (this.IsSaving)
			{
				break;
			}
		}
		this.m_idleCallbacks.RemoveRange(0, num);
	}

	// Token: 0x0600239B RID: 9115 RVA: 0x000A8F95 File Offset: 0x000A7395
	public void RegisterOnIdle(GenericVoid _callback)
	{
		if (this.IsSaving)
		{
			this.m_idleCallbacks.Add(_callback);
		}
		else
		{
			_callback();
		}
	}

	// Token: 0x0600239C RID: 9116 RVA: 0x000A8FB9 File Offset: 0x000A73B9
	public void UnregisterOnIdle(GenericVoid _callback)
	{
		this.m_idleCallbacks.Remove(_callback);
	}

	// Token: 0x0600239D RID: 9117 RVA: 0x000A8FC8 File Offset: 0x000A73C8
	private void ShowDialog(string _title, string _message, string _confirmText, string _declineText, string _cancelText, T17DialogBox.DialogEvent _confirmCallback, T17DialogBox.DialogEvent _declineCallback, T17DialogBox.DialogEvent _cancelCallback, T17DialogBox.Symbols _symbol)
	{
		this.m_activeInputDialog = T17DialogBoxManager.GetDialog(false);
		if (this.m_activeInputDialog != null)
		{
			this.m_activeInputDialog.Initialize(_title, _message, _confirmText, _declineText, _cancelText, _symbol, true, true, false);
			if (_confirmCallback != null)
			{
				T17DialogBox activeInputDialog = this.m_activeInputDialog;
				activeInputDialog.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(activeInputDialog.OnConfirm, (T17DialogBox.DialogEvent)Delegate.Combine(_confirmCallback, this.m_activeInputHideCallback));
			}
			if (_declineCallback != null)
			{
				T17DialogBox activeInputDialog2 = this.m_activeInputDialog;
				activeInputDialog2.OnDecline = (T17DialogBox.DialogEvent)Delegate.Combine(activeInputDialog2.OnDecline, (T17DialogBox.DialogEvent)Delegate.Combine(_declineCallback, this.m_activeInputHideCallback));
			}
			if (_cancelCallback != null)
			{
				T17DialogBox activeInputDialog3 = this.m_activeInputDialog;
				activeInputDialog3.OnCancel = (T17DialogBox.DialogEvent)Delegate.Combine(activeInputDialog3.OnCancel, (T17DialogBox.DialogEvent)Delegate.Combine(_cancelCallback, this.m_activeInputHideCallback));
			}
			this.m_activeInputDialog.Show();
		}
	}

	// Token: 0x0600239E RID: 9118 RVA: 0x000A90B0 File Offset: 0x000A74B0
	protected void ShowNoSpaceForMetaDialog(T17DialogBox.DialogEvent _retryCallback, T17DialogBox.DialogEvent _cancelCallback)
	{
		this.ShowDialog("MetaSave.NoSpace.Title", "MetaSave.NoSpace.Body", "Text.Button.Retry", null, null, _retryCallback, null, _cancelCallback, T17DialogBox.Symbols.Error);
	}

	// Token: 0x0600239F RID: 9119 RVA: 0x000A90D8 File Offset: 0x000A74D8
	protected void ShowNoSpaceForSaveDialog(T17DialogBox.DialogEvent _retryCallback, T17DialogBox.DialogEvent _cancelCallback)
	{
		this.ShowDialog("SaveSlot.NoSpace.Title", "SaveSlot.NoSpace.Body", "Text.Button.Retry", null, null, _retryCallback, null, _cancelCallback, T17DialogBox.Symbols.Error);
	}

	// Token: 0x060023A0 RID: 9120 RVA: 0x000A9100 File Offset: 0x000A7500
	protected void ShowCorruptedMetaDialog(SaveManagerBase.PlatformCallback _callback)
	{
		T17DialogBox.DialogEvent metaDeletedCallback = delegate()
		{
			_callback(SaveLoadResult.NotExist);
		};
		T17DialogBox.DialogEvent cancelCallback = delegate()
		{
			_callback(SaveLoadResult.Cancel);
		};
		this.ShowCorruptedMetaDialog(metaDeletedCallback, cancelCallback);
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x000A913C File Offset: 0x000A753C
	protected void ShowCorruptedMetaDialog(T17DialogBox.DialogEvent _metaDeletedCallback, T17DialogBox.DialogEvent _cancelCallback)
	{
		this.ShowDialog("MetaSave.CorruptedConfirmDelete.Title", "MetaSave.CorruptedConfirmDelete.Body", "Text.Button.Delete", null, "Text.Button.Cancel", delegate
		{
			this.DeleteMetaSave(delegate
			{
				_metaDeletedCallback();
			});
		}, null, _cancelCallback, T17DialogBox.Symbols.Error);
	}

	// Token: 0x060023A2 RID: 9122 RVA: 0x000A9188 File Offset: 0x000A7588
	protected void ShowCorruptedSaveDialog(SaveMode _type, int _slotNum, int _dlcNumber, T17DialogBox.DialogEvent _saveDeletedCallback, T17DialogBox.DialogEvent _cancelCallback)
	{
		this.ShowDialog("Save.CorruptedConfirmDelete.Title", "Save.CorruptedConfirmDelete.Body", "Text.Button.Delete", null, "Text.Button.Cancel", delegate
		{
			this.DeleteSave(_type, _slotNum, _dlcNumber, delegate
			{
				_saveDeletedCallback();
			});
		}, null, _cancelCallback, T17DialogBox.Symbols.Error);
	}

	// Token: 0x060023A3 RID: 9123 RVA: 0x000A91EA File Offset: 0x000A75EA
	protected void CloseSpinner()
	{
		if (this.m_spinner != null)
		{
			this.m_spinner.Hide();
			this.m_spinner = null;
		}
	}

	// Token: 0x060023A4 RID: 9124 RVA: 0x000A920F File Offset: 0x000A760F
	public bool HasActiveInputDialog()
	{
		return this.m_activeInputDialog != null && this.m_activeInputDialog.IsActive;
	}

	// Token: 0x060023A5 RID: 9125 RVA: 0x000A9230 File Offset: 0x000A7630
	public void CancelActiveInputDialog()
	{
		if (this.HasActiveInputDialog())
		{
			this.m_activeInputDialog.Cancel();
		}
		this.m_activeInputDialog = null;
	}

	// Token: 0x04001AED RID: 6893
	[SerializeField]
	public int m_maxSlots = 3;

	// Token: 0x04001AEE RID: 6894
	private bool m_savingMeta;

	// Token: 0x04001AEF RID: 6895
	private bool m_savingProgress;

	// Token: 0x04001AF0 RID: 6896
	private List<GenericVoid> m_idleCallbacks = new List<GenericVoid>(2);

	// Token: 0x04001AF1 RID: 6897
	[SerializeField]
	private GameObject m_metaSessionPrefab;

	// Token: 0x04001AF2 RID: 6898
	private GameObject m_metaSession;

	// Token: 0x04001AF3 RID: 6899
	private MetaGameProgress m_metaGameProgress;

	// Token: 0x04001AF4 RID: 6900
	private IEnumerator m_deleteAllCo;

	// Token: 0x04001AF5 RID: 6901
	private Suppressor m_showSave;

	// Token: 0x04001AF6 RID: 6902
	private Suppressor m_showLoad;

	// Token: 0x04001AF7 RID: 6903
	private T17DialogBox m_spinner;

	// Token: 0x04001AF8 RID: 6904
	private T17DialogBox m_activeInputDialog;

	// Token: 0x04001AF9 RID: 6905
	private T17DialogBox.DialogEvent m_activeInputHideCallback;

	// Token: 0x0200073C RID: 1852
	// (Invoke) Token: 0x060023A8 RID: 9128
	protected delegate void PlatformCallback(SaveLoadResult _result);
}
