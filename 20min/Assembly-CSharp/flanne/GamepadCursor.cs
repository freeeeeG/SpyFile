using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

namespace flanne
{
	// Token: 0x0200006E RID: 110
	public class GamepadCursor : MonoBehaviour
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x00017B14 File Offset: 0x00015D14
		private void UpdateMotion()
		{
			if (this._virtualMouse == null || Gamepad.current == null)
			{
				return;
			}
			Vector2 vector = Gamepad.current.leftStick.ReadValue();
			vector *= this.cursorSpeed * Time.deltaTime;
			Vector2 vector2 = this._virtualMouse.position.ReadValue() + vector;
			vector2.x = Mathf.Clamp(vector2.x, 0f, (float)Screen.width);
			vector2.y = Mathf.Clamp(vector2.y, 0f, (float)Screen.height);
			InputState.Change<Vector2>(this._virtualMouse.position, vector2, InputUpdateType.None, default(InputEventPtr));
			InputState.Change<Vector2>(this._virtualMouse.delta, vector, InputUpdateType.None, default(InputEventPtr));
			bool flag = Gamepad.current.aButton.IsPressed(0f);
			if (this._previousMouseState != Gamepad.current.aButton.isPressed)
			{
				MouseState state;
				this._virtualMouse.CopyState(out state);
				state.WithButton(MouseButton.Left, flag);
				InputState.Change<MouseState>(this._virtualMouse, state, InputUpdateType.None, default(InputEventPtr));
				this._previousMouseState = flag;
			}
			this.AnchorCursor(vector2);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00017C44 File Offset: 0x00015E44
		private void OnEnable()
		{
			if (this._currentMouse == null)
			{
				this._currentMouse = Mouse.current;
			}
			this.InitCursor();
			if (this._virtualMouse == null)
			{
				this._virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse", null, null);
			}
			else if (!this._virtualMouse.added)
			{
				InputSystem.AddDevice(this._virtualMouse);
			}
			InputUser.PerformPairingWithDevice(this._virtualMouse, this.playerInput.user, InputUserPairingOptions.None);
			Vector2 anchoredPosition = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			this.cursorTransform.anchoredPosition = anchoredPosition;
			Vector2 anchoredPosition2 = this.cursorTransform.anchoredPosition;
			InputState.Change<Vector2>(this._virtualMouse.position, anchoredPosition2, InputUpdateType.None, default(InputEventPtr));
			InputSystem.onAfterUpdate += this.UpdateMotion;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00017D18 File Offset: 0x00015F18
		private void OnDisable()
		{
			Cursor.visible = true;
			if (this.playerInput != null)
			{
				InputUser user = this.playerInput.user;
				this.playerInput.user.UnpairDevice(this._virtualMouse);
			}
			if (this._virtualMouse != null && this._virtualMouse.added)
			{
				InputSystem.RemoveDevice(this._virtualMouse);
			}
			InputSystem.onAfterUpdate -= this.UpdateMotion;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00017D8F File Offset: 0x00015F8F
		private void Update()
		{
			if (this.previousControlScheme != this.playerInput.currentControlScheme)
			{
				this.OnControlsChanged(this.playerInput);
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00017DB8 File Offset: 0x00015FB8
		private void AnchorCursor(Vector2 position)
		{
			Vector2 anchoredPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasTransform, position, this.mainCamera, out anchoredPosition);
			this.cursorTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00017DE8 File Offset: 0x00015FE8
		private void InitCursor()
		{
			if (this.playerInput.currentControlScheme == "Keyboard&Mouse")
			{
				this.cursorTransform.gameObject.SetActive(false);
				Cursor.visible = true;
			}
			else if (this.playerInput.currentControlScheme == "Gamepad")
			{
				this.cursorTransform.gameObject.SetActive(true);
				Cursor.visible = false;
			}
			this.previousControlScheme = this.playerInput.currentControlScheme;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00017E64 File Offset: 0x00016064
		private void OnControlsChanged(PlayerInput input)
		{
			if (input.currentControlScheme == "Keyboard&Mouse" && this.previousControlScheme != input.currentControlScheme)
			{
				this.cursorTransform.gameObject.SetActive(false);
				Cursor.visible = true;
				if (this._currentMouse == null)
				{
					this._currentMouse = Mouse.current;
				}
				this._currentMouse.WarpCursorPosition(this._virtualMouse.position.ReadValue());
				this.previousControlScheme = "Keyboard&Mouse";
				return;
			}
			if (input.currentControlScheme == "Gamepad" && this.previousControlScheme != input.currentControlScheme)
			{
				this.cursorTransform.gameObject.SetActive(true);
				Cursor.visible = false;
				InputState.Change<Vector2>(this._virtualMouse.position, this._currentMouse.position.ReadValue(), InputUpdateType.None, default(InputEventPtr));
				this.AnchorCursor(this._currentMouse.position.ReadValue());
				this.previousControlScheme = "Gamepad";
			}
		}

		// Token: 0x040002A5 RID: 677
		[SerializeField]
		private PlayerInput playerInput;

		// Token: 0x040002A6 RID: 678
		[SerializeField]
		private RectTransform cursorTransform;

		// Token: 0x040002A7 RID: 679
		[SerializeField]
		private RectTransform canvasTransform;

		// Token: 0x040002A8 RID: 680
		[SerializeField]
		private Camera mainCamera;

		// Token: 0x040002A9 RID: 681
		[SerializeField]
		private float cursorSpeed = 1000f;

		// Token: 0x040002AA RID: 682
		private const string gamepadScheme = "Gamepad";

		// Token: 0x040002AB RID: 683
		private const string mouseScheme = "Keyboard&Mouse";

		// Token: 0x040002AC RID: 684
		private string previousControlScheme = "";

		// Token: 0x040002AD RID: 685
		private bool _previousMouseState;

		// Token: 0x040002AE RID: 686
		private Mouse _virtualMouse;

		// Token: 0x040002AF RID: 687
		private Mouse _currentMouse;
	}
}
