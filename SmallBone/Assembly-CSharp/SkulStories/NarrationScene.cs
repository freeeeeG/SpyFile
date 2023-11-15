using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SkulStories
{
	// Token: 0x0200010A RID: 266
	public class NarrationScene : MonoBehaviour
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0000FF00 File Offset: 0x0000E100
		public bool changed
		{
			get
			{
				return this._changed;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0000FF08 File Offset: 0x0000E108
		public Image scene
		{
			get
			{
				return this._scene;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x0000FF10 File Offset: 0x0000E110
		public Image overlayScene
		{
			get
			{
				return this._overlayScene;
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000FF18 File Offset: 0x0000E118
		private void Start()
		{
			this._position = this._scene.transform.localPosition;
			this._originColor = new Color(1f, 1f, 1f, 0f);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000FF50 File Offset: 0x0000E150
		private void Initialize()
		{
			this._overlayScene.color = this._originColor;
			this._currentScene.color = this._originColor;
			this._currentScene.rectTransform.pivot = this._pivot;
			this._currentScene.transform.localPosition = this._position;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000FFAB File Offset: 0x0000E1AB
		public void Change(Sprite sprite, bool overlay)
		{
			if (overlay)
			{
				this.ChangeOverlayScene(sprite);
			}
			else
			{
				this.ChangeScene(sprite);
			}
			this._changed = true;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000FFC7 File Offset: 0x0000E1C7
		private void ChangeOverlayScene(Sprite sprite)
		{
			this._overlayScene.sprite = sprite;
			this._currentScene = this._overlayScene;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000FFE1 File Offset: 0x0000E1E1
		private void ChangeScene(Sprite sprite)
		{
			this._scene.sprite = sprite;
			this._currentScene = this._scene;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0000FFFB File Offset: 0x0000E1FB
		public void SetPivot(Vector2 pivot)
		{
			this._pivot = pivot;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00010004 File Offset: 0x0000E204
		public IEnumerator CFadeIn(float speed)
		{
			this.Initialize();
			Color startColor = this._currentScene.color;
			Color different = this._target - this._currentScene.color;
			float time = 0f;
			while (time < speed)
			{
				time += Chronometer.global.deltaTime;
				this._currentScene.color = startColor + different * (time / speed);
				yield return null;
			}
			this._changed = false;
			this._currentScene.color = this._target;
			this._scene.sprite = this._currentScene.sprite;
			this._scene.rectTransform.pivot = this._pivot;
			this._scene.transform.localPosition = this._position;
			yield break;
		}

		// Token: 0x040003F1 RID: 1009
		[SerializeField]
		private Image _scene;

		// Token: 0x040003F2 RID: 1010
		[SerializeField]
		private Image _overlayScene;

		// Token: 0x040003F3 RID: 1011
		[SerializeField]
		private Color _target;

		// Token: 0x040003F4 RID: 1012
		private Vector3 _position;

		// Token: 0x040003F5 RID: 1013
		private bool _changed;

		// Token: 0x040003F6 RID: 1014
		private Vector2 _pivot;

		// Token: 0x040003F7 RID: 1015
		private Image _currentScene;

		// Token: 0x040003F8 RID: 1016
		private Color _originColor;
	}
}
