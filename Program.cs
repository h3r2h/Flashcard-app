using System;
using System.Collections.Generic;
using System.IO;

namespace Flashcards_app
{
    internal class Program
    {
        static string userFile = "users.txt";
        static string currentQuestionsFile = "";
        static string currentMissedFile = "";

        static void Main(string[] args)
        {
            // 1. Login / Registration System
            Console.WriteLine("=== WELCOME TO FLASHCARDS ===");
            Console.Write("Enter your profile name: ");
            string username = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(username))
            {
                username = "default";
            }

            currentQuestionsFile = $"{username}_questions.txt";
            currentMissedFile = $"{username}_missed.txt";
            bool isAuthenticated = false;

            // Check if users.txt exists
            if (File.Exists(userFile))
            {
                string[] userLines = File.ReadAllLines(userFile);
                bool userFound = false;

                foreach (string line in userLines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 2 && parts[0] == username)
                    {
                        userFound = true;
                        // User exists, ask for password
                        Console.Write("Enter your password: ");
                        string password = ReadPassword(); // CHANGED FROM Console.ReadLine()

                        if (parts[1] == password)
                        {
                            Console.WriteLine("Login successful!");
                            isAuthenticated = true;
                        }
                        else
                        {
                            Console.WriteLine("Incorrect password!");
                        }
                        break;
                    }
                }

                // If username wasn't found in users.txt, register them
                if (!userFound)
                {
                    Console.WriteLine("Profile not found. Let's create a new account!");
                    Console.Write("Create a password: ");
                    string newPassword = ReadPassword(); // CHANGED FROM Console.ReadLine()

                    File.AppendAllText(userFile, $"{username},{newPassword}{Environment.NewLine}");
                    Console.WriteLine("Account created and logged in!");
                    isAuthenticated = true;
                }
            }
            else
            {
                // First user ever, create users.txt
                Console.WriteLine("No users registered yet. Let's create your account!");
                Console.Write("Create a password: ");
                string newPassword = ReadPassword(); // CHANGED FROM Console.ReadLine()

                File.WriteAllText(userFile, $"{username},{newPassword}{Environment.NewLine}");
                Console.WriteLine("Account created and logged in!");
                isAuthenticated = true;
            }

            if (!isAuthenticated)
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Press any key to enter the app...");
            Console.ReadKey();

            // 2. Main menu loop
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== FLASHCARD APP [{username.ToUpper()}] ===");
                Console.WriteLine("1. Play Quiz");
                Console.WriteLine("2. Add a Question");
                Console.WriteLine("3. Remove a Question");
                Console.WriteLine("4. Review Missed Questions");
                Console.WriteLine("5. Logout / Exit");
                Console.Write("Choose an option (1-5): ");

                string choice = Console.ReadLine();

                if (choice == "1") RunQuiz();
                else if (choice == "2") AddQuestion();
                else if (choice == "3") RemoveQuestion();
                else if (choice == "4") ReviewMissedQuestions();
                else if (choice == "5") break;
                else Console.WriteLine("Invalid choice.");

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static void RunQuiz()
        {
            if (!File.Exists(currentQuestionsFile))
            {
                Console.WriteLine("No questions found for this profile yet. Add some first!");
                return;
            }

            string[] lines = File.ReadAllLines(currentQuestionsFile);
            if (lines.Length == 0)
            {
                Console.WriteLine("The question file is empty.");
                return;
            }

            int score = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(',');

                if (parts.Length < 6) continue;

                Console.WriteLine($"Question: {parts[0]}");
                Console.WriteLine($"1) {parts[1]}");
                Console.WriteLine($"2) {parts[2]}");
                Console.WriteLine($"3) {parts[3]}");
                Console.WriteLine($"4) {parts[4]}");

                Console.Write("Your answer (1-4): ");
                if (int.TryParse(Console.ReadLine(), out int userChoice) && userChoice == int.Parse(parts[5]))
                {
                    Console.WriteLine("Correct!");
                    score++;
                }
                else
                {
                    Console.WriteLine($"Incorrect. Correct answer was {parts[5]}.");

                    // Automatically save the missed question line
                    File.AppendAllText(currentMissedFile, line + Environment.NewLine);
                }
                Console.WriteLine(new string('-', 30));
            }

