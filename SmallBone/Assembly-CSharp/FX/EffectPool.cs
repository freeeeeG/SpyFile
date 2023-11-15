using System;
using System.Collections;
using Singletons;
using UnityEngine;

namespace FX
{
	// Token: 0x0200022F RID: 559
	public class EffectPool : Singleton<EffectPool>
	{
		// Token: 0x06000AF4 RID: 2804 RVA: 0x0001E59D File Offset: 0x0001C79D
		protected override void Awake()
		{
			base.Awake();
			this._poolObjects = new EffectPoolInstance[300];
			base.StartCoroutine(this.CPrepareObjects());
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0001E5C2 File Offset: 0x0001C7C2
		private IEnumerator CUpdate()
		{
			for (;;)
			{
				for (int i = 0; i < this._border; i++)
				{
					EffectPoolInstance effectPoolInstance = this._poolObjects[i];
					if (!(effectPoolInstance == null))
					{
						effectPoolInstance.UpdateEffect();
						if (effectPoolInstance.state == EffectPoolInstance.State.Stopped)
						{
							this._border--;
							this._poolObjects[i] = this._poolObjects[this._border];
							this._poolObjects[this._border] = effectPoolInstance;
						}
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0001E5D1 File Offset: 0x0001C7D1
		private IEnumerator CPrepareObjects()
		{
			int num;
			for (int i = 0; i < 300; i = num + 1)
			{
				yield return null;
				this.InstantiatePoolObject(i);
				num = i;
			}
			base.StartCoroutine(this.CUpdate());
			yield break;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0001E5E0 File Offset: 0x0001C7E0
		private EffectPoolInstance GetPoolObjectOrInstantiate(int index)
		{
			EffectPoolInstance effectPoolInstance = this._poolObjects[index];
			if (effectPoolInstance == null || effectPoolInstance.state == EffectPoolInstance.State.Destroyed)
			{
				effectPoolInstance = this.InstantiatePoolObject(index);
			}
			return effectPoolInstance;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0001E611 File Offset: 0x0001C811
		private EffectPoolInstance InstantiatePoolObject(int index)
		{
			if (this._originalPoolObject != null)
			{
				this._poolObjects[index] = UnityEngine.Object.Instantiate<EffectPoolInstance>(this._originalPoolObject);
				return this._poolObjects[index];
			}
			return null;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0001E640 File Offset: 0x0001C840
		public EffectPoolInstance Play(RuntimeAnimatorController animation, float delay, float duration, bool loop, AnimationCurve fadeOutCurve, float fadeOutDuration)
		{
			EffectPoolInstance poolObjectOrInstantiate = this.GetPoolObjectOrInstantiate(this._border);
			if (poolObjectOrInstantiate == null)
			{
				return null;
			}
			poolObjectOrInstantiate.Play(animation, delay, duration, loop, fadeOutCurve, fadeOutDuration);
			this._border++;
			if (this._border == 300)
			{
				this._border--;
				EffectPoolInstance poolObjectOrInstantiate2 = this.GetPoolObjectOrInstantiate(0);
				if (poolObjectOrInstantiate2 == null)
				{
					return null;
				}
				poolObjectOrInstantiate2.Stop();
				this._poolObjects[this._border] = poolObjectOrInstantiate2;
				this._poolObjects[0] = poolObjectOrInstantiate;
			}
			return poolObjectOrInstantiate;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0001E6D0 File Offset: 0x0001C8D0
		public void Clear()
		{
			for (int i = 0; i < this._border; i++)
			{
				EffectPoolInstance poolObjectOrInstantiate = this.GetPoolObjectOrInstantiate(i);
				if (poolObjectOrInstantiate.state != EffectPoolInstance.State.Stopped)
				{
					poolObjectOrInstantiate.Stop();
				}
			}
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0001E704 File Offset: 0x0001C904
		public void ClearNonAttached()
		{
			for (int i = 0; i < this._border; i++)
			{
				EffectPoolInstance poolObjectOrInstantiate = this.GetPoolObjectOrInstantiate(i);
				if (poolObjectOrInstantiate.state != EffectPoolInstance.State.Stopped && !(poolObjectOrInstantiate.transform.parent != null))
				{
					poolObjectOrInstantiate.Stop();
				}
			}
		}

		// Token: 0x0400092C RID: 2348
		private const int _limit = 300;

		// Token: 0x0400092D RID: 2349
		[SerializeField]
		private EffectPoolInstance _originalPoolObject;

		// Token: 0x0400092E RID: 2350
		private EffectPoolInstance[] _poolObjects;

		// Token: 0x0400092F RID: 2351
		private int _border;
	}
}
