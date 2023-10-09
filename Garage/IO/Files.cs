using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.IO
{
    public class Files : IFiles
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
