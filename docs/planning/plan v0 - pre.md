# MealPlanner Design & Specification Document

## 1. Overview
MealPlanner is an API-based meal planning application with an Angular frontend, designed to integrate with **Grocy** and utilize **AI-generated meal suggestions**. The app ensures that family preferences, allergies, and dietary restrictions are followed while automatically managing missing recipes in Grocy.

## 2. Scope
### **Version 1 (V1) Scope**
- Generate a **7-day meal plan** based on a specified start date.
- Modify an existing **7-day meal plan** based on a start date.
- Store **family preferences, allergies, and dietary restrictions**.
- Integrate with **Grocy API** to fetch existing meals and store meal plans.
- Use **AI suggestions** for generating new meal plans.
- Automatically **create missing recipes in Grocy** only after the menu is accepted.

### **Excluded from V1**
- Manual recipe management.
- Authentication (planned for future versions).
- Persisting `MealPlan` in a database (handled only in memory during active session).

---

## 3. API Endpoints
All API routes will follow REST conventions and use JWT authentication in future versions.

### **1️⃣ POST /mealplan/generate**
Generate a new meal plan from a specified start date.
- **Body:**
```json
{
  "startDate": "2025-04-22"
}
```
- **Response:** 7-day proposed meal plan, including indicators for new recipes and AI suggestions.

### **2️⃣ POST /mealplan/update**
Update an existing meal plan for a specified start date.
- **Body:**
```json
{
  "startDate": "2025-04-22",
  "meals": [
    { "day": 1, "name": "Köttbullar och potatis" }
  ]
}
```
- **Behavior:** Updates Grocy and creates new recipes if required.

### **3️⃣ GET /settings/family**
Retrieve all family members and their food preferences.

### **4️⃣ POST /settings/family**
Add a new family member.
- **Body:**
```json
{
  "name": "Phoenix"
}
```

### **5️⃣ PUT /settings/family/{id}**
Update a family member's food preferences and dietary labels.

### **6️⃣ DELETE /settings/family/{id}**
Remove a family member.

### **7️⃣ POST /settings/family/{id}/preferences**
Add or update a food preference for a family member.
- **Body:**
```json
{
  "foodItem": "ris",
  "status": "include"
}
```

### **8️⃣ DELETE /settings/family/{id}/preferences/{foodItem}**
Remove a food preference for a family member.

### **9️⃣ PUT /settings/family/{id}/diet**
Update dietary preferences.
- **Body:**
```json
{
  "dietaryPreferences": ["keto", "lågkolhydrat"]
}
```

---

## 4. Teknisk Stack & Arkitektur
### **4.1 Teknologi-val**
- **Frontend:** Angular
- **Backend:** C# (.NET 8)
- **Database:** PostgreSQL (via Docker)
- **ORM:** Entity Framework Core + Npgsql
- **AI Integration:** Microsoft.Extensions.AI (stöd för OpenAI och lokala modeller som Ollama)
- **Dependency Injection:** .NET built-in DI
- **Logging:** Serilog + Seq
- **Security:** API keys for Grocy handled securely
- **Deployment:** Docker + Docker Compose

### **4.2 Projektstruktur**
```
MealPlanner/
│── src/
│   ├── MealPlanner.API/            # REST API
│   ├── MealPlanner.Application/    # Business logic + MediatR handlers
│   ├── MealPlanner.Infrastructure/ # Database (EF Core, PostgreSQL), Grocy API integration
│   ├── MealPlanner.Domain/         # Entities & core logic
│── frontend/                       # Angular frontend
│── docker-compose.yml              # Defines PostgreSQL & app containers
│── Dockerfile                      # Build file for .NET app
│── README.md
```

---

## 5. Data Model & Storage Strategy
### **5.1 Entities**
#### **1️⃣ MealPlan (Stored in Memory Only)**
`MealPlan` is **not persisted in a database** and exists only during the active session.
```json
{
    "StartDate": "YYYY-MM-DD",
    "Meals": [
        { "Day": 1, "RecipeId": 12, "Name": "Spaghetti Bolognese", "IsAI": false }
    ]
}
```

#### **2️⃣ FamilyMembers & Preferences (Unified)**
```sql
CREATE TABLE FamilyMembers (
    Id UUID PRIMARY KEY,
    UserId UUID REFERENCES Users(Id),
    Name TEXT NOT NULL
);

CREATE TABLE FoodPreferences (
    Id UUID PRIMARY KEY,
    FamilyMemberId UUID REFERENCES FamilyMembers(Id),
    FoodItem TEXT NOT NULL,
    Status TEXT CHECK (Status IN ('include', 'available_for_others', 'not_allowed'))
);
```
- **FoodItem** lagrar livsmedlet.
- **Status** har tre möjliga värden:
  - `'include'` → Trygg mat för selektiv ätare.
  - `'available_for_others'` → Kan lagas men inte ätas av individen.
  - `'not_allowed'` → Inte ens tillåten hemma.

---

## 6. Integrations
### **Grocy API Integration**
- **Fetch last two weeks' meals** from Grocy.
- **Check for missing recipes** in Grocy.
- **Create new recipes** in Grocy when a menu is accepted.

### **AI Integration (Microsoft.Extensions.AI)**
- Generate a **7-day meal plan** based on:
  - Past meals.
  - Family preferences.
  - Allergies & dietary settings.
- Modify plans based on **user feedback**.

---

## 7. Logging & Security
### **Logging (Serilog + Seq)**
- Log meal plan generation.
- Log Grocy API interactions.
- Log AI calls and responses.

### **Security Considerations**
- **API keys for Grocy** stored securely.
- **Prepared for .NET Core Authentication** (but not implemented in V1).

---

## 8. Future Considerations
- **Meal rating & tracking improvements**
- **Shopping list integration**
- **Multi-user support**
- **Support for CLI interface**
- **Offline-first mode**
- **Mobile-friendly frontend**
