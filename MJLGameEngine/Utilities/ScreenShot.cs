using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MJLGameEngine.Utilities
{
    public static class ScreenShot
    {
        public static void Save_Image_To_OneDrive(RenderTarget2D shot)
        {
            System.DateTime time = System.DateTime.Now;
            System.IO.StreamWriter writer =  new System.IO.StreamWriter("C:/Users/Me/OneDrive/Pictures/Screenshots/" + time.Year + "-" + time.Month + "-" + time.Day + "-" + time.Hour + "-" + time.Minute + "-" + time.Second + "-" + time.Millisecond + ".jpeg", false);
            
            shot.SaveAsJpeg(writer.BaseStream, shot.Width, shot.Height);
            writer.Close();
        }
    }
}
