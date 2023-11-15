using System;
using Characters;
using FX;
using Singletons;
using UnityEngine;

// Token: 0x02000052 RID: 82
public abstract class InteractiveObject : MonoBehaviour
{
	// Token: 0x17000033 RID: 51
	// (get) Token: 0x0600017E RID: 382 RVA: 0x000076D4 File Offset: 0x000058D4
	protected virtual bool _interactable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x0600017F RID: 383 RVA: 0x000076D7 File Offset: 0x000058D7
	public virtual CharacterInteraction.InteractionType interactionType
	{
		get
		{
			return this._interactionType;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000180 RID: 384 RVA: 0x000076DF File Offset: 0x000058DF
	public bool popupVisible
	{
		get
		{
			return this._character != null;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000181 RID: 385 RVA: 0x000076ED File Offset: 0x000058ED
	// (set) Token: 0x06000182 RID: 386 RVA: 0x000076F5 File Offset: 0x000058F5
	public bool activated
	{
		get
		{
			return this._activated;
		}
		private set
		{
			this._activated = value;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000183 RID: 387 RVA: 0x000076FE File Offset: 0x000058FE
	public bool interactable
	{
		get
		{
			return this._interactable;
		}
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00007706 File Offset: 0x00005906
	protected virtual void Awake()
	{
		this.ClosePopup();
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000770E File Offset: 0x0000590E
	private void OnDisable()
	{
		this._activated = false;
	}

	// Token: 0x06000186 RID: 390 RVA: 0x00007717 File Offset: 0x00005917
	public void Activate()
	{
		this._activated = true;
		this.OnActivate();
	}

	// Token: 0x06000187 RID: 391 RVA: 0x00007726 File Offset: 0x00005926
	public void Deactivate()
	{
		this.ClosePopup();
		this._activated = false;
		this.OnDeactivate();
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000773B File Offset: 0x0000593B
	public virtual void OnActivate()
	{
		PersistentSingleton<SoundManager>.Instance.PlaySound(this._activateSound, base.transform.position);
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00007759 File Offset: 0x00005959
	public virtual void OnDeactivate()
	{
		PersistentSingleton<SoundManager>.Instance.PlaySound(this._deactivateSound, base.transform.position);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00002191 File Offset: 0x00000391
	public virtual void InteractWith(Character character)
	{
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00002191 File Offset: 0x00000391
	public virtual void InteractWithByPressing(Character character)
	{
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00007778 File Offset: 0x00005978
	public virtual void OpenPopupBy(Character character)
	{
		this._character = character;
		this.pressingPercent = 0f;
		foreach (GameObject gameObject in this._uiObjects)
		{
			if (!(gameObject == null) && !gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
		}
		if (this._uiObject == null || this._uiObject.activeSelf || this.autoInteract)
		{
			return;
		}
		this._uiObject.SetActive(true);
	}

	// Token: 0x0600018D RID: 397 RVA: 0x000077F8 File Offset: 0x000059F8
	public virtual void ClosePopup()
	{
		this._character = null;
		foreach (GameObject gameObject in this._uiObjects)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		if (this._uiObject != null)
		{
			this._uiObject.SetActive(false);
		}
	}

	// Token: 0x04000145 RID: 325
	protected static readonly Vector2 _popupUIOffset = new Vector2(5f, 2f);

	// Token: 0x04000146 RID: 326
	protected static readonly int _activateHash = Animator.StringToHash("Activate");

	// Token: 0x04000147 RID: 327
	protected static readonly int _deactivateHash = Animator.StringToHash("Deactivate");

	// Token: 0x04000148 RID: 328
	[SerializeField]
	protected CharacterInteraction.InteractionType _interactionType;

	// Token: 0x04000149 RID: 329
	public bool autoInteract;

	// Token: 0x0400014A RID: 330
	[Space]
	[SerializeField]
	protected SoundInfo _activateSound;

	// Token: 0x0400014B RID: 331
	[SerializeField]
	protected SoundInfo _deactivateSound;

	// Token: 0x0400014C RID: 332
	[SerializeField]
	protected SoundInfo _interactSound;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	[Tooltip("모든 오브젝트에서 작동하는 건 아니며 코드상에서 직접 설정해주어야 함")]
	protected SoundInfo _interactFailSound;

	// Token: 0x0400014E RID: 334
	[SerializeField]
	[Space]
	protected GameObject _uiObject;

	// Token: 0x0400014F RID: 335
	[SerializeField]
	protected GameObject[] _uiObjects;

	// Token: 0x04000150 RID: 336
	[SerializeField]
	[Space]
	protected bool _activated = true;

	// Token: 0x04000151 RID: 337
	protected Character _character;

	// Token: 0x04000152 RID: 338
	[NonSerialized]
	public float pressingPercent;
}
