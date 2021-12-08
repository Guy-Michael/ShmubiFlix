﻿using System;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			backButton.Click += BackButton_Click;
			MainFrame.Navigate(new BrowsePage());
			
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			navigateBack();
		}

		private void navigateBack()
		{
			try
			{
				if (MainFrame.NavigationService.Content.ToString().Contains("PlayerPage"))
                {
					PlayerPage playerPage = (PlayerPage) MainFrame.NavigationService.Content;
					playerPage.stopPlayback();
				}
				
				MainFrame.NavigationService.GoBack();
				//MainFrame.NavigationService.Refresh();
				Window window = Window.GetWindow(this);
				window.WindowStyle = WindowStyle.SingleBorderWindow;
				window.WindowState = WindowState.Normal;
			}
			catch (InvalidOperationException e)
			{
				Console.WriteLine("Attempted to navigate backwards with the stack was empty.");
			}
		}
	}
}
