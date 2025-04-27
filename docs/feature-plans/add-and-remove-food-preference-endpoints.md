# 📄 Feature Plan: Add and Remove Food Preference Endpoints

---

## 1. Overview

This feature introduces dedicated, well-defined endpoints for **adding** and **removing** food preferences for a family member. The current implementation is embedded in the `FamilyController` and is not aligned with best practices for separation of concerns, validation, and repository usage. The new endpoints will:
- Use a dedicated controller (`FamilyFoodPreferencesController`)
- Follow clean architecture and repository patterns
- Provide robust validation and error handling
- Remove the old endpoints from `FamilyController`

**Special Considerations:**
- Requires authentication (user must own the family member)
- Handles edge cases (duplicate, not found, etc.)
- Versioning: v1, no breaking changes to other endpoints

---

## 2. Endpoints Overview

- `POST /settings/family/{familyMemberId}/preferences` → Add a food preference for a family member (fails if already exists)
- `DELETE /settings/family/{familyMemberId}/preferences/{foodItem}` → Remove a food preference from a family member

---

## 3. Command and Query Objects

| Name                          | Purpose                                         | File Path                                                                 | Related Tests | Used Directly as Request? |
|-------------------------------|-------------------------------------------------|--------------------------------------------------------------------------|---------------|---------------------------|
| `AddFoodPreferenceCommand`    | Add a food preference                           | `Application/Family/AddFoodPreference/AddFoodPreferenceCommand.cs`        | Yes           | Yes                       |
| `RemoveFoodPreferenceCommand` | Remove a food preference                        | `Application/Family/RemoveFoodPreference/RemoveFoodPreferenceCommand.cs`  | Yes           | Yes                       |

**AddFoodPreferenceCommand Fields:**

| Field Name       | Type    | Required? | Description                                 |
|------------------|---------|-----------|---------------------------------------------|
| FamilyMemberId   | Guid    | Yes       | The family member's ID                      |
| FoodItem         | string  | Yes       | The food item name                          |
| Status           | string  | Yes       | Preference status: Include, AvailableForOthers, NotAllowed |

**RemoveFoodPreferenceCommand Fields:**

| Field Name       | Type    | Required? | Description                                 |
|------------------|---------|-----------|---------------------------------------------|
| FamilyMemberId   | Guid    | Yes       | The family member's ID                      |
| FoodItem         | string  | Yes       | The food item name                          |

---

## 4. Repository Interfaces (Initial Version)

| Name                    | Purpose                                         | File Path                                                                 | Related Tests |
|-------------------------|-------------------------------------------------|--------------------------------------------------------------------------|---------------|
| `IFamilyMemberRepository` | Abstracts family member and child entity ops   | `Application/Family/Common/IFamilyMemberRepository.cs`                   | Integration   |

**Additions to Interface:**
- `Task<bool> HasFoodPreferenceAsync(Guid id, string foodItem, CancellationToken cancellationToken);`
- `void TrackNewFoodPreference(FamilyMember familyMember, FoodPreference foodPreference);`
- `void TrackRemoveFoodPreference(FamilyMember familyMember, FoodPreference foodPreference);`

---

## 5. Validators

| Name                              | Purpose                                         | File Path                                                                 | Related Tests |
|------------------------------------|-------------------------------------------------|--------------------------------------------------------------------------|---------------|
| `AddFoodPreferenceCommandValidator`    | Validates add food preference input            | `Application/Family/AddFoodPreference/AddFoodPreferenceCommandValidator.cs` | Unit          |
| `RemoveFoodPreferenceCommandValidator` | Validates remove food preference input         | `Application/Family/RemoveFoodPreference/RemoveFoodPreferenceCommandValidator.cs` | Unit          |

---

## 6. Domain Logic

| Name           | Purpose                                         | File Path                                               | Related Tests |
|----------------|-------------------------------------------------|--------------------------------------------------------|---------------|
| `FoodPreference` (Entity) | Represents a food preference         | `Domain/Family/FoodPreference.cs`                      | Unit          |
| `FamilyMember` (Aggregate) | Manages food preferences            | `Domain/Family/FamilyMember.cs`                        | Unit          |

---

## 7. Handlers

| Name                                  | Purpose                                         | File Path                                                                 | Related Tests |
|----------------------------------------|-------------------------------------------------|--------------------------------------------------------------------------|---------------|
| `AddFoodPreferenceCommandHandler`      | Handles adding a food preference                | `Application/Family/AddFoodPreference/AddFoodPreferenceCommandHandler.cs` | Unit + Integration |
| `RemoveFoodPreferenceCommandHandler`   | Handles removing a food preference              | `Application/Family/RemoveFoodPreference/RemoveFoodPreferenceCommandHandler.cs` | Unit + Integration |

---

## 8. Repository Implementation

| Name                    | Purpose                                         | File Path                                                                 | Related Tests |
|-------------------------|-------------------------------------------------|--------------------------------------------------------------------------|---------------|
| `FamilyMemberRepository` | Implements IFamilyMemberRepository              | `Infrastructure/Repositories/FamilyMemberRepository.cs`                  | Integration   |

**Additions:**
- Implement `TrackNewFoodPreference` and `TrackRemoveFoodPreference` methods

---

## 9. Controller Methods

| Controller                    | Endpoint                                               | Purpose                                         | File Path                                               | Related Tests |
|-------------------------------|-------------------------------------------------------|-------------------------------------------------|--------------------------------------------------------|---------------|
| `FamilyFoodPreferencesController` | `POST /settings/family/{familyMemberId}/preferences` | Add a food preference                           | `Api/Controllers/FamilyFoodPreferencesController.cs`    | API           |
| `FamilyFoodPreferencesController` | `DELETE /settings/family/{familyMemberId}/preferences/{foodItem}` | Remove a food preference                        | `Api/Controllers/FamilyFoodPreferencesController.cs`    | API           |

**Controller Method Signatures:**

```csharp
[HttpPost("{familyMemberId}/preferences")]
public async Task<ActionResult<FamilyMemberResponse>> AddFoodPreference(Guid familyMemberId, [FromBody] AddFoodPreferenceCommand command)

[HttpDelete("{familyMemberId}/preferences/{foodItem}")]
public async Task<ActionResult<FamilyMemberResponse>> RemoveFoodPreference(Guid familyMemberId, string foodItem)
```

---

## 10. Swagger Documentation Plan

- Document both endpoints with request/response examples
- List valid values for `Status`
- Document error responses (404, 400, 403)

---

## 11. Special Notes

- **Authorization:** Only the owner of the family member can add/remove preferences
- **Error Cases:** Not found, duplicate, invalid status, unauthorized
- **Versioning:** v1, no breaking changes

---

## 📑 Mandatory Naming and Model Rules

- Commands/Queries: [Action][Target]Command (e.g., AddFoodPreferenceCommand)
- Results: [Action][Target]Result (if needed)
- Request Objects: [Action][Target]Request (only if needed)
- Shared input/output objects: Add Dto suffix (e.g., FoodPreferenceDto)
- Otherwise: No Dto suffix

---

## Next Steps

- Implement new command, handler, validator, and repository methods for AddFoodPreference.
- Implement new controller (`FamilyFoodPreferencesController`) and endpoints.
- Remove old food preference endpoints from `FamilyController`.
- Add and update tests for new logic and endpoints.
- Update Swagger documentation.

---

**Feature Plan Finalized.**

This plan will be saved to `docs/feature-plans/add-and-remove-food-preference-endpoints.md`.  
Ready for implementation! 