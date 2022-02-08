using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;


namespace GraphVisual
{
    class CSV: IDisposable
    {
        Regex pattern = new Regex("([^,]+),?");
        StreamReader file;
        string currentLine;

        public CSV(string nameFile)
        {
            if (File.Exists(nameFile))
            {
                file = new StreamReader(nameFile);
            }
        }

        ~CSV()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (file!=null)
            {
                file.Close();
                file.Dispose();
                file = null;
            }
        }

        public void Next()
        {
            if (file != null)
            {
                currentLine = file.ReadLine();
            }
        }

        public string[] getValues()
        {
            string[] array=null;
            if (currentLine == null) this.Next();

            if (currentLine != null)
            {
                MatchCollection matches = pattern.Matches(currentLine);
                array = new string[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                    array[i] = matches[i].Groups[1].Value.ToString();
            }
            return array;
        }
    }
}
