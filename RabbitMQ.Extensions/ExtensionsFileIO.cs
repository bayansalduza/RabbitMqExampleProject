using RabbitMQ.Extensions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Extensions
{
    public class ExtensionsFileIO
    {
        public string Path { get; set; }
        public ExtensionsFileIO()
        {
            Path = @"C:\testdata.txt";
        }

        public void WriteFile(DataModel dataModel)
        {
            FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(dataModel.Data);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public DataModel ReadFile(string path)
        {
            DataModel dataModel = new DataModel();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs); 
            string yazi = sw.ReadLine();
            while (yazi != null)
            {
                dataModel.Data += yazi;
                yazi = sw.ReadLine();
            }
            sw.Close();
            fs.Close();
            return dataModel;
        }
    }
}
