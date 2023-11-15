using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FX;
using Level;
using UI;
using UnityEngine;

namespace Scenes
{
	// Token: 0x0200013F RID: 319
	public class GameBase : Scene<GameBase>
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00012B6E File Offset: 0x00010D6E
		public UIManager uiManager
		{
			get
			{
				return this._uiManager;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x00012B76 File Offset: 0x00010D76
		public Camera camera
		{
			get
			{
				return this._camera;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00012B7E File Offset: 0x00010D7E
		public CameraController cameraController
		{
			get
			{
				return this._cameraController;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00012B86 File Offset: 0x00010D86
		public CameraController minimapCameraController
		{
			get
			{
				return this._minimapCameraController;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00012B8E File Offset: 0x00010D8E
		public GameFadeInOut gameFadeInOut
		{
			get
			{
				return this._gameFadeInOut;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00012B96 File Offset: 0x00010D96
		public PoolObjectContainer poolObjectContainer
		{
			get
			{
				return this._poolObjectContainer;
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00012BA0 File Offset: 0x00010DA0
		public void SetBackground(ParallaxBackground background, float originHeight)
		{
			if (this._background != null)
			{
				UnityEngine.Object.Destroy(this._background.gameObject);
			}
			if (background == null)
			{
				return;
			}
			this._background = UnityEngine.Object.Instantiate<ParallaxBackground>(background, this._cameraController.transform);
			this._background.transform.localPosition = new Vector3(0f, 0f, -this.cameraController.transform.localPosition.z);
			base.StartCoroutine(this.CInitialize(originHeight));
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00012C30 File Offset: 0x00010E30
		public void ChangeBackgroundWithFade(ParallaxBackground background, float originHeight)
		{
			if (this._background == null)
			{
				this.SetBackground(background, originHeight);
				return;
			}
			base.StartCoroutine(GameBase.<ChangeBackgroundWithFade>g__CDestroy|20_0(this._background));
			this._background = UnityEngine.Object.Instantiate<ParallaxBackground>(background, this._cameraController.transform);
			this._background.transform.localPosition = new Vector3(0f, 0f, -this.cameraController.transform.localPosition.z);
			base.StartCoroutine(this.CInitialize(originHeight));
			this._background.FadeOut();
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00012CCB File Offset: 0x00010ECB
		private IEnumerator CInitialize(float originHeight)
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			this._background.Initialize(originHeight);
			yield break;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00012CE9 File Offset: 0x00010EE9
		[CompilerGenerated]
		internal static IEnumerator <ChangeBackgroundWithFade>g__CDestroy|20_0(ParallaxBackground parallax)
		{
			yield return parallax.CFadeIn();
			UnityEngine.Object.Destroy(parallax.gameObject);
			yield break;
		}

		// Token: 0x040004B1 RID: 1201
		[SerializeField]
		private UIManager _uiManager;

		// Token: 0x040004B2 RID: 1202
		[SerializeField]
		private Camera _camera;

		// Token: 0x040004B3 RID: 1203
		[SerializeField]
		private CameraController _cameraController;

		// Token: 0x040004B4 RID: 1204
		[SerializeField]
		private CameraController _minimapCameraController;

		// Token: 0x040004B5 RID: 1205
		[SerializeField]
		private GameFadeInOut _gameFadeInOut;

		// Token: 0x040004B6 RID: 1206
		[SerializeField]
		private PoolObjectContainer _poolObjectContainer;

		// Token: 0x040004B7 RID: 1207
		private ParallaxBackground _background;
	}
}
