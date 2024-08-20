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
    public class Squid
    {
        private int size;
        private int squidLife;
        private bool isHorizontal;
        private List<Point> squidLocation;
        private ImageView seeSquid;

        public int SquidLife
        {
            get { return squidLife; }
            set { squidLife = value; }
        }
        public List<Point> SquidLocation
        {
            get { return squidLocation; }
        }

        public ImageView SeeSquid
        {
            get { return seeSquid; }
            set { seeSquid = value; }
        }

        public Squid(int size, bool isHorizontal)
        {
            squidLocation = new List<Point>();
            this.size = size;
            this.isHorizontal = isHorizontal;
            squidLife = size;
        }

        public void CreateSquid()
        {
            Point temp;
            Random rand = new Random();

            squidLocation.Clear();

            if(isHorizontal)
            {
                temp = new Point(rand.Next(1, 9 - (size - 1)), rand.Next(1, 9));
                for (int i = 0; i < size; i++)
                {  
                    squidLocation.Add(temp);
                    temp = new Point(temp.X + 1, temp.Y);
                }
            }
            else
            {
                temp = new Point(rand.Next(1, 9), rand.Next(1, 9 - (size - 1)));
                for (int i = 0; i < size; i++)
                {
                    squidLocation.Add(temp);
                    temp = new Point(temp.X, temp.Y + 1);
                }
            }

        }
    }
}