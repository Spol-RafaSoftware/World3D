using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace World3D
{
    public class Srtm3PathManager
    {
        static string folder = @"C:\Public\WebGIS\WebGIS_SRTM3\";
        public static string Folder { get { return folder; } set { folder = value; } }

        public static string CreateFilename(int latitude, int longitude)
        {
            string file = "" + Path.DirectorySeparatorChar;
            if (latitude < 0)
                file += "S";
            else
                file += "N";
            file += Math.Abs(latitude).ToString("D2");
            if (longitude < 0)
                file += "W";
            else
                file += "E";
            file += Math.Abs(longitude).ToString("D3");
            file += ".hgt";

            return file;
        }
        
        public static string FindFilename(string filename)
        {
            foreach (string path in GetSearchPaths())
            {
                string ret = path + filename;
                if (File.Exists(ret))
                    return ret;
            }
            return null;
        }
        
        public static string[] GetSearchPaths()
        {
            if (!Directory.Exists(Folder))
                return new string[] { "" };
            List<string> paths = new List<string>();
            paths.Add(Folder);
            foreach (string path in Directory.GetDirectories(Folder))
                paths.Add(path);
            return paths.ToArray();
        }
        /*
        public static void CheckSearchPath()
        {
            bool valid = false;
            while (!valid)
            {
                while (!Directory.Exists(Folder))
                {
                    System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog()
                    {
                        Title = "Select an .hgt file from the WebGIS SRTM3 folder",
                        FileName = Folder,
                        Filter = "hgt files (*.hgt)|*.hgt|All files (*.*)|*.*"
                    };
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        DirectoryInfo dinfo = Directory.GetParent(ofd.FileName);
                        Folder = dinfo.FullName;
                    }
                    else return;
                }
                foreach (string path in GetSearchPaths())
                    foreach (string filename in Directory.GetFiles(Folder))
                    {
                        string ext = Path.GetExtension(filename).ToLower();
                        if (ext.Equals(".hgt"))
                        {
                            valid = true;
                            break;
                        }
                    }
                if (!valid)
                {
                    if (System.Windows.Forms.MessageBox.Show(
                        "Selected SRTM3 folder contains no *.hgt files",
                        "Invalid SRTM3 Folder",
                        System.Windows.Forms.MessageBoxButtons.RetryCancel, System.Windows.Forms.MessageBoxIcon.Warning)
                        == System.Windows.Forms.DialogResult.Cancel)
                    {
                        valid = true;
                    }
                    else
                        Folder = "";
                }
            }
        }*/
    }
}
