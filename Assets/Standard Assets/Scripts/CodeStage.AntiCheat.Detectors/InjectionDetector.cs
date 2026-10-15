// Decompile from assembly: Assembly-CSharp-firstpass.dll
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CodeStage.AntiCheat.Detectors
{
	[AddComponentMenu("Code Stage/Anti-Cheat Toolkit/Injection Detector")]
	public class InjectionDetector : ActDetectorBase
	{
		private class AllowedAssembly
		{
			public readonly string name;

			public readonly int[] hashes;

			public AllowedAssembly(string name, int[] hashes)
			{
				this.name = name;
				this.hashes = hashes;
			}
		}

		internal const string COMPONENT_NAME = "Injection Detector";

		internal const string FINAL_LOG_PREFIX = "[ACTk] Injection Detector: ";

		protected UnityAction<string> detectionActionWithArgument;

		private static int instancesInScene;

		private bool signaturesAreNotGenuine;

		private InjectionDetector.AllowedAssembly[] allowedAssemblies;

		private string[] hexTable;

		private static InjectionDetector _Instance_k__BackingField;

		public static InjectionDetector Instance
		{
			get;
			private set;
		}

		private static InjectionDetector GetOrCreateInstance
		{
			get
			{
				if (InjectionDetector.Instance != null)
				{
					return InjectionDetector.Instance;
				}
				if (ActDetectorBase.detectorsContainer == null)
				{
					ActDetectorBase.detectorsContainer = new GameObject("Anti-Cheat Toolkit Detectors");
				}
				InjectionDetector.Instance = ActDetectorBase.detectorsContainer.AddComponent<InjectionDetector>();
				return InjectionDetector.Instance;
			}
		}

		private InjectionDetector()
		{
		}

		public static void StartDetection()
		{
			if (InjectionDetector.Instance != null)
			{
				InjectionDetector.Instance.StartDetectionInternal(null, null);
			}
			else
			{
				UnityEngine.Debug.LogError("[ACTk] Injection Detector: can't be started since it doesn't exists in scene or not yet initialized!");
			}
		}

		public static void StartDetection(UnityAction callback)
		{
			InjectionDetector.GetOrCreateInstance.StartDetectionInternal(callback, null);
		}

		public static void StartDetection(UnityAction<string> callback)
		{
			InjectionDetector.GetOrCreateInstance.StartDetectionInternal(null, callback);
		}

		public static void StopDetection()
		{
			if (InjectionDetector.Instance != null)
			{
				InjectionDetector.Instance.StopDetectionInternal();
			}
		}

		public static void Dispose()
		{
			if (InjectionDetector.Instance != null)
			{
				InjectionDetector.Instance.DisposeInternal();
			}
		}

		private void Awake()
		{
			InjectionDetector.instancesInScene++;
			if (this.Init(InjectionDetector.Instance, "Injection Detector"))
			{
				InjectionDetector.Instance = this;
			}
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnLevelWasLoadedNew);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			InjectionDetector.instancesInScene--;
		}

		private void OnLevelWasLoadedNew(Scene scene, LoadSceneMode mode)
		{
			this.OnLevelLoadedCallback();
		}

		private void OnLevelLoadedCallback()
		{
			if (InjectionDetector.instancesInScene < 2)
			{
				if (!this.keepAlive)
				{
					this.DisposeInternal();
				}
			}
			else if (!this.keepAlive && InjectionDetector.Instance != this)
			{
				this.DisposeInternal();
			}
		}

		private void StartDetectionInternal(UnityAction callback, UnityAction<string> callbackWithArgument)
		{
			if (this.isRunning)
			{
				UnityEngine.Debug.LogWarning("[ACTk] Injection Detector: already running!", this);
				return;
			}
			if (!base.enabled)
			{
				UnityEngine.Debug.LogWarning("[ACTk] Injection Detector: disabled but StartDetection still called from somewhere (see stack trace for this message)!", this);
				return;
			}
			if ((callback != null || callbackWithArgument != null) && this.detectionEventHasListener)
			{
				UnityEngine.Debug.LogWarning("[ACTk] Injection Detector: has properly configured Detection Event in the inspector, but still get started with Action callback. Both Action and Detection Event will be called on detection. Are you sure you wish to do this?", this);
			}
			if (callback == null && callbackWithArgument == null && !this.detectionEventHasListener)
			{
				UnityEngine.Debug.LogWarning("[ACTk] Injection Detector: was started without any callbacks. Please configure Detection Event in the inspector, or pass the callback Action to the StartDetection method.", this);
				base.enabled = false;
				return;
			}
			this.detectionAction = callback;
			this.detectionActionWithArgument = callbackWithArgument;
			this.started = true;
			this.isRunning = true;
			if (this.allowedAssemblies == null)
			{
				this.LoadAndParseAllowedAssemblies();
			}
			if (this.signaturesAreNotGenuine)
			{
				this.OnCheatingDetected("signatures");
				return;
			}
			string cause;
			if (!this.FindInjectionInCurrentAssemblies(out cause))
			{
				AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(this.OnNewAssemblyLoaded);
			}
			else
			{
				this.OnCheatingDetected(cause);
			}
		}

		protected override void StartDetectionAutomatically()
		{
			this.StartDetectionInternal(null, null);
		}

		protected override void PauseDetector()
		{
			this.isRunning = false;
			AppDomain.CurrentDomain.AssemblyLoad -= new AssemblyLoadEventHandler(this.OnNewAssemblyLoaded);
		}

		protected override void ResumeDetector()
		{
			if (this.detectionAction == null && this.detectionActionWithArgument == null && !this.detectionEventHasListener)
			{
				return;
			}
			this.isRunning = true;
			AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(this.OnNewAssemblyLoaded);
		}

		protected override void StopDetectionInternal()
		{
			if (!this.started)
			{
				return;
			}
			AppDomain.CurrentDomain.AssemblyLoad -= new AssemblyLoadEventHandler(this.OnNewAssemblyLoaded);
			this.detectionAction = null;
			this.detectionActionWithArgument = null;
			this.started = false;
			this.isRunning = false;
		}

		protected override void DisposeInternal()
		{
			base.DisposeInternal();
			if (InjectionDetector.Instance == this)
			{
				InjectionDetector.Instance = null;
			}
		}

		private void OnCheatingDetected(string cause)
		{
			if (this.detectionActionWithArgument != null)
			{
				this.detectionActionWithArgument(cause);
			}
			base.OnCheatingDetected();
		}

		private void OnNewAssemblyLoaded(object sender, AssemblyLoadEventArgs args)
		{
			if (!this.AssemblyAllowed(args.LoadedAssembly))
			{
				this.OnCheatingDetected(args.LoadedAssembly.FullName);
			}
		}

		private bool FindInjectionInCurrentAssemblies(out string cause)
		{
			cause = null;
			bool result = false;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			if (assemblies.Length == 0)
			{
				cause = "no assemblies";
				result = true;
			}
			else
			{
				Assembly[] array = assemblies;
				for (int i = 0; i < array.Length; i++)
				{
					Assembly assembly = array[i];
					if (!this.AssemblyAllowed(assembly))
					{
						cause = assembly.FullName;
						result = true;
						break;
					}
				}
			}
			return result;
		}

		private bool AssemblyAllowed(Assembly ass)
		{
			string name = ass.GetName().Name;
			int assemblyHash = this.GetAssemblyHash(ass);
			bool result = false;
			for (int i = 0; i < this.allowedAssemblies.Length; i++)
			{
				InjectionDetector.AllowedAssembly allowedAssembly = this.allowedAssemblies[i];
				if (allowedAssembly.name == name && Array.IndexOf<int>(allowedAssembly.hashes, assemblyHash) != -1)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		private void LoadAndParseAllowedAssemblies()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("fndid", typeof(TextAsset));
			if (textAsset == null)
			{
				this.signaturesAreNotGenuine = true;
				return;
			}
			string[] separator = new string[]
			{
				":"
			};
			MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			int num = binaryReader.ReadInt32();
			this.allowedAssemblies = new InjectionDetector.AllowedAssembly[num];
			for (int i = 0; i < num; i++)
			{
				string text = binaryReader.ReadString();
				text = ObscuredString.EncryptDecrypt(text, "Elina");
				string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				int num2 = array.Length;
				if (num2 <= 1)
				{
					this.signaturesAreNotGenuine = true;
					binaryReader.Close();
					memoryStream.Close();
					return;
				}
				string name = array[0];
				int[] array2 = new int[num2 - 1];
				for (int j = 1; j < num2; j++)
				{
					array2[j - 1] = int.Parse(array[j]);
				}
				this.allowedAssemblies[i] = new InjectionDetector.AllowedAssembly(name, array2);
			}
			binaryReader.Close();
			memoryStream.Close();
			Resources.UnloadAsset(textAsset);
			this.hexTable = new string[256];
			for (int k = 0; k < 256; k++)
			{
				this.hexTable[k] = k.ToString("x2");
			}
		}

		private int GetAssemblyHash(Assembly ass)
		{
			AssemblyName name = ass.GetName();
			byte[] publicKeyToken = name.GetPublicKeyToken();
			string text;
			if (publicKeyToken.Length >= 8)
			{
				text = name.Name + this.PublicKeyTokenToString(publicKeyToken);
			}
			else
			{
				text = name.Name;
			}
			int num = 0;
			int length = text.Length;
			for (int i = 0; i < length; i++)
			{
				num += (int)text[i];
				num += num << 10;
				num ^= num >> 6;
			}
			num += num << 3;
			num ^= num >> 11;
			return num + (num << 15);
		}

		private string PublicKeyTokenToString(byte[] bytes)
		{
			string text = string.Empty;
			for (int i = 0; i < 8; i++)
			{
				text += this.hexTable[(int)bytes[i]];
			}
			return text;
		}
	}
}
