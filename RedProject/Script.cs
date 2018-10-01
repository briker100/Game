using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    public class Script : GameScreen
    {
        private readonly Player _player;

        public Script(Player player)
        {
            MenuItems = new Dictionary<string, Func<GameScreen>>
        {
            {"Look at the map and find the way out", () =>
                                    {
                                        MenuItems.Remove("Look at the map and find the way out");
                                        _player.Inventory.Add(new InventoryItem { Name = "Escape Badge", Description = "A badge that shows you know how to leave the cave." });
                                        return this;
                                    } },
            {"Go to [CAVE]", () => new PlayScreen(player) }
        };
            _player = player;
        }

        public override GameScreen Run()
        {
            Console.Clear();
            if (!_player.HasItem("MAP")) return Intro();
            return Menu();
        }

        private GameScreen Intro()
        {
            Write("You wake up with a headache, confused and\nsurrounded with broken weapons and shields everywhere.\nAs you shake your head you hear some sound walking towards you.");
            Write("Ah! There you are! Took me ages to find you!\nHere's a [MAP], see you outside of the [CAVE] in 30 minutes!");

            var bag = new InventoryItem { Name = "MAP", Description = "A useful way to find yourself out of the Cave. +50 points" };
            _player.Inventory.Add(bag);

            var name = string.Empty;
            int attempts = 0;
            while (string.IsNullOrEmpty(name))
            {
                attempts++;
                string prompt;
                if (attempts == 1)
                {
                    prompt = "You get up, pick up the map and remember why your there and what your name is.\nWhat is your Name (enter it!):";
                    name = Prompt(prompt);
                }
                else if (attempts < 3)
                {
                    prompt = "Uh, don't you remember your name?";
                    name = Prompt(prompt);
                }
                else
                {
                    name = "Bob Junior.";
                    prompt = string.Format("Ok nevermind, we'll call you {0}.", name);
                    Write(prompt);
                }
            }
            _player.Name = name;

            return Menu();
        }
    }

    public class PlayScreen : GameScreen
    {
        private readonly Player _player;
        private int choice;

        public PlayScreen(Player player)
        {
            MenuItems = new Dictionary<string, Func<GameScreen>>
                                    {
                                        { "Go to [CAVE]", () => new Script(player)
                }
                                    };
            _player = player;
        }

        public override GameScreen Run()
        {
            Console.Clear();

            if (_player.HasItem("Escape badge"))
            {
                Write("You walk outside of the cave and find the mysteroius man sitting down.");
                Write(_player.Name + ", finally! So managed to find the exit! Do you remember why you were there or who you are.\n\nGood! So you remember.");
                MenuItems = new Dictionary<string, Func<GameScreen>> { { "Then you know that we have failed.\nIt all rest on you now.\nYou are the last choice to susceed,but first\nYou need to find the town. Press 1 to go fowards with the mission.", () => null } };
            }
            else
            {
                Write("You try to find your way out but cant susceed!\n You walk in circles and find youself back where you woke up.");
                Write(_player.Name + "! What are you? I told you to come on out!");
            }

            Console.WriteLine("\t\t\t\t\t\tPoints: 50");

            Console.WriteLine($"\t\t\t\t\t\tCongratulations, {_player.Name} Won");
            Console.WriteLine("\t\t\t\t\t\t More coming out soon.");
            Console.Read();
            return Menu();
        }
    }
}