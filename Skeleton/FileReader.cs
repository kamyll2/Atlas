using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton
{
    class FileReader
    {
        public static FileReader instance;
        public String[] lines;
        public String fileUri = "..\\..\\bones_data.txt";

        public static FileReader getInstance()
        {
            if (instance == null)
            {
                instance = new FileReader();
            }
            return instance;
        }

        public void loadData()
        {
            lines = new String[100];
            using (var reader = new StreamReader(fileUri, Encoding.GetEncoding("Windows-1250")))
            {
                string line;
                int i = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lines[i] = line;
                    i++;
                    //Console.WriteLine(line);
                    // Do stuff with your line here, it will be called for each 
                    // line of text in your file.
                }
            }
        }

        private String getLineByName(String name)
        {
            foreach (String s in lines)
            {
                if (s.StartsWith(name))
                {
                    return s;
                }
            }
            return "";
        }

        public String getUrlFromBoneName(String name)
        {
            if (lines == null)
            {
                loadData();
            }
            String qwerty = getLineByName(name);
            int linkPos = qwerty.IndexOf("http");
            int length = qwerty.Length - linkPos;
            String url = qwerty.Substring(linkPos, length);

            return url;
        }

        public String getPLNameFromBoneName(String name)
        {
            if (lines == null)
            {
                loadData();
            }
            String qwerty = getLineByName(name);
            int spacePos = qwerty.IndexOf(" ");
            int linkPos = qwerty.IndexOf("http");
            String plName = qwerty.Substring(spacePos, linkPos - spacePos);

            return plName;
        }

    }
}
