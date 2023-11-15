using System;
using System.Collections;
using Level;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class CastleCameraController : MonoBehaviour
{
	// Token: 0x0600015E RID: 350 RVA: 0x0000710C File Offset: 0x0000530C
	private void Awake()
	{
		this._cameraController = Scene<GameBase>.instance.cameraController;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000711E File Offset: 0x0000531E
	private IEnumerator CUpdate()
	{
		yield return null;
		for (;;)
		{
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				yield return null;
			}
			else
			{
				Vector3 position = Singleton<Service>.Instance.levelManager.player.transform.position;
				if (this._inside.bounds.Contains(position))
				{
					if (this._state != CastleCameraController.State.Inside)
					{
						yield return this.CTransitionToInside();
					}
				}
				else if (this._outside.bounds.Contains(position))
				{
					yield return this.CTransitionToOutside(position);
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000712D File Offset: 0x0000532D
	private void OnEnable()
	{
		base.StartCoroutine(this.CUpdate());
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000713C File Offset: 0x0000533C
	private IEnumerator CTransitionToInside()
	{
		this._state = CastleCameraController.State.Inside;
		float time = 0f;
		Color color = this._cover.color;
		Map.Instance.ChangeLight(this._insideLightColor, this._insideLightIntensity, this._lightColorTransitionTime);
		this._cameraController.pause = true;
		this._cameraController.zone = null;
		Vector3 originalPosition = this._cameraController.transform.position;
		Vector3 targetPosition = this._inside.GetClampedPosition(this._cameraController.camera, this._cameraController.transform.position);
		while (time < 1f)
		{
			yield return null;
			this._cameraController.transform.position = Vector3.Lerp(originalPosition, targetPosition, this._curve.Evaluate(time));
			time += Time.unscaledDeltaTime * 1.5f;
		}
		while (time < 1f)
		{
			yield return null;
			color.a = time;
			this._cover.color = color;
			this._cameraController.transform.position = Vector3.Lerp(originalPosition, targetPosition, time);
			time += Time.unscaledDeltaTime * 1.5f;
		}
		this._cameraController.pause = false;
		this._cameraController.zone = this._inside;
		yield break;
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000714B File Offset: 0x0000534B
	private IEnumerator CTransitionToOutside(Vector3 playerPosition)
	{
		this._state = CastleCameraController.State.Outside;
		Map.Instance.ChangeLight(this._outsideLightColor, this._outsideLightIntensity, this._lightColorTransitionTime);
		this._cameraController.zone = this._outsideCameraZone;
		if (this._portal.bounds.Contains(playerPosition))
		{
			for (float time = 0f; time < 1f; time += Time.unscaledDeltaTime)
			{
				yield return null;
			}
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			if (Singleton<Service>.Instance.levelManager.currentChapter.type == Chapter.Type.Castle)
			{
				Singleton<Service>.Instance.levelManager.Load(Chapter.Type.Chapter1);
			}
			else
			{
				Singleton<Service>.Instance.levelManager.Load(Chapter.Type.HardmodeChapter1);
			}
		}
		yield break;
	}

	// Token: 0x0400011E RID: 286
	[SerializeField]
	private float _lightColorTransitionTime;

	// Token: 0x0400011F RID: 287
	[Space]
	[SerializeField]
	private CameraZone _inside;

	// Token: 0x04000120 RID: 288
	[SerializeField]
	private Color _insideLightColor;

	// Token: 0x04000121 RID: 289
	[SerializeField]
	private float _insideLightIntensity;

	// Token: 0x04000122 RID: 290
	[SerializeField]
	[Space]
	private BoxCollider2D _outside;

	// Token: 0x04000123 RID: 291
	[SerializeField]
	private CameraZone _outsideCameraZone;

	// Token: 0x04000124 RID: 292
	[SerializeField]
	private Color _outsideLightColor;

	// Token: 0x04000125 RID: 293
	[SerializeField]
	private float _outsideLightIntensity;

	// Token: 0x04000126 RID: 294
	[Space]
	[SerializeField]
	private Collider2D _portal;

	// Token: 0x04000127 RID: 295
	[SerializeField]
	private AnimationCurve _curve;

	// Token: 0x04000128 RID: 296
	[SerializeField]
	private SpriteRenderer _cover;

	// Token: 0x04000129 RID: 297
	private CastleCameraController.State _state;

	// Token: 0x0400012A RID: 298
	private CameraController _cameraController;

	// Token: 0x0200004B RID: 75
	private enum State
	{
		// Token: 0x0400012C RID: 300
		Inside,
		// Token: 0x0400012D RID: 301
		OutsideDiving,
		// Token: 0x0400012E RID: 302
		Outside
	}
}
