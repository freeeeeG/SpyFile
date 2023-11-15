using System;
using System.Collections;
using Characters.Abilities.Constraints;
using Scenes;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006C8 RID: 1736
	public sealed class CharacterSpotLight : MonoBehaviour
	{
		// Token: 0x060022DB RID: 8923 RVA: 0x000692A4 File Offset: 0x000674A4
		private void Update()
		{
			if (!this.activated)
			{
				return;
			}
			if (this._owner == null)
			{
				UnityEngine.Object.Destroy(this._spotlight.gameObject);
				return;
			}
			if (Vector2.SqrMagnitude(Scene<GameBase>.instance.cameraController.transform.position - this._owner.transform.position) > 1000f)
			{
				this._spotlight.gameObject.SetActive(false);
			}
			else
			{
				this._spotlight.gameObject.SetActive(true);
			}
			this._spotlight.position = this._owner.transform.position;
			if (this._constraints.Pass())
			{
				if (!this._constranitsFade)
				{
					return;
				}
				this._constranitsFade = false;
				this._fadeCoroutine.Stop();
				this._fadeCoroutine = this.StartCoroutineWithReference(this.CActivate());
			}
			if (!this._constranitsFade)
			{
				this._constranitsFade = true;
				this._fadeCoroutine.Stop();
				this._fadeCoroutine = this.StartCoroutineWithReference(this.CDeactivate());
			}
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x000693B8 File Offset: 0x000675B8
		public void Activate()
		{
			this.activated = true;
			this._fadeCoroutine.Stop();
			this._fadeCoroutine = this.StartCoroutineWithReference(this.CActivate());
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000693DE File Offset: 0x000675DE
		private IEnumerator CActivate()
		{
			this._spotlight.gameObject.SetActive(true);
			for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime * this._fadeSpeed)
			{
				this.SetFadeAlpha(t);
				yield return null;
			}
			this.SetFadeAlpha(1f);
			yield break;
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000693ED File Offset: 0x000675ED
		public void Deactivate()
		{
			this.activated = false;
			this._fadeCoroutine.Stop();
			this._fadeCoroutine = this.StartCoroutineWithReference(this.CDeactivate());
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x00069413 File Offset: 0x00067613
		private IEnumerator CDeactivate()
		{
			for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime * this._fadeSpeed)
			{
				this.SetFadeAlpha(1f - t);
				yield return null;
			}
			this.SetFadeAlpha(0f);
			this._spotlight.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x00069424 File Offset: 0x00067624
		private void SetFadeAlpha(float alpha)
		{
			Color color = this._renderer.material.color;
			color.a = alpha;
			this._renderer.material.color = color;
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x0006945B File Offset: 0x0006765B
		private void OnDestroy()
		{
			UnityEngine.Object.Destroy(this._spotlight.gameObject);
		}

		// Token: 0x04001DB9 RID: 7609
		[SerializeField]
		private Character _owner;

		// Token: 0x04001DBA RID: 7610
		[SerializeField]
		private Transform _spotlight;

		// Token: 0x04001DBB RID: 7611
		[SerializeField]
		private Renderer _renderer;

		// Token: 0x04001DBC RID: 7612
		private Constraint[] _constraints = new Constraint[]
		{
			new LetterBox(),
			new Dialogue(),
			new Story(),
			new EndingCredit()
		};

		// Token: 0x04001DBD RID: 7613
		private float _fadeSpeed = 1.5f;

		// Token: 0x04001DBE RID: 7614
		private bool activated;

		// Token: 0x04001DBF RID: 7615
		private CoroutineReference _fadeCoroutine;

		// Token: 0x04001DC0 RID: 7616
		private bool _constranitsFade;
	}
}
