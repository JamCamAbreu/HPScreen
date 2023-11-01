using HPScreen.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HPScreen.Entities.QuidditchPlayer;

namespace HPScreen.Entities
{
    public class QuidditchTeam
    {

        #region Singleton Implementation
        private static QuidditchTeam instance;
        private static object _lock = new object();
        public static QuidditchTeam Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new QuidditchTeam();
                        }
                    }
                }
                return instance;
            }
        }
        public List<QuidditchPlayer> Players { get; set; }
        public TeamValue Team1 { get; set; }
        public TeamValue Team2 { get; set; }
        public QuidditchTeam()
        {
            // Determine teams:
            int playersPerTeam = 3;
            List<TeamValue> teamtypes = new List<TeamValue>() {
                TeamValue.Gryffindor,
                TeamValue.Hufflepuff,
                TeamValue.RavenClaw,
                TeamValue.Slytherin
            };
            Team1 = teamtypes[Ran.Current.Next(0, teamtypes.Count - 1)];
            teamtypes.Remove(Team1);
            Team2 = teamtypes[Ran.Current.Next(0, teamtypes.Count - 1)];
            teamtypes.Remove(Team2);

            // Generate Players:
            Players = new List<QuidditchPlayer>();
            for (int i = 0; i < playersPerTeam; i++)
            {
                Players.Add(new QuidditchPlayer(Team1));
                Players.Add(new QuidditchPlayer(Team2));
            }
        }
        public void Draw()
        {
            foreach (QuidditchPlayer player in Players)
            {
                player.Draw();
            }
        }
        public void Update()
        {
            foreach (QuidditchPlayer player in Players)
            {
                player.Update();
            }
        }
        public void SetAbsolutePosition(float x, float y)
        {
            foreach (QuidditchPlayer p in Players)
            {
                p.SetAbsolutePosition(x, y);
            }
        }
        public void SetFlipped(bool flipped)
        {
            foreach (QuidditchPlayer p in Players)
            {
                p.Flipped = flipped;
            }
        }
        public void SetAbsoluteScale(float scale)
        {
            foreach (QuidditchPlayer p in Players)
            {
                p.TargetScale = scale;
                p.Scale = scale;
            }
        }
        public void RandomizeVisibility()
        {
            foreach (QuidditchPlayer p in Players)
            {
                if (Ran.Current.Next(0, 100) > 60)
                {
                    p.Visible = true;
                }
                else
                {
                    p.Visible = false;
                }
            }
        }
        #endregion
    }
}
