using Paloma;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDLevelLoader
{
    public class DoDLevel
    {
        private string name;
        private string habRelativePath;
        private Bitmap brefingImage;
        public string Name { get { return name; } }
        public string HabRelativePath { get { return habRelativePath; } }
        public Bitmap BrefingImage { get { return brefingImage; } }

        public DoDLevel(string name, string habRelativePath, string levelBaseFullPath, string levelFullPath)
        {
            this.name = name;
            this.habRelativePath = habRelativePath;
            string briefingImageFullPath = Path.Combine(levelFullPath, "briefing.tga");
            if (File.Exists(briefingImageFullPath))
            {
                brefingImage = TargaImage.LoadTargaImage(Path.Combine(levelFullPath, "briefing.tga"));
            }
            else
            {
                brefingImage = TargaImage.LoadTargaImage(Path.Combine(levelBaseFullPath, "default_briefing.tga"));
            }
        }
    }
}
