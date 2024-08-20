using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BattleSquid
{
    public class SquidGrid
    {
        private Squid twoSquid, threeSquid, fourSquid;
        private int squidBombs;
        public ImageView[,] bombsLeft;

        public int SquidBombs
        {
            get { return squidBombs; }
            set { squidBombs = value; }
        }

        public SquidGrid()
        {
            twoSquid = new Squid(2, RandomSquid());
            threeSquid = new Squid(3, RandomSquid());
            fourSquid = new Squid(4, RandomSquid());

            squidBombs = 24;

            GenerateSquid();
        }

        public void AssignSquids(ImageView two, ImageView three, ImageView four)
        {
            twoSquid.SeeSquid = two;
            threeSquid.SeeSquid = three;
            fourSquid.SeeSquid = four;
        }

        public void GetBombs(ImageView[,] bombsLeft)
        {
            this.bombsLeft = bombsLeft;
        }

        public void ThrowBomb()
        {
            squidBombs -= 1;
            try
            {
                if (squidBombs >= 12)
                    bombsLeft[1, squidBombs - 12].SetBackgroundResource(Resource.Drawable.bomb_gray);
                else
                    bombsLeft[0, squidBombs].SetBackgroundResource(Resource.Drawable.bomb_gray);
            }catch(IndexOutOfRangeException IORE)
            {
                Console.Error.WriteLine(IORE.Message);
            }

        }

        private bool RandomSquid()
        {
            Random rand = new Random();

            if (rand.NextDouble() < 0.5)
                return false;
            else
                return true;
        }

        private void GenerateSquid()
        {
            fourSquid.CreateSquid();

            do
            {
                threeSquid.CreateSquid();
                twoSquid.CreateSquid();
            } while (fourSquid.SquidLocation.Intersect(threeSquid.SquidLocation).Any() ||
            fourSquid.SquidLocation.Intersect(twoSquid.SquidLocation).Any() ||
            threeSquid.SquidLocation.Intersect(twoSquid.SquidLocation).Any());
        }

        public Squid CheckSquid(int x, int y)
        {
            if (fourSquid.SquidLocation.Exists(temp => temp.X == x + 1) && fourSquid.SquidLocation.Exists(temp => temp.Y == y + 1))
                return fourSquid;
            else if ((threeSquid.SquidLocation.Exists(temp => temp.X == x + 1) && threeSquid.SquidLocation.Exists(temp => temp.Y == y + 1)))
                return threeSquid;
            else if (twoSquid.SquidLocation.Exists(temp => temp.X == x + 1) && twoSquid.SquidLocation.Exists(temp => temp.Y == y + 1))
                return twoSquid;
            else
                return null;
        }

        public bool SquidState()
        {
            if (twoSquid.SquidLife == 0 && threeSquid.SquidLife == 0 && fourSquid.SquidLife == 0)
                return true;
            else
                return false;
        }
        
    }
}