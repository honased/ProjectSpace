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
        public PickupBubbleShield(Vector2 position)
        {
            new Transform2D(this) { Position = position };
            new Collider2D(this) { Shape = new BoundingCircle(position.X, position.Y, RADIUS), Tag = Globals.TAG_PICKUP };

            _componentType = typeof(BubbleShield);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(GetComponent<Transform2D>().Position, RADIUS, Color.Aqua);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
