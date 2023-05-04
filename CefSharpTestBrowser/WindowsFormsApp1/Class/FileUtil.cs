using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Class
{
    public static class FileUtil
    {
        public static void deleteFile(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    FileInfo f = new FileInfo(filename);
                    f.Delete();
                }
            }

            catch {

                return;
            }

        }
    }
}
