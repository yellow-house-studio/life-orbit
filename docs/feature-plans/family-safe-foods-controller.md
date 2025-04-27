# 📄 Feature Plan: Family SafeFoods Controller

---

## 1. Overview
- This feature refactors SafeFood add and remove endpoints from the large `FamilyController` into a new, dedicated `FamilySafeFoodsController`.
- Only add and remove operations are exposed (no update endpoint).
- Endpoints require authentication; the user must own the target FamilyMember.
- The repository interface (`IFamilyMemberRepository`) will be extended for safe food operations (e.g., `HasSafeFoodAsync`, `TrackNewSafeFood`).
- Validators must have unit tests (not integration tests).
- Remove SafeFood endpoints from `FamilyController` and update documentation.
- Follow the same controller and route pattern as `FamilyAllergiesController`.
- All new repository methods must have integration tests.

---

## 2. Endpoints Overview
- `PUT /settings/family/{familyMemberId}/safe-foods` → Add a safe food for a family member
- `DELETE /settings/family/{familyMemberId}/safe-foods/{foodItem}` → Remove a safe food for a family member

---

## 3. Command and Query Objects
Define Command/Query DTOs.

| Name | Purpose | File Path | Related Tests | Used Directly as Request? |
|:---|:---|:---|:---|:---|
| `AddSafeFoodCommand` | Represents request to add a safe food item | `server/YellowHouseStudio.LifeOrbit.Application/Family/AddSafeFood/AddSafeFoodCommand.cs` | Unit/Integration | Yes |
| `RemoveSafeFoodCommand` | Represents request to remove a safe food item | `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveSafeFood/RemoveSafeFoodCommand.cs` | Unit/Integration | Yes |
| `AddSafeFoodRequest` | Request DTO for adding a safe food | `server/YellowHouseStudio.LifeOrbit.Application/Family/Common/AddSafeFoodRequest.cs` | N/A | Yes |
| `SafeFoodResult` | Result DTO for safe food operations | `server/YellowHouseStudio.LifeOrbit.Application/Family/Common/SafeFoodResult.cs` | N/A | No |

**AddSafeFoodRequest fields:**
| Field Name | Type | Required? | Description |
|:---|:---|:---|:---|
| `FoodItem` | `string` | Yes | Name of the safe food item (max 100 chars) |

**SafeFoodResult fields:**
| Field Name | Type | Required? | Description |
|:---|:---|:---|:---|
| `FamilyMemberId` | `Guid` | Yes | The family member's ID |
| `SafeFoods` | `List<string>` | Yes | List of all safe foods for the member |

---

## 4. Repository Interfaces (Initial Version)
| Name | Purpose | File Path | Related Tests |
|:---|:---|:---|:---|
| `IFamilyMemberRepository` (extended) | Abstracts persistence for family member and safe foods (e.g., `HasSafeFoodAsync`, `TrackNewSafeFood`) | `server/YellowHouseStudio.LifeOrbit.Application/Family/Common/IFamilyMemberRepository.cs` | Integration: `server/YellowHouseStudio.LifeOrbit.Tests.Integration/Infrastructure/Repositories/FamilyMemberRepositoryTests.cs` |

---

## 5. Validators
| Name | Purpose | File Path | Related Tests |
|:---|:---|:---|:---|
| `AddSafeFoodCommandValidator` | Validates add safe food command | `server/YellowHouseStudio.LifeOrbit.Application/Family/AddSafeFood/AddSafeFoodCommandValidator.cs` | Unit: `server/YellowHouseStudio.LifeOrbit.Tests.Unit/Application/Family/AddSafeFood/AddSafeFoodCommandValidatorTests.cs` |
| `RemoveSafeFoodCommandValidator` | Validates remove safe food command | `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveSafeFood/RemoveSafeFoodCommandValidator.cs` | Unit: `server/YellowHouseStudio.LifeOrbit.Tests.Unit/Application/Family/RemoveSafeFood/RemoveSafeFoodCommandValidatorTests.cs` |

---

