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

    class Enemy : GameObject
    {
        private int hP;
        private int def;
        private int acc;
        private int atk;
        private string status;
        private string name;
        private int gold;

        float timer = 0;


        //sX = pixels horizontally for each sprite in the sheet
        //sY = height of spritesheet
        //count = # of images in spritesheet
        public Enemy(int h, int d, int ac, int at, int g, string n, Animation _idleAnim, Rectangle _loc, Camera _camera) : base(_idleAnim, _loc, _camera)
        {
            hP = h;
            def = d;
            acc = ac;
            atk = at;
            name = n;
            gold = g;
        }
        public int getHP()
        {
            return hP;
        }
        public void setHP(int newHP)
        {
            hP = newHP;
        }
        public int getDef()
        {
            return def;
        }
        public void setDef(int d)
        {
            def = d;
        }
        public int getAcc()
        {
            return acc;
        }
        public void setAcc(int ac)
        {
            acc = ac;
        }
        public int getAtk()
        {
            return atk;
        }
        public void setAtk(int at)
        {
            atk = at;
        }
        public int getGold()
        {
            return gold;
        }
        //poison, burn, etc.
        public void setStatus(string status, int dmg, int turns)
        {

        }
        //when player attacks the enemy
        public int damaged(int dmg)
        {
            if (dmg <= def)
                return 0;
            setHP(hP - (dmg - def));
            return (dmg - def);
        }
        //Below comes after Player Class is made
        //public string offense(int dmg, Player player)
        //{
        //    int attack = player.damaged(dmg);
        //    string ret = name + " dealt " + attack + " damage to you!";

        //}
        public Boolean isAlive()
        {
            if (hP <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
