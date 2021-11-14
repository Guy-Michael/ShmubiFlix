using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CustomMediaControls
{
	/// <summary>
	/// Interaction logic for PlayerPage.xaml
	/// </summary>
	public partial class PlayerPage : Page
	{
		public List<string> EpisodeList { get; set; }

		public int CurrentEpisodeNumber { get; set; }

		System.Timers.Timer metaStopWatch;
		Utils.SeasonMetaFile m_MetaFile;

		public PlayerPage(List<string> i_EpisodeList, int i_EpisodeNumber)
		{
			InitializeComponent();
			initAndStartStopwatch();


			initMetaFile(i_EpisodeList[0]);
			SetMedia(i_EpisodeList, i_EpisodeNumber);
			InitEventListeners();
			KeepAlive = false;
		}

		private void initMetaFile(string i_PathToEpisode1)
		{
			string pathToFolder = System.IO.Path.GetDirectoryName(i_PathToEpisode1);
			m_MetaFile = new Utils.SeasonMetaFile(pathToFolder);
		}

		private void initAndStartStopwatch()
		{
			metaStopWatch = new System.Timers.Timer();

			metaStopWatch.Interval = 5000;
			metaStopWatch.Start();
		}

		private void metaTimer_Elapsed(object sender, EventArgs args)
		{
			OnMetaTimerElapsed();
		}

		private void OnMetaTimerElapsed()
		{
			this.Dispatcher.Invoke(() =>
			{
				TimeSpan span = Player.Player.Position;
				string EpisodeName = System.IO.Path.GetFileName(EpisodeList[CurrentEpisodeNumber-1]);
				m_MetaFile.SetEpisodePosition(EpisodeName, span);
			});
		}

		public void player_DoubleClick(object sender, EventArgs args)
		{
			toggleFullScreen();
		}

		private void toggleFullScreen()
		{
			Window window = Window.GetWindow(this);


			if (window.WindowState == WindowState.Maximized)
			{
				fullScreenOff(window);
			}

			else
			{
				fullScreenOn(window);
			}
		}

		private static void fullScreenOn(Window window)
		{
			window.WindowStyle = WindowStyle.None;
			window.WindowState = WindowState.Maximized;
		}

		private static void fullScreenOff(Window window)
		{
			window.WindowStyle = WindowStyle.SingleBorderWindow;
			window.WindowState = WindowState.Normal;
		}

		public void InitEventListeners()
		{
			Player.ButtonPrevEpisode.Click += button_PreviousEpisode;
			Player.ButtonNextEpisode.Click += button_NextEpisode;
			Player.ButtonSkipBackward.Click += button_SkipBackward;
			Player.ButtonSkipForward.Click += button_SkipForward;
			Player.MouseDoubleClick += player_DoubleClick;
			metaStopWatch.Elapsed += metaTimer_Elapsed;
			Player.Player.MediaEnded += button_NextEpisode;

		}

		public void SetMedia(List<String> i_EpisodeList, int i_EpisodeNumber)
		{
			EpisodeList = i_EpisodeList;
			CurrentEpisodeNumber = i_EpisodeNumber;
			TimeSpan startingPosition;

			string episodeName = System.IO.Path.GetFileName(i_EpisodeList[i_EpisodeNumber - 1]);
			m_MetaFile.GetEpisodePosition(episodeName, out startingPosition);
			Player.InitEverything(EpisodeList[CurrentEpisodeNumber-1], startingPosition);
		}

		public void button_SkipForward(object sender, EventArgs args)
		{
			Player.MoveForwardInSeconds(new TimeSpan(0, 0, 15));
		}

		public void button_SkipBackward(object sender, EventArgs args)
		{
			Player.MoveBackwardInSeconds(new TimeSpan(0, 0, 15));
		}

		public void button_PreviousEpisode(object sender, EventArgs args)
		{
			if (CurrentEpisodeNumber - 1 < 0)
				return;

			Player.SkipToEpisode(EpisodeList[--CurrentEpisodeNumber]);
			UpdateLastPlayedEpisode(CurrentEpisodeNumber);
		}

		public void button_NextEpisode(object sender, EventArgs args)
		{
			if (CurrentEpisodeNumber + 1 > EpisodeList.Count)
				return;

			Player.SkipToEpisode(EpisodeList[++CurrentEpisodeNumber]);
			UpdateLastPlayedEpisode(CurrentEpisodeNumber);
		}

		public void UpdateLastPlayedEpisode(int i_EpisodeNumber)
		{
			string seasonPath = Path.GetDirectoryName(EpisodeList[0]);
			Utils.SeasonMetaFile.SetLastPlayedEpisode(seasonPath, i_EpisodeNumber);
		}
	}
}