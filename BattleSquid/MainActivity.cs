using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Media;
using Android.Content;
using System.Threading;
using System;

namespace BattleSquid
{
    [Activity(Label = "BattleSquid", MainLauncher = true, Icon = "@drawable/squid_boom")]
    public class MainActivity : Activity
    {
        private GridLayout mainLayout;
        private SquidGrid squidGrid;
        SquidButton[,] grid;
        MediaPlayer sploosh, kerboom;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            sploosh = MediaPlayer.Create(this, Resource.Raw.Sploosh);
            kerboom = MediaPlayer.Create(this, Resource.Raw.Kerboom);

            squidGrid = new SquidGrid();
            grid = new SquidButton[8, 8];
            LinearLayout[] linear = new LinearLayout[8];
            LinearLayout squidHolder = new LinearLayout(this);
            LinearLayout[] bombHolder = new LinearLayout[2];
            ImageView[] squids = new ImageView[3];
            ImageView[,] bombs = new ImageView[2, 12];
            Space[] spaces = new Space[2];

            var metrics = Resources.DisplayMetrics;

            mainLayout = new GridLayout(this);
            mainLayout.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            mainLayout.ColumnCount = 8;
            mainLayout.RowCount = 11;

            LinearLayout.LayoutParams linearPara = new LinearLayout.LayoutParams(metrics.WidthPixels / 6, metrics.WidthPixels / 8);
            LinearLayout.LayoutParams spacePara = new LinearLayout.LayoutParams(metrics.WidthPixels / 4, metrics.WidthPixels / 8);

            spaces[0] = new Space(this);
            spaces[0].LayoutParameters = spacePara;
            squidHolder.AddView(spaces[0]);

            for (int i = 0; i < 3; i++)
            {
                squids[i] = new ImageView(this);
                squids[i].SetBackgroundResource(Resource.Drawable.squid_icon);
                squids[i].LayoutParameters = linearPara;
                squidHolder.AddView(squids[i]);
            }

            squidGrid.AssignSquids(squids[0], squids[1], squids[2]);

            spaces[1] = new Space(this);
            spaces[1].LayoutParameters = spacePara;
            squidHolder.AddView(spaces[1]);

            mainLayout.AddView(squidHolder, new GridLayout.LayoutParams(GridLayout.InvokeSpec(0), GridLayout.InvokeSpec(0)));


            for (int i = 0; i < 8; i++)
            {
                linear[i] = new LinearLayout(this);
                for (int j = 0; j < 8; j++)
                {
                    grid[i, j] = new SquidButton(this, squidGrid.CheckSquid(j,i));
                    grid[i, j].Click += (sender, e) =>
                    {
                        MediaPlayer player;
                        SquidButton temp = (SquidButton)sender;

                        if (temp.IsSquid)
                        {
                            temp.AttackSquid();

                            temp.SetBackgroundResource(Resource.Drawable.hit);
                        } 
                        else
                        {
                            temp.SetBackgroundResource(Resource.Drawable.X);
                        }

                        
                        if(squidGrid.SquidState())
                        {
                            AlertDialog dg;
                            AlertDialog.Builder buidler = new AlertDialog.Builder(this);
                            dg = buidler.Create();
                            dg.SetTitle("Hooray!");
                            dg.SetMessage("You Defeated The Squids!\nYou Won!");
                            dg.SetButton("Play Again?", new EventHandler<DialogClickEventArgs>(
                                    (s, args) => {
                                        var restart = new Intent(this, typeof(MainActivity));
                                        StartActivity(restart);
                                    }));
                            dg.Show();
                        }
                        else
                        {
                            squidGrid.ThrowBomb();

                            if (squidGrid.SquidBombs == 0)
                            {

                                AlertDialog dg;
                                AlertDialog.Builder buidler = new AlertDialog.Builder(this);
                                dg = buidler.Create();
                                dg.SetTitle("Oh No!");
                                dg.SetMessage("The Squids Have Eaten You!\nYou Lose!");
                                dg.SetButton("Play Again", new EventHandler<DialogClickEventArgs>(
                                    (s, args) => {
                                        var restart = new Intent(this, typeof(MainActivity));
                                        StartActivity(restart);
                                    }));
                                dg.SetButton2("Quit", new EventHandler<DialogClickEventArgs>(
                                   (s, args) => {
                                       FinishAffinity();
                                   }));
                                dg.Show();
                            }
                        }

                        if (squidGrid.SquidBombs == 0)
                        {
                            if (squidGrid.SquidState())
                            {
                                player = MediaPlayer.Create(this, Resource.Raw.HoorayYay);
                                Thread t = new Thread(player.Start);
                                t.Start();
                                t.Join();
                            }
                            else
                            {
                                player = MediaPlayer.Create(this, Resource.Raw.LoseYell);
                                Thread t = new Thread(player.Start);
                                t.Start();
                                t.Join();
                            }
                        }
                        else if (squidGrid.SquidState())
                        {
                            player = MediaPlayer.Create(this, Resource.Raw.HoorayYay);
                            Thread t = new Thread(player.Start);
                            t.Start();
                            t.Join();
                        }
                        else
                        {

                            if (temp.IsSquid)
                            {
                                if (temp.IsAlive)
                                {
                                    Thread t = new Thread(kerboom.Start);
                                    t.Start();
                                    t.Join();
                                }
                                else
                                {
                                    player = MediaPlayer.Create(this, Resource.Raw.KillYay);
                                    Thread t = new Thread(player.Start);
                                    t.Start();
                                    t.Join();
                                }
                            }
                            else
                            {
                                Thread t = new Thread(sploosh.Start);
                                t.Start();
                                t.Join();
                            }
                        }
                        


                        temp.Enabled = false;

                    };
                    linear[i].AddView(grid[i, j], metrics.WidthPixels/8, metrics.WidthPixels/8);
                }
                mainLayout.AddView(linear[i], new GridLayout.LayoutParams(GridLayout.InvokeSpec(i+1), GridLayout.InvokeSpec(0)));
            }

            for (int i = 0; i < 2; i++)
            {
                bombHolder[i] = new LinearLayout(this);
                for (int j = 0; j < 12; j++)
                {
                    bombs[i, j] = new ImageView(this);
                    bombs[i, j].SetBackgroundResource(Resource.Drawable.bomb);
                    bombs[i, j].LayoutParameters = new LinearLayout.LayoutParams(metrics.WidthPixels / 12, metrics.WidthPixels / 12);

                    bombHolder[i].AddView(bombs[i, j]);
                }
                mainLayout.AddView(bombHolder[i], new GridLayout.LayoutParams(GridLayout.InvokeSpec(9 + i), GridLayout.InvokeSpec(0)));
            }
            squidGrid.GetBombs(bombs);
            this.AddContentView(mainLayout, new GridLayout.LayoutParams());

        }


    }
}

