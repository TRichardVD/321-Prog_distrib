using System;
using System.Numerics;
using Newtonsoft.Json;

namespace MyApp
{
    internal class Program
    {

        static void Main(string[] args)
        {

            var myActor = new Actor()
            {
                FirstName = "Jeam",
                LastName = "Bon",
                BirthDate = new DateTime(2000,04,23),
                Country = "Suisse",
                IsAlive = true,
            };


            var myCharacter = new Character()
            {
                FirstName = "Jerry",
                LastName = "Gérard",
                Description = "M. Gérard est quelqu'un de normal. C'est un pnj.",
                PlayedBy = myActor
            };

            var myEpisode = new Episode()
            {
                Title="La cafeteria",
                DurationMinutes = 25,
                SequenceNumber = 1,
                Director = "Dimitri rector",
                Synopsis = "Bienvenue dans la cafeteria !",
                Characters = new()
                {
                    myCharacter
                }
            };

            var mySaison = new Saison()
            {
                Number = 1,
                Name = "Eté et miel",
                Description = "L'été et le miel",
                Episodes = new()
                {
                    myEpisode
                }
            };

            string fileName = $"{myCharacter.FirstName.ToLower()}.json";
            
            File.WriteAllText(fileName, JsonConvert.SerializeObject(myCharacter, Formatting.Indented ));
            File.WriteAllText($"{myActor.FirstName.ToLower()}.json", JsonConvert.SerializeObject(myActor, Formatting.Indented));
            File.WriteAllText($"{myEpisode.Title.ToLower()}.json", JsonConvert.SerializeObject(myEpisode, Formatting.Indented));
            File.WriteAllText($"{mySaison.Name.ToLower()}.json", JsonConvert.SerializeObject(mySaison, Formatting.Indented));



            try
            {
                Character? perso = JsonConvert.DeserializeObject<Character>(File.ReadAllText(fileName));
                Console.WriteLine($"Le personnage de {perso?.FirstName} {perso?.LastName} est joué par {perso?.PlayedBy.FirstName} {perso?.PlayedBy.LastName}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
    public class Saison
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Episode> Episodes { get; set; }
    }

    public class Episode
    {
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public int SequenceNumber { get; set; }
        public string Director { get; set; }
        public string Synopsis { get; set; }
        public List<Character> Characters { get; set; } = new List<Character>();
    }

    public class Actor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public bool IsAlive { get; set; }
    }

    public class Character
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public Actor PlayedBy { get; set; }
    }

}