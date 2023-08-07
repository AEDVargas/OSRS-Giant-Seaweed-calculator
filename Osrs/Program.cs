﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Osrs
{
    class Program
    {
        // UO = Unpowered Orbs  = 52.5 Exp  req level   = 46
        // LL = Lantern Lens    = 55 Exp    req level   = 49
        // LO = Light Orbs      = 70 Exp    req level   = 87
        private static double[] expMethod = new double[3] { 52.5, 55, 70 };
        private static double[] glassPerSeaweed = new double[3] { 8.7, 8.9, 9.6 };
        private static double[] glassPerSand = new double[3] { 1.45, 1.49, 1.6 };
        private static int[] glassPH = new int[3] { 10000, 15000, 13500 };

        static void Main(string[] args)
        {
            Console.WriteLine("1 - Levels");
            Console.WriteLine("2 - Resources");

            int userInput;
            bool isValidInput = false;

            do
            {
               
                string input = Console.ReadLine();

                if (int.TryParse(input, out userInput))
                {
                    switch (userInput)
                    {
                        case 1:
                            CalculateFromLevels();
                            break;
                        case 2:
                            break;
                        default:
                            Console.WriteLine("Please enter a valid entry");
                            break;
                    }
                    isValidInput = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please select either options");
                }
            } while (!isValidInput);


            Console.ReadKey();
        }

        public static void CalculateFromLevels()
        {
            int currLevel = 0;
            int goalLevel = 0;
            int crafting = 0;

            User usr = new User();

            //Locally set the users name when entering
            string username = "";
            while (true)
            {
                try
                {
                    do
                    {
                        Console.WriteLine("Enter your crafting level");
                        //Console.Write("Current Level: ");
                        //currLevel = int.Parse(Console.ReadLine());

                        Console.Write("Enter Username: ");
                        username = Console.ReadLine();

                        Task.Run(async () =>
                        {
                            await GetUserData(username, usr);
                        }).Wait();


                        Console.WriteLine("\nCurrent level: " + usr.Level);
                        currLevel = usr.Level;

                        Console.WriteLine("\nEnter your goal level");
                        Console.Write("Goal Level: ");
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

                            //Select what is being crafted from the molten glass - options varies depending on levels
                            Console.Clear();
                            Console.WriteLine("Method of crafting:\n");

                            if (currLevel >= 46 && currLevel < 49)
                            {
                                Console.WriteLine("0 - Unpowered orbs");
                            }
                            else if (currLevel >= 46 && currLevel < 87)
                            {
                                Console.WriteLine("0 - Unpowered orbs \n1 - Lantern Lens");
                            }
                            else if (currLevel >= 46)
                            {
                                Console.WriteLine("0 - Unpowered orbs \n1 - Lantern Lens \n2 - Light orbs");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Current level is not high enough level for the current available selections. Consider other methods to train craft until level 46.");
                                break;
                            }
                            Console.Write("User: ");
                            crafting = int.Parse(Console.ReadLine());


                            //Select method of making molten glass
                            int userInput;
                            bool isValidInput = false;

                            do
                            {
                                Console.WriteLine("Method of Super Glass make:\n");
                                Console.WriteLine("1 - 2 Giant seaweed and 12 bucket of sand");
                                Console.WriteLine("2 - 3 Giant seaweed and 18 bucket of sand(w / e excess)");
                                Console.WriteLine("3 - 3 Giant seaweed and 18 bucket of sand(w excess)");
                                Console.Write("User: ");
                                string input = Console.ReadLine();

                                if (int.TryParse(input, out userInput))
                                {
                                    if (userInput >= 1 && userInput <= 3)
                                    {
                                        isValidInput = true;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Please enter a valid input ranging from 1 - 3");
                                    }
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Please enter an integer value ranging from 1 - 3");
                                }
                            } while (!isValidInput);

                            //Users exp
                            RSExp UsrExp = new RSExp();
                            double usrLvlToExp = UsrExp.LevelToXP(currLevel);

                            //Goal exp
                            RSExp goalExp = new RSExp();
                            double goalLvlToExp = goalExp.LevelToXP(goalLevel);

                            //calculate exp difference
                            double expDifference = goalLvlToExp - usrLvlToExp;

                            //Calculations to find the number of seaweeed need to reach user goal level
                            double results = 0;
                            int seaWeedCount = 0;

                            if (userInput == 1)
                            {
                                results = Math.Ceiling(expDifference / (Convert.ToInt32((glassPerSand[0] * 6) * expMethod[crafting] + 40)));
                                seaWeedCount = 2;
                            }
                            else if (userInput == 2)
                            {
                                results = Math.Ceiling(expDifference / (Convert.ToInt32((glassPerSand[1] * 6) * expMethod[crafting] + 60)));
                                seaWeedCount = 3;
                            }
                            else if (userInput == 3)
                            {
                                results = Math.Ceiling(expDifference / (Convert.ToInt32((glassPerSand[2] * 6) * expMethod[crafting] + 60)));
                                seaWeedCount = 3;
                            }

                            int limit = 20;

                            Console.Clear();
                            Console.WriteLine("--" + username + "--");
                            Console.WriteLine("Results\n");
                            Console.WriteLine("Exp to goal: " + string.Format("{0:n0}", expDifference) + "\n");
                            Console.WriteLine("Required resources: \n");

                            Console.WriteLine("Giant Seaweed:".PadRight(limit) + "|" + string.Format("{0:n0}", results));
                            Console.WriteLine("Bucket of sand:".PadRight(limit) + "|" + string.Format("{0:n0}", results * 6));

                            int casts = Convert.ToInt32(results / seaWeedCount);

                            Console.WriteLine("Glass make casts:".PadRight(limit) + "|" + string.Format("{0:n0}", casts));
                            Console.WriteLine("Astral Runes:".PadRight(limit) + "|" + string.Format("{0:n0}", casts * 2));

                            Console.WriteLine("Seaweed spores:".PadRight(limit) + "|" + string.Format("{0:n0}", Math.Ceiling(results / 30)));

                            double moltenGlass = results * glassPerSeaweed[userInput - 1];

                            Console.WriteLine("\n\nNumber of glass produced: " + string.Format("{0:n0}", moltenGlass));
                            Console.WriteLine("Hours to complete: " + string.Format("{0:n1}", moltenGlass / glassPH[userInput - 1]));

                            break;
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("Please enter a valid method selection");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Please enter a valid value ranging from 1 - 99");
                }
                break;
            }
        }

        static async Task GetUserData(string username, User setLevel)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string baseUrl = "https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws";
                    string url = $"{baseUrl}?player={Uri.EscapeDataString(username)}";

                    // Perform the API request
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Split the response into individual lines (each line represents a skill's data)
                        string[] skillLines = responseBody.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        // Print the skills, levels, and experience data with skill names
                        //Console.WriteLine("Skill          Level     Experience");
                        //Console.WriteLine("-----------------------------------");

                        string[] skillNames = {
                        "Overall", "Attack", "Defence", "Strength", "Hitpoints", "Ranged", "Prayer",
                        "Magic", "Cooking", "Woodcutting", "Fletching", "Fishing", "Firemaking",
                        "Crafting", "Smithing", "Mining", "Herblore", "Agility", "Thieving",
                        "Slayer", "Farming", "Runecraft", "Hunter", "Construction"
                    };

                        for (int i = 0; i < skillLines.Length; i++)
                        {
                            string[] data = skillLines[i].Split(',');
                            int skillLevel = int.Parse(data[1]);
                            int skillExp = int.Parse(data[2]);

                            if(skillNames[i] == "Crafting")
                            {
                                setLevel.Level = skillLevel;
                            }

                            //Console.WriteLine($"{skillNames[i],-14} {skillLevel,-8} {string.Format("{0:n0}", skillExp)}");
                        }
                    }
                    else
                    {
                        // Handle non-successful responses
                        //Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                //Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    public class User
    {
        private int level;
        private int experience;

        public int Level
        {
            get { return level; }
            set { level = value;  }
        }

        public int Experience
        {
            get { return experience; }
            set { experience = value; }
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
        /// - include api calls to get user levels from hiscores website ✓
        /// - select option:
        ///     > Calculate resources needed to reach a certain level 
        ///     > Calculate banked exp from number of resources (giant seaweed/sand)


    }
}
