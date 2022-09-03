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
		bool m_IsVideoPlaying;
		private TimeSpan m_TotalTime; // need to integrate this field with existing time span
		public bool IsVideoPlaying { get { return m_IsVideoPlaying; } }
		DispatcherTimer uiTimer;

		public MediaPlayer()
		{
			InitializeComponent();
			slider.AddHandler(MouseLeftButtonUpEvent,
					  new MouseButtonEventHandler(slider_MouseLeftButtonUp),
					  true);
			MouseMove += MediaPlayer_MouseMove;
			ToggleUI(Visibility.Hidden);
			uiTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2)};
		}

		private void MediaPlayer_MouseMove(object sender, MouseEventArgs e)
		{
			ToggleUI(Visibility.Visible);
			
			if(uiTimer.IsEnabled)
			{
				uiTimer.Stop();
			}
			
			uiTimer.Start();
			uiTimer.Tick += (objectSender, args) =>
			{
				ToggleUI(Visibility.Hidden);
			};
		}

		public void InitMediaElement(string i_PathToEpisode, TimeSpan i_StartingPosition)
		{
			Player.Source = new Uri(i_PathToEpisode, UriKind.RelativeOrAbsolute);
			EpisodeDetails.Text = formatEpisodeDetails(i_PathToEpisode);
			Player.Position = i_StartingPosition;
			Player.Play();
			m_IsVideoPlaying = true;
		}

		private string formatEpisodeDetails(string fullDetails)
		{
			string trimmed = fullDetails.Split('\\').LastOrDefault();
			//Season and episode details
			string[] allDetails = trimmed.Split('-');
			allDetails[0] = allDetails[0].Trim();
			allDetails[0]= allDetails[0].Replace('E', '.');
			allDetails[0] = allDetails[0].Replace("S", "");

			string[] seasonAndEpisode = allDetails[0].Split('.');
			
			//Capture season
			string season = $"Season {seasonAndEpisode[0]}";

			//Capture Episode
			string episode = $"Episode {seasonAndEpisode[1]}";

			//Capture title
			string title = allDetails[1].Trim();
			title = title.Substring(0, title.LastIndexOf('.'));
			return $"{season} {episode} - {title}";
		}
		public void InitEverything(string i_PathToEpisode, TimeSpan i_StartingPosition)
		{
			InitMediaElement(i_PathToEpisode, i_StartingPosition);
			playButton.Click += buttonPlay_Click;
			Player.MouseUp += mouse_Click;
		}

		private void slider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (m_TotalTime.TotalSeconds > 0)
			{
				Player.Position = TimeSpan.FromSeconds(slider.Value * m_TotalTime.TotalSeconds);
			}
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

		public void buttonPlay_Click(object sender, EventArgs args)
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
			EpisodeDetails.Text = formatEpisodeDetails(i_Source);
			Uri newSource = new Uri(i_Source, UriKind.RelativeOrAbsolute);
			Player.Source = newSource;
		}

		public void SkipToEpisode(Uri i_Source)
		{
			Player.Source = i_Source;
		}
	
		internal void stopPlayback()
    {
			Player.Close();
    }

		public bool IsPlaying()
		{
			return m_IsVideoPlaying;
		}

        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
			m_TotalTime = Player.NaturalDuration.TimeSpan;
			// Create a timer that will update the counters and the time slider
			DispatcherTimer timerVideoTime = new DispatcherTimer();
			timerVideoTime.Interval = TimeSpan.FromSeconds(1);
			timerVideoTime.Tick += new EventHandler(timer_Tick);
			timerVideoTime.Start();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			// Check if the movie finished calculate it's total time
			if(Player.NaturalDuration.HasTimeSpan)
			{
				if (Player.NaturalDuration.TimeSpan.TotalSeconds > 0)
				{
					if (m_TotalTime.TotalSeconds > 0)
					{
						// Updating time slider
						slider.Value = Player.Position.TotalSeconds /
										   m_TotalTime.TotalSeconds;
					}
				}
			}
		}

		public void ToggleUI(Visibility visibility)
		{
			ButtonPrevEpisode.Visibility = visibility;
			ButtonSkipBackward.Visibility = visibility;
			playButton.Visibility = visibility;
			ButtonSkipForward.Visibility = visibility;
			ButtonNextEpisode.Visibility = visibility;
			EpisodeDetails.Visibility = visibility;
		}
	}
}