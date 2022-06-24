using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UtilsEditor
{
    public static class StringList{
        public static List<string> Load(string path){
            List<string> list = new List<string>();
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            if(!textAsset) return null;
            
            TextReader textReader = new StringReader(textAsset.text);
            string line;

            while ((line = textReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                list.Add(line.TrimStart().TrimEnd());
            }
            return list;
        }
    }
}
