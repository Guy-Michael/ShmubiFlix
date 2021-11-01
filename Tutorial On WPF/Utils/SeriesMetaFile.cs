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

		public SeriesMetaFile(string i_PathToMetaFile)
		{
			m_PathToMetaFile = i_PathToMetaFile;
			checkSeasonMetaData();
		}

		private void checkSeasonMetaData()
		{
			string directoryPath = Path.GetDirectoryName(m_PathToMetaFile);
			string[] metaFile = Directory.GetFiles(directoryPath, "*.meta");
			
			if (metaFile.Length == 0)
			{
				generateSeasonMetaData();
			}
		}

		private void generateSeasonMetaData()
		{
			MessageBox.Show("generating the metafile.");
			string folderName = System.IO.Path.GetDirectoryName(m_PathToMetaFile);
			string pathToMetaFile = System.IO.Path.Combine(m_PathToMetaFile, folderName + ".meta");

			MessageBox.Show("Path to current file is:" + pathToMetaFile);

			StreamWriter metaStream = new StreamWriter(File.Create(pathToMetaFile));

			string titleString = "Title:" + folderName;
			string numberOfSeasonsString;

			int numberOfSeries = Directory.GetDirectories(m_PathToMetaFile).Length;

			numberOfSeasonsString = "Number-of-series:" + numberOfSeries.ToString();

			metaStream.WriteLine(titleString);
			metaStream.WriteLine(numberOfSeasonsString);
			metaStream.Close();
		}
	}
}
