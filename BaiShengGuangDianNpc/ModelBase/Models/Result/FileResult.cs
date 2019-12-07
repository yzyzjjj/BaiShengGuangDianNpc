using System.Collections.Generic;

namespace ModelBase.Models.Result
{
    public class FileResult : Result
    {
        public string data { get; set; }
    }
    public class FileResultMultiple : Result
    {
        public FileResultMultiple()
        {
            data = new List<UpFileInfo>();
        }
        public List<UpFileInfo> data { get; set; }
    }

    public class UpFileInfo
    {
        public UpFileInfo(string fileName, string newFileName)
        {
            oldName = fileName;
            newName = newFileName;
        }

        public string oldName { get; set; }
        public string newName { get; set; }
    }

    public class FileResultPath : Result
    {
        public FileResultPath()
        {
            data = new List<UpFilePath>();
        }
        public List<UpFilePath> data { get; set; }
    }
    public class UpFilePath
    {
        public UpFilePath(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        public string name { get; set; }
        public string path { get; set; }
    }
}
