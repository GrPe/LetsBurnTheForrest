using ForestFireSimulator.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ForestFireSimulator.ViewModel
{
    public class Controller
    {
        Model.Data data;
        Model.Assets assets;
        Image sourceImage;

        public bool ParseData(string[] data)
        {
            if (data.Length != 4)
            {
                return false;
            }

            Regex regex = new Regex("^[a-zA-Z0-9_]+\\.[bmp]{1}");

            if (!regex.IsMatch(data[0]))
            {
                return false;
            }

            if (!int.TryParse(data[1], out int p1))
            {
                return false;
            }

            if (!int.TryParse(data[2], out int p2))
            {
                return false;
            }

            if (!int.TryParse(data[3], out int p3))
            {
                return false;
            }

            this.data = new Model.Data(data[0], p1, p2, p3);

            return true;
        }

        public IEnumerable<Image> StartSimulation()
        {
            LoadAssets();

            Bitmap outputImage = new Bitmap(sourceImage.Width * 15, sourceImage.Height * 15);
            Random random = new Random();
            var array = ProcessImage(sourceImage);

            while (true)
            {
                var copy = CopyTable(array);
                for (int i = 1; i < array.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < array.GetLength(1) - 1; j++)
                    {
                        if (copy[i, j] == State.Burn)
                        {
                            if (random.Next(0, 100) < data.RenewTreePropability)
                            {
                                array[i, j] = State.Tree;
                            }
                        }
                        else if (copy[i, j] == State.OnFire)
                        {
                            array[i, j] = State.Burn;
                        }
                        else if (copy[i, j] == State.None)
                        {
                            if (random.Next(0, 100) < data.RenewTreePropability)
                            {
                                array[i, j] = State.Tree;
                            }
                        }
                        else
                        {
                            if (NeighborOnFire(copy, i, j))
                            {
                                if (random.Next(0, 100) < data.InflammationFromNeighborPropability)
                                {
                                    array[i, j] = State.OnFire;
                                }
                            }
                            else
                            {
                                if (random.Next(0, 100) < data.SelfInflammationPropability)
                                {
                                    array[i, j] = State.OnFire;
                                }
                            }
                        }
                    }
                }
                

                using (Graphics graphics = Graphics.FromImage(outputImage))
                {
                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        for (int j = 0; j < array.GetLength(1); j++)
                        {
                            if (array[i, j] == State.Tree)
                            {
                                graphics.DrawImage(assets.Tree, new Point(i * 15, j * 15));
                            }
                            else if (array[i, j] == State.OnFire)
                            {
                                graphics.DrawImage(assets.OnFire, new Point(i * 15, j * 15));
                            }
                            else if (array[i, j] == State.Burn)
                            {
                                graphics.DrawImage(assets.Burn, new Point(i * 15, j * 15));
                            }
                            else
                            {
                                graphics.DrawImage(assets.Empty, new Point(i * 15, j * 15));
                            }
                        }
                    }
                    graphics.Save();
                }

                sourceImage = outputImage;
                yield return outputImage;
            }
        }

        public void StopSimulation()
        {

        }

        private bool LoadAssets()
        {
            Image empty, burn, onFire, tree;

            try
            {
                using (Bitmap bmp = new Bitmap("assets.png"))
                {
                    Rectangle clone = new Rectangle(0, 0, 15, 15);
                    empty = bmp.Clone(clone, bmp.PixelFormat);

                    clone = new Rectangle(0, 17, 15, 15);
                    tree = bmp.Clone(clone, bmp.PixelFormat);

                    clone = new Rectangle(357, 34, 15, 15);
                    burn = bmp.Clone(clone, bmp.PixelFormat);

                    clone = new Rectangle(255, 170, 15, 15);
                    onFire = bmp.Clone(clone, bmp.PixelFormat);
                }

                sourceImage = new Bitmap(data.FilePath);
            }
            catch
            {
                return false;
            }


            assets = new Model.Assets(empty, burn, onFire, tree);
            return true;
        }

        private static State[,] CopyTable(State[,] states)
        {
            State[,] ret = new State[states.GetLength(0), states.GetLength(1)];

            for (int i = 0; i < states.GetLength(0); i++)
            {
                for (int j = 0; j < states.GetLength(1); j++)
                {
                    ret[i, j] = states[i, j];
                }
            }

            return ret;
        }

        private static bool NeighborOnFire(State[,] array, int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (array[i, j] == State.OnFire) return true;
                }
            }
            return false;
        }

        private static State[,] ProcessImage(Image image)
        {
            Dictionary<Color, State> translate = new Dictionary<Color, State>()
            {
                {Color.FromArgb(0xff, 0xff, 0xff),State.None },
                {Color.FromArgb(0xff, 0, 0), State.OnFire },
                {Color.FromArgb(0, 0xff, 0), State.Tree },
                {Color.FromArgb(0, 0 ,0 ), State.Burn }
            };

            State[,] array = null;

            using (Bitmap bmp = new Bitmap(image))
            {
                array = new State[bmp.Width + 2, bmp.Height + 2];
                for (int i = 1; i < bmp.Height; i++)
                {
                    for (int j = 1; j < bmp.Width; j++)
                    {
                        Color color = bmp.GetPixel(j, i);
                        try
                        {
                            array[i, j] = translate[color];
                        }
                        catch
                        {
                            array[i, j] = State.None;
                        }
                    }
                }
            }

            return array;
        }
    }
}
