using HonasGame.ECS;
using HonasGame.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using HonasGame.Rendering;

namespace ProjectSpace.Entities
{
    public class ShipTrail : Entity
    {
        public Vector2 Position { get; set; }
        public Vector2 EndPosition { get; set; }

        private double lifeTime;
        private const double MAX_LIFE_TIME = 1.0;

        public ShipTrail(float length, float direction)
        {
            EndPosition = new Vector2(
                length * MathF.Cos(direction),
                length * MathF.Sin(direction)
                );

            lifeTime = MAX_LIFE_TIME;
        }

        public override void Update(GameTime gameTime)
        {
            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;

            if (lifeTime < 0.0) Destroy();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(Position, Position - EndPosition, Color.FromNonPremultiplied(255, 255, 255, (int)((lifeTime/MAX_LIFE_TIME)*255)), 2.0f);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
