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

namespace Game_v1
{

    class Enemy : GameObject
    {
        Texture2D texture;
        Rectangle[] anims;
        int anim = 0;
        float timer = 0;
        int sourceX;
        int sourceY;

        //sX = pixels horizontally for each sprite in the sheet
        //sY = height of spritesheet
        //count = # of images in spritesheet
        public Enemy(Texture2D t, int sX, int sY, int count, Animation _idleAnim, Rectangle _loc, Camera _camera) : base(_idleAnim, _loc, _camera)
        {
            texture = t;
            sourceX = sX;
            sourceY = sY;
            anims = new Rectangle[count];
            loc = _loc;
            Initialize();
        }
        public void Initialize()
        {
            for (int i = 0; i < anims.Count(); i++)
            {
                anims[i] = new Rectangle(sourceX * i, 0, sourceX, sourceY);
            }
        }
        public void Update(GameTime gameTime)
        {
            timer++;
            if (timer % 2 == 0)
            {
                anim++;
                if (anim > 5)
                {
                    anim = 0;
                }
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, loc, anims[anim], Color.White);
        }
    }
}
