using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using HonasGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectSpace.Entities;
using System;

namespace ProjectSpace.Components
{
    public class BubbleShield : Component
    {
        private const float RADIUS = 18.0f;
        private const double MAX_LIFE = 5.0;

        private Collider2D _bubbleCollider;
        private double _life;

        public BubbleShield(Entity parent) : base(parent)
        {
            _bubbleCollider = new Collider2D(parent) { Shape = new BoundingCircle(0, 0, RADIUS) };
            if (parent is Ship ship) ship.Invincible = true;

            _life = MAX_LIFE;
        }

        public override void Update(GameTime gameTime)
        {
            _bubbleCollider.Shape.Position = Parent.GetComponent<Transform2D>().Position;

            if (_bubbleCollider.CollidesWith(Globals.TAG_ASTEROID, out Entity asteroid))
            {
                if (asteroid is Asteroid roid) roid.FullDestroy = true;
                asteroid.Destroy();

                Scene.GetEntity<ScoreCounter>()?.AddScore(100);
            }

            _life -= gameTime.ElapsedGameTime.TotalSeconds;

            if(_life <= 0.0)
            {
                Parent.RemoveComponent(_bubbleCollider);
                _bubbleCollider = null;
                Parent.RemoveComponent(this);
                if (Parent is Ship ship) ship.Invincible = false;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color;
            float t = (float)gameTime.TotalGameTime.TotalSeconds;
            if (_life <= 1.0)
            {
                color = Color.FromNonPremultiplied(0, 255, 255, (int)MathF.Round((MathF.Sin(t * 20.0f) + 1) / 2.0f) * 255);
            }
            else color = Color.Aqua;
            spriteBatch.DrawCircle(Parent.GetComponent<Transform2D>().Position, RADIUS + MathF.Sin(t * 2.0f), color);
        }
    }
}
