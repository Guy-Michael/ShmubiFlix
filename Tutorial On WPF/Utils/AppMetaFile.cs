using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMediaControls.Utils
{
	class AppMetaFile
	{
		public static string GetPathToVideoFolder()
		{
			string path = getWorkingDirectoryPath();
			string[] metaFile = Directory.GetFiles(path, "*.meta");

			if (metaFile.Length != 0 )
			{
				string pathToVideoFolder = File.ReadAllLines(metaFile[0])[0];
				return pathToVideoFolder;
			}

			return string.Empty;
		}

		public static bool Exists()
		{
			string folderPath = Directory.GetCurrentDirectory();
			string[] metaFiles = Directory.GetFiles(folderPath, "*.meta");

			return (metaFiles.Length != 0);
		}
		public static void SetPathToVideoFolder(string i_PathToVideoFolder)
		{
			string path = getWorkingDirectoryPath();
			string[] metaFile = Directory.GetFiles(path, "*.meta");

			if (metaFile.Length != 0)
			{
				File.WriteAllText(metaFile[0], i_PathToVideoFolder);
			}

			else
			{
				string metaFileName = Path.GetFileName(path);
				string metaFilePath = Path.Combine(path, metaFileName + ".meta");
				//File.Create(metaFilePath);

				File.WriteAllText(metaFilePath, i_PathToVideoFolder);
			}
		}

		private static string getWorkingDirectoryPath()
		{
			return Directory.GetCurrentDirectory();
		}
	}
}
