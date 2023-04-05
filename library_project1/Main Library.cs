using AdminHandlesBooks;

 namespace AdminHandlesBooks
{

    // Main Library




    internal class Program    //Registrering funktion börjar här!
    {
        static void Main(string[] args)
        {
            RegistrationPage();
        }

        static void RegistrationPage()
        {
            Console.WriteLine("Vänligen ange förnamn, efternamn, personnummer & lösenord för registrering.");
            Console.WriteLine("Ange utropstecken ! i Instant Login för att tas direkt till inloggning");
            Console.WriteLine("");

            Console.Write("Instant Login: ");
            var instant = Console.ReadLine();

            if (instant == "!")
            {
                LogInPage();
                return;
            }



            Console.WriteLine("förnamn: ");
            var firstname = Console.ReadLine();

            
            Console.WriteLine("efternamn: ");
            var lastname = Console.ReadLine();


            Console.Write("personnummer: ");    //ingen reglering för hur lång nummret bör vara!
            var personalNumber = Console.ReadLine();


            Console.Write("lösenord: ");
            var password = Console.ReadLine();




            var line = $"{firstname} {lastname} {personalNumber} {password}";
            string[] lines = { line };


           

            static bool UserRegistered(string personalNumber)
            {
                string[] UsersFromDb =
                System.IO.File.ReadAllLines(@"C:\Users\emil.lindgren8\source\repos\library_project1\Users.txt");


                for (int i = 0; i < UsersFromDb.Length; i++)
                {
                    var line = UsersFromDb[i].Trim();
                    string[] parts = line.Split(' ');

                    var currentPersonalNumber = parts[2];  //krashar om indexvärde utanför array anges

                    if (currentPersonalNumber == personalNumber)
                    {
                        return true;
                    }
                }

                return false;

            }

            static bool UserInfoIncorrect(string? firstName, string? lastName, string? personalNumber, string? password) //användar info check fortsättning
            {
                if (firstName == null || firstName == "") return true;
                if (lastName == null || lastName == "") return true;
                if (personalNumber == null || personalNumber == "") return true;
                if (password == null || password == "") return true;

                return false;
            }



            Console.WriteLine("");

            if (UserInfoIncorrect(firstname, lastname, personalNumber, password))
            {
                Console.Clear();
                Console.WriteLine("Ange riktiga uppgifter för att kunna registrera dig.");
                Console.WriteLine("");
                RegistrationPage();
                return;
            }
            else if (UserRegistered(personalNumber))
            {
                Console.Clear();
                Console.WriteLine("Användaren har redan registrerat sig. Ange ett nytt personnummer för att registrera dig.");
                Console.WriteLine("");
                RegistrationPage();
                return;
            }


            File.AppendAllLines(@"C:\Users\emil.lindgren8\source\repos\library_project1\Users.txt", lines);

            Console.WriteLine("Du är nu registrerad och kan logga in. Vänligen vänta för att skickas till loginsidan.");

            Thread.Sleep(4500);

            LogInPage();
        }

        //Login funktion 




        static void LogInPage()
        {




            bool wrongpassword = false;
            string personalNumber = "";
            string password = "";  



            while (!Authenticate(personalNumber, password))
            {



                if (wrongpassword)
                {
                    Console.WriteLine("Lösenord stämmer ej, dummer!");
                }
                else
                {
                    Console.WriteLine("Välkommen!");
                }

                Console.WriteLine("Ange vänligen personnummer och lösenord för att logga in");
                Console.WriteLine("");

                Console.Write("Personnummer: ");
                personalNumber = Console.ReadLine();

                Console.Write("Lösenord: ");
                password = Console.ReadLine();

                Console.WriteLine("");

                wrongpassword = true;


            }


            Console.WriteLine("Du är nu inloggad. Vänligen vänta för att skickas till Bibloteket.");
            Thread.Sleep(4500);
            Console.Clear();
            Booksystem();
           //ersatt ProfilePage();
        }

        static bool Authenticate(string personalNumber, string password)  //Authenticate funkar ej?
        {
            string[] usersFromDb = System.IO.File.ReadAllLines(@"C:\Users\emil.lindgren8\source\repos\library_project1\Users.txt");

            for (int i = 0; i < usersFromDb.Length; i++)
            {
                string[] parts = usersFromDb[i].Split(" ");

                string personalNumberFromDb = parts[2];
                string passFromDb = parts[3];


                if (personalNumber == personalNumberFromDb && password == passFromDb)
                {
                    return true;
                }
            }

            return false;

        }



        static void ProfilePage()  //ersattes, inte längre använd
        {
            Console.Clear();
            Console.WriteLine("ProfilSida");
            Console.WriteLine("Nu är du inloggad!");
        }
   
            static BookSystem system = BookSystem.GetInstance();
        static void Booksystem()
            {
              
                Console.WriteLine("Välkomen! Välj 1 för att gå vidare:\n");
                Console.WriteLine ("1. Lista alla böcker");
               //Console.WriteLine("2. Lägg till en bok");
               //Console.Write("Ditt val (1-2): ");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    //AddBookPage();
                    ListAllBooks();
                }
                
            
            }

            static void ListAllBooks()
            {
                Console.Clear();
                List<Book> books = system.GetBooks();

                for (var i = 0; i < books.Count; i++)
                {
                    Book book = books[i];

                    Console.WriteLine($"{i + 1}. {book.Name} {book.Author} {book.Genre}");    //formateringen i book textfilen sannolikt felaktig
                }

                Console.Write("\nVälj en bok (siffra): ");
                var choice = Console.ReadLine();
                int number;

                var isNumber = int.TryParse(choice, out number);

                if (isNumber && number > 0 && number < books.Count + 1)
                {
                    BookDetailPage(books[number - 1]);
                }

            }

            static void BookDetailPage(Book book)
            {
                Console.Clear();
                Console.WriteLine($"Name: {book.Name}");
                Console.WriteLine($"Author: {book.Author}");
                Console.WriteLine($"Genre: {book.Genre}");        
                Console.WriteLine($"Genre: {book.rowNumber}");
            Console.WriteLine($"Available: {book.Available}");

                Console.WriteLine("Vad vill du göra?\n");
                Console.WriteLine("1. Gå tillbaka");


                if (book.Available)
                {
                    Console.WriteLine("2. låna boken");
                }

                bool userIsRenting = system.currentUserIsRentingBook(book);

                if (userIsRenting)
                {
                    Console.WriteLine("2. Returnera Boken");

                }

                Console.Write("Ditt val (1-2): ");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    ListAllBooks();
                }
                else if (choice == "2" && book.Available)
                {
                    system.RentBook(book);

                    RentBookPage();
                }
                else if (choice == "2" && userIsRenting)
                {
                    system.ReturnBook(book);
                    ReturnBookPage();
                }
            }

            static void RentBookPage()
            {
                Console.Clear();

                while (true)
                {
                    Console.Write("Boken har nu lånats. Ange 1 för att återgå: ");
                    var choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        ListAllBooks();
                        break;
                    }
                }
            }

            static void ReturnBookPage()
            {
                Console.Clear();

                while (true)
                {
                    Console.Write("Boken har nu lämnats tillbaka. Ange 1 för att återgå: ");
                    var choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        ListAllBooks();
                        break;
                    }
                }
            }
        }





           

     
      






 }




    
