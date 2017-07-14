using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spire.Doc;
using Spire.Doc.Collections;

namespace UtilityCode
{
    public static class WordUtility
    {
        public static TableCollection GetRowsFromWord(string docPath)
        {
            Document document = new Document();
            document.LoadFromFile(docPath);
            return document.Sections[0].Tables;
        }
    }

}
