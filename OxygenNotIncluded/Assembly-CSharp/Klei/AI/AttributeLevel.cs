using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DD6 RID: 3542
	public class AttributeLevel
	{
		// Token: 0x06006D0B RID: 27915 RVA: 0x002AF534 File Offset: 0x002AD734
		public AttributeLevel(AttributeInstance attribute)
		{
			this.notification = new Notification(MISC.NOTIFICATIONS.LEVELUP.NAME, NotificationType.Good, new Func<List<Notification>, object, string>(AttributeLevel.OnLevelUpTooltip), null, true, 0f, null, null, null, true, false, false);
			this.attribute = attribute;
		}

		// Token: 0x06006D0C RID: 27916 RVA: 0x002AF57D File Offset: 0x002AD77D
		public int GetLevel()
		{
			return this.level;
		}

		// Token: 0x06006D0D RID: 27917 RVA: 0x002AF588 File Offset: 0x002AD788
		public void Apply(AttributeLevels levels)
		{
			Attributes attributes = levels.GetAttributes();
			if (this.modifier != null)
			{
				attributes.Remove(this.modifier);
				this.modifier = null;
			}
			this.modifier = new AttributeModifier(this.attribute.Id, (float)this.GetLevel(), DUPLICANTS.MODIFIERS.SKILLLEVEL.NAME, false, false, true);
			attributes.Add(this.modifier);
		}

		// Token: 0x06006D0E RID: 27918 RVA: 0x002AF5ED File Offset: 0x002AD7ED
		public void SetExperience(float experience)
		{
			this.experience = experience;
		}

		// Token: 0x06006D0F RID: 27919 RVA: 0x002AF5F6 File Offset: 0x002AD7F6
		public void SetLevel(int level)
		{
			this.level = level;
		}

		// Token: 0x06006D10 RID: 27920 RVA: 0x002AF600 File Offset: 0x002AD800
		public float GetExperienceForNextLevel()
		{
			float num = Mathf.Pow((float)this.level / (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600f;
			return Mathf.Pow(((float)this.level + 1f) / (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600f - num;
		}

		// Token: 0x06006D11 RID: 27921 RVA: 0x002AF660 File Offset: 0x002AD860
		public float GetPercentComplete()
		{
			return this.experience / this.GetExperienceForNextLevel();
		}

		// Token: 0x06006D12 RID: 27922 RVA: 0x002AF670 File Offset: 0x002AD870
		public void LevelUp(AttributeLevels levels)
		{
			this.level++;
			this.experience = 0f;
			this.Apply(levels);
			this.experience = 0f;
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, this.attribute.modifier.Name, levels.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			}
			levels.GetComponent<Notifier>().Add(this.notification, string.Format(MISC.NOTIFICATIONS.LEVELUP.SUFFIX, this.attribute.modifier.Name, this.level));
			StateMachine.Instance instance = new UpgradeFX.Instance(levels.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, -0.1f));
			ReportManager.Instance.ReportValue(ReportManager.ReportType.LevelUp, 1f, levels.GetProperName(), null);
			instance.StartSM();
			levels.Trigger(-110704193, this.attribute.Id);
		}

		// Token: 0x06006D13 RID: 27923 RVA: 0x002AF788 File Offset: 0x002AD988
		public bool AddExperience(AttributeLevels levels, float experience)
		{
			if (this.level >= DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL)
			{
				return false;
			}
			this.experience += experience;
			this.experience = Mathf.Max(0f, this.experience);
			if (this.experience >= this.GetExperienceForNextLevel())
			{
				this.LevelUp(levels);
				return true;
			}
			return false;
		}

		// Token: 0x06006D14 RID: 27924 RVA: 0x002AF7E0 File Offset: 0x002AD9E0
		private static string OnLevelUpTooltip(List<Notification> notifications, object data)
		{
			return MISC.NOTIFICATIONS.LEVELUP.TOOLTIP + notifications.ReduceMessages(false);
		}

		// Token: 0x040051D4 RID: 20948
		public float experience;

		// Token: 0x040051D5 RID: 20949
		public int level;

		// Token: 0x040051D6 RID: 20950
		public AttributeInstance attribute;

		// Token: 0x040051D7 RID: 20951
		public AttributeModifier modifier;

		// Token: 0x040051D8 RID: 20952
		public Notification notification;
	}
}
