using HonasGame.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HonasGame.Rendering;
using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS.Components;
using System;
using System.Collections.Generic;

namespace ProjectSpace.Entities.Hangar
{
    public class Hangar : Entity
    {
        private SpriteFont _font;
        private SpriteRenderer _renderer;
        private Transform2D _transform;
        private List<Tab> _tabs;

        private abstract class Tab
        {
            public Rectangle Bounds { get; set; }

            public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font);
        }

        private class BaseTab : Tab
        {
            public BaseTab()
            {
                Bounds = new Rectangle(0, (int)Camera.CameraSize.Y - 32, (int)Camera.CameraSize.X, 32);
            }

            public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
            {
                string[] options = new string[] { "UPGRADE SHIP", "TAKE FLIGHT", "RETREAT" };
                float increaseWidth = Bounds.Width / (float)(options.Length + 1);
                for(int i = 0; i < options.Length; i++)
                {
                    Vector2 origin = font.MeasureString(options[i]) / 2.0f;
                    spriteBatch.DrawString(font, options[i], new Vector2(increaseWidth * (i + 1), Bounds.Y + (Bounds.Height / 2.0f)), Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
                }
            }
        }

        public Hangar()
        {
            _transform = new Transform2D(this) { Position = Camera.CameraSize / 2.0f };
            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprHangarShip") };
            _renderer.Animation = "default";
            _renderer.CenterOrigin();

            _tabs = new List<Tab>();
            _tabs.Add(new BaseTab());
        }

        public override void Update(GameTime gameTime)
        {
            _transform.Position = (Camera.CameraSize / 2.0f) + Vector2.UnitY * ( MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds) * 5.0f );

            if(Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                Rooms.Goto(Rooms.RoomBattlefield);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw hangar lines
            spriteBatch.DrawLine(new Vector2(Camera.CameraSize.X / 3.0f + 10.0f, -10.0f), new Vector2(-10.0f, Camera.CameraSize.Y / 3.0f + 10.0f), Color.White, 2.0f);
            spriteBatch.DrawLine(new Vector2(Camera.CameraSize.X - Camera.CameraSize.X / 3.0f - 10.0f, -10.0f), new Vector2(Camera.CameraSize.X + 10.0f, Camera.CameraSize.Y / 3.0f + 10.0f), Color.White, 2.0f);

            foreach(Tab tab in _tabs)
            {
                spriteBatch.DrawRectangle(tab.Bounds, Color.White, 2.0f);
                tab.Draw(gameTime, spriteBatch, _font);
            }

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
