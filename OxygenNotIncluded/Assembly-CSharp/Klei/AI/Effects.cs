using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DF2 RID: 3570
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Effects")]
	public class Effects : KMonoBehaviour, ISaveLoadable, ISim1000ms
	{
		// Token: 0x06006DAD RID: 28077 RVA: 0x002B4113 File Offset: 0x002B2313
		protected override void OnPrefabInit()
		{
			this.autoRegisterSimRender = false;
		}

		// Token: 0x06006DAE RID: 28078 RVA: 0x002B411C File Offset: 0x002B231C
		protected override void OnSpawn()
		{
			if (this.saveLoadImmunities != null)
			{
				foreach (Effects.SaveLoadImmunities saveLoadImmunities in this.saveLoadImmunities)
				{
					if (Db.Get().effects.Exists(saveLoadImmunities.effectID))
					{
						Effect effect = Db.Get().effects.Get(saveLoadImmunities.effectID);
						this.AddImmunity(effect, saveLoadImmunities.giverID, true);
					}
				}
			}
			if (this.saveLoadEffects != null)
			{
				foreach (Effects.SaveLoadEffect saveLoadEffect in this.saveLoadEffects)
				{
					if (Db.Get().effects.Exists(saveLoadEffect.id))
					{
						Effect newEffect = Db.Get().effects.Get(saveLoadEffect.id);
						EffectInstance effectInstance = this.Add(newEffect, true);
						if (effectInstance != null)
						{
							effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
						}
					}
				}
			}
			if (this.effectsThatExpire.Count > 0)
			{
				SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
			}
		}

		// Token: 0x06006DAF RID: 28079 RVA: 0x002B4220 File Offset: 0x002B2420
		public EffectInstance Get(string effect_id)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect.Id == effect_id)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x06006DB0 RID: 28080 RVA: 0x002B4288 File Offset: 0x002B2488
		public EffectInstance Get(HashedString effect_id)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect.IdHash == effect_id)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x06006DB1 RID: 28081 RVA: 0x002B42F0 File Offset: 0x002B24F0
		public EffectInstance Get(Effect effect)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect == effect)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x06006DB2 RID: 28082 RVA: 0x002B434C File Offset: 0x002B254C
		public bool HasImmunityTo(Effect effect)
		{
			using (List<Effects.EffectImmunity>.Enumerator enumerator = this.effectImmunites.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect == effect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006DB3 RID: 28083 RVA: 0x002B43A8 File Offset: 0x002B25A8
		public EffectInstance Add(string effect_id, bool should_save)
		{
			Effect newEffect = Db.Get().effects.Get(effect_id);
			return this.Add(newEffect, should_save);
		}

		// Token: 0x06006DB4 RID: 28084 RVA: 0x002B43D0 File Offset: 0x002B25D0
		public EffectInstance Add(HashedString effect_id, bool should_save)
		{
			Effect newEffect = Db.Get().effects.Get(effect_id);
			return this.Add(newEffect, should_save);
		}

		// Token: 0x06006DB5 RID: 28085 RVA: 0x002B43F8 File Offset: 0x002B25F8
		public EffectInstance Add(Effect newEffect, bool should_save)
		{
			if (this.HasImmunityTo(newEffect))
			{
				return null;
			}
			Traits component = base.GetComponent<Traits>();
			if (component != null && component.IsEffectIgnored(newEffect))
			{
				return null;
			}
			Attributes attributes = this.GetAttributes();
			EffectInstance effectInstance = this.Get(newEffect);
			if (!string.IsNullOrEmpty(newEffect.stompGroup))
			{
				for (int i = this.effects.Count - 1; i >= 0; i--)
				{
					if (this.effects[i] != effectInstance && !(this.effects[i].effect.stompGroup != newEffect.stompGroup) && this.effects[i].effect.stompPriority > newEffect.stompPriority)
					{
						return null;
					}
				}
				for (int j = this.effects.Count - 1; j >= 0; j--)
				{
					if (this.effects[j] != effectInstance && !(this.effects[j].effect.stompGroup != newEffect.stompGroup) && this.effects[j].effect.stompPriority <= newEffect.stompPriority)
					{
						this.Remove(this.effects[j].effect);
					}
				}
			}
			if (effectInstance == null)
			{
				effectInstance = new EffectInstance(base.gameObject, newEffect, should_save);
				newEffect.AddTo(attributes);
				this.effects.Add(effectInstance);
				if (newEffect.duration > 0f)
				{
					this.effectsThatExpire.Add(effectInstance);
					if (this.effectsThatExpire.Count == 1)
					{
						SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
					}
				}
				base.Trigger(-1901442097, newEffect);
			}
			effectInstance.timeRemaining = newEffect.duration;
			return effectInstance;
		}

		// Token: 0x06006DB6 RID: 28086 RVA: 0x002B45B0 File Offset: 0x002B27B0
		public void Remove(Effect effect)
		{
			this.Remove(effect.IdHash);
		}

		// Token: 0x06006DB7 RID: 28087 RVA: 0x002B45C0 File Offset: 0x002B27C0
		public void Remove(HashedString effect_id)
		{
			int i = 0;
			while (i < this.effectsThatExpire.Count)
			{
				if (this.effectsThatExpire[i].effect.IdHash == effect_id)
				{
					int index = this.effectsThatExpire.Count - 1;
					this.effectsThatExpire[i] = this.effectsThatExpire[index];
					this.effectsThatExpire.RemoveAt(index);
					if (this.effectsThatExpire.Count == 0)
					{
						SimAndRenderScheduler.instance.Remove(this);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.effects.Count; j++)
			{
				if (this.effects[j].effect.IdHash == effect_id)
				{
					Attributes attributes = this.GetAttributes();
					EffectInstance effectInstance = this.effects[j];
					effectInstance.OnCleanUp();
					Effect effect = effectInstance.effect;
					effect.RemoveFrom(attributes);
					int index2 = this.effects.Count - 1;
					this.effects[j] = this.effects[index2];
					this.effects.RemoveAt(index2);
					base.Trigger(-1157678353, effect);
					return;
				}
			}
		}

		// Token: 0x06006DB8 RID: 28088 RVA: 0x002B46F4 File Offset: 0x002B28F4
		public void Remove(string effect_id)
		{
			int i = 0;
			while (i < this.effectsThatExpire.Count)
			{
				if (this.effectsThatExpire[i].effect.Id == effect_id)
				{
					int index = this.effectsThatExpire.Count - 1;
					this.effectsThatExpire[i] = this.effectsThatExpire[index];
					this.effectsThatExpire.RemoveAt(index);
					if (this.effectsThatExpire.Count == 0)
					{
						SimAndRenderScheduler.instance.Remove(this);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.effects.Count; j++)
			{
				if (this.effects[j].effect.Id == effect_id)
				{
					Attributes attributes = this.GetAttributes();
					EffectInstance effectInstance = this.effects[j];
					effectInstance.OnCleanUp();
					Effect effect = effectInstance.effect;
					effect.RemoveFrom(attributes);
					int index2 = this.effects.Count - 1;
					this.effects[j] = this.effects[index2];
					this.effects.RemoveAt(index2);
					base.Trigger(-1157678353, effect);
					return;
				}
			}
		}

		// Token: 0x06006DB9 RID: 28089 RVA: 0x002B4828 File Offset: 0x002B2A28
		public bool HasEffect(HashedString effect_id)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect.IdHash == effect_id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006DBA RID: 28090 RVA: 0x002B488C File Offset: 0x002B2A8C
		public bool HasEffect(string effect_id)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect.Id == effect_id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006DBB RID: 28091 RVA: 0x002B48F0 File Offset: 0x002B2AF0
		public bool HasEffect(Effect effect)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect == effect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006DBC RID: 28092 RVA: 0x002B494C File Offset: 0x002B2B4C
		public void Sim1000ms(float dt)
		{
			for (int i = 0; i < this.effectsThatExpire.Count; i++)
			{
				EffectInstance effectInstance = this.effectsThatExpire[i];
				if (effectInstance.IsExpired())
				{
					this.Remove(effectInstance.effect);
				}
				effectInstance.timeRemaining -= dt;
			}
		}

		// Token: 0x06006DBD RID: 28093 RVA: 0x002B49A0 File Offset: 0x002B2BA0
		public void AddImmunity(Effect effect, string giverID, bool shouldSave = true)
		{
			if (giverID != null)
			{
				foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
				{
					if (effectImmunity.giverID == giverID && effectImmunity.effect == effect)
					{
						return;
					}
				}
			}
			Effects.EffectImmunity item = new Effects.EffectImmunity(effect, giverID, shouldSave);
			this.effectImmunites.Add(item);
		}

		// Token: 0x06006DBE RID: 28094 RVA: 0x002B4A20 File Offset: 0x002B2C20
		public void RemoveImmunity(Effect effect, string ID)
		{
			Effects.EffectImmunity item = default(Effects.EffectImmunity);
			bool flag = false;
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				if (effectImmunity.effect == effect && (ID == null || ID == effectImmunity.giverID))
				{
					item = effectImmunity;
					flag = true;
				}
			}
			if (flag)
			{
				this.effectImmunites.Remove(item);
			}
		}

		// Token: 0x06006DBF RID: 28095 RVA: 0x002B4AA4 File Offset: 0x002B2CA4
		[OnSerializing]
		internal void OnSerializing()
		{
			List<Effects.SaveLoadEffect> list = new List<Effects.SaveLoadEffect>();
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.shouldSave)
				{
					Effects.SaveLoadEffect item = new Effects.SaveLoadEffect
					{
						id = effectInstance.effect.Id,
						timeRemaining = effectInstance.timeRemaining,
						saved = true
					};
					list.Add(item);
				}
			}
			this.saveLoadEffects = list.ToArray();
			List<Effects.SaveLoadImmunities> list2 = new List<Effects.SaveLoadImmunities>();
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				if (effectImmunity.shouldSave)
				{
					Effect effect = effectImmunity.effect;
					Effects.SaveLoadImmunities item2 = new Effects.SaveLoadImmunities
					{
						effectID = effect.Id,
						giverID = effectImmunity.giverID,
						saved = true
					};
					list2.Add(item2);
				}
			}
			this.saveLoadImmunities = list2.ToArray();
		}

		// Token: 0x06006DC0 RID: 28096 RVA: 0x002B4BE0 File Offset: 0x002B2DE0
		public List<Effects.SaveLoadImmunities> GetAllImmunitiesForSerialization()
		{
			List<Effects.SaveLoadImmunities> list = new List<Effects.SaveLoadImmunities>();
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				Effect effect = effectImmunity.effect;
				Effects.SaveLoadImmunities item = new Effects.SaveLoadImmunities
				{
					effectID = effect.Id,
					giverID = effectImmunity.giverID,
					saved = effectImmunity.shouldSave
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06006DC1 RID: 28097 RVA: 0x002B4C78 File Offset: 0x002B2E78
		public List<Effects.SaveLoadEffect> GetAllEffectsForSerialization()
		{
			List<Effects.SaveLoadEffect> list = new List<Effects.SaveLoadEffect>();
			foreach (EffectInstance effectInstance in this.effects)
			{
				Effects.SaveLoadEffect item = new Effects.SaveLoadEffect
				{
					id = effectInstance.effect.Id,
					timeRemaining = effectInstance.timeRemaining,
					saved = effectInstance.shouldSave
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06006DC2 RID: 28098 RVA: 0x002B4D0C File Offset: 0x002B2F0C
		public List<EffectInstance> GetTimeLimitedEffects()
		{
			return this.effectsThatExpire;
		}

		// Token: 0x06006DC3 RID: 28099 RVA: 0x002B4D14 File Offset: 0x002B2F14
		public void CopyEffects(Effects source)
		{
			foreach (EffectInstance effectInstance in source.effects)
			{
				this.Add(effectInstance.effect, effectInstance.shouldSave).timeRemaining = effectInstance.timeRemaining;
			}
			foreach (EffectInstance effectInstance2 in source.effectsThatExpire)
			{
				this.Add(effectInstance2.effect, effectInstance2.shouldSave).timeRemaining = effectInstance2.timeRemaining;
			}
		}

		// Token: 0x0400523B RID: 21051
		[Serialize]
		private Effects.SaveLoadEffect[] saveLoadEffects;

		// Token: 0x0400523C RID: 21052
		[Serialize]
		private Effects.SaveLoadImmunities[] saveLoadImmunities;

		// Token: 0x0400523D RID: 21053
		private List<EffectInstance> effects = new List<EffectInstance>();

		// Token: 0x0400523E RID: 21054
		private List<EffectInstance> effectsThatExpire = new List<EffectInstance>();

		// Token: 0x0400523F RID: 21055
		private List<Effects.EffectImmunity> effectImmunites = new List<Effects.EffectImmunity>();

		// Token: 0x02001F6C RID: 8044
		[Serializable]
		public struct EffectImmunity
		{
			// Token: 0x0600A261 RID: 41569 RVA: 0x003641BB File Offset: 0x003623BB
			public EffectImmunity(Effect e, string id, bool save = true)
			{
				this.giverID = id;
				this.effect = e;
				this.shouldSave = save;
			}

			// Token: 0x04008E0E RID: 36366
			public string giverID;

			// Token: 0x04008E0F RID: 36367
			public Effect effect;

			// Token: 0x04008E10 RID: 36368
			public bool shouldSave;
		}

		// Token: 0x02001F6D RID: 8045
		[Serializable]
		public struct SaveLoadImmunities
		{
			// Token: 0x04008E11 RID: 36369
			public string giverID;

			// Token: 0x04008E12 RID: 36370
			public string effectID;

			// Token: 0x04008E13 RID: 36371
			public bool saved;
		}

		// Token: 0x02001F6E RID: 8046
		[Serializable]
		public struct SaveLoadEffect
		{
			// Token: 0x04008E14 RID: 36372
			public string id;

			// Token: 0x04008E15 RID: 36373
			public float timeRemaining;

			// Token: 0x04008E16 RID: 36374
			public bool saved;
		}
	}
}
