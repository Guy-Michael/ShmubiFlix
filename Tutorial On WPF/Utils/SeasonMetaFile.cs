using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomMediaControls.Utils
{
	class SeasonMetaFile
	{
		string m_PathToFolder;
		string m_PathToMetaFile;

		public SeasonMetaFile(string i_PathToFolder)
		{
			m_PathToFolder = i_PathToFolder;
			setPathToMetaFile();
			checkSeasonMetaData();
		}

		private void setPathToMetaFile()
		{
			string folderName = System.IO.Path.GetFileName(m_PathToFolder);
			string metaFileName = folderName + ".meta";
			m_PathToMetaFile = System.IO.Path.Combine(m_PathToFolder, metaFileName);
		}

		private void checkSeasonMetaData()
		{ 

			if (!File.Exists(m_PathToMetaFile))
			{
				generateSeasonMetadata();
			}
		}

		private void generateSeasonMetadata()
		{
			StreamWriter metaStream = new StreamWriter(File.Create(m_PathToMetaFile));

			//List all files and and generate a position for them
			List<string> episodeList = Directory.GetFiles(m_PathToFolder, "*.mp4").ToList();
			episodeList.Sort();

			string lastPlayedEpisodeString = "Last-played-episode:" + "E1";
			metaStream.WriteLine(lastPlayedEpisodeString);

			metaStream.WriteLine("*** Start Positions Section ***");
			foreach (string episode in episodeList)
			{
				string episodeName = Path.GetFileName(episode);
				string positionString = "00:00:00";
				string episodePositionString = episodeName + "-" + positionString;
				metaStream.WriteLine(episodePositionString);
			}
			metaStream.WriteLine("*** End Position Section ***");
			metaStream.Close();
		}

		public void SetLastPlayedEpisode(string i_NewEpisode)
		{
			List<string> lines = extractDataFromMetaFile(m_PathToMetaFile);

			string chosenLine = string.Empty;
			int lineIndex = 0;
			foreach(string line in lines)
			{
				if (line.Contains("Last-played-episode"))
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
			string oldEpisode = chosenLine.Substring(colonIndex + 1);

			chosenLine = chosenLine.Replace(oldEpisode, i_NewEpisode);

			lines[lineIndex] = chosenLine;
			File.WriteAllLines(m_PathToMetaFile, lines);


		}


		public bool GetEpisodePosition(string i_EpisodeName, out TimeSpan o_CurrentPosition)
		{
			List<string> metaFileData = extractDataFromMetaFile(m_PathToFolder);
			string chosenLine = string.Empty;
			foreach (string line in metaFileData)
			{
				if (line.Contains(i_EpisodeName))
				{
					chosenLine = line;
					break;
				}
			}

			if (chosenLine.Equals(string.Empty))
			{
				o_CurrentPosition = new TimeSpan(0, 0, 0);
				return false;
			}

			//getting the current time span in string format.
			o_CurrentPosition = extractPositionFromEpisodeData(chosenLine);
			return true;
		}

		public bool SetEpisodePosition(string i_EpisodeName, TimeSpan i_NewPosition)
		{
			List<string> metaFileData = extractDataFromMetaFile(m_PathToFolder);
			string chosenLine = string.Empty;
			int lineIndex = 0;


			foreach (string line in metaFileData)
			{
				if (line.Contains(i_EpisodeName))
				{
					chosenLine = line;
					break;
				}

				lineIndex++;
			}

			if (chosenLine.Equals(string.Empty))
			{
				return false;
			}

			string newEpisodeData = updatePositionInEpisodeData(chosenLine, i_NewPosition);

			metaFileData[lineIndex] = newEpisodeData;

			File.WriteAllLines(m_PathToMetaFile, metaFileData.ToArray());
			return true;
		}

		private List<string> extractDataFromMetaFile(string i_PathToFile)
		{
			List<string> list = File.ReadAllLines(m_PathToMetaFile).ToList();
			return list;
		}

		private TimeSpan extractPositionFromEpisodeData(string i_EpisodeData)
		{
			//This assumes episode data is of the form:
			//season number - episode number - episode name - episode position.

			int index = i_EpisodeData.LastIndexOf('-');
			string positionString = i_EpisodeData.Substring(index + 1);
			TimeSpan position = TimeSpan.Parse(positionString);
			return position;
		}

		private string updatePositionInEpisodeData(string i_EpisodeData, TimeSpan i_NewPosition)
		{
			int index = i_EpisodeData.LastIndexOf('-');
			
			string dataStringWithoutPosition = i_EpisodeData.Substring(0, index + 1);
			string newPositionString = i_NewPosition.ToString();
			string fullDataString = dataStringWithoutPosition + newPositionString;
			
			return fullDataString;
		}
	}


}
