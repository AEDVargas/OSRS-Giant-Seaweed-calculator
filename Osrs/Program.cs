using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrs
{
    class Program
    {
        static void Main(string[] args)
        {
            // UO = Unpowered Orbs  = 52.5 Exp  req level   = 46
            // LL = Lantern Lens    = 55 Exp    req level   = 49
            // LO = Light Orbs      = 70 Exp    req level   = 87

            //Avg seaword per spore: 30 - 1 seed

            int currLevel = 0;
            int goalLevel = 0;
            int crafting = 0;

            double[] expMethod = new double[3]{ 52.5, 55, 70 };
            double[] glassPerSeaweed = new double[3] { 8.7, 8.9, 9.6 };
            double[] glassPerSand = new double[3] { 1.45, 1.49, 1.6 };

            while (true)
            {
                try
                {
                    do
                    {
                        Console.WriteLine("Enter your crafting level");
                        currLevel = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter your goal level");
                        goalLevel = int.Parse(Console.ReadLine());

                    } while (currLevel >= 100 && goalLevel >= 100);

                    while (goalLevel <= currLevel)
                    {
                        Console.Clear();
                        Console.WriteLine("Please enter a valid goal above your current goal (" + currLevel + ")");
                        goalLevel = int.Parse(Console.ReadLine());
                    }

                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("Method of crafting:\n");

                            if (currLevel >= 46 && currLevel < 49)
                            {
                                Console.WriteLine("0: Unpowered orbs");
                            }
                            else if (currLevel >= 46 && currLevel < 87)
                            {
                                Console.WriteLine("0: Unpowered orbs \n1: Lantern Lens");
                            }
                            else if (currLevel >= 46)
                            {
                                Console.WriteLine("0: Unpowered orbs \n1: Lantern Lens \n2: Light orbs");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Current level is not high enough level for the current available selections. Consider other methods to train craft until level 46.");
                                break;
                            }
                            crafting = int.Parse(Console.ReadLine());

                            Console.WriteLine("Method of Super Glass make:\n");

                            int X = 0;

                            //while(!int.TryParse(Console.ReadLine(), out X) && X > 0 && X < 3)
                            //{
                            //    Console.WriteLine("Please enter a valid input from 0 - 1");
                            //}

                            int test = 0;

                            do
                            {
                                try
                                {
                                    test = int.Parse(Console.ReadLine());
                                }
                                catch
                                {

                                }
                            } while (test > 0 && test < 3);


                            //Users exp
                            RSExp UsrExp = new RSExp();
                            double usrLvlToExp = UsrExp.LevelToXP(currLevel);

                            //Goal exp
                            RSExp goalExp = new RSExp();
                            double goalLvlToExp = goalExp.LevelToXP(goalLevel);

                            //calculate exp difference
                            double expDifference = goalLvlToExp - usrLvlToExp;

                            //Number of seaweed to reach level
                            double[] methods = new double[3];

                            for(int i = 0; i < methods.Length; i++)
                            {
                                if(crafting == 0)
                                {
                                    methods[i] = Math.Ceiling(expDifference / (Convert.ToInt32((glassPerSand[i] * 6) * expMethod[crafting] + 40)));
                                }
                                else
                                {
                                    methods[i] = Math.Ceiling(expDifference / (Convert.ToInt32((glassPerSand[i] * 6) * expMethod[crafting] + 60)));
                                }
                            }

                            Console.Clear();
                            Console.WriteLine("Method 0: 2 Giant seaweed + 12 bucket of sand");
                            Console.WriteLine("Method 1: 3 Giant seaweed - 18 bucket of sand(w / e excess)");
                            Console.WriteLine("Method 2: 3 Giant seaweed - 18 bucket of sand (w excess)");

                            Console.WriteLine("Results\n");

                            Console.WriteLine("Exp to goal: " + string.Format("{0:n0}", expDifference) + "\n");

                            for (int i = 0; i < glassPerSeaweed.Count(); i++)
                            {
                                Console.WriteLine("Method " + i + " "+ methods[i] + " giant seaweed  Required buckets of sand: " + methods[i] * 6);
                            }

                            int[] casts = new int[2] { Convert.ToInt32(methods[0] / 2), Convert.ToInt32(methods[2] / 3)};

                            Console.WriteLine("Number of casts: " + casts[0]);
                            

                            //Console.WriteLine("Number of casts: " + casts);
                            //Console.WriteLine("Number of atral runes required: " + casts * 2);
                            //Console.WriteLine("Magic Exp: " + string.Format("{0:n0}", casts * 78));

                            //Console.WriteLine("Crafting Exp from casting: " + string.Format("{0:n0}", casts * 40));
                            //Console.WriteLine("Crafting Exp from casting: " + string.Format("{0:n0}", casts * 60));
                            //Console.WriteLine("Crafting Exp from casting: " + string.Format("{0:n0}", casts * 60));


                            //double expFromBlowing_m1 = (m1 * 8.7) * expMethod[methodSelection];
                            //double expFromBlowing_m2 = (m2 * 8.9) * expMethod[methodSelection];
                            //double expFromBlowing_m3 = (m3 * 9.6) * expMethod[methodSelection];


                            //Console.WriteLine("Method 1 Exp from blowing: " + string.Format("{0:n0}", expFromBlowing_m1));
                            //Console.WriteLine("Method 2 Exp from blowing: " + string.Format("{0:n0}", expFromBlowing_m2));
                            //Console.WriteLine("Method 3 Exp from blowing: " + string.Format("{0:n0}", expFromBlowing_m3));

                            //Console.WriteLine("METHOD 1 TOTAL: " + expFromBlowing_m1 + (casts * 40));
                            //Console.WriteLine("METHOD 2 TOTAL: " + expFromBlowing_m2 + (casts * 60));
                            //Console.WriteLine("METHOD 3 TOTAL: " + expFromBlowing_m3 + (casts * 60));

                            break;
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("Please enter a valid method selection");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid value ranging from 1 - 99");
                }
                break;
            }

            Console.ReadKey();
        }
    }

    public class RSExp
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

        /// Future implementations
        /// include api calls to get user levels from hiscores website


    }
}
