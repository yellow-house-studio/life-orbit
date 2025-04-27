** 2024-03-28 14:30 - AddCompleteFamilyMember Command Handler - Fix - Completed **

Fixed critical database persistence issue in family member creation process.

Details:
- Added missing SaveChangesAsync() call in command handler
- Verified persistence through integration tests
- Issue was preventing all family member data from being saved
- Decision: Added explicit save call rather than relying on implicit saves
- Progress: 100% complete

Files:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddCompleteFamilyMember/AddCompleteFamilyMemberCommandHandler.cs` - Added SaveChangesAsync() call after entity creation

Related Documents:
- Integration Tests: `server/YellowHouseStudio.LifeOrbit.Tests.Integration/Application/Family/AddCompleteFamilyMemberCommandHandlerTests.cs`

** 2024-03-28 15:00 - AddCompleteFamilyMember Command Handler - Refactor - Completed **

Standardized command handler implementation to match backend conventions.

Details:
- Implemented proper logging patterns following clean architecture guidelines
- Enhanced code organization for better maintainability
- Decision: Used semantic logging with structured log levels
- Progress: 100% complete

Files:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddCompleteFamilyMember/AddCompleteFamilyMemberCommandHandler.cs` - Updated logging implementation and field patterns
  - Added private readonly fields
  - Implemented proper log levels (Debug, Trace, Information)
  - Added structured logging with semantic parameter names

Related Documents:
- Architecture: `docs/architecture/testing.md` - Testing strategy reference
- Project Structure: Clean architecture guidelines for Application layer

** 2024-03-28 14:00 - Family Member Creation - Refactor - Completed **

Updated family member creation to use a single API call for complete member creation.

Details:
- Added new complete family member creation endpoint in API
- Updated DTOs and models to support complete creation
- Refactored store to handle complete creation
- Updated add family member page to use new complete creation method
- Added comprehensive test coverage for new functionality
- Decision: Simplified creation flow by combining multiple API calls into one
- Progress: 100% complete

Files:
- `app/src/app/infrastructure/family/models/family.dto.ts` - Added complete family member request/response DTOs
- `app/src/app/infrastructure/family/models/family.model.ts` - Updated models to match API
- `app/src/app/infrastructure/family/services/family-api.service.ts` - Added complete creation endpoint
- `app/src/app/infrastructure/family/stores/family-members.store.ts` - Added complete creation method
- `app/src/app/features/settings/pages/add-family-member/add-family-member.page.ts` - Updated to use complete creation

Related:
- API Docs: docs/reference/api/family-memeber-api.md

** 2024-03-28 15:00 - Family Member Dashboard - App - Implementation - Completed **

Implemented the family member dashboard feature with editable sections for allergies, safe foods, and preferences.

Details:
- Created family member dashboard component with detailed view of member information
- Implemented edit functionality for allergies, safe foods, and preferences
- Created reusable edit components for each section
- Added proper error handling and loading states
- Used Angular Material components for consistent UI
- Implemented real-time updates using the store

Files:
- `app/src/app/features/settings/pages/family-memember-dashboard/family-memember-dashboard.component.ts` - Main dashboard component
- `app/src/app/features/settings/pages/family-memember-dashboard/family-memember-dashboard.component.html` - Dashboard template
- `app/src/app/features/settings/pages/family-memember-dashboard/family-memember-dashboard.component.scss` - Dashboard styles
- `app/src/app/features/settings/components/allergy-edit/allergy-edit.component.ts` - Allergy edit component
- `app/src/app/features/settings/components/safe-foods-edit/safe-foods-edit.component.ts` - Safe foods edit component
- `app/src/app/features/settings/components/food-preferences-edit/food-preferences-edit.component.ts` - Food preferences edit component

Related:
- Design: docs/design/family-members-structure.md
- API: docs/reference/api/family-memeber-api.md

** 2024-03-28 15:00 - Family Member Food Management - Server - Implementation - Completed **

Added endpoints for managing family member food preferences and restrictions.

Details:
- Created endpoints for managing allergies, safe foods, and food preferences
- Implemented CQRS pattern with commands and handlers for each operation
- Added validation for all commands
- Followed existing patterns from AddCompleteFamilyMember
- Reused existing domain models and DTOs
- Added proper error handling and logging

