using CustomMediaControls.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace CustomMediaControls
{
	/// <summary>
	/// Interaction logic for SeasonsAndEpisodesPage.xaml
	/// </summary>
	public partial class SeasonsAndEpisodesPage : Page
	{
		private string m_FullPath;
		private string[] m_CurrentSeasonEpisodeList;

		public SeasonsAndEpisodesPage(string i_SeriesName)
		{
			InitializeComponent();
			m_FullPath = @"C:\Users\Guy\Documents\That70sGossipGirlShowInPrison";
			LoadSeries(i_SeriesName);
		}

		public void LoadSeries(string name)
		{
			m_FullPath = System.IO.Path.Combine(m_FullPath, name);
			string[] seasons = Directory.GetDirectories(m_FullPath);

			int seasonNumber = 1;
			foreach (string seasonPath in seasons)
			{
				Button seasonX = new Button();
				seasonX.Height = 30;
				seasonX.Content = Path.GetFileName(seasonPath);
				seasonX.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));
				seasonX.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
				seasonX.Click += button_Season_Pressed;
				SeasonStack.Children.Add(seasonX);
			}
		}

		public void LoadSeasonEpisodes(string i_SeasonName)
		{
			EpisodeStack.Children.Clear();

			string extensions = @"*.mkv|*.avi|*.mp4|*.mpeg|*.flv";
			string fullPath = System.IO.Path.Combine(m_FullPath, i_SeasonName);

			m_CurrentSeasonEpisodeList = Directory.GetFiles(fullPath, "*.mp4");
			
			int episodeNumber = 1;
			foreach (string episode in m_CurrentSeasonEpisodeList)
			{
				EpisodeButton EpisodeX = new EpisodeButton();
				EpisodeX.Height = 30;

				int index = episode.LastIndexOf(@"\");
				string currentEpisode = episode.Substring(index + 1);

				EpisodeX.EpisodeNumber = episodeNumber++;
				EpisodeX.Content = currentEpisode;
				EpisodeX.Background = new SolidColorBrush(Color.FromRgb(20, 20, 20));
				EpisodeX.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
				EpisodeX.Click += button_Episode_Click;
				EpisodeStack.Children.Add(EpisodeX);
			}
		}

		public void button_Season_Pressed(object sender, EventArgs args)
		{
			string seasonName = (sender as Button).Content.ToString();
			Utils.SeriesMetaFile file = new Utils.SeriesMetaFile(m_FullPath);
			file.SetLastPlayedSeason(seasonName);
			LoadSeasonEpisodes(seasonName);
		}

		public void button_Episode_Click(object sender, EventArgs args)
		{
			int episodeNumber = (sender as EpisodeButton).EpisodeNumber;
			List<string> episodeList = m_CurrentSeasonEpisodeList.ToList<string>();

			launchPlayerPage(episodeList, episodeNumber);
		}

		private void PlayLastButton_Click(object sender, RoutedEventArgs e)
		{
			Utils.SeriesMetaFile seriesMeta = new Utils.SeriesMetaFile(m_FullPath);

			string lastSeasonString = seriesMeta.GetLastSeasonPlayed();

			string seasonPath = Path.Combine(m_FullPath, lastSeasonString);
			int episodeNumber = Utils.SeasonMetaFile.GetLastPlayedEpisode(seasonPath);

			List<string> episodeList = Directory.GetFiles(seasonPath).ToList();

			launchPlayerPage(episodeList, episodeNumber);
		}

		private void launchPlayerPage(List<string> i_EpisodeList, int i_EpisodeNumber)
		{
			string pathToSeason = Path.GetDirectoryName(i_EpisodeList[0]);
			Utils.SeasonMetaFile file = new Utils.SeasonMetaFile(pathToSeason);
			file.SetLastPlayedEpisode("E" + i_EpisodeNumber);
			PlayerPage player = new PlayerPage(i_EpisodeList, i_EpisodeNumber);
			NavigationService.Navigate(player);
		}
	}
}