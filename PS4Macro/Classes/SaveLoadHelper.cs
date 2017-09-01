using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PS4Macro
{
    public class SaveLoadHelper
    {
        private const string FILTER = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        private MacroPlayer m_MacroPlayer;

        public string CurrentFile { get; private set; }

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
            if (!string.IsNullOrWhiteSpace(CurrentFile))
            {
                // Save
                m_MacroPlayer.SaveFile(CurrentFile);
            }
            else
            {
                SaveAs();
            }
        }

        public void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            saveFileDialog.Filter = FILTER;
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save
                CurrentFile = saveFileDialog.FileName;
                m_MacroPlayer.SaveFile(CurrentFile);
            }
        }
    }
}
