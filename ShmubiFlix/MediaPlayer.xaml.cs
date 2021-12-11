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
	// Adding a meaningful comment
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class MediaPlayer : UserControl
	{
		public MediaPlayer()
		{
			InitializeComponent();
			slider.AddHandler(MouseLeftButtonUpEvent,
					  new MouseButtonEventHandler(slider_MouseLeftButtonUp),
					  true);
		}

		bool m_IsVideoPlaying;
		CancellationTokenSource m_TaskCancellationToken;
		private TimeSpan TotalTime; // need to integrate this field with existing time span

		public void InitMediaElement(string i_PathToEpisode, TimeSpan i_StartingPosition)
		{
			Player.Source = new Uri(i_PathToEpisode, UriKind.RelativeOrAbsolute);
			Player.Position = i_StartingPosition;
			Player.Play();
			m_IsVideoPlaying = true;
		}

		public void InitEverything(string i_PathToEpisode, TimeSpan i_StartingPosition)
		{
			m_TaskCancellationToken = new CancellationTokenSource();
			InitMediaElement(i_PathToEpisode, i_StartingPosition);
			playButton.Click += buttonPlay_Click;
			Player.MouseUp += mouse_Click;
		}

		private void slider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (TotalTime.TotalSeconds > 0)
			{
				Player.Position = TimeSpan.FromSeconds(slider.Value * TotalTime.TotalSeconds);
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

        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
			TotalTime = Player.NaturalDuration.TimeSpan;

			// Create a timer that will update the counters and the time slider
			DispatcherTimer timerVideoTime = new DispatcherTimer();
			timerVideoTime.Interval = TimeSpan.FromSeconds(1);
			timerVideoTime.Tick += new EventHandler(timer_Tick);
			timerVideoTime.Start();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			// Check if the movie finished calculate it's total time
			if (Player.NaturalDuration.TimeSpan.TotalSeconds > 0)
			{
				if (TotalTime.TotalSeconds > 0)
				{
					// Updating time slider
					slider.Value = Player.Position.TotalSeconds /
									   TotalTime.TotalSeconds;
				}
			}
		}

		internal void setSliderVisibility(Visibility visibility)
		{
			slider.Visibility = visibility;
		}

		private void Grid_MouseEnter(object sender, MouseEventArgs e)
		{
			setSliderVisibility(Visibility.Visible);
		}

		private void Grid_MouseLeave(object sender, MouseEventArgs e)
		{
			setSliderVisibility(Visibility.Hidden);
		}
	}
}