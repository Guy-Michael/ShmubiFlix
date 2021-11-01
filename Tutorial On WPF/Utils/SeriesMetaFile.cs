using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomMediaControls.Utils
{
	class SeriesMetaFile
	{
		string m_PathToMetaFile;
		string m_PathToFolder;

		public SeriesMetaFile(string i_PathToFolder)
		{
			m_PathToFolder = i_PathToFolder;
			string folderName = Path.GetFileName(m_PathToFolder);
			string metaFileName = folderName + ".meta";
			m_PathToMetaFile = Path.Combine(m_PathToFolder, metaFileName);

			MessageBox.Show("Path to meta: " + m_PathToMetaFile);
			checkSeriesMetaData();
		}

		private void checkSeriesMetaData()
		{
			//string directoryPath = Path.GetDirectoryName(m_PathToMetaFile);
			//string[] metaFile = Directory.GetFiles(directoryPath, "*.meta");
			
			if (!File.Exists(m_PathToMetaFile))
			{
				generateSeriesMetaData();
			}
		}

		private void generateSeriesMetaData()
		{
			//MessageBox.Show("generating the metafile.");
			//string folderName = System.IO.Path.GetDirectoryName(m_PathToMetaFile);
			//string pathToMetaFile = System.IO.Path.Combine(m_PathToMetaFile, folderName + ".meta");

			//MessageBox.Show("Path to current file is:" + pathToMetaFile);

			StreamWriter metaStream = new StreamWriter(File.Create(m_PathToMetaFile));

			string titleString = "Title:" + Path.GetFileName(m_PathToFolder);
			string numberOfSeasonsString;

			int numberOfSeries = Directory.GetDirectories(m_PathToFolder).Length;

			numberOfSeasonsString = "Number-of-seasons:" + numberOfSeries.ToString();

			metaStream.WriteLine(titleString);
			metaStream.WriteLine(numberOfSeasonsString);
			metaStream.Close();
		}
	}
}
