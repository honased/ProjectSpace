using HonasGame.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HonasGame.Rendering;
using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS.Components;
using System;

namespace ProjectSpace.Entities.Hangar
{
    public class Hangar : Entity
    {
        SpriteFont _font;
        SpriteRenderer _renderer;
        Transform2D _transform;

        public Hangar()
        {
            _transform = new Transform2D(this) { Position = Camera.CameraSize / 2.0f };
            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprHangarShip") };
            _renderer.Animation = "default";
            _renderer.CenterOrigin();
        }

        public override void Update(GameTime gameTime)
        {
            _transform.Position = (Camera.CameraSize / 2.0f) + Vector2.UnitY * ( MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds) * 5.0f );

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(new Rectangle(0, (int)(Camera.CameraSize.Y - 30), (int)(Camera.CameraSize.X), 30), Color.White, 2);
            spriteBatch.DrawString(_font, "UPGRADE", new Vector2(10, Camera.CameraSize.Y - 25), Color.White);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
