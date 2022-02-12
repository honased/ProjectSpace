using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace.Entities
{
    public class ScoreCounter : Entity
    {
        public int Score { get; private set; }
        private float _scale;
        private float _rotation;

        private SpriteFont _font;

        private const float BASE_SCALE = 2.0f;
        private float _opacity;

        public ScoreCounter()
        {
            Score = 0;
            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");
            _scale = BASE_SCALE;
            _rotation = 0.0f;
            _opacity = 0.0f;
        }

        public void AddScore(int scoreAmount)
        {
            Score += scoreAmount;
            _scale = BASE_SCALE + 1.0f;
            _rotation = (Globals.Random.Next(0, 10) < 5) ? 0.3f : -0.3f;
        }

        public override void Update(GameTime gameTime)
        {
            _scale = MathHelper.Lerp(_scale, BASE_SCALE, 0.2f);
            _rotation = MathHelper.Lerp(_rotation, 0.0f, 0.2f);
            _opacity = MathHelper.Lerp(_opacity, 1.0f, 0.1f);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            string scoreStr = Score.ToString();
            Vector2 origin = _font.MeasureString(scoreStr) / 2.0f;
            spriteBatch.DrawString(_font, scoreStr, new Vector2(Camera.CameraSize.X / 2.0f, 24), Color.FromNonPremultiplied(255, 255, 255, (int)(_opacity * 255)), _rotation, origin, _scale, SpriteEffects.None, 0.0f);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
