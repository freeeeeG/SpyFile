using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000711 RID: 1809
	public class PolymorphBody : MonoBehaviour
	{
		// Token: 0x060024A2 RID: 9378 RVA: 0x0006E170 File Offset: 0x0006C370
		private void Awake()
		{
			if (this._overrider == null)
			{
				this._overrider = new AnimationClipOverrider(this._baseAnimator);
			}
			this._overrider.Override("EmptyIdle", this._idleClip);
			this._overrider.Override("EmptyWalk", this._walkClip);
			this._overrider.Override("EmptyJumpUp", this._jumpClip);
			this._overrider.Override("EmptyJumpDown", this._fallClip);
			this._overrider.Override("EmptyJumpDownLoop", this._fallRepeatClip);
			this._operationOnEnd.Initialize();
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x0006E210 File Offset: 0x0006C410
		private void OnDestroy()
		{
			this._originalBody = null;
			this._body = null;
			this._baseAnimator = null;
			if (this._overrider != null)
			{
				this._overrider.Dispose();
				this._overrider = null;
			}
			this._idleClip = null;
			this._walkClip = null;
			this._jumpClip = null;
			this._fallClip = null;
			this._fallRepeatClip = null;
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x0006E26F File Offset: 0x0006C46F
		public void StartPolymorph(float duration)
		{
			this.StartPolymorph();
			base.StartCoroutine(this.CEndPolymorph(duration));
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x0006E288 File Offset: 0x0006C488
		public void StartPolymorph()
		{
			if (this._body.activeSelf)
			{
				return;
			}
			this._originalBody.gameObject.SetActive(false);
			this._body.SetActive(true);
			this.character.CancelAction();
			this._characterAnimation.AttachOverrider(this._overrider);
			this.character.animationController.ForceUpdate();
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x0006E2EC File Offset: 0x0006C4EC
		public void EndPolymorph()
		{
			if (!this._body.activeSelf)
			{
				return;
			}
			this._operationOnEnd.Run(this.character);
			this._originalBody.gameObject.SetActive(true);
			this._body.SetActive(false);
			this.character.CancelAction();
			this._characterAnimation.DetachOverrider(this._overrider);
			this.character.animationController.ForceUpdate();
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x0006E361 File Offset: 0x0006C561
		private IEnumerator CEndPolymorph(float duration)
		{
			yield return this.character.chronometer.master.WaitForSeconds(duration);
			this.EndPolymorph();
			yield break;
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x0006E377 File Offset: 0x0006C577
		private void OnDisable()
		{
			this.EndPolymorph();
		}

		// Token: 0x04001F15 RID: 7957
		[NonSerialized]
		public Character character;

		// Token: 0x04001F16 RID: 7958
		[SerializeField]
		private CharacterAnimation _characterAnimation;

		// Token: 0x04001F17 RID: 7959
		[SerializeField]
		private GameObject _originalBody;

		// Token: 0x04001F18 RID: 7960
		[SerializeField]
		private GameObject _body;

		// Token: 0x04001F19 RID: 7961
		[SerializeField]
		[Space]
		private RuntimeAnimatorController _baseAnimator;

		// Token: 0x04001F1A RID: 7962
		[SerializeField]
		private AnimationClip _idleClip;

		// Token: 0x04001F1B RID: 7963
		[SerializeField]
		private AnimationClip _walkClip;

		// Token: 0x04001F1C RID: 7964
		[SerializeField]
		private AnimationClip _jumpClip;

		// Token: 0x04001F1D RID: 7965
		[SerializeField]
		private AnimationClip _fallClip;

		// Token: 0x04001F1E RID: 7966
		[SerializeField]
		private AnimationClip _fallRepeatClip;

		// Token: 0x04001F1F RID: 7967
		[SerializeField]
		[Space]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operationOnEnd;

		// Token: 0x04001F20 RID: 7968
		protected AnimationClipOverrider _overrider;
	}
}
