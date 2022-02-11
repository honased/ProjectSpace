using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace.Entities.Menu
{
    public class MenuStar : Entity
    {
        private const double STAR_CREATION_TIME = 0.1;
        private const float SPD_CHANGE = (.005f / 2) * 60;

        private Texture2D _starTexture;

        private class Star
        {
            public Vector2 position;
            public Vector2 direction;
            public float opacity;
            public float speed;
        }

        private List<Star> _stars;
        private Coroutine _routine;

        public MenuStar()
        {
            _stars = new List<Star>();

            _starTexture = AssetLibrary.GetAsset<Texture2D>("laser");

            _routine = new Coroutine(this, CreateStars());
            _routine.Start();
        }

        protected override void Cleanup()
        {
            
        }

        public void KillMe()
        {
            _routine.Enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for(int i = 0; i < _stars.Count; i++)
            {
                var star = _stars[i];
                star.speed += SPD_CHANGE * t;
                star.opacity += SPD_CHANGE * t;

                star.position += star.direction * star.speed;

                if(!Camera.Bounds.Contains(star.position))
                {
                    _stars.RemoveAt(i);
                    i--;
                }
            }

            if (!_routine.Enabled && _stars.Count == 0) Destroy();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(Star star in _stars)
            {
                spriteBatch.Draw(_starTexture, star.position, Color.FromNonPremultiplied(255, 255, 255, (int)(star.opacity * 255)));
            }

            base.Draw(gameTime, spriteBatch);
        }

        private IEnumerator<double> CreateStars()
        {
            while(true)
            {
                var count = Constants.Random.Next(3);

                for(int i = 0; i < count; i++)
                {
                    Star star = new Star();
                    star.position = new Vector2((float)Constants.Random.NextDouble() * Camera.CameraSize.X, (float)Constants.Random.NextDouble() * Camera.CameraSize.Y);
                    star.direction = star.position - new Vector2(Camera.CameraSize.X / 2.0f, Camera.CameraSize.Y / 2.0f);
                    star.direction.Normalize();
                    star.opacity = 0.0f;
                    star.speed = 0.0f;

                    _stars.Add(star);
                }

                yield return STAR_CREATION_TIME;
            }
        }
    }
}
