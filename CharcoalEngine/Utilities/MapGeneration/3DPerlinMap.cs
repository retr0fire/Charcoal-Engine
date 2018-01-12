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
using System.IO;
using CharcoalEngine.Scene;
using CharcoalEngine;
using CharcoalEngine.Utilities;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows;
using System.Design;

namespace CharcoalEngine.Utilities.MapGeneration
{
    public static class _3DPerlinMap
    {
        public static float[,,] Create_3D_Perlin_Map_W_Octaves(int size, Random r, int octaves)
        {

            float[][,,] octave_heights = new float[octaves][,,];

            float[,,] final_heights = new float[size, size, size];

            for (int i = 0; i < octaves; i++)
            {
                octave_heights[i] = Create_3D_Perlin_Map(size, (int)Math.Pow(2, i), r);
            }

            for (int i = 0; i < octaves; i++)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        for (int z = 0; z < size; z++)
                        {
                            final_heights[x, y, z] += octave_heights[i][x, y, z];
                        }
                    }
                }
            }
            return final_heights;
        }

        public static float[,,] Create_3D_Perlin_Map(int size, int frequency, Random r)
        {
            float amplitude = 1.0f / (float)frequency;
            int divisor = size / (frequency);

            float[,,] heights = new float[size, size, size];

            Vector3[,,] gradients = new Vector3[size / divisor + 1, size / divisor + 1, size / divisor + 1];
            Vector3[,,] distance = new Vector3[size, size, size];

            for (int i = 0; i < size / divisor; i++)
            {
                for (int j = 0; j < size / divisor; j++)
                {
                    for (int k = 0; k < size / divisor; k++)
                    {
                        gradients[i, j, k] = new Vector3((float)r.Next(-10000, 10000) / 10000.0f, (float)r.Next(-10000, 10000) / 10000.0f, (float)r.Next(-10000, 10000) / 10000.0f);
                        gradients[i, j, k].Normalize();
                        //Text.Add(new TextPoint(new Vector2(i, j)*divisor, gradients[i, j].ToString()));
                    }
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        distance[i, j, k] = new Vector3(i - ((int)(i / divisor) * divisor), 
                                                        j - ((int)(j / divisor) * divisor), 
                                                        k - ((int)(k / divisor) * divisor)
                                                        ) / divisor;
                        
                    }
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        Vector3[,,] Dist = new Vector3[2,2,2];

                        float[,,] Points = new float[2,2,2];

                        Vector3 position = distance[i, j, k];
                                    
                        for (int xc = 0; xc < 2; xc++)
                        {
                            for (int yc = 0; yc < 2; yc++)
                            {
                                for (int zc = 0; zc < 2; zc++)
                                {
                                    int x = i / divisor;
                                    int y = j / divisor;
                                    int z = k / divisor;

                                    x += xc;
                                    y += yc;
                                    z += zc;

                                    if (x >= size / divisor)
                                        x = 0;
                                    if (y >= size / divisor)
                                        y = 0;
                                    if (z >= size / divisor)
                                        z = 0;

                                    Dist[xc,yc,zc] = new Vector3(xc, yc, zc);
                                    Points[xc,yc,zc] = Vector3.Dot(distance[i, j, k] - Dist[xc,yc,zc], gradients[x, y, z]);
                                }
                            }
                        }

                        float LerpSquare1 = Lerp(
                                                    Lerp(Points[0, 0, 0], Points[1, 0, 0], Fade(position.X)),
                                                    Lerp(Points[0, 1, 0], Points[1, 1, 0], Fade(position.X)),
                                                    Fade(position.Y)
                                                    );

                        float LerpSquare2 = Lerp(
                                                    Lerp(Points[0, 0, 1], Points[1, 0, 1], Fade(position.X)),
                                                    Lerp(Points[0, 1, 1], Points[1, 1, 1], Fade(position.X)),
                                                    Fade(position.Y)
                                                    );

                        float result = Lerp(LerpSquare1, LerpSquare2, Fade(position.Z));

                        heights[i, j, k] = result*amplitude;

                    }
                }
            }

            return heights;
        }

        public static float Fade(float t)
        {
            // Fade function as defined by Ken Perlin.  This eases coordinate values
            // so that they will ease towards integral values.  This ends up smoothing
            // the final output.
            return t * t * t * (t * (t * 6 - 15) + 10);         // 6t^5 - 15t^4 + 10t^3
        }

        public static float Lerp(float a, float b, float amount)
        {
            return MathHelper.Lerp(a, b, amount);
        }


    }
}
