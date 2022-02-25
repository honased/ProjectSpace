using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using HonasGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectSpace.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace.Entities
{
    public abstract class Pickup : Entity
    {
        protected Type _componentType;
        public void AddEffect(Entity e)
        {
            Activator.CreateInstance(_componentType, e);
            Destroy();
        }
    }

    public class PickupBubbleShield : Pickup
    {
        private const float RADIUS = 4.0f;
        private float _alpha;
        public PickupBubbleShield(Vector2 position)
        {
            new Transform2D(this) { Position = position };
            new Collider2D(this) { Shape = new BoundingCircle(position.X, position.Y, RADIUS), Tag = Globals.TAG_PICKUP };

            _componentType = typeof(BubbleShield);
            _alpha = 0;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _alpha = MathHelper.Lerp(_alpha, 255, 0.1f);
            spriteBatch.DrawCircle(GetComponent<Transform2D>().Position, RADIUS + MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds) * 1.0f, Color.FromNonPremultiplied(0, 255, 255, (int)_alpha));

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
