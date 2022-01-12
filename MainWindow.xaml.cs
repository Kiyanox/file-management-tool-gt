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
using System.Runtime.Serialization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.Xml;

namespace file_management_tool_gt
{
    [DataContract]
    public class File
    {
        [DataMember]
        public string filePath { get; set; }

        [DataMember]
        public string fileName { get; set; }

        public File(string filePath, string fileName)
        {
            this.filePath = filePath;
            this.fileName = fileName;
        }
    }

    public partial class MainWindow : Window
    {

        private String folderPath = "";
        private bool folderSelected = false;

        List<File> files = new List<File>();

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
                    File file = new File(Path.GetFileName(fileName), Path.GetFullPath(fileName));
                    files.Add(file);
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

        private void ZipItem(String filePath, String fileName)
        {
            string startPath = filePath;
            string zipPath = @".\" + fileName + ".zip";
            string folderPath = "";

            if (getIfFolderSelected())
            {
                folderPath = getFolderPath();
            }

            if(selectedFiles.SelectedItem != null)
            {
                ZipFile.CreateFromDirectory(startPath, zipPath);
            }
        }

        static void SaveViaDataContractSerialization<T>(T serializableObject, string filepath)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
            };
            var writer = XmlWriter.Create(filepath, settings);
            serializer.WriteObject(writer, serializableObject);
            writer.Close();
        }


        static T LoadViaDataContractSerialization<T>(string filepath)
        {
            var fileStream = new FileStream(filepath, FileMode.Open);
            var reader = XmlDictionaryReader.CreateTextReader(fileStream, new XmlDictionaryReaderQuotas());
            var serializer = new DataContractSerializer(typeof(T));
            T serializableObject = (T)serializer.ReadObject(reader, true);
            reader.Close();
            fileStream.Close();
            return serializableObject;
        }

    }


}
