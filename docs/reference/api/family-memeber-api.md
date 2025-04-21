# Family API Reference

This document provides detailed information about the Family API endpoints in the LifeOrbit application.

## Base URL

All endpoints are prefixed with `/settings/family`

## Endpoints

### Get Family Members

Retrieves a list of family members for a specific user.

```http
GET /settings/family?userId={userId}
```

#### Parameters

| Name    | Type   | In    | Required | Description                                |
|---------|--------|-------|----------|--------------------------------------------|
| userId  | UUID   | query | Yes      | The ID of the user to get family members for |

#### Response

```typescript
{
  "id": "UUID",
  "name": "string",
  "age": "number",
  "allergies": [
    {
      "allergen": "string",
      "severity": "string" // "AvailableForOthers" or "NotAllowed"
    }
  ],
  "safeFoods": [
    {
      "foodItem": "string"
    }
  ],
  "foodPreferences": [
    {
      "foodItem": "string",
      "status": "string" // "Include", "AvailableForOthers", or "NotAllowed"
    }
  ]
}[]
```

#### Response Codes

| Code | Description                                           |
|------|-------------------------------------------------------|
| 200  | Success. Returns list of family members                |
| 400  | Bad Request. Invalid userId format                     |
| 500  | Internal Server Error                                  |

### Add Family Member

Adds a new family member for a user.

```http
POST /settings/family
```

#### Request Body

```typescript
{
  "userId": "UUID",
  "name": "string",
  "age": "number"
}
```

#### Validation Rules

- **name**
  - Required
  - Maximum length: 100 characters
- **age**
  - Required
  - Must be between 0 and 120
- **userId**
  - Required
  - Must be a valid UUID

#### Response

```typescript
"UUID" // The ID of the newly created family member
```

#### Response Codes

| Code | Description                                           |
|------|-------------------------------------------------------|
| 200  | Success. Returns the ID of the created family member   |
| 400  | Bad Request. Invalid input or validation errors        |
| 500  | Internal Server Error                                  |

### Add Complete Family Member

Creates a new family member record with all associated details including allergies, safe foods, and food preferences.

```http
POST /settings/family/complete
```

#### Request Body

```typescript
{
  "userId": "UUID",
  "name": "string",
  "age": "number",
  "allergies": [
    {
      "allergen": "string",
      "severity": "string" // "AvailableForOthers" or "NotAllowed"
    }
  ],
  "safeFoods": [
    {
      "foodItem": "string"
    }
  ],
  "foodPreferences": [
    {
      "foodItem": "string",
      "status": "string" // "Include", "AvailableForOthers", or "NotAllowed"
    }
  ]
}
```

#### Validation Rules

- **name**
  - Required
  - Maximum length: 100 characters
- **age**
  - Required
  - Must be between 0 and 120
- **userId**
  - Required
  - Must be a valid UUID
- **allergies**
  - Optional
  - Array of valid Allergy objects
- **safeFoods**
  - Optional
  - Array of valid SafeFood objects
- **foodPreferences**
  - Optional
  - Array of valid FoodPreference objects

#### Response

```typescript
{
  "id": "UUID",          // The ID of the newly created family member
  "name": "string",
  "age": "number",
  "allergies": Allergy[],
  "safeFoods": SafeFood[],
  "foodPreferences": FoodPreference[]
}
```

#### Response Codes

| Code | Description                                           |
|------|-------------------------------------------------------|
| 201  | Created. Returns the complete family member object     |
| 400  | Bad Request. Invalid input or validation errors        |
| 500  | Internal Server Error                                  |

## Data Models

### FamilyMember

The core entity representing a family member in the system.

```typescript
{
  "id": "UUID",          // Unique identifier
  "userId": "UUID",      // Owner of the family member record
  "name": "string",      // Name of the family member
  "age": "number",       // Age of the family member
  "allergies": Allergy[],
  "safeFoods": SafeFood[],
  "foodPreferences": FoodPreference[]
}
```

### Allergy

Represents an allergy for a family member.

```typescript
{
  "allergen": "string",   // Name of the allergen
  "severity": "string"    // Severity level: "AvailableForOthers" or "NotAllowed"
}
```

### SafeFood

Represents a safe food item for a family member (particularly relevant for selective eaters).

```typescript
{
  "foodItem": "string"    // Name of the safe food item
}
```

### FoodPreference

Represents a food preference for a family member.

```typescript
{
  "foodItem": "string",   // Name of the food item
  "status": "string"      // Preference status: "Include", "AvailableForOthers", or "NotAllowed"
}
```

## Error Handling

The API uses standard HTTP response codes to indicate the success or failure of requests. For validation errors, a detailed error response will be returned:

```typescript
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "propertyName": [
      "error message"
    ]
  }
}
```

## Notes

- All endpoints require authentication (not shown in this document)
- All timestamps are in UTC
- The API follows RESTful conventions
- Request and response bodies are in JSON format 