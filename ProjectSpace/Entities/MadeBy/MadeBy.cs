using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace.Entities.MadeBy
{
    public class MadeBy : Entity
    {
        private SpriteFont _font;
        string text;

        public MadeBy()
        {
            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");

            Coroutine routine = new Coroutine(this, Routine());
            routine.Start();
            text = "";
        }

        private IEnumerator<double> Routine()
        {
            AssetLibrary.GetAsset<SoundEffect>("sndAlien1").Play();
            yield return 1.0;
            text = "A GAME BY";
            yield return 2.0;
            text = "";
            yield return 0.5;
            AssetLibrary.GetAsset<SoundEffect>("sndAlien2").Play();
            yield return 1.0;
            text = "ERIC HONAS";
            yield return 2.5;
            text = "";
            yield return 1.5;
            Rooms.Goto(Rooms.RoomMenu);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 origin = _font.MeasureString(text) / 2.0f;
            spriteBatch.DrawString(_font, text, Camera.CameraSize / 2.0f, Color.White, 0.0f, origin, 2.0f, SpriteEffects.None, 0.0f);
            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
