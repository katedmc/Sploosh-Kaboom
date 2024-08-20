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
    public class SquidButton : Button, ISquid
    {
        private bool isSquid;
        private bool isAlive;
        public Squid squid;

        public bool IsSquid
        {
            get
            {
                return isSquid;
            }
        }

        public bool IsAlive
        {
            get { return isAlive; }
        }


        public SquidButton(Context context, Squid squid) : base(context)
        {
            if (squid != null)
            {
                isSquid = true;
                isAlive = true;
                this.squid = squid;
            }
            else
            {
                isSquid = false;
            }

        }

        public void AttackSquid()
        {
            squid.SquidLife -= 1;
            if (squid.SquidLife == 0)
            {
                isAlive = false;
                squid.SeeSquid.SetBackgroundResource(Resource.Drawable.squid_boom);
            }
                
        }


    }
}