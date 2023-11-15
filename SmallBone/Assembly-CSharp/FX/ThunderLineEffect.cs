using System;
using System.Collections;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace FX
{
	// Token: 0x02000222 RID: 546
	public sealed class ThunderLineEffect : MonoBehaviour
	{
		// Token: 0x06000AC4 RID: 2756 RVA: 0x0001913A File Offset: 0x0001733A
		public void Acitvate()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000075E7 File Offset: 0x000057E7
		public void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0001D3E5 File Offset: 0x0001B5E5
		private void OnEnable()
		{
			base.StartCoroutine(this.CDrawLine());
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0001D3F4 File Offset: 0x0001B5F4
		private IEnumerator CDrawLine()
		{
			Chronometer chronometer = this.GetChronometer();
			for (;;)
			{
				this.DrawLine();
				if (chronometer == null)
				{
					yield return Chronometer.global.WaitForSeconds(this._updateTime);
				}
				else
				{
					yield return chronometer.WaitForSeconds(this._updateTime);
				}
			}
			yield break;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0001D404 File Offset: 0x0001B604
		private Chronometer GetChronometer()
		{
			ThunderLineEffect.ChronometerType chronometerType = this._chronometerType;
			if (chronometerType != ThunderLineEffect.ChronometerType.Character)
			{
				if (chronometerType != ThunderLineEffect.ChronometerType.Player)
				{
					return null;
				}
				return Singleton<Service>.Instance.levelManager.player.chronometer.master;
			}
			else
			{
				if (this._chonometerOwner == null)
				{
					return null;
				}
				return this._chonometerOwner.chronometer.master;
			}
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0001D460 File Offset: 0x0001B660
		private void DrawLine()
		{
			this._lineRenderer.widthCurve = this._widthCurve;
			this._lineRenderer.widthMultiplier = this._widthMultiplier;
			this._lineRenderer.startColor = this._startColor;
			this._lineRenderer.endColor = this._endColor;
			this._lineRenderer.positionCount = this._vertexCount;
			if (this._outlineEnable)
			{
				this._outLineRenderer.enabled = true;
				this._outLineRenderer.startColor = this._outlineStartColor;
				this._outLineRenderer.endColor = this._outlineEndColor;
				this._outLineRenderer.positionCount = this._vertexCount;
				this._outLineRenderer.widthMultiplier = this._widthMultiplier + this._outlineWidthMultiplier;
				this._outLineRenderer.widthCurve = this._outlineWidthCurve;
			}
			else if (this._outLineRenderer != null)
			{
				this._outLineRenderer.enabled = false;
			}
			Vector2 vector = this._endPoint.position - this._startPoint.position;
			Vector2 normalized = vector.normalized;
			float chunck = vector.magnitude / (float)this._vertexCount;
			Vector2 normalVector = Quaternion.Euler(0f, 0f, 90f) * normalized;
			Vector2 vector2 = this._startPoint.position;
			float amplitudeMultiplier = UnityEngine.Random.Range(-this._amplitude, this._amplitude);
			float spike = MMMaths.Chance(this._sparkChance) ? this._sparkAmount.value : 1f;
			Vector2 noise = UnityEngine.Random.insideUnitCircle * this._noise;
			Vector2 thunderVertex = this.GetThunderVertex(normalized, chunck, normalVector, vector2, 0, amplitudeMultiplier, spike, noise);
			this._lineRenderer.SetPosition(0, this._fixedStartPosiiton ? vector2 : thunderVertex);
			if (this._outlineEnable)
			{
				this._outLineRenderer.SetPosition(0, this._fixedStartPosiiton ? vector2 : thunderVertex);
			}
			for (int i = 1; i < this._vertexCount - 1; i++)
			{
				amplitudeMultiplier = UnityEngine.Random.Range(-this._amplitude, this._amplitude);
				spike = (MMMaths.Chance(this._sparkChance) ? this._sparkAmount.value : 1f);
				noise = UnityEngine.Random.insideUnitCircle * this._noise;
				thunderVertex = this.GetThunderVertex(normalized, chunck, normalVector, vector2, i, amplitudeMultiplier, spike, noise);
				this._lineRenderer.SetPosition(i, thunderVertex);
				if (this._outlineEnable)
				{
					this._outLineRenderer.SetPosition(i, thunderVertex);
				}
			}
			amplitudeMultiplier = UnityEngine.Random.Range(-this._amplitude, this._amplitude);
			spike = (MMMaths.Chance(this._sparkChance) ? this._sparkAmount.value : 1f);
			noise = UnityEngine.Random.insideUnitCircle * this._noise;
			thunderVertex = this.GetThunderVertex(normalized, chunck, normalVector, vector2, this._vertexCount - 1, amplitudeMultiplier, spike, noise);
			this._lineRenderer.SetPosition(this._vertexCount - 1, this._fixedEndPosiiton ? this._endPoint.position : thunderVertex);
			if (this._outlineEnable)
			{
				this._outLineRenderer.SetPosition(this._vertexCount - 1, this._fixedEndPosiiton ? this._endPoint.position : thunderVertex);
			}
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0001D7DC File Offset: 0x0001B9DC
		private Vector2 GetThunderVertex(Vector2 direcitonVector, float chunck, Vector2 normalVector, Vector2 startPosition, int index, float amplitudeMultiplier, float spike, Vector2 noise)
		{
			float num = this._amplitudeCurve.Evaluate((float)index / (float)this._lineRenderer.positionCount) * amplitudeMultiplier;
			Vector2 b = normalVector * (num * spike);
			return startPosition + chunck * (float)index * direcitonVector + b + noise;
		}

		// Token: 0x040008CC RID: 2252
		[SerializeField]
		[Header("Renderer")]
		private LineRenderer _lineRenderer;

		// Token: 0x040008CD RID: 2253
		[SerializeField]
		private LineRenderer _outLineRenderer;

		// Token: 0x040008CE RID: 2254
		[Header("Point")]
		[SerializeField]
		private Transform _startPoint;

		// Token: 0x040008CF RID: 2255
		[SerializeField]
		private Transform _endPoint;

		// Token: 0x040008D0 RID: 2256
		[SerializeField]
		private bool _fixedStartPosiiton = true;

		// Token: 0x040008D1 RID: 2257
		[SerializeField]
		private bool _fixedEndPosiiton = true;

		// Token: 0x040008D2 RID: 2258
		[Header("Amplitude")]
		[SerializeField]
		private Curve _amplitudeCurve;

		// Token: 0x040008D3 RID: 2259
		[SerializeField]
		private float _amplitude;

		// Token: 0x040008D4 RID: 2260
		[SerializeField]
		private int _vertexCount;

		// Token: 0x040008D5 RID: 2261
		[SerializeField]
		[Header("Width")]
		private AnimationCurve _widthCurve;

		// Token: 0x040008D6 RID: 2262
		[SerializeField]
		private AnimationCurve _outlineWidthCurve;

		// Token: 0x040008D7 RID: 2263
		[Range(0f, 3f)]
		[SerializeField]
		private float _widthMultiplier;

		// Token: 0x040008D8 RID: 2264
		[Range(0f, 3f)]
		[SerializeField]
		private float _outlineWidthMultiplier;

		// Token: 0x040008D9 RID: 2265
		[Header("Color")]
		[SerializeField]
		private Color _startColor;

		// Token: 0x040008DA RID: 2266
		[SerializeField]
		private Color _endColor;

		// Token: 0x040008DB RID: 2267
		[SerializeField]
		private Color _outlineStartColor;

		// Token: 0x040008DC RID: 2268
		[SerializeField]
		private Color _outlineEndColor;

		// Token: 0x040008DD RID: 2269
		[SerializeField]
		[Header("Option")]
		private ThunderLineEffect.ChronometerType _chronometerType;

		// Token: 0x040008DE RID: 2270
		[SerializeField]
		private Character _chonometerOwner;

		// Token: 0x040008DF RID: 2271
		[SerializeField]
		[FrameTime]
		private float _updateTime = 1f;

		// Token: 0x040008E0 RID: 2272
		[SerializeField]
		private bool _outlineEnable;

		// Token: 0x040008E1 RID: 2273
		[SerializeField]
		private float _noise;

		// Token: 0x040008E2 RID: 2274
		[SerializeField]
		[Range(0f, 1f)]
		private float _sparkChance;

		// Token: 0x040008E3 RID: 2275
		[SerializeField]
		private CustomFloat _sparkAmount;

		// Token: 0x02000223 RID: 547
		private enum ChronometerType
		{
			// Token: 0x040008E5 RID: 2277
			Global,
			// Token: 0x040008E6 RID: 2278
			Character,
			// Token: 0x040008E7 RID: 2279
			Player
		}
	}
}
