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

## Test Data Builders
Located in `YellowHouseStudio.LifeOrbit.Tests.Common/Builders`, builders help create test data consistently:

### Builder Pattern
- Use fluent builder pattern for readability
- Provide sensible defaults
- Allow customization of key properties
- Support complex object creation

Example:
```csharp
// Creating a test family member with allergies
var member = new FamilyMemberBuilder()
    .WithName("Test User")
    .WithAge(30)
    .WithAllergy("Peanuts", AllergySeverity.NotAllowed)
    .Build();

// Creating a test command
var command = new AddAllergyCommandBuilder()
    .WithFamilyMemberId(memberId)
    .WithAllergen("Milk")
    .WithSeverity(AllergySeverity.NotAllowed)
    .Build();
```

### Builder Guidelines
1. Place builders in the Tests.Common project
2. Follow naming convention: `{EntityName}Builder`
3. Provide fluent interface with `With{Property}` methods
4. Include sensible defaults for all required properties
5. Support building related entities when needed
6. Include XML documentation for complex builders

### When to Use Builders
- Creating complex domain entities
- Setting up test data with specific properties
- When the same test data structure is used in multiple tests
- For commands and queries with multiple properties

## Test Categories
Tests should be categorized using NUnit attributes:

```csharp
[TestFixture]
[Category("Integration")]
public class AddAllergyCommandHandlerTests
{
    [Test]
    [Category("Happy Path")]
    public async Task Handle_adds_allergy_to_family_member() { }

    [Test]
    [Category("Error Handling")]
    public async Task Handle_throws_not_found_for_non_existent_family_member() { }
}
```

Common categories:
- Unit
- Integration
- API
- Happy Path
- Error Handling
- Performance
- Security

## Test Data Management
1. Use constants for common test values
2. Avoid magic strings/numbers
3. Create helper methods for complex setup
4. Use builders for consistent test data
5. Keep test data realistic but simple

## Assertion Best Practices
1. Use FluentAssertions for readable assertions
2. Test one concept per test method
3. Make assertion failures descriptive
4. Include relevant property checks
5. Verify both positive and negative cases

Example:
```csharp
// Good - Clear and specific assertions
result.Should().NotBeNull();
result.Id.Should().Be(expectedId);
result.Name.Should().Be("Test User");
result.Allergies.Should().HaveCount(1);
result.Allergies.First().Allergen.Should().Be("Peanuts");

// Avoid - Too generic
result.Should().BeEquivalentTo(expected);
```

## Integration Test Guidelines
1. Use TestBase for common setup
2. Clean up test data after each test
3. Use realistic scenarios
4. Test through the full pipeline
5. Include error cases
6. Verify database state

## API Test Guidelines
1. Test complete HTTP lifecycle
2. Verify response status codes
3. Check response content
4. Test validation errors
5. Include authorization tests
6. Use test data builders 

Example:
```csharp
[Test]
public void CreateAllergy_WithValidData_ShouldCreateAllergy()
{
    // Arrange
    var allergyBuilder = new AllergyBuilder()
        .WithName("Peanuts")
        .WithSeverity(AllergySeverity.NotAllowed);  // Using correct enum value

    // Act
    var allergy = allergyBuilder.Build();

    // Assert
    allergy.Should().NotBeNull();
    allergy.Name.Should().Be("Peanuts");
    allergy.Severity.Should().Be(AllergySeverity.NotAllowed);
}
``` 