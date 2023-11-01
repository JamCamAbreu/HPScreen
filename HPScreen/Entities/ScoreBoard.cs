using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPScreen.Entities
{
    public class ScoreBoard
    {
        #region Singleton Implementation
        private static ScoreBoard instance;
        private static object _lock = new object();
        public static ScoreBoard Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new ScoreBoard();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public ScoreBoard() 
        { 
        
        }

        public void Update()
        {

        }
        public void Draw()
        {

        }
    }
}
