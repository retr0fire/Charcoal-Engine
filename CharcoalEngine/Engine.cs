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

namespace CharcoalEngine
{
    public static class Engine
    {
        /// <summary>
        /// Starts the game engine and initiates all classes
        /// </summary>
        /// <param name="game">The game using the engine</param>
        public static void StartGameEngine(Game game)
        {
            Utilities.EngineContent.Content = new ContentManager(game.Content.ServiceProvider, "EngineContent");
            GameEngineData.game = game;
        }
    }
}
