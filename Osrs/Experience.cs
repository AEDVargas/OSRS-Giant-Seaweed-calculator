using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrs
{
    public class Experience
    {

        public int Equate(double xp)
        {
            return (int)Math.Floor(
                xp + 300 * Math.Pow(2, xp / 7));
        }

        public double LevelToXP(int level)
        {
            double xp = 0;

            for (int i = 1; i < level; i++)
                xp += this.Equate(i);

            return Math.Floor(xp / 4);
        }

        public int XPToLevel(int xp)
        {
            int level = 1;

            while (this.LevelToXP(level) < xp)
                level++;

            return level;
        }
    }
}
