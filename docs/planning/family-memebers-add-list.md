
# Family Members – Feature Planning

This document outlines the requirements, API design, data model, and UI considerations for managing family members in the MealPlanner app.

---

## 🎯 Purpose

Support adding and viewing family members, including detailed information on:
- Name and age
- Allergies with severity (available_for_others / not_allowed)
- Safe foods for selective eaters
- General food preferences

---

## 📦 Data Model

```sql
CREATE TABLE FamilyMembers (
    Id UUID PRIMARY KEY,
    UserId UUID REFERENCES Users(Id),
    Name TEXT NOT NULL,
    Age INT NOT NULL
);

CREATE TABLE Allergies (
    Id UUID PRIMARY KEY,
    FamilyMemberId UUID REFERENCES FamilyMembers(Id),
    Allergen TEXT NOT NULL,
    Severity TEXT CHECK (Severity IN ('available_for_others', 'not_allowed'))
);

CREATE TABLE SafeFoods (
    Id UUID PRIMARY KEY,
    FamilyMemberId UUID REFERENCES FamilyMembers(Id),
    FoodItem TEXT NOT NULL
);

CREATE TABLE FoodPreferences (
    Id UUID PRIMARY KEY,
    FamilyMemberId UUID REFERENCES FamilyMembers(Id),
    FoodItem TEXT NOT NULL,
    Status TEXT CHECK (Status IN ('include', 'available_for_others', 'not_allowed'))
);
```

- **Allergies**: For foods that are harmful, including whether others can eat them.
- **SafeFoods**: For selective eaters — foods considered safe and preferred.
- **FoodPreferences**: General preferences not tied to medical or developmental needs.

---

## 🧠 API Design

### ✅ Get All Family Members
`GET /settings/family`
- Returns all family members and their full profile:
  - Age
  - Allergies
  - SafeFoods
  - FoodPreferences

### ➕ Add Family Member
`POST /settings/family`
```json
{
  "name": "Phoenix",
  "age": 5
}
```

### ✏️ Update Family Member
`PUT /settings/family/{id}`
```json
{
  "name": "Phoenix",
  "age": 6
}
```

### 🗑️ Delete Family Member
`DELETE /settings/family/{id}`

### 🔁 Add/Update Allergy
`POST /settings/family/{id}/allergies`
```json
{
  "allergen": "nuts",
  "severity": "not_allowed"
}
```

### ❌ Remove Allergy
`DELETE /settings/family/{id}/allergies/{allergen}`

### 💚 Add Safe Food
`POST /settings/family/{id}/safefoods`
```json
{
  "foodItem": "pasta"
}
```

### ❌ Remove Safe Food
`DELETE /settings/family/{id}/safefoods/{foodItem}`

### 🍽️ Add/Update Food Preference
`POST /settings/family/{id}/preferences`
```json
{
  "foodItem": "ris",
  "status": "include"
}
```

### ❌ Remove Food Preference
`DELETE /settings/family/{id}/preferences/{foodItem}`

---

## 💻 UI Planning

### View List of Family Members 
- Shows name, age
- Link to profile

### Add New Family Member
- Form with:
  - Name
  - Age
  - Allergy list (with severity selector)
  - Safe foods (multi-select or tags)
  - General preferences (with status)

### Edit Family Member
- Same as Add, but pre-filled with existing values

---

## 🔜 Next Steps

- [ ] Design API endpoints
- [ ] Create Angular services and interfaces
- [ ] Build UI components for list, add, and edit
