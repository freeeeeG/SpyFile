using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.SpecialMap
{
	// Token: 0x020001B2 RID: 434
	public class FollowMovement : MonoBehaviour
	{
		// Token: 0x06000938 RID: 2360 RVA: 0x0001A534 File Offset: 0x00018734
		private void Awake()
		{
			if (this._startOnAwake)
			{
				this.Run();
			}
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0001A544 File Offset: 0x00018744
		public void Run()
		{
			base.StartCoroutine("CChase");
			if (this._floatingY || this._floatingX)
			{
				base.StartCoroutine("CFloat");
			}
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0001A56E File Offset: 0x0001876E
		public void Stop()
		{
			base.StopCoroutine("CChase");
			if (this._floatingY || this._floatingX)
			{
				base.StopCoroutine("CFloat");
			}
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0001A596 File Offset: 0x00018796
		private IEnumerator CChase()
		{
			float elapsed = 0f;
			Transform targetTransform = this._target.GetTransform();
			float offsetX = this._offsetX.value;
			float offsetY = this._offsetY.value;
			for (;;)
			{
				if (!(targetTransform == null) && targetTransform.gameObject.activeInHierarchy)
				{
					float deltaTime = Chronometer.global.deltaTime;
					elapsed += deltaTime;
					Vector2 v = new Vector2(targetTransform.position.x + offsetX, targetTransform.position.y + offsetY);
					base.transform.position = Vector3.Lerp(base.transform.position, v, deltaTime * this._trackSpeed);
					yield return null;
				}
				else
				{
					yield return null;
					targetTransform = this._target.GetTransform();
				}
			}
			yield break;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0001A5A5 File Offset: 0x000187A5
		private IEnumerator CFloat()
		{
			float t = UnityEngine.Random.Range(0f, 3.1415927f);
			float floatAmplitude = 0.5f;
			float floatFrequency = 0.8f;
			for (;;)
			{
				Vector3 zero = Vector3.zero;
				t += Chronometer.global.deltaTime;
				if (this._floatingY)
				{
					zero.y = Mathf.Sin(t * 3.1415927f * floatFrequency) * floatAmplitude;
				}
				if (this._floatingX)
				{
					zero.x = Mathf.Sin(t * 3.1415927f * floatFrequency / 2f) * floatAmplitude;
				}
				this._body.localPosition = zero;
				yield return null;
			}
			yield break;
		}

		// Token: 0x04000794 RID: 1940
		[SerializeField]
		private bool _floatingY;

		// Token: 0x04000795 RID: 1941
		[SerializeField]
		private bool _floatingX;

		// Token: 0x04000796 RID: 1942
		[SerializeField]
		private Transform _body;

		// Token: 0x04000797 RID: 1943
		[SerializeField]
		private bool _startOnAwake;

		// Token: 0x04000798 RID: 1944
		[SerializeField]
		private FollowMovement.Target _target;

		// Token: 0x04000799 RID: 1945
		[SerializeField]
		private CustomFloat _offsetX;

		// Token: 0x0400079A RID: 1946
		[SerializeField]
		private CustomFloat _offsetY;

		// Token: 0x0400079B RID: 1947
		[SerializeField]
		private float _trackSpeed;

		// Token: 0x020001B3 RID: 435
		[Serializable]
		private class Target
		{
			// Token: 0x0600093E RID: 2366 RVA: 0x0001A5B4 File Offset: 0x000187B4
			internal Transform GetTransform()
			{
				switch (this._type)
				{
				case FollowMovement.Target.Type.Player:
					return Singleton<Service>.Instance.levelManager.player.transform;
				case FollowMovement.Target.Type.Character:
					return this._character.transform;
				case FollowMovement.Target.Type.Transform:
					return this._transform;
				case FollowMovement.Target.Type.ClosestTarget:
					return this.GetClosestObject();
				default:
					return Singleton<Service>.Instance.levelManager.player.transform;
				}
			}

			// Token: 0x0600093F RID: 2367 RVA: 0x0001A624 File Offset: 0x00018824
			private Transform GetClosestObject()
			{
				FollowMovement.Target._overlapper.contactFilter.SetLayerMask(this._layerMask);
				List<Collider2D> list = FollowMovement.Target._overlapper.OverlapCollider(this._range).results.Where(delegate(Collider2D result)
				{
					if (!this._hasCharacter)
					{
						return true;
					}
					Characters.Target component = result.GetComponent<Characters.Target>();
					return !(component == null) && component.character != null;
				}).ToList<Collider2D>();
				if (list.Count == 0)
				{
					return null;
				}
				if (list.Count == 1)
				{
					return list[0].transform;
				}
				float num = Physics2D.Distance(list[0], this._ownerRange).distance;
				int index = 0;
				for (int i = 1; i < list.Count; i++)
				{
					float distance = Physics2D.Distance(list[i], this._ownerRange).distance;
					if (num > distance)
					{
						index = i;
						num = distance;
					}
				}
				return list[index].transform;
			}

			// Token: 0x0400079C RID: 1948
			[SerializeField]
			private FollowMovement.Target.Type _type;

			// Token: 0x0400079D RID: 1949
			[SerializeField]
			private Character _character;

			// Token: 0x0400079E RID: 1950
			[SerializeField]
			private Transform _transform;

			// Token: 0x0400079F RID: 1951
			[SerializeField]
			private LayerMask _layerMask;

			// Token: 0x040007A0 RID: 1952
			[SerializeField]
			private Collider2D _range;

			// Token: 0x040007A1 RID: 1953
			[SerializeField]
			private Collider2D _ownerRange;

			// Token: 0x040007A2 RID: 1954
			[SerializeField]
			private bool _hasCharacter;

			// Token: 0x040007A3 RID: 1955
			private static NonAllocOverlapper _overlapper = new NonAllocOverlapper(15);

			// Token: 0x020001B4 RID: 436
			public enum Type
			{
				// Token: 0x040007A5 RID: 1957
				Player,
				// Token: 0x040007A6 RID: 1958
				Character,
				// Token: 0x040007A7 RID: 1959
				Transform,
				// Token: 0x040007A8 RID: 1960
				ClosestTarget
			}
		}
	}
}
