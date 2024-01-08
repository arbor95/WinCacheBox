using System;
using System.Collections.Generic;
using System.IO;

namespace WinCachebox
{
    /// <summary>
    /// Ein Delegate für ein language changed event
    /// </summary>
    public delegate void languageChangedEventHandler();

    /// <summary>
    /// Eine Klasse zum verwalten von Strings aus unterschiedlichen Sprach Files.
    /// </summary>
    public class LangStrings
    {

        /// <summary>
        /// Eine Structure, welche die „ID“ als String und deren Text („Trans“) aufnimmt.
        /// </summary>
        public struct _Translations
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="ID">ID as String</param>
            /// <param name="Trans">Übersetzung</param>
            public _Translations(String ID, String Trans)
            {
                this.IdString = ID;
                this.Translation = Trans;
            }
            public String IdString;
            public String Translation;
        }

        public List<_Translations> _StringList = new List<_Translations>();
        private List<_Translations> _RefTranslation;

        #region Events
        /// <summary>
        /// Wird ausgelöst, wenn sich die geladene Sprach-Datei geändert hat. 
        /// </summary>
        public event languageChangedEventHandler LangChanged;
        #endregion

        #region Public

        /// <summary>
        /// Gibt den Namen der angegebenen Sprach-Datei zurück.
        /// </summary>
        /// <param name="FilePath">Voller Pfad zur Sprach Datei.</param>
        /// <returns>Name der Sprach-Datei</returns>
        public String getLangNameFromFile(String FilePath)
        {
            String ApplicationPath = System.IO.Path.GetDirectoryName
                    (System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            using (StreamReader sr = new StreamReader(Path.Combine(ApplicationPath, FilePath)))
            {
                return sr.ReadLine().Trim();
            }
        }

        /// <summary>
        /// Liest die angegebene Sprach-Datei ein.
        /// </summary>
        /// <param name="FilePath">Voller Pfad zur Sprach Datei.</param>
        public void ReadTranslationsFile(String FilePath)
        {
            if (String.IsNullOrEmpty(FilePath)) { return; }

            if (_RefTranslation == null)
            {
                int pos = FilePath.LastIndexOf("\\") + 1;
                string RefPath = FilePath.Remove(pos, FilePath.Length - pos) + "en.lan";
                _RefTranslation = ReadFile(RefPath);
            }
            if (FilePath.EndsWith("lang"))
                FilePath = FilePath.Replace(".lang", ".lan");
            _StringList = ReadFile(FilePath);

            if (LangChanged != null) { LangChanged(); } // Fire changed event if not null
        }

        private List<_Translations> ReadFile(string Path)
        {
            List<_Translations> Temp = new List<_Translations>();
            string line;

            using (StreamReader sr = new StreamReader(Path))
            {
                // Read and display lines from the file until the end of 
                // the file is reached:
                while ((line = sr.ReadLine()) != null)
                {
                    int pos;

                    //skip empty lines
                    if (line == "") { continue; }

                    //skip komment line
                    pos = line.IndexOf("//");
                    if (pos == 1) { continue; }

                    // skip line without value
                    pos = line.IndexOf("=");
                    if (pos == -1) { continue; }

                    string readID = line.Substring(0, pos - 1);
                    string readTransl = line.Substring(pos + 1);
                    string ReplacedRead = readTransl.Trim().Replace("\\n", Environment.NewLine);
                    if (ReplacedRead.StartsWith("\""))
                    {
                        ReplacedRead = ReplacedRead.Substring(1);
                    }
                    if (ReplacedRead.EndsWith("\""))
                    {
                        ReplacedRead = ReplacedRead.Substring(0, ReplacedRead.Length - 1);
                    }
                    if (ReplacedRead.EndsWith("+")) {
                        ReplacedRead = ReplacedRead.Substring(0, ReplacedRead.Length - 1) + " ";
                    }
                    Temp.Add(new _Translations(readID.Trim(), ReplacedRead));
                }
            }
            return Temp;
        }

        /// <summary>
        /// Gibt die Übersetzung der geladenen Sprach-Datei anhand der ID zurück.
        /// </summary>
        /// <param name="StringId">ID der Übersetzung</param>
        /// <returns>Übersetzung</returns>
        public String Get(String StringId)
        {
            String retString = Get(StringId, false);
            if (retString == String.Empty)
            {
                retString = StringId; // "No translation found";
            }
            return retString;
        }

        public String Get(String StringId, String DefaultId)
        {
            String retString = Get(StringId, false);
            if (retString == String.Empty)
            {
                retString = DefaultId; // "No translation found";
            }
            return retString;
        }


        public String Get(String StringId, bool withoutRef)
        {
            String retString = String.Empty; // ;
            foreach (_Translations tmp in _StringList)
            {
                if (tmp.IdString == StringId)
                {
                    retString = tmp.Translation;
                    break;
                }
            }

            if (retString == String.Empty && !withoutRef)
            {
                foreach (_Translations tmp in _RefTranslation)
                {
                    if (tmp.IdString == StringId)
                    {
                        retString = tmp.Translation;
                        break;
                    }
                }
            }

            return retString;

        }

        #endregion
    }
}

