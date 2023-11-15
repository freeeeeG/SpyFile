using System;
using GameResources;
using InControl;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI
{
	// Token: 0x02000387 RID: 903
	public class ActionKeyImage : MonoBehaviour
	{
		// Token: 0x0600107E RID: 4222 RVA: 0x00030CA8 File Offset: 0x0002EEA8
		private void Awake()
		{
			this._action = this.FindAction();
			if (this._action == null)
			{
				throw new Exception("Couldn't found key " + this._actionName);
			}
			KeyMapper.Map.OnSimplifiedLastInputTypeChanged += this.OnLastInputTypeChanged;
			this._action.OnBindingsChanged += this.UpdateImage;
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00030D0C File Offset: 0x0002EF0C
		private void OnEnable()
		{
			this.UpdateImage();
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00030D0C File Offset: 0x0002EF0C
		private void Start()
		{
			this.UpdateImage();
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x00030D14 File Offset: 0x0002EF14
		private void OnDestroy()
		{
			KeyMapper.Map.OnSimplifiedLastInputTypeChanged -= this.OnLastInputTypeChanged;
			this._action.OnBindingsChanged -= this.UpdateImage;
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00030D0C File Offset: 0x0002EF0C
		private void OnLastInputTypeChanged(BindingSourceType bindingSourceType)
		{
			this.UpdateImage();
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00030D44 File Offset: 0x0002EF44
		private void UpdateImage()
		{
			if (this._image != null)
			{
				this._image.SetNativeSize();
			}
			BindingSourceType simplifiedLastInputType = KeyMapper.Map.SimplifiedLastInputType;
			foreach (BindingSource bindingSource in this._action.Bindings)
			{
				BindingSourceType bindingSourceType = KeyMap.SimplifyBindingSourceType(bindingSource.BindingSourceType);
				if (simplifiedLastInputType == bindingSourceType)
				{
					Sprite keyIconOrDefault = CommonResource.instance.GetKeyIconOrDefault(bindingSource, this._outline);
					if (this._image != null)
					{
						this._image.sprite = keyIconOrDefault;
						this._image.SetNativeSize();
					}
					if (this._spriteRenderer != null)
					{
						this._spriteRenderer.sprite = keyIconOrDefault;
					}
					break;
				}
			}
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00030E1C File Offset: 0x0002F01C
		private PlayerAction FindAction()
		{
			foreach (PlayerAction playerAction in KeyMapper.Map.Actions)
			{
				if (playerAction.Name.Equals(this._actionName, StringComparison.OrdinalIgnoreCase))
				{
					return playerAction;
				}
			}
			return null;
		}

		// Token: 0x04000D89 RID: 3465
		[SerializeField]
		private Image _image;

		// Token: 0x04000D8A RID: 3466
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04000D8B RID: 3467
		[SerializeField]
		private string _actionName;

		// Token: 0x04000D8C RID: 3468
		[SerializeField]
		private bool _outline;

		// Token: 0x04000D8D RID: 3469
		private PlayerAction _action;
	}
}
