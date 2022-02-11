using HonasGame;
using HonasGame.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace.Entities.Menu
{
    public class MenuTransition : Entity
    {
        private float _opacity;
        private Texture2D _tex;

        public MenuTransition()
        {
            _opacity = 0.0f;
            _tex = null;
        }

        protected override void Cleanup()
        {
            _tex.Dispose();
        }

        public override void Update(GameTime gameTime)
        {
            _opacity = MathHelper.SmoothStep(_opacity, 1.5f, 0.05f);

            if(_opacity >= 1.25f)
            {
                var menu = new Menu();
                Scene.Clear(menu);
                Scene.AddEntity(menu);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(_tex == null)
            {
                _tex = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _tex.SetData(new Color[] { Color.White });
            }

            spriteBatch.Draw(_tex, Camera.Bounds, Color.FromNonPremultiplied(0, 0, 0, (int)MathHelper.Clamp(_opacity*255, 0, 255)));

            base.Draw(gameTime, spriteBatch);
        }
    }
}
