// Decompile from assembly: Assembly-CSharp-firstpass.dll
using System;

namespace UnityEngine.Analytics
{
	[AddComponentMenu("Analytics/RemoteSettings")]
	public class RemoteSettings : MonoBehaviour
	{
		[SerializeField]
		private DriveableProperty m_DriveableProperty = new DriveableProperty();

		public DriveableProperty driveableProperty
		{
			get
			{
				return this.m_DriveableProperty;
			}
			set
			{
				this.m_DriveableProperty = value;
			}
		}

		private void Start()
		{
			this.RemoteSettingsUpdated();
			UnityEngine.RemoteSettings.Updated += new UnityEngine.RemoteSettings.UpdatedEventHandler(this.RemoteSettingsUpdated);
		}

		private void RemoteSettingsUpdated()
		{
			for (int i = 0; i < this.m_DriveableProperty.fields.Count; i++)
			{
				DriveableProperty.FieldWithRemoteSettingsKey fieldWithRemoteSettingsKey = this.m_DriveableProperty.fields[i];
				if (!string.IsNullOrEmpty(fieldWithRemoteSettingsKey.rsKeyName) && UnityEngine.RemoteSettings.HasKey(fieldWithRemoteSettingsKey.rsKeyName) && fieldWithRemoteSettingsKey.target != null && !string.IsNullOrEmpty(fieldWithRemoteSettingsKey.fieldPath))
				{
					if (fieldWithRemoteSettingsKey.type == "bool")
					{
						fieldWithRemoteSettingsKey.SetValue(UnityEngine.RemoteSettings.GetBool(fieldWithRemoteSettingsKey.rsKeyName));
					}
					else if (fieldWithRemoteSettingsKey.type == "float")
					{
						fieldWithRemoteSettingsKey.SetValue(UnityEngine.RemoteSettings.GetFloat(fieldWithRemoteSettingsKey.rsKeyName));
					}
					else if (fieldWithRemoteSettingsKey.type == "int")
					{
						fieldWithRemoteSettingsKey.SetValue(UnityEngine.RemoteSettings.GetInt(fieldWithRemoteSettingsKey.rsKeyName));
					}
					else if (fieldWithRemoteSettingsKey.type == "string")
					{
						fieldWithRemoteSettingsKey.SetValue(UnityEngine.RemoteSettings.GetString(fieldWithRemoteSettingsKey.rsKeyName));
					}
				}
			}
		}
	}
}
