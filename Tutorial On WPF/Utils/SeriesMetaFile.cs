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
			StreamWriter metaStream = new StreamWriter(File.Create(m_PathToMetaFile));

			string titleString = "Title:" + Path.GetFileName(m_PathToFolder);
			string numberOfSeasonsString;
			string lastPlayedSeasonString;
			int numberOfSeries = Directory.GetDirectories(m_PathToFolder).Length;

			numberOfSeasonsString = "Number-of-seasons:" + numberOfSeries.ToString();
			lastPlayedSeasonString = "Last-played-season:" + "Season 1";
			

			metaStream.WriteLine(titleString);
			metaStream.WriteLine(numberOfSeasonsString);
			metaStream.WriteLine(lastPlayedSeasonString);
			metaStream.Close();
		}

		public void SetLastPlayedSeason(string i_SeasonName)
		{			
			
			string[] lines = File.ReadAllLines(m_PathToMetaFile);
			string chosenLine = string.Empty;
			int lineIndex = 0;
			
			foreach(string line in lines)
			{
				if (line.Contains("Last-played-season"))
				{
					chosenLine = line;
					break;
				}

				lineIndex++;
			}


			if (chosenLine.Equals(string.Empty))
			{
				return;
			}

			int colonIndex = chosenLine.LastIndexOf(":");
			string oldSeason = chosenLine.Substring(colonIndex + 1);
			chosenLine = chosenLine.Replace(oldSeason, i_SeasonName);

			lines[lineIndex] = chosenLine;
			File.WriteAllLines(m_PathToMetaFile, lines);
		}
	}
}
