# C# Console Flashcard App

A lightweight, secure, and self-contained console-based flashcard utility built in **C#**. Designed for efficient studying and concept retention, this application features user profile management, secure password masking, local flat-file data persistence, and a dedicated review queue for missed questions.

---

## 🚀 Key Features

* **Secure Multi-User Profiles:** Login or register unique profiles. Passwords are securely masked with asterisks (`*`) in the console, complete with full backspace support.
* **Interactive Quiz Mode:** Test your knowledge with multiple-choice flashcards, receive instant feedback, and track your final scores.
* **Smart Error Tracking:** Any questions answered incorrectly are automatically funneled into a separate profile-specific missed-questions file for targeted revision.
* **Dynamic Question Management:** Easily add new questions or remove outdated ones directly from the main menu using dynamic list indexing.
* **Zero Dependencies:** Runs entirely offline using local plain-text files (`.txt`), requiring no external databases or complex configuration setups.

---

## 🛠️ Project Architecture & File Storage

The application automatically manages local text files to maintain session state and persistent data:
* `users.txt`: Stores profile usernames and passwords.
* `{username}_questions.txt`: Stores the flashcard question bank for each specific user.
* `{username}_missed.txt`: Stores incorrectly answered cards for review sessions.

---

## 💻 How to Run

1. Ensure you have the [.NET SDK](https://dotnet.microsoft.com/) installed on your machine.
2. Clone or download this repository.
3. Open your terminal or command prompt inside the project directory.
4. Run the application using the following command:
   ```bash
   dotnet run
