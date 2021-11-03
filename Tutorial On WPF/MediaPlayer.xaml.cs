using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Timers;

namespace CustomMediaControls
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class MediaPlayer : UserControl
	{
		public MediaPlayer()
		{
			InitializeComponent();
			//InitEverything();
		}

		bool m_FadeInProgress;
		bool m_IsVideoPlaying;
		int m_TimerInterval;
		int m_ControlsOpacity;

		//Button playButton;
		Stopwatch m_Timer;
		DispatcherTimer m_DispatcherTimer;
		CancellationTokenSource m_TaskCancellationToken;

		public void InitMediaElement(string i_PathToEpisode, TimeSpan i_StartingPosition)
		{
			Player.Source = new Uri(i_PathToEpisode, UriKind.RelativeOrAbsolute);
			Player.Position = i_StartingPosition;
			//Player.Source = new Uri(@"C:\Users\Guy\Desktop\videos\chapi 2.mp4", UriKind.RelativeOrAbsolute);
			Player.MouseMove += MediaWindowMouseMoveHandler;
			Player.MouseMove += CancelToken;
			Player.MouseDown += CancelToken;
			Player.Play();
			m_IsVideoPlaying = true;
		}

		public void InitEverything(string i_PathToEpisode, TimeSpan i_StartingPosition)
		{
			m_TaskCancellationToken = new CancellationTokenSource();
			InitMediaElement(i_PathToEpisode, i_StartingPosition);

			//m_Timer = new Stopwatch();

			ConstructNewCancellationToken();
			InitDispatcherTimer();

			playButton.Click += buttonPlay_Click;
			playButton.Click += CancelToken;

			Player.MouseUp += mouse_Click;

		}

		private void mouse_Click(object sender, MouseButtonEventArgs e)
		{
			if (m_IsVideoPlaying)
			{
				Player.Pause();
			}

			else
			{
				Player.Play();
			}

			m_IsVideoPlaying = !m_IsVideoPlaying;
		}

		public void InitDispatcherTimer()
		{
			m_DispatcherTimer = new DispatcherTimer();
			m_DispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
			m_DispatcherTimer.Tick += DispatcherTimer_Tick;
			m_TimerInterval = 0;
			m_DispatcherTimer.Start();
		}

		public void MediaWindowMouseMoveHandler(object sender, EventArgs args)
		{
			if (m_IsVideoPlaying)
			{
				FadeInMediaControls();
				InitDispatcherTimer();
			}
		}

		public async void FadeInMediaControls()
		{
			m_FadeInProgress = true;
			int seconds = 1;
			playButton.Visibility = Visibility.Visible;
			while (m_ControlsOpacity++ < 100)
			{
				if (m_TaskCancellationToken.IsCancellationRequested)
				{
					m_FadeInProgress = false;
					return;
				}

				playButton.Opacity = (m_ControlsOpacity / 100d);
				await Task.Delay((int)(seconds));
			}
			m_FadeInProgress = false;
			m_DispatcherTimer.Start();
		}

		public async void FadeOutMediaControls()
		{
			m_FadeInProgress = true;
			int seconds = 1;

			while ((m_ControlsOpacity -= 2) >= 0)
			{
				if (m_TaskCancellationToken.IsCancellationRequested)
				{
					m_ControlsOpacity = 100;
					playButton.Opacity = 1;
					m_FadeInProgress = false;
					return;
				}

				playButton.Opacity = (m_ControlsOpacity / 100d);
				await Task.Delay((int)(seconds));
			}
			m_FadeInProgress = false;
			playButton.Visibility = Visibility.Hidden;
		}

		public void CancelToken(object sender, EventArgs args)
		{
			m_TaskCancellationToken.Cancel();
			m_TaskCancellationToken.Dispose();
			m_TaskCancellationToken = new CancellationTokenSource();
		}

		public void ConstructNewCancellationToken()
		{
			m_TaskCancellationToken = new CancellationTokenSource();
		}

		public void DispatcherTimer_Tick(object sender, EventArgs args)
		{
			m_TimerInterval++;

			if (m_TimerInterval > 100 && m_IsVideoPlaying)
			{
				FadeOutMediaControls();
				InitDispatcherTimer();
			}
		}

		public void buttonPlay_Click(object sender, EventArgs args)
		{
			ConstructNewCancellationToken();
			if (m_IsVideoPlaying)
			{
				Player.Pause();
			}

			else
			{
				Player.Play();
			}

			m_IsVideoPlaying = !m_IsVideoPlaying;
		}
	
		public void MoveForwardInSeconds(TimeSpan i_TimeInSeconds)
		{
			Player.Position += i_TimeInSeconds;
		}

		public void MoveBackwardInSeconds(TimeSpan i_TimeInSeconds)
		{
			Player.Position -= i_TimeInSeconds;
		}

		public void SkipToEpisode(string i_Source)
		{
			Uri newSource = new Uri(i_Source, UriKind.RelativeOrAbsolute);
			Player.Source = newSource;
		}

		public void SkipToEpisode(Uri i_Source)
		{
			Player.Source = i_Source;
		}

		public bool IsPlaying()
		{
			return m_IsVideoPlaying;
		}
	
	}
}