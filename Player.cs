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
using System.IO;

namespace Paper_Down_Under
{
    class Player : GameObject
    {
        private Vector2 baseVel;
        private Vector2 acceleration;
        private KeyboardState kb;
        private Boolean isRight;
        private Boolean isCrouching;
        private Boolean isJumping;
        private Animation walkAnim;
        private Animation crouchAnim;
        private Animation jumpAnim;
        private Boolean canJump;
        const int TileLength = 64;
        private Rectangle ploc;
        private SpriteFont debugFont;
        Tile cTile;
        private const float VelStop = 1.5f;
        private const int walkSpeed = 10;
        private const float decceleration = .3f;

        public Player(Animation _idleAnim, Animation _walkAnim, Animation _jumpAnim, Animation _crouchAnim, SpriteFont _debugFont, Rectangle _loc, Camera _camera, Vector2 _baseVel, Vector2 _acceleration) : base(_idleAnim, _loc, _camera)
        {
            baseVel = _baseVel;
            acceleration = _acceleration;
            kb = Keyboard.GetState();
            walkAnim = _walkAnim;
            jumpAnim = _jumpAnim;
            crouchAnim = _crouchAnim;
            isCrouching = false;
            isJumping = false;
            ploc = new Rectangle(loc.X + 18, loc.Y + 15, loc.Width - 36, loc.Height - 15);
            debugFont = _debugFont;
        }

        public void update(GameTime gameTime, Room curr)
        {
            kb = Keyboard.GetState();
            Vector2 dir = new Vector2();

            if (canJump)
            {
                if (kb.IsKeyDown(Keys.Up))
                {
                    isJumping = true;
                    dir.Y -= 1;
                }
                else
                {
                    isJumping = false;
                    canJump = false;

                }
            }

            if (kb.IsKeyDown(Keys.Right))
            {
                dir.X += 1;
                isRight = true;
            }
            //To be Added once Collisions are fixed
            if (kb.IsKeyDown(Keys.Down))
            {
                dir.Y += 1;
                //isCrouching = true;
            }
            else if (vel.Y == 0)
            {
                //isCrouching = false;
            }
            if (kb.IsKeyDown(Keys.Left))
            {
                dir.X -= 1;
                isRight = false;
            }

            if (dir.Y < 0)
            {
                vel.Y -= 2 * acceleration.Y;
                if (vel.Y < -(2 * baseVel.Y / 3))
                {
                    vel.Y = -(2 * baseVel.Y / 3);
                    canJump = false;
                }
            }
            if (dir.X < 0 && vel.X > -baseVel.X)
            {
                vel.X -= acceleration.X;
                if (vel.X < -baseVel.X)
                {
                    vel.X = -baseVel.X;
                }
            }
            else if (dir.X > 0 && vel.X < baseVel.X)
            {
                vel.X += acceleration.X;
                if (vel.X > baseVel.X)
                {
                    vel.X = baseVel.X;
                }
            }

            if (dir.X == 0)
            {
                if (vel.X > 0)
                {
                    vel.X -= decceleration;
                    if (vel.X < 0)
                    {
                        vel.X = 0;
                    }
                }
                else if (vel.X < 0)
                {
                    vel.X += decceleration;
                    if (vel.X > 0)
                    {
                        vel.X = 0;
                    }
                }
            }

            vel.Y += (acceleration.Y / 2);
            if (vel.Y > baseVel.Y)
            {
                vel.Y = baseVel.Y;
            }

            Rectangle oldLoc = camera.loc;
            camera.loc.X += (int)vel.X;
            camera.loc.Y += (int)vel.Y;

            Tile[][] map = curr.tileMap;
            
            for (int r = 0; r < map.Length; r++)
            {
                for (int c = 0; c < map[r].Length; c++)
                {
                    if (map[r][c] != null && (map[r][c]._state == Tile.TileState.Platform || map[r][c]._state == Tile.TileState.Impassable))
                    {
                        Rectangle nLoc = new Rectangle(map[r][c]._loc.X - camera.loc.X,
                                                       map[r][c]._loc.Y - camera.loc.Y,
                                              map[r][c]._loc.Width, map[r][c]._loc.Height);
                        if (vel.Y > 0 && ploc.TouchTopOf(nLoc))
                        {
                            if (map[r][c] == cTile && cTile != null)
                            {
                                continue;
                            }
                            else if (map[r][c] != cTile && cTile != null)
                            {
                                isCrouching = false;
                                cTile = null;
                            }
                            vel.Y = -(vel.Y / 5);
                            camera.loc.Y = oldLoc.Y;
                            canJump = true;
                            if (kb.IsKeyDown(Keys.Down) && map[r][c]._state == Tile.TileState.Platform && ploc.TouchTopOf(nLoc))
                            {
                                vel.Y = 1f;
                                isCrouching = true;
                                cTile = map[r][c];
                            }
                            
                            
                        }
                        if (map[r][c]._state == Tile.TileState.Impassable && ploc.TouchBottomOf(nLoc))
                        {
                            vel.Y = 1f;
                            //camera.loc.Y = oldLoc.Y;
                            canJump = false;
                            //Console.WriteLine(vel.Y);
                        }
                        if (map[r][c]._state == Tile.TileState.Impassable && vel.X > 0 && ploc.TouchLeftOf(nLoc))
                        {
                            vel.X = -(vel.X / 3);
                            camera.loc.X = oldLoc.X;
                        }
                        if (map[r][c]._state == Tile.TileState.Impassable && vel.X < 0 && ploc.TouchRightOf(nLoc))
                        {
                            vel.X = -(vel.X / 3);
                            camera.loc.X = oldLoc.X;
                        }
                    }
                }
            }

            camera.loc.X += (int)vel.X;
            camera.loc.Y += (int)vel.Y;

            if (isJumping)
            {
                jumpAnim.next();
            }
            else
            {
                jumpAnim.last();
            }
            if (isCrouching)
            {
                crouchAnim.next();
            }
            else
            {
                crouchAnim.last();
            }
            if (vel.X > -VelStop && vel.X < VelStop)
            {
                idleAnim.next();
                walkAnim.last();
            }
            else
            {
                walkAnim.imgTick = (int)Math.Abs(Math.Round(walkSpeed / (vel.X / 2)));
                walkAnim.next();
                idleAnim.last();
            }
        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isRight)
            {
                if (isJumping)
                {
                    spriteBatch.Draw(jumpAnim.getFrame(), loc, Color.White);
                }
                else if (isCrouching)
                {
                    spriteBatch.Draw(crouchAnim.getFrame(), loc, Color.White);
                }
                else if (vel.X > -VelStop && vel.X < VelStop)
                {
                    spriteBatch.Draw(idleAnim.getFrame(), loc, Color.White);
                }
                else
                {
                    spriteBatch.Draw(walkAnim.getFrame(), loc, Color.White);
                }
            }
            else
            {
                if (isJumping)
                {
                    spriteBatch.Draw(jumpAnim.getFrame(), loc, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 1f);
                }
                else if (isCrouching)
                {
                    spriteBatch.Draw(crouchAnim.getFrame(), loc, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 1f);
                }
                else if (vel.X > -VelStop && vel.X < VelStop)
                {
                    spriteBatch.Draw(idleAnim.getFrame(), loc, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 1f);
                }
                else
                {
                    spriteBatch.Draw(walkAnim.getFrame(), loc, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 1f);
                }
            }
            displayGUI(gameTime, spriteBatch);
        }

