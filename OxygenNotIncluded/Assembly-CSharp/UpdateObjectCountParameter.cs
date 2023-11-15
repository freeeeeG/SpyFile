﻿using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x0200045B RID: 1115
internal class UpdateObjectCountParameter : LoopingSoundParameterUpdater
{
	// Token: 0x0600186B RID: 6251 RVA: 0x0007EABC File Offset: 0x0007CCBC
	public static UpdateObjectCountParameter.Settings GetSettings(HashedString path_hash, SoundDescription description)
	{
		UpdateObjectCountParameter.Settings settings = default(UpdateObjectCountParameter.Settings);
		if (!UpdateObjectCountParameter.settings.TryGetValue(path_hash, out settings))
		{
			settings = default(UpdateObjectCountParameter.Settings);
			EventDescription eventDescription = RuntimeManager.GetEventDescription(description.path);
			USER_PROPERTY user_PROPERTY;
			if (eventDescription.getUserProperty("minObj", out user_PROPERTY) == RESULT.OK)
			{
				settings.minObjects = (float)((short)user_PROPERTY.floatValue());
			}
			else
			{
				settings.minObjects = 1f;
			}
			USER_PROPERTY user_PROPERTY2;
			if (eventDescription.getUserProperty("maxObj", out user_PROPERTY2) == RESULT.OK)
			{
				settings.maxObjects = user_PROPERTY2.floatValue();
			}
			else
			{
				settings.maxObjects = 0f;
			}
			USER_PROPERTY user_PROPERTY3;
			if (eventDescription.getUserProperty("curveType", out user_PROPERTY3) == RESULT.OK && user_PROPERTY3.stringValue() == "exp")
			{
				settings.useExponentialCurve = true;
			}
			settings.parameterId = description.GetParameterId(UpdateObjectCountParameter.parameterHash);
			settings.path = path_hash;
			UpdateObjectCountParameter.settings[path_hash] = settings;
		}
		return settings;
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x0007EBA4 File Offset: 0x0007CDA4
	public static void ApplySettings(EventInstance ev, int count, UpdateObjectCountParameter.Settings settings)
	{
		float num = 0f;
		if (settings.maxObjects != settings.minObjects)
		{
			num = ((float)count - settings.minObjects) / (settings.maxObjects - settings.minObjects);
			num = Mathf.Clamp01(num);
		}
		if (settings.useExponentialCurve)
		{
			num *= num;
		}
		ev.setParameterByID(settings.parameterId, num, false);
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x0007EC00 File Offset: 0x0007CE00
	public UpdateObjectCountParameter() : base("objectCount")
	{
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x0007EC20 File Offset: 0x0007CE20
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
		UpdateObjectCountParameter.Entry item = new UpdateObjectCountParameter.Entry
		{
			ev = sound.ev,
			settings = settings
		};
		this.entries.Add(item);
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x0007EC6C File Offset: 0x0007CE6C
	public override void Update(float dt)
	{
		DictionaryPool<HashedString, int, LoopingSoundManager>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, int, LoopingSoundManager>.Allocate();
		foreach (UpdateObjectCountParameter.Entry entry in this.entries)
		{
			int num = 0;
			pooledDictionary.TryGetValue(entry.settings.path, out num);
			num = (pooledDictionary[entry.settings.path] = num + 1);
		}
		foreach (UpdateObjectCountParameter.Entry entry2 in this.entries)
		{
			int count = pooledDictionary[entry2.settings.path];
			UpdateObjectCountParameter.ApplySettings(entry2.ev, count, entry2.settings);
		}
		pooledDictionary.Recycle();
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x0007ED58 File Offset: 0x0007CF58
	public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x0007EDB0 File Offset: 0x0007CFB0
	public static void Clear()
	{
		UpdateObjectCountParameter.settings.Clear();
	}

	// Token: 0x04000D7C RID: 3452
	private List<UpdateObjectCountParameter.Entry> entries = new List<UpdateObjectCountParameter.Entry>();

	// Token: 0x04000D7D RID: 3453
	private static Dictionary<HashedString, UpdateObjectCountParameter.Settings> settings = new Dictionary<HashedString, UpdateObjectCountParameter.Settings>();

	// Token: 0x04000D7E RID: 3454
	private static readonly HashedString parameterHash = "objectCount";

	// Token: 0x020010DC RID: 4316
	private struct Entry
	{
		// Token: 0x04005A4B RID: 23115
		public EventInstance ev;

		// Token: 0x04005A4C RID: 23116
		public UpdateObjectCountParameter.Settings settings;
	}

	// Token: 0x020010DD RID: 4317
	public struct Settings
	{
		// Token: 0x04005A4D RID: 23117
		public HashedString path;

		// Token: 0x04005A4E RID: 23118
		public PARAMETER_ID parameterId;

		// Token: 0x04005A4F RID: 23119
		public float minObjects;

		// Token: 0x04005A50 RID: 23120
		public float maxObjects;

		// Token: 0x04005A51 RID: 23121
		public bool useExponentialCurve;
	}
}
