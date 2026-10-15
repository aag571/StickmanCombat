// Decompile from assembly: Assembly-CSharp-firstpass.dll
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Analytics
{
	[Serializable]
	public class DriveableProperty
	{
		[Serializable]
		public class FieldWithRemoteSettingsKey
		{
			[SerializeField]
			private UnityEngine.Object m_Target;

			[SerializeField]
			private string m_FieldPath;

			[SerializeField]
			private string m_RSKeyName;

			[SerializeField]
			private string m_Type;

			public UnityEngine.Object target
			{
				get
				{
					return this.m_Target;
				}
				set
				{
					this.m_Target = value;
				}
			}

			public string fieldPath
			{
				get
				{
					return this.m_FieldPath;
				}
				set
				{
					this.m_FieldPath = value;
				}
			}

			public string rsKeyName
			{
				get
				{
					return this.m_RSKeyName;
				}
				set
				{
					this.m_RSKeyName = value;
				}
			}

			public string type
			{
				get
				{
					return this.m_Type;
				}
				set
				{
					this.m_Type = value;
				}
			}

			public object SetValueRecursive(object val, object target, string path)
			{
				if (path.Length == 0)
				{
					return val;
				}
				string[] array = path.Split(new char[]
				{
					'.'
				}, 2);
				string text = array[0];
				string text2 = (array.Length <= 1) ? string.Empty : array[1];
				Type type = target.GetType();
				FieldInfo field = type.GetField(text);
				PropertyInfo property = type.GetProperty(text);
				object[] index = null;
				object value;
				if (field != null)
				{
					value = field.GetValue(target);
				}
				else
				{
					if (property == null)
					{
						throw new InvalidOperationException(string.Format("Member '{0}' on target {1} is neither a field nor property", text, target));
					}
					ParameterInfo[] indexParameters = property.GetIndexParameters();
					if (indexParameters.GetLength(0) == 1 && indexParameters[0].ParameterType == typeof(int))
					{
						array = text2.Split(new char[]
						{
							'.'
						}, 2);
						int num;
						if (array[0] != null && int.TryParse(array[0], out num))
						{
							index = new object[]
							{
								num
							};
							text2 = ((array.Length <= 1) ? string.Empty : array[1]);
						}
					}
					value = property.GetValue(target, index);
				}
				object obj = this.SetValueRecursive(val, value, text2);
				if (obj != null)
				{
					if (field != null)
					{
						if (field.IsInitOnly)
						{
							UnityEngine.Debug.LogWarning("You probably shouldn't set a field on a readonly struct even though it works (sometimes)");
						}
						field.SetValue(target, obj);
						if (target.GetType().IsValueType)
						{
							return target;
						}
					}
					else
					{
						if (!property.CanWrite)
						{
							throw new InvalidOperationException(string.Format("Property '{0}' on target {1} is readonly", text, target));
						}
						property.SetValue(target, obj, index);
					}
				}
				return null;
			}

			public void SetValue(object val)
			{
				this.SetValueRecursive(val, this.m_Target, this.m_FieldPath);
			}
		}

		[SerializeField]
		private List<DriveableProperty.FieldWithRemoteSettingsKey> m_Fields;

		public List<DriveableProperty.FieldWithRemoteSettingsKey> fields
		{
			get
			{
				return this.m_Fields;
			}
			set
			{
				this.m_Fields = value;
			}
		}
	}
}
