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
	/// Interaction logic for BrowsePage.xaml
	/// </summary>
	public partial class BrowsePage : Page
	{
		private string m_FullPath;
		public BrowsePage()
		{
			m_FullPath = @"C:\Users\Guy\Documents\That70sGossipGirlShowInPrison";
			InitializeComponent();
			InitBrowswPage();
		}

		public void InitBrowswPage()
		{
			checkForSeriesMetaFiles();
			List<Thumbnail> thumbnails = new List<Thumbnail>();

			thumbnail1.Click += thumbnail_LoadSeries;
			thumbnails.Add(thumbnail1);

			thumbnail2.Click += thumbnail_LoadSeries;
			thumbnails.Add(thumbnail2);

			thumbnail3.Click += thumbnail_LoadSeries;
			thumbnails.Add(thumbnail3);

			//Choose root folder.
			IEnumerable<string> showNames = Directory.EnumerateDirectories(m_FullPath);
			List<string> list = showNames.ToList();
			
			for(int i = 0; i < thumbnails.Count; i++)
			{
				thumbnails[i].InitThumbnail(list[i]);
			}
		}

		public void thumbnail_LoadSeries(object sender, EventArgs args)
		{
			string name = (sender as Thumbnail).Content.ToString();
			SeasonsAndEpisodesPage page = new SeasonsAndEpisodesPage(name);
			NavigationService.GetNavigationService(this).Navigate(page);
		}

		private void checkForSeriesMetaFiles()
		{
			//capture each series folder and check it for meta data.
			string[] folders = Directory.GetDirectories(m_FullPath);
			foreach (string folderPath in folders)
			{
				Utils.SeriesMetaFile file = new Utils.SeriesMetaFile(folderPath);
				//string[] metaFile = Directory.GetFiles(folderPath, "*.meta");

				//if (metaFile.Length != 0)
				//{
				//	//Do nothing for now.
				//	//parseMetadata(metaFile[0]);
				//}

				//else
				//{
				//	MessageBox.Show("Meta file is absent in " + folderPath);
				//	generateMetadataForASeries(folderPath);

				//}
			}
		}

		private void generateMetadataForASeries(string i_PathToSeriesFolder)
		{
			string folderName = System.IO.Path.GetFileName(i_PathToSeriesFolder);
			string pathToMetaFile = System.IO.Path.Combine(i_PathToSeriesFolder, folderName + ".meta");


			StreamWriter metaStream = new StreamWriter(File.Create(pathToMetaFile));

			string titleString = "Title:" + folderName;
			string numberOfSeasonsString;

			int numberOfSeries = Directory.GetDirectories(i_PathToSeriesFolder).Length;

			numberOfSeasonsString = "Number-of-series:" + numberOfSeries.ToString();

			metaStream.WriteLine(titleString);
			metaStream.WriteLine(numberOfSeasonsString);
			metaStream.Close();
		}
	}
}
