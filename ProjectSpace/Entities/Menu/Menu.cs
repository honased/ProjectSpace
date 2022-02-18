using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace.Entities.Menu
{
    public class Menu : Entity
    {
        private Vector2 _position;
        private SpriteFont _font;

        private string[] _menuOptions;
        private int _selectedItem;
        private Texture2D _shipTex;
        private Vector2 _shipPos;
        private MenuStar _star;
        private bool _menuActive;
        private float _opacity;
        private float _shipAngle;
        private bool _destroy;

        public bool Quit { get; private set; } = false;

        public Menu()
        {
            _position = new Vector2(Camera.CameraSize.X / 2.0f, Camera.CameraSize.Y - 64);
            new Transform2D(this) { Position = _position };
            new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprKeys"), Origin = new Vector2(100, 25), Scale = new Vector2(1.0f), Animation = "default" };

            if(Scene.GetEntity<MenuStar>() == null)
            {
                _star = new MenuStar();
                Scene.AddEntity(_star);
            }
            else
            {
                _star = Scene.GetEntity<MenuStar>();
            }

            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");

            _menuOptions = new string[] { "PLAY", "QUIT" };
            _selectedItem = 0;

            _shipTex = AssetLibrary.GetAsset<Texture2D>("ship");
            _shipPos = Vector2.Zero;
            _menuActive = true;
            _opacity = 0.0f;

            _shipAngle = MathHelper.PiOver2;
            _destroy = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (_destroy)
            {
                Destroy();
                return;
            }

            GetComponent<Transform2D>().Position = _position + new Vector2(0, MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds) * 5.0f);

            GetComponent<SpriteRenderer>().Color = Color.FromNonPremultiplied(255, 255, 255, (int)(_opacity * 255));

            if(_menuActive)
            {
                if (Input.IsKeyPressed(Keys.A) || Input.IsKeyPressed(Keys.Left)) _selectedItem -= 1;
                if (Input.IsKeyPressed(Keys.D) || Input.IsKeyPressed(Keys.Right)) _selectedItem += 1;

                if (Input.IsKeyPressed(Keys.Space))
                {
                    Camera.ShakeScreen(0.5f, 0.1);
                    switch (_menuOptions[_selectedItem])
                    {
                        case "PLAY":
                            _menuActive = false;
                            _star.KillMe();
                            break;

                        case "QUIT":
                            Quit = true;
                            break;
                    }
                }

                _selectedItem = (_selectedItem + _menuOptions.Length) % _menuOptions.Length;

                _opacity = MathHelper.Lerp(_opacity, 1.0f, 0.05f);
            }
            else
            {
                _opacity = MathHelper.Lerp(_opacity, 0.0f, 0.1f);
            }

            if (_menuActive)
            {
                float _widthAddon = Camera.CameraSize.X / (_menuOptions.Length + 1);
                if (_shipPos == Vector2.Zero)
                {
                    _shipPos = new Vector2(_widthAddon * (_selectedItem + 1), Camera.CameraSize.Y / 2.0f - 24.0f);
                }
                else
                {
                    _shipPos.X = MathHelper.Lerp(_shipPos.X, _widthAddon * (_selectedItem + 1), 0.1f);
                }
            }
            else if(!_destroy)
            {
                _shipPos.X = MathHelper.Lerp(_shipPos.X, Camera.CameraSize.X / 2.0f, 0.1f);
                _shipPos.Y = MathHelper.Lerp(_shipPos.Y, Camera.CameraSize.Y / 2.0f, 0.1f);
                _shipAngle = MathHelper.Lerp(_shipAngle, 0.0f, 0.1f);

                if(Math.Abs(_shipPos.X - (Camera.CameraSize.X / 2.0f)) < 0.5f 
                    && Math.Abs(_shipPos.Y - (Camera.CameraSize.Y / 2.0f)) < 0.5f
                    && Math.Abs(_shipAngle) < 0.01f)
                {
                    _destroy = true;
                    Rooms.Goto(Rooms.RoomBattlefield, this);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int opacityToInt = (int)(_opacity * 255);

            string menuText = "Distant Horizons";
            Vector2 origin = _font.MeasureString(menuText) / 2.0f;
            Vector2 offset = new Vector2(MathF.Sin(((float)gameTime.TotalGameTime.TotalSeconds + 3.2f) / 1.2f) * 5.0f, 0);
            spriteBatch.DrawString(_font, "Distant Horizons", new Vector2(Camera.CameraSize.X / 2.0f, 50.0f) + offset, Color.FromNonPremultiplied(255, 255, 255, opacityToInt), 0.0f, origin, 3.0f, SpriteEffects.None, 0.0f);

            // Draw menu Items
            float _widthAddon = Camera.CameraSize.X / (_menuOptions.Length + 1);
            for(int i = 0; i < _menuOptions.Length; i++)
            {
                Color color;
                if (_selectedItem == i) color = Color.FromNonPremultiplied(255, 215, 0, opacityToInt);
                else color = Color.FromNonPremultiplied(255, 255, 255, opacityToInt);
                origin = _font.MeasureString(_menuOptions[i]) / 2.0f;
                spriteBatch.DrawString(_font, _menuOptions[i], new Vector2(_widthAddon * (i+1), Camera.CameraSize.Y/2.0f), color, 0.0f, origin, 2.0f, SpriteEffects.None, 0.0f);
            }

            // Draw Ship
            spriteBatch.Draw(_shipTex, _shipPos, null, (_menuActive) ? Color.FromNonPremultiplied(255, 255, 255, opacityToInt) : Color.White, _shipAngle, new Vector2(8, 8), Vector2.One, SpriteEffects.None, 0.0f);

            // Draw HighScore
            string highScore = $"Highscore: {Globals.HighScore}";
            origin = _font.MeasureString(highScore);
            origin.X /= 2.0f;

            spriteBatch.DrawString(_font, highScore, new Vector2(Camera.CameraSize.X / 2.0f, Camera.CameraSize.Y - 8.0f), Color.FromNonPremultiplied(255, 255, 255, opacityToInt), 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
