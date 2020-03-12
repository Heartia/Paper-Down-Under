using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Paper_Down_Under
{
    abstract class GameObject
    {
        protected Animation idleAnim;
        protected Rectangle loc;
        protected Vector2 pos;
        protected Vector2 vel;
        protected Camera camera;

        public GameObject(Animation _idleAnim, Rectangle _loc, Camera _camera)
        {
            idleAnim = _idleAnim;
            loc = _loc;
            pos = new Vector2(loc.X, loc.Y);
            vel = new Vector2(0, 0);
            camera = _camera;
        }

        public Rectangle _loc
        {
            get
            {
                return loc;
            }
        }

        public Vector2 _pos
        {
            get
            {
                return pos;
            }
        }

        public Vector2 _vel
        {
            get
            {
                return vel;
            }
        }

        public void update()
        {
            pos.X += vel.X;
            pos.Y += vel.Y;
            loc.X = (int)Math.Round(pos.X);
            loc.Y = (int)Math.Round(pos.Y);
            idleAnim.next();
        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (loc.Intersects(camera._loc))
            {
                spriteBatch.Draw(idleAnim.getFrame(), loc, Color.White);
            }
        }
    }
}
