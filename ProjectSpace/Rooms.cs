using HonasGame.Assets;
using HonasGame.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ProjectSpace.Entities;
using ProjectSpace.Entities.Hangar;
using ProjectSpace.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace
{
    public static class Rooms
    {
        public delegate void RoomGenerator();

        public static void Goto(RoomGenerator generator, Entity exclude = null)
        {
            Scene.Clear(exclude);
            generator();
        }

        public static void RoomMenu()
        {
            Scene.AddEntity(new Menu());
        }

        public static void RoomBattlefield()
        {
            Scene.AddEntity(new Ship());
            Scene.AddEntity(new ScoreCounter());
            Scene.AddEntity(new Spawner());
            Scene.AddEntity(new FlashingMessage("Destroy Asteroids", AssetLibrary.GetAsset<SoundEffect>("sndDestroy")));
            Scene.AddEntity(new PickupBubbleShield(new Vector2(60, 60)));
        }

        public static void RoomHangar()
        {
            Scene.AddEntity(new Hangar());
        }
    }
}
