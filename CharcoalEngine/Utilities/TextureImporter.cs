using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Design;
using System.Reflection;
using System.Diagnostics;
using CharcoalEngine.Scene;
using System.IO;
using System.Windows.Forms;

namespace CharcoalEngine.Utilities
{
    public static class TextureImporter
    {
        public static Texture2D LoadTextureFromFile(string FileName)
        {
            Texture2D Texture;
            //try to directly load image through XNA
            try
            {
                Texture = Texture2D.FromStream(Engine.Game.GraphicsDevice, new System.IO.StreamReader(FileName).BaseStream);
                return Texture;
            }
            catch
            {

                //if that doesn't work, try to convert it to a .png
                Image image;
                try
                {
                    image = Image.FromFile(FileName);
                }
                catch
                {
                    //if the image fails to load, return an error
                    return null;
                }

                //change the file ending
                for (int i = FileName.Length - 1; i > 0; i--)
                {
                    if (FileName[i] != '.')
                        FileName = FileName.Remove(FileName.Length - 1, 1);
                    else
                    {
                        FileName = FileName.Remove(FileName.Length - 1, 1);
                        break;
                    }
                }

                //save the file as a png
                FileName += ".png";
                try
                {
                    image.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    Console.WriteLine("Image save failed, possible file already exists...");
                }

                //try to load it again
                try
                {
                    Texture = Texture2D.FromStream(Engine.g, new System.IO.StreamReader(FileName).BaseStream);
                    return Texture;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}