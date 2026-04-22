using System.IO;
namespace ProgectGame
{
    class StartGame // Main  class 
    {

        static void Main(string[] args)
        {

            Console.WriteLine("\n=============== WELCOME TO NUMBER GAME ===============\n");
            Gamebody obj = new Gamebody();
            obj.Run();
        }
    }

    class Gamebody  // Gamebody class created
    {
        private Random getrandom = new Random();                          // .
        private Gamefunctions user = new Gamefunctions();                 // defined class's object for call functions
        private Gamefunctions computer = new Gamefunctions();              //.   
        private MaintainLog logobject = new MaintainLog();                  //..


        private int[] UsrRoundScore = new int[3];     // array for save each round score 
        private int[] CompRoundScore = new int[3];

        // function for play game in round
        public void Run()
        {
            logobject.Clearlog();  // clearing log

            string name = user.Inputname();

            user.Playername = name;
            computer.Playername = "Computer";


            for (int rnd = 1; rnd <= 3; rnd++)    // loop for play round 3 times
            {
                Console.WriteLine($"\n\n*********** ROUND {rnd} ***********");
                logobject.Addlog($"*********** ROUND {rnd} ***********");
                StartUserRound(rnd);
                StartCompRound(rnd);


                UsrRoundScore[rnd - 1] = user.CalcScore();
                CompRoundScore[rnd - 1] = computer.CalcScore();
            }

            WinnerResult();
        }

        // function for make play by user
        private void StartUserRound(int round)
        {
            user.Getcards(getrandom);
            Console.WriteLine($"Your card are:({user.card[0]}, {user.card[1]})");    // provide initaial cards for player
            logobject.Addlog($"{user.Playername} get card:({user.card[0]}, {user.card[1]}) Score:{computer.CalcScore()}");

            string Input = "";

            while (Input != "Y" && Input != "N")
            {
                Console.Write("Do you want to swap a card? (Y/N): ");
                try
                {

                    Input = Console.ReadLine()?.Trim().ToUpper();
                }
                catch
                {
                    Input = "";
                }
            }

            if (Input == "Y")  // for card get swap
            {
                int CardPosn = 0;

                while (CardPosn != 1 && CardPosn != 2)
                {
                    Console.Write("\nEnter which card is use to swap(1 or 2): ");
                    string Choose = Console.ReadLine()?.Trim();
                    if (Choose == "1" || Choose == "2")
                    {
                        CardPosn = Convert.ToInt32(Choose);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, Please enter either 1 or 2.");
                    }
                }

                int index = CardPosn - 1;
                int oldCard = user.card[index];
                user.card[index] = getrandom.Next(1, 9);

                Console.WriteLine($"Your card {CardPosn} swapped.\nPrevious card:{oldCard}, New card:{user.card[index]}");
                logobject.Addlog($"{user.Playername} swapped card {CardPosn} (Previous card:{oldCard}, New card:{user.card[index]})");
            }
            else // for stay with card
            {
                logobject.Addlog($"{user.Playername} choose to stayed with cards ({user.card[0]}, {user.card[1]})");
            }

            int Rndscore = user.CalcScore();       // calculate score
            UsrRoundScore[round - 1] = Rndscore;

            Console.WriteLine($"\nYour round {round} score is :{Rndscore}");
            if (Input == "Y")
                logobject.Addlog($"{user.Playername} final cards:({computer.card[0]}, {computer.card[1]}) Score: {Rndscore}");

        }



        // function for play computer round
        private void StartCompRound(int round)
        {
            computer.Getcards(getrandom);   // computer get it`s initial card
            logobject.Addlog($"\nComputer get card:({computer.card[0]}, {computer.card[1]}) Score:{computer.CalcScore()}");

            int Try = 1;

            while (computer.CalcScore() >= 3 && Try < 2)  // for make computer can swap if score is greater than or equal to 3
            {
                int index = getrandom.Next(0, 2);
                int oldCard = computer.card[index];
                computer.card[index] = getrandom.Next(1, 9);
                Try++;

                logobject.Addlog($"Computer swapped card {index + 1} (Previous card:{oldCard}, New card:{computer.card[index]})");
            }

            int Rndscore = computer.CalcScore();   // calculate score after swaped
            CompRoundScore[round - 1] = Rndscore;

            if (Try == 2)
            {
                logobject.Addlog($"Computer final cards:({computer.card[0]}, {computer.card[1]}) Score:{Rndscore}\n");

            }
            else
            {
                logobject.Addlog($"{computer.Playername} choose to stayed with cards ({user.card[0]}, {user.card[1]})\n");
            }
        }



