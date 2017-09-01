using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PS4Macro.Classes
{
    public class SaveLoadHelper : INotifyPropertyChanged
    {
        public const string DEFAULT_FILE_NAME = "untitled.xml";
        private const string FILTER = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private string m_CurrentFile = null;
        public string CurrentFile
        {
            get { return m_CurrentFile; }
            private set
            {
                if (value != m_CurrentFile)
                {
                    m_CurrentFile = value;
                    NotifyPropertyChanged("CurrentFile");
                }
            }
        }
        #endregion

        private MacroPlayer m_MacroPlayer;

        /* Constructor */
        public SaveLoadHelper(MacroPlayer macroPlayer)
        {
            m_MacroPlayer = macroPlayer;
        }

        public void ClearCurrentFile()
        {
            CurrentFile = null;
        }

        public void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            openFileDialog.Filter = FILTER;
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load
                CurrentFile = openFileDialog.FileName;
                m_MacroPlayer.LoadFile(CurrentFile);
            }
        }

        public void Save()
        {
            // Currently has a file loaded
            if (!string.IsNullOrWhiteSpace(CurrentFile))
            {
                if (File.Exists(CurrentFile))
                {
                    // Save
                    m_MacroPlayer.SaveFile(CurrentFile);
                }
                else
                {
                    // Save as
                    SaveAs(DEFAULT_FILE_NAME);
                }
            }
            // No file loaded
            else
            {
                // Save as
                SaveAs(DEFAULT_FILE_NAME);
            }
        }

        public void SaveAs(string fileName = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            saveFileDialog.Filter = FILTER;
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = fileName == null ? Path.GetFileName(CurrentFile) : fileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save
                CurrentFile = saveFileDialog.FileName;
                m_MacroPlayer.SaveFile(CurrentFile);
            }
        }
    }
}
