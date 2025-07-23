# 🗺️ untitled GO — A Pokémon GO-style Game Built with Unity

**untitled GO** is a location-based game inspired by Pokémon GO, developed in Unity. It features a full 3D map of Moscow, real-time geolocation tracking, character progression, battles, and a robust client-server backend. Players explore the city, engage in battles, and upgrade their character and gear while their position syncs with real-world movement.

---

## 🚀 Features

- 📍 **Real-time Geolocation** — Player character movement is synchronized with the user’s actual GPS location.
- 🗺️ **3D Map of Moscow** — The city is represented in a detailed 3D environment based on real-world coordinates.
- ⚔️ **Combat System** — Players can walk to battle locations, fight a single enemy, and earn rewards upon victory.
- 🎮 **Stats & Progression** — Upgrade your character and equipment using coins, items, and artifacts.
- 🔐 **Client-Server Architecture**:
  - Account registration and login
  - JWT-based session management
  - Secure save/load of player data
  - Server-side validation of all in-game actions

---

## 🧩 Tech Stack

### Client (Unity):
- Unity 2022+
- C#
- Tiled map made with blenderOSM with chunk download from server
- Unity Location Service (GPS)
- Unity UI
- Custom battle system
- HTTP communication via JSON

### Server (ASP.NET Core):
- ASP.NET Core Web API
- JWT authentication
- MongoDB (item storage)
- SQL Server Express (for accounts)
- Entity Framework Core
