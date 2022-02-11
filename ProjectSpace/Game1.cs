using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectSpace.Entities;
using ProjectSpace.Entities.Menu;

namespace ProjectSpace
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            Camera.CameraSize = new Vector2(480, 270);
            Camera.Bounds = new Rectangle(-32, -32, (int)Camera.CameraSize.X + 64, (int)Camera.CameraSize.Y + 64);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            AssetLibrary.AddAsset("fntText", Content.Load<SpriteFont>("Fonts/fntText"));

            AssetLibrary.AddAsset("ship", Content.Load<Texture2D>("Sprites/Ship"));
            AssetLibrary.AddAsset("shipFragment", Content.Load<Texture2D>("Sprites/ShipFragment"));
            AssetLibrary.AddAsset("asteroid", Content.Load<Texture2D>("Sprites/Asteroid"));
            AssetLibrary.AddAsset("asteroidSmall", Content.Load<Texture2D>("Sprites/Asteroid Small"));
            AssetLibrary.AddAsset("asteroidFragment", Content.Load<Texture2D>("Sprites/AsteroidFragment"));
            AssetLibrary.AddAsset("laser", Content.Load<Texture2D>("Sprites/Laser"));
            AssetLibrary.AddAsset("keys", Content.Load<Texture2D>("Sprites/Keys"));

            Sprite spr = new Sprite(AssetLibrary.GetAsset<Texture2D>("ship"));
            AssetLibrary.AddAsset("sprShip", spr);

            spr = new Sprite(AssetLibrary.GetAsset<Texture2D>("laser"));
            AssetLibrary.AddAsset("sprLaser", spr);

            // Big asteroid
            spr = new Sprite(AssetLibrary.GetAsset<Texture2D>("asteroid"));
            spr.Animations.Add("default", new SpriteAnimation()
            {
                Frames = new Rectangle[] {
                    new Rectangle(0, 0, 32, 32),
                    new Rectangle(32, 0, 32, 32)
                }
            });
            AssetLibrary.AddAsset("sprAsteroid", spr);

            spr = new Sprite(AssetLibrary.GetAsset<Texture2D>("asteroidSmall"));
            spr.Animations.Add("default", new SpriteAnimation()
            {
                Frames = new Rectangle[] {
                    new Rectangle(0, 0, 16, 16),
                    new Rectangle(16, 0, 16, 16)
                }
            });
            AssetLibrary.AddAsset("sprAsteroidSmall", spr);

            spr = new Sprite(AssetLibrary.GetAsset<Texture2D>("asteroidFragment"));
            spr.Animations.Add("default", SpriteAnimation.FromSpritesheet(8, 0.0, 0, 0, 16, 16));
            AssetLibrary.AddAsset("sprAsteroidFragment", spr);

            spr = new Sprite(AssetLibrary.GetAsset<Texture2D>("shipFragment"));
            spr.Animations.Add("default", SpriteAnimation.FromSpritesheet(7, 0.0, 0, 0, 16, 16));
            AssetLibrary.AddAsset("sprShipFragment", spr);

            spr = new Sprite(AssetLibrary.GetAsset<Texture2D>("keys"));
            spr.Animations.Add("default", SpriteAnimation.FromSpritesheet(6, 0.5, 0, 0, 200, 50));
            AssetLibrary.AddAsset("sprKeys", spr);

            AssetLibrary.AddAsset("sndDestroy", Content.Load<SoundEffect>("SFX/Destroy"));

            //Scene.AddEntity(new Ship());
            Scene.AddEntity(new Menu());
            //Scene.AddEntity(new ScoreCounter());
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Scene.GetEntity<Menu>()?.Quit == true) Exit();

            // TODO: Add your update logic here
            Input.Update();
            Scene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.GetMatrix(new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)));
            Scene.Draw(gameTime, _spriteBatch);

            Vector2 size = AssetLibrary.GetAsset<SpriteFont>("fntText").MeasureString("Distant Horizons");

            //_spriteBatch.DrawString(AssetLibrary.GetAsset<SpriteFont>("fntText"), "Distant Horizonts", Camera.CameraSize / new Vector2(2.0f, 2.5f), Color.PaleVioletRed, 0.0f, size / 2.0f, 3.0f, SpriteEffects.None, 0.0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
