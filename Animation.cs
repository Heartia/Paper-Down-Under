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
    class Animation
    {
        private Texture2D spritesheet;
        private Rectangle partOfSpritesheet;
        private GraphicsDevice graphicsDevice;
        private int[] imgTicks;
        private int ticks;
        private int index;

        public Animation(Texture2D _spritesheet, GraphicsDevice _graphicsDevice, Rectangle _partOfSpritesheet, int[] _imgTicks)
        {
            spritesheet = _spritesheet;
            graphicsDevice = _graphicsDevice;
            partOfSpritesheet = _partOfSpritesheet;
            imgTicks = _imgTicks;
            ticks = 0;
        }

        public Texture2D getFrame()
        {
            // Create a new texture of the desired size
            Texture2D croppedTexture = new Texture2D(graphicsDevice, partOfSpritesheet.Width, partOfSpritesheet.Height);

            // Copy the data from the cropped region into a buffer, then into the new texture
            Color[] data = new Color[partOfSpritesheet.Width * partOfSpritesheet.Height];
            spritesheet.GetData(0, partOfSpritesheet, data, 0, partOfSpritesheet.Width * partOfSpritesheet.Height);
            croppedTexture.SetData(data);

            return croppedTexture;
        }

        public void next()
        {
            ticks++;
            if (ticks >= imgTicks[index])
            {
                ticks = 0;
                index++;
                if (index >= imgTicks.Length)
                {
                    index = 0;
                }
                partOfSpritesheet = new Rectangle(partOfSpritesheet.Width * index, partOfSpritesheet.Y, partOfSpritesheet.Width, partOfSpritesheet.Height);
            }
        }
    }
}
