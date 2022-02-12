using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace.Entities
{
    public class Laser : Entity
    {
        private Transform2D _transform;
        private SpriteRenderer _renderer;
        private Collider2D _collider;

        public Vector2 Velocity { get; set; }

        public Laser()
        {
            _transform = new Transform2D(this) { Position = new Vector2(128, 128) };
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprLaser"), Origin = new Vector2(2, 2) };
            _collider = new Collider2D(this) { Shape = new BoundingRectangle(0, 0, 4, 4) { Origin = new Vector2(2, 2) }, Tag = 0 };

            _renderer.Color = Color.Red;
        }

        public override void Update(GameTime gameTime)
        {
            _transform.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _collider.Shape.Position = _transform.Position;

            if(_collider.CollidesWith(Globals.TAG_ASTEROID, out Entity asteroid))
            {
                Destroy();
                if (asteroid is Asteroid roid) roid.FullDestroy = true;
                asteroid.Destroy();

                Scene.GetEntity<ScoreCounter>()?.AddScore(100);
            }

            if(!Camera.Bounds.Contains(_transform.Position))
            {
                Destroy();
            }

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
