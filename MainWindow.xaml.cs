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
using System.IO;
using System.IO.Compression;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;

namespace file_management_tool_gt
{
    public partial class MainWindow : Window
    {

        private String folderPath = "";
        private bool folderSelected = false;

        // Setter functions

       
        public void SetFolderPath(String path) { 
           this.folderPath = path;
        }

        public void SetFolderSelected(bool isFolderSelected)
        {
            this.folderSelected = isFolderSelected;
        }

        // Getter functions
        public String getFolderPath()
        {
            return this.folderPath;
        }
        public bool getIfFolderSelected()
        {
            return this.folderSelected;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void selectFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Jar Files: (.jar)|*.jar";
            if(openFileDialog.ShowDialog() == true)
            {
                foreach(string fileName in openFileDialog.FileNames)
                {
                    selectedFiles.Items.Add(Path.GetFileName(fileName));
                    
                }
            }
        }
        private void selectRootFolderPath(object sender, RoutedEventArgs e)
        {
            String actualFolderPath = selectFolder();

            if(actualFolderPath != "")
            {
                SetFolderSelected(true);
                SetFolderPath(actualFolderPath);
                RootFolderPathTextBox.Text = actualFolderPath;
            }
        }

        private static String selectFolder()
        {
            var ofd = new System.Windows.Forms.FolderBrowserDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return ofd.SelectedPath;
            } 
            else
            {
                return "";
            }
        }

        private void LbiSelected(object sender, RoutedEventArgs e)
        {
            /*MessageBox.Show("Test");*/
        }
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if(selectedFiles.SelectedItem != null)
            {
                selectedFiles.Items.Remove(selectedFiles.SelectedItem);
            } 
        }

    }
}
