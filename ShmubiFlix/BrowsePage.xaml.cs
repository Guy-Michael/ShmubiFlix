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
using System.Windows.Forms;

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
			//m_FullPath = @"C:\Users\Guy\Documents\That70sGossipGirlShowInPrison";
			InitializeComponent();
			//checkForAppMetaFile();
			InitBrowswPage();
		}

		public void InitBrowswPage()
		{
			if (!Utils.AppMetaFile.Exists())
				return;

			checkForAppMetaFile();

			checkForSeriesMetaFiles();


			List<Thumbnail> thumbnails = new List<Thumbnail>();

			string[] directories = Directory.GetDirectories(m_FullPath);

			for (int i = 0; i < directories.Length; i++)
			{
				Thumbnail nail = new Thumbnail();
				nail.Click += thumbnail_LoadSeries;
				thumbnails.Add(nail);
				Unigrid.Children.Add(nail);
			}
			
			//Choose root folder.
			List<string> list = Directory.EnumerateDirectories(m_FullPath).ToList();
			
			for(int i = 0; i < thumbnails.Count; i++)
			{
				thumbnails[i].InitThumbnail(list[i]);
			}
		}

		private void checkForAppMetaFile()
		{
			if (Utils.AppMetaFile.Exists())
			{
				m_FullPath = Utils.AppMetaFile.GetPathToVideoFolder();
			}
		}

		public void thumbnail_LoadSeries(object sender, EventArgs args)
		{
			string name = (sender as Thumbnail).Content.ToString();
			SeasonsAndEpisodesPage page = new SeasonsAndEpisodesPage(m_FullPath, name);
			NavigationService.GetNavigationService(this).Navigate(page);
		}

		private void checkForSeriesMetaFiles()
		{
			//capture each series folder and check it for meta data.
			string[] folders = Directory.GetDirectories(m_FullPath);
			foreach (string folderPath in folders)
			{
				Utils.SeriesMetaFile file = new Utils.SeriesMetaFile(folderPath);
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

		private void SetRootFolder_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				m_FullPath = dialog.SelectedPath;
				Utils.AppMetaFile.SetPathToVideoFolder(m_FullPath);

				InitBrowswPage();
			}
		}
	}
}
