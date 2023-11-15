using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Operations.Fx;
using FX;
using GameResources;
using UnityEditor;
using UnityEngine;

namespace Level.Chapter4
{
	// Token: 0x0200063F RID: 1599
	public class PlatformContainer : MonoBehaviour
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x00061076 File Offset: 0x0005F276
		public Platform center
		{
			get
			{
				return this._center;
			}
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x00061080 File Offset: 0x0005F280
		private void Awake()
		{
			this._platforms = new Platform[this._container.childCount - 1];
			int num = 0;
			foreach (object obj in this._container)
			{
				Transform transform = (Transform)obj;
				if (!(this._center.transform == transform))
				{
					Platform component = transform.GetComponent<Platform>();
					if (!(component == null))
					{
						this._platforms[num++] = component;
					}
				}
			}
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x00061120 File Offset: 0x0005F320
		public void RandomTakeTo(Platform[] platforms)
		{
			this._platforms.Shuffle<Platform>();
			Platform[] array = this._platforms.Take(platforms.Length).ToArray<Platform>();
			for (int i = 0; i < platforms.Length; i++)
			{
				platforms[i] = array[i];
			}
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x00061160 File Offset: 0x0005F360
		public bool CanPurify()
		{
			Platform[] platforms = this._platforms;
			for (int i = 0; i < platforms.Length; i++)
			{
				if (!platforms[i].tentacleAlives)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x00061190 File Offset: 0x0005F390
		public void NoTentacleTakeTo(Platform[] results)
		{
			this._platforms.Shuffle<Platform>();
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = null;
			}
			int num = 0;
			int num2 = 0;
			int num3 = results.Length;
			int num4 = this._platforms.Length;
			while (num < num3 && num2 < num4)
			{
				if (this._platforms[num2].tentacleAlives)
				{
					num2++;
				}
				else
				{
					results[num++] = this._platforms[num2++];
				}
			}
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x00061204 File Offset: 0x0005F404
		public void Appear()
		{
			foreach (object obj in this._container)
			{
				Transform transform = (Transform)obj;
				PlatformContainer.Assets.appearance.Spawn(transform.position, 0f, 1f);
				transform.gameObject.SetActive(true);
			}
		}

		// Token: 0x04001B2F RID: 6959
		[SerializeField]
		private Platform _center;

		// Token: 0x04001B30 RID: 6960
		[SerializeField]
		[Subcomponent(typeof(SpawnEffect))]
		private SpawnEffect _spawnEffect;

		// Token: 0x04001B31 RID: 6961
		[SerializeField]
		private Transform _container;

		// Token: 0x04001B32 RID: 6962
		[SerializeReference]
		public List<INode> multipleNodes = new List<INode>
		{
			new Node(),
			new DerivedNode()
		};

		// Token: 0x04001B33 RID: 6963
		private Platform[] _platforms;

		// Token: 0x02000640 RID: 1600
		private class Assets
		{
			// Token: 0x04001B34 RID: 6964
			internal static EffectInfo appearance = new EffectInfo(CommonResource.instance.enemyAppearanceEffect);
		}
	}
}
