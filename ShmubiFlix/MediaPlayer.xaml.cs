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
using CustomMediaControls.delegates;

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
		}

		bool m_IsVideoPlaying;
		CancellationTokenSource m_TaskCancellationToken;

		public bool IsVideoPlaying { get { return m_IsVideoPlaying; } }

		public void InitMediaElement(string i_PathToEpisode, TimeSpan i_StartingPosition)
		{
			Player.Source = new Uri(i_PathToEpisode, UriKind.RelativeOrAbsolute);
			Player.Position = i_StartingPosition;
			Player.Play();
			m_IsVideoPlaying = true;
			
		}

		private void testc()
        {
            Console.WriteLine("test");
        }

		public void InitEverything(string i_PathToEpisode, TimeSpan i_StartingPosition)
		{
			m_TaskCancellationToken = new CancellationTokenSource();
			InitMediaElement(i_PathToEpisode, i_StartingPosition);
			playButton.Click += buttonPlay_Click;
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
	
		internal void stopPlayback()
        {
			Player.Close();
        }
	}
}