        // function for define winner
        private void WinnerResult()
        {

            int UsrSC = UsrRoundScore[0] + UsrRoundScore[1] + UsrRoundScore[2];           // ading all round score of user
            int CompSC = CompRoundScore[0] + CompRoundScore[1] + CompRoundScore[2];

            Console.WriteLine("\n\n################ GAME OVER ################");

            Console.WriteLine("\n^^^^^^^^^ ROUND SCORES ^^^^^^^^^");
            for (int i = 0; i < 3; i++)  // display each round score
            {
                Console.WriteLine($"Round {i + 1}: \n\t{user.Playername} = {UsrRoundScore[i]} \n\tComputer = {CompRoundScore[i]}\n");
            }

            Console.WriteLine($"\n^^^^^^^^^ TOTAL SCORE ^^^^^^^^^\n{user.Playername}: {UsrSC}");  // display total score of rounds 
            Console.WriteLine($"Computer: {CompSC}");

            string win;
            if (UsrSC < CompSC)  // for decide who is winner based on total score
                win = user.Playername;
            else if (CompSC < UsrSC)
                win = "Computer";
            else   //  in case of draw, winner decide based on last round card sum 
            {
                if (user.Add() < computer.Add())
                    win = user.Playername;
                else if (computer.Add() < user.Add())
                    win = "Computer";
                else
                    win = "Draw";
            }

            Console.WriteLine($"\n\n^^^^^^^^^^^^^^^^^^^^\nWinner is: {win}\n^^^^^^^^^^^^^^^^^^^^");

            if (win == user.Playername)
                Console.WriteLine("CONGRATULATIONS YOU WON!");
            else Console.WriteLine("Better Luck Next Time");


            logobject.Addlog($"^^^^^^^^^^^^^^^^^^^^\nWinner: {win}\n^^^^^^^^^^^^^^^^^^^^");  // display winner
            Console.WriteLine("\n\nGame play saved to GameLogfile.txt");
        }
    }
    class Gamefunctions   // Gamefunctions class created
    {
        public string Playername;           // public field for player`s name
        public int[] card = new int[2];     // array decleration for cards store
        public string Inputname()     // function for take player name
        {
            string UserName = "";
            while (string.IsNullOrWhiteSpace(UserName))
            {
                Console.Write("Start game by Entering name: ");
                UserName = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    Console.WriteLine("Please enter name to start, Game can`t start without name.");
                }
            }
            return UserName;
        }

        public void Getcards(Random getrandom)    // function for make card shuffle
        {
            card[0] = getrandom.Next(1, 9);   // generate random first card from 1 to 8
            card[1] = getrandom.Next(1, 9);   // generate random second card from 1 to 8
        }


        public int CalcScore()  // function for return score
        {
            int max = Math.Max(card[0], card[1]);   // find greater card
            int min = Math.Min(card[0], card[1]);   // find lower card
            return max - min;
        }


        public int Add()    // function for return two cards sum
        {
            return card[0] + card[1];   
        }
    }
   
    class MaintainLog  //MaintainLog class created
    {
        private string Storefile = "GameLog.txt";

        public void Clearlog()    // for clear log file in case log is already exist of old game play
        {
            try
            {
                File.WriteAllText(Storefile, "");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get error during clearing log file: {ex.Message}");
            }
        }


        public void Addlog(string data)  // for add log data of game play in logfile
        {
            try
            {
                File.AppendAllText(Storefile, data + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get error during writing to log file: {ex.Message}");
            }
        }

    }
   
}
