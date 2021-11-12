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
	/// Interaction logic for Thumbnail.xaml
	/// </summary>
	public partial class Thumbnail : Button
	{
		string[] metaData;
		public Thumbnail()
		{
			InitializeComponent();
			VerticalContentAlignment = VerticalAlignment.Bottom;
		}

		public void InitThumbnail(string i_FolderPath)
		{
			string[] fileName = Directory.GetFiles(i_FolderPath, "*.meta");
			string[] thumbnailFolders = Directory.GetDirectories(i_FolderPath, "Thumbnails");

			string[] thumbnails = Directory.GetFiles(thumbnailFolders[0]);

			if (fileName.Length != 0)
			{
				metaData = File.ReadAllLines(fileName[0]);
				for (int i = 0; i < metaData.Length; i++)
				{
					int index = metaData[i].IndexOf(":");
					metaData[i] = metaData[i].Substring(index + 1);
				}
				Content = metaData[0];

				ImageBrush imageBrush = new ImageBrush();
				BitmapImage image = new BitmapImage(new Uri(thumbnails[0]));
				imageBrush.ImageSource = image;
				Background = imageBrush;

			}
		}
	}
}