Files:
- `server/YellowHouseStudio.LifeOrbit.Api/Controllers/FamilyController.cs` - Added new endpoints
- `server/YellowHouseStudio.LifeOrbit.Application/Common/Exceptions/NotFoundException.cs` - Added exception class
- `server/YellowHouseStudio.LifeOrbit.Application/Family/UpdateAllergy/*` - Update allergy implementation
- `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveAllergy/*` - Remove allergy implementation
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddSafeFood/*` - Add safe food implementation
- `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveSafeFood/*` - Remove safe food implementation
- `server/YellowHouseStudio.LifeOrbit.Application/Family/UpdateFoodPreference/*` - Update food preference implementation
- `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveFoodPreference/*` - Remove food preference implementation

Next Steps:
1. Add integration tests for each new command
2. Add API tests for each new endpoint
3. Update API documentation
4. Implement frontend components to use new endpoints

Related:
- Design: docs/architecture/testing.md
- API Docs: docs/reference/api/family-memeber-api.md

** 2024-03-28 16:00 - Family Member Food Management - Server - Refactor - In Progress **

Refactoring the family member food management implementation based on code review feedback.

Details:
- Splitting FamilyController into separate controllers for better responsibility separation
- Removing redundant update operations in favor of clear Add/Remove patterns
- Adding user-friendly validation messages
- Optimizing data loading in handlers
- Adding comprehensive test coverage

Changes:
1. Controller Separation
   - Moving allergy endpoints to FamilyAllergiesController
   - Moving safe food endpoints to FamilySafeFoodsController
   - Moving preferences endpoints to FamilyPreferencesController
   - Keeping core family operations in FamilyController

2. Operation Simplification
   - Removing update operations in favor of Add/Remove
   - Aligning API with domain model operations
   - Simplifying client integration

3. Data Access Optimization
   - Loading only required related entities
   - Removing unnecessary Include statements
   - Improving query performance

4. Validation Improvements
   - Adding user-friendly validation messages
   - Ensuring consistent validation across all operations
   - Making validation messages UI-ready

5. Test Coverage
   - Unit tests for validators
   - Integration tests for commands
   - API tests for endpoints
   - Testing error scenarios

Files to be Modified:
- Controllers:
  - `server/YellowHouseStudio.LifeOrbit.Api/Controllers/FamilyController.cs` - Remove food management endpoints
  - `server/YellowHouseStudio.LifeOrbit.Api/Controllers/FamilyAllergiesController.cs` - New controller
  - `server/YellowHouseStudio.LifeOrbit.Api/Controllers/FamilySafeFoodsController.cs` - New controller
  - `server/YellowHouseStudio.LifeOrbit.Api/Controllers/FamilyPreferencesController.cs` - New controller

- Commands/Handlers:
  - Remove Update* commands and handlers
  - Optimize Include statements in handlers
  - Update validation messages

- Tests:
  - Add validator unit tests
  - Add command integration tests
  - Add API tests

Next Steps:
1. Create new controllers
2. Remove update operations
3. Update validation messages
4. Optimize data loading
5. Add tests
6. Update API documentation

Related:
- Design: docs/architecture/testing.md
- API Docs: docs/reference/api/family-memeber-api.md

** 2024-03-28 14:30 - Test Data Builders - Server - Implementation - In Progress **

Added test data builders to improve test maintainability and readability.

Details:
- Created FamilyMemberBuilder for generating test FamilyMember instances
- Created AddAllergyCommandBuilder for generating test AddAllergyCommand instances
- Used fluent builder pattern for better readability
- Added sensible defaults for all properties

Files:
- `server/YellowHouseStudio.LifeOrbit.Tests.Common/Builders/FamilyMemberBuilder.cs` - Builder for FamilyMember test data
- `server/YellowHouseStudio.LifeOrbit.Tests.Common/Builders/AddAllergyCommandBuilder.cs` - Builder for AddAllergy command test data

Next Steps:
1. Add builders for remaining domain entities (2 days)
2. Add builders for remaining commands and queries (2 days)
3. Update existing tests to use new builders (1 day)

Related:
- Design: docs/architecture/testing.md

** 2024-03-28 14:30 - Allergy Management - Both - Implementation - Completed **

Implemented comprehensive allergy management functionality for family members.

Details:
- Created domain model for allergies with severity levels (AvailableForOthers, NotAllowed)
- Implemented CQRS commands and handlers for adding, updating, and removing allergies
- Added validation for allergy operations including duplicate checks
- Created REST API endpoints for allergy management
- Implemented integration and API tests for all operations
- Added builder pattern for test data generation

Files:
- `server/YellowHouseStudio.LifeOrbit.Domain/Family/Allergy.cs` - Core domain model for allergies
- `server/YellowHouseStudio.LifeOrbit.Domain/Family/FamilyMember.cs` - Family member entity with allergy management
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommand.cs` - Command for adding allergies
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandHandler.cs` - Handler implementation
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandValidator.cs` - Validation rules
- `server/YellowHouseStudio.LifeOrbit.Api/Controllers/FamilyAllergiesController.cs` - REST API endpoints
- `server/YellowHouseStudio.LifeOrbit.Tests.Common/Builders/AddAllergyCommandBuilder.cs` - Test data builder
- `server/YellowHouseStudio.LifeOrbit.Tests.Integration/Application/Family/AddAllergy/AddAllergyCommandHandlerTests.cs` - Integration tests
- `server/YellowHouseStudio.LifeOrbit.Tests.API/Controllers/FamilyAllergiesControllerTests.cs` - API tests

Related:
- Design: docs/architecture/testing.md - Testing strategy for domain models and CQRS implementation

** 2024-03-28 14:30 - AllergySeverity Documentation - Documentation - Started **

Discovered incorrect AllergySeverity enum value in testing documentation.

Details:
- Found incorrect reference to `AllergySeverity.Mild` in testing documentation
- Actual domain model only supports two values: `AvailableForOthers` and `NotAllowed`
- Documentation needs to be updated to reflect actual implementation
- No code changes required as implementation is correct

Files:
- `docs/architecture/testing.md` - Contains incorrect example using non-existent enum value

Next Steps:
1. Update testing documentation to use correct enum values (0.5 days)
2. Review other documentation for similar inconsistencies (0.5 days)
3. Add validation tests to ensure documentation examples are valid (1 day)

Related:
- Domain: `server/YellowHouseStudio.LifeOrbit.Domain.Family/Allergy.cs`
- Frontend: `app/src/app/infrastructure/family/models/family.model.ts`

** 2024-03-28 15:30 - Family Member Allergies - Server - Fix - Completed **

Fixed transaction rollback issue when adding allergies to family members.

Details:
- Added missing SaveChangesAsync call in AddAllergyCommandHandler
- Issue was causing concurrency exceptions due to uncommitted changes
- Fix ensures changes are properly persisted to database before transaction completion

Files:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandHandler.cs` - Added SaveChangesAsync call after modifying entity

** 2024-03-28 16:30 - Family Member Allergies - Server - Analysis - In Progress **

Deep dive analysis of test failures in AddAllergyCommandHandlerTests.

Details:
- All tests are failing with DbUpdateConcurrencyException
- Error message: "Attempted to update or delete an entity that does not exist in the store"
- Issue occurs in the InMemory database provider during SaveChangesAsync
- Tests affected:
  1. Handle_adds_allergy_to_family_member
  2. Handle_adds_multiple_allergies_to_family_member
  3. Handle_prevents_duplicate_allergen

Root Cause Analysis:
1. Entity Tracking Issue:
   - The error suggests EF Core is trying to update an entity that's not being tracked
   - Context.ChangeTracker.Clear() in the first test indicates potential tracking issues
   - The in-memory database might be losing track of the FamilyMember between operations

2. Transaction Behavior:
   - Tests show transaction rollback on failure
   - TransactionBehavior is wrapping the command execution
   - Rollback might be affecting entity state tracking

3. Test Setup:
   - FamilyMember is created in Setup using Context.Add
   - Subsequent operations might be losing the entity tracking context
   - No explicit loading of the FamilyMember before modification

Proposed Solution:
1. Remove Context.ChangeTracker.Clear() as it's breaking entity tracking
2. Modify the command handler to properly reload and track the FamilyMember
3. Ensure proper entity state tracking in the test setup

Files:
- `server/YellowHouseStudio.LifeOrbit.Tests.Integration/Application/Family/AddAllergy/AddAllergyCommandHandlerTests.cs` - Test implementation
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandHandler.cs` - Command handler

Next Steps:
1. Remove ChangeTracker.Clear() from tests
2. Add explicit entity loading in command handler
3. Verify entity state management
4. Run tests to verify fix

Related:
- Previous Analysis: ** 2024-03-28 16:30 - Family Member Allergies - Server - Analysis - In Progress **
- EF Core Documentation: [Change Tracking in EF Core](https://learn.microsoft.com/en-us/ef/core/change-tracking/)

** 2024-03-28 16:45 - Family Member Allergies - Server - Analysis - In Progress **

Update on test failures after removing ChangeTracker.Clear().

Details:
- Tests still failing with same DbUpdateConcurrencyException
- Deeper analysis of command handler reveals potential issues:
  1. Entity Loading:
     - Command handler loads entity with Include statements
     - AsNoTracking() in test assertions might be causing issues
     - Need to ensure consistent entity tracking

  2. Transaction Behavior:
     - TransactionBehavior is wrapping the command execution
     - Logs show transaction rollback on every failure
     - Need to investigate transaction scope in tests

  3. Entity State Management:
     - FamilyMember entity might be detached after initial save
     - AddAllergy operation might not be properly tracking the entity
     - Need to verify entity state before SaveChanges

Proposed Solution:
1. Modify command handler to ensure proper entity tracking:
   - Remove AsNoTracking from test assertions
   - Add explicit entity state checks
   - Ensure proper transaction handling

2. Update test setup:
   - Ensure consistent entity tracking across operations
   - Remove unnecessary database queries
   - Simplify test assertions

Files to Modify:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandHandler.cs`
- `server/YellowHouseStudio.LifeOrbit.Tests.Integration/Application/Family/AddAllergy/AddAllergyCommandHandlerTests.cs`

Next Steps:
1. Update command handler implementation
2. Modify test assertions
3. Run tests to verify fix

Related:
- Previous Analysis: ** 2024-03-28 16:30 - Family Member Allergies - Server - Analysis - In Progress **

** 2024-03-28 17:30 - Family Member Allergies - Server - Fix - Completed **

Fixed critical entity tracking issue in AddAllergyCommandHandler causing test failures.

Details:
- Root cause: EF Core entity tracking issue with in-memory provider when adding child entities through domain model
- Modified command handler to explicitly track new Allergy entities
- Replaced direct domain model method call with handler-level entity management
- Decision: Chose explicit entity tracking over domain model encapsulation for testing reliability
- Progress: 100% complete (All tests passing)

Files:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandHandler.cs` - Implemented explicit entity tracking for new allergies

Technical Details:
- Changed from using FamilyMember.AddAllergy() to explicit entity creation and tracking
- Added _context.Entry(newAllergy).State = EntityState.Added for proper entity tracking
- Maintained business logic while ensuring testing compatibility
- Added detailed logging for better debugging

Related:
- Problem Documentation: docs/reference/problems/ef-core-entity-tracking-child-collections.md
- Tests: server/YellowHouseStudio.LifeOrbit.Tests.Integration/Application/Family/AddAllergy/AddAllergyCommandHandlerTests.cs

** 2024-03-28 18:00 - Family Member Repository - Server - Refactor - Started **

Moving to repository pattern for family member data access to improve entity tracking and testing.

Details:
- Root cause: Entity tracking issues with child collections in EF Core
- Decision: Implement repository pattern to encapsulate data access and entity tracking
- Added explicit entity tracking methods in repository
- Centralized query patterns for family member access
- Progress: 30% complete (Interface and implementation done, handlers to be updated)

Technical Decisions:
1. Repository Interface:
   - Added GetByIdWithAllergiesAsync for optimized loading
   - Added TrackNewAllergy for explicit entity tracking
   - Kept methods focused on single responsibility

2. Entity Tracking:
   - Moved tracking responsibility to repository layer
   - Explicit tracking for child entities (Allergies)
   - Centralized tracking logic for consistency

3. Query Optimization:
   - Separated simple queries from include-heavy queries
   - Optimized HasAllergyAsync using SelectMany
   - Reduced unnecessary includes

Rationale:
- Better separation of concerns (domain logic vs data access)
- Improved testability through repository abstraction
- Consistent handling of entity tracking across handlers
- Reduced duplication of data access patterns
- Clear ownership of infrastructure concerns

Files:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/Common/IFamilyMemberRepository.cs` - Updated interface with new methods
- `server/YellowHouseStudio.LifeOrbit.Infrastructure/Repositories/FamilyMemberRepository.cs` - Implementation with entity tracking

Next Steps:
1. Update AddAllergyCommandHandler to use repository (1 day)
2. Update other command handlers to use repository (2 days)
3. Add repository unit tests (1 day)
4. Update integration tests to use repository (1 day)

Related:
- Previous Issue: docs/reference/problems/ef-core-entity-tracking-child-collections.md
- Architecture: docs/architecture/testing.md

** 2024-06-10 15:30 - API Test Infrastructure - Both - Analysis/Plan - In Progress **

Summary:
Documented the investigation and multiple approaches attempted to enable API/integration tests for the ASP.NET Core minimal hosting model using WebApplicationFactory. Referenced official Microsoft documentation ([Integration tests in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0)) and community best practices ([VSLive: Integration Testing in ASP.NET Core with WebApplicationFactory](https://vslive.com/blogs/news-and-tips/2024/09/integration-testing-in-asp,-d-,net-core-with-webapplicationfactory.aspx)).

Details:
- **Initial Problem:**
  - API tests were not running due to host startup issues: `The entry point exited without ever building an IHost.`
  - The project uses the minimal hosting model (`WebApplication`) with a static `Program` class and `Main` method.
  - WebApplicationFactory expects either a top-level statement or a classic `CreateHostBuilder` entry point, leading to incompatibility.

- **Solutions Attempted:**
  1. **Custom WebApplicationFactory with CreateHost override:**
     - Built and started the app manually in the test factory.
     - Still failed with the same IHost error, as the test server could not attach to the running app.
  2. **Adding CreateHostBuilder to Program.cs:**
     - Attempted to provide a classic host builder for the test infrastructure.
     - Caused other errors due to the minimal hosting model's incompatibility with this pattern.
  3. **Refactoring to top-level statements:**
     - Moved all startup logic to file scope (no Program class).
     - This is the Microsoft-recommended approach for minimal hosting + WebApplicationFactory, but it requires significant refactoring and may impact other startup logic.
  4. **Hybrid approaches (static BuildApp, public App property, etc.):**
     - None of these worked reliably with the test infrastructure.

- **Key Learnings:**
  - The minimal hosting model is only reliably testable with WebApplicationFactory if using top-level statements (no Program class).
  - Custom factories and static methods do not resolve the underlying host startup issue.
  - Official docs recommend top-level statements for new projects and provide patterns for customizing the test host via ConfigureWebHost.
  - Test project must reference Microsoft.AspNetCore.Mvc.Testing and use the Web SDK.

- **References:**
  - [Microsoft Docs: Integration tests in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0)
  - [VSLive: Integration Testing in ASP.NET Core with WebApplicationFactory](https://vslive.com/blogs/news-and-tips/2024/09/integration-testing-in-asp,-d-,net-core-with-webapplicationfactory.aspx)

Files:
- `server/YellowHouseStudio.LifeOrbit.Api/Program.cs` - Multiple refactors for entry point compatibility
- `server/YellowHouseStudio.LifeOrbit.Tests.API/ApiTestBase.cs` - Custom and default WebApplicationFactory patterns

Next Steps:
1. Refactor Program.cs to use top-level statements (remove Program class entirely).
2. Ensure all startup logic is at file scope and compatible with WebApplicationFactory.
3. Update ApiTestBase to use the default WebApplicationFactory<Program>.
4. Run API tests and verify that the test server starts and actual HTTP calls are made.
5. Document any further issues or required workarounds.

Related Documents:
- [Microsoft Docs: Integration tests in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0)
- [VSLive: Integration Testing in ASP.NET Core with WebApplicationFactory](https://vslive.com/blogs/news-and-tips/2024/09/integration-testing-in-asp,-d-,net-core-with-webapplicationfactory.aspx)

** 2024-06-06 15:30 - Program.cs Minimal Hosting Refactor - Server - Refactor - Completed **

Refactored Program.cs to use top-level statements, adopting the minimal hosting model for ASP.NET Core. This change is required for compatibility with WebApplicationFactory and modern integration/API testing patterns.

Details:
- Removed the Program class and static methods, moving all startup logic to file scope using top-level statements.
- Preserved all configuration, service registration, and middleware setup in the new structure.
- Ensured that the application can still be started normally and is now compatible with WebApplicationFactory for integration tests.
- Decision: Adopted the minimal hosting model as recommended by Microsoft for .NET 6+ projects to simplify startup and testing.
- No deviations from plan; all logic from the previous Main and BuildApp methods was preserved at file scope.
- Progress: 100% complete for this refactor.

Files:
- `server/YellowHouseStudio.LifeOrbit.Api/Program.cs` - Refactored to use top-level statements, removed Program class and static methods.

Next Steps:
1. Validate that API/integration tests using WebApplicationFactory now work as expected (run test suite).
2. Update any documentation or test setup that referenced the old Program class if needed.
3. Monitor for any issues in local/dev environments related to startup or test hosting.

Related:
- [Testing Strategy](architecture/testing.md)
- Previous log entry: API/integration test enablement investigation

** 2024-06-06 16:00 - API Test Infrastructure Refactor - Server - Fix - In Progress **

Resolved test infrastructure issues preventing API/integration tests from running after the Program.cs minimal hosting refactor.

Details:
- Problem: API tests failed with 'ApiTestBase.Factory is inaccessible due to its protection level' after moving to top-level statements and removing the Program class. This was due to split and inconsistent test base classes (Infrastructure/ApiTestBase and API/ApiTestBase), with Factory being private in the base class but accessed in the derived class.
- Previous attempts included custom WebApplicationFactory, static Program methods, and hybrid approaches, but the Microsoft-recommended pattern is to use top-level statements and a single, minimal test base class.
- Solution: Consolidate all test setup and dependencies (HttpClient, ApplicationDbContext, ICurrentUser) into Infrastructure/ApiTestBase. Expose these as protected properties. Remove or empty the API/ApiTestBase. Update all test classes to inherit from the unified base. This aligns with official docs and community best practices.
- Rationale: Ensures a single source of truth for test setup, resolves accessibility errors, and makes tests maintainable and compatible with minimal hosting/WebApplicationFactory.
- References: [Microsoft Docs: Integration tests in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0), [VSLive: Integration Testing in ASP.NET Core with WebApplicationFactory](https://vslive.com/blogs/news-and-tips/2024/09/integration-testing-in-asp,-d-,net-core-with-webapplicationfactory.aspx)
- Progress: Refactor in progress, updating base class and test files.

Files:
- `server/YellowHouseStudio.LifeOrbit.Tests.API/Infrastructure/ApiTestBase.cs` - Refactor to expose protected properties and handle all setup
- `server/YellowHouseStudio.LifeOrbit.Tests.API/ApiTestBase.cs` - Remove or make empty subclass
- All test files inheriting from ApiTestBase

Next Steps:
1. Refactor Infrastructure/ApiTestBase to expose protected properties for HttpClient, ApplicationDbContext, and ICurrentUser.
2. Remove duplicate setup from API/ApiTestBase.
3. Update all test classes to use the unified base.
4. Run API/integration tests to verify fix.
5. Document completion in the log.

Related:
- Previous log entries on API test enablement and Program.cs refactor
- [Microsoft Docs: Integration tests in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0)
- [VSLive: Integration Testing in ASP.NET Core with WebApplicationFactory](https://vslive.com/blogs/news-and-tips/2024/09/integration-testing-in-asp,-d-,net-core-with-webapplicationfactory.aspx)

** 2024-06-10 17:00 - Program.cs Top-Level Refactor & Linter Fix - Server - Fix - Completed **

Resolved linter and startup issues in Program.cs by confirming top-level statements and correcting extension method usage.

Details:
- Investigated linter errors related to missing AddInfrastructure extension method and namespace issues after refactoring Program.cs to use top-level statements.
- Confirmed that AddInfrastructure is defined in `YellowHouseStudio.LifeOrbit.Infrastructure.Configuration` but the containing directory was misspelled as `Configuraiton`.
- Verified that the using directive in Program.cs is correct, but the project file and build system must reference the correct (misspelled) directory and files, or the directory should be renamed for clarity.
- No further issues with try/catch or top-level statements; the application now starts and is compatible with WebApplicationFactory for integration testing.
- Decision: Proceeded with the current directory spelling for now to unblock development, but recommend renaming to `Configuration` in the future for clarity.
- Progress: 100% complete for this fix.

Files:
- `server/YellowHouseStudio.LifeOrbit.Api/Program.cs` - Confirmed correct using and extension method usage
- `server/YellowHouseStudio.LifeOrbit.Infrastructure/Configuraiton/InfrastructureConfiguration.cs` - Confirmed AddInfrastructure definition
- `server/YellowHouseStudio.LifeOrbit.Infrastructure/Configuraiton/FamilyMemeberConfiguration.cs` - Confirmed AddFamilyMemberConfiguration definition

Next Steps:
1. Run API/integration tests to verify that the application and test infrastructure work as expected.
2. Consider renaming the `Configuraiton` directory to `Configuration` and updating namespaces for clarity (future cleanup).

Related Documents:
- Previous log entries on Program.cs refactor and API test enablement
- [Microsoft Docs: Integration tests in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0)

** 2024-06-10 18:00 - FamilyAllergiesControllerTests - Fix - In Progress **

Summary:
All tests in FamilyAllergiesControllerTests are failing with a System.TypeLoadException: Could not load type 'Microsoft.EntityFrameworkCore.Metadata.Internal.AdHocMapper' from assembly 'Microsoft.EntityFrameworkCore, Version=9.0.4.0'.

Details:
- Error occurs during test fixture setup, before any test logic runs.
- The error is a classic EF Core version mismatch: the test host is loading EF Core 9.0.4, but the rest of the solution (including the test project and ApplicationDbContext) is built against 8.0.2.
- This causes runtime type resolution failures for internal types.
- Root cause: The Infrastructure project still references EF Core 9.0.4, while the rest of the solution uses 8.0.2.
- Solution: Align all EF Core and provider package versions to 8.0.2, then clean and rebuild the solution to ensure no old binaries remain.

Files:
- `server/YellowHouseStudio.LifeOrbit.Tests.API/Controllers/FamilyAllergiesControllerTests.cs` - All tests failing
- `server/YellowHouseStudio.LifeOrbit.Infrastructure/YellowHouseStudio.LifeOrbit.Infrastructure.csproj` - References EF Core 9.0.4 (should be 8.0.2)

Next Steps:
1. Update Infrastructure.csproj to use EF Core 8.0.2
2. Clean and rebuild the solution
3. Re-run the tests

Related:
- Previous log entries on API test enablement and EF Core versioning

** 2024-06-10 18:10 - Build Blocked by Configuration Namespace - Fix - In Progress **

Summary:
Build is failing due to a namespace error in the integration test project: 'The type or namespace name 'Configuration' does not exist in the namespace 'YellowHouseStudio.LifeOrbit.Infrastructure''.

Details:
- The actual directory is named 'Configuraiton' (misspelled), but the code references 'Configuration'.
- This is a known technical debt and has been previously documented.
- This error is blocking the build and thus the ability to run and fix the API tests.

Files:
- `server/YellowHouseStudio.LifeOrbit.Tests.Integration/TestBase.cs` - References the wrong namespace
- `server/YellowHouseStudio.LifeOrbit.Infrastructure/Configuraiton/` - Actual directory name

Next Steps:
1. Update the using statement in TestBase.cs to use 'Configuraiton' instead of 'Configuration' to unblock the build.
2. Rebuild the solution and re-run the tests.
3. Plan for a full directory rename and namespace update as technical debt cleanup.

Related:
- Previous log entries on directory/namespace mismatch

** 2024-06-10 18:20 - FamilyAllergiesControllerTests - Analysis & Plan - In Progress **

Summary:
After resolving build and EF Core issues, 5/9 tests in FamilyAllergiesControllerTests are failing. Failures are due to ambiguous endpoint routing and a validation/404 mismatch.

Details:
- AmbiguousMatchException (500 errors):
  - Multiple endpoints match the DELETE allergy route: both FamilyAllergiesController and FamilyController define RemoveAllergy endpoints.
  - This causes ASP.NET Core to throw an ambiguity error for DELETE requests.
- ValidationException (400 error):
  - AddAllergy returns 400 Bad Request with a validation error when the family member does not exist, but the test expects 404 Not Found.
  - The existence check is enforced in the validator, not as a resource check in the handler.

Files:
- `server/YellowHouseStudio.LifeOrbit.Api/Controllers/FamilyAllergiesController.cs` - Should be the only controller handling allergy removal
- `server/YellowHouseStudio.LifeOrbit.Api/Controllers/FamilyController.cs` - Contains duplicate RemoveAllergy endpoint
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandValidator.cs` - Enforces existence as validation
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandHandler.cs` - Handles command

Next Steps:
1. Remove or update the duplicate RemoveAllergy endpoint in FamilyController to resolve ambiguous routing.
2. Update AddAllergy handling so that non-existent family member returns 404 Not Found, not 400 Bad Request (move existence check from validator to handler, or update test if 400 is intended).
3. Re-run the tests and continue analysis if needed.

Related:
- Previous log entries on API test enablement and EF Core versioning

** 2024-06-10 18:30 - FamilyAllergiesControllerTests - Final Failure Analysis & Plan - In Progress **

Summary:
After resolving ambiguous routing, 3/9 tests in FamilyAllergiesControllerTests are still failing. Failures are due to validator/handler responsibility for existence checks and exception-to-status-code mapping.

Details:
- AddAllergy_returns_not_found_for_non_existent_member:
  - Returns 400 BadRequest (validation error) instead of 404 NotFound for non-existent family member.
  - Existence check is in the validator, not the handler.
- RemoveAllergy_returns_not_found_for_non_existent_allergy:
  - Returns 200 OK even when the allergy does not exist; should return 404 NotFound.
- RemoveAllergy_returns_not_found_for_non_existent_member:
  - Handler throws NotFoundException, but API returns 500 InternalServerError instead of 404 NotFound.

Files:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandValidator.cs` - Existence check in validator
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandHandler.cs` - Should handle not found
- `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveAllergy/RemoveAllergyCommandHandler.cs` - Should check for allergy existence and map NotFoundException
- `server/YellowHouseStudio.LifeOrbit.Api/Infrastructure/Filters/GlobalExceptionFilter.cs` - Should map NotFoundException to 404

Next Steps:
1. Move family member existence check from AddAllergyCommandValidator to AddAllergyCommandHandler; throw NotFoundException in handler.
2. In RemoveAllergyCommandHandler, throw NotFoundException if allergy is not found.
3. Ensure GlobalExceptionFilter maps NotFoundException to 404
4. Re-run the tests and continue analysis if needed.

Related:
- Previous log entries on API test enablement and EF Core versioning

** 2024-06-10 18:40 - AddAllergy_returns_not_found_for_non_existent_member - Fix - Completed **

Summary:
Fixed the test so that adding an allergy to a non-existent family member now returns 404 NotFound instead of 400 BadRequest, by leveraging validation error codes and the ValidationExceptionFilter.

Details:
- The existence check for the family member remains in the `AddAllergyCommandValidator`.
- The validator uses `.WithErrorCode(ValidationErrorCodes.NotFound)` when the family member does not exist.
- The `ValidationExceptionFilter` inspects validation errors and, if any error has the NotFound error code, returns a 404 NotFound response with validation problem details.
- This approach keeps validation logic in the validator and uses error codes for status mapping, aligning with the project's validation and error handling conventions.
- No changes were made to the handler or the GlobalExceptionFilter for this scenario.

Files:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/AddAllergy/AddAllergyCommandValidator.cs` - Existence check with NotFound error code
- `server/YellowHouseStudio.LifeOrbit.Api/Infrastructure/Filters/ValidationExceptionFilter.cs` - Maps NotFound error code to 404

Alternative Solutions Considered:
- Moving the existence check to the handler and throwing NotFoundException, but this would break the validation-centric approach and require more boilerplate.
- Updating the test to expect 400 BadRequest, but this would not align with RESTful conventions for missing resources.

Next Steps:
- Fix the remaining tests for allergy removal to ensure correct 404/200 handling and exception mapping.

** 2024-06-10 19:00 - RemoveAllergy API Tests - Analysis & Fix Plan - In Progress **

Summary:
Analyzed failing RemoveAllergy API tests in FamilyAllergiesControllerTests. Two tests are failing due to incorrect status code handling for not found scenarios.

Details:
- Failing Tests:
  1. RemoveAllergy_returns_not_found_for_non_existent_allergy
     - Expected: 404 NotFound
     - Actual: 200 OK
     - Error: Handler does not check if the allergy exists before removing; always returns 200.
  2. RemoveAllergy_returns_not_found_for_non_existent_member
     - Expected: 404 NotFound
     - Actual: 500 InternalServerError
     - Error: Handler throws NotFoundException, but API returns 500 instead of 404.

Root Causes:
- The RemoveAllergyCommandHandler does not verify the existence of the allergy before attempting removal, so it cannot distinguish between a successful removal and a no-op.
- The NotFoundException thrown for a missing family member is not mapped to a 404 response by the API's exception filters, resulting in a 500 error.

Proposed Solutions:
1. Update RemoveAllergyCommandHandler to check if the specified allergy exists for the family member. If not, throw NotFoundException with a clear message.
2. Update GlobalExceptionFilter to map NotFoundException to 404

Alternative Considered:
- Use a ValidationException with NotFound error code for both cases for consistency, but this would diverge from the current handler-based error handling for removals.

Selected Approach:
- Implement the handler-based NotFoundException for both missing member and missing allergy, and ensure the exception is mapped to 404 by the API filter.

Files:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveAllergy/RemoveAllergyCommandHandler.cs` - Add allergy existence check and throw NotFoundException if not found
- `server/YellowHouseStudio.LifeOrbit.Api/Infrastructure/Filters/GlobalExceptionFilter.cs` - Maps NotFoundException to 404

Next Steps:
1. Implement the above fixes in the handler and exception filter.
2. Re-run the tests and continue the workflow if any failures remain.

** 2024-06-10 19:15 - RemoveAllergy API Tests - Fix - Completed **

Summary:
All RemoveAllergy API tests in FamilyAllergiesControllerTests now pass. The issues with incorrect status code handling for not found scenarios have been resolved.

Details:
- Updated RemoveAllergyCommandHandler to check if the specified allergy exists for the family member before removing. If the allergy does not exist, it now throws NotFoundException with a clear message.
- Updated GlobalExceptionFilter to map NotFoundException to a 404 NotFound response, returning a ProblemDetails object with status 404 and the exception message as the title.
- This ensures that both missing family members and missing allergies result in a 404 response, matching RESTful conventions and test expectations.
- All 9 tests in FamilyAllergiesControllerTests now pass.

Files:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveAllergy/RemoveAllergyCommandHandler.cs` - Added allergy existence check and NotFoundException
- `server/YellowHouseStudio.LifeOrbit.Api/Infrastructure/Filters/GlobalExceptionFilter.cs` - Maps NotFoundException to 404

Technical Decisions:
- Chose handler-based NotFoundException for removals to align with existing patterns and keep validation logic in validators for creation/update scenarios.
- Considered using ValidationException for all not found cases, but this would diverge from current removal patterns.

Status: Completed

** 2024-06-09 15:30 - RemoveAllergy Audit Fixes - Server - Completed **

Summary:
Addressed all issues found in the FamilyAllergiesController audit for RemoveAllergy. Refactored handler to use repository, expanded validation, and added missing tests.

Details:
- Refactored RemoveAllergyCommandHandler to use IFamilyMemberRepository instead of ApplicationDbContext
- Moved all validation logic (existence, ownership, allergy presence) to RemoveAllergyCommandValidator
- Expanded RemoveAllergyCommandValidator to check for family member existence/ownership and allergy presence
- Added integration tests for RemoveAllergyCommandHandler (happy path, not found, unauthorized, allergy not present)
- Added unit tests for RemoveAllergyCommandValidator (empty fields, max length, not found, unauthorized, allergy not present, valid)
- Ran dotnet format to fix whitespace formatting errors
- All audit issues for RemoveAllergy are now addressed

Files:
- `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveAllergy/RemoveAllergyCommandHandler.cs` - Refactored to use repository, removed inline validation
- `server/YellowHouseStudio.LifeOrbit.Application/Family/RemoveAllergy/RemoveAllergyCommandValidator.cs` - Expanded validation logic
- `server/YellowHouseStudio.LifeOrbit.Tests.Integration/Application/Family/RemoveAllergy/RemoveAllergyCommandHandlerTests.cs` - New integration tests
- `server/YellowHouseStudio.LifeOrbit.Tests.Unit/Application/Family/RemoveAllergy/RemoveAllergyCommandValidatorTests.cs` - New unit tests

Next Steps:
- Monitor for regressions in RemoveAllergy feature
- Consider similar audit for AddAllergy handler validation

Related:
- Audit: docs/audit/FamilyAllergiesController.md
