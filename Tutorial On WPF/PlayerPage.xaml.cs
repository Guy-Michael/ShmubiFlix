using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

		public PlayerPage()
		{
			InitializeComponent();
			m_MetaFile = new Utils.SeasonMetaFile(@"C:\Users\Guy\Documents\That70sGossipGirlShowInPrison\That 70s Show\Season 1");
			//m_MetaFile = new Utils.SeasonMetaFile(@"C:\Users\Guy\Documents\That70sGossipGirlShowInPrison\That 70s Show\Season 1\Season 1.meta");
			metaStopWatch = new System.Timers.Timer();
			
			metaStopWatch.Interval = 5000;
			metaStopWatch.Start();

			InitEventListeners();
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

		public void InitEventListeners()
		{
			Player.ButtonPrevEpisode.Click += button_PreviousEpisode;
			Player.ButtonNextEpisode.Click += button_NextEpisode;
			Player.ButtonSkipBackward.Click += button_SkipBackward;
			Player.ButtonSkipForward.Click += button_SkipForward;
			metaStopWatch.Elapsed += metaTimer_Elapsed;

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
			Player.SkipToEpisode(EpisodeList[--CurrentEpisodeNumber]);
		}

		public void button_NextEpisode(object sender, EventArgs args)
		{
			Player.SkipToEpisode(EpisodeList[++CurrentEpisodeNumber]);
		}
	}
}
