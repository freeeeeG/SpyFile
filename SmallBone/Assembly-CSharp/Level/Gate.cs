using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004DB RID: 1243
	public class Gate : InteractiveObject
	{
		// Token: 0x0600183D RID: 6205 RVA: 0x0004BF72 File Offset: 0x0004A172
		private void OnDestroy()
		{
			this._animator = null;
			this._gateGraphicSetting.Dispose();
			this._gateProperty = null;
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0004BF8D File Offset: 0x0004A18D
		public override void OnActivate()
		{
			base.OnActivate();
			if (this._animator != null)
			{
				this._animator.Play(InteractiveObject._activateHash);
			}
			if (this._type == Gate.Type.None)
			{
				return;
			}
			this._gateProperty.ActivateGameObject();
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x0004BFC7 File Offset: 0x0004A1C7
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			this._animator.Play(InteractiveObject._deactivateHash);
			if (this._type == Gate.Type.None)
			{
				return;
			}
			this._gateProperty.DeactivateGameObject();
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0004BFF4 File Offset: 0x0004A1F4
		public override void InteractWith(Character character)
		{
			if (this._used)
			{
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			this._used = true;
			Singleton<Service>.Instance.levelManager.LoadNextMap(this._nodeIndex);
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0004C042 File Offset: 0x0004A242
		public void ShowDestroyed(bool terminal)
		{
			this._collider.enabled = false;
			this._animator.runtimeAnimatorController = (terminal ? this._destoryedForTerminal : this._destoryed);
			base.Deactivate();
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0004C074 File Offset: 0x0004A274
		public void Set(Gate.Type type, NodeIndex nodeIndex)
		{
			if (type == Gate.Type.None)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this._nodeIndex = nodeIndex;
			this._gateProperty = this._gateGraphicSetting.GetPropertyOf(type);
			this._animator.runtimeAnimatorController = this._gateProperty.animator;
			if (base.activated)
			{
				this._animator.Play(InteractiveObject._activateHash);
				this._gateProperty.ActivateGameObject();
				return;
			}
			this._gateProperty.DeactivateGameObject();
		}

		// Token: 0x0400151B RID: 5403
		[GetComponent]
		[SerializeField]
		private Animator _animator;

		// Token: 0x0400151C RID: 5404
		[SerializeField]
		[GetComponent]
		private Collider2D _collider;

		// Token: 0x0400151D RID: 5405
		[SerializeField]
		private RuntimeAnimatorController _destoryed;

		// Token: 0x0400151E RID: 5406
		[SerializeField]
		private RuntimeAnimatorController _destoryedForTerminal;

		// Token: 0x0400151F RID: 5407
		[SerializeField]
		private Gate.GateGraphicSetting _gateGraphicSetting;

		// Token: 0x04001520 RID: 5408
		private Gate.GateGraphicSetting.GateProperty _gateProperty;

		// Token: 0x04001521 RID: 5409
		private Gate.Type _type;

		// Token: 0x04001522 RID: 5410
		private NodeIndex _nodeIndex;

		// Token: 0x04001523 RID: 5411
		private bool _used;

		// Token: 0x020004DC RID: 1244
		[Serializable]
		public class GateGraphicSetting : ReorderableArray<Gate.GateGraphicSetting.GateProperty>
		{
			// Token: 0x06001844 RID: 6212 RVA: 0x0004C0EF File Offset: 0x0004A2EF
			public GateGraphicSetting(params Gate.GateGraphicSetting.GateProperty[] gateProperties)
			{
				this.values = gateProperties;
			}

			// Token: 0x06001845 RID: 6213 RVA: 0x0004C100 File Offset: 0x0004A300
			public Gate.GateGraphicSetting.GateProperty GetPropertyOf(Gate.Type type)
			{
				foreach (Gate.GateGraphicSetting.GateProperty gateProperty in this.values)
				{
					if (gateProperty.type.Equals(type))
					{
						return gateProperty;
					}
				}
				return null;
			}

			// Token: 0x06001846 RID: 6214 RVA: 0x0004C148 File Offset: 0x0004A348
			public void Dispose()
			{
				for (int i = 0; i < this.values.Length; i++)
				{
					this.values[i].Dispose();
					this.values[i] = null;
				}
				this.values = null;
			}

			// Token: 0x020004DD RID: 1245
			[Serializable]
			public class GateProperty
			{
				// Token: 0x06001847 RID: 6215 RVA: 0x0004C185 File Offset: 0x0004A385
				public GateProperty(Gate.Type type, RuntimeAnimatorController animator, GameObject gameObject)
				{
					this._type = type;
					this._animator = animator;
					this._gameObject = gameObject;
				}

				// Token: 0x170004CE RID: 1230
				// (get) Token: 0x06001848 RID: 6216 RVA: 0x0004C1A2 File Offset: 0x0004A3A2
				public Gate.Type type
				{
					get
					{
						return this._type;
					}
				}

				// Token: 0x170004CF RID: 1231
				// (get) Token: 0x06001849 RID: 6217 RVA: 0x0004C1AA File Offset: 0x0004A3AA
				public RuntimeAnimatorController animator
				{
					get
					{
						return this._animator;
					}
				}

				// Token: 0x170004D0 RID: 1232
				// (get) Token: 0x0600184A RID: 6218 RVA: 0x0004C1B2 File Offset: 0x0004A3B2
				public GameObject gameObject
				{
					get
					{
						return this._gameObject;
					}
				}

				// Token: 0x0600184B RID: 6219 RVA: 0x0004C1BA File Offset: 0x0004A3BA
				public void ActivateGameObject()
				{
					if (this._gameObject != null)
					{
						this._gameObject.SetActive(true);
					}
				}

				// Token: 0x0600184C RID: 6220 RVA: 0x0004C1D6 File Offset: 0x0004A3D6
				public void DeactivateGameObject()
				{
					if (this._gameObject != null)
					{
						this._gameObject.SetActive(false);
					}
				}

				// Token: 0x0600184D RID: 6221 RVA: 0x0004C1F2 File Offset: 0x0004A3F2
				public void Dispose()
				{
					this._animator = null;
					this._gameObject = null;
				}

				// Token: 0x04001524 RID: 5412
				[SerializeField]
				private Gate.Type _type;

				// Token: 0x04001525 RID: 5413
				[SerializeField]
				private RuntimeAnimatorController _animator;

				// Token: 0x04001526 RID: 5414
				[SerializeField]
				private GameObject _gameObject;
			}
		}

		// Token: 0x020004DE RID: 1246
		public enum Type
		{
			// Token: 0x04001528 RID: 5416
			None,
			// Token: 0x04001529 RID: 5417
			Normal,
			// Token: 0x0400152A RID: 5418
			Grave,
			// Token: 0x0400152B RID: 5419
			Chest,
			// Token: 0x0400152C RID: 5420
			Npc,
			// Token: 0x0400152D RID: 5421
			Terminal,
			// Token: 0x0400152E RID: 5422
			Adventurer,
			// Token: 0x0400152F RID: 5423
			Boss
		}
	}
}
