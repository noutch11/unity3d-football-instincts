# Football Instincts: Full-Stack Multiplayer Soccer Project

**Note: This project was last edited in 2019. It utilizes legacy Unity syntax (e.g., the `WWW` class) and the classic Photon Networking framework. It's here as a demonstration of full-stack architectural design and network synchronization logic.**

A comprehensive multiplayer sports simulation featuring a custom authentication system, a persistent MySQL/PHP backend, and real-time physics-driven gameplay.

## System Architecture & Backend Integration
* **Custom Authentication Lifecycle:** Engineered a complete user account system (Register, Login, Password Recovery) utilizing C# to interface with a remote MySQL database via PHP middleware.
* **PHP Middleware Layer:** Wrote a suite of PHP scripts to handle server-side logic, including MD5 password hashing, database queries, and leaderboard management.
* **Persistent User Data:** Developed a data synchronization layer to track and update player stats, levels, and "In-game Coins" across sessions.

## Real-Time Multiplayer & Networking
* **Photon Lobby System:** Implemented a global "Lounge" (lobby) using Photon Networking, allowing for synchronized player lists and team invitations.
* **Live Chat Engine:** Developed a real-time global chat with RPC (Remote Procedure Call) synchronization and a custom regex-based profanity filter.
* **Team Management:** Engineered logic for team captaincy, player invitations, and "Ready to play" status synchronization before match initialization.

## Gameplay Mechanics & AI
* **Physics-Based Ball Logic:** Authored a sophisticated ball controller utilizing Impulse-based forces for kicking, and collision detection to manage "Dribbler" ownership and goal events.
* **Kinetic Player Controls:** Developed a movement system that balances stamina management with acceleration and shot-power charging logic.
* **Goalkeeper AI:** Engineered an automated goalkeeper with state-based logic for "Idle," "Diving Save," and "Throw" behaviors, utilizing Vector3.Lerp for strategic positioning relative to the ball.

## Administrative Tasks & Moderation
* **In-Game Admin Tools:** Built a secure administrative panel for real-time player moderation, including Ban/Unban functionality, user promotion/demotion, and global system alerts.
* **Session Security:** Implemented server-side `Admincheck.php` to prevent unauthorized access to sensitive administrative PHP modules.

## Project Structure
* **`Match/`**: Core gameplay scripts for physics, AI, and player controls.
* **`Global+Other/`**: Networking, database connectivity, and persistent global settings.
* **`Lounge/`**: UI and logic for the multiplayer lobby and social features.
* **`PHP Scripts/`**: The backend API layer for MySQL interaction.

## Tech Stack
* **Engine:** Unity (Legacy 2018/2019)
* **Languages:** C#, PHP, SQL
* **Networking:** Photon Unity Networking
* **Database:** MySQL
* **UI:** Legacy Unity UI System (RectTransform anchoring logic)
