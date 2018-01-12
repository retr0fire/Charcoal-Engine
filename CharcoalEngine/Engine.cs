using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CharcoalEngine
{
    public static class Engine
    {
        public static Game Game;
        public static GraphicsDevice g;
        public static ContentManager Content;
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public static Scene.Scene ActiveScene;

        /// <summary>
        /// Starts the game engine and initiates all classes
        /// </summary>
        /// <param name="game">The game using the engine</param>
        public static void StartGameEngine(Game game, ContentManager content, GraphicsDeviceManager gr)
        {
            Content = content;
            Game = game;
            g = Game.GraphicsDevice;
            graphics = gr;
            spriteBatch = new SpriteBatch(g);
        }
    }
}