## 6. Domain Logic
| Name | Purpose | File Path | Related Tests |
|:---|:---|:---|:---|
| `FamilyMember` (Entity) | Aggregate root for safe foods | `server/YellowHouseStudio.LifeOrbit.Domain/Family/FamilyMember.cs` | Unit: `server/YellowHouseStudio.LifeOrbit.Tests.Unit/Domain/Family/FamilyMemberTests.cs` |
| `SafeFood` (Entity) | Value object for safe food | `server/YellowHouseStudio.LifeOrbit.Domain/Family/SafeFood.cs` | Unit: `server/YellowHouseStudio.LifeOrbit.Tests.Unit/Domain/Family/SafeFoodTests.cs` |

---

## 7. Handlers
| Name | Purpose | File Path | Related Tests |
|:---|:---|:---|:---|
| `AddSafeFoodCommandHandler` | Handles adding a safe food | `server/YellowHouseStudio.LifeOrbit.Application/Family/AddSafeFood/AddSafeFoodCommandHandler.cs` | Unit + Integration |
| `RemoveSafeFoodCommandHandler` | Handles removing a safe food | `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveSafeFood/RemoveSafeFoodCommandHandler.cs` | Unit + Integration |

---

## 8. Repository Implementation
| Name | Purpose | File Path | Related Tests |
|:---|:---|:---|:---|
| `FamilyMemberRepository` (extended) | Implements IFamilyMemberRepository for safe food operations | `server/YellowHouseStudio.LifeOrbit.Infrastructure/Repositories/FamilyMemberRepository.cs` | Integration: `server/YellowHouseStudio.LifeOrbit.Tests.Integration/Infrastructure/Repositories/FamilyMemberRepositoryTests.cs` |

---

## 9. Controller Methods
| Controller | Endpoint | Purpose | File Path | Related Tests |
|:---|:---|:---|:---|:---|
| `FamilySafeFoodsController` | `PUT /settings/family/{familyMemberId}/safe-foods` | Add a safe food for a family member | `server/YellowHouseStudio.LifeOrbit.Api/Controllers/FamilySafeFoodsController.cs` | API: `server/YellowHouseStudio.LifeOrbit.Tests.API/Controllers/FamilySafeFoodsControllerTests.cs` |
| `FamilySafeFoodsController` | `DELETE /settings/family/{familyMemberId}/safe-foods/{foodItem}` | Remove a safe food for a family member | `server/YellowHouseStudio.LifeOrbit.Api/Controllers/FamilySafeFoodsController.cs` | API: `server/YellowHouseStudio.LifeOrbit.Tests.API/Controllers/FamilySafeFoodsControllerTests.cs` |

**Controller Method Signatures:**

```csharp
// Add safe food
[HttpPut("settings/family/{familyMemberId}/safe-foods")]
public async Task<ActionResult<SafeFoodResult>> AddSafeFood(Guid familyMemberId, [FromBody] AddSafeFoodRequest request)

// Remove safe food
[HttpDelete("settings/family/{familyMemberId}/safe-foods/{foodItem}")]
public async Task<ActionResult<SafeFoodResult>> RemoveSafeFood(Guid familyMemberId, string foodItem)
```

---

## 10. Swagger Documentation Plan
- List all endpoints:
  - PUT `/settings/family/{familyMemberId}/safe-foods` (Add safe food)
  - DELETE `/settings/family/{familyMemberId}/safe-foods/{foodItem}` (Remove safe food)
- Expected requests:
  - Add: `AddSafeFoodRequest` body
  - Remove: `familyMemberId` route param, `foodItem` route param
- Expected responses:
  - 200 OK: `SafeFoodResult`
  - 400 BadRequest: Validation errors
  - 404 NotFound: Family member or food not found
- XML summaries and OpenAPI annotations must be added to all controller methods.

---

## 11. Special Notes
- **Authorization:** Endpoints require authentication; user must own the target FamilyMember (same as `FamilyAllergiesController`).
- **Error Handling:** Return 404 if family member not found, 400 for validation errors (including if safe food is not found for removal).
- **Versioning:** Follows current API versioning strategy.
- **Deprecation:** Remove SafeFood endpoints from `FamilyController` and update documentation.
- **Testing:** Ensure new controller is fully covered by API, unit, and repository integration tests at all levels as defined by the plan template.

---

## 📑 Mandatory Naming and Model Rules
- Commands/Queries: [Action][Target]Command
- Results: [Action][Target]Result
- Request Objects (only if needed): [Action][Target]Request
- Shared input/output objects: Add Dto suffix (e.g., SafeFoodDto).
- Otherwise: No Dto suffix.

---

**Feature Plan Finalized.** 