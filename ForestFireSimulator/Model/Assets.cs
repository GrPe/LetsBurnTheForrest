using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestFireSimulator.Model
{
    public class Assets
    {
        public Image Empty { get; private set; }
        public Image Burn { get; private set; }
        public Image OnFire { get; private set; }
        public Image Tree { get; private set; }

        public Assets(Image empty, Image burn, Image onFire, Image tree)
        {
            Empty = empty;
            Burn = burn;
            OnFire = onFire;
            Tree = tree;
        }
    }
}
