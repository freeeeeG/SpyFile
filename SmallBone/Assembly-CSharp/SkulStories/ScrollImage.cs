using System;
using System.Collections;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace SkulStories
{
	// Token: 0x0200011A RID: 282
	public class ScrollImage : Sequence
	{
		// Token: 0x0600058E RID: 1422 RVA: 0x00010B0E File Offset: 0x0000ED0E
		private void Start()
		{
			this._narrationScene = Scene<GameBase>.instance.uiManager.narrationScene;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00010B28 File Offset: 0x0000ED28
		private void CheckType()
		{
			ScrollImage.Type type = this._type;
			if (type == ScrollImage.Type.Scene)
			{
				this._scene = this._narrationScene.scene;
				return;
			}
			if (type != ScrollImage.Type.OverlayScene)
			{
				return;
			}
			this._scene = this._narrationScene.overlayScene;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00010B67 File Offset: 0x0000ED67
		public override IEnumerator CRun()
		{
			if (this._narration.skipped)
			{
				yield break;
			}
			this.CheckType();
			Vector3 startPosition = this._scene.transform.localPosition;
			float moveTime = 0f;
			Vector2 targetPosition = new Vector2(startPosition.x * this._amountX, startPosition.y * this._amountY);
			yield return Chronometer.global.WaitForSeconds(this._startDelay);
			while (moveTime < 1f)
			{
				moveTime += Chronometer.global.deltaTime / this._speed;
				if (this._narrationScene.changed)
				{
					break;
				}
				this._scene.transform.localPosition = Vector2.Lerp(startPosition, targetPosition, this.curve.Evaluate(moveTime));
				yield return null;
			}
			this._narrationScene.scene.rectTransform.pivot = this._scene.rectTransform.pivot;
			this._narrationScene.scene.transform.localPosition = this._scene.transform.localPosition;
			yield break;
		}

		// Token: 0x04000434 RID: 1076
		[SerializeField]
		private ScrollImage.Type _type;

		// Token: 0x04000435 RID: 1077
		[SerializeField]
		private float _amountX;

		// Token: 0x04000436 RID: 1078
		[SerializeField]
		private float _amountY = 1f;

		// Token: 0x04000437 RID: 1079
		[SerializeField]
		private float _speed;

		// Token: 0x04000438 RID: 1080
		[SerializeField]
		private float _startDelay;

		// Token: 0x04000439 RID: 1081
		[SerializeField]
		private Curve curve;

		// Token: 0x0400043A RID: 1082
		private Image _scene;

		// Token: 0x0400043B RID: 1083
		private NarrationScene _narrationScene;

		// Token: 0x0200011B RID: 283
		private enum Type
		{
			// Token: 0x0400043D RID: 1085
			Scene,
			// Token: 0x0400043E RID: 1086
			OverlayScene
		}
	}
}
