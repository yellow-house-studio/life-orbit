# Testing Strategy

## Test Projects Organization

### Unit Tests (`YellowHouseStudio.LifeOrbit.Tests.Unit`)
- Location: `server/YellowHouseStudio.LifeOrbit.Tests.Unit`
- Purpose: Test individual components in isolation
- Characteristics:
  - No database dependencies
  - No file system access
  - No network calls
  - Fast execution
  - Uses mocks/stubs when needed
- Example: Testing domain model behavior, value objects, pure functions
- Structure follows the solution structure:
  ```
  Tests.Unit/
  ├── Domain/
  │   └── Family/
  │       ├── FamilyMemberTests.cs
  │       └── ...
  ├── Application/
  │   └── Services/
  └── Infrastructure/
      └── Services/
  ```

### Integration Tests (`YellowHouseStudio.LifeOrbit.Tests.Integration`)
- Location: `server/YellowHouseStudio.LifeOrbit.Tests.Integration`
- Purpose: Test component interactions
- Characteristics:
  - Uses real database (usually in-memory)
  - Tests multiple layers working together
  - May use real file system
  - Slower than unit tests
- Example: Testing command/query handlers with EF Core
- Structure follows the solution structure:
  ```
  Tests.Integration/
  ├── Application/
  │   └── Family/
  │       ├── AddFamilyMemberCommandHandlerTests.cs
  │       └── ...
  └── Infrastructure/
      └── Repositories/
  ```

### API Tests (`YellowHouseStudio.LifeOrbit.Tests.API`)
- Location: `server/YellowHouseStudio.LifeOrbit.Tests.API`
- Purpose: End-to-end testing through HTTP endpoints
- Characteristics:
  - Tests full HTTP request/response cycle
  - Uses test server
  - Tests authentication/authorization
  - Tests API contracts
  - Slowest execution time
- Note: Could be renamed to E2E, but API Tests is also a common convention that clearly indicates the entry point
- Structure follows API endpoints:
  ```
  Tests.API/
  ├── Family/
  │   ├── AddFamilyMemberTests.cs
  │   └── ...
  └── Settings/
      └── ...
  ```

## Test Naming Conventions

All test methods follow the pattern:
```csharp
MethodName_action_subject_condition()
```

Example:
```csharp
Constructor_creates_family_member_with_valid_properties()
AddAllergy_adds_new_allergy_when_allergen_does_not_exist()
```

## Test Organization

Each test follows the Arrange-Act-Assert pattern:
```csharp
[Test]
public void Method_action_subject_condition()
{
    // Arrange
    // Set up test data and conditions

    // Act
    // Perform the action being tested

    // Assert
    // Verify the results
}
```

## Testing Tools
- NUnit: Testing framework
- FluentAssertions: Readable assertions
- Mock (when needed): Mocking framework
- EF Core In-Memory Database: For integration tests
- TestServer: For API tests

## Best Practices
1. One assertion concept per test
2. Descriptive test names that explain the scenario
3. Use appropriate test type for the scenario
4. Keep unit tests fast and isolated
5. Use integration tests for database operations
6. Use API tests for end-to-end scenarios
7. Follow AAA pattern
8. Use meaningful test data 