using System;
using System.Collections.Generic;
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
	/// Interaction logic for EpisodeButton.xaml
	/// </summary>
	public partial class EpisodeButton : Button
	{
		string m_SeriesName;
		int m_SeasonNumber;
		int m_EpisodeNumber;

		public string SeriesName
		{
			get { return m_SeriesName; }
			set { m_SeriesName = value; }
		}

		public int SeasonNumber
		{
			get { return m_SeasonNumber; }
			set { m_SeasonNumber = value; }
		}

		public int EpisodeNumber
		{
			get { return m_EpisodeNumber; }
			set { m_EpisodeNumber = value; }
		}
		public EpisodeButton()
		{
			InitializeComponent();
		}
	}
}
