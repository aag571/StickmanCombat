// Decompile from assembly: Assembly-CSharp-firstpass.dll
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CodeStage.AntiCheat.Detectors
{
	[AddComponentMenu("Code Stage/Anti-Cheat Toolkit/Time Cheating Detector")]
	public class TimeCheatingDetector : ActDetectorBase
	{
		internal const string COMPONENT_NAME = "Time Cheating Detector";

		private const string FINAL_LOG_PREFIX = "[ACTk] Time Cheating Detector: ";

		private const string TIME_SERVER = "pool.ntp.org";

		private const int NTP_DATA_BUFFER_LENGTH = 48;

		private static int instancesInScene;

		private readonly DateTime date1900 = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		[Range(1f, 60f), Tooltip("Time (in minutes) between detector checks.")]
		public int interval = 1;

		[Tooltip("Maximum allowed difference between online and offline time, in minutes.")]
		public int threshold = 65;

		private Socket asyncSocket;

		private Action<double> getOnlineTimeCallback;

		private string targetHost;

		private byte[] targetIP;

		private IPEndPoint targetEndpoint;

		private byte[] ntpData = new byte[48];

		private SocketAsyncEventArgs connectArgs;

		private SocketAsyncEventArgs sendArgs;

		private SocketAsyncEventArgs receiveArgs;

		private static TimeCheatingDetector _Instance_k__BackingField;

		public static TimeCheatingDetector Instance
		{
			get;
			private set;
		}

		private static TimeCheatingDetector GetOrCreateInstance
		{
			get
			{
				if (TimeCheatingDetector.Instance != null)
				{
					return TimeCheatingDetector.Instance;
				}
				if (ActDetectorBase.detectorsContainer == null)
				{
					ActDetectorBase.detectorsContainer = new GameObject("Anti-Cheat Toolkit Detectors");
				}
				TimeCheatingDetector.Instance = ActDetectorBase.detectorsContainer.AddComponent<TimeCheatingDetector>();
				return TimeCheatingDetector.Instance;
			}
		}

		private TimeCheatingDetector()
		{
		}

		public static void StartDetection()
		{
			if (TimeCheatingDetector.Instance != null)
			{
				TimeCheatingDetector.Instance.StartDetectionInternal(null, TimeCheatingDetector.Instance.interval);
			}
			else
			{
				UnityEngine.Debug.LogError("[ACTk] Time Cheating Detector: can't be started since it doesn't exists in scene or not yet initialized!");
			}
		}

		public static void StartDetection(UnityAction callback)
		{
			TimeCheatingDetector.StartDetection(callback, TimeCheatingDetector.GetOrCreateInstance.interval);
		}

		public static void StartDetection(UnityAction callback, int interval)
		{
			TimeCheatingDetector.GetOrCreateInstance.StartDetectionInternal(callback, interval);
		}

		public static void StopDetection()
		{
			if (TimeCheatingDetector.Instance != null)
			{
				TimeCheatingDetector.Instance.StopDetectionInternal();
			}
		}

		public static void Dispose()
		{
			if (TimeCheatingDetector.Instance != null)
			{
				TimeCheatingDetector.Instance.DisposeInternal();
			}
		}

		private void Awake()
		{
			TimeCheatingDetector.instancesInScene++;
			if (this.Init(TimeCheatingDetector.Instance, "Time Cheating Detector"))
			{
				TimeCheatingDetector.Instance = this;
			}
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnLevelWasLoadedNew);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			TimeCheatingDetector.instancesInScene--;
		}

		private void OnLevelWasLoadedNew(Scene scene, LoadSceneMode mode)
		{
			this.OnLevelLoadedCallback();
		}

		private void OnLevelLoadedCallback()
		{
			if (TimeCheatingDetector.instancesInScene < 2)
			{
				if (!this.keepAlive)
				{
					this.DisposeInternal();
				}
			}
			else if (!this.keepAlive && TimeCheatingDetector.Instance != this)
			{
				this.DisposeInternal();
			}
		}

		private void StartDetectionInternal(UnityAction callback, int checkInterval)
		{
			if (this.isRunning)
			{
				UnityEngine.Debug.LogWarning("[ACTk] Time Cheating Detector: already running!", this);
				return;
			}
			if (!base.enabled)
			{
				UnityEngine.Debug.LogWarning("[ACTk] Time Cheating Detector: disabled but StartDetection still called from somewhere (see stack trace for this message)!", this);
				return;
			}
			if (callback != null && this.detectionEventHasListener)
			{
				UnityEngine.Debug.LogWarning("[ACTk] Time Cheating Detector: has properly configured Detection Event in the inspector, but still get started with Action callback. Both Action and Detection Event will be called on detection. Are you sure you wish to do this?", this);
			}
			if (callback == null && !this.detectionEventHasListener)
			{
				UnityEngine.Debug.LogWarning("[ACTk] Time Cheating Detector: was started without any callbacks. Please configure Detection Event in the inspector, or pass the callback Action to the StartDetection method.", this);
				base.enabled = false;
				return;
			}
			this.detectionAction = callback;
			this.interval = checkInterval;
			base.InvokeRepeating("CheckForCheat", 0f, (float)(this.interval * 60));
			this.started = true;
			this.isRunning = true;
		}

		protected override void StartDetectionAutomatically()
		{
			this.StartDetectionInternal(null, this.interval);
		}

		protected override void PauseDetector()
		{
			this.isRunning = false;
		}

		protected override void ResumeDetector()
		{
			if (this.detectionAction == null && !this.detectionEventHasListener)
			{
				return;
			}
			this.isRunning = true;
		}

		protected override void StopDetectionInternal()
		{
			if (!this.started)
			{
				return;
			}
			base.CancelInvoke("CheckForCheat");
			this.detectionAction = null;
			this.started = false;
			this.isRunning = false;
		}

		protected override void DisposeInternal()
		{
			base.DisposeInternal();
			if (TimeCheatingDetector.Instance == this)
			{
				TimeCheatingDetector.Instance = null;
			}
			if (this.asyncSocket.Connected)
			{
				this.asyncSocket.Close();
			}
		}

		private void CheckForCheat()
		{
			if (!this.isRunning)
			{
				return;
			}
			this.GetOnlineTimeAsync("pool.ntp.org", new Action<double>(this.OnTimeGot));
		}

		private void OnTimeGot(double onlineTime)
		{
			if (onlineTime <= 0.0)
			{
				UnityEngine.Debug.LogWarning("[ACTk] Time Cheating Detector: Can't retrieve time from time server!");
				return;
			}
			double localTime = this.GetLocalTime();
			TimeSpan timeSpan = new TimeSpan((long)onlineTime * 10000L);
			TimeSpan timeSpan2 = new TimeSpan((long)localTime * 10000L);
			double value = timeSpan.TotalMinutes - timeSpan2.TotalMinutes;
			if (Math.Abs(value) > (double)this.threshold)
			{
				this.OnCheatingDetected();
			}
		}

		public static double GetOnlineTime(string server)
		{
			double result;
			try
			{
				byte[] array = new byte[48];
				array[0] = 27;
				IPAddress[] addressList = Dns.GetHostEntry(server).AddressList;
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				socket.Connect(new IPEndPoint(addressList[0], 123));
				socket.ReceiveTimeout = 3000;
				socket.Send(array);
				socket.Receive(array);
				socket.Close();
				ulong num = (ulong)array[40] << 24 | (ulong)array[41] << 16 | (ulong)array[42] << 8 | (ulong)array[43];
				ulong num2 = (ulong)array[44] << 24 | (ulong)array[45] << 16 | (ulong)array[46] << 8 | (ulong)array[47];
				result = num * 1000.0 + num2 * 1000.0 / 4294967296.0;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"[ACTk] Time Cheating Detector: Could not get NTP time from ",
					server,
					" =/\n",
					ex
				}));
				result = -1.0;
			}
			return result;
		}

		public void GetOnlineTimeAsync(string server, Action<double> callback)
		{
			try
			{
				IPAddress[] addressList = Dns.GetHostEntry(server).AddressList;
				if (addressList.Length == 0)
				{
					UnityEngine.Debug.Log("[ACTk] Time Cheating Detector: Could not resolve IP from the host " + server + " =/");
					callback(-1.0);
				}
				else
				{
					if (this.asyncSocket == null)
					{
						this.asyncSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					}
					this.targetHost = server;
					IPAddress iPAddress = addressList[0];
					byte[] addressBytes = iPAddress.GetAddressBytes();
					if (addressBytes != this.targetIP)
					{
						this.targetEndpoint = new IPEndPoint(iPAddress, 123);
						this.targetIP = addressBytes;
					}
					if (this.connectArgs == null)
					{
						this.connectArgs = new SocketAsyncEventArgs();
						this.connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.OnSockedConnected);
					}
					this.connectArgs.RemoteEndPoint = this.targetEndpoint;
					this.asyncSocket.ReceiveTimeout = 3000;
					this.getOnlineTimeCallback = callback;
					this.asyncSocket.ConnectAsync(this.connectArgs);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"[ACTk] Time Cheating Detector: Could not get NTP time from ",
					server,
					" =/\n",
					ex
				}));
				callback(-1.0);
			}
		}

		private void OnSockedConnected(object sender, SocketAsyncEventArgs e)
		{
			if (e.SocketError != SocketError.Success)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"[ACTk] Time Cheating Detector: Could not get NTP time from ",
					this.targetHost,
					" =/\n",
					e
				}));
				if (this.getOnlineTimeCallback != null)
				{
					this.getOnlineTimeCallback(-1.0);
				}
				return;
			}
			if (!this.isRunning)
			{
				return;
			}
			this.ntpData[0] = 27;
			if (this.sendArgs == null)
			{
				this.sendArgs = new SocketAsyncEventArgs();
				this.sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.OnSocketSend);
				this.sendArgs.UserToken = this.asyncSocket;
				this.sendArgs.SetBuffer(this.ntpData, 0, 48);
			}
			this.sendArgs.RemoteEndPoint = this.targetEndpoint;
			this.asyncSocket.SendAsync(this.sendArgs);
		}

		private void OnSocketSend(object sender, SocketAsyncEventArgs e)
		{
			if (!this.isRunning)
			{
				return;
			}
			if (e.SocketError == SocketError.Success)
			{
				if (e.LastOperation == SocketAsyncOperation.Send)
				{
					if (this.receiveArgs == null)
					{
						this.receiveArgs = new SocketAsyncEventArgs();
						this.receiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.OnSocketReceive);
						this.receiveArgs.UserToken = this.asyncSocket;
						this.receiveArgs.SetBuffer(this.ntpData, 0, 48);
					}
					this.receiveArgs.RemoteEndPoint = this.targetEndpoint;
					this.asyncSocket.ReceiveAsync(this.receiveArgs);
				}
			}
			else
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"[ACTk] Time Cheating Detector: Could not get NTP time from ",
					this.targetHost,
					" =/\n",
					e
				}));
				if (this.getOnlineTimeCallback != null)
				{
					this.getOnlineTimeCallback(-1.0);
				}
			}
		}

		private void OnSocketReceive(object sender, SocketAsyncEventArgs e)
		{
			if (!this.isRunning)
			{
				return;
			}
			this.ntpData = e.Buffer;
			ulong num = (ulong)this.ntpData[40] << 24 | (ulong)this.ntpData[41] << 16 | (ulong)this.ntpData[42] << 8 | (ulong)this.ntpData[43];
			ulong num2 = (ulong)this.ntpData[44] << 24 | (ulong)this.ntpData[45] << 16 | (ulong)this.ntpData[46] << 8 | (ulong)this.ntpData[47];
			double obj = num * 1000.0 + num2 * 1000.0 / 4294967296.0;
			if (this.getOnlineTimeCallback != null)
			{
				this.getOnlineTimeCallback(obj);
			}
		}

		private double GetLocalTime()
		{
			return DateTime.UtcNow.Subtract(this.date1900).TotalMilliseconds;
		}
	}
}