        public void displayGUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            enclosedDrawString(spriteBatch, debugFont, "[" + (camera.loc.X + loc.X) + ", " + (camera.loc.Y + loc.Y) + "]", new Vector2(10, 10), new Color(0, 255, 0), Color.Green);
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            try
            {
                enclosedDrawString(spriteBatch, debugFont, "FPS: " + Math.Round(frameRate, 1), new Vector2(10, 30), new Color(0, 255, 0), Color.Green);
            }
            catch (ArgumentException e)
            {
                enclosedDrawString(spriteBatch, debugFont, "FPS: 0", new Vector2(10, 30), new Color(0, 255, 0), Color.Green);
            }
        }

        public void enclosedDrawString(SpriteBatch spriteBatch, SpriteFont font, string message, Vector2 position, Color c1, Color c2)
        {
            spriteBatch.DrawString(font, message, new Vector2(position.X - 1, position.Y), c2);
            spriteBatch.DrawString(font, message, new Vector2(position.X - 1, position.Y - 1), c2);
            spriteBatch.DrawString(font, message, new Vector2(position.X, position.Y - 1), c2);
            spriteBatch.DrawString(font, message, new Vector2(position.X + 1, position.Y - 1), c2);
            spriteBatch.DrawString(font, message, new Vector2(position.X + 1, position.Y), c2);
            spriteBatch.DrawString(font, message, new Vector2(position.X + 1, position.Y + 1), c2);
            spriteBatch.DrawString(font, message, new Vector2(position.X, position.Y + 1), c2);
            spriteBatch.DrawString(font, message, new Vector2(position.X - 1, position.Y + 1), c2);
            spriteBatch.DrawString(font, message, position, c1);
        }
    }
}