            Console.WriteLine($"Quiz Finished! Score: {score} out of {lines.Length}");
        }

        static void AddQuestion()
        {
            Console.Write("Question text: "); string q = Console.ReadLine();
            Console.Write("Option 1: "); string o1 = Console.ReadLine();
            Console.Write("Option 2: "); string o2 = Console.ReadLine();
            Console.Write("Option 3: "); string o3 = Console.ReadLine();
            Console.Write("Option 4: "); string o4 = Console.ReadLine();
            Console.Write("Correct answer (1-4): "); string ans = Console.ReadLine();

            File.AppendAllText(currentQuestionsFile, $"{q},{o1},{o2},{o3},{o4},{ans}{Environment.NewLine}");
            Console.WriteLine("Question added to your profile!");
        }

        static void RemoveQuestion()
        {
            if (!File.Exists(currentQuestionsFile)) return;

            // 1. Read all lines into a List
            List<string> lines = new List<string>(File.ReadAllLines(currentQuestionsFile));

            if (lines.Count == 0)
            {
                Console.WriteLine("No questions found.");
                return;
            }

            // 2. Display the questions so the user can see them
            Console.WriteLine("Current Questions:");
            for (int i = 0; i < lines.Count; i++)
            {
                string[] parts = lines[i].Split(',');
                Console.WriteLine($"{i + 1}) {parts[0]}");
            }

            // 3. Ask which number to delete
            Console.Write("Enter the number of the question to remove: ");
            if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= lines.Count)
            {
                // 4. Remove it using the 0-based index
                lines.RemoveAt(num - 1);

                // 5. Save the updated list back to the file
                File.WriteAllLines(currentQuestionsFile, lines);
                Console.WriteLine("Question removed successfully!");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        static List<string> GetQuestionsList()
        {
            if (!File.Exists(currentQuestionsFile))
            {
                Console.WriteLine("No questions found for this profile yet. Add some first!");
                return null;
            }

            List<string> lines = new List<string>(File.ReadAllLines(currentQuestionsFile));

            if (lines.Count == 0)
            {
                Console.WriteLine("The question file is empty.");
                return null;
            }

            return lines;
        }

        static void ReviewMissedQuestions()
        {
            if (!File.Exists(currentMissedFile))
            {
                Console.WriteLine("No missed questions recorded yet. Great job!");
                return;
            }

            string[] lines = File.ReadAllLines(currentMissedFile);
            if (lines.Length == 0)
            {
                Console.WriteLine("No missed questions recorded yet. Great job!");
                return;
            }

            Console.WriteLine($"=== REVIEWING MISSED QUESTIONS ({lines.Length}) ===");
            int score = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(',');

                if (parts.Length < 6) continue;

                Console.WriteLine($"Question: {parts[0]}");
                Console.WriteLine($"1) {parts[1]}");
                Console.WriteLine($"2) {parts[2]}");
                Console.WriteLine($"3) {parts[3]}");
                Console.WriteLine($"4) {parts[4]}");

                Console.Write("Your answer (1-4): ");
                if (int.TryParse(Console.ReadLine(), out int userChoice) && userChoice == int.Parse(parts[5]))
                {
                    Console.WriteLine("Correct!");
                    score++;
                }
                else
                {
                    Console.WriteLine($"Incorrect. Correct answer was {parts[5]}.");
                }
                Console.WriteLine(new string('-', 30));
            }

            Console.WriteLine($"Review Finished! Score: {score} out of {lines.Length}");

            Console.Write("Would you like to clear your missed questions list? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() == "y")
            {
                File.WriteAllText(currentMissedFile, string.Empty);
                Console.WriteLine("Missed questions list cleared!");
            }
        }

        // --- ADDED HELPER SUBROUTINE FOR PASSWORD MASKING ---
        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            }
            while (true);

            Console.WriteLine();
            return password;
        }
    }
}
