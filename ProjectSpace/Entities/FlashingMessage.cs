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

namespace ProjectSpace.Entities
{
    public class FlashingMessage : Entity
    {
        private SoundEffect _snd;
        private string _text;
        private SpriteFont _font;
        private bool _visible;
        private bool _destroy;

        public FlashingMessage(string text, SoundEffect snd)
        {
            _text = text;
            _snd = snd;
            _visible = false;
            var routine = new Coroutine(this, Routine());
            routine.Start();

            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");
            _destroy = false;
        }

        private IEnumerator<double> Routine()
        {
            for(int i = 0; i < 3; i++)
            {
                yield return 0.5;
                if (i == 0) _snd?.Play();
                _visible = true;
                yield return 0.5;
                _visible = false;
            }

            _destroy = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(_destroy)
            {
                Destroy();
                return;
            }

            if (_visible)
            {
                Vector2 origin = _font.MeasureString(_text) / 2.0f;
                Vector2 position = new Vector2(Camera.CameraSize.X / 2.0f, Camera.CameraSize.Y / 3.0f);
                spriteBatch.DrawString(_font, _text, position, Color.White, 0.0f, origin, 2.0f, SpriteEffects.None, 0.0f);
            }

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
