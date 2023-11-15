using System;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000B17 RID: 2839
public class T17EventSystem : EventSystem
{
	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06003969 RID: 14697 RVA: 0x00110A28 File Offset: 0x0010EE28
	public GamepadUser AssignedGamepadUser
	{
		get
		{
			return this.m_AssignedGamepadUser;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x0600396A RID: 14698 RVA: 0x00110A30 File Offset: 0x0010EE30
	public SuppressionController SuppressionController
	{
		get
		{
			return this.m_suppressionController;
		}
	}

	// Token: 0x0600396B RID: 14699 RVA: 0x00110A38 File Offset: 0x0010EE38
	protected override void Awake()
	{
		base.Awake();
		T17EventSystemsManager.Instance.RegisterEventSystem(this);
		InControlInputModule component = base.GetComponent<InControlInputModule>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		this.m_T17Module = base.GetComponent<T17StandaloneInputModule>();
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x00110A7B File Offset: 0x0010EE7B
	public void ResetSystem()
	{
		this.m_AssignedGamepadUser = null;
		this.m_suppressionController.Reset();
	}

	// Token: 0x0600396D RID: 14701 RVA: 0x00110A90 File Offset: 0x0010EE90
	protected override void Update()
	{
		if (this.m_suppressionController.IsSuppressed())
		{
			this.m_suppressionController.UpdateSuppressors();
			if (!this.m_suppressionController.IsSuppressed())
			{
				this.m_T17Module.ClearEventSystemLogicalButtons();
			}
			return;
		}
		EventSystem current = EventSystem.current;
		EventSystem.current = this;
		base.Update();
		EventSystem.current = current;
	}

	// Token: 0x0600396E RID: 14702 RVA: 0x00110AEC File Offset: 0x0010EEEC
	protected override void OnApplicationFocus(bool bHasFocus)
	{
	}

	// Token: 0x0600396F RID: 14703 RVA: 0x00110AF0 File Offset: 0x0010EEF0
	private void LateUpdate()
	{
		if (this.m_suppressionController.IsSuppressed())
		{
			return;
		}
		if (this.m_ObjectToSetAtTheEndOfUpdate != null && base.currentSelectedGameObject != this.m_ObjectToSetAtTheEndOfUpdate)
		{
			base.SetSelectedGameObject(this.m_ObjectToSetAtTheEndOfUpdate);
			T17StandaloneInputModule t17StandaloneInputModule = (T17StandaloneInputModule)base.currentInputModule;
			if (t17StandaloneInputModule != null)
			{
				t17StandaloneInputModule.SetLastSelected(this.m_ObjectToSetAtTheEndOfUpdate);
			}
		}
		if (base.currentSelectedGameObject != null)
		{
			this.m_LastRequestedSelectedGameobject = base.currentSelectedGameObject;
		}
		this.m_ObjectToSetAtTheEndOfUpdate = null;
	}

	// Token: 0x06003970 RID: 14704 RVA: 0x00110B89 File Offset: 0x0010EF89
	public void ForceDeselectSelectionObject()
	{
		base.SetSelectedGameObject(null);
		this.m_LastRequestedSelectedGameobject = null;
	}

	// Token: 0x06003971 RID: 14705 RVA: 0x00110B99 File Offset: 0x0010EF99
	public new void SetSelectedGameObject(GameObject target)
	{
		this.m_LastRequestedSelectedGameobject = target;
		this.m_ObjectToSetAtTheEndOfUpdate = target;
	}

	// Token: 0x06003972 RID: 14706 RVA: 0x00110BA9 File Offset: 0x0010EFA9
	public new void SetSelectedGameObject(GameObject selected, BaseEventData pointer)
	{
		base.SetSelectedGameObject(selected, pointer);
	}

	// Token: 0x06003973 RID: 14707 RVA: 0x00110BB3 File Offset: 0x0010EFB3
	public GameObject GetLastRequestedSelectedGameobject()
	{
		return this.m_LastRequestedSelectedGameobject;
	}

	// Token: 0x06003974 RID: 14708 RVA: 0x00110BBB File Offset: 0x0010EFBB
	public GameObject GetPendingSelectedGameObject()
	{
		return this.m_ObjectToSetAtTheEndOfUpdate;
	}

	// Token: 0x06003975 RID: 14709 RVA: 0x00110BC4 File Offset: 0x0010EFC4
	public void SetAssignedGamepadUser(GamepadUser gamepadUser)
	{
		this.m_AssignedGamepadUser = gamepadUser;
		if (this.m_AssignedGamepadUser != null)
		{
			this.m_T17Module.inputActionsPerSecond = 5f;
			this.m_T17Module.InvertYAxis = true;
			this.m_T17Module.Initialize();
		}
	}

	// Token: 0x06003976 RID: 14710 RVA: 0x00110C10 File Offset: 0x0010F010
	public void ResetInputModule()
	{
		if (this.m_T17Module != null)
		{
			this.m_T17Module.Initialize();
		}
	}

	// Token: 0x06003977 RID: 14711 RVA: 0x00110C2E File Offset: 0x0010F02E
	public Suppressor Disable(UnityEngine.Object _suppressor)
	{
		return this.m_suppressionController.AddSuppressor(_suppressor);
	}

	// Token: 0x06003978 RID: 14712 RVA: 0x00110C3C File Offset: 0x0010F03C
	public void ReleaseSuppressor(Suppressor _suppressor)
	{
		_suppressor.Release();
		this.m_suppressionController.UpdateSuppressors();
	}

	// Token: 0x06003979 RID: 14713 RVA: 0x00110C4F File Offset: 0x0010F04F
	public bool IsDisabled()
	{
		return this.m_suppressionController != null && this.m_suppressionController.IsSuppressed();
	}

	// Token: 0x04002E31 RID: 11825
	private GameObject m_LastRequestedSelectedGameobject;

	// Token: 0x04002E32 RID: 11826
	private GameObject m_ObjectToSetAtTheEndOfUpdate;

	// Token: 0x04002E33 RID: 11827
	private GamepadUser m_AssignedGamepadUser;

	// Token: 0x04002E34 RID: 11828
	private T17StandaloneInputModule m_T17Module;

	// Token: 0x04002E35 RID: 11829
	private SuppressionController m_suppressionController = new SuppressionController();
}
